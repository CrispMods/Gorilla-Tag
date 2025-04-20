using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D45 RID: 3397
	[WeaverGenerated]
	[Serializable]
	internal class UnityArraySurrogate@ReaderWriter@Fusion_NetworkBool : UnityArraySurrogate<NetworkBool, ReaderWriter@Fusion_NetworkBool>
	{
		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06005542 RID: 21826 RVA: 0x000674DA File Offset: 0x000656DA
		// (set) Token: 0x06005543 RID: 21827 RVA: 0x000674E2 File Offset: 0x000656E2
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

		// Token: 0x06005544 RID: 21828 RVA: 0x000674EB File Offset: 0x000656EB
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@Fusion_NetworkBool()
		{
		}

		// Token: 0x04005743 RID: 22339
		[WeaverGenerated]
		public NetworkBool[] Data = Array.Empty<NetworkBool>();
	}
}
