using System;
using System.Runtime.CompilerServices;

namespace Utilities
{
	// Token: 0x02000960 RID: 2400
	public class DoubleAverages : AverageCalculator<double>
	{
		// Token: 0x06003A90 RID: 14992 RVA: 0x0010DB9E File Offset: 0x0010BD9E
		public DoubleAverages(int sampleCount) : base(sampleCount)
		{
			this.Reset();
		}

		// Token: 0x06003A91 RID: 14993 RVA: 0x0010DBAD File Offset: 0x0010BDAD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double PlusEquals(double value, double sample)
		{
			return value + sample;
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x0010DBB2 File Offset: 0x0010BDB2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double MinusEquals(double value, double sample)
		{
			return value - sample;
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x0010DBB7 File Offset: 0x0010BDB7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double Divide(double value, int sampleCount)
		{
			return value / (double)sampleCount;
		}

		// Token: 0x06003A94 RID: 14996 RVA: 0x0010DBBD File Offset: 0x0010BDBD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double Multiply(double value, int sampleCount)
		{
			return value * (double)sampleCount;
		}
	}
}
