using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C9E RID: 3230
	public struct Vector2Spring
	{
		// Token: 0x0600517C RID: 20860 RVA: 0x0018FAD2 File Offset: 0x0018DCD2
		public void Reset()
		{
			this.Value = Vector2.zero;
			this.Velocity = Vector2.zero;
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x0018FAEA File Offset: 0x0018DCEA
		public void Reset(Vector2 initValue)
		{
			this.Value = initValue;
			this.Velocity = Vector2.zero;
		}

		// Token: 0x0600517E RID: 20862 RVA: 0x0018FAFE File Offset: 0x0018DCFE
		public void Reset(Vector2 initValue, Vector2 initVelocity)
		{
			this.Value = initValue;
			this.Velocity = initVelocity;
		}

		// Token: 0x0600517F RID: 20863 RVA: 0x0018FB10 File Offset: 0x0018DD10
		public Vector2 TrackDampingRatio(Vector2 targetValue, float angularFrequency, float dampingRatio, float deltaTime)
		{
			if (angularFrequency < MathUtil.Epsilon)
			{
				this.Velocity = Vector2.zero;
				return this.Value;
			}
			Vector2 a = targetValue - this.Value;
			float num = 1f + 2f * deltaTime * dampingRatio * angularFrequency;
			float num2 = angularFrequency * angularFrequency;
			float num3 = deltaTime * num2;
			float num4 = deltaTime * num3;
			float d = 1f / (num + num4);
			Vector2 a2 = num * this.Value + deltaTime * this.Velocity + num4 * targetValue;
			Vector2 a3 = this.Velocity + num3 * a;
			this.Velocity = a3 * d;
			this.Value = a2 * d;
			if (this.Velocity.magnitude < MathUtil.Epsilon && a.magnitude < MathUtil.Epsilon)
			{
				this.Velocity = Vector2.zero;
				this.Value = targetValue;
			}
			return this.Value;
		}

		// Token: 0x06005180 RID: 20864 RVA: 0x0018FC0C File Offset: 0x0018DE0C
		public Vector2 TrackHalfLife(Vector2 targetValue, float frequencyHz, float halfLife, float deltaTime)
		{
			if (halfLife < MathUtil.Epsilon)
			{
				this.Velocity = Vector2.zero;
				this.Value = targetValue;
				return this.Value;
			}
			float num = frequencyHz * MathUtil.TwoPi;
			float dampingRatio = 0.6931472f / (num * halfLife);
			return this.TrackDampingRatio(targetValue, num, dampingRatio, deltaTime);
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x0018FC58 File Offset: 0x0018DE58
		public Vector2 TrackExponential(Vector2 targetValue, float halfLife, float deltaTime)
		{
			if (halfLife < MathUtil.Epsilon)
			{
				this.Velocity = Vector2.zero;
				this.Value = targetValue;
				return this.Value;
			}
			float angularFrequency = 0.6931472f / halfLife;
			float dampingRatio = 1f;
			return this.TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
		}

		// Token: 0x040053C1 RID: 21441
		public static readonly int Stride = 16;

		// Token: 0x040053C2 RID: 21442
		public Vector2 Value;

		// Token: 0x040053C3 RID: 21443
		public Vector2 Velocity;
	}
}
