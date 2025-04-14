using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D10 RID: 3344
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@System_Single : IElementReaderWriter<float>
	{
		// Token: 0x060053D7 RID: 21463 RVA: 0x0019C3F1 File Offset: 0x0019A5F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe float Read(byte* data, int index)
		{
			return *(float*)(data + index * 4);
		}

		// Token: 0x060053D8 RID: 21464 RVA: 0x0019C1CC File Offset: 0x0019A3CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref float ReadRef(byte* data, int index)
		{
			return ref *(float*)(data + index * 4);
		}

		// Token: 0x060053D9 RID: 21465 RVA: 0x0019C3FD File Offset: 0x0019A5FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, float val)
		{
			*(float*)(data + index * 4) = val;
		}

		// Token: 0x060053DA RID: 21466 RVA: 0x000444E2 File Offset: 0x000426E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x060053DB RID: 21467 RVA: 0x0019C40C File Offset: 0x0019A60C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(float val)
		{
			return val.GetHashCode();
		}

		// Token: 0x060053DC RID: 21468 RVA: 0x0019C420 File Offset: 0x0019A620
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

		// Token: 0x0400562A RID: 22058
		[WeaverGenerated]
		public static IElementReaderWriter<float> Instance;
	}
}
