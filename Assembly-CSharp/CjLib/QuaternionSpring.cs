using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CA1 RID: 3233
	public struct QuaternionSpring
	{
		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06005191 RID: 20881 RVA: 0x0019004F File Offset: 0x0018E24F
		// (set) Token: 0x06005192 RID: 20882 RVA: 0x0019005D File Offset: 0x0018E25D
		public Quaternion ValueQuat
		{
			get
			{
				return QuaternionUtil.FromVector4(this.ValueVec, true);
			}
			set
			{
				this.ValueVec = QuaternionUtil.ToVector4(value);
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06005193 RID: 20883 RVA: 0x0019006B File Offset: 0x0018E26B
		// (set) Token: 0x06005194 RID: 20884 RVA: 0x00190079 File Offset: 0x0018E279
		public Quaternion VelocityQuat
		{
			get
			{
				return QuaternionUtil.FromVector4(this.VelocityVec, false);
			}
			set
			{
				this.VelocityVec = QuaternionUtil.ToVector4(value);
			}
		}

		// Token: 0x06005195 RID: 20885 RVA: 0x00190087 File Offset: 0x0018E287
		public void Reset()
		{
			this.ValueVec = QuaternionUtil.ToVector4(Quaternion.identity);
			this.VelocityVec = Vector4.zero;
		}

		// Token: 0x06005196 RID: 20886 RVA: 0x001900A4 File Offset: 0x0018E2A4
		public void Reset(Vector4 initValue)
		{
			this.ValueVec = initValue;
			this.VelocityVec = Vector4.zero;
		}

		// Token: 0x06005197 RID: 20887 RVA: 0x001900B8 File Offset: 0x0018E2B8
		public void Reset(Vector4 initValue, Vector4 initVelocity)
		{
			this.ValueVec = initValue;
			this.VelocityVec = initVelocity;
		}

		// Token: 0x06005198 RID: 20888 RVA: 0x001900C8 File Offset: 0x0018E2C8
		public void Reset(Quaternion initValue)
		{
			this.ValueVec = QuaternionUtil.ToVector4(initValue);
			this.VelocityVec = Vector4.zero;
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x001900E1 File Offset: 0x0018E2E1
		public void Reset(Quaternion initValue, Quaternion initVelocity)
		{
			this.ValueVec = QuaternionUtil.ToVector4(initValue);
			this.VelocityVec = QuaternionUtil.ToVector4(initVelocity);
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x001900FC File Offset: 0x0018E2FC
		public Quaternion TrackDampingRatio(Quaternion targetValue, float angularFrequency, float dampingRatio, float deltaTime)
		{
			if (angularFrequency < MathUtil.Epsilon)
			{
				this.VelocityVec = QuaternionUtil.ToVector4(Quaternion.identity);
				return QuaternionUtil.FromVector4(this.ValueVec, true);
			}
			Vector4 vector = QuaternionUtil.ToVector4(targetValue);
			if (Vector4.Dot(this.ValueVec, vector) < 0f)
			{
				vector = -vector;
			}
			Vector4 a = vector - this.ValueVec;
			float num = 1f + 2f * deltaTime * dampingRatio * angularFrequency;
			float num2 = angularFrequency * angularFrequency;
			float num3 = deltaTime * num2;
			float num4 = deltaTime * num3;
			float d = 1f / (num + num4);
			Vector4 a2 = num * this.ValueVec + deltaTime * this.VelocityVec + num4 * vector;
			Vector4 a3 = this.VelocityVec + num3 * a;
			this.VelocityVec = a3 * d;
			this.ValueVec = a2 * d;
			if (this.VelocityVec.magnitude < MathUtil.Epsilon && a.magnitude < MathUtil.Epsilon)
			{
				this.VelocityVec = QuaternionUtil.ToVector4(Quaternion.identity);
				this.ValueVec = vector;
			}
			return QuaternionUtil.FromVector4(this.ValueVec, true);
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x00190230 File Offset: 0x0018E430
		public Quaternion TrackHalfLife(Quaternion targetValue, float frequencyHz, float halfLife, float deltaTime)
		{
			if (halfLife < MathUtil.Epsilon)
			{
				this.VelocityVec = QuaternionUtil.ToVector4(Quaternion.identity);
				this.ValueVec = QuaternionUtil.ToVector4(targetValue);
				return targetValue;
			}
			float num = frequencyHz * MathUtil.TwoPi;
			float dampingRatio = 0.6931472f / (num * halfLife);
			return this.TrackDampingRatio(targetValue, num, dampingRatio, deltaTime);
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x00190280 File Offset: 0x0018E480
		public Quaternion TrackExponential(Quaternion targetValue, float halfLife, float deltaTime)
		{
			if (halfLife < MathUtil.Epsilon)
			{
				this.VelocityVec = QuaternionUtil.ToVector4(Quaternion.identity);
				this.ValueVec = QuaternionUtil.ToVector4(targetValue);
				return targetValue;
			}
			float angularFrequency = 0.6931472f / halfLife;
			float dampingRatio = 1f;
			return this.TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
		}

		// Token: 0x040053CC RID: 21452
		public static readonly int Stride = 32;

		// Token: 0x040053CD RID: 21453
		public Vector4 ValueVec;

		// Token: 0x040053CE RID: 21454
		public Vector4 VelocityVec;
	}
}
