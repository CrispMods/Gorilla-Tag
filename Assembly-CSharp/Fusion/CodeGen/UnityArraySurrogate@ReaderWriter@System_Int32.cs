using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D20 RID: 3360
	[WeaverGenerated]
	[Serializable]
	internal class UnityArraySurrogate@ReaderWriter@System_Int32 : UnityArraySurrogate<int, ReaderWriter@System_Int32>
	{
		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06005401 RID: 21505 RVA: 0x0019C63C File Offset: 0x0019A83C
		// (set) Token: 0x06005402 RID: 21506 RVA: 0x0019C644 File Offset: 0x0019A844
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

		// Token: 0x06005403 RID: 21507 RVA: 0x0019C64D File Offset: 0x0019A84D
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@System_Int32()
		{
		}

		// Token: 0x04005714 RID: 22292
		[WeaverGenerated]
		public int[] Data = Array.Empty<int>();
	}
}
