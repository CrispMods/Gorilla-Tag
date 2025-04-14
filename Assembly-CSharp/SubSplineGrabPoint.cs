using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000400 RID: 1024
[Serializable]
public class SubSplineGrabPoint : SubLineGrabPoint
{
	// Token: 0x0600190B RID: 6411 RVA: 0x0007AB3D File Offset: 0x00078D3D
	public override Matrix4x4 GetTransformation_GripPointLocalToAdvOriginLocal(AdvancedItemState.PreData advancedItemState, SlotTransformOverride slotTransformOverride)
	{
		return CatmullRomSpline.Evaluate(this.controlPointsTransformsRelativeToGrabOrigin, advancedItemState.distAlongLine);
	}

	// Token: 0x0600190C RID: 6412 RVA: 0x0007AB50 File Offset: 0x00078D50
	public override void InitializePoints(Transform anchor, Transform grabPointAnchor, Transform advancedGrabPointOrigin)
	{
		base.InitializePoints(anchor, grabPointAnchor, advancedGrabPointOrigin);
		this.controlPointsRelativeToGrabOrigin = new List<Vector3>();
		foreach (Transform transform in this.spline.controlPointTransforms)
		{
			this.controlPointsRelativeToGrabOrigin.Add(advancedGrabPointOrigin.InverseTransformPoint(transform.position));
			this.controlPointsTransformsRelativeToGrabOrigin.Add(advancedGrabPointOrigin.worldToLocalMatrix * transform.localToWorldMatrix);
		}
	}

	// Token: 0x0600190D RID: 6413 RVA: 0x0007ABC4 File Offset: 0x00078DC4
	public override AdvancedItemState.PreData GetPreData(Transform objectTransform, Transform handTransform, Transform targetDock, SlotTransformOverride slotTransformOverride)
	{
		Vector3 worldPoint = objectTransform.InverseTransformPoint(handTransform.position);
		Vector3 vector;
		return new AdvancedItemState.PreData
		{
			distAlongLine = CatmullRomSpline.GetClosestEvaluationOnSpline(this.controlPointsRelativeToGrabOrigin, worldPoint, out vector),
			pointType = AdvancedItemState.PointType.DistanceBased
		};
	}

	// Token: 0x0600190E RID: 6414 RVA: 0x0007AC00 File Offset: 0x00078E00
	public override float EvaluateScore(Transform objectTransform, Transform handTransform, Transform targetDock)
	{
		Vector3 vector = objectTransform.InverseTransformPoint(handTransform.position);
		Vector3 a;
		CatmullRomSpline.GetClosestEvaluationOnSpline(this.controlPointsRelativeToGrabOrigin, vector, out a);
		return Vector3.SqrMagnitude(a - vector);
	}

	// Token: 0x04001BEE RID: 7150
	public CatmullRomSpline spline;

	// Token: 0x04001BEF RID: 7151
	public List<Vector3> controlPointsRelativeToGrabOrigin = new List<Vector3>();

	// Token: 0x04001BF0 RID: 7152
	public List<Matrix4x4> controlPointsTransformsRelativeToGrabOrigin = new List<Matrix4x4>();
}
