using System;
using System.Collections.Generic;
using CjLib;
using GorillaLocomotion.Climbing;
using GorillaTag.GuidedRefs;
using GT_CustomMapSupportRuntime;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B78 RID: 2936
	[RequireComponent(typeof(Collider))]
	public class WaterVolume : BaseGuidedRefTargetMono
	{
		// Token: 0x14000086 RID: 134
		// (add) Token: 0x06004985 RID: 18821 RVA: 0x0019913C File Offset: 0x0019733C
		// (remove) Token: 0x06004986 RID: 18822 RVA: 0x00199174 File Offset: 0x00197374
		public event WaterVolume.WaterVolumeEvent ColliderEnteredVolume;

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06004987 RID: 18823 RVA: 0x001991AC File Offset: 0x001973AC
		// (remove) Token: 0x06004988 RID: 18824 RVA: 0x001991E4 File Offset: 0x001973E4
		public event WaterVolume.WaterVolumeEvent ColliderExitedVolume;

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x06004989 RID: 18825 RVA: 0x0019921C File Offset: 0x0019741C
		// (remove) Token: 0x0600498A RID: 18826 RVA: 0x00199254 File Offset: 0x00197454
		public event WaterVolume.WaterVolumeEvent ColliderEnteredWater;

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x0600498B RID: 18827 RVA: 0x0019928C File Offset: 0x0019748C
		// (remove) Token: 0x0600498C RID: 18828 RVA: 0x001992C4 File Offset: 0x001974C4
		public event WaterVolume.WaterVolumeEvent ColliderExitedWater;

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x0600498D RID: 18829 RVA: 0x0005FD80 File Offset: 0x0005DF80
		public GTPlayer.LiquidType LiquidType
		{
			get
			{
				return this.liquidType;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x0600498E RID: 18830 RVA: 0x0005FD88 File Offset: 0x0005DF88
		public WaterCurrent Current
		{
			get
			{
				return this.waterCurrent;
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x0600498F RID: 18831 RVA: 0x0005FD90 File Offset: 0x0005DF90
		public WaterParameters Parameters
		{
			get
			{
				return this.waterParams;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06004990 RID: 18832 RVA: 0x001992FC File Offset: 0x001974FC
		private VRRig PlayerVRRig
		{
			get
			{
				if (this.playerVRRig == null)
				{
					GorillaTagger instance = GorillaTagger.Instance;
					if (instance != null)
					{
						this.playerVRRig = instance.offlineVRRig;
					}
				}
				return this.playerVRRig;
			}
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x00199338 File Offset: 0x00197538
		public bool GetSurfaceQueryForPoint(Vector3 point, out WaterVolume.SurfaceQuery result, bool debugDraw = false)
		{
			result = default(WaterVolume.SurfaceQuery);
			Ray ray = new Ray(new Vector3(point.x, this.volumeMaxHeight, point.z), Vector3.down);
			Ray ray2 = new Ray(new Vector3(point.x, this.volumeMinHeight, point.z), Vector3.up);
			float num = this.volumeMaxHeight - this.volumeMinHeight;
			float num2 = float.MinValue;
			float num3 = float.MaxValue;
			bool flag = false;
			bool flag2 = false;
			float num4 = 0f;
			for (int i = 0; i < this.surfaceColliders.Count; i++)
			{
				bool enabled = this.surfaceColliders[i].enabled;
				this.surfaceColliders[i].enabled = true;
				RaycastHit hit;
				if (this.surfaceColliders[i].Raycast(ray, out hit, num) && hit.point.y > num2 && this.HitOutsideSurfaceOfMesh(ray.direction, this.surfaceColliders[i], hit))
				{
					num2 = hit.point.y;
					flag = true;
					result.surfacePoint = hit.point;
					result.surfaceNormal = hit.normal;
				}
				RaycastHit hit2;
				if (this.surfaceColliders[i].Raycast(ray2, out hit2, num) && hit2.point.y < num3 && this.HitOutsideSurfaceOfMesh(ray2.direction, this.surfaceColliders[i], hit2))
				{
					num3 = hit2.point.y;
					flag2 = true;
					num4 = hit2.point.y;
				}
				this.surfaceColliders[i].enabled = enabled;
			}
			if (!flag && this.surfacePlane != null)
			{
				flag = true;
				result.surfacePoint = point - Vector3.Dot(point - this.surfacePlane.position, this.surfacePlane.up) * this.surfacePlane.up;
				result.surfaceNormal = this.surfacePlane.up;
			}
			if (flag && flag2)
			{
				result.maxDepth = result.surfacePoint.y - num4;
			}
			else if (flag)
			{
				result.maxDepth = result.surfacePoint.y - this.volumeMinHeight;
			}
			else
			{
				result.maxDepth = this.volumeMaxHeight - this.volumeMinHeight;
			}
			if (debugDraw)
			{
				if (flag)
				{
					DebugUtil.DrawLine(ray.origin, ray.origin + ray.direction * num, Color.green, false);
					DebugUtil.DrawSphere(result.surfacePoint, 0.001f, 12, 12, Color.green, false, DebugUtil.Style.SolidColor);
				}
				else
				{
					DebugUtil.DrawLine(ray.origin, ray.origin + ray.direction * num, Color.red, false);
				}
				if (flag2)
				{
					DebugUtil.DrawLine(ray2.origin, ray2.origin + ray2.direction * num, Color.yellow, false);
					DebugUtil.DrawSphere(new Vector3(result.surfacePoint.x, num4, result.surfacePoint.z), 0.001f, 12, 12, Color.yellow, false, DebugUtil.Style.SolidColor);
				}
			}
			return flag;
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x0019967C File Offset: 0x0019787C
		private bool HitOutsideSurfaceOfMesh(Vector3 castDir, MeshCollider meshCollider, RaycastHit hit)
		{
			if (!WaterVolume.meshTrianglesDict.TryGetValue(meshCollider.sharedMesh, out this.sharedMeshTris))
			{
				this.sharedMeshTris = (int[])meshCollider.sharedMesh.triangles.Clone();
				WaterVolume.meshTrianglesDict.Add(meshCollider.sharedMesh, this.sharedMeshTris);
			}
			if (!WaterVolume.meshVertsDict.TryGetValue(meshCollider.sharedMesh, out this.sharedMeshVerts))
			{
				this.sharedMeshVerts = (Vector3[])meshCollider.sharedMesh.vertices.Clone();
				WaterVolume.meshVertsDict.Add(meshCollider.sharedMesh, this.sharedMeshVerts);
			}
			Vector3 b = this.sharedMeshVerts[this.sharedMeshTris[hit.triangleIndex * 3]];
			Vector3 a = this.sharedMeshVerts[this.sharedMeshTris[hit.triangleIndex * 3 + 1]];
			Vector3 a2 = this.sharedMeshVerts[this.sharedMeshTris[hit.triangleIndex * 3 + 2]];
			Vector3 vector = meshCollider.transform.TransformDirection(Vector3.Cross(a - b, a2 - b).normalized);
			bool flag = Vector3.Dot(castDir, vector) < 0f;
			if (this.debugDrawSurfaceCast)
			{
				Color color = flag ? Color.blue : Color.red;
				DebugUtil.DrawLine(hit.point, hit.point + vector * 0.3f, color, false);
			}
			return flag;
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x001997F0 File Offset: 0x001979F0
		private void DebugDrawMeshColliderHitTriangle(RaycastHit hit)
		{
			MeshCollider meshCollider = hit.collider as MeshCollider;
			if (meshCollider != null)
			{
				Mesh sharedMesh = meshCollider.sharedMesh;
				int[] triangles = sharedMesh.triangles;
				Vector3[] vertices = sharedMesh.vertices;
				Vector3 vector = meshCollider.gameObject.transform.TransformPoint(vertices[triangles[hit.triangleIndex * 3]]);
				Vector3 vector2 = meshCollider.gameObject.transform.TransformPoint(vertices[triangles[hit.triangleIndex * 3 + 1]]);
				Vector3 vector3 = meshCollider.gameObject.transform.TransformPoint(vertices[triangles[hit.triangleIndex * 3 + 2]]);
				Vector3 normalized = Vector3.Cross(vector2 - vector, vector3 - vector).normalized;
				float d = 0.2f;
				DebugUtil.DrawLine(vector, vector + normalized * d, Color.blue, false);
				DebugUtil.DrawLine(vector2, vector2 + normalized * d, Color.blue, false);
				DebugUtil.DrawLine(vector3, vector3 + normalized * d, Color.blue, false);
				DebugUtil.DrawLine(vector, vector2, Color.blue, false);
				DebugUtil.DrawLine(vector, vector3, Color.blue, false);
				DebugUtil.DrawLine(vector2, vector3, Color.blue, false);
			}
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x0005FD98 File Offset: 0x0005DF98
		public bool RaycastWater(Vector3 origin, Vector3 direction, out RaycastHit hit, float distance, int layerMask)
		{
			if (this.triggerCollider != null)
			{
				return Physics.Raycast(new Ray(origin, direction), out hit, distance, layerMask, QueryTriggerInteraction.Collide);
			}
			hit = default(RaycastHit);
			return false;
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x0019993C File Offset: 0x00197B3C
		public bool CheckColliderInVolume(Collider collider, out bool inWater, out bool surfaceDetected)
		{
			for (int i = 0; i < this.persistentColliders.Count; i++)
			{
				if (this.persistentColliders[i].collider == collider)
				{
					inWater = this.persistentColliders[i].inWater;
					surfaceDetected = this.persistentColliders[i].surfaceDetected;
					return true;
				}
			}
			inWater = false;
			surfaceDetected = false;
			return false;
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x0005FDC3 File Offset: 0x0005DFC3
		protected override void Awake()
		{
			base.Awake();
			this.RefreshColliders();
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x001999A8 File Offset: 0x00197BA8
		public void RefreshColliders()
		{
			this.triggerCollider = base.GetComponent<Collider>();
			if (this.volumeColliders == null || this.volumeColliders.Count < 1)
			{
				this.volumeColliders = new List<Collider>();
				this.volumeColliders.Add(base.gameObject.GetComponent<Collider>());
			}
			float num = float.MinValue;
			float num2 = float.MaxValue;
			for (int i = 0; i < this.volumeColliders.Count; i++)
			{
				float y = this.volumeColliders[i].bounds.max.y;
				float y2 = this.volumeColliders[i].bounds.min.y;
				if (y > num)
				{
					num = y;
				}
				if (y2 < num2)
				{
					num2 = y2;
				}
			}
			this.volumeMaxHeight = num;
			this.volumeMinHeight = num2;
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x00199A78 File Offset: 0x00197C78
		private void OnDisable()
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			for (int i = 0; i < this.persistentColliders.Count; i++)
			{
				WaterOverlappingCollider waterOverlappingCollider = this.persistentColliders[i];
				waterOverlappingCollider.inVolume = false;
				waterOverlappingCollider.playDripEffect = false;
				WaterVolume.WaterVolumeEvent colliderExitedVolume = this.ColliderExitedVolume;
				if (colliderExitedVolume != null)
				{
					colliderExitedVolume(this, waterOverlappingCollider.collider);
				}
				this.persistentColliders[i] = waterOverlappingCollider;
			}
			this.RemoveCollidersOutsideVolume(Time.time);
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x00199AF0 File Offset: 0x00197CF0
		private void Update()
		{
			if (this.persistentColliders.Count < 1)
			{
				return;
			}
			float time = Time.time;
			this.RemoveCollidersOutsideVolume(time);
			for (int i = 0; i < this.persistentColliders.Count; i++)
			{
				WaterOverlappingCollider waterOverlappingCollider = this.persistentColliders[i];
				bool inWater = waterOverlappingCollider.inWater;
				if (waterOverlappingCollider.inVolume)
				{
					this.CheckColliderAgainstWater(ref waterOverlappingCollider, time);
				}
				else
				{
					waterOverlappingCollider.inWater = false;
				}
				this.TryRegisterOwnershipOfCollider(waterOverlappingCollider.collider, waterOverlappingCollider.inWater, waterOverlappingCollider.surfaceDetected);
				if (waterOverlappingCollider.inWater && !inWater)
				{
					this.OnWaterSurfaceEnter(ref waterOverlappingCollider);
				}
				else if (!waterOverlappingCollider.inWater && inWater)
				{
					this.OnWaterSurfaceExit(ref waterOverlappingCollider, time);
				}
				if (this.HasOwnershipOfCollider(waterOverlappingCollider.collider) && waterOverlappingCollider.surfaceDetected)
				{
					if (!waterOverlappingCollider.inWater)
					{
						this.ColliderOutOfWaterUpdate(ref waterOverlappingCollider, time);
					}
					else
					{
						this.ColliderInWaterUpdate(ref waterOverlappingCollider, time);
					}
				}
				this.persistentColliders[i] = waterOverlappingCollider;
			}
		}

		// Token: 0x0600499A RID: 18842 RVA: 0x00199BE8 File Offset: 0x00197DE8
		private void RemoveCollidersOutsideVolume(float currentTime)
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			for (int i = this.persistentColliders.Count - 1; i >= 0; i--)
			{
				WaterOverlappingCollider waterOverlappingCollider = this.persistentColliders[i];
				if (waterOverlappingCollider.collider == null || !waterOverlappingCollider.collider.gameObject.activeInHierarchy || (!waterOverlappingCollider.inVolume && (!waterOverlappingCollider.playDripEffect || currentTime - waterOverlappingCollider.lastInWaterTime > this.waterParams.postExitDripDuration)))
				{
					this.UnregisterOwnershipOfCollider(waterOverlappingCollider.collider);
					GTPlayer instance = GTPlayer.Instance;
					if (waterOverlappingCollider.collider == instance.headCollider || waterOverlappingCollider.collider == instance.bodyCollider)
					{
						instance.OnExitWaterVolume(waterOverlappingCollider.collider, this);
					}
					this.persistentColliders.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600499B RID: 18843 RVA: 0x00199CC0 File Offset: 0x00197EC0
		private void CheckColliderAgainstWater(ref WaterOverlappingCollider persistentCollider, float currentTime)
		{
			Vector3 position = persistentCollider.collider.transform.position;
			bool flag = true;
			if (persistentCollider.surfaceDetected && persistentCollider.scaleMultiplier > 0.99f && this.isStationary)
			{
				flag = ((position - Vector3.Dot(position - persistentCollider.lastSurfaceQuery.surfacePoint, persistentCollider.lastSurfaceQuery.surfaceNormal) * persistentCollider.lastSurfaceQuery.surfaceNormal - persistentCollider.lastSurfaceQuery.surfacePoint).sqrMagnitude > this.waterParams.recomputeSurfaceForColliderDist * this.waterParams.recomputeSurfaceForColliderDist);
			}
			if (flag)
			{
				WaterVolume.SurfaceQuery lastSurfaceQuery;
				if (this.GetSurfaceQueryForPoint(position, out lastSurfaceQuery, this.debugDrawSurfaceCast))
				{
					persistentCollider.surfaceDetected = true;
					persistentCollider.lastSurfaceQuery = lastSurfaceQuery;
				}
				else
				{
					persistentCollider.surfaceDetected = false;
					persistentCollider.lastSurfaceQuery = default(WaterVolume.SurfaceQuery);
				}
			}
			if (persistentCollider.surfaceDetected)
			{
				bool flag2 = ((persistentCollider.collider is MeshCollider) ? persistentCollider.collider.ClosestPointOnBounds(position + Vector3.down * 10f) : persistentCollider.collider.ClosestPoint(position + Vector3.down * 10f)).y < persistentCollider.lastSurfaceQuery.surfacePoint.y;
				bool flag3 = ((persistentCollider.collider is MeshCollider) ? persistentCollider.collider.ClosestPointOnBounds(position + Vector3.up * 10f) : persistentCollider.collider.ClosestPoint(position + Vector3.up * 10f)).y > persistentCollider.lastSurfaceQuery.surfacePoint.y - persistentCollider.lastSurfaceQuery.maxDepth;
				persistentCollider.inWater = (flag2 && flag3);
			}
			else
			{
				persistentCollider.inWater = false;
			}
			if (persistentCollider.inWater)
			{
				persistentCollider.lastInWaterTime = currentTime;
			}
		}

		// Token: 0x0600499C RID: 18844 RVA: 0x00199EA8 File Offset: 0x001980A8
		private Vector3 GetColliderVelocity(ref WaterOverlappingCollider persistentCollider)
		{
			GTPlayer instance = GTPlayer.Instance;
			Vector3 result = Vector3.one * (this.waterParams.splashSpeedRequirement + 0.1f);
			if (persistentCollider.velocityTracker != null)
			{
				result = persistentCollider.velocityTracker.GetAverageVelocity(true, 0.1f, false);
			}
			else if (persistentCollider.collider == instance.headCollider || persistentCollider.collider == instance.bodyCollider)
			{
				result = instance.AveragedVelocity;
			}
			else if (persistentCollider.collider.attachedRigidbody != null && !persistentCollider.collider.attachedRigidbody.isKinematic)
			{
				result = persistentCollider.collider.attachedRigidbody.velocity;
			}
			return result;
		}

		// Token: 0x0600499D RID: 18845 RVA: 0x00199F60 File Offset: 0x00198160
		private void OnWaterSurfaceEnter(ref WaterOverlappingCollider persistentCollider)
		{
			WaterVolume.WaterVolumeEvent colliderEnteredWater = this.ColliderEnteredWater;
			if (colliderEnteredWater != null)
			{
				colliderEnteredWater(this, persistentCollider.collider);
			}
			GTPlayer instance = GTPlayer.Instance;
			if (persistentCollider.collider == instance.headCollider || persistentCollider.collider == instance.bodyCollider)
			{
				instance.OnEnterWaterVolume(persistentCollider.collider, this);
			}
			if (this.HasOwnershipOfCollider(persistentCollider.collider))
			{
				Vector3 colliderVelocity = this.GetColliderVelocity(ref persistentCollider);
				bool flag = Vector3.Dot(colliderVelocity, -persistentCollider.lastSurfaceQuery.surfaceNormal) > this.waterParams.splashSpeedRequirement * persistentCollider.scaleMultiplier;
				bool flag2 = Vector3.Dot(colliderVelocity, -persistentCollider.lastSurfaceQuery.surfaceNormal) > this.waterParams.bigSplashSpeedRequirement * persistentCollider.scaleMultiplier;
				persistentCollider.PlayRippleEffect(this.waterParams.rippleEffect, persistentCollider.lastSurfaceQuery.surfacePoint, persistentCollider.lastSurfaceQuery.surfaceNormal, this.waterParams.rippleEffectScale, Time.time, this);
				if (this.waterParams.playSplashEffect && flag && (flag2 || !persistentCollider.playBigSplash))
				{
					persistentCollider.PlaySplashEffect(this.waterParams.splashEffect, persistentCollider.lastRipplePosition, this.waterParams.splashEffectScale, persistentCollider.playBigSplash && flag2, true, this);
				}
			}
		}

		// Token: 0x0600499E RID: 18846 RVA: 0x0019A0AC File Offset: 0x001982AC
		private void OnWaterSurfaceExit(ref WaterOverlappingCollider persistentCollider, float currentTime)
		{
			WaterVolume.WaterVolumeEvent colliderExitedWater = this.ColliderExitedWater;
			if (colliderExitedWater != null)
			{
				colliderExitedWater(this, persistentCollider.collider);
			}
			persistentCollider.nextDripTime = currentTime + this.waterParams.perDripTimeDelay + UnityEngine.Random.Range(-this.waterParams.perDripTimeRandRange * 0.5f, this.waterParams.perDripTimeRandRange * 0.5f);
			GTPlayer instance = GTPlayer.Instance;
			if (persistentCollider.collider == instance.headCollider || persistentCollider.collider == instance.bodyCollider)
			{
				instance.OnExitWaterVolume(persistentCollider.collider, this);
			}
			if (this.HasOwnershipOfCollider(persistentCollider.collider))
			{
				float num = Vector3.Dot(this.GetColliderVelocity(ref persistentCollider), persistentCollider.lastSurfaceQuery.surfaceNormal);
				bool flag = num > this.waterParams.splashSpeedRequirement * persistentCollider.scaleMultiplier;
				bool flag2 = num > this.waterParams.bigSplashSpeedRequirement * persistentCollider.scaleMultiplier;
				persistentCollider.PlayRippleEffect(this.waterParams.rippleEffect, persistentCollider.lastSurfaceQuery.surfacePoint, persistentCollider.lastSurfaceQuery.surfaceNormal, this.waterParams.rippleEffectScale, Time.time, this);
				if (this.waterParams.playSplashEffect && flag && (flag2 || !persistentCollider.playBigSplash))
				{
					persistentCollider.PlaySplashEffect(this.waterParams.splashEffect, persistentCollider.lastRipplePosition, this.waterParams.splashEffectScale, persistentCollider.playBigSplash && flag2, false, this);
				}
			}
		}

		// Token: 0x0600499F RID: 18847 RVA: 0x0019A218 File Offset: 0x00198418
		private void ColliderOutOfWaterUpdate(ref WaterOverlappingCollider persistentCollider, float currentTime)
		{
			if (currentTime < persistentCollider.lastInWaterTime + this.waterParams.postExitDripDuration && currentTime > persistentCollider.nextDripTime && persistentCollider.playDripEffect)
			{
				persistentCollider.nextDripTime = currentTime + this.waterParams.perDripTimeDelay + UnityEngine.Random.Range(-this.waterParams.perDripTimeRandRange * 0.5f, this.waterParams.perDripTimeRandRange * 0.5f);
				float dripScale = this.waterParams.rippleEffectScale * 2f * (this.waterParams.perDripDefaultRadius + UnityEngine.Random.Range(-this.waterParams.perDripRadiusRandRange * 0.5f, this.waterParams.perDripRadiusRandRange * 0.5f));
				persistentCollider.PlayDripEffect(this.waterParams.rippleEffect, persistentCollider.lastSurfaceQuery.surfacePoint, persistentCollider.lastSurfaceQuery.surfaceNormal, dripScale);
			}
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x0019A300 File Offset: 0x00198500
		private void ColliderInWaterUpdate(ref WaterOverlappingCollider persistentCollider, float currentTime)
		{
			Vector3 vector = Vector3.ProjectOnPlane(persistentCollider.collider.transform.position - persistentCollider.lastSurfaceQuery.surfacePoint, persistentCollider.lastSurfaceQuery.surfaceNormal) + persistentCollider.lastSurfaceQuery.surfacePoint;
			bool flag;
			if (persistentCollider.overrideBoundingRadius)
			{
				flag = ((persistentCollider.collider.transform.position - vector).sqrMagnitude < persistentCollider.boundingRadiusOverride * persistentCollider.boundingRadiusOverride);
			}
			else
			{
				flag = ((persistentCollider.collider.ClosestPointOnBounds(vector) - vector).sqrMagnitude < 0.001f);
			}
			if (flag)
			{
				float num = Mathf.Max(this.waterParams.minDistanceBetweenRipples, this.waterParams.defaultDistanceBetweenRipples * (persistentCollider.lastRippleScale / this.waterParams.rippleEffectScale));
				bool flag2 = (persistentCollider.lastRipplePosition - vector).sqrMagnitude > num * num;
				bool flag3 = currentTime - persistentCollider.lastRippleTime > this.waterParams.minTimeBetweenRipples;
				if (flag2 || flag3)
				{
					persistentCollider.PlayRippleEffect(this.waterParams.rippleEffect, persistentCollider.lastSurfaceQuery.surfacePoint, persistentCollider.lastSurfaceQuery.surfaceNormal, this.waterParams.rippleEffectScale, currentTime, this);
					return;
				}
			}
			else
			{
				persistentCollider.lastRippleTime = currentTime;
			}
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x0019A450 File Offset: 0x00198650
		private void TryRegisterOwnershipOfCollider(Collider collider, bool isInWater, bool isSurfaceDetected)
		{
			WaterVolume waterVolume;
			if (WaterVolume.sharedColliderRegistry.TryGetValue(collider, out waterVolume))
			{
				if (waterVolume != this)
				{
					bool flag;
					bool flag2;
					waterVolume.CheckColliderInVolume(collider, out flag, out flag2);
					if ((isSurfaceDetected && !flag2) || (isInWater && !flag))
					{
						WaterVolume.sharedColliderRegistry.Remove(collider);
						WaterVolume.sharedColliderRegistry.Add(collider, this);
						return;
					}
				}
			}
			else
			{
				WaterVolume.sharedColliderRegistry.Add(collider, this);
			}
		}

		// Token: 0x060049A2 RID: 18850 RVA: 0x0005FDD1 File Offset: 0x0005DFD1
		private void UnregisterOwnershipOfCollider(Collider collider)
		{
			if (WaterVolume.sharedColliderRegistry.ContainsKey(collider))
			{
				WaterVolume.sharedColliderRegistry.Remove(collider);
			}
		}

		// Token: 0x060049A3 RID: 18851 RVA: 0x0019A4B4 File Offset: 0x001986B4
		private bool HasOwnershipOfCollider(Collider collider)
		{
			WaterVolume x;
			return WaterVolume.sharedColliderRegistry.TryGetValue(collider, out x) && x == this;
		}

		// Token: 0x060049A4 RID: 18852 RVA: 0x0019A4DC File Offset: 0x001986DC
		public void OnTriggerEnter(Collider other)
		{
			GorillaTriggerColliderHandIndicator component = other.GetComponent<GorillaTriggerColliderHandIndicator>();
			if (other.isTrigger && component == null)
			{
				return;
			}
			WaterVolume.WaterVolumeEvent colliderEnteredVolume = this.ColliderEnteredVolume;
			if (colliderEnteredVolume != null)
			{
				colliderEnteredVolume(this, other);
			}
			for (int i = 0; i < this.persistentColliders.Count; i++)
			{
				if (this.persistentColliders[i].collider == other)
				{
					WaterOverlappingCollider value = this.persistentColliders[i];
					value.inVolume = true;
					this.persistentColliders[i] = value;
					return;
				}
			}
			WaterOverlappingCollider waterOverlappingCollider = new WaterOverlappingCollider
			{
				collider = other
			};
			waterOverlappingCollider.inVolume = true;
			waterOverlappingCollider.lastInWaterTime = Time.time - this.waterParams.postExitDripDuration - 10f;
			WaterSplashOverride component2 = other.GetComponent<WaterSplashOverride>();
			if (component2 != null)
			{
				if (component2.suppressWaterEffects)
				{
					return;
				}
				waterOverlappingCollider.playBigSplash = component2.playBigSplash;
				waterOverlappingCollider.playDripEffect = component2.playDrippingEffect;
				waterOverlappingCollider.overrideBoundingRadius = component2.overrideBoundingRadius;
				waterOverlappingCollider.boundingRadiusOverride = component2.boundingRadiusOverride;
				waterOverlappingCollider.scaleMultiplier = (component2.scaleByPlayersScale ? GTPlayer.Instance.scale : 1f);
			}
			else
			{
				waterOverlappingCollider.playDripEffect = true;
				waterOverlappingCollider.overrideBoundingRadius = false;
				waterOverlappingCollider.scaleMultiplier = 1f;
				waterOverlappingCollider.playBigSplash = false;
			}
			GTPlayer instance = GTPlayer.Instance;
			if (component != null)
			{
				waterOverlappingCollider.velocityTracker = (component.isLeftHand ? instance.leftHandCenterVelocityTracker : instance.rightHandCenterVelocityTracker);
				waterOverlappingCollider.scaleMultiplier = instance.scale;
			}
			else
			{
				waterOverlappingCollider.velocityTracker = other.GetComponent<GorillaVelocityTracker>();
			}
			if (this.PlayerVRRig != null && this.waterParams.sendSplashEffectRPCs && (component != null || waterOverlappingCollider.collider == instance.headCollider || waterOverlappingCollider.collider == instance.bodyCollider))
			{
				waterOverlappingCollider.photonViewForRPC = this.PlayerVRRig.netView;
			}
			this.persistentColliders.Add(waterOverlappingCollider);
		}

		// Token: 0x060049A5 RID: 18853 RVA: 0x0019A6EC File Offset: 0x001988EC
		private void OnTriggerExit(Collider other)
		{
			GorillaTriggerColliderHandIndicator component = other.GetComponent<GorillaTriggerColliderHandIndicator>();
			if (other.isTrigger && component == null)
			{
				return;
			}
			WaterVolume.WaterVolumeEvent colliderExitedVolume = this.ColliderExitedVolume;
			if (colliderExitedVolume != null)
			{
				colliderExitedVolume(this, other);
			}
			for (int i = 0; i < this.persistentColliders.Count; i++)
			{
				if (this.persistentColliders[i].collider == other)
				{
					WaterOverlappingCollider value = this.persistentColliders[i];
					value.inVolume = false;
					this.persistentColliders[i] = value;
				}
			}
		}

		// Token: 0x060049A6 RID: 18854 RVA: 0x0005FDEC File Offset: 0x0005DFEC
		public void SetPropertiesFromPlaceholder(WaterVolumeProperties properties, List<Collider> waterVolumeColliders, WaterParameters parameters)
		{
			this.surfacePlane = properties.surfacePlane;
			this.surfaceColliders = properties.surfaceColliders;
			this.volumeColliders = waterVolumeColliders;
			this.liquidType = (GTPlayer.LiquidType)Math.Clamp(properties.liquidType - CMSZoneShaderSettings.EZoneLiquidType.Water, 0, 1);
			this.waterParams = parameters;
		}

		// Token: 0x04004BD8 RID: 19416
		[SerializeField]
		public Transform surfacePlane;

		// Token: 0x04004BD9 RID: 19417
		[SerializeField]
		private List<MeshCollider> surfaceColliders = new List<MeshCollider>();

		// Token: 0x04004BDA RID: 19418
		[SerializeField]
		public List<Collider> volumeColliders = new List<Collider>();

		// Token: 0x04004BDB RID: 19419
		[SerializeField]
		private GTPlayer.LiquidType liquidType;

		// Token: 0x04004BDC RID: 19420
		[SerializeField]
		private WaterCurrent waterCurrent;

		// Token: 0x04004BDD RID: 19421
		[SerializeField]
		private WaterParameters waterParams;

		// Token: 0x04004BDE RID: 19422
		[SerializeField]
		public bool isStationary = true;

		// Token: 0x04004BDF RID: 19423
		public const string WaterSplashRPC = "RPC_PlaySplashEffect";

		// Token: 0x04004BE0 RID: 19424
		public static float[] splashRPCSendTimes = new float[4];

		// Token: 0x04004BE1 RID: 19425
		private static Dictionary<Collider, WaterVolume> sharedColliderRegistry = new Dictionary<Collider, WaterVolume>(16);

		// Token: 0x04004BE2 RID: 19426
		private static Dictionary<Mesh, int[]> meshTrianglesDict = new Dictionary<Mesh, int[]>(16);

		// Token: 0x04004BE3 RID: 19427
		private static Dictionary<Mesh, Vector3[]> meshVertsDict = new Dictionary<Mesh, Vector3[]>(16);

		// Token: 0x04004BE4 RID: 19428
		private int[] sharedMeshTris;

		// Token: 0x04004BE5 RID: 19429
		private Vector3[] sharedMeshVerts;

		// Token: 0x04004BEA RID: 19434
		private VRRig playerVRRig;

		// Token: 0x04004BEB RID: 19435
		private float volumeMaxHeight;

		// Token: 0x04004BEC RID: 19436
		private float volumeMinHeight;

		// Token: 0x04004BED RID: 19437
		private bool debugDrawSurfaceCast;

		// Token: 0x04004BEE RID: 19438
		private Collider triggerCollider;

		// Token: 0x04004BEF RID: 19439
		private List<WaterOverlappingCollider> persistentColliders = new List<WaterOverlappingCollider>(16);

		// Token: 0x04004BF0 RID: 19440
		private GuidedRefTargetIdSO _guidedRefTargetId;

		// Token: 0x04004BF1 RID: 19441
		private UnityEngine.Object _guidedRefTargetObject;

		// Token: 0x02000B79 RID: 2937
		public struct SurfaceQuery
		{
			// Token: 0x170007BA RID: 1978
			// (get) Token: 0x060049A9 RID: 18857 RVA: 0x0005FE8C File Offset: 0x0005E08C
			public Plane surfacePlane
			{
				get
				{
					return new Plane(this.surfaceNormal, this.surfacePoint);
				}
			}

			// Token: 0x04004BF2 RID: 19442
			public Vector3 surfacePoint;

			// Token: 0x04004BF3 RID: 19443
			public Vector3 surfaceNormal;

			// Token: 0x04004BF4 RID: 19444
			public float maxDepth;
		}

		// Token: 0x02000B7A RID: 2938
		// (Invoke) Token: 0x060049AB RID: 18859
		public delegate void WaterVolumeEvent(WaterVolume volume, Collider collider);
	}
}
