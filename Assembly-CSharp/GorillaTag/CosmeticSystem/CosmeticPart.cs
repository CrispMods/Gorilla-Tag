using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BF1 RID: 3057
	[Serializable]
	public struct CosmeticPart
	{
		// Token: 0x04004EAA RID: 20138
		public GTAssetRef<GameObject> prefabAssetRef;

		// Token: 0x04004EAB RID: 20139
		[Tooltip("Determines how the cosmetic part will be attached to the player.")]
		public CosmeticAttachInfo[] attachAnchors;

		// Token: 0x04004EAC RID: 20140
		[NonSerialized]
		public ECosmeticPartType partType;
	}
}
