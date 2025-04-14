using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D2F RID: 3375
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@Fusion_NetworkString : UnityValueSurrogate<NetworkString<_128>, ReaderWriter@Fusion_NetworkString>
	{
		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06005425 RID: 21541 RVA: 0x0019CD7C File Offset: 0x0019AF7C
		// (set) Token: 0x06005426 RID: 21542 RVA: 0x0019CD84 File Offset: 0x0019AF84
		[WeaverGenerated]
		public override NetworkString<_128> DataProperty
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

		// Token: 0x06005427 RID: 21543 RVA: 0x0019CD8D File Offset: 0x0019AF8D
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@Fusion_NetworkString()
		{
		}

		// Token: 0x04005900 RID: 22784
		[WeaverGenerated]
		public NetworkString<_128> Data;
	}
}
