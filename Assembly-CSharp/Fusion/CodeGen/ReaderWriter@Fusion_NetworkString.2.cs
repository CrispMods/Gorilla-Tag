using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D5C RID: 3420
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@Fusion_NetworkString : IElementReaderWriter<NetworkString<_128>>
	{
		// Token: 0x06005575 RID: 21877 RVA: 0x00067692 File Offset: 0x00065892
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe NetworkString<_128> Read(byte* data, int index)
		{
			return *(NetworkString<_128>*)(data + index * 516);
		}

		// Token: 0x06005576 RID: 21878 RVA: 0x000676A2 File Offset: 0x000658A2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref NetworkString<_128> ReadRef(byte* data, int index)
		{
			return ref *(NetworkString<_128>*)(data + index * 516);
		}

		// Token: 0x06005577 RID: 21879 RVA: 0x000676AD File Offset: 0x000658AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, NetworkString<_128> val)
		{
			*(NetworkString<_128>*)(data + index * 516) = val;
		}

		// Token: 0x06005578 RID: 21880 RVA: 0x000676BE File Offset: 0x000658BE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 129;
		}

		// Token: 0x06005579 RID: 21881 RVA: 0x001D1474 File Offset: 0x001CF674
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(NetworkString<_128> val)
		{
			return val.GetHashCode();
		}

		// Token: 0x0600557A RID: 21882 RVA: 0x001D1490 File Offset: 0x001CF690
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[WeaverGenerated]
		public static IElementReaderWriter<NetworkString<_128>> GetInstance()
		{
			if (ReaderWriter@Fusion_NetworkString.Instance == null)
			{
				ReaderWriter@Fusion_NetworkString.Instance = default(ReaderWriter@Fusion_NetworkString);
			}
			return ReaderWriter@Fusion_NetworkString.Instance;
		}

		// Token: 0x040059F9 RID: 23033
		[WeaverGenerated]
		public static IElementReaderWriter<NetworkString<_128>> Instance;
	}
}
