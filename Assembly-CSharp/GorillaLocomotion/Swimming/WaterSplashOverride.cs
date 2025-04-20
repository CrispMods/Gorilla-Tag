using System;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B77 RID: 2935
	public class WaterSplashOverride : MonoBehaviour
	{
		// Token: 0x04004BD2 RID: 19410
		public bool suppressWaterEffects;

		// Token: 0x04004BD3 RID: 19411
		public bool playBigSplash;

		// Token: 0x04004BD4 RID: 19412
		public bool playDrippingEffect = true;

		// Token: 0x04004BD5 RID: 19413
		public bool scaleByPlayersScale;

		// Token: 0x04004BD6 RID: 19414
		public bool overrideBoundingRadius;

		// Token: 0x04004BD7 RID: 19415
		public float boundingRadiusOverride = 1f;
	}
}
