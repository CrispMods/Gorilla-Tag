using System;
using System.Runtime.CompilerServices;

namespace Utilities
{
	// Token: 0x02000964 RID: 2404
	public class FloatAverages : AverageCalculator<float>
	{
		// Token: 0x06003AA1 RID: 15009 RVA: 0x000556AB File Offset: 0x000538AB
		public FloatAverages(int sampleCount) : base(sampleCount)
		{
			this.Reset();
		}

		// Token: 0x06003AA2 RID: 15010 RVA: 0x00055695 File Offset: 0x00053895
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float PlusEquals(float value, float sample)
		{
			return value + sample;
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x0005569A File Offset: 0x0005389A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float MinusEquals(float value, float sample)
		{
			return value - sample;
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x000556BA File Offset: 0x000538BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Divide(float value, int sampleCount)
		{
			return value / (float)sampleCount;
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x000556C0 File Offset: 0x000538C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Multiply(float value, int sampleCount)
		{
			return value * (float)sampleCount;
		}
	}
}
