using System;
using GorillaLocomotion.Climbing;
using Photon.Pun;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B48 RID: 2888
	public struct WaterOverlappingCollider
	{
		// Token: 0x06004835 RID: 18485 RVA: 0x0015D200 File Offset: 0x0015B400
		public void PlayRippleEffect(GameObject rippleEffectPrefab, Vector3 surfacePoint, Vector3 surfaceNormal, float defaultRippleScale, float currentTime, WaterVolume volume)
		{
			this.lastRipplePosition = this.GetClosestPositionOnSurface(surfacePoint, surfaceNormal);
			this.lastBoundingRadius = this.GetBoundingRadiusOnSurface(surfaceNormal);
			this.lastRippleScale = defaultRippleScale * this.lastBoundingRadius * 2f * this.scaleMultiplier;
			this.lastRippleTime = currentTime;
			ObjectPools.instance.Instantiate(rippleEffectPrefab, this.lastRipplePosition, Quaternion.FromToRotation(Vector3.up, this.lastSurfaceQuery.surfaceNormal) * Quaternion.AngleAxis(-90f, Vector3.right), this.lastRippleScale).GetComponent<WaterRippleEffect>().PlayEffect(volume);
		}

		// Token: 0x06004836 RID: 18486 RVA: 0x0015D298 File Offset: 0x0015B498
		public void PlaySplashEffect(GameObject splashEffectPrefab, Vector3 splashPosition, float splashScale, bool bigSplash, bool enteringWater, WaterVolume volume)
		{
			Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, this.lastSurfaceQuery.surfaceNormal) * Quaternion.AngleAxis(-90f, Vector3.right);
			ObjectPools.instance.Instantiate(splashEffectPrefab, splashPosition, quaternion, splashScale * this.scaleMultiplier).GetComponent<WaterSplashEffect>().PlayEffect(bigSplash, enteringWater, this.scaleMultiplier, volume);
			if (this.photonViewForRPC != null)
			{
				float time = Time.time;
				int num = -1;
				float num2 = time + 10f;
				for (int i = 0; i < WaterVolume.splashRPCSendTimes.Length; i++)
				{
					if (WaterVolume.splashRPCSendTimes[i] < num2)
					{
						num2 = WaterVolume.splashRPCSendTimes[i];
						num = i;
					}
				}
				if (time - 0.5f > num2)
				{
					WaterVolume.splashRPCSendTimes[num] = time;
					this.photonViewForRPC.SendRPC("RPC_PlaySplashEffect", RpcTarget.Others, new object[]
					{
						splashPosition,
						quaternion,
						splashScale * this.scaleMultiplier,
						this.lastBoundingRadius,
						bigSplash,
						enteringWater
					});
				}
			}
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x0015D3B8 File Offset: 0x0015B5B8
		public void PlayDripEffect(GameObject rippleEffectPrefab, Vector3 surfacePoint, Vector3 surfaceNormal, float dripScale)
		{
			Vector3 closestPositionOnSurface = this.GetClosestPositionOnSurface(surfacePoint, surfaceNormal);
			float d = this.overrideBoundingRadius ? this.boundingRadiusOverride : this.lastBoundingRadius;
			Vector3 b = Vector3.ProjectOnPlane(Random.onUnitSphere * d * 0.5f, surfaceNormal);
			ObjectPools.instance.Instantiate(rippleEffectPrefab, closestPositionOnSurface + b, Quaternion.FromToRotation(Vector3.up, this.lastSurfaceQuery.surfaceNormal) * Quaternion.AngleAxis(-90f, Vector3.right), dripScale * this.scaleMultiplier);
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x0015D446 File Offset: 0x0015B646
		public Vector3 GetClosestPositionOnSurface(Vector3 surfacePoint, Vector3 surfaceNormal)
		{
			return Vector3.ProjectOnPlane(this.collider.transform.position - surfacePoint, surfaceNormal) + surfacePoint;
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x0015D46C File Offset: 0x0015B66C
		private float GetBoundingRadiusOnSurface(Vector3 surfaceNormal)
		{
			if (this.overrideBoundingRadius)
			{
				this.lastBoundingRadius = this.boundingRadiusOverride;
				return this.boundingRadiusOverride;
			}
			Vector3 extents = this.collider.bounds.extents;
			Vector3 vector = Vector3.ProjectOnPlane(this.collider.transform.right * extents.x, surfaceNormal);
			Vector3 vector2 = Vector3.ProjectOnPlane(this.collider.transform.up * extents.y, surfaceNormal);
			Vector3 vector3 = Vector3.ProjectOnPlane(this.collider.transform.forward * extents.z, surfaceNormal);
			float sqrMagnitude = vector.sqrMagnitude;
			float sqrMagnitude2 = vector2.sqrMagnitude;
			float sqrMagnitude3 = vector3.sqrMagnitude;
			if (sqrMagnitude >= sqrMagnitude2 && sqrMagnitude >= sqrMagnitude3)
			{
				return vector.magnitude;
			}
			if (sqrMagnitude2 >= sqrMagnitude && sqrMagnitude2 >= sqrMagnitude3)
			{
				return vector2.magnitude;
			}
			return vector3.magnitude;
		}

		// Token: 0x04004AB4 RID: 19124
		public bool playBigSplash;

		// Token: 0x04004AB5 RID: 19125
		public bool playDripEffect;

		// Token: 0x04004AB6 RID: 19126
		public bool overrideBoundingRadius;

		// Token: 0x04004AB7 RID: 19127
		public float boundingRadiusOverride;

		// Token: 0x04004AB8 RID: 19128
		public float scaleMultiplier;

		// Token: 0x04004AB9 RID: 19129
		public Collider collider;

		// Token: 0x04004ABA RID: 19130
		public GorillaVelocityTracker velocityTracker;

		// Token: 0x04004ABB RID: 19131
		public WaterVolume.SurfaceQuery lastSurfaceQuery;

		// Token: 0x04004ABC RID: 19132
		public NetworkView photonViewForRPC;

		// Token: 0x04004ABD RID: 19133
		public bool surfaceDetected;

		// Token: 0x04004ABE RID: 19134
		public bool inWater;

		// Token: 0x04004ABF RID: 19135
		public bool inVolume;

		// Token: 0x04004AC0 RID: 19136
		public float lastBoundingRadius;

		// Token: 0x04004AC1 RID: 19137
		public Vector3 lastRipplePosition;

		// Token: 0x04004AC2 RID: 19138
		public float lastRippleScale;

		// Token: 0x04004AC3 RID: 19139
		public float lastRippleTime;

		// Token: 0x04004AC4 RID: 19140
		public float lastInWaterTime;

		// Token: 0x04004AC5 RID: 19141
		public float nextDripTime;
	}
}
