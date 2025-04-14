using System;
using System.Collections;
using GorillaTag.Cosmetics;
using UnityEngine;

namespace GorillaTag.Shared.Scripts
{
	// Token: 0x02000BE4 RID: 3044
	public class FirecrackerProjectile : MonoBehaviour, ITickSystemTick, IProjectile
	{
		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06004C97 RID: 19607 RVA: 0x00174859 File Offset: 0x00172A59
		// (set) Token: 0x06004C98 RID: 19608 RVA: 0x00174861 File Offset: 0x00172A61
		public bool TickRunning { get; set; }

		// Token: 0x06004C99 RID: 19609 RVA: 0x0017486A File Offset: 0x00172A6A
		public void Tick()
		{
			if (Time.time - this.timeCreated > this.forceBackToPoolAfterSec || Time.time - this.timeExploded > this.explosionTime)
			{
				Action<FirecrackerProjectile> onHitComplete = this.OnHitComplete;
				if (onHitComplete == null)
				{
					return;
				}
				onHitComplete(this);
			}
		}

		// Token: 0x06004C9A RID: 19610 RVA: 0x001748A8 File Offset: 0x00172AA8
		private void OnEnable()
		{
			TickSystem<object>.AddCallbackTarget(this);
			this.m_timer.Start();
			this.timeExploded = float.PositiveInfinity;
			this.timeCreated = float.PositiveInfinity;
			this.collisionEntered = false;
			if (this.disableWhenHit)
			{
				this.disableWhenHit.SetActive(true);
			}
		}

		// Token: 0x06004C9B RID: 19611 RVA: 0x001748FC File Offset: 0x00172AFC
		private void OnDisable()
		{
			TickSystem<object>.RemoveCallbackTarget(this);
			this.m_timer.Stop();
		}

		// Token: 0x06004C9C RID: 19612 RVA: 0x0017490F File Offset: 0x00172B0F
		private void Awake()
		{
			this.rb = base.GetComponent<Rigidbody>();
			this.audioSource = base.GetComponent<AudioSource>();
			this.m_timer.callback = new Action(this.Detonate);
		}

		// Token: 0x06004C9D RID: 19613 RVA: 0x00174940 File Offset: 0x00172B40
		private void Detonate()
		{
			this.m_timer.Stop();
			this.timeExploded = Time.time;
			if (this.disableWhenHit)
			{
				this.disableWhenHit.SetActive(false);
			}
			this.collisionEntered = false;
		}

		// Token: 0x06004C9E RID: 19614 RVA: 0x00174978 File Offset: 0x00172B78
		public void Launch(Vector3 startPosition, Quaternion startRotation, Vector3 velocity, float scale)
		{
			base.transform.position = startPosition;
			base.transform.rotation = startRotation;
			base.transform.localScale = Vector3.one * scale;
			this.rb.velocity = velocity;
		}

		// Token: 0x06004C9F RID: 19615 RVA: 0x001749B8 File Offset: 0x00172BB8
		private void OnCollisionEnter(Collision other)
		{
			if (this.collisionEntered)
			{
				return;
			}
			Vector3 point = other.contacts[0].point;
			Vector3 normal = other.contacts[0].normal;
			if (this.sizzleDuration > 0f)
			{
				base.StartCoroutine(this.Sizzle(point, normal));
			}
			else
			{
				Action<FirecrackerProjectile, Vector3> onHitStart = this.OnHitStart;
				if (onHitStart != null)
				{
					onHitStart(this, point);
				}
				this.Detonate(point, normal);
			}
			this.collisionEntered = true;
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x00174A32 File Offset: 0x00172C32
		private IEnumerator Sizzle(Vector3 contactPoint, Vector3 normal)
		{
			if (this.audioSource && this.sizzleAudioClip != null)
			{
				this.audioSource.GTPlayOneShot(this.sizzleAudioClip, 1f);
			}
			yield return new WaitForSeconds(this.sizzleDuration);
			Action<FirecrackerProjectile, Vector3> onHitStart = this.OnHitStart;
			if (onHitStart != null)
			{
				onHitStart(this, contactPoint);
			}
			this.Detonate(contactPoint, normal);
			yield break;
		}

		// Token: 0x06004CA1 RID: 19617 RVA: 0x00174A50 File Offset: 0x00172C50
		private void Detonate(Vector3 contactPoint, Vector3 normal)
		{
			this.timeExploded = Time.time;
			GameObject gameObject = ObjectPools.instance.Instantiate(this.explosionEffect, contactPoint);
			gameObject.transform.up = normal;
			gameObject.transform.position = base.transform.position;
			SoundBankPlayer soundBankPlayer;
			if (gameObject.TryGetComponent<SoundBankPlayer>(out soundBankPlayer) && soundBankPlayer.soundBank)
			{
				soundBankPlayer.Play();
			}
			if (this.disableWhenHit)
			{
				this.disableWhenHit.SetActive(false);
			}
			this.collisionEntered = false;
		}

		// Token: 0x04004E5B RID: 20059
		[SerializeField]
		private GameObject explosionEffect;

		// Token: 0x04004E5C RID: 20060
		[SerializeField]
		private float forceBackToPoolAfterSec = 20f;

		// Token: 0x04004E5D RID: 20061
		[SerializeField]
		private float explosionTime = 5f;

		// Token: 0x04004E5E RID: 20062
		[SerializeField]
		private GameObject disableWhenHit;

		// Token: 0x04004E5F RID: 20063
		[SerializeField]
		private float sizzleDuration;

		// Token: 0x04004E60 RID: 20064
		[SerializeField]
		private AudioClip sizzleAudioClip;

		// Token: 0x04004E61 RID: 20065
		public Action<FirecrackerProjectile> OnHitComplete;

		// Token: 0x04004E62 RID: 20066
		public Action<FirecrackerProjectile, Vector3> OnHitStart;

		// Token: 0x04004E63 RID: 20067
		private Rigidbody rb;

		// Token: 0x04004E64 RID: 20068
		private float timeCreated = float.PositiveInfinity;

		// Token: 0x04004E65 RID: 20069
		private float timeExploded = float.PositiveInfinity;

		// Token: 0x04004E66 RID: 20070
		private AudioSource audioSource;

		// Token: 0x04004E67 RID: 20071
		private TickSystemTimer m_timer = new TickSystemTimer(40f);

		// Token: 0x04004E68 RID: 20072
		private bool collisionEntered;
	}
}
