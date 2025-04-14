using System;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B4D RID: 2893
	public class WaterSplashOverride : MonoBehaviour
	{
		// Token: 0x04004AEE RID: 19182
		public bool suppressWaterEffects;

		// Token: 0x04004AEF RID: 19183
		public bool playBigSplash;

		// Token: 0x04004AF0 RID: 19184
		public bool playDrippingEffect = true;

		// Token: 0x04004AF1 RID: 19185
		public bool scaleByPlayersScale;

		// Token: 0x04004AF2 RID: 19186
		public bool overrideBoundingRadius;

		// Token: 0x04004AF3 RID: 19187
		public float boundingRadiusOverride = 1f;
	}
}
