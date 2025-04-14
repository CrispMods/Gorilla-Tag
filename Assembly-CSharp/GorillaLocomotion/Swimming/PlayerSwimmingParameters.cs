using System;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B41 RID: 2881
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerSwimmingParameters", order = 1)]
	public class PlayerSwimmingParameters : ScriptableObject
	{
		// Token: 0x04004A4C RID: 19020
		[Header("Base Settings")]
		public float floatingWaterLevelBelowHead = 0.6f;

		// Token: 0x04004A4D RID: 19021
		public float buoyancyFadeDist = 0.3f;

		// Token: 0x04004A4E RID: 19022
		public bool extendBouyancyFromSpeed;

		// Token: 0x04004A4F RID: 19023
		public float buoyancyExtensionDecayHalflife = 0.2f;

		// Token: 0x04004A50 RID: 19024
		public float baseUnderWaterDampingHalfLife = 0.25f;

		// Token: 0x04004A51 RID: 19025
		public float swimUnderWaterDampingHalfLife = 1.1f;

		// Token: 0x04004A52 RID: 19026
		public AnimationCurve speedToBouyancyExtension = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004A53 RID: 19027
		public Vector2 speedToBouyancyExtensionMinMax = Vector2.zero;

		// Token: 0x04004A54 RID: 19028
		public float swimmingVelocityOutOfWaterDrainRate = 3f;

		// Token: 0x04004A55 RID: 19029
		[Range(0f, 1f)]
		public float underwaterJumpsAsSwimVelocityFactor = 1f;

		// Token: 0x04004A56 RID: 19030
		[Range(0f, 1f)]
		public float swimmingHapticsStrength = 0.5f;

		// Token: 0x04004A57 RID: 19031
		[Header("Surface Jumping")]
		public bool allowWaterSurfaceJumps;

		// Token: 0x04004A58 RID: 19032
		public float waterSurfaceJumpHandSpeedThreshold = 1f;

		// Token: 0x04004A59 RID: 19033
		public float waterSurfaceJumpAmount;

		// Token: 0x04004A5A RID: 19034
		public float waterSurfaceJumpMaxSpeed = 1f;

		// Token: 0x04004A5B RID: 19035
		public AnimationCurve waterSurfaceJumpPalmFacingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004A5C RID: 19036
		public AnimationCurve waterSurfaceJumpHandVelocityFacingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004A5D RID: 19037
		[Header("Diving")]
		public bool applyDiveSteering;

		// Token: 0x04004A5E RID: 19038
		public bool applyDiveDampingMultiplier;

		// Token: 0x04004A5F RID: 19039
		public float diveDampingMultiplier = 1f;

		// Token: 0x04004A60 RID: 19040
		[Tooltip("In degrees")]
		public float maxDiveSteerAnglePerStep = 1f;

		// Token: 0x04004A61 RID: 19041
		public float diveVelocityAveragingWindow = 0.1f;

		// Token: 0x04004A62 RID: 19042
		public bool applyDiveSwimVelocityConversion;

		// Token: 0x04004A63 RID: 19043
		[Tooltip("In meters per second")]
		public float diveSwimVelocityConversionRate = 3f;

		// Token: 0x04004A64 RID: 19044
		public float diveMaxSwimVelocityConversion = 3f;

		// Token: 0x04004A65 RID: 19045
		public bool reduceDiveSteeringBelowVelocityPlane;

		// Token: 0x04004A66 RID: 19046
		public float reduceDiveSteeringBelowPlaneFadeStartDist = 0.4f;

		// Token: 0x04004A67 RID: 19047
		public float reduceDiveSteeringBelowPlaneFadeEndDist = 0.55f;

		// Token: 0x04004A68 RID: 19048
		public AnimationCurve palmFacingToRedirectAmount = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004A69 RID: 19049
		public Vector2 palmFacingToRedirectAmountMinMax = Vector2.zero;

		// Token: 0x04004A6A RID: 19050
		public AnimationCurve swimSpeedToRedirectAmount = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004A6B RID: 19051
		public Vector2 swimSpeedToRedirectAmountMinMax = Vector2.zero;

		// Token: 0x04004A6C RID: 19052
		public AnimationCurve swimSpeedToMaxRedirectAngle = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004A6D RID: 19053
		public Vector2 swimSpeedToMaxRedirectAngleMinMax = Vector2.zero;

		// Token: 0x04004A6E RID: 19054
		public AnimationCurve handSpeedToRedirectAmount = AnimationCurve.Linear(0f, 1f, 1f, 0f);

		// Token: 0x04004A6F RID: 19055
		public Vector2 handSpeedToRedirectAmountMinMax = Vector2.zero;

		// Token: 0x04004A70 RID: 19056
		public AnimationCurve handAccelToRedirectAmount = AnimationCurve.Linear(0f, 1f, 1f, 0f);

		// Token: 0x04004A71 RID: 19057
		public Vector2 handAccelToRedirectAmountMinMax = Vector2.zero;

		// Token: 0x04004A72 RID: 19058
		public AnimationCurve nonDiveDampingHapticsAmount = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004A73 RID: 19059
		public Vector2 nonDiveDampingHapticsAmountMinMax = Vector2.zero;
	}
}
