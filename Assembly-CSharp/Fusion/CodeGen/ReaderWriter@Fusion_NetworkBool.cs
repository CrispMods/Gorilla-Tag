using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D06 RID: 3334
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@Fusion_NetworkBool : IElementReaderWriter<NetworkBool>
	{
		// Token: 0x060053BC RID: 21436 RVA: 0x0019C249 File Offset: 0x0019A449
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe NetworkBool Read(byte* data, int index)
		{
			return *(NetworkBool*)(data + index * 4);
		}

		// Token: 0x060053BD RID: 21437 RVA: 0x0019C1CC File Offset: 0x0019A3CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref NetworkBool ReadRef(byte* data, int index)
		{
			return ref *(NetworkBool*)(data + index * 4);
		}

		// Token: 0x060053BE RID: 21438 RVA: 0x0019C259 File Offset: 0x0019A459
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, NetworkBool val)
		{
			*(NetworkBool*)(data + index * 4) = val;
		}

		// Token: 0x060053BF RID: 21439 RVA: 0x000444E2 File Offset: 0x000426E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x060053C0 RID: 21440 RVA: 0x0019C26C File Offset: 0x0019A46C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(NetworkBool val)
		{
			return val.GetHashCode();
		}

		// Token: 0x060053C1 RID: 21441 RVA: 0x0019C288 File Offset: 0x0019A488
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

		// Token: 0x040055FE RID: 22014
		[WeaverGenerated]
		public static IElementReaderWriter<NetworkBool> Instance;
	}
}
