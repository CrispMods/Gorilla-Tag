using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D51 RID: 3409
	[WeaverGenerated]
	[Serializable]
	internal class UnityArraySurrogate@ReaderWriter@System_Int32 : UnityArraySurrogate<int, ReaderWriter@System_Int32>
	{
		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06005563 RID: 21859 RVA: 0x000675E1 File Offset: 0x000657E1
		// (set) Token: 0x06005564 RID: 21860 RVA: 0x000675E9 File Offset: 0x000657E9
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

		// Token: 0x06005565 RID: 21861 RVA: 0x000675F2 File Offset: 0x000657F2
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@System_Int32()
		{
		}

		// Token: 0x04005820 RID: 22560
		[WeaverGenerated]
		public int[] Data = Array.Empty<int>();
	}
}
