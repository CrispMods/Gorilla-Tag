using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D32 RID: 3378
	[WeaverGenerated]
	[Serializable]
	internal class UnityDictionarySurrogate@ReaderWriter@System_Int32@ReaderWriter@System_Int32 : UnityDictionarySurrogate<int, ReaderWriter@System_Int32, int, ReaderWriter@System_Int32>
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06005428 RID: 21544 RVA: 0x00065C68 File Offset: 0x00063E68
		// (set) Token: 0x06005429 RID: 21545 RVA: 0x00065C70 File Offset: 0x00063E70
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

		// Token: 0x0600542A RID: 21546 RVA: 0x00065C79 File Offset: 0x00063E79
		[WeaverGenerated]
		public UnityDictionarySurrogate@ReaderWriter@System_Int32@ReaderWriter@System_Int32()
		{
		}

		// Token: 0x04005949 RID: 22857
		[WeaverGenerated]
		public SerializableDictionary<int, int> Data = SerializableDictionary.Create<int, int>();
	}
}
