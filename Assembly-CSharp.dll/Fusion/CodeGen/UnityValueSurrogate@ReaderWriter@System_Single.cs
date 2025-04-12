using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D14 RID: 3348
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@System_Single : UnityValueSurrogate<float, ReaderWriter@System_Single>
	{
		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x060053E9 RID: 21481 RVA: 0x00065A4B File Offset: 0x00063C4B
		// (set) Token: 0x060053EA RID: 21482 RVA: 0x00065A53 File Offset: 0x00063C53
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

		// Token: 0x060053EB RID: 21483 RVA: 0x00065A5C File Offset: 0x00063C5C
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@System_Single()
		{
		}

		// Token: 0x0400563D RID: 22077
		[WeaverGenerated]
		public float Data;
	}
}
