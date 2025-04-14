using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CED RID: 3309
	public struct FloatSpring
	{
		// Token: 0x06005373 RID: 21363 RVA: 0x0019AE93 File Offset: 0x00199093
		public void Reset()
		{
			this.Value = 0f;
			this.Velocity = 0f;
		}

		// Token: 0x06005374 RID: 21364 RVA: 0x0019AEAB File Offset: 0x001990AB
		public void Reset(float initValue)
		{
			this.Value = initValue;
			this.Velocity = 0f;
		}

		// Token: 0x06005375 RID: 21365 RVA: 0x0019AEBF File Offset: 0x001990BF
		public void Reset(float initValue, float initVelocity)
		{
			this.Value = initValue;
			this.Velocity = initVelocity;
		}

		// Token: 0x06005376 RID: 21366 RVA: 0x0019AED0 File Offset: 0x001990D0
		public float TrackDampingRatio(float targetValue, float angularFrequency, float dampingRatio, float deltaTime)
		{
			if (angularFrequency < MathUtil.Epsilon)
			{
				this.Velocity = 0f;
				return this.Value;
			}
			float num = targetValue - this.Value;
			float num2 = 1f + 2f * deltaTime * dampingRatio * angularFrequency;
			float num3 = angularFrequency * angularFrequency;
			float num4 = deltaTime * num3;
			float num5 = deltaTime * num4;
			float num6 = 1f / (num2 + num5);
			float num7 = num2 * this.Value + deltaTime * this.Velocity + num5 * targetValue;
			float num8 = this.Velocity + num4 * num;
			this.Velocity = num8 * num6;
			this.Value = num7 * num6;
			if (Mathf.Abs(this.Velocity) < MathUtil.Epsilon && Mathf.Abs(num) < MathUtil.Epsilon)
			{
				this.Velocity = 0f;
				this.Value = targetValue;
			}
			return this.Value;
		}

		// Token: 0x06005377 RID: 21367 RVA: 0x0019AFA0 File Offset: 0x001991A0
		public float TrackHalfLife(float targetValue, float frequencyHz, float halfLife, float deltaTime)
		{
			if (halfLife < MathUtil.Epsilon)
			{
				this.Velocity = 0f;
				this.Value = targetValue;
				return this.Value;
			}
			float num = frequencyHz * MathUtil.TwoPi;
			float dampingRatio = 0.6931472f / (num * halfLife);
			return this.TrackDampingRatio(targetValue, num, dampingRatio, deltaTime);
		}

		// Token: 0x06005378 RID: 21368 RVA: 0x0019AFEC File Offset: 0x001991EC
		public float TrackExponential(float targetValue, float halfLife, float deltaTime)
		{
			if (halfLife < MathUtil.Epsilon)
			{
				this.Velocity = 0f;
				this.Value = targetValue;
				return this.Value;
			}
			float angularFrequency = 0.6931472f / halfLife;
			float dampingRatio = 1f;
			return this.TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
		}

		// Token: 0x040055CA RID: 21962
		public static readonly int Stride = 8;

		// Token: 0x040055CB RID: 21963
		public float Value;

		// Token: 0x040055CC RID: 21964
		public float Velocity;
	}
}
