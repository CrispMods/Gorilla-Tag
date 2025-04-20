using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTag.Cosmetics.Summer
{
	// Token: 0x02000C91 RID: 3217
	public class Projectile : MonoBehaviour, IProjectile
	{
		// Token: 0x0600504E RID: 20558 RVA: 0x000647F3 File Offset: 0x000629F3
		protected void Awake()
		{
			this.rigidbody = base.GetComponentInChildren<Rigidbody>();
			this.impactEffectSpawned = false;
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x00030607 File Offset: 0x0002E807
		protected void OnEnable()
		{
		}

		// Token: 0x06005050 RID: 20560 RVA: 0x001BB978 File Offset: 0x001B9B78
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

		// Token: 0x06005051 RID: 20561 RVA: 0x00064808 File Offset: 0x00062A08
		private bool IsTagValid(GameObject obj)
		{
			return this.collisionTags.Contains(obj.tag);
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x001BB9E8 File Offset: 0x001B9BE8
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

		// Token: 0x06005053 RID: 20563 RVA: 0x001BB9E8 File Offset: 0x001B9BE8
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

		// Token: 0x06005054 RID: 20564 RVA: 0x001BBAB4 File Offset: 0x001B9CB4
		private void SpawnImpactEffect(GameObject prefab, Vector3 position, Vector3 normal)
		{
			Vector3 position2 = position + normal * this.impactEffectOffset;
			GameObject gameObject = ObjectPools.instance.Instantiate(prefab, position2);
			gameObject.transform.up = normal;
			gameObject.transform.position = position2;
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x0006481B File Offset: 0x00062A1B
		private void DestroyProjectile()
		{
			this.impactEffectSpawned = false;
			if (ObjectPools.instance.DoesPoolExist(base.gameObject))
			{
				ObjectPools.instance.Destroy(base.gameObject);
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x040053BD RID: 21437
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x040053BE RID: 21438
		[SerializeField]
		private GameObject impactEffect;

		// Token: 0x040053BF RID: 21439
		[SerializeField]
		private AudioClip launchAudio;

		// Token: 0x040053C0 RID: 21440
		[SerializeField]
		private LayerMask collisionLayerMasks;

		// Token: 0x040053C1 RID: 21441
		[SerializeField]
		private List<string> collisionTags = new List<string>();

		// Token: 0x040053C2 RID: 21442
		[SerializeField]
		private bool destroyOnCollisionEnter;

		// Token: 0x040053C3 RID: 21443
		[SerializeField]
		private float destroyDelay = 1f;

		// Token: 0x040053C4 RID: 21444
		[Tooltip("Distance from the surface that the particle should spawn.")]
		[SerializeField]
		private float impactEffectOffset = 0.1f;

		// Token: 0x040053C5 RID: 21445
		private bool impactEffectSpawned;

		// Token: 0x040053C6 RID: 21446
		private Rigidbody rigidbody;
	}
}
