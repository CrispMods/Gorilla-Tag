using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000C1F RID: 3103
	[Serializable]
	public struct CosmeticPart
	{
		// Token: 0x04004FA0 RID: 20384
		public GTAssetRef<GameObject> prefabAssetRef;

		// Token: 0x04004FA1 RID: 20385
		[Tooltip("Determines how the cosmetic part will be attached to the player.")]
		public CosmeticAttachInfo[] attachAnchors;

		// Token: 0x04004FA2 RID: 20386
		[NonSerialized]
		public ECosmeticPartType partType;
	}
}
