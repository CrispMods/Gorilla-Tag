using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D64 RID: 3428
	[WeaverGenerated]
	[Serializable]
	internal class UnityLinkedListSurrogate@ReaderWriter@System_Byte : UnityLinkedListSurrogate<byte, ReaderWriter@System_Byte>
	{
		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06005587 RID: 21895 RVA: 0x0006771B File Offset: 0x0006591B
		// (set) Token: 0x06005588 RID: 21896 RVA: 0x00067723 File Offset: 0x00065923
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

		// Token: 0x06005589 RID: 21897 RVA: 0x0006772C File Offset: 0x0006592C
		[WeaverGenerated]
		public UnityLinkedListSurrogate@ReaderWriter@System_Byte()
		{
		}

		// Token: 0x04005A4C RID: 23116
		[WeaverGenerated]
		public byte[] Data = Array.Empty<byte>();
	}
}
