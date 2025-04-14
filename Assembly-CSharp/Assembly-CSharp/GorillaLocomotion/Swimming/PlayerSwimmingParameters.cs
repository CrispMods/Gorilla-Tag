using System;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B44 RID: 2884
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerSwimmingParameters", order = 1)]
	public class PlayerSwimmingParameters : ScriptableObject
	{
		// Token: 0x04004A5E RID: 19038
		[Header("Base Settings")]
		public float floatingWaterLevelBelowHead = 0.6f;

		// Token: 0x04004A5F RID: 19039
		public float buoyancyFadeDist = 0.3f;

		// Token: 0x04004A60 RID: 19040
		public bool extendBouyancyFromSpeed;

		// Token: 0x04004A61 RID: 19041
		public float buoyancyExtensionDecayHalflife = 0.2f;

		// Token: 0x04004A62 RID: 19042
		public float baseUnderWaterDampingHalfLife = 0.25f;

		// Token: 0x04004A63 RID: 19043
		public float swimUnderWaterDampingHalfLife = 1.1f;

		// Token: 0x04004A64 RID: 19044
		public AnimationCurve speedToBouyancyExtension = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004A65 RID: 19045
		public Vector2 speedToBouyancyExtensionMinMax = Vector2.zero;

		// Token: 0x04004A66 RID: 19046
		public float swimmingVelocityOutOfWaterDrainRate = 3f;

		// Token: 0x04004A67 RID: 19047
		[Range(0f, 1f)]
		public float underwaterJumpsAsSwimVelocityFactor = 1f;

		// Token: 0x04004A68 RID: 19048
		[Range(0f, 1f)]
		public float swimmingHapticsStrength = 0.5f;

		// Token: 0x04004A69 RID: 19049
		[Header("Surface Jumping")]
		public bool allowWaterSurfaceJumps;

		// Token: 0x04004A6A RID: 19050
		public float waterSurfaceJumpHandSpeedThreshold = 1f;

		// Token: 0x04004A6B RID: 19051
		public float waterSurfaceJumpAmount;

		// Token: 0x04004A6C RID: 19052
		public float waterSurfaceJumpMaxSpeed = 1f;

		// Token: 0x04004A6D RID: 19053
		public AnimationCurve waterSurfaceJumpPalmFacingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004A6E RID: 19054
		public AnimationCurve waterSurfaceJumpHandVelocityFacingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004A6F RID: 19055
		[Header("Diving")]
		public bool applyDiveSteering;

		// Token: 0x04004A70 RID: 19056
		public bool applyDiveDampingMultiplier;

		// Token: 0x04004A71 RID: 19057
		public float diveDampingMultiplier = 1f;

		// Token: 0x04004A72 RID: 19058
		[Tooltip("In degrees")]
		public float maxDiveSteerAnglePerStep = 1f;

		// Token: 0x04004A73 RID: 19059
		public float diveVelocityAveragingWindow = 0.1f;

		// Token: 0x04004A74 RID: 19060
		public bool applyDiveSwimVelocityConversion;

		// Token: 0x04004A75 RID: 19061
		[Tooltip("In meters per second")]
		public float diveSwimVelocityConversionRate = 3f;

		// Token: 0x04004A76 RID: 19062
		public float diveMaxSwimVelocityConversion = 3f;

		// Token: 0x04004A77 RID: 19063
		public bool reduceDiveSteeringBelowVelocityPlane;

		// Token: 0x04004A78 RID: 19064
		public float reduceDiveSteeringBelowPlaneFadeStartDist = 0.4f;

		// Token: 0x04004A79 RID: 19065
		public float reduceDiveSteeringBelowPlaneFadeEndDist = 0.55f;

		// Token: 0x04004A7A RID: 19066
		public AnimationCurve palmFacingToRedirectAmount = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004A7B RID: 19067
		public Vector2 palmFacingToRedirectAmountMinMax = Vector2.zero;

		// Token: 0x04004A7C RID: 19068
		public AnimationCurve swimSpeedToRedirectAmount = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004A7D RID: 19069
		public Vector2 swimSpeedToRedirectAmountMinMax = Vector2.zero;

		// Token: 0x04004A7E RID: 19070
		public AnimationCurve swimSpeedToMaxRedirectAngle = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004A7F RID: 19071
		public Vector2 swimSpeedToMaxRedirectAngleMinMax = Vector2.zero;

		// Token: 0x04004A80 RID: 19072
		public AnimationCurve handSpeedToRedirectAmount = AnimationCurve.Linear(0f, 1f, 1f, 0f);

		// Token: 0x04004A81 RID: 19073
		public Vector2 handSpeedToRedirectAmountMinMax = Vector2.zero;

		// Token: 0x04004A82 RID: 19074
		public AnimationCurve handAccelToRedirectAmount = AnimationCurve.Linear(0f, 1f, 1f, 0f);

		// Token: 0x04004A83 RID: 19075
		public Vector2 handAccelToRedirectAmountMinMax = Vector2.zero;

		// Token: 0x04004A84 RID: 19076
		public AnimationCurve nonDiveDampingHapticsAmount = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004A85 RID: 19077
		public Vector2 nonDiveDampingHapticsAmountMinMax = Vector2.zero;
	}
}
