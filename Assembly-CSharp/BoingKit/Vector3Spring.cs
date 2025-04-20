using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000D20 RID: 3360
	public struct Vector3Spring
	{
		// Token: 0x060054E3 RID: 21731 RVA: 0x000670E0 File Offset: 0x000652E0
		public void Reset()
		{
			this.Value = Vector3.zero;
			this.Velocity = Vector3.zero;
		}

		// Token: 0x060054E4 RID: 21732 RVA: 0x000670F8 File Offset: 0x000652F8
		public void Reset(Vector3 initValue)
		{
			this.Value = initValue;
			this.Velocity = Vector3.zero;
		}

		// Token: 0x060054E5 RID: 21733 RVA: 0x0006710C File Offset: 0x0006530C
		public void Reset(Vector3 initValue, Vector3 initVelocity)
		{
			this.Value = initValue;
			this.Velocity = initVelocity;
		}

		// Token: 0x060054E6 RID: 21734 RVA: 0x001D050C File Offset: 0x001CE70C
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

		// Token: 0x060054E7 RID: 21735 RVA: 0x001D0608 File Offset: 0x001CE808
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

		// Token: 0x060054E8 RID: 21736 RVA: 0x001D0654 File Offset: 0x001CE854
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

		// Token: 0x040056DC RID: 22236
		public static readonly int Stride = 32;

		// Token: 0x040056DD RID: 22237
		public Vector3 Value;

		// Token: 0x040056DE RID: 22238
		private float m_padding0;

		// Token: 0x040056DF RID: 22239
		public Vector3 Velocity;

		// Token: 0x040056E0 RID: 22240
		private float m_padding1;
	}
}
