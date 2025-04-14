using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTag.Cosmetics.Summer
{
	// Token: 0x02000C60 RID: 3168
	public class Projectile : MonoBehaviour, IProjectile
	{
		// Token: 0x06004EEE RID: 20206 RVA: 0x00183A3B File Offset: 0x00181C3B
		protected void Awake()
		{
			this.rigidbody = base.GetComponentInChildren<Rigidbody>();
			this.impactEffectSpawned = false;
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x000023F4 File Offset: 0x000005F4
		protected void OnEnable()
		{
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x00183A50 File Offset: 0x00181C50
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

		// Token: 0x06004EF1 RID: 20209 RVA: 0x00183ABF File Offset: 0x00181CBF
		private bool IsTagValid(GameObject obj)
		{
			return this.collisionTags.Contains(obj.tag);
		}

		// Token: 0x06004EF2 RID: 20210 RVA: 0x00183AD4 File Offset: 0x00181CD4
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

		// Token: 0x06004EF3 RID: 20211 RVA: 0x00183BA0 File Offset: 0x00181DA0
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

		// Token: 0x06004EF4 RID: 20212 RVA: 0x00183C6C File Offset: 0x00181E6C
		private void SpawnImpactEffect(GameObject prefab, Vector3 position, Vector3 normal)
		{
			Vector3 position2 = position + normal * this.impactEffectOffset;
			GameObject gameObject = ObjectPools.instance.Instantiate(prefab, position2);
			gameObject.transform.up = normal;
			gameObject.transform.position = position2;
		}

		// Token: 0x06004EF5 RID: 20213 RVA: 0x00183CAF File Offset: 0x00181EAF
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

		// Token: 0x040052B1 RID: 21169
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x040052B2 RID: 21170
		[SerializeField]
		private GameObject impactEffect;

		// Token: 0x040052B3 RID: 21171
		[SerializeField]
		private AudioClip launchAudio;

		// Token: 0x040052B4 RID: 21172
		[SerializeField]
		private LayerMask collisionLayerMasks;

		// Token: 0x040052B5 RID: 21173
		[SerializeField]
		private List<string> collisionTags = new List<string>();

		// Token: 0x040052B6 RID: 21174
		[SerializeField]
		private bool destroyOnCollisionEnter;

		// Token: 0x040052B7 RID: 21175
		[SerializeField]
		private float destroyDelay = 1f;

		// Token: 0x040052B8 RID: 21176
		[Tooltip("Distance from the surface that the particle should spawn.")]
		[SerializeField]
		private float impactEffectOffset = 0.1f;

		// Token: 0x040052B9 RID: 21177
		private bool impactEffectSpawned;

		// Token: 0x040052BA RID: 21178
		private Rigidbody rigidbody;
	}
}
