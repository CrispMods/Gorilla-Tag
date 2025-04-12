using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D35 RID: 3381
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@System_Byte : IElementReaderWriter<byte>
	{
		// Token: 0x0600542B RID: 21547 RVA: 0x00065C8C File Offset: 0x00063E8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe byte Read(byte* data, int index)
		{
			return data[index * 4];
		}

		// Token: 0x0600542C RID: 21548 RVA: 0x00065932 File Offset: 0x00063B32
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref byte ReadRef(byte* data, int index)
		{
			return ref data[index * 4];
		}

		// Token: 0x0600542D RID: 21549 RVA: 0x00065C98 File Offset: 0x00063E98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, byte val)
		{
			data[index * 4] = val;
		}

		// Token: 0x0600542E RID: 21550 RVA: 0x00038586 File Offset: 0x00036786
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x0600542F RID: 21551 RVA: 0x001C93D8 File Offset: 0x001C75D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(byte val)
		{
			return val.GetHashCode();
		}

		// Token: 0x06005430 RID: 21552 RVA: 0x001C93EC File Offset: 0x001C75EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[WeaverGenerated]
		public static IElementReaderWriter<byte> GetInstance()
		{
			if (ReaderWriter@System_Byte.Instance == null)
			{
				ReaderWriter@System_Byte.Instance = default(ReaderWriter@System_Byte);
			}
			return ReaderWriter@System_Byte.Instance;
		}

		// Token: 0x04005951 RID: 22865
		[WeaverGenerated]
		public static IElementReaderWriter<byte> Instance;
	}
}
