using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D5D RID: 3421
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@Fusion_NetworkString : UnityValueSurrogate<NetworkString<_128>, ReaderWriter@Fusion_NetworkString>
	{
		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x0600557B RID: 21883 RVA: 0x000676C5 File Offset: 0x000658C5
		// (set) Token: 0x0600557C RID: 21884 RVA: 0x000676CD File Offset: 0x000658CD
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

		// Token: 0x0600557D RID: 21885 RVA: 0x000676D6 File Offset: 0x000658D6
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@Fusion_NetworkString()
		{
		}

		// Token: 0x040059FA RID: 23034
		[WeaverGenerated]
		public NetworkString<_128> Data;
	}
}
