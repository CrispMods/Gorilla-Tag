using System;
using System.Runtime.CompilerServices;

namespace Utilities
{
	// Token: 0x02000961 RID: 2401
	public class FloatAverages : AverageCalculator<float>
	{
		// Token: 0x06003A95 RID: 14997 RVA: 0x0010DBC3 File Offset: 0x0010BDC3
		public FloatAverages(int sampleCount) : base(sampleCount)
		{
			this.Reset();
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x0010DBAD File Offset: 0x0010BDAD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float PlusEquals(float value, float sample)
		{
			return value + sample;
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x0010DBB2 File Offset: 0x0010BDB2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float MinusEquals(float value, float sample)
		{
			return value - sample;
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x0010DBD2 File Offset: 0x0010BDD2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Divide(float value, int sampleCount)
		{
			return value / (float)sampleCount;
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x0010DBD8 File Offset: 0x0010BDD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Multiply(float value, int sampleCount)
		{
			return value * (float)sampleCount;
		}
	}
}
