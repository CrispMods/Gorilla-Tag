using System;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B49 RID: 2889
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WaterParameters", order = 1)]
	public class WaterParameters : ScriptableObject
	{
		// Token: 0x04004AC6 RID: 19142
		[Header("Splash Effect")]
		public bool playSplashEffect = true;

		// Token: 0x04004AC7 RID: 19143
		public GameObject splashEffect;

		// Token: 0x04004AC8 RID: 19144
		public float splashEffectScale = 1f;

		// Token: 0x04004AC9 RID: 19145
		public bool sendSplashEffectRPCs;

		// Token: 0x04004ACA RID: 19146
		public float splashSpeedRequirement = 0.8f;

		// Token: 0x04004ACB RID: 19147
		public float bigSplashSpeedRequirement = 1.9f;

		// Token: 0x04004ACC RID: 19148
		public Gradient splashColorBySpeedGradient;

		// Token: 0x04004ACD RID: 19149
		[Header("Ripple Effect")]
		public bool playRippleEffect = true;

		// Token: 0x04004ACE RID: 19150
		public GameObject rippleEffect;

		// Token: 0x04004ACF RID: 19151
		public float rippleEffectScale = 1f;

		// Token: 0x04004AD0 RID: 19152
		public float defaultDistanceBetweenRipples = 0.75f;

		// Token: 0x04004AD1 RID: 19153
		public float minDistanceBetweenRipples = 0.2f;

		// Token: 0x04004AD2 RID: 19154
		public float minTimeBetweenRipples = 0.75f;

		// Token: 0x04004AD3 RID: 19155
		public Color rippleSpriteColor = Color.white;

		// Token: 0x04004AD4 RID: 19156
		[Header("Drip Effect")]
		public bool playDripEffect = true;

		// Token: 0x04004AD5 RID: 19157
		public float postExitDripDuration = 1.5f;

		// Token: 0x04004AD6 RID: 19158
		public float perDripTimeDelay = 0.2f;

		// Token: 0x04004AD7 RID: 19159
		public float perDripTimeRandRange = 0.15f;

		// Token: 0x04004AD8 RID: 19160
		public float perDripDefaultRadius = 0.01f;

		// Token: 0x04004AD9 RID: 19161
		public float perDripRadiusRandRange = 0.01f;

		// Token: 0x04004ADA RID: 19162
		[Header("Misc")]
		public float recomputeSurfaceForColliderDist = 0.2f;

		// Token: 0x04004ADB RID: 19163
		public bool allowBubblesInVolume;
	}
}
