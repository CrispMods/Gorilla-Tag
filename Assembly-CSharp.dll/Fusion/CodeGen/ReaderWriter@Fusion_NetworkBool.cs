using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D09 RID: 3337
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@Fusion_NetworkBool : IElementReaderWriter<NetworkBool>
	{
		// Token: 0x060053C8 RID: 21448 RVA: 0x00065967 File Offset: 0x00063B67
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe NetworkBool Read(byte* data, int index)
		{
			return *(NetworkBool*)(data + index * 4);
		}

		// Token: 0x060053C9 RID: 21449 RVA: 0x00065932 File Offset: 0x00063B32
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref NetworkBool ReadRef(byte* data, int index)
		{
			return ref *(NetworkBool*)(data + index * 4);
		}

		// Token: 0x060053CA RID: 21450 RVA: 0x00065977 File Offset: 0x00063B77
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, NetworkBool val)
		{
			*(NetworkBool*)(data + index * 4) = val;
		}

		// Token: 0x060053CB RID: 21451 RVA: 0x00038586 File Offset: 0x00036786
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x060053CC RID: 21452 RVA: 0x001C9168 File Offset: 0x001C7368
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(NetworkBool val)
		{
			return val.GetHashCode();
		}

		// Token: 0x060053CD RID: 21453 RVA: 0x001C9184 File Offset: 0x001C7384
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

		// Token: 0x04005610 RID: 22032
		[WeaverGenerated]
		public static IElementReaderWriter<NetworkBool> Instance;
	}
}
