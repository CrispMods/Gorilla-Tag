using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D36 RID: 3382
	[WeaverGenerated]
	[Serializable]
	internal class UnityLinkedListSurrogate@ReaderWriter@System_Byte : UnityLinkedListSurrogate<byte, ReaderWriter@System_Byte>
	{
		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06005431 RID: 21553 RVA: 0x00065CA5 File Offset: 0x00063EA5
		// (set) Token: 0x06005432 RID: 21554 RVA: 0x00065CAD File Offset: 0x00063EAD
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

		// Token: 0x06005433 RID: 21555 RVA: 0x00065CB6 File Offset: 0x00063EB6
		[WeaverGenerated]
		public UnityLinkedListSurrogate@ReaderWriter@System_Byte()
		{
		}

		// Token: 0x04005952 RID: 22866
		[WeaverGenerated]
		public byte[] Data = Array.Empty<byte>();
	}
}
