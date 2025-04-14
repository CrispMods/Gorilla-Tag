using System;
using System.Collections.Generic;
using AA;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B42 RID: 2882
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyWaterInteraction : MonoBehaviour
	{
		// Token: 0x0600480E RID: 18446 RVA: 0x0015B6A2 File Offset: 0x001598A2
		protected void Awake()
		{
			this.rb = base.GetComponent<Rigidbody>();
			this.baseAngularDrag = this.rb.angularDrag;
			RigidbodyWaterInteractionManager.RegisterRBWI(this);
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x0015B6C7 File Offset: 0x001598C7
		protected void OnEnable()
		{
			this.overlappingWaterVolumes.Clear();
			RigidbodyWaterInteractionManager.RegisterRBWI(this);
		}

		// Token: 0x06004810 RID: 18448 RVA: 0x0015B6DA File Offset: 0x001598DA
		protected void OnDisable()
		{
			this.overlappingWaterVolumes.Clear();
			RigidbodyWaterInteractionManager.UnregisterRBWI(this);
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x0015B6ED File Offset: 0x001598ED
		private void OnDestroy()
		{
			RigidbodyWaterInteractionManager.UnregisterRBWI(this);
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x0015B6F8 File Offset: 0x001598F8
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

		// Token: 0x06004813 RID: 18451 RVA: 0x0015BC28 File Offset: 0x00159E28
		protected void OnTriggerEnter(Collider other)
		{
			WaterVolume component = other.GetComponent<WaterVolume>();
			if (component != null && !this.overlappingWaterVolumes.Contains(component))
			{
				this.overlappingWaterVolumes.Add(component);
			}
		}

		// Token: 0x06004814 RID: 18452 RVA: 0x0015BC60 File Offset: 0x00159E60
		protected void OnTriggerExit(Collider other)
		{
			WaterVolume component = other.GetComponent<WaterVolume>();
			if (component != null && this.overlappingWaterVolumes.Contains(component))
			{
				this.overlappingWaterVolumes.Remove(component);
			}
		}

		// Token: 0x04004A74 RID: 19060
		public bool applyDamping = true;

		// Token: 0x04004A75 RID: 19061
		public bool applyBuoyancyForce = true;

		// Token: 0x04004A76 RID: 19062
		public bool applyAngularDrag = true;

		// Token: 0x04004A77 RID: 19063
		public bool applyWaterCurrents = true;

		// Token: 0x04004A78 RID: 19064
		public bool applySurfaceTorque = true;

		// Token: 0x04004A79 RID: 19065
		public float underWaterDampingHalfLife = 0.25f;

		// Token: 0x04004A7A RID: 19066
		public float waterSurfaceDampingHalfLife = 1f;

		// Token: 0x04004A7B RID: 19067
		public float underWaterBuoyancyFactor = 0.5f;

		// Token: 0x04004A7C RID: 19068
		public float angularDrag = 0.5f;

		// Token: 0x04004A7D RID: 19069
		public float surfaceTorqueAmount = 0.5f;

		// Token: 0x04004A7E RID: 19070
		public bool enablePreciseWaterCollision;

		// Token: 0x04004A7F RID: 19071
		public float objectRadiusForWaterCollision = 0.25f;

		// Token: 0x04004A80 RID: 19072
		[Range(0f, 1f)]
		public float buoyancyEquilibrium = 0.8f;

		// Token: 0x04004A81 RID: 19073
		private Rigidbody rb;

		// Token: 0x04004A82 RID: 19074
		private List<WaterVolume> overlappingWaterVolumes = new List<WaterVolume>();

		// Token: 0x04004A83 RID: 19075
		private List<WaterCurrent> activeWaterCurrents = new List<WaterCurrent>(16);

		// Token: 0x04004A84 RID: 19076
		private float baseAngularDrag = 0.05f;
	}
}
