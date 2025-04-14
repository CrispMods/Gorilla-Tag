using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CA3 RID: 3235
	public struct Vector4Spring
	{
		// Token: 0x06005196 RID: 20886 RVA: 0x00190443 File Offset: 0x0018E643
		public void Reset()
		{
			this.Value = Vector4.zero;
			this.Velocity = Vector4.zero;
		}

		// Token: 0x06005197 RID: 20887 RVA: 0x0019045B File Offset: 0x0018E65B
		public void Reset(Vector4 initValue)
		{
			this.Value = initValue;
			this.Velocity = Vector4.zero;
		}

		// Token: 0x06005198 RID: 20888 RVA: 0x0019046F File Offset: 0x0018E66F
		public void Reset(Vector4 initValue, Vector4 initVelocity)
		{
			this.Value = initValue;
			this.Velocity = initVelocity;
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x00190480 File Offset: 0x0018E680
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

		// Token: 0x0600519A RID: 20890 RVA: 0x0019057C File Offset: 0x0018E77C
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

		// Token: 0x0600519B RID: 20891 RVA: 0x001905C8 File Offset: 0x0018E7C8
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

		// Token: 0x040053DB RID: 21467
		public static readonly int Stride = 32;

		// Token: 0x040053DC RID: 21468
		public Vector4 Value;

		// Token: 0x040053DD RID: 21469
		public Vector4 Velocity;
	}
}
