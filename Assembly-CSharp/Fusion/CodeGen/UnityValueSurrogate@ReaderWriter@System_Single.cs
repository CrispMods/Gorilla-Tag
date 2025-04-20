using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D42 RID: 3394
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@System_Single : UnityValueSurrogate<float, ReaderWriter@System_Single>
	{
		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x0600553F RID: 21823 RVA: 0x000674C1 File Offset: 0x000656C1
		// (set) Token: 0x06005540 RID: 21824 RVA: 0x000674C9 File Offset: 0x000656C9
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

		// Token: 0x06005541 RID: 21825 RVA: 0x000674D2 File Offset: 0x000656D2
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@System_Single()
		{
		}

		// Token: 0x04005737 RID: 22327
		[WeaverGenerated]
		public float Data;
	}
}
