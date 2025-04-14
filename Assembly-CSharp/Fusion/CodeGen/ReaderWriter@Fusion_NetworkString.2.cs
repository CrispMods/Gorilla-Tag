using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D2B RID: 3371
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@Fusion_NetworkString : IElementReaderWriter<NetworkString<_128>>
	{
		// Token: 0x06005413 RID: 21523 RVA: 0x0019C738 File Offset: 0x0019A938
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe NetworkString<_128> Read(byte* data, int index)
		{
			return *(NetworkString<_128>*)(data + index * 516);
		}

		// Token: 0x06005414 RID: 21524 RVA: 0x0019C748 File Offset: 0x0019A948
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref NetworkString<_128> ReadRef(byte* data, int index)
		{
			return ref *(NetworkString<_128>*)(data + index * 516);
		}

		// Token: 0x06005415 RID: 21525 RVA: 0x0019C753 File Offset: 0x0019A953
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, NetworkString<_128> val)
		{
			*(NetworkString<_128>*)(data + index * 516) = val;
		}

		// Token: 0x06005416 RID: 21526 RVA: 0x0019C764 File Offset: 0x0019A964
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 129;
		}

		// Token: 0x06005417 RID: 21527 RVA: 0x0019C76C File Offset: 0x0019A96C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(NetworkString<_128> val)
		{
			return val.GetHashCode();
		}

		// Token: 0x06005418 RID: 21528 RVA: 0x0019C788 File Offset: 0x0019A988
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

		// Token: 0x040058ED RID: 22765
		[WeaverGenerated]
		public static IElementReaderWriter<NetworkString<_128>> Instance;
	}
}
