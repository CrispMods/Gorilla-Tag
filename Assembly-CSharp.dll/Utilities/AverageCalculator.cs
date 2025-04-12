using System;
using System.Runtime.CompilerServices;

namespace Utilities
{
	// Token: 0x02000962 RID: 2402
	public abstract class AverageCalculator<T> where T : struct
	{
		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06003A93 RID: 14995 RVA: 0x0005566A File Offset: 0x0005386A
		public T Average
		{
			get
			{
				return this.m_average;
			}
		}

		// Token: 0x06003A94 RID: 14996 RVA: 0x00055672 File Offset: 0x00053872
		public AverageCalculator(int sampleCount)
		{
			this.m_samples = new T[sampleCount];
		}

		// Token: 0x06003A95 RID: 14997 RVA: 0x0014B644 File Offset: 0x00149844
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

		// Token: 0x06003A96 RID: 14998 RVA: 0x0014B6D8 File Offset: 0x001498D8
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

		// Token: 0x06003A97 RID: 14999 RVA: 0x0014B730 File Offset: 0x00149930
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual T DefaultTypeValue()
		{
			return default(T);
		}

		// Token: 0x06003A98 RID: 15000
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract T PlusEquals(T value, T sample);

		// Token: 0x06003A99 RID: 15001
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract T MinusEquals(T value, T sample);

		// Token: 0x06003A9A RID: 15002
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract T Divide(T value, int sampleCount);

		// Token: 0x06003A9B RID: 15003
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract T Multiply(T value, int sampleCount);

		// Token: 0x04003BBF RID: 15295
		private T[] m_samples;

		// Token: 0x04003BC0 RID: 15296
		private T m_average;

		// Token: 0x04003BC1 RID: 15297
		private T m_total;

		// Token: 0x04003BC2 RID: 15298
		private int m_index;
	}
}
