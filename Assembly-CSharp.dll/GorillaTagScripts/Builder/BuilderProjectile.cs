using System;
using GorillaLocomotion;
using GorillaLocomotion.Swimming;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A08 RID: 2568
	public class BuilderProjectile : MonoBehaviour
	{
		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06004045 RID: 16453 RVA: 0x00059310 File Offset: 0x00057510
		// (set) Token: 0x06004046 RID: 16454 RVA: 0x00059318 File Offset: 0x00057518
		public Vector3 launchPosition { get; private set; }

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x06004047 RID: 16455 RVA: 0x0016AE64 File Offset: 0x00169064
		// (remove) Token: 0x06004048 RID: 16456 RVA: 0x0016AE9C File Offset: 0x0016909C
		public event BuilderProjectile.ProjectileImpactEvent OnImpact;

		// Token: 0x06004049 RID: 16457 RVA: 0x0016AED4 File Offset: 0x001690D4
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

		// Token: 0x0600404A RID: 16458 RVA: 0x00059321 File Offset: 0x00057521
		protected void Awake()
		{
			this.projectileRigidbody = base.GetComponent<Rigidbody>();
			this.forceComponent = base.GetComponent<ConstantForce>();
			this.initialScale = base.transform.localScale.x;
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x0016B098 File Offset: 0x00169298
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

		// Token: 0x0600404C RID: 16460 RVA: 0x0016B130 File Offset: 0x00169330
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

		// Token: 0x0600404D RID: 16461 RVA: 0x0016B1D8 File Offset: 0x001693D8
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

		// Token: 0x0600404E RID: 16462 RVA: 0x00059351 File Offset: 0x00057551
		private void OnEnable()
		{
			this.timeCreated = 0f;
			this.particleLaunched = false;
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x00059365 File Offset: 0x00057565
		protected void OnDisable()
		{
			this.particleLaunched = false;
			if (this.projectileSource != null)
			{
				this.projectileSource.UnRegisterProjectile(this);
			}
			this.projectileSource = null;
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x0016B268 File Offset: 0x00169468
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

		// Token: 0x06004051 RID: 16465 RVA: 0x0016B2E4 File Offset: 0x001694E4
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

		// Token: 0x06004052 RID: 16466 RVA: 0x0016B2E4 File Offset: 0x001694E4
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

		// Token: 0x06004053 RID: 16467 RVA: 0x0016B39C File Offset: 0x0016959C
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

		// Token: 0x04004164 RID: 16740
		public BuilderProjectileLauncher projectileSource;

		// Token: 0x04004165 RID: 16741
		[Tooltip("Rotates to point along the Y axis after spawn.")]
		public GameObject surfaceImpactEffectPrefab;

		// Token: 0x04004166 RID: 16742
		[Tooltip("Distance from the surface that the particle should spawn.")]
		private float impactEffectOffset;

		// Token: 0x04004167 RID: 16743
		public float lifeTime = 20f;

		// Token: 0x04004168 RID: 16744
		public bool faceDirectionOfTravel = true;

		// Token: 0x04004169 RID: 16745
		private bool particleLaunched;

		// Token: 0x0400416A RID: 16746
		private float timeCreated;

		// Token: 0x0400416C RID: 16748
		private Rigidbody projectileRigidbody;

		// Token: 0x0400416D RID: 16749
		public int projectileId;

		// Token: 0x0400416E RID: 16750
		private float initialScale;

		// Token: 0x0400416F RID: 16751
		private Vector3 previousPosition;

		// Token: 0x04004170 RID: 16752
		[HideInInspector]
		public SlingshotProjectile.AOEKnockbackConfig? aoeKnockbackConfig;

		// Token: 0x04004171 RID: 16753
		[HideInInspector]
		public float? impactSoundVolumeOverride;

		// Token: 0x04004172 RID: 16754
		[HideInInspector]
		public float? impactSoundPitchOverride;

		// Token: 0x04004173 RID: 16755
		[HideInInspector]
		public float impactEffectScaleMultiplier = 1f;

		// Token: 0x04004174 RID: 16756
		[HideInInspector]
		public float gravityMultiplier = 1f;

		// Token: 0x04004175 RID: 16757
		private ConstantForce forceComponent;

		// Token: 0x02000A09 RID: 2569
		// (Invoke) Token: 0x06004056 RID: 16470
		public delegate void ProjectileImpactEvent(BuilderProjectile projectile, Vector3 impactPos, NetPlayer hitPlayer);
	}
}
