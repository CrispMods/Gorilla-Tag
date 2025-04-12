using System;
using System.Runtime.CompilerServices;

namespace Utilities
{
	// Token: 0x02000965 RID: 2405
	public class NetTimeAverages : DoubleAverages
	{
		// Token: 0x06003AA6 RID: 15014 RVA: 0x000556C6 File Offset: 0x000538C6
		public NetTimeAverages(int sampleCount) : base(sampleCount)
		{
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x000556CF File Offset: 0x000538CF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double DefaultTypeValue()
		{
			return 1.0;
		}
	}
}
