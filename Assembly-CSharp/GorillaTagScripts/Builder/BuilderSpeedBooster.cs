using System;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A39 RID: 2617
	public class BuilderSpeedBooster : MonoBehaviour
	{
		// Token: 0x060041B7 RID: 16823 RVA: 0x0005AF7B File Offset: 0x0005917B
		private void Awake()
		{
			this.volume = base.GetComponent<Collider>();
			this.windRenderer.enabled = false;
			this.boosting = false;
		}

		// Token: 0x060041B8 RID: 16824 RVA: 0x00172B64 File Offset: 0x00170D64
		private void LateUpdate()
		{
			if (this.audioSource && this.audioSource != null && !this.audioSource.isPlaying && this.audioSource.enabled)
			{
				this.audioSource.enabled = false;
			}
		}

		// Token: 0x060041B9 RID: 16825 RVA: 0x0012CDBC File Offset: 0x0012AFBC
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

		// Token: 0x060041BA RID: 16826 RVA: 0x00172BB4 File Offset: 0x00170DB4
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

		// Token: 0x060041BB RID: 16827 RVA: 0x00172C98 File Offset: 0x00170E98
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

		// Token: 0x060041BC RID: 16828 RVA: 0x00172D28 File Offset: 0x00170F28
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

		// Token: 0x060041BD RID: 16829 RVA: 0x00173134 File Offset: 0x00171334
		public void OnDrawGizmosSelected()
		{
			base.GetComponents<Collider>();
			Gizmos.color = Color.magenta;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(this.pullTOCenterMinDistance / base.transform.lossyScale.x, 1f, this.pullTOCenterMinDistance / base.transform.lossyScale.z));
		}

		// Token: 0x04004285 RID: 17029
		[SerializeField]
		public bool scaleWithSize = true;

		// Token: 0x04004286 RID: 17030
		[SerializeField]
		private float accel;

		// Token: 0x04004287 RID: 17031
		[SerializeField]
		private float maxDepth = -1f;

		// Token: 0x04004288 RID: 17032
		[SerializeField]
		private float maxSpeed;

		// Token: 0x04004289 RID: 17033
		[SerializeField]
		private bool disableGrip;

		// Token: 0x0400428A RID: 17034
		[SerializeField]
		private bool dampenLateralVelocity = true;

		// Token: 0x0400428B RID: 17035
		[SerializeField]
		private float dampenXVelPerc;

		// Token: 0x0400428C RID: 17036
		[SerializeField]
		private float dampenZVelPerc;

		// Token: 0x0400428D RID: 17037
		[SerializeField]
		private bool applyPullToCenterAcceleration = true;

		// Token: 0x0400428E RID: 17038
		[SerializeField]
		private float pullToCenterAccel;

		// Token: 0x0400428F RID: 17039
		[SerializeField]
		private float pullToCenterMaxSpeed;

		// Token: 0x04004290 RID: 17040
		[SerializeField]
		private float pullTOCenterMinDistance = 0.1f;

		// Token: 0x04004291 RID: 17041
		[SerializeField]
		private float addedWorldUpVelocity = 10f;

		// Token: 0x04004292 RID: 17042
		[SerializeField]
		private float maxBoostDuration = 2f;

		// Token: 0x04004293 RID: 17043
		private bool boosting;

		// Token: 0x04004294 RID: 17044
		private double enterTime;

		// Token: 0x04004295 RID: 17045
		private Collider volume;

		// Token: 0x04004296 RID: 17046
		public AudioClip exitClip;

		// Token: 0x04004297 RID: 17047
		public AudioSource audioSource;

		// Token: 0x04004298 RID: 17048
		public MeshRenderer windRenderer;

		// Token: 0x04004299 RID: 17049
		private Vector3 enterPos;

		// Token: 0x0400429A RID: 17050
		private bool positiveForce = true;
	}
}
