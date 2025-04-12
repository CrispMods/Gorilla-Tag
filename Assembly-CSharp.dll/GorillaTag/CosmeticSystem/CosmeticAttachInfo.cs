using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BF1 RID: 3057
	[Serializable]
	public struct CosmeticAttachInfo
	{
		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06004CC2 RID: 19650 RVA: 0x001A684C File Offset: 0x001A4A4C
		public static CosmeticAttachInfo Identity
		{
			get
			{
				return new CosmeticAttachInfo
				{
					selectSide = ECosmeticSelectSide.Both,
					parentBone = GTHardCodedBones.EBone.None,
					offset = XformOffset.Identity
				};
			}
		}

		// Token: 0x06004CC3 RID: 19651 RVA: 0x0006172D File Offset: 0x0005F92D
		public CosmeticAttachInfo(ECosmeticSelectSide selectSide, GTHardCodedBones.EBone parentBone, XformOffset offset)
		{
			this.selectSide = selectSide;
			this.parentBone = parentBone;
			this.offset = offset;
		}

		// Token: 0x04004E9D RID: 20125
		[Tooltip("(Not used for holdables) Determines if the cosmetic part be shown depending on the hand that is used to press the in-game wardrobe \"EQUIP\" button.\n- Both: Show no matter what hand is used.\n- Left: Only show if the left hand selected.\n- Right: Only show if the right hand selected.\n")]
		public StringEnum<ECosmeticSelectSide> selectSide;

		// Token: 0x04004E9E RID: 20126
		public GTHardCodedBones.SturdyEBone parentBone;

		// Token: 0x04004E9F RID: 20127
		public XformOffset offset;
	}
}
