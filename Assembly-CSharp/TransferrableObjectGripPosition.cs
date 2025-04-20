using System;
using UnityEngine;

// Token: 0x02000415 RID: 1045
public class TransferrableObjectGripPosition : MonoBehaviour
{
	// Token: 0x060019E0 RID: 6624 RVA: 0x00041646 File Offset: 0x0003F846
	private void Awake()
	{
		if (this.parentObject == null)
		{
			this.parentObject = base.transform.parent.GetComponent<TransferrableItemSlotTransformOverride>();
		}
		this.parentObject.AddGripPosition(this.attachmentType, this);
	}

	// Token: 0x060019E1 RID: 6625 RVA: 0x0004167E File Offset: 0x0003F87E
	public SubGrabPoint CreateSubGrabPoint(SlotTransformOverride overrideContainer)
	{
		return new SubGrabPoint();
	}

	// Token: 0x04001CC9 RID: 7369
	[SerializeField]
	private TransferrableItemSlotTransformOverride parentObject;

	// Token: 0x04001CCA RID: 7370
	[SerializeField]
	private TransferrableObject.PositionState attachmentType;
}
