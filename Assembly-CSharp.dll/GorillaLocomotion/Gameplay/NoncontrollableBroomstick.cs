using System;
using Photon.Pun;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B5D RID: 2909
	public class NoncontrollableBroomstick : MonoBehaviour, IGorillaGrabable
	{
		// Token: 0x060048B2 RID: 18610 RVA: 0x00194AD0 File Offset: 0x00192CD0
		private void Start()
		{
			this.smoothRotationTrackingRateExp = Mathf.Exp(this.smoothRotationTrackingRate);
			this.progressPerFixedUpdate = Time.fixedDeltaTime / this.duration;
			this.progress = this.SplineProgressOffet;
			this.secondsToCycles = 1.0 / (double)this.duration;
			if (this.unitySpline != null)
			{
				this.nativeSpline = new NativeSpline(this.unitySpline.Spline, this.unitySpline.transform.localToWorldMatrix, Allocator.Persistent);
			}
		}

		// Token: 0x060048B3 RID: 18611 RVA: 0x00194B60 File Offset: 0x00192D60
		protected virtual void FixedUpdate()
		{
			if (PhotonNetwork.InRoom)
			{
				double num = PhotonNetwork.Time * this.secondsToCycles + (double)this.SplineProgressOffet;
				this.progress = (float)(num % 1.0);
			}
			else
			{
				this.progress = (this.progress + this.progressPerFixedUpdate) % 1f;
			}
			Quaternion a = Quaternion.identity;
			if (this.unitySpline != null)
			{
				float3 v;
				float3 @float;
				float3 float2;
				this.nativeSpline.Evaluate(this.progress, out v, out @float, out float2);
				base.transform.position = v;
				if (this.lookForward)
				{
					a = Quaternion.LookRotation(new Vector3(@float.x, @float.y, @float.z));
				}
			}
			else if (this.spline != null)
			{
				Vector3 point = this.spline.GetPoint(this.progress, this.constantVelocity);
				base.transform.position = point;
				if (this.lookForward)
				{
					a = Quaternion.LookRotation(this.spline.GetDirection(this.progress, this.constantVelocity));
				}
			}
			if (this.lookForward)
			{
				base.transform.rotation = Quaternion.Slerp(a, base.transform.rotation, Mathf.Exp(-this.smoothRotationTrackingRateExp * Time.deltaTime));
			}
		}

		// Token: 0x060048B4 RID: 18612 RVA: 0x00038586 File Offset: 0x00036786
		bool IGorillaGrabable.CanBeGrabbed(GorillaGrabber grabber)
		{
			return true;
		}

		// Token: 0x060048B5 RID: 18613 RVA: 0x0005E7B7 File Offset: 0x0005C9B7
		void IGorillaGrabable.OnGrabbed(GorillaGrabber g, out Transform grabbedObject, out Vector3 grabbedLocalPosition)
		{
			grabbedObject = base.transform;
			grabbedLocalPosition = base.transform.InverseTransformPoint(g.transform.position);
		}

		// Token: 0x060048B6 RID: 18614 RVA: 0x0002F75F File Offset: 0x0002D95F
		void IGorillaGrabable.OnGrabReleased(GorillaGrabber g)
		{
		}

		// Token: 0x060048B7 RID: 18615 RVA: 0x0005E7DD File Offset: 0x0005C9DD
		private void OnDestroy()
		{
			this.nativeSpline.Dispose();
		}

		// Token: 0x060048B8 RID: 18616 RVA: 0x0005E7EA File Offset: 0x0005C9EA
		public bool MomentaryGrabOnly()
		{
			return this.momentaryGrabOnly;
		}

		// Token: 0x060048BA RID: 18618 RVA: 0x000314EB File Offset: 0x0002F6EB
		string IGorillaGrabable.get_name()
		{
			return base.name;
		}

		// Token: 0x04004B5D RID: 19293
		public SplineContainer unitySpline;

		// Token: 0x04004B5E RID: 19294
		public BezierSpline spline;

		// Token: 0x04004B5F RID: 19295
		public float duration = 30f;

		// Token: 0x04004B60 RID: 19296
		public float smoothRotationTrackingRate = 0.5f;

		// Token: 0x04004B61 RID: 19297
		public bool lookForward = true;

		// Token: 0x04004B62 RID: 19298
		[SerializeField]
		private float SplineProgressOffet;

		// Token: 0x04004B63 RID: 19299
		private float progress;

		// Token: 0x04004B64 RID: 19300
		private float smoothRotationTrackingRateExp;

		// Token: 0x04004B65 RID: 19301
		[SerializeField]
		private bool constantVelocity;

		// Token: 0x04004B66 RID: 19302
		private float progressPerFixedUpdate;

		// Token: 0x04004B67 RID: 19303
		private double secondsToCycles;

		// Token: 0x04004B68 RID: 19304
		private NativeSpline nativeSpline;

		// Token: 0x04004B69 RID: 19305
		[SerializeField]
		private bool momentaryGrabOnly = true;
	}
}
