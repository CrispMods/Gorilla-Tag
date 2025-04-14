using System;
using Photon.Pun;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B5A RID: 2906
	public class NoncontrollableBroomstick : MonoBehaviour, IGorillaGrabable
	{
		// Token: 0x060048A6 RID: 18598 RVA: 0x00160418 File Offset: 0x0015E618
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

		// Token: 0x060048A7 RID: 18599 RVA: 0x001604A8 File Offset: 0x0015E6A8
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

		// Token: 0x060048A8 RID: 18600 RVA: 0x000444E2 File Offset: 0x000426E2
		bool IGorillaGrabable.CanBeGrabbed(GorillaGrabber grabber)
		{
			return true;
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x001605F1 File Offset: 0x0015E7F1
		void IGorillaGrabable.OnGrabbed(GorillaGrabber g, out Transform grabbedObject, out Vector3 grabbedLocalPosition)
		{
			grabbedObject = base.transform;
			grabbedLocalPosition = base.transform.InverseTransformPoint(g.transform.position);
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x000023F4 File Offset: 0x000005F4
		void IGorillaGrabable.OnGrabReleased(GorillaGrabber g)
		{
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x00160617 File Offset: 0x0015E817
		private void OnDestroy()
		{
			this.nativeSpline.Dispose();
		}

		// Token: 0x060048AC RID: 18604 RVA: 0x00160624 File Offset: 0x0015E824
		public bool MomentaryGrabOnly()
		{
			return this.momentaryGrabOnly;
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x0001227B File Offset: 0x0001047B
		string IGorillaGrabable.get_name()
		{
			return base.name;
		}

		// Token: 0x04004B4B RID: 19275
		public SplineContainer unitySpline;

		// Token: 0x04004B4C RID: 19276
		public BezierSpline spline;

		// Token: 0x04004B4D RID: 19277
		public float duration = 30f;

		// Token: 0x04004B4E RID: 19278
		public float smoothRotationTrackingRate = 0.5f;

		// Token: 0x04004B4F RID: 19279
		public bool lookForward = true;

		// Token: 0x04004B50 RID: 19280
		[SerializeField]
		private float SplineProgressOffet;

		// Token: 0x04004B51 RID: 19281
		private float progress;

		// Token: 0x04004B52 RID: 19282
		private float smoothRotationTrackingRateExp;

		// Token: 0x04004B53 RID: 19283
		[SerializeField]
		private bool constantVelocity;

		// Token: 0x04004B54 RID: 19284
		private float progressPerFixedUpdate;

		// Token: 0x04004B55 RID: 19285
		private double secondsToCycles;

		// Token: 0x04004B56 RID: 19286
		private NativeSpline nativeSpline;

		// Token: 0x04004B57 RID: 19287
		[SerializeField]
		private bool momentaryGrabOnly = true;
	}
}
