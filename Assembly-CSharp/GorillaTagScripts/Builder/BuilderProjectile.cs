using System;
using GorillaLocomotion;
using GorillaLocomotion.Swimming;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A32 RID: 2610
	public class BuilderProjectile : MonoBehaviour
	{
		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x0600417E RID: 16766 RVA: 0x0005AD12 File Offset: 0x00058F12
		// (set) Token: 0x0600417F RID: 16767 RVA: 0x0005AD1A File Offset: 0x00058F1A
		public Vector3 launchPosition { get; private set; }

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06004180 RID: 16768 RVA: 0x00171CE8 File Offset: 0x0016FEE8
		// (remove) Token: 0x06004181 RID: 16769 RVA: 0x00171D20 File Offset: 0x0016FF20
		public event BuilderProjectile.ProjectileImpactEvent OnImpact;

		// Token: 0x06004182 RID: 16770 RVA: 0x00171D58 File Offset: 0x0016FF58
		public void Launch(Vector3 position, Vector3 velocity, BuilderProjectileLauncher sourceObject, int projectileCount, float scale, int timeStamp)
		{
			this.particleLaunched = true;
			this.timeCreated = Time.time;
			this.projectileSource = sourceObject;
			float num = (NetworkSystem.Instance.ServerTimestamp - timeStamp) / 1000f;
			if (num >= this.lifeTime)
			{
				this.Deactivate();
				return;
			}
			this.timeCreated -= num;
			Vector3 vector = Vector3.ProjectOnPlane(velocity, Vector3.up);
			float f = 0.017453292f * Vector3.Angle(vector, velocity);
			float num2 = this.projectileRigidbody.mass * this.gravityMultiplier * ((scale < 1f) ? scale : 1f) * 9.8f;
			Vector3 b = num * Mathf.Cos(f) * vector;
			float d = velocity.z * num * Mathf.Sin(f) - 0.5f * num2 * num * num;
			this.launchPosition = position + b + d * Vector3.down;
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
			Vector3 vector2 = this.projectileRigidbody.mass * this.gravityMultiplier * ((scale < 1f) ? scale : 1f) * Physics.gravity;
			this.forceComponent.force = vector2;
			this.projectileRigidbody.velocity = velocity + num * vector2;
			this.projectileId = projectileCount;
			this.projectileRigidbody.position = position;
			this.projectileSource.RegisterProjectile(this);
		}

		// Token: 0x06004183 RID: 16771 RVA: 0x0005AD23 File Offset: 0x00058F23
		protected void Awake()
		{
			this.projectileRigidbody = base.GetComponent<Rigidbody>();
			this.forceComponent = base.GetComponent<ConstantForce>();
			this.initialScale = base.transform.localScale.x;
		}

		// Token: 0x06004184 RID: 16772 RVA: 0x00171F1C File Offset: 0x0017011C
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

		// Token: 0x06004185 RID: 16773 RVA: 0x00171FB4 File Offset: 0x001701B4
		private void SpawnImpactEffect(GameObject prefab, Vector3 position, Vector3 normal)
		{
			Vector3 position2 = position + normal * this.impactEffectOffset;
			GameObject gameObject = ObjectPools.instance.Instantiate(prefab, position2);
			Vector3 localScale = base.transform.localScale;
			gameObject.transform.localScale = localScale * this.impactEffectScaleMultiplier;
			gameObject.transform.up = normal;
			SurfaceImpactFX component = gameObject.GetComponent<SurfaceImpactFX>();
			if (component != null)
			{
				component.SetScale(localScale.x * this.impactEffectScaleMultiplier);
			}
			SoundBankPlayer component2 = gameObject.GetComponent<SoundBankPlayer>();
			if (component2 != null && !component2.playOnEnable)
			{
				component2.Play(this.impactSoundVolumeOverride, this.impactSoundPitchOverride);
			}
		}

		// Token: 0x06004186 RID: 16774 RVA: 0x0017205C File Offset: 0x0017025C
		public void ApplyHitKnockback(Vector3 hitNormal)
		{
			if (this.aoeKnockbackConfig != null && this.aoeKnockbackConfig.Value.applyAOEKnockback)
			{
				Vector3 a = Vector3.ProjectOnPlane(hitNormal, Vector3.up);
				a.Normalize();
				Vector3 direction = 0.75f * a + 0.25f * Vector3.up;
				direction.Normalize();
				GTPlayer instance = GTPlayer.Instance;
				instance.ApplyKnockback(direction, this.aoeKnockbackConfig.Value.knockbackVelocity, instance.scale < 0.9f);
			}
		}

		// Token: 0x06004187 RID: 16775 RVA: 0x0005AD53 File Offset: 0x00058F53
		private void OnEnable()
		{
			this.timeCreated = 0f;
			this.particleLaunched = false;
		}

		// Token: 0x06004188 RID: 16776 RVA: 0x0005AD67 File Offset: 0x00058F67
		protected void OnDisable()
		{
			this.particleLaunched = false;
			if (this.projectileSource != null)
			{
				this.projectileSource.UnRegisterProjectile(this);
			}
			this.projectileSource = null;
		}

		// Token: 0x06004189 RID: 16777 RVA: 0x001720EC File Offset: 0x001702EC
		public void UpdateProjectile()
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

		// Token: 0x0600418A RID: 16778 RVA: 0x00172168 File Offset: 0x00170368
		private void OnCollisionEnter(Collision other)
		{
			if (!this.particleLaunched)
			{
				return;
			}
			BuilderPieceCollider component = other.transform.GetComponent<BuilderPieceCollider>();
			if (component != null && component.piece.gameObject.Equals(this.projectileSource.gameObject))
			{
				return;
			}
			ContactPoint contact = other.GetContact(0);
			if (other.collider.gameObject.IsOnLayer(UnityLayer.GorillaBodyCollider))
			{
				this.ApplyHitKnockback(-1f * contact.normal);
			}
			this.SpawnImpactEffect(this.surfaceImpactEffectPrefab, contact.point, contact.normal);
			BuilderProjectile.ProjectileImpactEvent onImpact = this.OnImpact;
			if (onImpact != null)
			{
				onImpact(this, contact.point, null);
			}
			this.Deactivate();
		}

		// Token: 0x0600418B RID: 16779 RVA: 0x00172168 File Offset: 0x00170368
		protected void OnCollisionStay(Collision other)
		{
			if (!this.particleLaunched)
			{
				return;
			}
			BuilderPieceCollider component = other.transform.GetComponent<BuilderPieceCollider>();
			if (component != null && component.piece.gameObject.Equals(this.projectileSource.gameObject))
			{
				return;
			}
			ContactPoint contact = other.GetContact(0);
			if (other.collider.gameObject.IsOnLayer(UnityLayer.GorillaBodyCollider))
			{
				this.ApplyHitKnockback(-1f * contact.normal);
			}
			this.SpawnImpactEffect(this.surfaceImpactEffectPrefab, contact.point, contact.normal);
			BuilderProjectile.ProjectileImpactEvent onImpact = this.OnImpact;
			if (onImpact != null)
			{
				onImpact(this, contact.point, null);
			}
			this.Deactivate();
		}

		// Token: 0x0600418C RID: 16780 RVA: 0x00172220 File Offset: 0x00170420
		protected void OnTriggerEnter(Collider other)
		{
			if (!this.particleLaunched)
			{
				return;
			}
			if (!NetworkSystem.Instance.InRoom || GorillaGameManager.instance == null)
			{
				return;
			}
			if (!other.gameObject.IsOnLayer(UnityLayer.GorillaTagCollider))
			{
				return;
			}
			VRRig componentInParent = other.GetComponentInParent<VRRig>();
			NetPlayer netPlayer = (componentInParent != null) ? componentInParent.creator : null;
			if (netPlayer == null)
			{
				return;
			}
			if (netPlayer.IsLocal)
			{
				return;
			}
			this.SpawnImpactEffect(this.surfaceImpactEffectPrefab, base.transform.position, Vector3.up);
			this.Deactivate();
		}

		// Token: 0x0400424C RID: 16972
		public BuilderProjectileLauncher projectileSource;

		// Token: 0x0400424D RID: 16973
		[Tooltip("Rotates to point along the Y axis after spawn.")]
		public GameObject surfaceImpactEffectPrefab;

		// Token: 0x0400424E RID: 16974
		[Tooltip("Distance from the surface that the particle should spawn.")]
		private float impactEffectOffset;

		// Token: 0x0400424F RID: 16975
		public float lifeTime = 20f;

		// Token: 0x04004250 RID: 16976
		public bool faceDirectionOfTravel = true;

		// Token: 0x04004251 RID: 16977
		private bool particleLaunched;

		// Token: 0x04004252 RID: 16978
		private float timeCreated;

		// Token: 0x04004254 RID: 16980
		private Rigidbody projectileRigidbody;

		// Token: 0x04004255 RID: 16981
		public int projectileId;

		// Token: 0x04004256 RID: 16982
		private float initialScale;

		// Token: 0x04004257 RID: 16983
		private Vector3 previousPosition;

		// Token: 0x04004258 RID: 16984
		[HideInInspector]
		public SlingshotProjectile.AOEKnockbackConfig? aoeKnockbackConfig;

		// Token: 0x04004259 RID: 16985
		[HideInInspector]
		public float? impactSoundVolumeOverride;

		// Token: 0x0400425A RID: 16986
		[HideInInspector]
		public float? impactSoundPitchOverride;

		// Token: 0x0400425B RID: 16987
		[HideInInspector]
		public float impactEffectScaleMultiplier = 1f;

		// Token: 0x0400425C RID: 16988
		[HideInInspector]
		public float gravityMultiplier = 1f;

		// Token: 0x0400425D RID: 16989
		private ConstantForce forceComponent;

		// Token: 0x02000A33 RID: 2611
		// (Invoke) Token: 0x0600418F RID: 16783
		public delegate void ProjectileImpactEvent(BuilderProjectile projectile, Vector3 impactPos, NetPlayer hitPlayer);
	}
}
