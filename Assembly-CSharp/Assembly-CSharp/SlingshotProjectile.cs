using System;
using GorillaGameModes;
using GorillaLocomotion;
using GorillaLocomotion.Swimming;
using GorillaTag.Reactions;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000387 RID: 903
public class SlingshotProjectile : MonoBehaviour
{
	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06001530 RID: 5424 RVA: 0x00067C08 File Offset: 0x00065E08
	// (set) Token: 0x06001531 RID: 5425 RVA: 0x00067C10 File Offset: 0x00065E10
	public Vector3 launchPosition { get; private set; }

	// Token: 0x1400003F RID: 63
	// (add) Token: 0x06001532 RID: 5426 RVA: 0x00067C1C File Offset: 0x00065E1C
	// (remove) Token: 0x06001533 RID: 5427 RVA: 0x00067C54 File Offset: 0x00065E54
	public event SlingshotProjectile.ProjectileImpactEvent OnImpact;

	// Token: 0x06001534 RID: 5428 RVA: 0x00067C8C File Offset: 0x00065E8C
	public void Launch(Vector3 position, Vector3 velocity, NetPlayer player, bool blueTeam, bool orangeTeam, int projectileCount, float scale, bool shouldOverrideColor = false, Color overrideColor = default(Color))
	{
		if (this.playerImpactEffectPrefab != null)
		{
			RoomSystem.playerImpactEffectPrefab = this.playerImpactEffectPrefab;
		}
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

	// Token: 0x06001535 RID: 5429 RVA: 0x00067DB8 File Offset: 0x00065FB8
	protected void Awake()
	{
		this.projectileRigidbody = base.GetComponent<Rigidbody>();
		this.forceComponent = base.GetComponent<ConstantForce>();
		this.initialScale = base.transform.localScale.x;
		this.matPropBlock = new MaterialPropertyBlock();
		this.spawnWorldEffects = base.GetComponent<SpawnWorldEffects>();
		this.defaultPlayerImpact = RoomSystem.playerImpactEffectPrefab;
	}

	// Token: 0x06001536 RID: 5430 RVA: 0x00067E18 File Offset: 0x00066018
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
		if (this.playerImpactEffectPrefab != null)
		{
			RoomSystem.playerImpactEffectPrefab = this.defaultPlayerImpact;
		}
		ObjectPools.instance.Destroy(base.gameObject);
	}

	// Token: 0x06001537 RID: 5431 RVA: 0x00067EC8 File Offset: 0x000660C8
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

	// Token: 0x06001538 RID: 5432 RVA: 0x00067FAC File Offset: 0x000661AC
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

	// Token: 0x06001539 RID: 5433 RVA: 0x00068150 File Offset: 0x00066350
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

	// Token: 0x0600153A RID: 5434 RVA: 0x000681FE File Offset: 0x000663FE
	protected void OnEnable()
	{
		this.timeCreated = 0f;
		this.particleLaunched = false;
		SlingshotProjectileManager.RegisterSP(this);
	}

	// Token: 0x0600153B RID: 5435 RVA: 0x00068218 File Offset: 0x00066418
	protected void OnDisable()
	{
		this.particleLaunched = false;
		SlingshotProjectileManager.UnregisterSP(this);
	}

	// Token: 0x0600153C RID: 5436 RVA: 0x00068228 File Offset: 0x00066428
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

	// Token: 0x0600153D RID: 5437 RVA: 0x000682A3 File Offset: 0x000664A3
	public void DestroyAfterRelease()
	{
		this.SpawnImpactEffect(this.surfaceImpactEffectPrefab, base.transform.position, Vector3.up);
		this.Deactivate();
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x000682C8 File Offset: 0x000664C8
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

	// Token: 0x0600153F RID: 5439 RVA: 0x00068358 File Offset: 0x00066558
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

	// Token: 0x06001540 RID: 5440 RVA: 0x000683E4 File Offset: 0x000665E4
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

	// Token: 0x06001541 RID: 5441 RVA: 0x00068414 File Offset: 0x00066614
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
		UnityEvent<VRRig> onHitPlayer = this.OnHitPlayer;
		if (onHitPlayer == null)
		{
			return;
		}
		onHitPlayer.Invoke(other.GetComponentInParent<VRRig>());
	}

	// Token: 0x06001542 RID: 5442 RVA: 0x000685BA File Offset: 0x000667BA
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

	// Token: 0x0400177F RID: 6015
	public NetPlayer projectileOwner;

	// Token: 0x04001780 RID: 6016
	[Tooltip("Rotates to point along the Y axis after spawn.")]
	public GameObject surfaceImpactEffectPrefab;

	// Token: 0x04001781 RID: 6017
	[Tooltip("if left empty, the default player impact that is set in Room System Setting will be played")]
	public GameObject playerImpactEffectPrefab;

	// Token: 0x04001782 RID: 6018
	[Tooltip("Distance from the surface that the particle should spawn.")]
	[SerializeField]
	private float impactEffectOffset;

	// Token: 0x04001783 RID: 6019
	[SerializeField]
	private SoundBankPlayer launchSoundBankPlayer;

	// Token: 0x04001784 RID: 6020
	public float lifeTime = 20f;

	// Token: 0x04001785 RID: 6021
	public Color defaultColor = Color.white;

	// Token: 0x04001786 RID: 6022
	public Color orangeColor = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x04001787 RID: 6023
	public Color blueColor = new Color(0f, 0.72f, 1f, 1f);

	// Token: 0x04001788 RID: 6024
	[Tooltip("Renderers with team specific meshes, materials, effects, etc.")]
	public Renderer defaultBall;

	// Token: 0x04001789 RID: 6025
	[Tooltip("Renderers with team specific meshes, materials, effects, etc.")]
	public Renderer orangeBall;

	// Token: 0x0400178A RID: 6026
	[Tooltip("Renderers with team specific meshes, materials, effects, etc.")]
	public Renderer blueBall;

	// Token: 0x0400178B RID: 6027
	public bool colorizeBalls;

	// Token: 0x0400178C RID: 6028
	public bool faceDirectionOfTravel = true;

	// Token: 0x0400178D RID: 6029
	private bool particleLaunched;

	// Token: 0x0400178E RID: 6030
	private float timeCreated;

	// Token: 0x04001790 RID: 6032
	private Rigidbody projectileRigidbody;

	// Token: 0x04001791 RID: 6033
	private Color teamColor = Color.white;

	// Token: 0x04001792 RID: 6034
	private Renderer teamRenderer;

	// Token: 0x04001793 RID: 6035
	public int myProjectileCount;

	// Token: 0x04001794 RID: 6036
	private float initialScale;

	// Token: 0x04001795 RID: 6037
	private Vector3 previousPosition;

	// Token: 0x04001796 RID: 6038
	[HideInInspector]
	public SlingshotProjectile.AOEKnockbackConfig? aoeKnockbackConfig;

	// Token: 0x04001797 RID: 6039
	[HideInInspector]
	public float? impactSoundVolumeOverride;

	// Token: 0x04001798 RID: 6040
	[HideInInspector]
	public float? impactSoundPitchOverride;

	// Token: 0x04001799 RID: 6041
	[HideInInspector]
	public float impactEffectScaleMultiplier = 1f;

	// Token: 0x0400179A RID: 6042
	[HideInInspector]
	public float gravityMultiplier = 1f;

	// Token: 0x0400179B RID: 6043
	private ConstantForce forceComponent;

	// Token: 0x0400179C RID: 6044
	private GameObject defaultPlayerImpact;

	// Token: 0x0400179E RID: 6046
	private MaterialPropertyBlock matPropBlock;

	// Token: 0x0400179F RID: 6047
	private SpawnWorldEffects spawnWorldEffects;

	// Token: 0x040017A0 RID: 6048
	private static readonly int colorShaderProp = Shader.PropertyToID("_Color");

	// Token: 0x040017A1 RID: 6049
	private static readonly int baseColorShaderProp = Shader.PropertyToID("_BaseColor");

	// Token: 0x040017A2 RID: 6050
	public UnityEvent<VRRig> OnHitPlayer;

	// Token: 0x02000388 RID: 904
	[Serializable]
	public struct AOEKnockbackConfig
	{
		// Token: 0x040017A3 RID: 6051
		public bool applyAOEKnockback;

		// Token: 0x040017A4 RID: 6052
		[Tooltip("Full knockback velocity is imparted within the inner radius")]
		public float aeoInnerRadius;

		// Token: 0x040017A5 RID: 6053
		[Tooltip("Partial knockback velocity is imparted between the inner and outer radius")]
		public float aeoOuterRadius;

		// Token: 0x040017A6 RID: 6054
		public float knockbackVelocity;

		// Token: 0x040017A7 RID: 6055
		[Tooltip("The required impact velocity to achieve full knockback velocity")]
		public float impactVelocityThreshold;

		// Token: 0x040017A8 RID: 6056
		[SerializeField]
		public PlayerEffect playerProximityEffect;
	}

	// Token: 0x02000389 RID: 905
	// (Invoke) Token: 0x06001546 RID: 5446
	public delegate void ProjectileImpactEvent(SlingshotProjectile projectile, Vector3 impactPos, NetPlayer hitPlayer);
}
