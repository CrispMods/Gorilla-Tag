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
		// (get) Token: 0x060053EC RID: 21484 RVA: 0x00065A64 File Offset: 0x00063C64
		// (set) Token: 0x060053ED RID: 21485 RVA: 0x00065A6C File Offset: 0x00063C6C
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

		// Token: 0x060053EE RID: 21486 RVA: 0x00065A75 File Offset: 0x00063C75
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@Fusion_NetworkBool()
		{
		}

		// Token: 0x04005649 RID: 22089
		[WeaverGenerated]
		public NetworkBool[] Data = Array.Empty<NetworkBool>();
	}
}
