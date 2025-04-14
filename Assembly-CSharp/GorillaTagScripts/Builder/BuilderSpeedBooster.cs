using System;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A0C RID: 2572
	public class BuilderSpeedBooster : MonoBehaviour
	{
		// Token: 0x06004072 RID: 16498 RVA: 0x001321D5 File Offset: 0x001303D5
		private void Awake()
		{
			this.volume = base.GetComponent<Collider>();
			this.windRenderer.enabled = false;
			this.boosting = false;
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x001321F8 File Offset: 0x001303F8
		private void LateUpdate()
		{
			if (this.audioSource && this.audioSource != null && !this.audioSource.isPlaying && this.audioSource.enabled)
			{
				this.audioSource.enabled = false;
			}
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x00132248 File Offset: 0x00130448
		private bool TriggerFilter(Collider other, out Rigidbody rb, out Transform xf)
		{
			rb = null;
			xf = null;
			if (other.gameObject == GorillaTagger.Instance.headCollider.gameObject)
			{
				rb = GorillaTagger.Instance.GetComponent<Rigidbody>();
				xf = GorillaTagger.Instance.headCollider.GetComponent<Transform>();
			}
			return rb != null && xf != null;
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x001322A8 File Offset: 0x001304A8
		public void OnTriggerEnter(Collider other)
		{
			Rigidbody rigidbody = null;
			Transform transform = null;
			if (!this.TriggerFilter(other, out rigidbody, out transform))
			{
				return;
			}
			if ((double)GorillaTagger.Instance.offlineVRRig.scaleFactor > 0.99)
			{
				return;
			}
			this.positiveForce = (Vector3.Dot(base.transform.up, rigidbody.velocity) > 0f);
			if (this.positiveForce)
			{
				this.windRenderer.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
			}
			else
			{
				this.windRenderer.transform.localRotation = Quaternion.Euler(0f, 180f, -90f);
			}
			this.windRenderer.enabled = true;
			this.enterPos = transform.position;
			if (!this.boosting)
			{
				this.boosting = true;
				this.enterTime = Time.timeAsDouble;
			}
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x0013238C File Offset: 0x0013058C
		public void OnTriggerExit(Collider other)
		{
			Rigidbody rigidbody = null;
			Transform transform = null;
			if (!this.TriggerFilter(other, out rigidbody, out transform))
			{
				return;
			}
			this.windRenderer.enabled = false;
			if ((double)GorillaTagger.Instance.offlineVRRig.scaleFactor > 0.99)
			{
				return;
			}
			if (this.boosting && this.audioSource)
			{
				this.audioSource.enabled = true;
				this.audioSource.Stop();
				this.audioSource.GTPlayOneShot(this.exitClip, 1f);
			}
			this.boosting = false;
		}

		// Token: 0x06004077 RID: 16503 RVA: 0x0013241C File Offset: 0x0013061C
		public void OnTriggerStay(Collider other)
		{
			if (!this.boosting)
			{
				return;
			}
			Rigidbody rigidbody = null;
			Transform transform = null;
			if (!this.TriggerFilter(other, out rigidbody, out transform))
			{
				return;
			}
			if ((double)GorillaTagger.Instance.offlineVRRig.scaleFactor > 0.99)
			{
				this.OnTriggerExit(other);
				return;
			}
			if (Time.timeAsDouble > this.enterTime + (double)this.maxBoostDuration)
			{
				this.OnTriggerExit(other);
				return;
			}
			if (this.disableGrip)
			{
				GTPlayer.Instance.SetMaximumSlipThisFrame();
			}
			SizeManager sizeManager = null;
			if (this.scaleWithSize)
			{
				sizeManager = rigidbody.GetComponent<SizeManager>();
			}
			Vector3 vector = rigidbody.velocity;
			if (this.scaleWithSize && sizeManager)
			{
				vector /= sizeManager.currentScale;
			}
			Vector3 b = Vector3.Dot(transform.position - base.transform.position, base.transform.up) * base.transform.up;
			Vector3 a = base.transform.position + b - transform.position;
			float num = a.magnitude + 0.0001f;
			Vector3 vector2 = a / num;
			float num2 = Vector3.Dot(vector, vector2);
			float d = this.accel;
			if (this.maxDepth > -1f)
			{
				float num3 = Vector3.Dot(transform.position - this.enterPos, vector2);
				float num4 = this.maxDepth - num3;
				float b2 = 0f;
				if (num4 > 0.0001f)
				{
					b2 = num2 * num2 / num4;
				}
				d = Mathf.Max(this.accel, b2);
			}
			float deltaTime = Time.deltaTime;
			Vector3 vector3 = base.transform.up * d * deltaTime;
			if (!this.positiveForce)
			{
				vector3 *= -1f;
			}
			vector += vector3;
			if ((double)Vector3.Dot(vector3, Vector3.down) <= 0.1)
			{
				vector += Vector3.up * this.addedWorldUpVelocity * deltaTime;
			}
			Vector3 a2 = Mathf.Min(Vector3.Dot(vector, base.transform.up), this.maxSpeed) * base.transform.up;
			Vector3 a3 = Vector3.Dot(vector, base.transform.right) * base.transform.right;
			Vector3 a4 = Vector3.Dot(vector, base.transform.forward) * base.transform.forward;
			float d2 = 1f;
			float d3 = 1f;
			if (this.dampenLateralVelocity)
			{
				d2 = 1f - this.dampenXVelPerc * 0.01f * deltaTime;
				d3 = 1f - this.dampenZVelPerc * 0.01f * deltaTime;
			}
			vector = a2 + d2 * a3 + d3 * a4;
			if (this.applyPullToCenterAcceleration && this.pullToCenterAccel > 0f && this.pullToCenterMaxSpeed > 0f)
			{
				vector -= num2 * vector2;
				if (num > this.pullTOCenterMinDistance)
				{
					num2 += this.pullToCenterAccel * deltaTime;
					float b3 = Mathf.Min(this.pullToCenterMaxSpeed, num / deltaTime);
					num2 = Mathf.Min(num2, b3);
				}
				else
				{
					num2 = 0f;
				}
				vector += num2 * vector2;
				if (vector.magnitude > 0.0001f)
				{
					Vector3 vector4 = Vector3.Cross(base.transform.up, vector2);
					float magnitude = vector4.magnitude;
					if (magnitude > 0.0001f)
					{
						vector4 /= magnitude;
						num2 = Vector3.Dot(vector, vector4);
						vector -= num2 * vector4;
						num2 -= this.pullToCenterAccel * deltaTime;
						num2 = Mathf.Max(0f, num2);
						vector += num2 * vector4;
					}
				}
			}
			if (this.scaleWithSize && sizeManager)
			{
				vector *= sizeManager.currentScale;
			}
			rigidbody.velocity = vector;
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x00132828 File Offset: 0x00130A28
		public void OnDrawGizmosSelected()
		{
			base.GetComponents<Collider>();
			Gizmos.color = Color.magenta;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(this.pullTOCenterMinDistance / base.transform.lossyScale.x, 1f, this.pullTOCenterMinDistance / base.transform.lossyScale.z));
		}

		// Token: 0x0400418B RID: 16779
		[SerializeField]
		public bool scaleWithSize = true;

		// Token: 0x0400418C RID: 16780
		[SerializeField]
		private float accel;

		// Token: 0x0400418D RID: 16781
		[SerializeField]
		private float maxDepth = -1f;

		// Token: 0x0400418E RID: 16782
		[SerializeField]
		private float maxSpeed;

		// Token: 0x0400418F RID: 16783
		[SerializeField]
		private bool disableGrip;

		// Token: 0x04004190 RID: 16784
		[SerializeField]
		private bool dampenLateralVelocity = true;

		// Token: 0x04004191 RID: 16785
		[SerializeField]
		private float dampenXVelPerc;

		// Token: 0x04004192 RID: 16786
		[SerializeField]
		private float dampenZVelPerc;

		// Token: 0x04004193 RID: 16787
		[SerializeField]
		private bool applyPullToCenterAcceleration = true;

		// Token: 0x04004194 RID: 16788
		[SerializeField]
		private float pullToCenterAccel;

		// Token: 0x04004195 RID: 16789
		[SerializeField]
		private float pullToCenterMaxSpeed;

		// Token: 0x04004196 RID: 16790
		[SerializeField]
		private float pullTOCenterMinDistance = 0.1f;

		// Token: 0x04004197 RID: 16791
		[SerializeField]
		private float addedWorldUpVelocity = 10f;

		// Token: 0x04004198 RID: 16792
		[SerializeField]
		private float maxBoostDuration = 2f;

		// Token: 0x04004199 RID: 16793
		private bool boosting;

		// Token: 0x0400419A RID: 16794
		private double enterTime;

		// Token: 0x0400419B RID: 16795
		private Collider volume;

		// Token: 0x0400419C RID: 16796
		public AudioClip exitClip;

		// Token: 0x0400419D RID: 16797
		public AudioSource audioSource;

		// Token: 0x0400419E RID: 16798
		public MeshRenderer windRenderer;

		// Token: 0x0400419F RID: 16799
		private Vector3 enterPos;

		// Token: 0x040041A0 RID: 16800
		private bool positiveForce = true;
	}
}
