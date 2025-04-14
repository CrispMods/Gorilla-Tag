using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BF6 RID: 3062
	[Serializable]
	public struct CosmeticPlacementInfo
	{
		// Token: 0x04004EC1 RID: 20161
		[Tooltip("The bone to attach the cosmetic to.")]
		public GTHardCodedBones.SturdyEBone parentBone;

		// Token: 0x04004EC2 RID: 20162
		public XformOffset offset;
	}
}
