using System;
using System.Runtime.CompilerServices;
using GorillaExtensions;
using UnityEngine;

// Token: 0x020003FF RID: 1023
[Serializable]
public class SubLineGrabPoint : SubGrabPoint
{
	// Token: 0x06001904 RID: 6404 RVA: 0x0007A930 File Offset: 0x00078B30
	public override Matrix4x4 GetTransformation_GripPointLocalToAdvOriginLocal(AdvancedItemState.PreData advancedItemState, SlotTransformOverride slotTransformOverride)
	{
		float distAlongLine = advancedItemState.distAlongLine;
		Vector3 pos = Vector3.Lerp(this.startPointRelativeTransformToGrabPointOrigin.Position(), this.endPointRelativeTransformToGrabPointOrigin.Position(), distAlongLine);
		Quaternion q = Quaternion.Slerp(this.startPointRelativeTransformToGrabPointOrigin.rotation, this.endPointRelativeTransformToGrabPointOrigin.rotation, distAlongLine);
		return Matrix4x4.TRS(pos, q, Vector3.one);
	}

	// Token: 0x06001905 RID: 6405 RVA: 0x0007A988 File Offset: 0x00078B88
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

	// Token: 0x06001906 RID: 6406 RVA: 0x0007AA21 File Offset: 0x00078C21
	public override AdvancedItemState.PreData GetPreData(Transform objectTransform, Transform handTransform, Transform targetDock, SlotTransformOverride slotTransformOverride)
	{
		return new AdvancedItemState.PreData
		{
			distAlongLine = SubLineGrabPoint.<GetPreData>g__FindNearestFractionOnLine|8_0(objectTransform.TransformPoint(this.startPointRelativeToGrabPointOrigin), objectTransform.TransformPoint(this.endPointRelativeToGrabPointOrigin), handTransform.position),
			pointType = AdvancedItemState.PointType.DistanceBased
		};
	}

	// Token: 0x06001907 RID: 6407 RVA: 0x0007AA58 File Offset: 0x00078C58
	public override float EvaluateScore(Transform objectTransform, Transform handTransform, Transform targetDock)
	{
		float t = SubLineGrabPoint.<EvaluateScore>g__FindNearestFractionOnLine|9_0(objectTransform.TransformPoint(this.startPointRelativeToGrabPointOrigin), objectTransform.TransformPoint(this.endPointRelativeToGrabPointOrigin), handTransform.position);
		Vector3 a = Vector3.Lerp(this.startPointRelativeTransformToGrabPointOrigin.Position(), this.endPointRelativeTransformToGrabPointOrigin.Position(), t);
		Vector3 b = objectTransform.InverseTransformPoint(handTransform.position);
		return Vector3.SqrMagnitude(a - b);
	}

	// Token: 0x06001909 RID: 6409 RVA: 0x0007AAC8 File Offset: 0x00078CC8
	[CompilerGenerated]
	internal static float <GetPreData>g__FindNearestFractionOnLine|8_0(Vector3 origin, Vector3 end, Vector3 point)
	{
		Vector3 vector = end - origin;
		float magnitude = vector.magnitude;
		vector /= magnitude;
		return Mathf.Clamp01(Vector3.Dot(point - origin, vector) / magnitude);
	}

	// Token: 0x0600190A RID: 6410 RVA: 0x0007AB04 File Offset: 0x00078D04
	[CompilerGenerated]
	internal static float <EvaluateScore>g__FindNearestFractionOnLine|9_0(Vector3 origin, Vector3 end, Vector3 point)
	{
		Vector3 vector = end - origin;
		float magnitude = vector.magnitude;
		vector /= magnitude;
		return Mathf.Clamp01(Vector3.Dot(point - origin, vector) / magnitude);
	}

	// Token: 0x04001BE8 RID: 7144
	public Transform startPoint;

	// Token: 0x04001BE9 RID: 7145
	public Transform endPoint;

	// Token: 0x04001BEA RID: 7146
	public Vector3 startPointRelativeToGrabPointOrigin;

	// Token: 0x04001BEB RID: 7147
	public Vector3 endPointRelativeToGrabPointOrigin;

	// Token: 0x04001BEC RID: 7148
	public Matrix4x4 startPointRelativeTransformToGrabPointOrigin;

	// Token: 0x04001BED RID: 7149
	public Matrix4x4 endPointRelativeTransformToGrabPointOrigin;
}
