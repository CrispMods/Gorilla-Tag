using System;
using System.Runtime.CompilerServices;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200040A RID: 1034
[Serializable]
public class SubLineGrabPoint : SubGrabPoint
{
	// Token: 0x06001951 RID: 6481 RVA: 0x000D07EC File Offset: 0x000CE9EC
	public override Matrix4x4 GetTransformation_GripPointLocalToAdvOriginLocal(AdvancedItemState.PreData advancedItemState, SlotTransformOverride slotTransformOverride)
	{
		float distAlongLine = advancedItemState.distAlongLine;
		Vector3 pos = Vector3.Lerp(this.startPointRelativeTransformToGrabPointOrigin.Position(), this.endPointRelativeTransformToGrabPointOrigin.Position(), distAlongLine);
		Quaternion q = Quaternion.Slerp(this.startPointRelativeTransformToGrabPointOrigin.rotation, this.endPointRelativeTransformToGrabPointOrigin.rotation, distAlongLine);
		return Matrix4x4.TRS(pos, q, Vector3.one);
	}

	// Token: 0x06001952 RID: 6482 RVA: 0x000D0844 File Offset: 0x000CEA44
	public override void InitializePoints(Transform anchor, Transform grabPointAnchor, Transform advancedGrabPointOrigin)
	{
		base.InitializePoints(anchor, grabPointAnchor, advancedGrabPointOrigin);
		if (this.startPoint == null || this.endPoint == null)
		{
			return;
		}
		this.startPointRelativeToGrabPointOrigin = advancedGrabPointOrigin.InverseTransformPoint(this.startPoint.position);
		this.endPointRelativeToGrabPointOrigin = advancedGrabPointOrigin.InverseTransformPoint(this.endPoint.position);
		this.endPointRelativeTransformToGrabPointOrigin = advancedGrabPointOrigin.worldToLocalMatrix * this.endPoint.localToWorldMatrix;
		this.startPointRelativeTransformToGrabPointOrigin = advancedGrabPointOrigin.worldToLocalMatrix * this.startPoint.localToWorldMatrix;
	}

	// Token: 0x06001953 RID: 6483 RVA: 0x00041178 File Offset: 0x0003F378
	public override AdvancedItemState.PreData GetPreData(Transform objectTransform, Transform handTransform, Transform targetDock, SlotTransformOverride slotTransformOverride)
	{
		return new AdvancedItemState.PreData
		{
			distAlongLine = SubLineGrabPoint.<GetPreData>g__FindNearestFractionOnLine|8_0(objectTransform.TransformPoint(this.startPointRelativeToGrabPointOrigin), objectTransform.TransformPoint(this.endPointRelativeToGrabPointOrigin), handTransform.position),
			pointType = AdvancedItemState.PointType.DistanceBased
		};
	}

	// Token: 0x06001954 RID: 6484 RVA: 0x000D08E0 File Offset: 0x000CEAE0
	public override float EvaluateScore(Transform objectTransform, Transform handTransform, Transform targetDock)
	{
		float t = SubLineGrabPoint.<EvaluateScore>g__FindNearestFractionOnLine|9_0(objectTransform.TransformPoint(this.startPointRelativeToGrabPointOrigin), objectTransform.TransformPoint(this.endPointRelativeToGrabPointOrigin), handTransform.position);
		Vector3 a = Vector3.Lerp(this.startPointRelativeTransformToGrabPointOrigin.Position(), this.endPointRelativeTransformToGrabPointOrigin.Position(), t);
		Vector3 b = objectTransform.InverseTransformPoint(handTransform.position);
		return Vector3.SqrMagnitude(a - b);
	}

	// Token: 0x06001956 RID: 6486 RVA: 0x000D0948 File Offset: 0x000CEB48
	[CompilerGenerated]
	internal static float <GetPreData>g__FindNearestFractionOnLine|8_0(Vector3 origin, Vector3 end, Vector3 point)
	{
		Vector3 vector = end - origin;
		float magnitude = vector.magnitude;
		vector /= magnitude;
		return Mathf.Clamp01(Vector3.Dot(point - origin, vector) / magnitude);
	}

	// Token: 0x06001957 RID: 6487 RVA: 0x000D0948 File Offset: 0x000CEB48
	[CompilerGenerated]
	internal static float <EvaluateScore>g__FindNearestFractionOnLine|9_0(Vector3 origin, Vector3 end, Vector3 point)
	{
		Vector3 vector = end - origin;
		float magnitude = vector.magnitude;
		vector /= magnitude;
		return Mathf.Clamp01(Vector3.Dot(point - origin, vector) / magnitude);
	}

	// Token: 0x04001C31 RID: 7217
	public Transform startPoint;

	// Token: 0x04001C32 RID: 7218
	public Transform endPoint;

	// Token: 0x04001C33 RID: 7219
	public Vector3 startPointRelativeToGrabPointOrigin;

	// Token: 0x04001C34 RID: 7220
	public Vector3 endPointRelativeToGrabPointOrigin;

	// Token: 0x04001C35 RID: 7221
	public Matrix4x4 startPointRelativeTransformToGrabPointOrigin;

	// Token: 0x04001C36 RID: 7222
	public Matrix4x4 endPointRelativeTransformToGrabPointOrigin;
}
