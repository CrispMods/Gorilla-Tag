using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D34 RID: 3380
	[WeaverGenerated]
	[Serializable]
	internal class UnityLinkedListSurrogate@ReaderWriter@System_Int32 : UnityLinkedListSurrogate<int, ReaderWriter@System_Int32>
	{
		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06005428 RID: 21544 RVA: 0x0019C870 File Offset: 0x0019AA70
		// (set) Token: 0x06005429 RID: 21545 RVA: 0x0019C878 File Offset: 0x0019AA78
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

		// Token: 0x0600542A RID: 21546 RVA: 0x0019C881 File Offset: 0x0019AA81
		[WeaverGenerated]
		public UnityLinkedListSurrogate@ReaderWriter@System_Int32()
		{
		}

		// Token: 0x04005941 RID: 22849
		[WeaverGenerated]
		public int[] Data = Array.Empty<int>();
	}
}
