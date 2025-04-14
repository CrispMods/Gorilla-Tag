using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D1D RID: 3357
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@System_Int32 : UnityValueSurrogate<int, ReaderWriter@System_Int32>
	{
		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x060053FE RID: 21502 RVA: 0x0019CB3C File Offset: 0x0019AD3C
		// (set) Token: 0x060053FF RID: 21503 RVA: 0x0019CB44 File Offset: 0x0019AD44
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

		// Token: 0x06005400 RID: 21504 RVA: 0x0019CB4D File Offset: 0x0019AD4D
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@System_Int32()
		{
		}

		// Token: 0x04005652 RID: 22098
		[WeaverGenerated]
		public int Data;
	}
}
