using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000C1D RID: 3101
	[Serializable]
	public struct CosmeticHoldableSlotAttachInfo
	{
		// Token: 0x04004F84 RID: 20356
		[Tooltip("The anchor that this holdable cosmetic can attach to.")]
		public GTSturdyEnum<GTHardCodedBones.EHandAndStowSlots> stowSlot;

		// Token: 0x04004F85 RID: 20357
		public XformOffset offset;
	}
}
