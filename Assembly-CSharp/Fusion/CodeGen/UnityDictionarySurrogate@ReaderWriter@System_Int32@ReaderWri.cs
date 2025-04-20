using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D60 RID: 3424
	[WeaverGenerated]
	[Serializable]
	internal class UnityDictionarySurrogate@ReaderWriter@System_Int32@ReaderWriter@System_Int32 : UnityDictionarySurrogate<int, ReaderWriter@System_Int32, int, ReaderWriter@System_Int32>
	{
		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x0600557E RID: 21886 RVA: 0x000676DE File Offset: 0x000658DE
		// (set) Token: 0x0600557F RID: 21887 RVA: 0x000676E6 File Offset: 0x000658E6
		[WeaverGenerated]
		public override SerializableDictionary<int, int> DataProperty
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

		// Token: 0x06005580 RID: 21888 RVA: 0x000676EF File Offset: 0x000658EF
		[WeaverGenerated]
		public UnityDictionarySurrogate@ReaderWriter@System_Int32@ReaderWriter@System_Int32()
		{
		}

		// Token: 0x04005A43 RID: 23107
		[WeaverGenerated]
		public SerializableDictionary<int, int> Data = SerializableDictionary.Create<int, int>();
	}
}
