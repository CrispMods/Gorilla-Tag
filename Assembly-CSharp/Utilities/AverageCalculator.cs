using System;
using System.Runtime.CompilerServices;

namespace Utilities
{
	// Token: 0x02000985 RID: 2437
	public abstract class AverageCalculator<T> where T : struct
	{
		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06003B9F RID: 15263 RVA: 0x00056F30 File Offset: 0x00055130
		public T Average
		{
			get
			{
				return this.m_average;
			}
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x00056F38 File Offset: 0x00055138
		public AverageCalculator(int sampleCount)
		{
			this.m_samples = new T[sampleCount];
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x001515E0 File Offset: 0x0014F7E0
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

		// Token: 0x06003BA2 RID: 15266 RVA: 0x00151674 File Offset: 0x0014F874
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

		// Token: 0x06003BA3 RID: 15267 RVA: 0x001516CC File Offset: 0x0014F8CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual T DefaultTypeValue()
		{
			return default(T);
		}

		// Token: 0x06003BA4 RID: 15268
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract T PlusEquals(T value, T sample);

		// Token: 0x06003BA5 RID: 15269
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract T MinusEquals(T value, T sample);

		// Token: 0x06003BA6 RID: 15270
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract T Divide(T value, int sampleCount);

		// Token: 0x06003BA7 RID: 15271
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract T Multiply(T value, int sampleCount);

		// Token: 0x04003C87 RID: 15495
		private T[] m_samples;

		// Token: 0x04003C88 RID: 15496
		private T m_average;

		// Token: 0x04003C89 RID: 15497
		private T m_total;

		// Token: 0x04003C8A RID: 15498
		private int m_index;
	}
}
