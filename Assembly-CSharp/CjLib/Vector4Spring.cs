using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CA0 RID: 3232
	public struct Vector4Spring
	{
		// Token: 0x0600518A RID: 20874 RVA: 0x0018FE7B File Offset: 0x0018E07B
		public void Reset()
		{
			this.Value = Vector4.zero;
			this.Velocity = Vector4.zero;
		}

		// Token: 0x0600518B RID: 20875 RVA: 0x0018FE93 File Offset: 0x0018E093
		public void Reset(Vector4 initValue)
		{
			this.Value = initValue;
			this.Velocity = Vector4.zero;
		}

		// Token: 0x0600518C RID: 20876 RVA: 0x0018FEA7 File Offset: 0x0018E0A7
		public void Reset(Vector4 initValue, Vector4 initVelocity)
		{
			this.Value = initValue;
			this.Velocity = initVelocity;
		}

		// Token: 0x0600518D RID: 20877 RVA: 0x0018FEB8 File Offset: 0x0018E0B8
		public Vector4 TrackDampingRatio(Vector4 targetValue, float angularFrequency, float dampingRatio, float deltaTime)
		{
			if (angularFrequency < MathUtil.Epsilon)
			{
				this.Velocity = Vector4.zero;
				return this.Value;
			}
			Vector4 a = targetValue - this.Value;
			float num = 1f + 2f * deltaTime * dampingRatio * angularFrequency;
			float num2 = angularFrequency * angularFrequency;
			float num3 = deltaTime * num2;
			float num4 = deltaTime * num3;
			float d = 1f / (num + num4);
			Vector4 a2 = num * this.Value + deltaTime * this.Velocity + num4 * targetValue;
			Vector4 a3 = this.Velocity + num3 * a;
			this.Velocity = a3 * d;
			this.Value = a2 * d;
			if (this.Velocity.magnitude < MathUtil.Epsilon && a.magnitude < MathUtil.Epsilon)
			{
				this.Velocity = Vector4.zero;
				this.Value = targetValue;
			}
			return this.Value;
		}

		// Token: 0x0600518E RID: 20878 RVA: 0x0018FFB4 File Offset: 0x0018E1B4
		public Vector4 TrackHalfLife(Vector4 targetValue, float frequencyHz, float halfLife, float deltaTime)
		{
			if (halfLife < MathUtil.Epsilon)
			{
				this.Velocity = Vector4.zero;
				this.Value = targetValue;
				return this.Value;
			}
			float num = frequencyHz * MathUtil.TwoPi;
			float dampingRatio = 0.6931472f / (num * halfLife);
			return this.TrackDampingRatio(targetValue, num, dampingRatio, deltaTime);
		}

		// Token: 0x0600518F RID: 20879 RVA: 0x00190000 File Offset: 0x0018E200
		public Vector4 TrackExponential(Vector4 targetValue, float halfLife, float deltaTime)
		{
			if (halfLife < MathUtil.Epsilon)
			{
				this.Velocity = Vector4.zero;
				this.Value = targetValue;
				return this.Value;
			}
			float angularFrequency = 0.6931472f / halfLife;
			float dampingRatio = 1f;
			return this.TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
		}

		// Token: 0x040053C9 RID: 21449
		public static readonly int Stride = 32;

		// Token: 0x040053CA RID: 21450
		public Vector4 Value;

		// Token: 0x040053CB RID: 21451
		public Vector4 Velocity;
	}
}
