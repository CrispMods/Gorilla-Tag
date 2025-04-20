using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D63 RID: 3427
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@System_Byte : IElementReaderWriter<byte>
	{
		// Token: 0x06005581 RID: 21889 RVA: 0x00067702 File Offset: 0x00065902
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe byte Read(byte* data, int index)
		{
			return data[index * 4];
		}

		// Token: 0x06005582 RID: 21890 RVA: 0x000673A8 File Offset: 0x000655A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref byte ReadRef(byte* data, int index)
		{
			return ref data[index * 4];
		}

		// Token: 0x06005583 RID: 21891 RVA: 0x0006770E File Offset: 0x0006590E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, byte val)
		{
			data[index * 4] = val;
		}

		// Token: 0x06005584 RID: 21892 RVA: 0x00039846 File Offset: 0x00037A46
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x06005585 RID: 21893 RVA: 0x001D14BC File Offset: 0x001CF6BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(byte val)
		{
			return val.GetHashCode();
		}

		// Token: 0x06005586 RID: 21894 RVA: 0x001D14D0 File Offset: 0x001CF6D0
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

		// Token: 0x04005A4B RID: 23115
		[WeaverGenerated]
		public static IElementReaderWriter<byte> Instance;
	}
}
