using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BEE RID: 3054
	[Serializable]
	public struct CosmeticAttachInfo
	{
		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06004CB6 RID: 19638 RVA: 0x00175224 File Offset: 0x00173424
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

		// Token: 0x06004CB7 RID: 19639 RVA: 0x00175260 File Offset: 0x00173460
		public CosmeticAttachInfo(ECosmeticSelectSide selectSide, GTHardCodedBones.EBone parentBone, XformOffset offset)
		{
			this.selectSide = selectSide;
			this.parentBone = parentBone;
			this.offset = offset;
		}

		// Token: 0x04004E8B RID: 20107
		[Tooltip("(Not used for holdables) Determines if the cosmetic part be shown depending on the hand that is used to press the in-game wardrobe \"EQUIP\" button.\n- Both: Show no matter what hand is used.\n- Left: Only show if the left hand selected.\n- Right: Only show if the right hand selected.\n")]
		public StringEnum<ECosmeticSelectSide> selectSide;

		// Token: 0x04004E8C RID: 20108
		public GTHardCodedBones.SturdyEBone parentBone;

		// Token: 0x04004E8D RID: 20109
		public XformOffset offset;
	}
}
