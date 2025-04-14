using System;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A11 RID: 2577
	public class RecyclerForceVolume : MonoBehaviour
	{
		// Token: 0x0600408B RID: 16523 RVA: 0x001330AE File Offset: 0x001312AE
		private void Awake()
		{
			this.volume = base.GetComponent<Collider>();
			this.hasWindFX = (this.windEffectRenderer != null);
			if (this.hasWindFX)
			{
				this.windEffectRenderer.enabled = false;
			}
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x001330E4 File Offset: 0x001312E4
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

		// Token: 0x0600408D RID: 16525 RVA: 0x00133144 File Offset: 0x00131344
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

		// Token: 0x0600408E RID: 16526 RVA: 0x001331F0 File Offset: 0x001313F0
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

		// Token: 0x0600408F RID: 16527 RVA: 0x00133224 File Offset: 0x00131424
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

		// Token: 0x040041BA RID: 16826
		[SerializeField]
		public bool scaleWithSize = true;

		// Token: 0x040041BB RID: 16827
		[SerializeField]
		private float accel;

		// Token: 0x040041BC RID: 16828
		[SerializeField]
		private float maxDepth = -1f;

		// Token: 0x040041BD RID: 16829
		[SerializeField]
		private float maxSpeed;

		// Token: 0x040041BE RID: 16830
		[SerializeField]
		private bool disableGrip;

		// Token: 0x040041BF RID: 16831
		[SerializeField]
		private bool dampenLateralVelocity = true;

		// Token: 0x040041C0 RID: 16832
		[SerializeField]
		private float dampenXVelPerc;

		// Token: 0x040041C1 RID: 16833
		[FormerlySerializedAs("dampenZVelPerc")]
		[SerializeField]
		private float dampenYVelPerc;

		// Token: 0x040041C2 RID: 16834
		[SerializeField]
		private bool applyPullToCenterAcceleration = true;

		// Token: 0x040041C3 RID: 16835
		[SerializeField]
		private float pullToCenterAccel;

		// Token: 0x040041C4 RID: 16836
		[SerializeField]
		private float pullToCenterMaxSpeed;

		// Token: 0x040041C5 RID: 16837
		[SerializeField]
		private float pullTOCenterMinDistance = 0.1f;

		// Token: 0x040041C6 RID: 16838
		private Collider volume;

		// Token: 0x040041C7 RID: 16839
		public GameObject windSFX;

		// Token: 0x040041C8 RID: 16840
		[SerializeField]
		private MeshRenderer windEffectRenderer;

		// Token: 0x040041C9 RID: 16841
		private bool hasWindFX;

		// Token: 0x040041CA RID: 16842
		private Vector3 enterPos;
	}
}
