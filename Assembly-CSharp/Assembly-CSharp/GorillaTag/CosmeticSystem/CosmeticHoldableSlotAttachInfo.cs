using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BF2 RID: 3058
	[Serializable]
	public struct CosmeticHoldableSlotAttachInfo
	{
		// Token: 0x04004EA0 RID: 20128
		[Tooltip("The anchor that this holdable cosmetic can attach to.")]
		public GTSturdyEnum<GTHardCodedBones.EHandAndStowSlots> stowSlot;

		// Token: 0x04004EA1 RID: 20129
		public XformOffset offset;
	}
}
