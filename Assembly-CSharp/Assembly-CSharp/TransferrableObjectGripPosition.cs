using System;
using UnityEngine;

// Token: 0x0200040A RID: 1034
public class TransferrableObjectGripPosition : MonoBehaviour
{
	// Token: 0x06001996 RID: 6550 RVA: 0x0007E787 File Offset: 0x0007C987
	private void Awake()
	{
		if (this.parentObject == null)
		{
			this.parentObject = base.transform.parent.GetComponent<TransferrableItemSlotTransformOverride>();
		}
		this.parentObject.AddGripPosition(this.attachmentType, this);
	}

	// Token: 0x06001997 RID: 6551 RVA: 0x0007E7BF File Offset: 0x0007C9BF
	public SubGrabPoint CreateSubGrabPoint(SlotTransformOverride overrideContainer)
	{
		return new SubGrabPoint();
	}

	// Token: 0x04001C81 RID: 7297
	[SerializeField]
	private TransferrableItemSlotTransformOverride parentObject;

	// Token: 0x04001C82 RID: 7298
	[SerializeField]
	private TransferrableObject.PositionState attachmentType;
}
