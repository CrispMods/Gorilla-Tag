using System;
using GorillaLocomotion.Climbing;
using Photon.Pun;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B75 RID: 2933
	public struct WaterOverlappingCollider
	{
		// Token: 0x0600497E RID: 18814 RVA: 0x00198D40 File Offset: 0x00196F40
		public void PlayRippleEffect(GameObject rippleEffectPrefab, Vector3 surfacePoint, Vector3 surfaceNormal, float defaultRippleScale, float currentTime, WaterVolume volume)
		{
			this.lastRipplePosition = this.GetClosestPositionOnSurface(surfacePoint, surfaceNormal);
			this.lastBoundingRadius = this.GetBoundingRadiusOnSurface(surfaceNormal);
			this.lastRippleScale = defaultRippleScale * this.lastBoundingRadius * 2f * this.scaleMultiplier;
			this.lastRippleTime = currentTime;
			ObjectPools.instance.Instantiate(rippleEffectPrefab, this.lastRipplePosition, Quaternion.FromToRotation(Vector3.up, this.lastSurfaceQuery.surfaceNormal) * Quaternion.AngleAxis(-90f, Vector3.right), this.lastRippleScale).GetComponent<WaterRippleEffect>().PlayEffect(volume);
		}

		// Token: 0x0600497F RID: 18815 RVA: 0x00198DD8 File Offset: 0x00196FD8
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

		// Token: 0x06004980 RID: 18816 RVA: 0x00198EF8 File Offset: 0x001970F8
		public void PlayDripEffect(GameObject rippleEffectPrefab, Vector3 surfacePoint, Vector3 surfaceNormal, float dripScale)
		{
			Vector3 closestPositionOnSurface = this.GetClosestPositionOnSurface(surfacePoint, surfaceNormal);
			float d = this.overrideBoundingRadius ? this.boundingRadiusOverride : this.lastBoundingRadius;
			Vector3 b = Vector3.ProjectOnPlane(UnityEngine.Random.onUnitSphere * d * 0.5f, surfaceNormal);
			ObjectPools.instance.Instantiate(rippleEffectPrefab, closestPositionOnSurface + b, Quaternion.FromToRotation(Vector3.up, this.lastSurfaceQuery.surfaceNormal) * Quaternion.AngleAxis(-90f, Vector3.right), dripScale * this.scaleMultiplier);
		}

		// Token: 0x06004981 RID: 18817 RVA: 0x0005FD42 File Offset: 0x0005DF42
		public Vector3 GetClosestPositionOnSurface(Vector3 surfacePoint, Vector3 surfaceNormal)
		{
			return Vector3.ProjectOnPlane(this.collider.transform.position - surfacePoint, surfaceNormal) + surfacePoint;
		}

		// Token: 0x06004982 RID: 18818 RVA: 0x00198F88 File Offset: 0x00197188
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

		// Token: 0x04004BAA RID: 19370
		public bool playBigSplash;

		// Token: 0x04004BAB RID: 19371
		public bool playDripEffect;

		// Token: 0x04004BAC RID: 19372
		public bool overrideBoundingRadius;

		// Token: 0x04004BAD RID: 19373
		public float boundingRadiusOverride;

		// Token: 0x04004BAE RID: 19374
		public float scaleMultiplier;

		// Token: 0x04004BAF RID: 19375
		public Collider collider;

		// Token: 0x04004BB0 RID: 19376
		public GorillaVelocityTracker velocityTracker;

		// Token: 0x04004BB1 RID: 19377
		public WaterVolume.SurfaceQuery lastSurfaceQuery;

		// Token: 0x04004BB2 RID: 19378
		public NetworkView photonViewForRPC;

		// Token: 0x04004BB3 RID: 19379
		public bool surfaceDetected;

		// Token: 0x04004BB4 RID: 19380
		public bool inWater;

		// Token: 0x04004BB5 RID: 19381
		public bool inVolume;

		// Token: 0x04004BB6 RID: 19382
		public float lastBoundingRadius;

		// Token: 0x04004BB7 RID: 19383
		public Vector3 lastRipplePosition;

		// Token: 0x04004BB8 RID: 19384
		public float lastRippleScale;

		// Token: 0x04004BB9 RID: 19385
		public float lastRippleTime;

		// Token: 0x04004BBA RID: 19386
		public float lastInWaterTime;

		// Token: 0x04004BBB RID: 19387
		public float nextDripTime;
	}
}
