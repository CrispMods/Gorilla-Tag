using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BF4 RID: 3060
	[Serializable]
	public struct CosmeticPart
	{
		// Token: 0x04004EBC RID: 20156
		public GTAssetRef<GameObject> prefabAssetRef;

		// Token: 0x04004EBD RID: 20157
		[Tooltip("Determines how the cosmetic part will be attached to the player.")]
		public CosmeticAttachInfo[] attachAnchors;

		// Token: 0x04004EBE RID: 20158
		[NonSerialized]
		public ECosmeticPartType partType;
	}
}
