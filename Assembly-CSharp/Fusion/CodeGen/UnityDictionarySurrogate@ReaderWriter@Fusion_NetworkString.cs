using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D4E RID: 3406
	[WeaverGenerated]
	[Serializable]
	internal class UnityDictionarySurrogate@ReaderWriter@Fusion_NetworkString`1<Fusion__32>@ReaderWriter@Fusion_NetworkString : UnityDictionarySurrogate<NetworkString<_32>, ReaderWriter@Fusion_NetworkString, NetworkString<_32>, ReaderWriter@Fusion_NetworkString>
	{
		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06005557 RID: 21847 RVA: 0x00067575 File Offset: 0x00065775
		// (set) Token: 0x06005558 RID: 21848 RVA: 0x0006757D File Offset: 0x0006577D
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

		// Token: 0x06005559 RID: 21849 RVA: 0x00067586 File Offset: 0x00065786
		[WeaverGenerated]
		public UnityDictionarySurrogate@ReaderWriter@Fusion_NetworkString`1<Fusion__32>@ReaderWriter@Fusion_NetworkString()
		{
		}

		// Token: 0x0400581D RID: 22557
		[WeaverGenerated]
		public SerializableDictionary<NetworkString<_32>, NetworkString<_32>> Data = SerializableDictionary.Create<NetworkString<_32>, NetworkString<_32>>();
	}
}
