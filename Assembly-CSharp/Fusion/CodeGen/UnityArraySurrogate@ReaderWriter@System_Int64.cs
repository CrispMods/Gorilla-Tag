using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D50 RID: 3408
	[WeaverGenerated]
	[Serializable]
	internal class UnityArraySurrogate@ReaderWriter@System_Int64 : UnityArraySurrogate<long, ReaderWriter@System_Int64>
	{
		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06005560 RID: 21856 RVA: 0x000675BD File Offset: 0x000657BD
		// (set) Token: 0x06005561 RID: 21857 RVA: 0x000675C5 File Offset: 0x000657C5
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

		// Token: 0x06005562 RID: 21858 RVA: 0x000675CE File Offset: 0x000657CE
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@System_Int64()
		{
		}

		// Token: 0x0400581F RID: 22559
		[WeaverGenerated]
		public long[] Data = Array.Empty<long>();
	}
}
