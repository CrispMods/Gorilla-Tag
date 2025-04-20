using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D3C RID: 3388
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@Fusion_NetworkString : UnityValueSurrogate<NetworkString<_32>, ReaderWriter@Fusion_NetworkString>
	{
		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x0600552D RID: 21805 RVA: 0x0006744A File Offset: 0x0006564A
		// (set) Token: 0x0600552E RID: 21806 RVA: 0x00067452 File Offset: 0x00065652
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

		// Token: 0x0600552F RID: 21807 RVA: 0x0006745B File Offset: 0x0006565B
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@Fusion_NetworkString()
		{
		}

		// Token: 0x0400572F RID: 22319
		[WeaverGenerated]
		public NetworkString<_32> Data;
	}
}
