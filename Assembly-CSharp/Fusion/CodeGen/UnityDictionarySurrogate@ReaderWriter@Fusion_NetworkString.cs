using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D1D RID: 3357
	[WeaverGenerated]
	[Serializable]
	internal class UnityDictionarySurrogate@ReaderWriter@Fusion_NetworkString`1<Fusion__32>@ReaderWriter@Fusion_NetworkString : UnityDictionarySurrogate<NetworkString<_32>, ReaderWriter@Fusion_NetworkString, NetworkString<_32>, ReaderWriter@Fusion_NetworkString>
	{
		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x060053F5 RID: 21493 RVA: 0x0019C58D File Offset: 0x0019A78D
		// (set) Token: 0x060053F6 RID: 21494 RVA: 0x0019C595 File Offset: 0x0019A795
		[WeaverGenerated]
		public override SerializableDictionary<NetworkString<_32>, NetworkString<_32>> DataProperty
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

		// Token: 0x060053F7 RID: 21495 RVA: 0x0019C59E File Offset: 0x0019A79E
		[WeaverGenerated]
		public UnityDictionarySurrogate@ReaderWriter@Fusion_NetworkString`1<Fusion__32>@ReaderWriter@Fusion_NetworkString()
		{
		}

		// Token: 0x04005711 RID: 22289
		[WeaverGenerated]
		public SerializableDictionary<NetworkString<_32>, NetworkString<_32>> Data = SerializableDictionary.Create<NetworkString<_32>, NetworkString<_32>>();
	}
}
