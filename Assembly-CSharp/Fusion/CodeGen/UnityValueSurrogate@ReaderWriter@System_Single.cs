using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D11 RID: 3345
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@System_Single : UnityValueSurrogate<float, ReaderWriter@System_Single>
	{
		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x060053DD RID: 21469 RVA: 0x0019C44C File Offset: 0x0019A64C
		// (set) Token: 0x060053DE RID: 21470 RVA: 0x0019C454 File Offset: 0x0019A654
		[WeaverGenerated]
		public override float DataProperty
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

		// Token: 0x060053DF RID: 21471 RVA: 0x0019C45D File Offset: 0x0019A65D
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@System_Single()
		{
		}

		// Token: 0x0400562B RID: 22059
		[WeaverGenerated]
		public float Data;
	}
}
