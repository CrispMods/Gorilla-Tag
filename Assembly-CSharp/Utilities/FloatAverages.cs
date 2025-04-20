using System;
using System.Runtime.CompilerServices;

namespace Utilities
{
	// Token: 0x02000987 RID: 2439
	public class FloatAverages : AverageCalculator<float>
	{
		// Token: 0x06003BAD RID: 15277 RVA: 0x00056F71 File Offset: 0x00055171
		public FloatAverages(int sampleCount) : base(sampleCount)
		{
			this.Reset();
		}

		// Token: 0x06003BAE RID: 15278 RVA: 0x00056F5B File Offset: 0x0005515B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float PlusEquals(float value, float sample)
		{
			return value + sample;
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x00056F60 File Offset: 0x00055160
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float MinusEquals(float value, float sample)
		{
			return value - sample;
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x00056F80 File Offset: 0x00055180
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Divide(float value, int sampleCount)
		{
			return value / (float)sampleCount;
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x00056F86 File Offset: 0x00055186
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Multiply(float value, int sampleCount)
		{
			return value * (float)sampleCount;
		}
	}
}
