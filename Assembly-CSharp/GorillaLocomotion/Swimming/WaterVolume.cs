using System;
using System.Collections.Generic;
using CjLib;
using GorillaLocomotion.Climbing;
using GorillaTag.GuidedRefs;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B4B RID: 2891
	[RequireComponent(typeof(Collider))]
	public class WaterVolume : BaseGuidedRefTargetMono
	{
		// Token: 0x14000082 RID: 130
		// (add) Token: 0x0600483C RID: 18492 RVA: 0x0015D638 File Offset: 0x0015B838
		// (remove) Token: 0x0600483D RID: 18493 RVA: 0x0015D670 File Offset: 0x0015B870
		public event WaterVolume.WaterVolumeEvent ColliderEnteredVolume;

		// Token: 0x14000083 RID: 131
		// (add) Token: 0x0600483E RID: 18494 RVA: 0x0015D6A8 File Offset: 0x0015B8A8
		// (remove) Token: 0x0600483F RID: 18495 RVA: 0x0015D6E0 File Offset: 0x0015B8E0
		public event WaterVolume.WaterVolumeEvent ColliderExitedVolume;

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06004840 RID: 18496 RVA: 0x0015D718 File Offset: 0x0015B918
		// (remove) Token: 0x06004841 RID: 18497 RVA: 0x0015D750 File Offset: 0x0015B950
		public event WaterVolume.WaterVolumeEvent ColliderEnteredWater;

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x06004842 RID: 18498 RVA: 0x0015D788 File Offset: 0x0015B988
		// (remove) Token: 0x06004843 RID: 18499 RVA: 0x0015D7C0 File Offset: 0x0015B9C0
		public event WaterVolume.WaterVolumeEvent ColliderExitedWater;

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06004844 RID: 18500 RVA: 0x0015D7F5 File Offset: 0x0015B9F5
		public GTPlayer.LiquidType LiquidType
		{
			get
			{
				return this.liquidType;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06004845 RID: 18501 RVA: 0x0015D7FD File Offset: 0x0015B9FD
		public WaterCurrent Current
		{
			get
			{
				return this.waterCurrent;
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06004846 RID: 18502 RVA: 0x0015D805 File Offset: 0x0015BA05
		public WaterParameters Parameters
		{
			get
			{
				return this.waterParams;
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06004847 RID: 18503 RVA: 0x0015D810 File Offset: 0x0015BA10
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

		// Token: 0x06004848 RID: 18504 RVA: 0x0015D84C File Offset: 0x0015BA4C
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

		// Token: 0x06004849 RID: 18505 RVA: 0x0015DB90 File Offset: 0x0015BD90
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

		// Token: 0x0600484A RID: 18506 RVA: 0x0015DD04 File Offset: 0x0015BF04
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

		// Token: 0x0600484B RID: 18507 RVA: 0x0015DE50 File Offset: 0x0015C050
		public bool RaycastWater(Vector3 origin, Vector3 direction, out RaycastHit hit, float distance, int layerMask)
		{
			if (this.triggerCollider != null)
			{
				return Physics.Raycast(new Ray(origin, direction), out hit, distance, layerMask, QueryTriggerInteraction.Collide);
			}
			hit = default(RaycastHit);
			return false;
		}

		// Token: 0x0600484C RID: 18508 RVA: 0x0015DE7C File Offset: 0x0015C07C
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

		// Token: 0x0600484D RID: 18509 RVA: 0x0015DEE8 File Offset: 0x0015C0E8
		protected override void Awake()
		{
			base.Awake();
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

		// Token: 0x0600484E RID: 18510 RVA: 0x0015DFC0 File Offset: 0x0015C1C0
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

		// Token: 0x0600484F RID: 18511 RVA: 0x0015E038 File Offset: 0x0015C238
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

		// Token: 0x06004850 RID: 18512 RVA: 0x0015E130 File Offset: 0x0015C330
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

		// Token: 0x06004851 RID: 18513 RVA: 0x0015E208 File Offset: 0x0015C408
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

		// Token: 0x06004852 RID: 18514 RVA: 0x0015E3F0 File Offset: 0x0015C5F0
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

		// Token: 0x06004853 RID: 18515 RVA: 0x0015E4A8 File Offset: 0x0015C6A8
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

		// Token: 0x06004854 RID: 18516 RVA: 0x0015E5F4 File Offset: 0x0015C7F4
		private void OnWaterSurfaceExit(ref WaterOverlappingCollider persistentCollider, float currentTime)
		{
			WaterVolume.WaterVolumeEvent colliderExitedWater = this.ColliderExitedWater;
			if (colliderExitedWater != null)
			{
				colliderExitedWater(this, persistentCollider.collider);
			}
			persistentCollider.nextDripTime = currentTime + this.waterParams.perDripTimeDelay + Random.Range(-this.waterParams.perDripTimeRandRange * 0.5f, this.waterParams.perDripTimeRandRange * 0.5f);
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

		// Token: 0x06004855 RID: 18517 RVA: 0x0015E760 File Offset: 0x0015C960
		private void ColliderOutOfWaterUpdate(ref WaterOverlappingCollider persistentCollider, float currentTime)
		{
			if (currentTime < persistentCollider.lastInWaterTime + this.waterParams.postExitDripDuration && currentTime > persistentCollider.nextDripTime && persistentCollider.playDripEffect)
			{
				persistentCollider.nextDripTime = currentTime + this.waterParams.perDripTimeDelay + Random.Range(-this.waterParams.perDripTimeRandRange * 0.5f, this.waterParams.perDripTimeRandRange * 0.5f);
				float dripScale = this.waterParams.rippleEffectScale * 2f * (this.waterParams.perDripDefaultRadius + Random.Range(-this.waterParams.perDripRadiusRandRange * 0.5f, this.waterParams.perDripRadiusRandRange * 0.5f));
				persistentCollider.PlayDripEffect(this.waterParams.rippleEffect, persistentCollider.lastSurfaceQuery.surfacePoint, persistentCollider.lastSurfaceQuery.surfaceNormal, dripScale);
			}
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x0015E848 File Offset: 0x0015CA48
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

		// Token: 0x06004857 RID: 18519 RVA: 0x0015E998 File Offset: 0x0015CB98
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

		// Token: 0x06004858 RID: 18520 RVA: 0x0015E9FA File Offset: 0x0015CBFA
		private void UnregisterOwnershipOfCollider(Collider collider)
		{
			if (WaterVolume.sharedColliderRegistry.ContainsKey(collider))
			{
				WaterVolume.sharedColliderRegistry.Remove(collider);
			}
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x0015EA18 File Offset: 0x0015CC18
		private bool HasOwnershipOfCollider(Collider collider)
		{
			WaterVolume x;
			return WaterVolume.sharedColliderRegistry.TryGetValue(collider, out x) && x == this;
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x0015EA40 File Offset: 0x0015CC40
		private void OnTriggerEnter(Collider other)
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

		// Token: 0x0600485B RID: 18523 RVA: 0x0015EC50 File Offset: 0x0015CE50
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

		// Token: 0x04004AE2 RID: 19170
		[SerializeField]
		public Transform surfacePlane;

		// Token: 0x04004AE3 RID: 19171
		[SerializeField]
		private List<MeshCollider> surfaceColliders = new List<MeshCollider>();

		// Token: 0x04004AE4 RID: 19172
		[SerializeField]
		public List<Collider> volumeColliders = new List<Collider>();

		// Token: 0x04004AE5 RID: 19173
		[SerializeField]
		private GTPlayer.LiquidType liquidType;

		// Token: 0x04004AE6 RID: 19174
		[SerializeField]
		private WaterCurrent waterCurrent;

		// Token: 0x04004AE7 RID: 19175
		[SerializeField]
		private WaterParameters waterParams;

		// Token: 0x04004AE8 RID: 19176
		[SerializeField]
		public bool isStationary = true;

		// Token: 0x04004AE9 RID: 19177
		public const string WaterSplashRPC = "RPC_PlaySplashEffect";

		// Token: 0x04004AEA RID: 19178
		public static float[] splashRPCSendTimes = new float[4];

		// Token: 0x04004AEB RID: 19179
		private static Dictionary<Collider, WaterVolume> sharedColliderRegistry = new Dictionary<Collider, WaterVolume>(16);

		// Token: 0x04004AEC RID: 19180
		private static Dictionary<Mesh, int[]> meshTrianglesDict = new Dictionary<Mesh, int[]>(16);

		// Token: 0x04004AED RID: 19181
		private static Dictionary<Mesh, Vector3[]> meshVertsDict = new Dictionary<Mesh, Vector3[]>(16);

		// Token: 0x04004AEE RID: 19182
		private int[] sharedMeshTris;

		// Token: 0x04004AEF RID: 19183
		private Vector3[] sharedMeshVerts;

		// Token: 0x04004AF4 RID: 19188
		private VRRig playerVRRig;

		// Token: 0x04004AF5 RID: 19189
		private float volumeMaxHeight;

		// Token: 0x04004AF6 RID: 19190
		private float volumeMinHeight;

		// Token: 0x04004AF7 RID: 19191
		private bool debugDrawSurfaceCast;

		// Token: 0x04004AF8 RID: 19192
		private Collider triggerCollider;

		// Token: 0x04004AF9 RID: 19193
		private List<WaterOverlappingCollider> persistentColliders = new List<WaterOverlappingCollider>(16);

		// Token: 0x04004AFA RID: 19194
		private GuidedRefTargetIdSO _guidedRefTargetId;

		// Token: 0x04004AFB RID: 19195
		private Object _guidedRefTargetObject;

		// Token: 0x02000B4C RID: 2892
		public struct SurfaceQuery
		{
			// Token: 0x1700079E RID: 1950
			// (get) Token: 0x0600485E RID: 18526 RVA: 0x0015ED3D File Offset: 0x0015CF3D
			public Plane surfacePlane
			{
				get
				{
					return new Plane(this.surfaceNormal, this.surfacePoint);
				}
			}

			// Token: 0x04004AFC RID: 19196
			public Vector3 surfacePoint;

			// Token: 0x04004AFD RID: 19197
			public Vector3 surfaceNormal;

			// Token: 0x04004AFE RID: 19198
			public float maxDepth;
		}

		// Token: 0x02000B4D RID: 2893
		// (Invoke) Token: 0x06004860 RID: 18528
		public delegate void WaterVolumeEvent(WaterVolume volume, Collider collider);
	}
}
