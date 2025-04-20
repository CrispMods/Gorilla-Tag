using System;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B76 RID: 2934
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WaterParameters", order = 1)]
	public class WaterParameters : ScriptableObject
	{
		// Token: 0x04004BBC RID: 19388
		[Header("Splash Effect")]
		public bool playSplashEffect = true;

		// Token: 0x04004BBD RID: 19389
		public GameObject splashEffect;

		// Token: 0x04004BBE RID: 19390
		public float splashEffectScale = 1f;

		// Token: 0x04004BBF RID: 19391
		public bool sendSplashEffectRPCs;

		// Token: 0x04004BC0 RID: 19392
		public float splashSpeedRequirement = 0.8f;

		// Token: 0x04004BC1 RID: 19393
		public float bigSplashSpeedRequirement = 1.9f;

		// Token: 0x04004BC2 RID: 19394
		public Gradient splashColorBySpeedGradient;

		// Token: 0x04004BC3 RID: 19395
		[Header("Ripple Effect")]
		public bool playRippleEffect = true;

		// Token: 0x04004BC4 RID: 19396
		public GameObject rippleEffect;

		// Token: 0x04004BC5 RID: 19397
		public float rippleEffectScale = 1f;

		// Token: 0x04004BC6 RID: 19398
		public float defaultDistanceBetweenRipples = 0.75f;

		// Token: 0x04004BC7 RID: 19399
		public float minDistanceBetweenRipples = 0.2f;

		// Token: 0x04004BC8 RID: 19400
		public float minTimeBetweenRipples = 0.75f;

		// Token: 0x04004BC9 RID: 19401
		public Color rippleSpriteColor = Color.white;

		// Token: 0x04004BCA RID: 19402
		[Header("Drip Effect")]
		public bool playDripEffect = true;

		// Token: 0x04004BCB RID: 19403
		public float postExitDripDuration = 1.5f;

		// Token: 0x04004BCC RID: 19404
		public float perDripTimeDelay = 0.2f;

		// Token: 0x04004BCD RID: 19405
		public float perDripTimeRandRange = 0.15f;

		// Token: 0x04004BCE RID: 19406
		public float perDripDefaultRadius = 0.01f;

		// Token: 0x04004BCF RID: 19407
		public float perDripRadiusRandRange = 0.01f;

		// Token: 0x04004BD0 RID: 19408
		[Header("Misc")]
		public float recomputeSurfaceForColliderDist = 0.2f;

		// Token: 0x04004BD1 RID: 19409
		public bool allowBubblesInVolume;
	}
}
