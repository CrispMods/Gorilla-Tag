using System;
using System.Collections.Generic;
using AA;
using CjLib;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B4A RID: 2890
	public class WaterCurrent : MonoBehaviour
	{
		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06004838 RID: 18488 RVA: 0x0015D285 File Offset: 0x0015B485
		public float Speed
		{
			get
			{
				return this.currentSpeed;
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06004839 RID: 18489 RVA: 0x0015D28D File Offset: 0x0015B48D
		public float Accel
		{
			get
			{
				return this.currentAccel;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x0600483A RID: 18490 RVA: 0x0015D295 File Offset: 0x0015B495
		public float InwardSpeed
		{
			get
			{
				return this.inwardCurrentSpeed;
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x0600483B RID: 18491 RVA: 0x0015D29D File Offset: 0x0015B49D
		public float InwardAccel
		{
			get
			{
				return this.inwardCurrentAccel;
			}
		}

		// Token: 0x0600483C RID: 18492 RVA: 0x0015D2A8 File Offset: 0x0015B4A8
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

		// Token: 0x0600483D RID: 18493 RVA: 0x0015D55C File Offset: 0x0015B75C
		private void Update()
		{
			if (this.debugDrawCurrentQueries)
			{
				DebugUtil.DrawSphere(this.debugSplinePoint, 0.15f, 12, 12, Color.green, false, DebugUtil.Style.Wireframe);
				DebugUtil.DrawArrow(this.debugSplinePoint, this.debugSplinePoint + this.debugCurrentVelocity, 0.1f, 0.1f, 12, 0.1f, Color.yellow, false, DebugUtil.Style.Wireframe);
			}
		}

		// Token: 0x0600483E RID: 18494 RVA: 0x0015D5C0 File Offset: 0x0015B7C0
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

		// Token: 0x0600483F RID: 18495 RVA: 0x0015D6A8 File Offset: 0x0015B8A8
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

		// Token: 0x04004AB8 RID: 19128
		[SerializeField]
		private List<CatmullRomSpline> splines = new List<CatmullRomSpline>();

		// Token: 0x04004AB9 RID: 19129
		[SerializeField]
		private float fullEffectDistance = 1f;

		// Token: 0x04004ABA RID: 19130
		[SerializeField]
		private float fadeDistance = 0.5f;

		// Token: 0x04004ABB RID: 19131
		[SerializeField]
		private float currentSpeed = 1f;

		// Token: 0x04004ABC RID: 19132
		[SerializeField]
		private float currentAccel = 10f;

		// Token: 0x04004ABD RID: 19133
		[SerializeField]
		private float velocityAnticipationAdjustment = 0.05f;

		// Token: 0x04004ABE RID: 19134
		[SerializeField]
		private float inwardCurrentFullEffectRadius = 1f;

		// Token: 0x04004ABF RID: 19135
		[SerializeField]
		private float inwardCurrentNoEffectRadius = 0.25f;

		// Token: 0x04004AC0 RID: 19136
		[SerializeField]
		private float inwardCurrentSpeed = 1f;

		// Token: 0x04004AC1 RID: 19137
		[SerializeField]
		private float inwardCurrentAccel = 10f;

		// Token: 0x04004AC2 RID: 19138
		[SerializeField]
		private float dampingHalfLife = 0.25f;

		// Token: 0x04004AC3 RID: 19139
		[SerializeField]
		private bool debugDrawCurrentQueries;

		// Token: 0x04004AC4 RID: 19140
		private Vector3 debugCurrentVelocity = Vector3.zero;

		// Token: 0x04004AC5 RID: 19141
		private Vector3 debugSplinePoint = Vector3.zero;
	}
}
