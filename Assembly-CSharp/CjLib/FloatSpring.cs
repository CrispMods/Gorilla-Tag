using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C9D RID: 3229
	public struct FloatSpring
	{
		// Token: 0x06005175 RID: 20853 RVA: 0x0018F929 File Offset: 0x0018DB29
		public void Reset()
		{
			this.Value = 0f;
			this.Velocity = 0f;
		}

		// Token: 0x06005176 RID: 20854 RVA: 0x0018F941 File Offset: 0x0018DB41
		public void Reset(float initValue)
		{
			this.Value = initValue;
			this.Velocity = 0f;
		}

		// Token: 0x06005177 RID: 20855 RVA: 0x0018F955 File Offset: 0x0018DB55
		public void Reset(float initValue, float initVelocity)
		{
			this.Value = initValue;
			this.Velocity = initVelocity;
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x0018F968 File Offset: 0x0018DB68
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

		// Token: 0x06005179 RID: 20857 RVA: 0x0018FA38 File Offset: 0x0018DC38
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

		// Token: 0x0600517A RID: 20858 RVA: 0x0018FA84 File Offset: 0x0018DC84
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

		// Token: 0x040053BE RID: 21438
		public static readonly int Stride = 8;

		// Token: 0x040053BF RID: 21439
		public float Value;

		// Token: 0x040053C0 RID: 21440
		public float Velocity;
	}
}
