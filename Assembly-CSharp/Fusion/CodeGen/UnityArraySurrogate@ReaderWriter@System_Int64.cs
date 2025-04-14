using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D1F RID: 3359
	[WeaverGenerated]
	[Serializable]
	internal class UnityArraySurrogate@ReaderWriter@System_Int64 : UnityArraySurrogate<long, ReaderWriter@System_Int64>
	{
		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x060053FE RID: 21502 RVA: 0x0019C618 File Offset: 0x0019A818
		// (set) Token: 0x060053FF RID: 21503 RVA: 0x0019C620 File Offset: 0x0019A820
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

		// Token: 0x06005400 RID: 21504 RVA: 0x0019C629 File Offset: 0x0019A829
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@System_Int64()
		{
		}

		// Token: 0x04005713 RID: 22291
		[WeaverGenerated]
		public long[] Data = Array.Empty<long>();
	}
}
