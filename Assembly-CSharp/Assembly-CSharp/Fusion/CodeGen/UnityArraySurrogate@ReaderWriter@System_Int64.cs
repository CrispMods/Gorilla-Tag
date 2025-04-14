using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D22 RID: 3362
	[WeaverGenerated]
	[Serializable]
	internal class UnityArraySurrogate@ReaderWriter@System_Int64 : UnityArraySurrogate<long, ReaderWriter@System_Int64>
	{
		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x0600540A RID: 21514 RVA: 0x0019CBE0 File Offset: 0x0019ADE0
		// (set) Token: 0x0600540B RID: 21515 RVA: 0x0019CBE8 File Offset: 0x0019ADE8
		[WeaverGenerated]
		public override long[] DataProperty
		{
			[WeaverGenerated]
			get
			{
				return this.Data;
			}
			[WeaverGenerated]
			set
			{
				this.Data = value;
			}
		}

		// Token: 0x0600540C RID: 21516 RVA: 0x0019CBF1 File Offset: 0x0019ADF1
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@System_Int64()
		{
		}

		// Token: 0x04005725 RID: 22309
		[WeaverGenerated]
		public long[] Data = Array.Empty<long>();
	}
}
