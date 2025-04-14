using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BF2 RID: 3058
	[Serializable]
	public struct CosmeticPartMirrorOption
	{
		// Token: 0x04004EAD RID: 20141
		public ECosmeticPartMirrorAxis axis;

		// Token: 0x04004EAE RID: 20142
		[Tooltip("This will multiply the local scale for the selected axis by -1.")]
		public bool negativeScale;
	}
}
