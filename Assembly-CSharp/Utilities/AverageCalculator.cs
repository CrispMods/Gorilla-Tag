using System;
using System.Runtime.CompilerServices;

namespace Utilities
{
	// Token: 0x0200095F RID: 2399
	public abstract class AverageCalculator<T> where T : struct
	{
		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06003A87 RID: 14983 RVA: 0x0010DA7E File Offset: 0x0010BC7E
		public T Average
		{
			get
			{
				return this.m_average;
			}
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x0010DA86 File Offset: 0x0010BC86
		public AverageCalculator(int sampleCount)
		{
			this.m_samples = new T[sampleCount];
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x0010DA9C File Offset: 0x0010BC9C
		public virtual void AddSample(T sample)
		{
			T sample2 = this.m_samples[this.m_index];
			this.m_total = this.MinusEquals(this.m_total, sample2);
			this.m_total = this.PlusEquals(this.m_total, sample);
			this.m_average = this.Divide(this.m_total, this.m_samples.Length);
			this.m_samples[this.m_index] = sample;
			int num = this.m_index + 1;
			this.m_index = num;
			this.m_index = num % this.m_samples.Length;
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x0010DB30 File Offset: 0x0010BD30
		public virtual void Reset()
		{
			T t = this.DefaultTypeValue();
			for (int i = 0; i < this.m_samples.Length; i++)
			{
				this.m_samples[i] = t;
			}
			this.m_index = 0;
			this.m_average = t;
			this.m_total = this.Multiply(t, this.m_samples.Length);
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x0010DB88 File Offset: 0x0010BD88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual T DefaultTypeValue()
		{
			return default(T);
		}

		// Token: 0x06003A8C RID: 14988
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract T PlusEquals(T value, T sample);

		// Token: 0x06003A8D RID: 14989
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract T MinusEquals(T value, T sample);

		// Token: 0x06003A8E RID: 14990
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract T Divide(T value, int sampleCount);

		// Token: 0x06003A8F RID: 14991
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract T Multiply(T value, int sampleCount);

		// Token: 0x04003BAD RID: 15277
		private T[] m_samples;

		// Token: 0x04003BAE RID: 15278
		private T m_average;

		// Token: 0x04003BAF RID: 15279
		private T m_total;

		// Token: 0x04003BB0 RID: 15280
		private int m_index;
	}
}
