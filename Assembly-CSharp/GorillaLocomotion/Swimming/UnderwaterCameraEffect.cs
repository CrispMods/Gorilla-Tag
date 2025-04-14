using System;
using CjLib;
using GorillaTag;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B44 RID: 2884
	[ExecuteAlways]
	public class UnderwaterCameraEffect : MonoBehaviour
	{
		// Token: 0x0600481E RID: 18462 RVA: 0x0015BE24 File Offset: 0x0015A024
		private void SetOffScreenPosition()
		{
			base.transform.localScale = new Vector3(2f * (this.frustumPlaneExtents.x + 0.04f), 0f, 1f);
			base.transform.localPosition = new Vector3(0f, -(this.frustumPlaneExtents.y + 0.04f), this.distanceFromCamera);
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x0015BE90 File Offset: 0x0015A090
		private void SetFullScreenPosition()
		{
			base.transform.localScale = new Vector3(2f * (this.frustumPlaneExtents.x + 0.04f), 2f * (this.frustumPlaneExtents.y + 0.04f), 1f);
			base.transform.localPosition = new Vector3(0f, 0f, this.distanceFromCamera);
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x0015BF00 File Offset: 0x0015A100
		private void OnEnable()
		{
			if (this.targetCamera == null)
			{
				this.targetCamera = Camera.main;
			}
			this.hasTargetCamera = (this.targetCamera != null);
			this.InitializeShaderProperties();
		}

		// Token: 0x06004821 RID: 18465 RVA: 0x0015BF34 File Offset: 0x0015A134
		private void Start()
		{
			this.player = GTPlayer.Instance;
			this.cachedAspectRatio = this.targetCamera.aspect;
			this.cachedFov = this.targetCamera.fieldOfView;
			this.CalculateFrustumPlaneBounds(this.cachedFov, this.cachedAspectRatio);
			this.SetOffScreenPosition();
		}

		// Token: 0x06004822 RID: 18466 RVA: 0x0015BF88 File Offset: 0x0015A188
		private void LateUpdate()
		{
			if (!this.hasTargetCamera)
			{
				return;
			}
			if (!this.player)
			{
				return;
			}
			if (this.player.HeadOverlappingWaterVolumes.Count < 1)
			{
				this.SetCameraOverlapState(UnderwaterCameraEffect.CameraOverlapWaterState.OutOfWater);
				if (this.planeRenderer.enabled)
				{
					this.planeRenderer.enabled = false;
					this.SetOffScreenPosition();
				}
				if (this.underwaterParticleEffect != null && this.underwaterParticleEffect.gameObject.activeInHierarchy)
				{
					this.underwaterParticleEffect.UpdateParticleEffect(false, ref this.waterSurface);
				}
				return;
			}
			if (this.targetCamera.aspect != this.cachedAspectRatio || this.targetCamera.fieldOfView != this.cachedFov)
			{
				this.cachedAspectRatio = this.targetCamera.aspect;
				this.cachedFov = this.targetCamera.fieldOfView;
				this.CalculateFrustumPlaneBounds(this.cachedFov, this.cachedAspectRatio);
			}
			bool flag = false;
			float num = float.MinValue;
			Vector3 position = this.targetCamera.transform.position;
			for (int i = 0; i < this.player.HeadOverlappingWaterVolumes.Count; i++)
			{
				WaterVolume.SurfaceQuery surfaceQuery;
				if (this.player.HeadOverlappingWaterVolumes[i].GetSurfaceQueryForPoint(position, out surfaceQuery, false))
				{
					float num2 = Vector3.Dot(surfaceQuery.surfacePoint - position, surfaceQuery.surfaceNormal);
					if (num2 > num)
					{
						flag = true;
						num = num2;
						this.waterSurface = surfaceQuery;
					}
				}
			}
			if (flag)
			{
				Vector3 inPoint = this.targetCamera.transform.InverseTransformPoint(this.waterSurface.surfacePoint);
				Vector3 inNormal = this.targetCamera.transform.InverseTransformDirection(this.waterSurface.surfaceNormal);
				Plane p = new Plane(inNormal, inPoint);
				Plane p2 = new Plane(Vector3.forward, -this.distanceFromCamera);
				Vector3 vector;
				Vector3 vector2;
				if (this.IntersectPlanes(p2, p, out vector, out vector2))
				{
					Vector3 normalized = Vector3.Cross(vector2, Vector3.forward).normalized;
					float num3 = Vector3.Dot(new Vector3(vector.x, vector.y, 0f), normalized);
					if (num3 > this.frustumPlaneExtents.y + 0.04f)
					{
						this.SetFullScreenPosition();
						this.SetCameraOverlapState(UnderwaterCameraEffect.CameraOverlapWaterState.FullySubmerged);
					}
					else if (num3 < -(this.frustumPlaneExtents.y + 0.04f))
					{
						this.SetOffScreenPosition();
						this.SetCameraOverlapState(UnderwaterCameraEffect.CameraOverlapWaterState.OutOfWater);
					}
					else
					{
						float num4 = num3;
						num4 += this.GetFrustumCoverageDistance(-normalized) + 0.04f;
						float num5 = this.GetFrustumCoverageDistance(vector2) + 0.04f;
						num5 += this.GetFrustumCoverageDistance(-vector2) + 0.04f;
						base.transform.localScale = new Vector3(num5, num4, 1f);
						base.transform.localPosition = normalized * (num3 - num4 * 0.5f) + new Vector3(0f, 0f, this.distanceFromCamera);
						float angle = Vector3.SignedAngle(Vector3.up, normalized, Vector3.forward);
						base.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
						this.SetCameraOverlapState(UnderwaterCameraEffect.CameraOverlapWaterState.PartiallySubmerged);
					}
					if (this.debugDraw)
					{
						Vector3 a = this.targetCamera.transform.TransformPoint(vector);
						Vector3 a2 = this.targetCamera.transform.TransformDirection(vector2);
						DebugUtil.DrawLine(a - 2f * this.frustumPlaneExtents.x * a2, a + 2f * this.frustumPlaneExtents.x * a2, Color.white, false);
					}
				}
				else if (new Plane(this.waterSurface.surfaceNormal, this.waterSurface.surfacePoint).GetSide(this.targetCamera.transform.position))
				{
					this.SetFullScreenPosition();
					this.SetCameraOverlapState(UnderwaterCameraEffect.CameraOverlapWaterState.FullySubmerged);
				}
				else
				{
					this.SetOffScreenPosition();
					this.SetCameraOverlapState(UnderwaterCameraEffect.CameraOverlapWaterState.OutOfWater);
				}
			}
			else
			{
				this.SetOffScreenPosition();
				this.SetCameraOverlapState(UnderwaterCameraEffect.CameraOverlapWaterState.OutOfWater);
			}
			if (this.underwaterParticleEffect != null && this.underwaterParticleEffect.gameObject.activeInHierarchy)
			{
				this.underwaterParticleEffect.UpdateParticleEffect(flag, ref this.waterSurface);
			}
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x0015C3C0 File Offset: 0x0015A5C0
		[DebugOption]
		private void InitializeShaderProperties()
		{
			Shader.DisableKeyword("_GLOBAL_CAMERA_TOUCHING_WATER");
			Shader.DisableKeyword("_GLOBAL_CAMERA_FULLY_UNDERWATER");
			float w = -Vector3.Dot(this.waterSurface.surfaceNormal, this.waterSurface.surfacePoint);
			Shader.SetGlobalVector(this.shaderParam_GlobalCameraOverlapWaterSurfacePlane, new Vector4(this.waterSurface.surfaceNormal.x, this.waterSurface.surfaceNormal.y, this.waterSurface.surfaceNormal.z, w));
		}

		// Token: 0x06004824 RID: 18468 RVA: 0x0015C440 File Offset: 0x0015A640
		private void SetCameraOverlapState(UnderwaterCameraEffect.CameraOverlapWaterState state)
		{
			if (state != this.cameraOverlapWaterState || state == UnderwaterCameraEffect.CameraOverlapWaterState.Uninitialized)
			{
				this.cameraOverlapWaterState = state;
				switch (this.cameraOverlapWaterState)
				{
				case UnderwaterCameraEffect.CameraOverlapWaterState.Uninitialized:
				case UnderwaterCameraEffect.CameraOverlapWaterState.OutOfWater:
					Shader.DisableKeyword("_GLOBAL_CAMERA_TOUCHING_WATER");
					Shader.DisableKeyword("_GLOBAL_CAMERA_FULLY_UNDERWATER");
					break;
				case UnderwaterCameraEffect.CameraOverlapWaterState.PartiallySubmerged:
					Shader.EnableKeyword("_GLOBAL_CAMERA_TOUCHING_WATER");
					Shader.DisableKeyword("_GLOBAL_CAMERA_FULLY_UNDERWATER");
					break;
				case UnderwaterCameraEffect.CameraOverlapWaterState.FullySubmerged:
					Shader.EnableKeyword("_GLOBAL_CAMERA_TOUCHING_WATER");
					Shader.EnableKeyword("_GLOBAL_CAMERA_FULLY_UNDERWATER");
					break;
				}
			}
			if (this.cameraOverlapWaterState == UnderwaterCameraEffect.CameraOverlapWaterState.PartiallySubmerged)
			{
				Plane plane = new Plane(this.waterSurface.surfaceNormal, this.waterSurface.surfacePoint);
				Shader.SetGlobalVector(this.shaderParam_GlobalCameraOverlapWaterSurfacePlane, new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance));
			}
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x0015C520 File Offset: 0x0015A720
		private void CalculateFrustumPlaneBounds(float fieldOfView, float aspectRatio)
		{
			float num = Mathf.Tan(0.017453292f * fieldOfView * 0.5f) * this.distanceFromCamera;
			float num2 = aspectRatio * num + 0.04f;
			float num3 = 1f / aspectRatio * num + 0.04f;
			this.frustumPlaneExtents = new Vector2(num2, num3);
			this.frustumPlaneCornersLocal[0] = new Vector3(-num2, -num3, this.distanceFromCamera);
			this.frustumPlaneCornersLocal[1] = new Vector3(-num2, num3, this.distanceFromCamera);
			this.frustumPlaneCornersLocal[2] = new Vector3(num2, num3, this.distanceFromCamera);
			this.frustumPlaneCornersLocal[3] = new Vector3(num2, -num3, this.distanceFromCamera);
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x0015C5D8 File Offset: 0x0015A7D8
		private bool IntersectPlanes(Plane p1, Plane p2, out Vector3 point, out Vector3 direction)
		{
			direction = Vector3.Cross(p1.normal, p2.normal);
			float num = Vector3.Dot(direction, direction);
			if (num < Mathf.Epsilon)
			{
				point = Vector3.zero;
				return false;
			}
			point = Vector3.Cross(direction, p1.distance * p2.normal - p2.distance * p1.normal) / num;
			return true;
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x0015C66C File Offset: 0x0015A86C
		private float GetFrustumCoverageDistance(Vector3 localDirection)
		{
			float num = float.MinValue;
			for (int i = 0; i < this.frustumPlaneCornersLocal.Length; i++)
			{
				float num2 = Vector3.Dot(this.frustumPlaneCornersLocal[i], localDirection);
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x04004A88 RID: 19080
		private const float edgeBuffer = 0.04f;

		// Token: 0x04004A89 RID: 19081
		[SerializeField]
		private Camera targetCamera;

		// Token: 0x04004A8A RID: 19082
		[SerializeField]
		private MeshRenderer planeRenderer;

		// Token: 0x04004A8B RID: 19083
		[SerializeField]
		private UnderwaterParticleEffects underwaterParticleEffect;

		// Token: 0x04004A8C RID: 19084
		[SerializeField]
		private float distanceFromCamera = 0.02f;

		// Token: 0x04004A8D RID: 19085
		[SerializeField]
		[DebugOption]
		private bool debugDraw;

		// Token: 0x04004A8E RID: 19086
		private float cachedAspectRatio = 1f;

		// Token: 0x04004A8F RID: 19087
		private float cachedFov = 90f;

		// Token: 0x04004A90 RID: 19088
		private readonly Vector3[] frustumPlaneCornersLocal = new Vector3[4];

		// Token: 0x04004A91 RID: 19089
		private Vector2 frustumPlaneExtents;

		// Token: 0x04004A92 RID: 19090
		private GTPlayer player;

		// Token: 0x04004A93 RID: 19091
		private WaterVolume.SurfaceQuery waterSurface;

		// Token: 0x04004A94 RID: 19092
		private const string kShaderKeyword_GlobalCameraTouchingWater = "_GLOBAL_CAMERA_TOUCHING_WATER";

		// Token: 0x04004A95 RID: 19093
		private const string kShaderKeyword_GlobalCameraFullyUnderwater = "_GLOBAL_CAMERA_FULLY_UNDERWATER";

		// Token: 0x04004A96 RID: 19094
		private int shaderParam_GlobalCameraOverlapWaterSurfacePlane = Shader.PropertyToID("_GlobalCameraOverlapWaterSurfacePlane");

		// Token: 0x04004A97 RID: 19095
		private bool hasTargetCamera;

		// Token: 0x04004A98 RID: 19096
		[DebugReadout]
		private UnderwaterCameraEffect.CameraOverlapWaterState cameraOverlapWaterState;

		// Token: 0x02000B45 RID: 2885
		private enum CameraOverlapWaterState
		{
			// Token: 0x04004A9A RID: 19098
			Uninitialized,
			// Token: 0x04004A9B RID: 19099
			OutOfWater,
			// Token: 0x04004A9C RID: 19100
			PartiallySubmerged,
			// Token: 0x04004A9D RID: 19101
			FullySubmerged
		}
	}
}
