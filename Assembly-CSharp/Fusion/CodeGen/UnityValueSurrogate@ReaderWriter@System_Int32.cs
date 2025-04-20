using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D4B RID: 3403
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@System_Int32 : UnityValueSurrogate<int, ReaderWriter@System_Int32>
	{
		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06005554 RID: 21844 RVA: 0x0006755C File Offset: 0x0006575C
		// (set) Token: 0x06005555 RID: 21845 RVA: 0x00067564 File Offset: 0x00065764
		[WeaverGenerated]
		public override int DataProperty
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

		// Token: 0x06005556 RID: 21846 RVA: 0x0006756D File Offset: 0x0006576D
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@System_Int32()
		{
		}

		// Token: 0x0400574C RID: 22348
		[WeaverGenerated]
		public int Data;
	}
}
