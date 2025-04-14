using System;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B4A RID: 2890
	public class WaterSplashOverride : MonoBehaviour
	{
		// Token: 0x04004ADC RID: 19164
		public bool suppressWaterEffects;

		// Token: 0x04004ADD RID: 19165
		public bool playBigSplash;

		// Token: 0x04004ADE RID: 19166
		public bool playDrippingEffect = true;

		// Token: 0x04004ADF RID: 19167
		public bool scaleByPlayersScale;

		// Token: 0x04004AE0 RID: 19168
		public bool overrideBoundingRadius;

		// Token: 0x04004AE1 RID: 19169
		public float boundingRadiusOverride = 1f;
	}
}
