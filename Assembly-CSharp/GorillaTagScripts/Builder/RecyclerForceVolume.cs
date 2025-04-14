using System;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A0E RID: 2574
	public class RecyclerForceVolume : MonoBehaviour
	{
		// Token: 0x0600407F RID: 16511 RVA: 0x00132AE6 File Offset: 0x00130CE6
		private void Awake()
		{
			this.volume = base.GetComponent<Collider>();
			this.hasWindFX = (this.windEffectRenderer != null);
			if (this.hasWindFX)
			{
				this.windEffectRenderer.enabled = false;
			}
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x00132B1C File Offset: 0x00130D1C
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

		// Token: 0x06004081 RID: 16513 RVA: 0x00132B7C File Offset: 0x00130D7C
		public void OnTriggerEnter(Collider other)
		{
			Rigidbody rigidbody = null;
			Transform transform = null;
			if (!this.TriggerFilter(other, out rigidbody, out transform))
			{
				return;
			}
			this.enterPos = transform.position;
			ObjectPools.instance.Instantiate(this.windSFX, this.enterPos);
			if (this.hasWindFX)
			{
				this.windEffectRenderer.transform.position = base.transform.position + Vector3.Dot(this.enterPos - base.transform.position, base.transform.right) * base.transform.right;
				this.windEffectRenderer.enabled = true;
			}
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x00132C28 File Offset: 0x00130E28
		public void OnTriggerExit(Collider other)
		{
			Rigidbody rigidbody = null;
			Transform transform = null;
			if (!this.TriggerFilter(other, out rigidbody, out transform))
			{
				return;
			}
			if (this.hasWindFX)
			{
				this.windEffectRenderer.enabled = false;
			}
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x00132C5C File Offset: 0x00130E5C
		public void OnTriggerStay(Collider other)
		{
			Rigidbody rigidbody = null;
			Transform transform = null;
			if (!this.TriggerFilter(other, out rigidbody, out transform))
			{
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
			Vector3 a = Vector3.Dot(base.transform.position - transform.position, base.transform.up) * base.transform.up;
			float num = a.magnitude + 0.0001f;
			Vector3 vector2 = a / num;
			float num2 = Vector3.Dot(vector, vector2);
			float d = this.accel;
			if (this.maxDepth > -1f)
			{
				float num3 = Vector3.Dot(transform.position - this.enterPos, vector2);
				float num4 = this.maxDepth - num3;
				float b = 0f;
				if (num4 > 0.0001f)
				{
					b = num2 * num2 / num4;
				}
				d = Mathf.Max(this.accel, b);
			}
			float deltaTime = Time.deltaTime;
			Vector3 b2 = base.transform.forward * d * deltaTime;
			vector += b2;
			Vector3 a2 = Vector3.Dot(vector, base.transform.up) * base.transform.up;
			Vector3 a3 = Vector3.Dot(vector, base.transform.right) * base.transform.right;
			Vector3 b3 = Mathf.Clamp(Vector3.Dot(vector, base.transform.forward), -1f * this.maxSpeed, this.maxSpeed) * base.transform.forward;
			float d2 = 1f;
			float d3 = 1f;
			if (this.dampenLateralVelocity)
			{
				d2 = 1f - this.dampenXVelPerc * 0.01f * deltaTime;
				d3 = 1f - this.dampenYVelPerc * 0.01f * deltaTime;
			}
			vector = d3 * a2 + d2 * a3 + b3;
			if (this.applyPullToCenterAcceleration && this.pullToCenterAccel > 0f && this.pullToCenterMaxSpeed > 0f)
			{
				vector -= num2 * vector2;
				if (num > this.pullTOCenterMinDistance)
				{
					num2 += this.pullToCenterAccel * deltaTime;
					float num5 = Mathf.Min(this.pullToCenterMaxSpeed, num / deltaTime);
					num2 = Mathf.Clamp(num2, -1f * num5, num5);
				}
				else
				{
					num2 = 0f;
				}
				vector += num2 * vector2;
			}
			if (this.scaleWithSize && sizeManager)
			{
				vector *= sizeManager.currentScale;
			}
			rigidbody.velocity = vector;
		}

		// Token: 0x040041A8 RID: 16808
		[SerializeField]
		public bool scaleWithSize = true;

		// Token: 0x040041A9 RID: 16809
		[SerializeField]
		private float accel;

		// Token: 0x040041AA RID: 16810
		[SerializeField]
		private float maxDepth = -1f;

		// Token: 0x040041AB RID: 16811
		[SerializeField]
		private float maxSpeed;

		// Token: 0x040041AC RID: 16812
		[SerializeField]
		private bool disableGrip;

		// Token: 0x040041AD RID: 16813
		[SerializeField]
		private bool dampenLateralVelocity = true;

		// Token: 0x040041AE RID: 16814
		[SerializeField]
		private float dampenXVelPerc;

		// Token: 0x040041AF RID: 16815
		[FormerlySerializedAs("dampenZVelPerc")]
		[SerializeField]
		private float dampenYVelPerc;

		// Token: 0x040041B0 RID: 16816
		[SerializeField]
		private bool applyPullToCenterAcceleration = true;

		// Token: 0x040041B1 RID: 16817
		[SerializeField]
		private float pullToCenterAccel;

		// Token: 0x040041B2 RID: 16818
		[SerializeField]
		private float pullToCenterMaxSpeed;

		// Token: 0x040041B3 RID: 16819
		[SerializeField]
		private float pullTOCenterMinDistance = 0.1f;

		// Token: 0x040041B4 RID: 16820
		private Collider volume;

		// Token: 0x040041B5 RID: 16821
		public GameObject windSFX;

		// Token: 0x040041B6 RID: 16822
		[SerializeField]
		private MeshRenderer windEffectRenderer;

		// Token: 0x040041B7 RID: 16823
		private bool hasWindFX;

		// Token: 0x040041B8 RID: 16824
		private Vector3 enterPos;
	}
}
