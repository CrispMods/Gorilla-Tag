using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D0A RID: 3338
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@Fusion_NetworkBool : UnityValueSurrogate<NetworkBool, ReaderWriter@Fusion_NetworkBool>
	{
		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x060053CE RID: 21454 RVA: 0x00065988 File Offset: 0x00063B88
		// (set) Token: 0x060053CF RID: 21455 RVA: 0x00065990 File Offset: 0x00063B90
		[WeaverGenerated]
		public override NetworkBool DataProperty
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

		// Token: 0x060053D0 RID: 21456 RVA: 0x00065999 File Offset: 0x00063B99
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@Fusion_NetworkBool()
		{
		}

		// Token: 0x04005611 RID: 22033
		[WeaverGenerated]
		public NetworkBool Data;
	}
}
