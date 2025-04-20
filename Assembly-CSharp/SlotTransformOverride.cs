using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x0200040C RID: 1036
[Serializable]
public class SlotTransformOverride
{
	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x0600195D RID: 6493 RVA: 0x000411E8 File Offset: 0x0003F3E8
	// (set) Token: 0x0600195E RID: 6494 RVA: 0x000411F5 File Offset: 0x0003F3F5
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

	// Token: 0x0600195F RID: 6495 RVA: 0x000D0A6C File Offset: 0x000CEC6C
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

	// Token: 0x06001960 RID: 6496 RVA: 0x00041215 File Offset: 0x0003F415
	public void AddLineButton()
	{
		this.multiPoints.Add(new SubLineGrabPoint());
	}

	// Token: 0x06001961 RID: 6497 RVA: 0x000D0B18 File Offset: 0x000CED18
	public void AddSubGrabPoint(TransferrableObjectGripPosition togp)
	{
		SubGrabPoint item = togp.CreateSubGrabPoint(this);
		this.multiPoints.Add(item);
	}

	// Token: 0x04001C3A RID: 7226
	[Obsolete("(2024-08-20 MattO) Cosmetics use xformOffsets now which fills in the appropriate data for this component. If you are doing something weird then `overrideTransformMatrix` must be used instead. This will probably be removed after 2024-09-15.")]
	public Transform overrideTransform;

	// Token: 0x04001C3B RID: 7227
	[Obsolete("(2024-08-20 MattO) Cosmetics use xformOffsets now which fills in the appropriate data for this component. If you are doing something weird then `overrideTransformMatrix` must be used instead. This will probably be removed after 2024-09-15.")]
	[Delayed]
	public string overrideTransform_path;

	// Token: 0x04001C3C RID: 7228
	public TransferrableObject.PositionState positionState;

	// Token: 0x04001C3D RID: 7229
	public bool useAdvancedGrab;

	// Token: 0x04001C3E RID: 7230
	public Matrix4x4 overrideTransformMatrix = Matrix4x4.identity;

	// Token: 0x04001C3F RID: 7231
	public Transform advancedGrabPointAnchor;

	// Token: 0x04001C40 RID: 7232
	public Transform advancedGrabPointOrigin;

	// Token: 0x04001C41 RID: 7233
	[SerializeReference]
	public List<SubGrabPoint> multiPoints = new List<SubGrabPoint>();

	// Token: 0x04001C42 RID: 7234
	public Matrix4x4 AdvOriginLocalToParentAnchorLocal;

	// Token: 0x04001C43 RID: 7235
	public Matrix4x4 AdvAnchorLocalToAdvOriginLocal;
}
