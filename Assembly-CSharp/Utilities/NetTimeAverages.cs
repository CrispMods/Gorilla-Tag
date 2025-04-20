using System;
using System.Runtime.CompilerServices;

namespace Utilities
{
	// Token: 0x02000988 RID: 2440
	public class NetTimeAverages : DoubleAverages
	{
		// Token: 0x06003BB2 RID: 15282 RVA: 0x00056F8C File Offset: 0x0005518C
		public NetTimeAverages(int sampleCount) : base(sampleCount)
		{
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x00056F95 File Offset: 0x00055195
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double DefaultTypeValue()
		{
			return 1.0;
		}
	}
}
