using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CA1 RID: 3233
	public struct Vector2Spring
	{
		// Token: 0x06005188 RID: 20872 RVA: 0x0019009A File Offset: 0x0018E29A
		public void Reset()
		{
			this.Value = Vector2.zero;
			this.Velocity = Vector2.zero;
		}

		// Token: 0x06005189 RID: 20873 RVA: 0x001900B2 File Offset: 0x0018E2B2
		public void Reset(Vector2 initValue)
		{
			this.Value = initValue;
			this.Velocity = Vector2.zero;
		}

		// Token: 0x0600518A RID: 20874 RVA: 0x001900C6 File Offset: 0x0018E2C6
		public void Reset(Vector2 initValue, Vector2 initVelocity)
		{
			this.Value = initValue;
			this.Velocity = initVelocity;
		}

		// Token: 0x0600518B RID: 20875 RVA: 0x001900D8 File Offset: 0x0018E2D8
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

		// Token: 0x0600518C RID: 20876 RVA: 0x001901D4 File Offset: 0x0018E3D4
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

		// Token: 0x0600518D RID: 20877 RVA: 0x00190220 File Offset: 0x0018E420
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

		// Token: 0x040053D3 RID: 21459
		public static readonly int Stride = 16;

		// Token: 0x040053D4 RID: 21460
		public Vector2 Value;

		// Token: 0x040053D5 RID: 21461
		public Vector2 Velocity;
	}
}
