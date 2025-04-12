using System;
using GorillaLocomotion.Climbing;
using Photon.Pun;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B4B RID: 2891
	public struct WaterOverlappingCollider
	{
		// Token: 0x06004841 RID: 18497 RVA: 0x00191D20 File Offset: 0x0018FF20
		public void PlayRippleEffect(GameObject rippleEffectPrefab, Vector3 surfacePoint, Vector3 surfaceNormal, float defaultRippleScale, float currentTime, WaterVolume volume)
		{
			this.lastRipplePosition = this.GetClosestPositionOnSurface(surfacePoint, surfaceNormal);
			this.lastBoundingRadius = this.GetBoundingRadiusOnSurface(surfaceNormal);
			this.lastRippleScale = defaultRippleScale * this.lastBoundingRadius * 2f * this.scaleMultiplier;
			this.lastRippleTime = currentTime;
			ObjectPools.instance.Instantiate(rippleEffectPrefab, this.lastRipplePosition, Quaternion.FromToRotation(Vector3.up, this.lastSurfaceQuery.surfaceNormal) * Quaternion.AngleAxis(-90f, Vector3.right), this.lastRippleScale).GetComponent<WaterRippleEffect>().PlayEffect(volume);
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x00191DB8 File Offset: 0x0018FFB8
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

		// Token: 0x06004843 RID: 18499 RVA: 0x00191ED8 File Offset: 0x001900D8
		public void PlayDripEffect(GameObject rippleEffectPrefab, Vector3 surfacePoint, Vector3 surfaceNormal, float dripScale)
		{
			Vector3 closestPositionOnSurface = this.GetClosestPositionOnSurface(surfacePoint, surfaceNormal);
			float d = this.overrideBoundingRadius ? this.boundingRadiusOverride : this.lastBoundingRadius;
			Vector3 b = Vector3.ProjectOnPlane(UnityEngine.Random.onUnitSphere * d * 0.5f, surfaceNormal);
			ObjectPools.instance.Instantiate(rippleEffectPrefab, closestPositionOnSurface + b, Quaternion.FromToRotation(Vector3.up, this.lastSurfaceQuery.surfaceNormal) * Quaternion.AngleAxis(-90f, Vector3.right), dripScale * this.scaleMultiplier);
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x0005E355 File Offset: 0x0005C555
		public Vector3 GetClosestPositionOnSurface(Vector3 surfacePoint, Vector3 surfaceNormal)
		{
			return Vector3.ProjectOnPlane(this.collider.transform.position - surfacePoint, surfaceNormal) + surfacePoint;
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x00191F68 File Offset: 0x00190168
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

		// Token: 0x04004AC6 RID: 19142
		public bool playBigSplash;

		// Token: 0x04004AC7 RID: 19143
		public bool playDripEffect;

		// Token: 0x04004AC8 RID: 19144
		public bool overrideBoundingRadius;

		// Token: 0x04004AC9 RID: 19145
		public float boundingRadiusOverride;

		// Token: 0x04004ACA RID: 19146
		public float scaleMultiplier;

		// Token: 0x04004ACB RID: 19147
		public Collider collider;

		// Token: 0x04004ACC RID: 19148
		public GorillaVelocityTracker velocityTracker;

		// Token: 0x04004ACD RID: 19149
		public WaterVolume.SurfaceQuery lastSurfaceQuery;

		// Token: 0x04004ACE RID: 19150
		public NetworkView photonViewForRPC;

		// Token: 0x04004ACF RID: 19151
		public bool surfaceDetected;

		// Token: 0x04004AD0 RID: 19152
		public bool inWater;

		// Token: 0x04004AD1 RID: 19153
		public bool inVolume;

		// Token: 0x04004AD2 RID: 19154
		public float lastBoundingRadius;

		// Token: 0x04004AD3 RID: 19155
		public Vector3 lastRipplePosition;

		// Token: 0x04004AD4 RID: 19156
		public float lastRippleScale;

		// Token: 0x04004AD5 RID: 19157
		public float lastRippleTime;

		// Token: 0x04004AD6 RID: 19158
		public float lastInWaterTime;

		// Token: 0x04004AD7 RID: 19159
		public float nextDripTime;
	}
}
