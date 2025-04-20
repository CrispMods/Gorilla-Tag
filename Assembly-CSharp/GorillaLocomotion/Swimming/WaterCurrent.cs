using System;
using System.Collections.Generic;
using AA;
using CjLib;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B74 RID: 2932
	public class WaterCurrent : MonoBehaviour
	{
		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06004975 RID: 18805 RVA: 0x0005FD22 File Offset: 0x0005DF22
		public float Speed
		{
			get
			{
				return this.currentSpeed;
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06004976 RID: 18806 RVA: 0x0005FD2A File Offset: 0x0005DF2A
		public float Accel
		{
			get
			{
				return this.currentAccel;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06004977 RID: 18807 RVA: 0x0005FD32 File Offset: 0x0005DF32
		public float InwardSpeed
		{
			get
			{
				return this.inwardCurrentSpeed;
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06004978 RID: 18808 RVA: 0x0005FD3A File Offset: 0x0005DF3A
		public float InwardAccel
		{
			get
			{
				return this.inwardCurrentAccel;
			}
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x00198820 File Offset: 0x00196A20
		public bool GetCurrentAtPoint(Vector3 worldPoint, Vector3 startingVelocity, float dt, out Vector3 currentVelocity, out Vector3 velocityChange)
		{
			float num = (this.fullEffectDistance + this.fadeDistance) * (this.fullEffectDistance + this.fadeDistance);
			bool result = false;
			velocityChange = Vector3.zero;
			currentVelocity = Vector3.zero;
			float num2 = 0.0001f;
			float magnitude = startingVelocity.magnitude;
			if (magnitude > num2)
			{
				Vector3 a = startingVelocity / magnitude;
				float d = Spring.DamperDecayExact(magnitude, this.dampingHalfLife, dt, 1E-05f);
				Vector3 a2 = a * d;
				velocityChange += a2 - startingVelocity;
			}
			for (int i = 0; i < this.splines.Count; i++)
			{
				CatmullRomSpline catmullRomSpline = this.splines[i];
				Vector3 vector;
				float closestEvaluationOnSpline = catmullRomSpline.GetClosestEvaluationOnSpline(worldPoint, out vector);
				Vector3 a3 = catmullRomSpline.Evaluate(closestEvaluationOnSpline);
				Vector3 vector2 = a3 - worldPoint;
				if (vector2.sqrMagnitude < num)
				{
					result = true;
					float magnitude2 = vector2.magnitude;
					float num3 = (magnitude2 > this.fullEffectDistance) ? (1f - Mathf.Clamp01((magnitude2 - this.fullEffectDistance) / this.fadeDistance)) : 1f;
					float t = Mathf.Clamp01(closestEvaluationOnSpline + this.velocityAnticipationAdjustment);
					Vector3 forwardTangent = catmullRomSpline.GetForwardTangent(t, 0.01f);
					if (this.currentSpeed > num2 && Vector3.Dot(startingVelocity, forwardTangent) < num3 * this.currentSpeed)
					{
						velocityChange += forwardTangent * (this.currentAccel * dt);
					}
					else if (this.currentSpeed < num2 && Vector3.Dot(startingVelocity, forwardTangent) > num3 * this.currentSpeed)
					{
						velocityChange -= forwardTangent * (this.currentAccel * dt);
					}
					currentVelocity += forwardTangent * num3 * this.currentSpeed;
					float num4 = Mathf.InverseLerp(this.inwardCurrentNoEffectRadius, this.inwardCurrentFullEffectRadius, magnitude2);
					if (num4 > num2)
					{
						vector = Vector3.ProjectOnPlane(vector2, forwardTangent);
						Vector3 normalized = vector.normalized;
						if (this.inwardCurrentSpeed > num2 && Vector3.Dot(startingVelocity, normalized) < num4 * this.inwardCurrentSpeed)
						{
							velocityChange += normalized * (this.InwardAccel * dt);
						}
						else if (this.inwardCurrentSpeed < num2 && Vector3.Dot(startingVelocity, normalized) > num4 * this.inwardCurrentSpeed)
						{
							velocityChange -= normalized * (this.InwardAccel * dt);
						}
					}
					this.debugSplinePoint = a3;
				}
			}
			this.debugCurrentVelocity = velocityChange.normalized;
			return result;
		}

		// Token: 0x0600497A RID: 18810 RVA: 0x00198AD4 File Offset: 0x00196CD4
		private void Update()
		{
			if (this.debugDrawCurrentQueries)
			{
				DebugUtil.DrawSphere(this.debugSplinePoint, 0.15f, 12, 12, Color.green, false, DebugUtil.Style.Wireframe);
				DebugUtil.DrawArrow(this.debugSplinePoint, this.debugSplinePoint + this.debugCurrentVelocity, 0.1f, 0.1f, 12, 0.1f, Color.yellow, false, DebugUtil.Style.Wireframe);
			}
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x00198B38 File Offset: 0x00196D38
		private void OnDrawGizmosSelected()
		{
			int num = 16;
			for (int i = 0; i < this.splines.Count; i++)
			{
				CatmullRomSpline catmullRomSpline = this.splines[i];
				Vector3 b = catmullRomSpline.Evaluate(0f);
				for (int j = 1; j <= num; j++)
				{
					float t = (float)j / (float)num;
					Vector3 vector = catmullRomSpline.Evaluate(t);
					vector - b;
					Quaternion rotation = Quaternion.LookRotation(catmullRomSpline.GetForwardTangent(t, 0.01f), Vector3.up);
					Gizmos.color = new Color(0f, 0.5f, 0.75f);
					this.DrawGizmoCircle(vector, rotation, this.fullEffectDistance);
					Gizmos.color = new Color(0f, 0.25f, 0.5f);
					this.DrawGizmoCircle(vector, rotation, this.fullEffectDistance + this.fadeDistance);
				}
			}
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x00198C20 File Offset: 0x00196E20
		private void DrawGizmoCircle(Vector3 center, Quaternion rotation, float radius)
		{
			Vector3 point = Vector3.right * radius;
			int num = 16;
			for (int i = 1; i <= num; i++)
			{
				float f = (float)i / (float)num * 2f * 3.1415927f;
				Vector3 vector = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0f) * radius;
				Gizmos.DrawLine(center + rotation * point, center + rotation * vector);
				point = vector;
			}
		}

		// Token: 0x04004B9C RID: 19356
		[SerializeField]
		private List<CatmullRomSpline> splines = new List<CatmullRomSpline>();

		// Token: 0x04004B9D RID: 19357
		[SerializeField]
		private float fullEffectDistance = 1f;

		// Token: 0x04004B9E RID: 19358
		[SerializeField]
		private float fadeDistance = 0.5f;

		// Token: 0x04004B9F RID: 19359
		[SerializeField]
		private float currentSpeed = 1f;

		// Token: 0x04004BA0 RID: 19360
		[SerializeField]
		private float currentAccel = 10f;

		// Token: 0x04004BA1 RID: 19361
		[SerializeField]
		private float velocityAnticipationAdjustment = 0.05f;

		// Token: 0x04004BA2 RID: 19362
		[SerializeField]
		private float inwardCurrentFullEffectRadius = 1f;

		// Token: 0x04004BA3 RID: 19363
		[SerializeField]
		private float inwardCurrentNoEffectRadius = 0.25f;

		// Token: 0x04004BA4 RID: 19364
		[SerializeField]
		private float inwardCurrentSpeed = 1f;

		// Token: 0x04004BA5 RID: 19365
		[SerializeField]
		private float inwardCurrentAccel = 10f;

		// Token: 0x04004BA6 RID: 19366
		[SerializeField]
		private float dampingHalfLife = 0.25f;

		// Token: 0x04004BA7 RID: 19367
		[SerializeField]
		private bool debugDrawCurrentQueries;

		// Token: 0x04004BA8 RID: 19368
		private Vector3 debugCurrentVelocity = Vector3.zero;

		// Token: 0x04004BA9 RID: 19369
		private Vector3 debugSplinePoint = Vector3.zero;
	}
}
