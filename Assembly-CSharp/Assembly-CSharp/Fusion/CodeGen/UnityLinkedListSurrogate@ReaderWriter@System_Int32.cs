using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D37 RID: 3383
	[WeaverGenerated]
	[Serializable]
	internal class UnityLinkedListSurrogate@ReaderWriter@System_Int32 : UnityLinkedListSurrogate<int, ReaderWriter@System_Int32>
	{
		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06005434 RID: 21556 RVA: 0x0019CE38 File Offset: 0x0019B038
		// (set) Token: 0x06005435 RID: 21557 RVA: 0x0019CE40 File Offset: 0x0019B040
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

		// Token: 0x06005436 RID: 21558 RVA: 0x0019CE49 File Offset: 0x0019B049
		[WeaverGenerated]
		public UnityLinkedListSurrogate@ReaderWriter@System_Int32()
		{
		}

		// Token: 0x04005953 RID: 22867
		[WeaverGenerated]
		public int[] Data = Array.Empty<int>();
	}
}
