using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D3B RID: 3387
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@Fusion_NetworkString : IElementReaderWriter<NetworkString<_32>>
	{
		// Token: 0x06005527 RID: 21799 RVA: 0x00067417 File Offset: 0x00065617
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe NetworkString<_32> Read(byte* data, int index)
		{
			return *(NetworkString<_32>*)(data + index * 132);
		}

		// Token: 0x06005528 RID: 21800 RVA: 0x00067427 File Offset: 0x00065627
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref NetworkString<_32> ReadRef(byte* data, int index)
		{
			return ref *(NetworkString<_32>*)(data + index * 132);
		}

		// Token: 0x06005529 RID: 21801 RVA: 0x00067432 File Offset: 0x00065632
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, NetworkString<_32> val)
		{
			*(NetworkString<_32>*)(data + index * 132) = val;
		}

		// Token: 0x0600552A RID: 21802 RVA: 0x00067443 File Offset: 0x00065643
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 33;
		}

		// Token: 0x0600552B RID: 21803 RVA: 0x001D1294 File Offset: 0x001CF494
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(NetworkString<_32> val)
		{
			return val.GetHashCode();
		}

		// Token: 0x0600552C RID: 21804 RVA: 0x001D12B0 File Offset: 0x001CF4B0
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

		// Token: 0x0400572E RID: 22318
		[WeaverGenerated]
		public static IElementReaderWriter<NetworkString<_32>> Instance;
	}
}
