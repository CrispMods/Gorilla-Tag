using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x02000401 RID: 1025
[Serializable]
public class SlotTransformOverride
{
	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06001910 RID: 6416 RVA: 0x0007AC53 File Offset: 0x00078E53
	// (set) Token: 0x06001911 RID: 6417 RVA: 0x0007AC60 File Offset: 0x00078E60
	private XformOffset _EdXformOffsetRepresenationOf_overrideTransformMatrix
	{
		get
		{
			return new XformOffset(this.overrideTransformMatrix);
		}
		set
		{
			this.overrideTransformMatrix = Matrix4x4.TRS(value.pos, value.rot, value.scale);
		}
	}

	// Token: 0x06001912 RID: 6418 RVA: 0x0007AC80 File Offset: 0x00078E80
	public void Initialize(Component component, Transform anchor)
	{
		if (!this.useAdvancedGrab)
		{
			return;
		}
		this.AdvOriginLocalToParentAnchorLocal = anchor.worldToLocalMatrix * this.advancedGrabPointOrigin.localToWorldMatrix;
		this.AdvAnchorLocalToAdvOriginLocal = this.advancedGrabPointOrigin.worldToLocalMatrix * this.advancedGrabPointAnchor.localToWorldMatrix;
		foreach (SubGrabPoint subGrabPoint in this.multiPoints)
		{
			if (subGrabPoint == null)
			{
				break;
			}
			subGrabPoint.InitializePoints(anchor, this.advancedGrabPointAnchor, this.advancedGrabPointOrigin);
		}
	}

	// Token: 0x06001913 RID: 6419 RVA: 0x0007AD2C File Offset: 0x00078F2C
	public void AddLineButton()
	{
		this.multiPoints.Add(new SubLineGrabPoint());
	}

	// Token: 0x06001914 RID: 6420 RVA: 0x0007AD40 File Offset: 0x00078F40
	public void AddSubGrabPoint(TransferrableObjectGripPosition togp)
	{
		SubGrabPoint item = togp.CreateSubGrabPoint(this);
		this.multiPoints.Add(item);
	}

	// Token: 0x04001BF1 RID: 7153
	[Obsolete("(2024-08-20 MattO) Cosmetics use xformOffsets now which fills in the appropriate data for this component. If you are doing something weird then `overrideTransformMatrix` must be used instead. This will probably be removed after 2024-09-15.")]
	public Transform overrideTransform;

	// Token: 0x04001BF2 RID: 7154
	[Obsolete("(2024-08-20 MattO) Cosmetics use xformOffsets now which fills in the appropriate data for this component. If you are doing something weird then `overrideTransformMatrix` must be used instead. This will probably be removed after 2024-09-15.")]
	[Delayed]
	public string overrideTransform_path;

	// Token: 0x04001BF3 RID: 7155
	public TransferrableObject.PositionState positionState;

	// Token: 0x04001BF4 RID: 7156
	public bool useAdvancedGrab;

	// Token: 0x04001BF5 RID: 7157
	public Matrix4x4 overrideTransformMatrix = Matrix4x4.identity;

	// Token: 0x04001BF6 RID: 7158
	public Transform advancedGrabPointAnchor;

	// Token: 0x04001BF7 RID: 7159
	public Transform advancedGrabPointOrigin;

	// Token: 0x04001BF8 RID: 7160
	[SerializeReference]
	public List<SubGrabPoint> multiPoints = new List<SubGrabPoint>();

	// Token: 0x04001BF9 RID: 7161
	public Matrix4x4 AdvOriginLocalToParentAnchorLocal;

	// Token: 0x04001BFA RID: 7162
	public Matrix4x4 AdvAnchorLocalToAdvOriginLocal;
}
