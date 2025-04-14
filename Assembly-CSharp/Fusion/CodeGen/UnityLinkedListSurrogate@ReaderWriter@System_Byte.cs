using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D33 RID: 3379
	[WeaverGenerated]
	[Serializable]
	internal class UnityLinkedListSurrogate@ReaderWriter@System_Byte : UnityLinkedListSurrogate<byte, ReaderWriter@System_Byte>
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06005425 RID: 21541 RVA: 0x0019C84C File Offset: 0x0019AA4C
		// (set) Token: 0x06005426 RID: 21542 RVA: 0x0019C854 File Offset: 0x0019AA54
		[WeaverGenerated]
		public override byte[] DataProperty
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

		// Token: 0x06005427 RID: 21543 RVA: 0x0019C85D File Offset: 0x0019AA5D
		[WeaverGenerated]
		public UnityLinkedListSurrogate@ReaderWriter@System_Byte()
		{
		}

		// Token: 0x04005940 RID: 22848
		[WeaverGenerated]
		public byte[] Data = Array.Empty<byte>();
	}
}
