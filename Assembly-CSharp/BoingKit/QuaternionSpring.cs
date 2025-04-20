using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000D22 RID: 3362
	public struct QuaternionSpring
	{
		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x060054F1 RID: 21745 RVA: 0x0006716A File Offset: 0x0006536A
		// (set) Token: 0x060054F2 RID: 21746 RVA: 0x00067178 File Offset: 0x00065378
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

		// Token: 0x060054F3 RID: 21747 RVA: 0x00067186 File Offset: 0x00065386
		public void Reset()
		{
			this.ValueVec = QuaternionUtil.ToVector4(Quaternion.identity);
			this.VelocityVec = Vector4.zero;
		}

		// Token: 0x060054F4 RID: 21748 RVA: 0x000671A3 File Offset: 0x000653A3
		public void Reset(Vector4 initValue)
		{
			this.ValueVec = initValue;
			this.VelocityVec = Vector4.zero;
		}

		// Token: 0x060054F5 RID: 21749 RVA: 0x000671B7 File Offset: 0x000653B7
		public void Reset(Vector4 initValue, Vector4 initVelocity)
		{
			this.ValueVec = initValue;
			this.VelocityVec = initVelocity;
		}

		// Token: 0x060054F6 RID: 21750 RVA: 0x000671C7 File Offset: 0x000653C7
		public void Reset(Quaternion initValue)
		{
			this.ValueVec = QuaternionUtil.ToVector4(initValue);
			this.VelocityVec = Vector4.zero;
		}

		// Token: 0x060054F7 RID: 21751 RVA: 0x000671E0 File Offset: 0x000653E0
		public void Reset(Quaternion initValue, Quaternion initVelocity)
		{
			this.ValueVec = QuaternionUtil.ToVector4(initValue);
			this.VelocityVec = QuaternionUtil.ToVector4(initVelocity);
		}

		// Token: 0x060054F8 RID: 21752 RVA: 0x001D082C File Offset: 0x001CEA2C
		public Quaternion TrackDampingRatio(Vector4 targetValueVec, float angularFrequency, float dampingRatio, float deltaTime)
		{
			if (angularFrequency < MathUtil.Epsilon)
			{
				this.VelocityVec = QuaternionUtil.ToVector4(Quaternion.identity);
				return QuaternionUtil.FromVector4(this.ValueVec, true);
			}
			if (Vector4.Dot(this.ValueVec, targetValueVec) < 0f)
			{
				targetValueVec = -targetValueVec;
			}
			Vector4 a = targetValueVec - this.ValueVec;
			float num = 1f + 2f * deltaTime * dampingRatio * angularFrequency;
			float num2 = angularFrequency * angularFrequency;
			float num3 = deltaTime * num2;
			float num4 = deltaTime * num3;
			float d = 1f / (num + num4);
			Vector4 a2 = num * this.ValueVec + deltaTime * this.VelocityVec + num4 * targetValueVec;
			Vector4 a3 = this.VelocityVec + num3 * a;
			this.VelocityVec = a3 * d;
			this.ValueVec = a2 * d;
			if (this.VelocityVec.magnitude < MathUtil.Epsilon && a.magnitude < MathUtil.Epsilon)
			{
				this.VelocityVec = Vector4.zero;
				this.ValueVec = targetValueVec;
			}
			return QuaternionUtil.FromVector4(this.ValueVec, true);
		}

		// Token: 0x060054F9 RID: 21753 RVA: 0x000671FA File Offset: 0x000653FA
		public Quaternion TrackDampingRatio(Quaternion targetValue, float angularFrequency, float dampingRatio, float deltaTime)
		{
			return this.TrackDampingRatio(QuaternionUtil.ToVector4(targetValue), angularFrequency, dampingRatio, deltaTime);
		}

		// Token: 0x060054FA RID: 21754 RVA: 0x001D0954 File Offset: 0x001CEB54
		public Quaternion TrackHalfLife(Vector4 targetValueVec, float frequencyHz, float halfLife, float deltaTime)
		{
			if (halfLife < MathUtil.Epsilon)
			{
				this.VelocityVec = Vector4.zero;
				this.ValueVec = targetValueVec;
				return QuaternionUtil.FromVector4(targetValueVec, true);
			}
			float num = frequencyHz * MathUtil.TwoPi;
			float dampingRatio = 0.6931472f / (num * halfLife);
			return this.TrackDampingRatio(targetValueVec, num, dampingRatio, deltaTime);
		}

		// Token: 0x060054FB RID: 21755 RVA: 0x001D09A0 File Offset: 0x001CEBA0
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

		// Token: 0x060054FC RID: 21756 RVA: 0x001D09F0 File Offset: 0x001CEBF0
		public Quaternion TrackExponential(Vector4 targetValueVec, float halfLife, float deltaTime)
		{
			if (halfLife < MathUtil.Epsilon)
			{
				this.VelocityVec = Vector4.zero;
				this.ValueVec = targetValueVec;
				return QuaternionUtil.FromVector4(targetValueVec, true);
			}
			float angularFrequency = 0.6931472f / halfLife;
			float dampingRatio = 1f;
			return this.TrackDampingRatio(targetValueVec, angularFrequency, dampingRatio, deltaTime);
		}

		// Token: 0x060054FD RID: 21757 RVA: 0x001D0A38 File Offset: 0x001CEC38
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

		// Token: 0x040056E4 RID: 22244
		public static readonly int Stride = 32;

		// Token: 0x040056E5 RID: 22245
		public Vector4 ValueVec;

		// Token: 0x040056E6 RID: 22246
		public Vector4 VelocityVec;
	}
}
