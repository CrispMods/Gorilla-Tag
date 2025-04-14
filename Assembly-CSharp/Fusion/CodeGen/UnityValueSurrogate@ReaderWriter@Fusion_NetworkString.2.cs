using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D2C RID: 3372
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@Fusion_NetworkString : UnityValueSurrogate<NetworkString<_128>, ReaderWriter@Fusion_NetworkString>
	{
		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06005419 RID: 21529 RVA: 0x0019C7B4 File Offset: 0x0019A9B4
		// (set) Token: 0x0600541A RID: 21530 RVA: 0x0019C7BC File Offset: 0x0019A9BC
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

		// Token: 0x0600541B RID: 21531 RVA: 0x0019C7C5 File Offset: 0x0019A9C5
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@Fusion_NetworkString()
		{
		}

		// Token: 0x040058EE RID: 22766
		[WeaverGenerated]
		public NetworkString<_128> Data;
	}
}
