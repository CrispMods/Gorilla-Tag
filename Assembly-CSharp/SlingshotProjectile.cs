using System;
using GorillaExtensions;
using GorillaGameModes;
using GorillaLocomotion;
using GorillaLocomotion.Swimming;
using GorillaTag.Reactions;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000392 RID: 914
public class SlingshotProjectile : MonoBehaviour
{
	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06001579 RID: 5497 RVA: 0x0003E8C0 File Offset: 0x0003CAC0
	// (set) Token: 0x0600157A RID: 5498 RVA: 0x0003E8C8 File Offset: 0x0003CAC8
	public Vector3 launchPosition { get; private set; }

	// Token: 0x14000040 RID: 64
	// (add) Token: 0x0600157B RID: 5499 RVA: 0x000C006C File Offset: 0x000BE26C
	// (remove) Token: 0x0600157C RID: 5500 RVA: 0x000C00A4 File Offset: 0x000BE2A4
	public event SlingshotProjectile.ProjectileImpactEvent OnImpact;

	// Token: 0x0600157D RID: 5501 RVA: 0x000C00DC File Offset: 0x000BE2DC
	public void Launch(Vector3 position, Vector3 velocity, NetPlayer player, bool blueTeam, bool orangeTeam, int projectileCount, float scale, bool shouldOverrideColor = false, Color overrideColor = default(Color))
	{
		if (this.launchSoundBankPlayer != null)
		{
			this.launchSoundBankPlayer.Play();
		}
		this.particleLaunched = true;
		this.timeCreated = Time.time;
		this.launchPosition = position;
		Transform transform = base.transform;
		transform.position = position;
		transform.localScale = Vector3.one * scale;
		base.GetComponent<Collider>().contactOffset = 0.01f * scale;
		RigidbodyWaterInteraction component = base.GetComponent<RigidbodyWaterInteraction>();
		if (component != null)
		{
			component.objectRadiusForWaterCollision = 0.02f * scale;
		}
		this.projectileRigidbody.useGravity = false;
		this.forceComponent.force = Physics.gravity * this.projectileRigidbody.mass * this.gravityMultiplier * ((scale < 1f) ? scale : 1f);
		this.projectileRigidbody.velocity = velocity;
		this.projectileOwner = player;
		this.myProjectileCount = projectileCount;
		this.projectileRigidbody.position = position;
		this.ApplyTeamModelAndColor(blueTeam, orangeTeam, shouldOverrideColor, overrideColor);
	}

	// Token: 0x0600157E RID: 5502 RVA: 0x000C01F0 File Offset: 0x000BE3F0
	protected void Awake()
	{
		if (this.playerImpactEffectPrefab == null)
		{
			this.playerImpactEffectPrefab = this.surfaceImpactEffectPrefab;
		}
		this.projectileRigidbody = base.GetComponent<Rigidbody>();
		this.forceComponent = base.GetComponent<ConstantForce>();
		this.initialScale = base.transform.localScale.x;
		this.matPropBlock = new MaterialPropertyBlock();
		this.spawnWorldEffects = base.GetComponent<SpawnWorldEffects>();
	}

	// Token: 0x0600157F RID: 5503 RVA: 0x000C025C File Offset: 0x000BE45C
	public void Deactivate()
	{
		base.transform.localScale = Vector3.one * this.initialScale;
		this.projectileRigidbody.useGravity = true;
		this.forceComponent.force = Vector3.zero;
		this.OnImpact = null;
		this.aoeKnockbackConfig = null;
		this.impactSoundVolumeOverride = null;
		this.impactSoundPitchOverride = null;
		this.impactEffectScaleMultiplier = 1f;
		this.gravityMultiplier = 1f;
		ObjectPools.instance.Destroy(base.gameObject);
	}

	// Token: 0x06001580 RID: 5504 RVA: 0x000C02F4 File Offset: 0x000BE4F4
	private void SpawnImpactEffect(GameObject prefab, Vector3 position, Vector3 normal)
	{
		Vector3 position2 = position + normal * this.impactEffectOffset;
		GameObject gameObject = ObjectPools.instance.Instantiate(prefab, position2);
		Vector3 localScale = base.transform.localScale;
		gameObject.transform.localScale = localScale * this.impactEffectScaleMultiplier;
		gameObject.transform.up = normal;
		GorillaColorizableBase component = gameObject.GetComponent<GorillaColorizableBase>();
		if (component != null)
		{
			component.SetColor(this.teamColor);
		}
		SurfaceImpactFX component2 = gameObject.GetComponent<SurfaceImpactFX>();
		if (component2 != null)
		{
			component2.SetScale(localScale.x * this.impactEffectScaleMultiplier);
		}
		SoundBankPlayer component3 = gameObject.GetComponent<SoundBankPlayer>();
		if (component3 != null && !component3.playOnEnable)
		{
			component3.Play(this.impactSoundVolumeOverride, this.impactSoundPitchOverride);
		}
		if (this.spawnWorldEffects != null)
		{
			this.spawnWorldEffects.RequestSpawn(position, normal);
		}
	}

	// Token: 0x06001581 RID: 5505 RVA: 0x000C03D8 File Offset: 0x000BE5D8
	public void CheckForAOEKnockback(Vector3 impactPosition, float impactSpeed)
	{
		if (this.aoeKnockbackConfig != null && this.aoeKnockbackConfig.Value.applyAOEKnockback)
		{
			Vector3 a = GTPlayer.Instance.HeadCenterPosition - impactPosition;
			if (a.sqrMagnitude < this.aoeKnockbackConfig.Value.aeoOuterRadius * this.aoeKnockbackConfig.Value.aeoOuterRadius)
			{
				float magnitude = a.magnitude;
				Vector3 direction = (magnitude > 0.001f) ? (a / magnitude) : Vector3.up;
				float num = Mathf.InverseLerp(this.aoeKnockbackConfig.Value.aeoOuterRadius, this.aoeKnockbackConfig.Value.aeoInnerRadius, magnitude);
				float num2 = Mathf.InverseLerp(0f, this.aoeKnockbackConfig.Value.impactVelocityThreshold, impactSpeed);
				GTPlayer.Instance.ApplyKnockback(direction, this.aoeKnockbackConfig.Value.knockbackVelocity * num * num2, false);
				this.impactEffectScaleMultiplier = Mathf.Lerp(1f, this.impactEffectScaleMultiplier, num2);
				if (this.impactSoundVolumeOverride != null)
				{
					this.impactSoundVolumeOverride = new float?(Mathf.Lerp(this.impactSoundVolumeOverride.Value * 0.5f, this.impactSoundVolumeOverride.Value, num2));
				}
				float num3 = Mathf.Lerp(this.aoeKnockbackConfig.Value.aeoInnerRadius, this.aoeKnockbackConfig.Value.aeoOuterRadius, 0.25f);
				if (this.aoeKnockbackConfig.Value.playerProximityEffect != PlayerEffect.NONE && a.sqrMagnitude < num3 * num3)
				{
					RoomSystem.SendPlayerEffect(PlayerEffect.SNOWBALL_IMPACT, NetworkSystem.Instance.LocalPlayer);
				}
			}
		}
	}

	// Token: 0x06001582 RID: 5506 RVA: 0x000C057C File Offset: 0x000BE77C
	public void ApplyTeamModelAndColor(bool blueTeam, bool orangeTeam, bool shouldOverrideColor = false, Color overrideColor = default(Color))
	{
		if (shouldOverrideColor)
		{
			this.teamColor = overrideColor;
		}
		else
		{
			this.teamColor = (blueTeam ? this.blueColor : (orangeTeam ? this.orangeColor : this.defaultColor));
		}
		this.blueBall.enabled = blueTeam;
		this.orangeBall.enabled = orangeTeam;
		this.defaultBall.enabled = (!blueTeam && !orangeTeam);
		this.teamRenderer = (blueTeam ? this.blueBall : (orangeTeam ? this.orangeBall : this.defaultBall));
		this.ApplyColor(this.teamRenderer, (this.colorizeBalls || shouldOverrideColor) ? this.teamColor : Color.white);
	}

	// Token: 0x06001583 RID: 5507 RVA: 0x0003E8D1 File Offset: 0x0003CAD1
	protected void OnEnable()
	{
		this.timeCreated = 0f;
		this.particleLaunched = false;
		SlingshotProjectileManager.RegisterSP(this);
	}

	// Token: 0x06001584 RID: 5508 RVA: 0x0003E8EB File Offset: 0x0003CAEB
	protected void OnDisable()
	{
		this.particleLaunched = false;
		SlingshotProjectileManager.UnregisterSP(this);
	}

	// Token: 0x06001585 RID: 5509 RVA: 0x000C062C File Offset: 0x000BE82C
	public void InvokeUpdate()
	{
		if (this.particleLaunched)
		{
			if (Time.time > this.timeCreated + this.lifeTime)
			{
				this.Deactivate();
			}
			if (this.faceDirectionOfTravel)
			{
				Transform transform = base.transform;
				Vector3 position = transform.position;
				Vector3 forward = position - this.previousPosition;
				transform.rotation = ((forward.sqrMagnitude > 0f) ? Quaternion.LookRotation(forward) : transform.rotation);
				this.previousPosition = position;
			}
		}
	}

	// Token: 0x06001586 RID: 5510 RVA: 0x0003E8FA File Offset: 0x0003CAFA
	public void DestroyAfterRelease()
	{
		this.SpawnImpactEffect(this.surfaceImpactEffectPrefab, base.transform.position, Vector3.up);
		this.Deactivate();
	}

	// Token: 0x06001587 RID: 5511 RVA: 0x000C06A8 File Offset: 0x000BE8A8
	protected void OnCollisionEnter(Collision collision)
	{
		if (!this.particleLaunched)
		{
			return;
		}
		SlingshotProjectileHitNotifier slingshotProjectileHitNotifier;
		if (collision.collider.gameObject.TryGetComponent<SlingshotProjectileHitNotifier>(out slingshotProjectileHitNotifier))
		{
			slingshotProjectileHitNotifier.InvokeHit(this, collision);
		}
		ContactPoint contact = collision.GetContact(0);
		this.CheckForAOEKnockback(contact.point, collision.relativeVelocity.magnitude);
		this.SpawnImpactEffect(this.surfaceImpactEffectPrefab, contact.point, contact.normal);
		SlingshotProjectile.ProjectileImpactEvent onImpact = this.OnImpact;
		if (onImpact != null)
		{
			onImpact(this, contact.point, null);
		}
		this.Deactivate();
	}

	// Token: 0x06001588 RID: 5512 RVA: 0x000C0738 File Offset: 0x000BE938
	protected void OnCollisionStay(Collision collision)
	{
		if (!this.particleLaunched)
		{
			return;
		}
		SlingshotProjectileHitNotifier slingshotProjectileHitNotifier;
		if (collision.gameObject.TryGetComponent<SlingshotProjectileHitNotifier>(out slingshotProjectileHitNotifier))
		{
			slingshotProjectileHitNotifier.InvokeCollisionStay(this, collision);
		}
		ContactPoint contact = collision.GetContact(0);
		this.CheckForAOEKnockback(contact.point, collision.relativeVelocity.magnitude);
		this.SpawnImpactEffect(this.surfaceImpactEffectPrefab, contact.point, contact.normal);
		SlingshotProjectile.ProjectileImpactEvent onImpact = this.OnImpact;
		if (onImpact != null)
		{
			onImpact(this, contact.point, null);
		}
		this.Deactivate();
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x000C07C4 File Offset: 0x000BE9C4
	protected void OnTriggerExit(Collider other)
	{
		if (!this.particleLaunched)
		{
			return;
		}
		SlingshotProjectileHitNotifier slingshotProjectileHitNotifier;
		if (other.gameObject.TryGetComponent<SlingshotProjectileHitNotifier>(out slingshotProjectileHitNotifier))
		{
			slingshotProjectileHitNotifier.InvokeTriggerExit(this, other);
		}
	}

	// Token: 0x0600158A RID: 5514 RVA: 0x000C07F4 File Offset: 0x000BE9F4
	protected void OnTriggerEnter(Collider other)
	{
		if (!this.particleLaunched)
		{
			return;
		}
		SlingshotProjectileHitNotifier slingshotProjectileHitNotifier;
		if (other.gameObject.TryGetComponent<SlingshotProjectileHitNotifier>(out slingshotProjectileHitNotifier))
		{
			slingshotProjectileHitNotifier.InvokeTriggerEnter(this, other);
		}
		if (this.projectileOwner == NetworkSystem.Instance.LocalPlayer)
		{
			if (!NetworkSystem.Instance.InRoom || GorillaGameManager.instance == null)
			{
				return;
			}
			GorillaPaintbrawlManager component = GorillaGameManager.instance.gameObject.GetComponent<GorillaPaintbrawlManager>();
			if (!other.gameObject.IsOnLayer(UnityLayer.GorillaTagCollider) && !other.gameObject.IsOnLayer(UnityLayer.GorillaSlingshotCollider))
			{
				return;
			}
			VRRig componentInParent = other.GetComponentInParent<VRRig>();
			NetPlayer netPlayer = (componentInParent != null) ? componentInParent.creator : null;
			if (netPlayer == null)
			{
				return;
			}
			if (NetworkSystem.Instance.LocalPlayer == netPlayer)
			{
				return;
			}
			SlingshotProjectile.ProjectileImpactEvent onImpact = this.OnImpact;
			if (onImpact != null)
			{
				onImpact(this, base.transform.position, netPlayer);
			}
			if (component && !component.LocalCanHit(NetworkSystem.Instance.LocalPlayer, netPlayer))
			{
				return;
			}
			if (component && GameMode.ActiveNetworkHandler)
			{
				GameMode.ActiveNetworkHandler.SendRPC("RPC_ReportSlingshotHit", false, new object[]
				{
					(netPlayer as PunNetPlayer).PlayerRef,
					base.transform.position,
					this.myProjectileCount
				});
				PlayerGameEvents.GameModeObjectiveTriggered();
			}
			RoomSystem.SendImpactEffect(base.transform.position, this.teamColor.r, this.teamColor.g, this.teamColor.b, this.teamColor.a, this.myProjectileCount);
			this.Deactivate();
		}
		Rigidbody attachedRigidbody = other.attachedRigidbody;
		VRRig arg;
		if (attachedRigidbody.IsNotNull() && attachedRigidbody.gameObject.TryGetComponent<VRRig>(out arg))
		{
			UnityEvent<VRRig> onHitPlayer = this.OnHitPlayer;
			if (onHitPlayer == null)
			{
				return;
			}
			onHitPlayer.Invoke(arg);
		}
	}

	// Token: 0x0600158B RID: 5515 RVA: 0x0003E91E File Offset: 0x0003CB1E
	private void ApplyColor(Renderer rend, Color color)
	{
		if (!rend)
		{
			return;
		}
		this.matPropBlock.SetColor(SlingshotProjectile.baseColorShaderProp, color);
		this.matPropBlock.SetColor(SlingshotProjectile.colorShaderProp, color);
		rend.SetPropertyBlock(this.matPropBlock);
	}

	// Token: 0x040017C6 RID: 6086
	public NetPlayer projectileOwner;

	// Token: 0x040017C7 RID: 6087
	[Tooltip("Rotates to point along the Y axis after spawn.")]
	public GameObject surfaceImpactEffectPrefab;

	// Token: 0x040017C8 RID: 6088
	[Tooltip("if left empty, the default player impact that is set in Room System Setting will be played")]
	public GameObject playerImpactEffectPrefab;

	// Token: 0x040017C9 RID: 6089
	[Tooltip("Distance from the surface that the particle should spawn.")]
	[SerializeField]
	private float impactEffectOffset;

	// Token: 0x040017CA RID: 6090
	[SerializeField]
	private SoundBankPlayer launchSoundBankPlayer;

	// Token: 0x040017CB RID: 6091
	public float lifeTime = 20f;

	// Token: 0x040017CC RID: 6092
	public Color defaultColor = Color.white;

	// Token: 0x040017CD RID: 6093
	public Color orangeColor = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x040017CE RID: 6094
	public Color blueColor = new Color(0f, 0.72f, 1f, 1f);

	// Token: 0x040017CF RID: 6095
	[Tooltip("Renderers with team specific meshes, materials, effects, etc.")]
	public Renderer defaultBall;

	// Token: 0x040017D0 RID: 6096
	[Tooltip("Renderers with team specific meshes, materials, effects, etc.")]
	public Renderer orangeBall;

	// Token: 0x040017D1 RID: 6097
	[Tooltip("Renderers with team specific meshes, materials, effects, etc.")]
	public Renderer blueBall;

	// Token: 0x040017D2 RID: 6098
	public bool colorizeBalls;

	// Token: 0x040017D3 RID: 6099
	public bool faceDirectionOfTravel = true;

	// Token: 0x040017D4 RID: 6100
	private bool particleLaunched;

	// Token: 0x040017D5 RID: 6101
	private float timeCreated;

	// Token: 0x040017D7 RID: 6103
	private Rigidbody projectileRigidbody;

	// Token: 0x040017D8 RID: 6104
	private Color teamColor = Color.white;

	// Token: 0x040017D9 RID: 6105
	private Renderer teamRenderer;

	// Token: 0x040017DA RID: 6106
	public int myProjectileCount;

	// Token: 0x040017DB RID: 6107
	private float initialScale;

	// Token: 0x040017DC RID: 6108
	private Vector3 previousPosition;

	// Token: 0x040017DD RID: 6109
	[HideInInspector]
	public SlingshotProjectile.AOEKnockbackConfig? aoeKnockbackConfig;

	// Token: 0x040017DE RID: 6110
	[HideInInspector]
	public float? impactSoundVolumeOverride;

	// Token: 0x040017DF RID: 6111
	[HideInInspector]
	public float? impactSoundPitchOverride;

	// Token: 0x040017E0 RID: 6112
	[HideInInspector]
	public float impactEffectScaleMultiplier = 1f;

	// Token: 0x040017E1 RID: 6113
	[HideInInspector]
	public float gravityMultiplier = 1f;

	// Token: 0x040017E2 RID: 6114
	private ConstantForce forceComponent;

	// Token: 0x040017E4 RID: 6116
	private MaterialPropertyBlock matPropBlock;

	// Token: 0x040017E5 RID: 6117
	private SpawnWorldEffects spawnWorldEffects;

	// Token: 0x040017E6 RID: 6118
	private static readonly int colorShaderProp = Shader.PropertyToID("_Color");

	// Token: 0x040017E7 RID: 6119
	private static readonly int baseColorShaderProp = Shader.PropertyToID("_BaseColor");

	// Token: 0x040017E8 RID: 6120
	public UnityEvent<VRRig> OnHitPlayer;

	// Token: 0x02000393 RID: 915
	[Serializable]
	public struct AOEKnockbackConfig
	{
		// Token: 0x040017E9 RID: 6121
		public bool applyAOEKnockback;

		// Token: 0x040017EA RID: 6122
		[Tooltip("Full knockback velocity is imparted within the inner radius")]
		public float aeoInnerRadius;

		// Token: 0x040017EB RID: 6123
		[Tooltip("Partial knockback velocity is imparted between the inner and outer radius")]
		public float aeoOuterRadius;

		// Token: 0x040017EC RID: 6124
		public float knockbackVelocity;

		// Token: 0x040017ED RID: 6125
		[Tooltip("The required impact velocity to achieve full knockback velocity")]
		public float impactVelocityThreshold;

		// Token: 0x040017EE RID: 6126
		[SerializeField]
		public PlayerEffect playerProximityEffect;
	}

	// Token: 0x02000394 RID: 916
	// (Invoke) Token: 0x0600158F RID: 5519
	public delegate void ProjectileImpactEvent(SlingshotProjectile projectile, Vector3 impactPos, NetPlayer hitPlayer);
}
