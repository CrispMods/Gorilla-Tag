using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D65 RID: 3429
	[WeaverGenerated]
	[Serializable]
	internal class UnityLinkedListSurrogate@ReaderWriter@System_Int32 : UnityLinkedListSurrogate<int, ReaderWriter@System_Int32>
	{
		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x0600558A RID: 21898 RVA: 0x0006773F File Offset: 0x0006593F
		// (set) Token: 0x0600558B RID: 21899 RVA: 0x00067747 File Offset: 0x00065947
		[WeaverGenerated]
		public override int[] DataProperty
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

		// Token: 0x0600558C RID: 21900 RVA: 0x00067750 File Offset: 0x00065950
		[WeaverGenerated]
		public UnityLinkedListSurrogate@ReaderWriter@System_Int32()
		{
		}

		// Token: 0x04005A4D RID: 23117
		[WeaverGenerated]
		public int[] Data = Array.Empty<int>();
	}
}
