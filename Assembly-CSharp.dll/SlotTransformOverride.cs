using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x02000401 RID: 1025
[Serializable]
public class SlotTransformOverride
{
	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06001913 RID: 6419 RVA: 0x0003FEFE File Offset: 0x0003E0FE
	// (set) Token: 0x06001914 RID: 6420 RVA: 0x0003FF0B File Offset: 0x0003E10B
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

	// Token: 0x06001915 RID: 6421 RVA: 0x000CE244 File Offset: 0x000CC444
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

	// Token: 0x06001916 RID: 6422 RVA: 0x0003FF2B File Offset: 0x0003E12B
	public void AddLineButton()
	{
		this.multiPoints.Add(new SubLineGrabPoint());
	}

	// Token: 0x06001917 RID: 6423 RVA: 0x000CE2F0 File Offset: 0x000CC4F0
	public void AddSubGrabPoint(TransferrableObjectGripPosition togp)
	{
		SubGrabPoint item = togp.CreateSubGrabPoint(this);
		this.multiPoints.Add(item);
	}

	// Token: 0x04001BF2 RID: 7154
	[Obsolete("(2024-08-20 MattO) Cosmetics use xformOffsets now which fills in the appropriate data for this component. If you are doing something weird then `overrideTransformMatrix` must be used instead. This will probably be removed after 2024-09-15.")]
	public Transform overrideTransform;

	// Token: 0x04001BF3 RID: 7155
	[Obsolete("(2024-08-20 MattO) Cosmetics use xformOffsets now which fills in the appropriate data for this component. If you are doing something weird then `overrideTransformMatrix` must be used instead. This will probably be removed after 2024-09-15.")]
	[Delayed]
	public string overrideTransform_path;

	// Token: 0x04001BF4 RID: 7156
	public TransferrableObject.PositionState positionState;

	// Token: 0x04001BF5 RID: 7157
	public bool useAdvancedGrab;

	// Token: 0x04001BF6 RID: 7158
	public Matrix4x4 overrideTransformMatrix = Matrix4x4.identity;

	// Token: 0x04001BF7 RID: 7159
	public Transform advancedGrabPointAnchor;

	// Token: 0x04001BF8 RID: 7160
	public Transform advancedGrabPointOrigin;

	// Token: 0x04001BF9 RID: 7161
	[SerializeReference]
	public List<SubGrabPoint> multiPoints = new List<SubGrabPoint>();

	// Token: 0x04001BFA RID: 7162
	public Matrix4x4 AdvOriginLocalToParentAnchorLocal;

	// Token: 0x04001BFB RID: 7163
	public Matrix4x4 AdvAnchorLocalToAdvOriginLocal;
}
