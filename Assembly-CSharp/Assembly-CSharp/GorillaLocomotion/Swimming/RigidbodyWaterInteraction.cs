using System;
using System.Collections.Generic;
using AA;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B45 RID: 2885
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyWaterInteraction : MonoBehaviour
	{
		// Token: 0x0600481A RID: 18458 RVA: 0x0015BC6A File Offset: 0x00159E6A
		protected void Awake()
		{
			this.rb = base.GetComponent<Rigidbody>();
			this.baseAngularDrag = this.rb.angularDrag;
			RigidbodyWaterInteractionManager.RegisterRBWI(this);
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x0015BC8F File Offset: 0x00159E8F
		protected void OnEnable()
		{
			this.overlappingWaterVolumes.Clear();
			RigidbodyWaterInteractionManager.RegisterRBWI(this);
		}

		// Token: 0x0600481C RID: 18460 RVA: 0x0015BCA2 File Offset: 0x00159EA2
		protected void OnDisable()
		{
			this.overlappingWaterVolumes.Clear();
			RigidbodyWaterInteractionManager.UnregisterRBWI(this);
		}

		// Token: 0x0600481D RID: 18461 RVA: 0x0015BCB5 File Offset: 0x00159EB5
		private void OnDestroy()
		{
			RigidbodyWaterInteractionManager.UnregisterRBWI(this);
		}

		// Token: 0x0600481E RID: 18462 RVA: 0x0015BCC0 File Offset: 0x00159EC0
		public void InvokeFixedUpdate()
		{
			if (this.rb.isKinematic)
			{
				return;
			}
			bool flag = this.overlappingWaterVolumes.Count > 0;
			WaterVolume.SurfaceQuery surfaceQuery = default(WaterVolume.SurfaceQuery);
			float num = float.MinValue;
			if (flag && this.enablePreciseWaterCollision)
			{
				Vector3 vector = base.transform.position + Vector3.down * 2f * this.objectRadiusForWaterCollision * this.buoyancyEquilibrium;
				bool flag2 = false;
				this.activeWaterCurrents.Clear();
				for (int i = 0; i < this.overlappingWaterVolumes.Count; i++)
				{
					WaterVolume.SurfaceQuery surfaceQuery2;
					if (this.overlappingWaterVolumes[i].GetSurfaceQueryForPoint(vector, out surfaceQuery2, false))
					{
						float num2 = Vector3.Dot(surfaceQuery2.surfacePoint - vector, surfaceQuery2.surfaceNormal);
						if (num2 > num)
						{
							num = num2;
							surfaceQuery = surfaceQuery2;
							flag2 = true;
						}
						WaterCurrent waterCurrent = this.overlappingWaterVolumes[i].Current;
						if (this.applyWaterCurrents && waterCurrent != null && num2 > 0f && !this.activeWaterCurrents.Contains(waterCurrent))
						{
							this.activeWaterCurrents.Add(waterCurrent);
						}
					}
				}
				if (flag2)
				{
					bool flag3 = num > -(1f - this.buoyancyEquilibrium) * 2f * this.objectRadiusForWaterCollision;
					float num3 = this.enablePreciseWaterCollision ? this.objectRadiusForWaterCollision : 0f;
					bool flag4 = base.transform.position.y + num3 - (surfaceQuery.surfacePoint.y - surfaceQuery.maxDepth) > 0f;
					flag = (flag3 && flag4);
				}
				else
				{
					flag = false;
				}
			}
			if (flag)
			{
				float fixedDeltaTime = Time.fixedDeltaTime;
				Vector3 vector2 = this.rb.velocity;
				Vector3 vector3 = Vector3.zero;
				if (this.applyWaterCurrents)
				{
					Vector3 a = Vector3.zero;
					for (int j = 0; j < this.activeWaterCurrents.Count; j++)
					{
						WaterCurrent waterCurrent2 = this.activeWaterCurrents[j];
						Vector3 startingVelocity = vector2 + vector3;
						Vector3 b;
						Vector3 b2;
						if (waterCurrent2.GetCurrentAtPoint(base.transform.position, startingVelocity, fixedDeltaTime, out b, out b2))
						{
							a += b;
							vector3 += b2;
						}
					}
					if (this.enablePreciseWaterCollision)
					{
						Vector3 position = (surfaceQuery.surfacePoint + (base.transform.position + Vector3.down * this.objectRadiusForWaterCollision)) * 0.5f;
						this.rb.AddForceAtPosition(vector3, position, ForceMode.VelocityChange);
					}
					else
					{
						vector2 += vector3;
					}
				}
				if (this.applyBuoyancyForce)
				{
					Vector3 vector4 = Vector3.zero;
					if (this.enablePreciseWaterCollision)
					{
						float b3 = 2f * this.objectRadiusForWaterCollision * this.buoyancyEquilibrium;
						float d = Mathf.InverseLerp(0f, b3, num);
						vector4 = -Physics.gravity * this.underWaterBuoyancyFactor * d * fixedDeltaTime;
					}
					else
					{
						vector4 = -Physics.gravity * this.underWaterBuoyancyFactor * fixedDeltaTime;
					}
					if (vector3.sqrMagnitude > 0.001f)
					{
						float magnitude = vector3.magnitude;
						Vector3 vector5 = vector3 / magnitude;
						float num4 = Vector3.Dot(vector4, vector5);
						if (num4 < 0f)
						{
							vector4 -= num4 * vector5;
						}
					}
					vector2 += vector4;
				}
				float magnitude2 = vector2.magnitude;
				if (magnitude2 > 0.001f && this.applyDamping)
				{
					Vector3 a2 = vector2 / magnitude2;
					float num5 = Spring.DamperDecayExact(magnitude2, this.underWaterDampingHalfLife, fixedDeltaTime, 1E-05f);
					if (this.enablePreciseWaterCollision)
					{
						float a3 = Spring.DamperDecayExact(magnitude2, this.waterSurfaceDampingHalfLife, fixedDeltaTime, 1E-05f);
						float t = Mathf.Clamp(-(base.transform.position.y - surfaceQuery.surfacePoint.y) / this.objectRadiusForWaterCollision, -1f, 1f) * 0.5f + 0.5f;
						vector2 = Mathf.Lerp(a3, num5, t) * a2;
					}
					else
					{
						vector2 = num5 * a2;
					}
				}
				if (this.applySurfaceTorque && this.enablePreciseWaterCollision)
				{
					float num6 = base.transform.position.y - surfaceQuery.surfacePoint.y;
					if (num6 < this.objectRadiusForWaterCollision && num6 > 0f)
					{
						Vector3 rhs = vector2 - Vector3.Dot(vector2, surfaceQuery.surfaceNormal) * surfaceQuery.surfaceNormal;
						Vector3 normalized = Vector3.Cross(surfaceQuery.surfaceNormal, rhs).normalized;
						float num7 = Vector3.Dot(this.rb.angularVelocity, normalized);
						float num8 = rhs.magnitude / this.objectRadiusForWaterCollision - num7;
						if (num8 > 0f)
						{
							this.rb.AddTorque(this.surfaceTorqueAmount * num8 * normalized, ForceMode.Acceleration);
						}
					}
				}
				this.rb.velocity = vector2;
				this.rb.angularDrag = this.angularDrag;
				return;
			}
			this.rb.angularDrag = this.baseAngularDrag;
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x0015C1F0 File Offset: 0x0015A3F0
		protected void OnTriggerEnter(Collider other)
		{
			WaterVolume component = other.GetComponent<WaterVolume>();
			if (component != null && !this.overlappingWaterVolumes.Contains(component))
			{
				this.overlappingWaterVolumes.Add(component);
			}
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x0015C228 File Offset: 0x0015A428
		protected void OnTriggerExit(Collider other)
		{
			WaterVolume component = other.GetComponent<WaterVolume>();
			if (component != null && this.overlappingWaterVolumes.Contains(component))
			{
				this.overlappingWaterVolumes.Remove(component);
			}
		}

		// Token: 0x04004A86 RID: 19078
		public bool applyDamping = true;

		// Token: 0x04004A87 RID: 19079
		public bool applyBuoyancyForce = true;

		// Token: 0x04004A88 RID: 19080
		public bool applyAngularDrag = true;

		// Token: 0x04004A89 RID: 19081
		public bool applyWaterCurrents = true;

		// Token: 0x04004A8A RID: 19082
		public bool applySurfaceTorque = true;

		// Token: 0x04004A8B RID: 19083
		public float underWaterDampingHalfLife = 0.25f;

		// Token: 0x04004A8C RID: 19084
		public float waterSurfaceDampingHalfLife = 1f;

		// Token: 0x04004A8D RID: 19085
		public float underWaterBuoyancyFactor = 0.5f;

		// Token: 0x04004A8E RID: 19086
		public float angularDrag = 0.5f;

		// Token: 0x04004A8F RID: 19087
		public float surfaceTorqueAmount = 0.5f;

		// Token: 0x04004A90 RID: 19088
		public bool enablePreciseWaterCollision;

		// Token: 0x04004A91 RID: 19089
		public float objectRadiusForWaterCollision = 0.25f;

		// Token: 0x04004A92 RID: 19090
		[Range(0f, 1f)]
		public float buoyancyEquilibrium = 0.8f;

		// Token: 0x04004A93 RID: 19091
		private Rigidbody rb;

		// Token: 0x04004A94 RID: 19092
		private List<WaterVolume> overlappingWaterVolumes = new List<WaterVolume>();

		// Token: 0x04004A95 RID: 19093
		private List<WaterCurrent> activeWaterCurrents = new List<WaterCurrent>(16);

		// Token: 0x04004A96 RID: 19094
		private float baseAngularDrag = 0.05f;
	}
}
