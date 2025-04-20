using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000C21 RID: 3105
	[Serializable]
	public struct CosmeticPlacementInfo
	{
		// Token: 0x04004FA5 RID: 20389
		[Tooltip("The bone to attach the cosmetic to.")]
		public GTHardCodedBones.SturdyEBone parentBone;

		// Token: 0x04004FA6 RID: 20390
		public XformOffset offset;
	}
}
