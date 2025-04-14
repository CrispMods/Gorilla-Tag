using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D17 RID: 3351
	[WeaverGenerated]
	[Serializable]
	internal class UnityArraySurrogate@ReaderWriter@Fusion_NetworkBool : UnityArraySurrogate<NetworkBool, ReaderWriter@Fusion_NetworkBool>
	{
		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x060053EC RID: 21484 RVA: 0x0019CA2D File Offset: 0x0019AC2D
		// (set) Token: 0x060053ED RID: 21485 RVA: 0x0019CA35 File Offset: 0x0019AC35
		[WeaverGenerated]
		public override NetworkBool[] DataProperty
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

		// Token: 0x060053EE RID: 21486 RVA: 0x0019CA3E File Offset: 0x0019AC3E
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@Fusion_NetworkBool()
		{
		}

		// Token: 0x04005649 RID: 22089
		[WeaverGenerated]
		public NetworkBool[] Data = Array.Empty<NetworkBool>();
	}
}
