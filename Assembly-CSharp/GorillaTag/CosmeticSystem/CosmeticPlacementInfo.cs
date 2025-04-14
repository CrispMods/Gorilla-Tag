using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BF3 RID: 3059
	[Serializable]
	public struct CosmeticPlacementInfo
	{
		// Token: 0x04004EAF RID: 20143
		[Tooltip("The bone to attach the cosmetic to.")]
		public GTHardCodedBones.SturdyEBone parentBone;

		// Token: 0x04004EB0 RID: 20144
		public XformOffset offset;
	}
}
