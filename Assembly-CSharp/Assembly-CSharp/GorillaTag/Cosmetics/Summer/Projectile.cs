using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTag.Cosmetics.Summer
{
	// Token: 0x02000C63 RID: 3171
	public class Projectile : MonoBehaviour, IProjectile
	{
		// Token: 0x06004EFA RID: 20218 RVA: 0x00184003 File Offset: 0x00182203
		protected void Awake()
		{
			this.rigidbody = base.GetComponentInChildren<Rigidbody>();
			this.impactEffectSpawned = false;
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x000023F4 File Offset: 0x000005F4
		protected void OnEnable()
		{
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x00184018 File Offset: 0x00182218
		public void Launch(Vector3 startPosition, Quaternion startRotation, Vector3 velocity, float scale)
		{
			Transform transform = base.transform;
			transform.position = startPosition;
			transform.rotation = startRotation;
			transform.localScale = Vector3.one * scale;
			if (this.rigidbody != null)
			{
				this.rigidbody.velocity = velocity;
			}
			if (this.audioSource)
			{
				this.audioSource.GTPlayOneShot(this.launchAudio, 1f);
			}
		}

		// Token: 0x06004EFD RID: 20221 RVA: 0x00184087 File Offset: 0x00182287
		private bool IsTagValid(GameObject obj)
		{
			return this.collisionTags.Contains(obj.tag);
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x0018409C File Offset: 0x0018229C
		private void OnCollisionEnter(Collision other)
		{
			if (this.impactEffectSpawned)
			{
				return;
			}
			if (this.collisionTags.Count > 0 && !this.IsTagValid(other.gameObject))
			{
				return;
			}
			if ((1 << other.gameObject.layer & this.collisionLayerMasks) == 0)
			{
				return;
			}
			ContactPoint contact = other.GetContact(0);
			this.SpawnImpactEffect(this.impactEffect, contact.point, contact.normal);
			SoundBankPlayer component = this.impactEffect.GetComponent<SoundBankPlayer>();
			if (component != null && !component.playOnEnable)
			{
				component.Play();
			}
			this.impactEffectSpawned = true;
			if (this.destroyOnCollisionEnter)
			{
				if (this.destroyDelay > 0f)
				{
					base.Invoke("DestroyProjectile", this.destroyDelay);
					return;
				}
				this.DestroyProjectile();
			}
		}

		// Token: 0x06004EFF RID: 20223 RVA: 0x00184168 File Offset: 0x00182368
		private void OnCollisionStay(Collision other)
		{
			if (this.impactEffectSpawned)
			{
				return;
			}
			if (this.collisionTags.Count > 0 && !this.IsTagValid(other.gameObject))
			{
				return;
			}
			if ((1 << other.gameObject.layer & this.collisionLayerMasks) == 0)
			{
				return;
			}
			ContactPoint contact = other.GetContact(0);
			this.SpawnImpactEffect(this.impactEffect, contact.point, contact.normal);
			SoundBankPlayer component = this.impactEffect.GetComponent<SoundBankPlayer>();
			if (component != null && !component.playOnEnable)
			{
				component.Play();
			}
			this.impactEffectSpawned = true;
			if (this.destroyOnCollisionEnter)
			{
				if (this.destroyDelay > 0f)
				{
					base.Invoke("DestroyProjectile", this.destroyDelay);
					return;
				}
				this.DestroyProjectile();
			}
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x00184234 File Offset: 0x00182434
		private void SpawnImpactEffect(GameObject prefab, Vector3 position, Vector3 normal)
		{
			Vector3 position2 = position + normal * this.impactEffectOffset;
			GameObject gameObject = ObjectPools.instance.Instantiate(prefab, position2);
			gameObject.transform.up = normal;
			gameObject.transform.position = position2;
		}

		// Token: 0x06004F01 RID: 20225 RVA: 0x00184277 File Offset: 0x00182477
		private void DestroyProjectile()
		{
			this.impactEffectSpawned = false;
			if (ObjectPools.instance.DoesPoolExist(base.gameObject))
			{
				ObjectPools.instance.Destroy(base.gameObject);
				return;
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x040052C3 RID: 21187
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x040052C4 RID: 21188
		[SerializeField]
		private GameObject impactEffect;

		// Token: 0x040052C5 RID: 21189
		[SerializeField]
		private AudioClip launchAudio;

		// Token: 0x040052C6 RID: 21190
		[SerializeField]
		private LayerMask collisionLayerMasks;

		// Token: 0x040052C7 RID: 21191
		[SerializeField]
		private List<string> collisionTags = new List<string>();

		// Token: 0x040052C8 RID: 21192
		[SerializeField]
		private bool destroyOnCollisionEnter;

		// Token: 0x040052C9 RID: 21193
		[SerializeField]
		private float destroyDelay = 1f;

		// Token: 0x040052CA RID: 21194
		[Tooltip("Distance from the surface that the particle should spawn.")]
		[SerializeField]
		private float impactEffectOffset = 0.1f;

		// Token: 0x040052CB RID: 21195
		private bool impactEffectSpawned;

		// Token: 0x040052CC RID: 21196
		private Rigidbody rigidbody;
	}
}
