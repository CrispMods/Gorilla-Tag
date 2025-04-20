using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D37 RID: 3383
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@Fusion_NetworkBool : IElementReaderWriter<NetworkBool>
	{
		// Token: 0x0600551E RID: 21790 RVA: 0x000673DD File Offset: 0x000655DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe NetworkBool Read(byte* data, int index)
		{
			return *(NetworkBool*)(data + index * 4);
		}

		// Token: 0x0600551F RID: 21791 RVA: 0x000673A8 File Offset: 0x000655A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref NetworkBool ReadRef(byte* data, int index)
		{
			return ref *(NetworkBool*)(data + index * 4);
		}

		// Token: 0x06005520 RID: 21792 RVA: 0x000673ED File Offset: 0x000655ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, NetworkBool val)
		{
			*(NetworkBool*)(data + index * 4) = val;
		}

		// Token: 0x06005521 RID: 21793 RVA: 0x00039846 File Offset: 0x00037A46
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x06005522 RID: 21794 RVA: 0x001D124C File Offset: 0x001CF44C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(NetworkBool val)
		{
			return val.GetHashCode();
		}

		// Token: 0x06005523 RID: 21795 RVA: 0x001D1268 File Offset: 0x001CF468
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[WeaverGenerated]
		public static IElementReaderWriter<NetworkBool> GetInstance()
		{
			if (ReaderWriter@Fusion_NetworkBool.Instance == null)
			{
				ReaderWriter@Fusion_NetworkBool.Instance = default(ReaderWriter@Fusion_NetworkBool);
			}
			return ReaderWriter@Fusion_NetworkBool.Instance;
		}

		// Token: 0x0400570A RID: 22282
		[WeaverGenerated]
		public static IElementReaderWriter<NetworkBool> Instance;
	}
}
