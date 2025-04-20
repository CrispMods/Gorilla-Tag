using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200040B RID: 1035
[Serializable]
public class SubSplineGrabPoint : SubLineGrabPoint
{
	// Token: 0x06001958 RID: 6488 RVA: 0x000411B7 File Offset: 0x0003F3B7
	public override Matrix4x4 GetTransformation_GripPointLocalToAdvOriginLocal(AdvancedItemState.PreData advancedItemState, SlotTransformOverride slotTransformOverride)
	{
		return CatmullRomSpline.Evaluate(this.controlPointsTransformsRelativeToGrabOrigin, advancedItemState.distAlongLine);
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x000D0984 File Offset: 0x000CEB84
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

	// Token: 0x0600195A RID: 6490 RVA: 0x000D09F8 File Offset: 0x000CEBF8
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

	// Token: 0x0600195B RID: 6491 RVA: 0x000D0A34 File Offset: 0x000CEC34
	public override float EvaluateScore(Transform objectTransform, Transform handTransform, Transform targetDock)
	{
		Vector3 vector = objectTransform.InverseTransformPoint(handTransform.position);
		Vector3 a;
		CatmullRomSpline.GetClosestEvaluationOnSpline(this.controlPointsRelativeToGrabOrigin, vector, out a);
		return Vector3.SqrMagnitude(a - vector);
	}

	// Token: 0x04001C37 RID: 7223
	public CatmullRomSpline spline;

	// Token: 0x04001C38 RID: 7224
	public List<Vector3> controlPointsRelativeToGrabOrigin = new List<Vector3>();

	// Token: 0x04001C39 RID: 7225
	public List<Matrix4x4> controlPointsTransformsRelativeToGrabOrigin = new List<Matrix4x4>();
}
