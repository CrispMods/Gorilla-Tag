using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000400 RID: 1024
[Serializable]
public class SubSplineGrabPoint : SubLineGrabPoint
{
	// Token: 0x0600190E RID: 6414 RVA: 0x0007AEC1 File Offset: 0x000790C1
	public override Matrix4x4 GetTransformation_GripPointLocalToAdvOriginLocal(AdvancedItemState.PreData advancedItemState, SlotTransformOverride slotTransformOverride)
	{
		return CatmullRomSpline.Evaluate(this.controlPointsTransformsRelativeToGrabOrigin, advancedItemState.distAlongLine);
	}

	// Token: 0x0600190F RID: 6415 RVA: 0x0007AED4 File Offset: 0x000790D4
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

	// Token: 0x06001910 RID: 6416 RVA: 0x0007AF48 File Offset: 0x00079148
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

	// Token: 0x06001911 RID: 6417 RVA: 0x0007AF84 File Offset: 0x00079184
	public override float EvaluateScore(Transform objectTransform, Transform handTransform, Transform targetDock)
	{
		Vector3 vector = objectTransform.InverseTransformPoint(handTransform.position);
		Vector3 a;
		CatmullRomSpline.GetClosestEvaluationOnSpline(this.controlPointsRelativeToGrabOrigin, vector, out a);
		return Vector3.SqrMagnitude(a - vector);
	}

	// Token: 0x04001BEF RID: 7151
	public CatmullRomSpline spline;

	// Token: 0x04001BF0 RID: 7152
	public List<Vector3> controlPointsRelativeToGrabOrigin = new List<Vector3>();

	// Token: 0x04001BF1 RID: 7153
	public List<Matrix4x4> controlPointsTransformsRelativeToGrabOrigin = new List<Matrix4x4>();
}
