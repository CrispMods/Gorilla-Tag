using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D19 RID: 3353
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@System_Int32 : IElementReaderWriter<int>
	{
		// Token: 0x060053EC RID: 21484 RVA: 0x0019C519 File Offset: 0x0019A719
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe int Read(byte* data, int index)
		{
			return *(int*)(data + index * 4);
		}

		// Token: 0x060053ED RID: 21485 RVA: 0x0019C1CC File Offset: 0x0019A3CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref int ReadRef(byte* data, int index)
		{
			return ref *(int*)(data + index * 4);
		}

		// Token: 0x060053EE RID: 21486 RVA: 0x0019C525 File Offset: 0x0019A725
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, int val)
		{
			*(int*)(data + index * 4) = val;
		}

		// Token: 0x060053EF RID: 21487 RVA: 0x000444E2 File Offset: 0x000426E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x060053F0 RID: 21488 RVA: 0x0019C534 File Offset: 0x0019A734
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(int val)
		{
			return val.GetHashCode();
		}

		// Token: 0x060053F1 RID: 21489 RVA: 0x0019C548 File Offset: 0x0019A748
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[WeaverGenerated]
		public static IElementReaderWriter<int> GetInstance()
		{
			if (ReaderWriter@System_Int32.Instance == null)
			{
				ReaderWriter@System_Int32.Instance = default(ReaderWriter@System_Int32);
			}
			return ReaderWriter@System_Int32.Instance;
		}

		// Token: 0x0400563F RID: 22079
		[WeaverGenerated]
		public static IElementReaderWriter<int> Instance;
	}
}
