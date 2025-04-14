using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CEF RID: 3311
	public struct Vector3Spring
	{
		// Token: 0x06005381 RID: 21377 RVA: 0x0019B20F File Offset: 0x0019940F
		public void Reset()
		{
			this.Value = Vector3.zero;
			this.Velocity = Vector3.zero;
		}

		// Token: 0x06005382 RID: 21378 RVA: 0x0019B227 File Offset: 0x00199427
		public void Reset(Vector3 initValue)
		{
			this.Value = initValue;
			this.Velocity = Vector3.zero;
		}

		// Token: 0x06005383 RID: 21379 RVA: 0x0019B23B File Offset: 0x0019943B
		public void Reset(Vector3 initValue, Vector3 initVelocity)
		{
			this.Value = initValue;
			this.Velocity = initVelocity;
		}

		// Token: 0x06005384 RID: 21380 RVA: 0x0019B24C File Offset: 0x0019944C
		public Vector3 TrackDampingRatio(Vector3 targetValue, float angularFrequency, float dampingRatio, float deltaTime)
		{
			if (angularFrequency < MathUtil.Epsilon)
			{
				this.Velocity = Vector3.zero;
				return this.Value;
			}
			Vector3 a = targetValue - this.Value;
			float num = 1f + 2f * deltaTime * dampingRatio * angularFrequency;
			float num2 = angularFrequency * angularFrequency;
			float num3 = deltaTime * num2;
			float num4 = deltaTime * num3;
			float d = 1f / (num + num4);
			Vector3 a2 = num * this.Value + deltaTime * this.Velocity + num4 * targetValue;
			Vector3 a3 = this.Velocity + num3 * a;
			this.Velocity = a3 * d;
			this.Value = a2 * d;
			if (this.Velocity.magnitude < MathUtil.Epsilon && a.magnitude < MathUtil.Epsilon)
			{
				this.Velocity = Vector3.zero;
				this.Value = targetValue;
			}
			return this.Value;
		}

		// Token: 0x06005385 RID: 21381 RVA: 0x0019B348 File Offset: 0x00199548
		public Vector3 TrackHalfLife(Vector3 targetValue, float frequencyHz, float halfLife, float deltaTime)
		{
			if (halfLife < MathUtil.Epsilon)
			{
				this.Velocity = Vector3.zero;
				this.Value = targetValue;
				return this.Value;
			}
			float num = frequencyHz * MathUtil.TwoPi;
			float dampingRatio = 0.6931472f / (num * halfLife);
			return this.TrackDampingRatio(targetValue, num, dampingRatio, deltaTime);
		}

		// Token: 0x06005386 RID: 21382 RVA: 0x0019B394 File Offset: 0x00199594
		public Vector3 TrackExponential(Vector3 targetValue, float halfLife, float deltaTime)
		{
			if (halfLife < MathUtil.Epsilon)
			{
				this.Velocity = Vector3.zero;
				this.Value = targetValue;
				return this.Value;
			}
			float angularFrequency = 0.6931472f / halfLife;
			float dampingRatio = 1f;
			return this.TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
		}

		// Token: 0x040055D0 RID: 21968
		public static readonly int Stride = 32;

		// Token: 0x040055D1 RID: 21969
		public Vector3 Value;

		// Token: 0x040055D2 RID: 21970
		private float m_padding0;

		// Token: 0x040055D3 RID: 21971
		public Vector3 Velocity;

		// Token: 0x040055D4 RID: 21972
		private float m_padding1;
	}
}
