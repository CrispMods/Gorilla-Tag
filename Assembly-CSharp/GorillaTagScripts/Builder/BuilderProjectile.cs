using System;
using GorillaLocomotion;
using GorillaLocomotion.Swimming;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A05 RID: 2565
	public class BuilderProjectile : MonoBehaviour
	{
		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06004039 RID: 16441 RVA: 0x00131035 File Offset: 0x0012F235
		// (set) Token: 0x0600403A RID: 16442 RVA: 0x0013103D File Offset: 0x0012F23D
		public Vector3 launchPosition { get; private set; }

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x0600403B RID: 16443 RVA: 0x00131048 File Offset: 0x0012F248
		// (remove) Token: 0x0600403C RID: 16444 RVA: 0x00131080 File Offset: 0x0012F280
		public event BuilderProjectile.ProjectileImpactEvent OnImpact;

		// Token: 0x0600403D RID: 16445 RVA: 0x001310B8 File Offset: 0x0012F2B8
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

		// Token: 0x0600403E RID: 16446 RVA: 0x00131279 File Offset: 0x0012F479
		protected void Awake()
		{
			this.projectileRigidbody = base.GetComponent<Rigidbody>();
			this.forceComponent = base.GetComponent<ConstantForce>();
			this.initialScale = base.transform.localScale.x;
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x001312AC File Offset: 0x0012F4AC
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

		// Token: 0x06004040 RID: 16448 RVA: 0x00131344 File Offset: 0x0012F544
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

		// Token: 0x06004041 RID: 16449 RVA: 0x001313EC File Offset: 0x0012F5EC
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

		// Token: 0x06004042 RID: 16450 RVA: 0x0013147C File Offset: 0x0012F67C
		private void OnEnable()
		{
			this.timeCreated = 0f;
			this.particleLaunched = false;
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x00131490 File Offset: 0x0012F690
		protected void OnDisable()
		{
			this.particleLaunched = false;
			if (this.projectileSource != null)
			{
				this.projectileSource.UnRegisterProjectile(this);
			}
			this.projectileSource = null;
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x001314BC File Offset: 0x0012F6BC
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

		// Token: 0x06004045 RID: 16453 RVA: 0x00131538 File Offset: 0x0012F738
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

		// Token: 0x06004046 RID: 16454 RVA: 0x001315F0 File Offset: 0x0012F7F0
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

		// Token: 0x06004047 RID: 16455 RVA: 0x001316A8 File Offset: 0x0012F8A8
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

		// Token: 0x04004152 RID: 16722
		public BuilderProjectileLauncher projectileSource;

		// Token: 0x04004153 RID: 16723
		[Tooltip("Rotates to point along the Y axis after spawn.")]
		public GameObject surfaceImpactEffectPrefab;

		// Token: 0x04004154 RID: 16724
		[Tooltip("Distance from the surface that the particle should spawn.")]
		private float impactEffectOffset;

		// Token: 0x04004155 RID: 16725
		public float lifeTime = 20f;

		// Token: 0x04004156 RID: 16726
		public bool faceDirectionOfTravel = true;

		// Token: 0x04004157 RID: 16727
		private bool particleLaunched;

		// Token: 0x04004158 RID: 16728
		private float timeCreated;

		// Token: 0x0400415A RID: 16730
		private Rigidbody projectileRigidbody;

		// Token: 0x0400415B RID: 16731
		public int projectileId;

		// Token: 0x0400415C RID: 16732
		private float initialScale;

		// Token: 0x0400415D RID: 16733
		private Vector3 previousPosition;

		// Token: 0x0400415E RID: 16734
		[HideInInspector]
		public SlingshotProjectile.AOEKnockbackConfig? aoeKnockbackConfig;

		// Token: 0x0400415F RID: 16735
		[HideInInspector]
		public float? impactSoundVolumeOverride;

		// Token: 0x04004160 RID: 16736
		[HideInInspector]
		public float? impactSoundPitchOverride;

		// Token: 0x04004161 RID: 16737
		[HideInInspector]
		public float impactEffectScaleMultiplier = 1f;

		// Token: 0x04004162 RID: 16738
		[HideInInspector]
		public float gravityMultiplier = 1f;

		// Token: 0x04004163 RID: 16739
		private ConstantForce forceComponent;

		// Token: 0x02000A06 RID: 2566
		// (Invoke) Token: 0x0600404A RID: 16458
		public delegate void ProjectileImpactEvent(BuilderProjectile projectile, Vector3 impactPos, NetPlayer hitPlayer);
	}
}
