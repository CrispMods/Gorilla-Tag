using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C9F RID: 3231
	public struct Vector3Spring
	{
		// Token: 0x06005183 RID: 20867 RVA: 0x0018FCA7 File Offset: 0x0018DEA7
		public void Reset()
		{
			this.Value = Vector3.zero;
			this.Velocity = Vector3.zero;
		}

		// Token: 0x06005184 RID: 20868 RVA: 0x0018FCBF File Offset: 0x0018DEBF
		public void Reset(Vector3 initValue)
		{
			this.Value = initValue;
			this.Velocity = Vector3.zero;
		}

		// Token: 0x06005185 RID: 20869 RVA: 0x0018FCD3 File Offset: 0x0018DED3
		public void Reset(Vector3 initValue, Vector3 initVelocity)
		{
			this.Value = initValue;
			this.Velocity = initVelocity;
		}

		// Token: 0x06005186 RID: 20870 RVA: 0x0018FCE4 File Offset: 0x0018DEE4
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

		// Token: 0x06005187 RID: 20871 RVA: 0x0018FDE0 File Offset: 0x0018DFE0
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

		// Token: 0x06005188 RID: 20872 RVA: 0x0018FE2C File Offset: 0x0018E02C
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

		// Token: 0x040053C4 RID: 21444
		public static readonly int Stride = 32;

		// Token: 0x040053C5 RID: 21445
		public Vector3 Value;

		// Token: 0x040053C6 RID: 21446
		private float m_padding0;

		// Token: 0x040053C7 RID: 21447
		public Vector3 Velocity;

		// Token: 0x040053C8 RID: 21448
		private float m_padding1;
	}
}
