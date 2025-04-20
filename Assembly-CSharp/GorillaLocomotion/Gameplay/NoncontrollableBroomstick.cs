using System;
using Photon.Pun;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B87 RID: 2951
	public class NoncontrollableBroomstick : MonoBehaviour, IGorillaGrabable
	{
		// Token: 0x060049F1 RID: 18929 RVA: 0x0019BAE8 File Offset: 0x00199CE8
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

		// Token: 0x060049F2 RID: 18930 RVA: 0x0019BB78 File Offset: 0x00199D78
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

		// Token: 0x060049F3 RID: 18931 RVA: 0x00039846 File Offset: 0x00037A46
		bool IGorillaGrabable.CanBeGrabbed(GorillaGrabber grabber)
		{
			return true;
		}

		// Token: 0x060049F4 RID: 18932 RVA: 0x000601EF File Offset: 0x0005E3EF
		void IGorillaGrabable.OnGrabbed(GorillaGrabber g, out Transform grabbedObject, out Vector3 grabbedLocalPosition)
		{
			grabbedObject = base.transform;
			grabbedLocalPosition = base.transform.InverseTransformPoint(g.transform.position);
		}

		// Token: 0x060049F5 RID: 18933 RVA: 0x00030607 File Offset: 0x0002E807
		void IGorillaGrabable.OnGrabReleased(GorillaGrabber g)
		{
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x00060215 File Offset: 0x0005E415
		private void OnDestroy()
		{
			this.nativeSpline.Dispose();
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x00060222 File Offset: 0x0005E422
		public bool MomentaryGrabOnly()
		{
			return this.momentaryGrabOnly;
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x0003261E File Offset: 0x0003081E
		string IGorillaGrabable.get_name()
		{
			return base.name;
		}

		// Token: 0x04004C41 RID: 19521
		public SplineContainer unitySpline;

		// Token: 0x04004C42 RID: 19522
		public BezierSpline spline;

		// Token: 0x04004C43 RID: 19523
		public float duration = 30f;

		// Token: 0x04004C44 RID: 19524
		public float smoothRotationTrackingRate = 0.5f;

		// Token: 0x04004C45 RID: 19525
		public bool lookForward = true;

		// Token: 0x04004C46 RID: 19526
		[SerializeField]
		private float SplineProgressOffet;

		// Token: 0x04004C47 RID: 19527
		private float progress;

		// Token: 0x04004C48 RID: 19528
		private float smoothRotationTrackingRateExp;

		// Token: 0x04004C49 RID: 19529
		[SerializeField]
		private bool constantVelocity;

		// Token: 0x04004C4A RID: 19530
		private float progressPerFixedUpdate;

		// Token: 0x04004C4B RID: 19531
		private double secondsToCycles;

		// Token: 0x04004C4C RID: 19532
		private NativeSpline nativeSpline;

		// Token: 0x04004C4D RID: 19533
		[SerializeField]
		private bool momentaryGrabOnly = true;
	}
}
