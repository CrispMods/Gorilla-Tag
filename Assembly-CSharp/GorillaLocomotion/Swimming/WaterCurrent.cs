﻿using System;
using System.Collections.Generic;
using AA;
using CjLib;
using UnityEngine;

namespace GorillaLocomotion.Swimming
{
	// Token: 0x02000B47 RID: 2887
	public class WaterCurrent : MonoBehaviour
	{
		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x0600482C RID: 18476 RVA: 0x0015CCBD File Offset: 0x0015AEBD
		public float Speed
		{
			get
			{
				return this.currentSpeed;
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x0600482D RID: 18477 RVA: 0x0015CCC5 File Offset: 0x0015AEC5
		public float Accel
		{
			get
			{
				return this.currentAccel;
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x0600482E RID: 18478 RVA: 0x0015CCCD File Offset: 0x0015AECD
		public float InwardSpeed
		{
			get
			{
				return this.inwardCurrentSpeed;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x0600482F RID: 18479 RVA: 0x0015CCD5 File Offset: 0x0015AED5
		public float InwardAccel
		{
			get
			{
				return this.inwardCurrentAccel;
			}
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x0015CCE0 File Offset: 0x0015AEE0
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

		// Token: 0x06004831 RID: 18481 RVA: 0x0015CF94 File Offset: 0x0015B194
		private void Update()
		{
			if (this.debugDrawCurrentQueries)
			{
				DebugUtil.DrawSphere(this.debugSplinePoint, 0.15f, 12, 12, Color.green, false, DebugUtil.Style.Wireframe);
				DebugUtil.DrawArrow(this.debugSplinePoint, this.debugSplinePoint + this.debugCurrentVelocity, 0.1f, 0.1f, 12, 0.1f, Color.yellow, false, DebugUtil.Style.Wireframe);
			}
		}

		// Token: 0x06004832 RID: 18482 RVA: 0x0015CFF8 File Offset: 0x0015B1F8
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

		// Token: 0x06004833 RID: 18483 RVA: 0x0015D0E0 File Offset: 0x0015B2E0
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

		// Token: 0x04004AA6 RID: 19110
		[SerializeField]
		private List<CatmullRomSpline> splines = new List<CatmullRomSpline>();

		// Token: 0x04004AA7 RID: 19111
		[SerializeField]
		private float fullEffectDistance = 1f;

		// Token: 0x04004AA8 RID: 19112
		[SerializeField]
		private float fadeDistance = 0.5f;

		// Token: 0x04004AA9 RID: 19113
		[SerializeField]
		private float currentSpeed = 1f;

		// Token: 0x04004AAA RID: 19114
		[SerializeField]
		private float currentAccel = 10f;

		// Token: 0x04004AAB RID: 19115
		[SerializeField]
		private float velocityAnticipationAdjustment = 0.05f;

		// Token: 0x04004AAC RID: 19116
		[SerializeField]
		private float inwardCurrentFullEffectRadius = 1f;

		// Token: 0x04004AAD RID: 19117
		[SerializeField]
		private float inwardCurrentNoEffectRadius = 0.25f;

		// Token: 0x04004AAE RID: 19118
		[SerializeField]
		private float inwardCurrentSpeed = 1f;

		// Token: 0x04004AAF RID: 19119
		[SerializeField]
		private float inwardCurrentAccel = 10f;

		// Token: 0x04004AB0 RID: 19120
		[SerializeField]
		private float dampingHalfLife = 0.25f;

		// Token: 0x04004AB1 RID: 19121
		[SerializeField]
		private bool debugDrawCurrentQueries;

		// Token: 0x04004AB2 RID: 19122
		private Vector3 debugCurrentVelocity = Vector3.zero;

		// Token: 0x04004AB3 RID: 19123
		private Vector3 debugSplinePoint = Vector3.zero;
	}
}
