using System;
using System.Runtime.CompilerServices;

namespace Utilities
{
	// Token: 0x02000963 RID: 2403
	public class DoubleAverages : AverageCalculator<double>
	{
		// Token: 0x06003A9C RID: 15004 RVA: 0x00055686 File Offset: 0x00053886
		public DoubleAverages(int sampleCount) : base(sampleCount)
		{
			this.Reset();
		}

		// Token: 0x06003A9D RID: 15005 RVA: 0x00055695 File Offset: 0x00053895
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double PlusEquals(double value, double sample)
		{
			return value + sample;
		}

		// Token: 0x06003A9E RID: 15006 RVA: 0x0005569A File Offset: 0x0005389A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double MinusEquals(double value, double sample)
		{
			return value - sample;
		}

		// Token: 0x06003A9F RID: 15007 RVA: 0x0005569F File Offset: 0x0005389F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double Divide(double value, int sampleCount)
		{
			return value / (double)sampleCount;
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x000556A5 File Offset: 0x000538A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double Multiply(double value, int sampleCount)
		{
			return value * (double)sampleCount;
		}
	}
}
