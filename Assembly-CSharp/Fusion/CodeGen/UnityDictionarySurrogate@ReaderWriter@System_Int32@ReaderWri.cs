using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D2F RID: 3375
	[WeaverGenerated]
	[Serializable]
	internal class UnityDictionarySurrogate@ReaderWriter@System_Int32@ReaderWriter@System_Int32 : UnityDictionarySurrogate<int, ReaderWriter@System_Int32, int, ReaderWriter@System_Int32>
	{
		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x0600541C RID: 21532 RVA: 0x0019C7CD File Offset: 0x0019A9CD
		// (set) Token: 0x0600541D RID: 21533 RVA: 0x0019C7D5 File Offset: 0x0019A9D5
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

		// Token: 0x0600541E RID: 21534 RVA: 0x0019C7DE File Offset: 0x0019A9DE
		[WeaverGenerated]
		public UnityDictionarySurrogate@ReaderWriter@System_Int32@ReaderWriter@System_Int32()
		{
		}

		// Token: 0x04005937 RID: 22839
		[WeaverGenerated]
		public SerializableDictionary<int, int> Data = SerializableDictionary.Create<int, int>();
	}
}
