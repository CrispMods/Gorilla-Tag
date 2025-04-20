using System;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B6E RID: 2926
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerSwimmingParameters", order = 1)]
	public class PlayerSwimmingParameters : ScriptableObject
	{
		// Token: 0x04004B42 RID: 19266
		[Header("Base Settings")]
		public float floatingWaterLevelBelowHead = 0.6f;

		// Token: 0x04004B43 RID: 19267
		public float buoyancyFadeDist = 0.3f;

		// Token: 0x04004B44 RID: 19268
		public bool extendBouyancyFromSpeed;

		// Token: 0x04004B45 RID: 19269
		public float buoyancyExtensionDecayHalflife = 0.2f;

		// Token: 0x04004B46 RID: 19270
		public float baseUnderWaterDampingHalfLife = 0.25f;

		// Token: 0x04004B47 RID: 19271
		public float swimUnderWaterDampingHalfLife = 1.1f;

		// Token: 0x04004B48 RID: 19272
		public AnimationCurve speedToBouyancyExtension = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004B49 RID: 19273
		public Vector2 speedToBouyancyExtensionMinMax = Vector2.zero;

		// Token: 0x04004B4A RID: 19274
		public float swimmingVelocityOutOfWaterDrainRate = 3f;

		// Token: 0x04004B4B RID: 19275
		[Range(0f, 1f)]
		public float underwaterJumpsAsSwimVelocityFactor = 1f;

		// Token: 0x04004B4C RID: 19276
		[Range(0f, 1f)]
		public float swimmingHapticsStrength = 0.5f;

		// Token: 0x04004B4D RID: 19277
		[Header("Surface Jumping")]
		public bool allowWaterSurfaceJumps;

		// Token: 0x04004B4E RID: 19278
		public float waterSurfaceJumpHandSpeedThreshold = 1f;

		// Token: 0x04004B4F RID: 19279
		public float waterSurfaceJumpAmount;

		// Token: 0x04004B50 RID: 19280
		public float waterSurfaceJumpMaxSpeed = 1f;

		// Token: 0x04004B51 RID: 19281
		public AnimationCurve waterSurfaceJumpPalmFacingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004B52 RID: 19282
		public AnimationCurve waterSurfaceJumpHandVelocityFacingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004B53 RID: 19283
		[Header("Diving")]
		public bool applyDiveSteering;

		// Token: 0x04004B54 RID: 19284
		public bool applyDiveDampingMultiplier;

		// Token: 0x04004B55 RID: 19285
		public float diveDampingMultiplier = 1f;

		// Token: 0x04004B56 RID: 19286
		[Tooltip("In degrees")]
		public float maxDiveSteerAnglePerStep = 1f;

		// Token: 0x04004B57 RID: 19287
		public float diveVelocityAveragingWindow = 0.1f;

		// Token: 0x04004B58 RID: 19288
		public bool applyDiveSwimVelocityConversion;

		// Token: 0x04004B59 RID: 19289
		[Tooltip("In meters per second")]
		public float diveSwimVelocityConversionRate = 3f;

		// Token: 0x04004B5A RID: 19290
		public float diveMaxSwimVelocityConversion = 3f;

		// Token: 0x04004B5B RID: 19291
		public bool reduceDiveSteeringBelowVelocityPlane;

		// Token: 0x04004B5C RID: 19292
		public float reduceDiveSteeringBelowPlaneFadeStartDist = 0.4f;

		// Token: 0x04004B5D RID: 19293
		public float reduceDiveSteeringBelowPlaneFadeEndDist = 0.55f;

		// Token: 0x04004B5E RID: 19294
		public AnimationCurve palmFacingToRedirectAmount = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004B5F RID: 19295
		public Vector2 palmFacingToRedirectAmountMinMax = Vector2.zero;

		// Token: 0x04004B60 RID: 19296
		public AnimationCurve swimSpeedToRedirectAmount = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004B61 RID: 19297
		public Vector2 swimSpeedToRedirectAmountMinMax = Vector2.zero;

		// Token: 0x04004B62 RID: 19298
		public AnimationCurve swimSpeedToMaxRedirectAngle = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004B63 RID: 19299
		public Vector2 swimSpeedToMaxRedirectAngleMinMax = Vector2.zero;

		// Token: 0x04004B64 RID: 19300
		public AnimationCurve handSpeedToRedirectAmount = AnimationCurve.Linear(0f, 1f, 1f, 0f);

		// Token: 0x04004B65 RID: 19301
		public Vector2 handSpeedToRedirectAmountMinMax = Vector2.zero;

		// Token: 0x04004B66 RID: 19302
		public AnimationCurve handAccelToRedirectAmount = AnimationCurve.Linear(0f, 1f, 1f, 0f);

		// Token: 0x04004B67 RID: 19303
		public Vector2 handAccelToRedirectAmountMinMax = Vector2.zero;

		// Token: 0x04004B68 RID: 19304
		public AnimationCurve nonDiveDampingHapticsAmount = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004B69 RID: 19305
		public Vector2 nonDiveDampingHapticsAmountMinMax = Vector2.zero;
	}
}
