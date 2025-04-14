using System;
using System.Runtime.CompilerServices;

namespace Utilities
{
	// Token: 0x02000965 RID: 2405
	public class NetTimeAverages : DoubleAverages
	{
		// Token: 0x06003AA6 RID: 15014 RVA: 0x0010E1A6 File Offset: 0x0010C3A6
		public NetTimeAverages(int sampleCount) : base(sampleCount)
		{
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x0010E1AF File Offset: 0x0010C3AF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double DefaultTypeValue()
		{
			return 1.0;
		}
	}
}
