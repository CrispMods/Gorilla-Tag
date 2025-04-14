using System;
using System.Runtime.CompilerServices;

namespace Utilities
{
	// Token: 0x02000964 RID: 2404
	public class FloatAverages : AverageCalculator<float>
	{
		// Token: 0x06003AA1 RID: 15009 RVA: 0x0010E18B File Offset: 0x0010C38B
		public FloatAverages(int sampleCount) : base(sampleCount)
		{
			this.Reset();
		}

		// Token: 0x06003AA2 RID: 15010 RVA: 0x0010E175 File Offset: 0x0010C375
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float PlusEquals(float value, float sample)
		{
			return value + sample;
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x0010E17A File Offset: 0x0010C37A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float MinusEquals(float value, float sample)
		{
			return value - sample;
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x0010E19A File Offset: 0x0010C39A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Divide(float value, int sampleCount)
		{
			return value / (float)sampleCount;
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x0010E1A0 File Offset: 0x0010C3A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Multiply(float value, int sampleCount)
		{
			return value * (float)sampleCount;
		}
	}
}
