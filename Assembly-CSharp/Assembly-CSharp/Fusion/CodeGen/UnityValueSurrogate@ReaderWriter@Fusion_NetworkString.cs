using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D0E RID: 3342
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@Fusion_NetworkString : UnityValueSurrogate<NetworkString<_32>, ReaderWriter@Fusion_NetworkString>
	{
		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x060053D7 RID: 21463 RVA: 0x0019C910 File Offset: 0x0019AB10
		// (set) Token: 0x060053D8 RID: 21464 RVA: 0x0019C918 File Offset: 0x0019AB18
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

		// Token: 0x060053D9 RID: 21465 RVA: 0x0019C921 File Offset: 0x0019AB21
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@Fusion_NetworkString()
		{
		}

		// Token: 0x04005635 RID: 22069
		[WeaverGenerated]
		public NetworkString<_32> Data;
	}
}
