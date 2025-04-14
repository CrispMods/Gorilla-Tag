using System;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B4C RID: 2892
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WaterParameters", order = 1)]
	public class WaterParameters : ScriptableObject
	{
		// Token: 0x04004AD8 RID: 19160
		[Header("Splash Effect")]
		public bool playSplashEffect = true;

		// Token: 0x04004AD9 RID: 19161
		public GameObject splashEffect;

		// Token: 0x04004ADA RID: 19162
		public float splashEffectScale = 1f;

		// Token: 0x04004ADB RID: 19163
		public bool sendSplashEffectRPCs;

		// Token: 0x04004ADC RID: 19164
		public float splashSpeedRequirement = 0.8f;

		// Token: 0x04004ADD RID: 19165
		public float bigSplashSpeedRequirement = 1.9f;

		// Token: 0x04004ADE RID: 19166
		public Gradient splashColorBySpeedGradient;

		// Token: 0x04004ADF RID: 19167
		[Header("Ripple Effect")]
		public bool playRippleEffect = true;

		// Token: 0x04004AE0 RID: 19168
		public GameObject rippleEffect;

		// Token: 0x04004AE1 RID: 19169
		public float rippleEffectScale = 1f;

		// Token: 0x04004AE2 RID: 19170
		public float defaultDistanceBetweenRipples = 0.75f;

		// Token: 0x04004AE3 RID: 19171
		public float minDistanceBetweenRipples = 0.2f;

		// Token: 0x04004AE4 RID: 19172
		public float minTimeBetweenRipples = 0.75f;

		// Token: 0x04004AE5 RID: 19173
		public Color rippleSpriteColor = Color.white;

		// Token: 0x04004AE6 RID: 19174
		[Header("Drip Effect")]
		public bool playDripEffect = true;

		// Token: 0x04004AE7 RID: 19175
		public float postExitDripDuration = 1.5f;

		// Token: 0x04004AE8 RID: 19176
		public float perDripTimeDelay = 0.2f;

		// Token: 0x04004AE9 RID: 19177
		public float perDripTimeRandRange = 0.15f;

		// Token: 0x04004AEA RID: 19178
		public float perDripDefaultRadius = 0.01f;

		// Token: 0x04004AEB RID: 19179
		public float perDripRadiusRandRange = 0.01f;

		// Token: 0x04004AEC RID: 19180
		[Header("Misc")]
		public float recomputeSurfaceForColliderDist = 0.2f;

		// Token: 0x04004AED RID: 19181
		public bool allowBubblesInVolume;
	}
}
