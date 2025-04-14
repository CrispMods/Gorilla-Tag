using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BF5 RID: 3061
	[Serializable]
	public struct CosmeticPartMirrorOption
	{
		// Token: 0x04004EBF RID: 20159
		public ECosmeticPartMirrorAxis axis;

		// Token: 0x04004EC0 RID: 20160
		[Tooltip("This will multiply the local scale for the selected axis by -1.")]
		public bool negativeScale;
	}
}
