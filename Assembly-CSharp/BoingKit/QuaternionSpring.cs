using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CF1 RID: 3313
	public struct QuaternionSpring
	{
		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x0600538F RID: 21391 RVA: 0x0019B5B7 File Offset: 0x001997B7
		// (set) Token: 0x06005390 RID: 21392 RVA: 0x0019B5C5 File Offset: 0x001997C5
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

		// Token: 0x06005391 RID: 21393 RVA: 0x0019B5D3 File Offset: 0x001997D3
		public void Reset()
		{
			this.ValueVec = QuaternionUtil.ToVector4(Quaternion.identity);
			this.VelocityVec = Vector4.zero;
		}

		// Token: 0x06005392 RID: 21394 RVA: 0x0019B5F0 File Offset: 0x001997F0
		public void Reset(Vector4 initValue)
		{
			this.ValueVec = initValue;
			this.VelocityVec = Vector4.zero;
		}

		// Token: 0x06005393 RID: 21395 RVA: 0x0019B604 File Offset: 0x00199804
		public void Reset(Vector4 initValue, Vector4 initVelocity)
		{
			this.ValueVec = initValue;
			this.VelocityVec = initVelocity;
		}

		// Token: 0x06005394 RID: 21396 RVA: 0x0019B614 File Offset: 0x00199814
		public void Reset(Quaternion initValue)
		{
			this.ValueVec = QuaternionUtil.ToVector4(initValue);
			this.VelocityVec = Vector4.zero;
		}

		// Token: 0x06005395 RID: 21397 RVA: 0x0019B62D File Offset: 0x0019982D
		public void Reset(Quaternion initValue, Quaternion initVelocity)
		{
			this.ValueVec = QuaternionUtil.ToVector4(initValue);
			this.VelocityVec = QuaternionUtil.ToVector4(initVelocity);
		}

		// Token: 0x06005396 RID: 21398 RVA: 0x0019B648 File Offset: 0x00199848
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

		// Token: 0x06005397 RID: 21399 RVA: 0x0019B76D File Offset: 0x0019996D
		public Quaternion TrackDampingRatio(Quaternion targetValue, float angularFrequency, float dampingRatio, float deltaTime)
		{
			return this.TrackDampingRatio(QuaternionUtil.ToVector4(targetValue), angularFrequency, dampingRatio, deltaTime);
		}

		// Token: 0x06005398 RID: 21400 RVA: 0x0019B780 File Offset: 0x00199980
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

		// Token: 0x06005399 RID: 21401 RVA: 0x0019B7CC File Offset: 0x001999CC
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

		// Token: 0x0600539A RID: 21402 RVA: 0x0019B81C File Offset: 0x00199A1C
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

		// Token: 0x0600539B RID: 21403 RVA: 0x0019B864 File Offset: 0x00199A64
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

		// Token: 0x040055D8 RID: 21976
		public static readonly int Stride = 32;

		// Token: 0x040055D9 RID: 21977
		public Vector4 ValueVec;

		// Token: 0x040055DA RID: 21978
		public Vector4 VelocityVec;
	}
}
