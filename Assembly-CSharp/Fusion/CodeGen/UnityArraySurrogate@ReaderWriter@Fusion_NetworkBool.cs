using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D14 RID: 3348
	[WeaverGenerated]
	[Serializable]
	internal class UnityArraySurrogate@ReaderWriter@Fusion_NetworkBool : UnityArraySurrogate<NetworkBool, ReaderWriter@Fusion_NetworkBool>
	{
		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x060053E0 RID: 21472 RVA: 0x0019C465 File Offset: 0x0019A665
		// (set) Token: 0x060053E1 RID: 21473 RVA: 0x0019C46D File Offset: 0x0019A66D
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

		// Token: 0x060053E2 RID: 21474 RVA: 0x0019C476 File Offset: 0x0019A676
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@Fusion_NetworkBool()
		{
		}

		// Token: 0x04005637 RID: 22071
		[WeaverGenerated]
		public NetworkBool[] Data = Array.Empty<NetworkBool>();
	}
}
