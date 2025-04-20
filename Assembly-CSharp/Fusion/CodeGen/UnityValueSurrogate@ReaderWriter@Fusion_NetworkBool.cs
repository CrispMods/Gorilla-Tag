using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D38 RID: 3384
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@Fusion_NetworkBool : UnityValueSurrogate<NetworkBool, ReaderWriter@Fusion_NetworkBool>
	{
		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06005524 RID: 21796 RVA: 0x000673FE File Offset: 0x000655FE
		// (set) Token: 0x06005525 RID: 21797 RVA: 0x00067406 File Offset: 0x00065606
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

		// Token: 0x06005526 RID: 21798 RVA: 0x0006740F File Offset: 0x0006560F
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@Fusion_NetworkBool()
		{
		}

		// Token: 0x0400570B RID: 22283
		[WeaverGenerated]
		public NetworkBool Data;
	}
}
