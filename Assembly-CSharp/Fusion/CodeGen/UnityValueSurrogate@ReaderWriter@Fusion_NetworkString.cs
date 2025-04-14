using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D0B RID: 3339
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@Fusion_NetworkString : UnityValueSurrogate<NetworkString<_32>, ReaderWriter@Fusion_NetworkString>
	{
		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x060053CB RID: 21451 RVA: 0x0019C348 File Offset: 0x0019A548
		// (set) Token: 0x060053CC RID: 21452 RVA: 0x0019C350 File Offset: 0x0019A550
		[WeaverGenerated]
		public override NetworkString<_32> DataProperty
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

		// Token: 0x060053CD RID: 21453 RVA: 0x0019C359 File Offset: 0x0019A559
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@Fusion_NetworkString()
		{
		}

		// Token: 0x04005623 RID: 22051
		[WeaverGenerated]
		public NetworkString<_32> Data;
	}
}
