using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BEF RID: 3055
	[Serializable]
	public struct CosmeticHoldableSlotAttachInfo
	{
		// Token: 0x04004E8E RID: 20110
		[Tooltip("The anchor that this holdable cosmetic can attach to.")]
		public GTSturdyEnum<GTHardCodedBones.EHandAndStowSlots> stowSlot;

		// Token: 0x04004E8F RID: 20111
		public XformOffset offset;
	}
}
