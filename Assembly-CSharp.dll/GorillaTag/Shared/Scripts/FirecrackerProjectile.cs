using System;
using System.Collections;
using GorillaTag.Cosmetics;
using UnityEngine;

namespace GorillaTag.Shared.Scripts
{
	// Token: 0x02000BE7 RID: 3047
	public class FirecrackerProjectile : MonoBehaviour, ITickSystemTick, IProjectile
	{
		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06004CA3 RID: 19619 RVA: 0x000615AB File Offset: 0x0005F7AB
		// (set) Token: 0x06004CA4 RID: 19620 RVA: 0x000615B3 File Offset: 0x0005F7B3
		public bool TickRunning { get; set; }

		// Token: 0x06004CA5 RID: 19621 RVA: 0x000615BC File Offset: 0x0005F7BC
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

		// Token: 0x06004CA6 RID: 19622 RVA: 0x001A6008 File Offset: 0x001A4208
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

		// Token: 0x06004CA7 RID: 19623 RVA: 0x000615F7 File Offset: 0x0005F7F7
		private void OnDisable()
		{
			TickSystem<object>.RemoveCallbackTarget(this);
			this.m_timer.Stop();
		}

		// Token: 0x06004CA8 RID: 19624 RVA: 0x0006160A File Offset: 0x0005F80A
		private void Awake()
		{
			this.rb = base.GetComponent<Rigidbody>();
			this.audioSource = base.GetComponent<AudioSource>();
			this.m_timer.callback = new Action(this.Detonate);
		}

		// Token: 0x06004CA9 RID: 19625 RVA: 0x0006163B File Offset: 0x0005F83B
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

		// Token: 0x06004CAA RID: 19626 RVA: 0x00061673 File Offset: 0x0005F873
		public void Launch(Vector3 startPosition, Quaternion startRotation, Vector3 velocity, float scale)
		{
			base.transform.position = startPosition;
			base.transform.rotation = startRotation;
			base.transform.localScale = Vector3.one * scale;
			this.rb.velocity = velocity;
		}

		// Token: 0x06004CAB RID: 19627 RVA: 0x001A605C File Offset: 0x001A425C
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

		// Token: 0x06004CAC RID: 19628 RVA: 0x000616B0 File Offset: 0x0005F8B0
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

		// Token: 0x06004CAD RID: 19629 RVA: 0x001A60D8 File Offset: 0x001A42D8
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

		// Token: 0x04004E6D RID: 20077
		[SerializeField]
		private GameObject explosionEffect;

		// Token: 0x04004E6E RID: 20078
		[SerializeField]
		private float forceBackToPoolAfterSec = 20f;

		// Token: 0x04004E6F RID: 20079
		[SerializeField]
		private float explosionTime = 5f;

		// Token: 0x04004E70 RID: 20080
		[SerializeField]
		private GameObject disableWhenHit;

		// Token: 0x04004E71 RID: 20081
		[SerializeField]
		private float sizzleDuration;

		// Token: 0x04004E72 RID: 20082
		[SerializeField]
		private AudioClip sizzleAudioClip;

		// Token: 0x04004E73 RID: 20083
		public Action<FirecrackerProjectile> OnHitComplete;

		// Token: 0x04004E74 RID: 20084
		public Action<FirecrackerProjectile, Vector3> OnHitStart;

		// Token: 0x04004E75 RID: 20085
		private Rigidbody rb;

		// Token: 0x04004E76 RID: 20086
		private float timeCreated = float.PositiveInfinity;

		// Token: 0x04004E77 RID: 20087
		private float timeExploded = float.PositiveInfinity;

		// Token: 0x04004E78 RID: 20088
		private AudioSource audioSource;

		// Token: 0x04004E79 RID: 20089
		private TickSystemTimer m_timer = new TickSystemTimer(40f);

		// Token: 0x04004E7A RID: 20090
		private bool collisionEntered;
	}
}
