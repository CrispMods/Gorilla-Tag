using System;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A0F RID: 2575
	public class BuilderSpeedBooster : MonoBehaviour
	{
		// Token: 0x0600407E RID: 16510 RVA: 0x0013279D File Offset: 0x0013099D
		private void Awake()
		{
			this.volume = base.GetComponent<Collider>();
			this.windRenderer.enabled = false;
			this.boosting = false;
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x001327C0 File Offset: 0x001309C0
		private void LateUpdate()
		{
			if (this.audioSource && this.audioSource != null && !this.audioSource.isPlaying && this.audioSource.enabled)
			{
				this.audioSource.enabled = false;
			}
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x00132810 File Offset: 0x00130A10
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

		// Token: 0x06004081 RID: 16513 RVA: 0x00132870 File Offset: 0x00130A70
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

		// Token: 0x06004082 RID: 16514 RVA: 0x00132954 File Offset: 0x00130B54
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

		// Token: 0x06004083 RID: 16515 RVA: 0x001329E4 File Offset: 0x00130BE4
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

		// Token: 0x06004084 RID: 16516 RVA: 0x00132DF0 File Offset: 0x00130FF0
		public void OnDrawGizmosSelected()
		{
			base.GetComponents<Collider>();
			Gizmos.color = Color.magenta;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(this.pullTOCenterMinDistance / base.transform.lossyScale.x, 1f, this.pullTOCenterMinDistance / base.transform.lossyScale.z));
		}

		// Token: 0x0400419D RID: 16797
		[SerializeField]
		public bool scaleWithSize = true;

		// Token: 0x0400419E RID: 16798
		[SerializeField]
		private float accel;

		// Token: 0x0400419F RID: 16799
		[SerializeField]
		private float maxDepth = -1f;

		// Token: 0x040041A0 RID: 16800
		[SerializeField]
		private float maxSpeed;

		// Token: 0x040041A1 RID: 16801
		[SerializeField]
		private bool disableGrip;

		// Token: 0x040041A2 RID: 16802
		[SerializeField]
		private bool dampenLateralVelocity = true;

		// Token: 0x040041A3 RID: 16803
		[SerializeField]
		private float dampenXVelPerc;

		// Token: 0x040041A4 RID: 16804
		[SerializeField]
		private float dampenZVelPerc;

		// Token: 0x040041A5 RID: 16805
		[SerializeField]
		private bool applyPullToCenterAcceleration = true;

		// Token: 0x040041A6 RID: 16806
		[SerializeField]
		private float pullToCenterAccel;

		// Token: 0x040041A7 RID: 16807
		[SerializeField]
		private float pullToCenterMaxSpeed;

		// Token: 0x040041A8 RID: 16808
		[SerializeField]
		private float pullTOCenterMinDistance = 0.1f;

		// Token: 0x040041A9 RID: 16809
		[SerializeField]
		private float addedWorldUpVelocity = 10f;

		// Token: 0x040041AA RID: 16810
		[SerializeField]
		private float maxBoostDuration = 2f;

		// Token: 0x040041AB RID: 16811
		private bool boosting;

		// Token: 0x040041AC RID: 16812
		private double enterTime;

		// Token: 0x040041AD RID: 16813
		private Collider volume;

		// Token: 0x040041AE RID: 16814
		public AudioClip exitClip;

		// Token: 0x040041AF RID: 16815
		public AudioSource audioSource;

		// Token: 0x040041B0 RID: 16816
		public MeshRenderer windRenderer;

		// Token: 0x040041B1 RID: 16817
		private Vector3 enterPos;

		// Token: 0x040041B2 RID: 16818
		private bool positiveForce = true;
	}
}
