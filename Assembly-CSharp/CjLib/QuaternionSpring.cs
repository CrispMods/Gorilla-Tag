using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CD2 RID: 3282
	public struct QuaternionSpring
	{
		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x060052F3 RID: 21235 RVA: 0x00065BC5 File Offset: 0x00063DC5
		// (set) Token: 0x060052F4 RID: 21236 RVA: 0x00065BD3 File Offset: 0x00063DD3
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

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x060052F5 RID: 21237 RVA: 0x00065BE1 File Offset: 0x00063DE1
		// (set) Token: 0x060052F6 RID: 21238 RVA: 0x00065BEF File Offset: 0x00063DEF
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

		// Token: 0x060052F7 RID: 21239 RVA: 0x00065BFD File Offset: 0x00063DFD
		public void Reset()
		{
			this.ValueVec = QuaternionUtil.ToVector4(Quaternion.identity);
			this.VelocityVec = Vector4.zero;
		}

		// Token: 0x060052F8 RID: 21240 RVA: 0x00065C1A File Offset: 0x00063E1A
		public void Reset(Vector4 initValue)
		{
			this.ValueVec = initValue;
			this.VelocityVec = Vector4.zero;
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x00065C2E File Offset: 0x00063E2E
		public void Reset(Vector4 initValue, Vector4 initVelocity)
		{
			this.ValueVec = initValue;
			this.VelocityVec = initVelocity;
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x00065C3E File Offset: 0x00063E3E
		public void Reset(Quaternion initValue)
		{
			this.ValueVec = QuaternionUtil.ToVector4(initValue);
			this.VelocityVec = Vector4.zero;
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x00065C57 File Offset: 0x00063E57
		public void Reset(Quaternion initValue, Quaternion initVelocity)
		{
			this.ValueVec = QuaternionUtil.ToVector4(initValue);
			this.VelocityVec = QuaternionUtil.ToVector4(initVelocity);
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x001C69C4 File Offset: 0x001C4BC4
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

		// Token: 0x060052FD RID: 21245 RVA: 0x001C6AF8 File Offset: 0x001C4CF8
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

		// Token: 0x060052FE RID: 21246 RVA: 0x001C6B48 File Offset: 0x001C4D48
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

		// Token: 0x040054D8 RID: 21720
		public static readonly int Stride = 32;

		// Token: 0x040054D9 RID: 21721
		public Vector4 ValueVec;

		// Token: 0x040054DA RID: 21722
		public Vector4 VelocityVec;
	}
}
