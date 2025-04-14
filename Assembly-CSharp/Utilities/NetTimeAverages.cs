using System;
using System.Runtime.CompilerServices;

namespace Utilities
{
	// Token: 0x02000962 RID: 2402
	public class NetTimeAverages : DoubleAverages
	{
		// Token: 0x06003A9A RID: 15002 RVA: 0x0010DBDE File Offset: 0x0010BDDE
		public NetTimeAverages(int sampleCount) : base(sampleCount)
		{
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x0010DBE7 File Offset: 0x0010BDE7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override double DefaultTypeValue()
		{
			return 1.0;
		}
	}
}
