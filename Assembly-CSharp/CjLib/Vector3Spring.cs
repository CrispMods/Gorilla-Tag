using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CD0 RID: 3280
	public struct Vector3Spring
	{
		// Token: 0x060052E5 RID: 21221 RVA: 0x00065B3B File Offset: 0x00063D3B
		public void Reset()
		{
			this.Value = Vector3.zero;
			this.Velocity = Vector3.zero;
		}

		// Token: 0x060052E6 RID: 21222 RVA: 0x00065B53 File Offset: 0x00063D53
		public void Reset(Vector3 initValue)
		{
			this.Value = initValue;
			this.Velocity = Vector3.zero;
		}

		// Token: 0x060052E7 RID: 21223 RVA: 0x00065B67 File Offset: 0x00063D67
		public void Reset(Vector3 initValue, Vector3 initVelocity)
		{
			this.Value = initValue;
			this.Velocity = initVelocity;
		}

		// Token: 0x060052E8 RID: 21224 RVA: 0x001C66A4 File Offset: 0x001C48A4
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

		// Token: 0x060052E9 RID: 21225 RVA: 0x001C67A0 File Offset: 0x001C49A0
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

		// Token: 0x060052EA RID: 21226 RVA: 0x001C67EC File Offset: 0x001C49EC
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

		// Token: 0x040054D0 RID: 21712
		public static readonly int Stride = 32;

		// Token: 0x040054D1 RID: 21713
		public Vector3 Value;

		// Token: 0x040054D2 RID: 21714
		private float m_padding0;

		// Token: 0x040054D3 RID: 21715
		public Vector3 Velocity;

		// Token: 0x040054D4 RID: 21716
		private float m_padding1;
	}
}
