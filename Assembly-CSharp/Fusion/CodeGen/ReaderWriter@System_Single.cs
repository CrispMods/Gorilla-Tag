using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D41 RID: 3393
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@System_Single : IElementReaderWriter<float>
	{
		// Token: 0x06005539 RID: 21817 RVA: 0x000674A8 File Offset: 0x000656A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe float Read(byte* data, int index)
		{
			return *(float*)(data + index * 4);
		}

		// Token: 0x0600553A RID: 21818 RVA: 0x000673A8 File Offset: 0x000655A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref float ReadRef(byte* data, int index)
		{
			return ref *(float*)(data + index * 4);
		}

		// Token: 0x0600553B RID: 21819 RVA: 0x000674B4 File Offset: 0x000656B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, float val)
		{
			*(float*)(data + index * 4) = val;
		}

		// Token: 0x0600553C RID: 21820 RVA: 0x00039846 File Offset: 0x00037A46
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x0600553D RID: 21821 RVA: 0x001D1324 File Offset: 0x001CF524
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(float val)
		{
			return val.GetHashCode();
		}

		// Token: 0x0600553E RID: 21822 RVA: 0x001D1338 File Offset: 0x001CF538
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[WeaverGenerated]
		public static IElementReaderWriter<float> GetInstance()
		{
			if (ReaderWriter@System_Single.Instance == null)
			{
				ReaderWriter@System_Single.Instance = default(ReaderWriter@System_Single);
			}
			return ReaderWriter@System_Single.Instance;
		}

		// Token: 0x04005736 RID: 22326
		[WeaverGenerated]
		public static IElementReaderWriter<float> Instance;
	}
}
