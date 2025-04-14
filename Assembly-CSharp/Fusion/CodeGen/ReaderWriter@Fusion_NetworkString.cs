using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D0A RID: 3338
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@Fusion_NetworkString : IElementReaderWriter<NetworkString<_32>>
	{
		// Token: 0x060053C5 RID: 21445 RVA: 0x0019C2CD File Offset: 0x0019A4CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe NetworkString<_32> Read(byte* data, int index)
		{
			return *(NetworkString<_32>*)(data + index * 132);
		}

		// Token: 0x060053C6 RID: 21446 RVA: 0x0019C2DD File Offset: 0x0019A4DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref NetworkString<_32> ReadRef(byte* data, int index)
		{
			return ref *(NetworkString<_32>*)(data + index * 132);
		}

		// Token: 0x060053C7 RID: 21447 RVA: 0x0019C2E8 File Offset: 0x0019A4E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, NetworkString<_32> val)
		{
			*(NetworkString<_32>*)(data + index * 132) = val;
		}

		// Token: 0x060053C8 RID: 21448 RVA: 0x0019C2F9 File Offset: 0x0019A4F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 33;
		}

		// Token: 0x060053C9 RID: 21449 RVA: 0x0019C300 File Offset: 0x0019A500
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(NetworkString<_32> val)
		{
			return val.GetHashCode();
		}

		// Token: 0x060053CA RID: 21450 RVA: 0x0019C31C File Offset: 0x0019A51C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[WeaverGenerated]
		public static IElementReaderWriter<NetworkString<_32>> GetInstance()
		{
			if (ReaderWriter@Fusion_NetworkString.Instance == null)
			{
				ReaderWriter@Fusion_NetworkString.Instance = default(ReaderWriter@Fusion_NetworkString);
			}
			return ReaderWriter@Fusion_NetworkString.Instance;
		}

		// Token: 0x04005622 RID: 22050
		[WeaverGenerated]
		public static IElementReaderWriter<NetworkString<_32>> Instance;
	}
}
