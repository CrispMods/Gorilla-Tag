using System;
using System.Runtime.CompilerServices;

namespace Utilities
{
	// Token: 0x02000986 RID: 2438
	public class DoubleAverages : AverageCalculator<double>
	{
		// Token: 0x06003BA8 RID: 15272 RVA: 0x00056F4C File Offset: 0x0005514C
		public DoubleAverages(int sampleCount) : base(sampleCount)
		{
			this.Reset();
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x00056F5B File Offset: 0x0005515B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double PlusEquals(double value, double sample)
		{
			return value + sample;
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x00056F60 File Offset: 0x00055160
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double MinusEquals(double value, double sample)
		{
			return value - sample;
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x00056F65 File Offset: 0x00055165
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double Divide(double value, int sampleCount)
		{
			return value / (double)sampleCount;
		}

		// Token: 0x06003BAC RID: 15276 RVA: 0x00056F6B File Offset: 0x0005516B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double Multiply(double value, int sampleCount)
		{
			return value * (double)sampleCount;
		}
	}
}
