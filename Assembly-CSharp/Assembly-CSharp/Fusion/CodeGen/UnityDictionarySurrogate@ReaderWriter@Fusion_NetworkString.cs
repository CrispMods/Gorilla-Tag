using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D20 RID: 3360
	[WeaverGenerated]
	[Serializable]
	internal class UnityDictionarySurrogate@ReaderWriter@Fusion_NetworkString`1<Fusion__32>@ReaderWriter@Fusion_NetworkString : UnityDictionarySurrogate<NetworkString<_32>, ReaderWriter@Fusion_NetworkString, NetworkString<_32>, ReaderWriter@Fusion_NetworkString>
	{
		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06005401 RID: 21505 RVA: 0x0019CB55 File Offset: 0x0019AD55
		// (set) Token: 0x06005402 RID: 21506 RVA: 0x0019CB5D File Offset: 0x0019AD5D
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

		// Token: 0x06005403 RID: 21507 RVA: 0x0019CB66 File Offset: 0x0019AD66
		[WeaverGenerated]
		public UnityDictionarySurrogate@ReaderWriter@Fusion_NetworkString`1<Fusion__32>@ReaderWriter@Fusion_NetworkString()
		{
		}

		// Token: 0x04005723 RID: 22307
		[WeaverGenerated]
		public SerializableDictionary<NetworkString<_32>, NetworkString<_32>> Data = SerializableDictionary.Create<NetworkString<_32>, NetworkString<_32>>();
	}
}
