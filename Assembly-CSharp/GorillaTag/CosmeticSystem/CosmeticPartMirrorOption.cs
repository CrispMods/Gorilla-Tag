using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000C20 RID: 3104
	[Serializable]
	public struct CosmeticPartMirrorOption
	{
		// Token: 0x04004FA3 RID: 20387
		public ECosmeticPartMirrorAxis axis;

		// Token: 0x04004FA4 RID: 20388
		[Tooltip("This will multiply the local scale for the selected axis by -1.")]
		public bool negativeScale;
	}
}
