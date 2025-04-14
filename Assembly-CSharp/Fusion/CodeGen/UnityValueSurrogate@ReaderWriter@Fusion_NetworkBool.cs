using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D07 RID: 3335
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@Fusion_NetworkBool : UnityValueSurrogate<NetworkBool, ReaderWriter@Fusion_NetworkBool>
	{
		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x060053C2 RID: 21442 RVA: 0x0019C2B4 File Offset: 0x0019A4B4
		// (set) Token: 0x060053C3 RID: 21443 RVA: 0x0019C2BC File Offset: 0x0019A4BC
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

		// Token: 0x060053C4 RID: 21444 RVA: 0x0019C2C5 File Offset: 0x0019A4C5
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@Fusion_NetworkBool()
		{
		}

		// Token: 0x040055FF RID: 22015
		[WeaverGenerated]
		public NetworkBool Data;
	}
}
