﻿using System;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000409 RID: 1033
[Serializable]
public class SubGrabPoint
{
	// Token: 0x06001947 RID: 6471 RVA: 0x00041144 File Offset: 0x0003F344
	public virtual Matrix4x4 GetTransformation_GripPointLocalToAdvOriginLocal(AdvancedItemState.PreData advancedItemState, SlotTransformOverride slotTransformOverride)
	{
		return this.gripPointLocalToAdvOriginLocal;
	}

	// Token: 0x06001948 RID: 6472 RVA: 0x0004114C File Offset: 0x0003F34C
	public virtual Quaternion GetRotationRelativeToObjectAnchor(AdvancedItemState advancedItemState, SlotTransformOverride slotTransformOverride)
	{
		return this.gripRotation_ParentAnchorLocal;
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x00041154 File Offset: 0x0003F354
	public virtual Vector3 GetGrabPositionRelativeToGrabPointOrigin(AdvancedItemState advancedItemState, SlotTransformOverride slotTransformOverride)
	{
		return this.gripPoint_AdvOriginLocal;
	}

	// Token: 0x0600194A RID: 6474 RVA: 0x000D0498 File Offset: 0x000CE698
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

	// Token: 0x0600194B RID: 6475 RVA: 0x0004115C File Offset: 0x0003F35C
	public Vector3 GetPositionOnObject(Transform transferableObject, SlotTransformOverride slotTransformOverride)
	{
		return transferableObject.TransformPoint(this.gripPoint_AdvOriginLocal);
	}

	// Token: 0x0600194C RID: 6476 RVA: 0x000D0548 File Offset: 0x000CE748
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

	// Token: 0x0600194D RID: 6477 RVA: 0x000D05B8 File Offset: 0x000CE7B8
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

	// Token: 0x0600194E RID: 6478 RVA: 0x0004116A File Offset: 0x0003F36A
	public virtual AdvancedItemState.PreData GetPreData(Transform objectTransform, Transform handTransform, Transform targetDock, SlotTransformOverride slotTransformOverride)
	{
		return new AdvancedItemState.PreData
		{
			pointType = AdvancedItemState.PointType.Standard
		};
	}

	// Token: 0x0600194F RID: 6479 RVA: 0x000D077C File Offset: 0x000CE97C
	public virtual float EvaluateScore(Transform objectTransform, Transform handTransform, Transform targetDock)
	{
		Vector3 b = objectTransform.InverseTransformPoint(handTransform.position);
		float num = Vector3.SqrMagnitude(this.gripPoint_AdvOriginLocal - b);
		float f;
		Vector3 vector;
		(Quaternion.Inverse(objectTransform.rotation * this.gripRotation_AdvOriginLocal) * targetDock.rotation * this.advAnchor_ParentAnchorLocal).ToAngleAxis(out f, out vector);
		return num + Mathf.Abs(f) * 0.0001f;
	}

	// Token: 0x04001C28 RID: 7208
	[FormerlySerializedAs("transform")]
	public Transform gripPoint;

	// Token: 0x04001C29 RID: 7209
	public LimitAxis limitAxis;

	// Token: 0x04001C2A RID: 7210
	public bool allowReverseGrip;

	// Token: 0x04001C2B RID: 7211
	private Vector3 gripPoint_AdvOriginLocal;

	// Token: 0x04001C2C RID: 7212
	private Vector3 gripPointOffset_AdvOriginLocal;

	// Token: 0x04001C2D RID: 7213
	public Quaternion gripRotation_AdvOriginLocal;

	// Token: 0x04001C2E RID: 7214
	public Quaternion advAnchor_ParentAnchorLocal;

	// Token: 0x04001C2F RID: 7215
	public Quaternion gripRotation_ParentAnchorLocal;

	// Token: 0x04001C30 RID: 7216
	public Matrix4x4 gripPointLocalToAdvOriginLocal;
}
