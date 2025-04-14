using System;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020003FE RID: 1022
[Serializable]
public class SubGrabPoint
{
	// Token: 0x060018FA RID: 6394 RVA: 0x0007A5A5 File Offset: 0x000787A5
	public virtual Matrix4x4 GetTransformation_GripPointLocalToAdvOriginLocal(AdvancedItemState.PreData advancedItemState, SlotTransformOverride slotTransformOverride)
	{
		return this.gripPointLocalToAdvOriginLocal;
	}

	// Token: 0x060018FB RID: 6395 RVA: 0x0007A5AD File Offset: 0x000787AD
	public virtual Quaternion GetRotationRelativeToObjectAnchor(AdvancedItemState advancedItemState, SlotTransformOverride slotTransformOverride)
	{
		return this.gripRotation_ParentAnchorLocal;
	}

	// Token: 0x060018FC RID: 6396 RVA: 0x0007A5B5 File Offset: 0x000787B5
	public virtual Vector3 GetGrabPositionRelativeToGrabPointOrigin(AdvancedItemState advancedItemState, SlotTransformOverride slotTransformOverride)
	{
		return this.gripPoint_AdvOriginLocal;
	}

	// Token: 0x060018FD RID: 6397 RVA: 0x0007A5C0 File Offset: 0x000787C0
	public virtual void InitializePoints(Transform anchor, Transform grabPointAnchor, Transform advancedGrabPointOrigin)
	{
		if (this.gripPoint == null)
		{
			return;
		}
		this.gripPoint_AdvOriginLocal = advancedGrabPointOrigin.InverseTransformPoint(this.gripPoint.position);
		this.gripRotation_AdvOriginLocal = Quaternion.Inverse(advancedGrabPointOrigin.rotation) * this.gripPoint.rotation;
		this.advAnchor_ParentAnchorLocal = Quaternion.Inverse(anchor.rotation) * grabPointAnchor.rotation;
		this.gripRotation_ParentAnchorLocal = Quaternion.Inverse(anchor.rotation) * this.gripPoint.rotation;
		this.gripPointLocalToAdvOriginLocal = advancedGrabPointOrigin.worldToLocalMatrix * this.gripPoint.localToWorldMatrix;
	}

	// Token: 0x060018FE RID: 6398 RVA: 0x0007A66D File Offset: 0x0007886D
	public Vector3 GetPositionOnObject(Transform transferableObject, SlotTransformOverride slotTransformOverride)
	{
		return transferableObject.TransformPoint(this.gripPoint_AdvOriginLocal);
	}

	// Token: 0x060018FF RID: 6399 RVA: 0x0007A67C File Offset: 0x0007887C
	public virtual Matrix4x4 GetTransformFromPositionState(AdvancedItemState advancedItemState, SlotTransformOverride slotTransformOverride, Transform targetDockXf)
	{
		Quaternion q = advancedItemState.deltaRotation;
		if (!q.IsValid())
		{
			q = Quaternion.identity;
		}
		Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, q, Vector3.one);
		Matrix4x4 matrix4x2 = this.GetTransformation_GripPointLocalToAdvOriginLocal(advancedItemState.preData, slotTransformOverride) * matrix4x.inverse;
		Matrix4x4 rhs = slotTransformOverride.AdvAnchorLocalToAdvOriginLocal * matrix4x2.inverse;
		return slotTransformOverride.AdvOriginLocalToParentAnchorLocal * rhs;
	}

	// Token: 0x06001900 RID: 6400 RVA: 0x0007A6EC File Offset: 0x000788EC
	public AdvancedItemState GetAdvancedItemStateFromHand(Transform objectTransform, Transform handTransform, Transform targetDock, SlotTransformOverride slotTransformOverride)
	{
		AdvancedItemState.PreData preData = this.GetPreData(objectTransform, handTransform, targetDock, slotTransformOverride);
		Matrix4x4 matrix4x = targetDock.localToWorldMatrix * slotTransformOverride.AdvOriginLocalToParentAnchorLocal * slotTransformOverride.AdvAnchorLocalToAdvOriginLocal;
		Matrix4x4 rhs = objectTransform.localToWorldMatrix * this.GetTransformation_GripPointLocalToAdvOriginLocal(preData, slotTransformOverride);
		Quaternion quaternion = (matrix4x.inverse * rhs).rotation;
		Vector3 vector = quaternion * Vector3.up;
		Vector3 vector2 = quaternion * Vector3.right;
		Vector3 vector3 = quaternion * Vector3.forward;
		bool reverseGrip = false;
		Vector2 up = Vector2.up;
		float angle = 0f;
		switch (this.limitAxis)
		{
		case LimitAxis.NoMovement:
			quaternion = Quaternion.identity;
			break;
		case LimitAxis.YAxis:
			if (this.allowReverseGrip)
			{
				if (Vector3.Dot(vector, Vector3.up) < 0f)
				{
					Debug.Log("Using Reverse Grip");
					reverseGrip = true;
					vector = Vector3.down;
				}
				else
				{
					vector = Vector3.up;
				}
			}
			else
			{
				vector = Vector3.up;
			}
			vector2 = Vector3.Cross(vector, vector3);
			vector3 = Vector3.Cross(vector2, vector);
			up = new Vector2(vector3.z, vector3.x);
			quaternion = Quaternion.LookRotation(vector3, vector);
			break;
		case LimitAxis.XAxis:
			vector2 = Vector3.right;
			vector3 = Vector3.Cross(vector2, vector);
			vector = Vector3.Cross(vector3, vector2);
			break;
		case LimitAxis.ZAxis:
			vector3 = Vector3.forward;
			vector2 = Vector3.Cross(vector, vector3);
			vector = Vector3.Cross(vector3, vector2);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		return new AdvancedItemState
		{
			preData = preData,
			limitAxis = this.limitAxis,
			angle = angle,
			reverseGrip = reverseGrip,
			angleVectorWhereUpIsStandard = up,
			deltaRotation = quaternion
		};
	}

	// Token: 0x06001901 RID: 6401 RVA: 0x0007A8B0 File Offset: 0x00078AB0
	public virtual AdvancedItemState.PreData GetPreData(Transform objectTransform, Transform handTransform, Transform targetDock, SlotTransformOverride slotTransformOverride)
	{
		return new AdvancedItemState.PreData
		{
			pointType = AdvancedItemState.PointType.Standard
		};
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x0007A8C0 File Offset: 0x00078AC0
	public virtual float EvaluateScore(Transform objectTransform, Transform handTransform, Transform targetDock)
	{
		Vector3 b = objectTransform.InverseTransformPoint(handTransform.position);
		float num = Vector3.SqrMagnitude(this.gripPoint_AdvOriginLocal - b);
		float f;
		Vector3 vector;
		(Quaternion.Inverse(objectTransform.rotation * this.gripRotation_AdvOriginLocal) * targetDock.rotation * this.advAnchor_ParentAnchorLocal).ToAngleAxis(out f, out vector);
		return num + Mathf.Abs(f) * 0.0001f;
	}

	// Token: 0x04001BDF RID: 7135
	[FormerlySerializedAs("transform")]
	public Transform gripPoint;

	// Token: 0x04001BE0 RID: 7136
	public LimitAxis limitAxis;

	// Token: 0x04001BE1 RID: 7137
	public bool allowReverseGrip;

	// Token: 0x04001BE2 RID: 7138
	private Vector3 gripPoint_AdvOriginLocal;

	// Token: 0x04001BE3 RID: 7139
	private Vector3 gripPointOffset_AdvOriginLocal;

	// Token: 0x04001BE4 RID: 7140
	public Quaternion gripRotation_AdvOriginLocal;

	// Token: 0x04001BE5 RID: 7141
	public Quaternion advAnchor_ParentAnchorLocal;

	// Token: 0x04001BE6 RID: 7142
	public Quaternion gripRotation_ParentAnchorLocal;

	// Token: 0x04001BE7 RID: 7143
	public Matrix4x4 gripPointLocalToAdvOriginLocal;
}
