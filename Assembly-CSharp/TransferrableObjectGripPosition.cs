using System;
using UnityEngine;

// Token: 0x0200040A RID: 1034
public class TransferrableObjectGripPosition : MonoBehaviour
{
	// Token: 0x06001993 RID: 6547 RVA: 0x0007E403 File Offset: 0x0007C603
	private void Awake()
	{
		if (this.parentObject == null)
		{
			this.parentObject = base.transform.parent.GetComponent<TransferrableItemSlotTransformOverride>();
		}
		this.parentObject.AddGripPosition(this.attachmentType, this);
	}

	// Token: 0x06001994 RID: 6548 RVA: 0x0007E43B File Offset: 0x0007C63B
	public SubGrabPoint CreateSubGrabPoint(SlotTransformOverride overrideContainer)
	{
		return new SubGrabPoint();
	}

	// Token: 0x04001C80 RID: 7296
	[SerializeField]
	private TransferrableItemSlotTransformOverride parentObject;

	// Token: 0x04001C81 RID: 7297
	[SerializeField]
	private TransferrableObject.PositionState attachmentType;
}
