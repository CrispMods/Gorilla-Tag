using System;
using System.Collections;
using GorillaTag.Cosmetics;
using UnityEngine;

namespace GorillaTag.Shared.Scripts
{
	// Token: 0x02000C12 RID: 3090
	public class FirecrackerProjectile : MonoBehaviour, ITickSystemTick, IProjectile
	{
		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06004DE3 RID: 19939 RVA: 0x00062F6C File Offset: 0x0006116C
		// (set) Token: 0x06004DE4 RID: 19940 RVA: 0x00062F74 File Offset: 0x00061174
		public bool TickRunning { get; set; }

		// Token: 0x06004DE5 RID: 19941 RVA: 0x00062F7D File Offset: 0x0006117D
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

		// Token: 0x06004DE6 RID: 19942 RVA: 0x001ACFD4 File Offset: 0x001AB1D4
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

		// Token: 0x06004DE7 RID: 19943 RVA: 0x00062FB8 File Offset: 0x000611B8
		private void OnDisable()
		{
			TickSystem<object>.RemoveCallbackTarget(this);
			this.m_timer.Stop();
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x00062FCB File Offset: 0x000611CB
		private void Awake()
		{
			this.rb = base.GetComponent<Rigidbody>();
			this.audioSource = base.GetComponent<AudioSource>();
			this.m_timer.callback = new Action(this.Detonate);
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x00062FFC File Offset: 0x000611FC
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

		// Token: 0x06004DEA RID: 19946 RVA: 0x00063034 File Offset: 0x00061234
		public void Launch(Vector3 startPosition, Quaternion startRotation, Vector3 velocity, float scale)
		{
			base.transform.position = startPosition;
			base.transform.rotation = startRotation;
			base.transform.localScale = Vector3.one * scale;
			this.rb.velocity = velocity;
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x001AD028 File Offset: 0x001AB228
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

		// Token: 0x06004DEC RID: 19948 RVA: 0x00063071 File Offset: 0x00061271
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

		// Token: 0x06004DED RID: 19949 RVA: 0x001AD0A4 File Offset: 0x001AB2A4
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

		// Token: 0x04004F51 RID: 20305
		[SerializeField]
		private GameObject explosionEffect;

		// Token: 0x04004F52 RID: 20306
		[SerializeField]
		private float forceBackToPoolAfterSec = 20f;

		// Token: 0x04004F53 RID: 20307
		[SerializeField]
		private float explosionTime = 5f;

		// Token: 0x04004F54 RID: 20308
		[SerializeField]
		private GameObject disableWhenHit;

		// Token: 0x04004F55 RID: 20309
		[SerializeField]
		private float sizzleDuration;

		// Token: 0x04004F56 RID: 20310
		[SerializeField]
		private AudioClip sizzleAudioClip;

		// Token: 0x04004F57 RID: 20311
		public Action<FirecrackerProjectile> OnHitComplete;

		// Token: 0x04004F58 RID: 20312
		public Action<FirecrackerProjectile, Vector3> OnHitStart;

		// Token: 0x04004F59 RID: 20313
		private Rigidbody rb;

		// Token: 0x04004F5A RID: 20314
		private float timeCreated = float.PositiveInfinity;

		// Token: 0x04004F5B RID: 20315
		private float timeExploded = float.PositiveInfinity;

		// Token: 0x04004F5C RID: 20316
		private AudioSource audioSource;

		// Token: 0x04004F5D RID: 20317
		private TickSystemTimer m_timer = new TickSystemTimer(40f);

		// Token: 0x04004F5E RID: 20318
		private bool collisionEntered;
	}
}
