using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000C1C RID: 3100
	[Serializable]
	public struct CosmeticAttachInfo
	{
		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06004E02 RID: 19970 RVA: 0x001AD818 File Offset: 0x001ABA18
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

		// Token: 0x06004E03 RID: 19971 RVA: 0x000630EE File Offset: 0x000612EE
		public CosmeticAttachInfo(ECosmeticSelectSide selectSide, GTHardCodedBones.EBone parentBone, XformOffset offset)
		{
			this.selectSide = selectSide;
			this.parentBone = parentBone;
			this.offset = offset;
		}

		// Token: 0x04004F81 RID: 20353
		[Tooltip("(Not used for holdables) Determines if the cosmetic part be shown depending on the hand that is used to press the in-game wardrobe \"EQUIP\" button.\n- Both: Show no matter what hand is used.\n- Left: Only show if the left hand selected.\n- Right: Only show if the right hand selected.\n")]
		public StringEnum<ECosmeticSelectSide> selectSide;

		// Token: 0x04004F82 RID: 20354
		public GTHardCodedBones.SturdyEBone parentBone;

		// Token: 0x04004F83 RID: 20355
		public XformOffset offset;
	}
}
