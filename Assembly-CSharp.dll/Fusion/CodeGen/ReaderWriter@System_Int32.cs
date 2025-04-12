using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D1C RID: 3356
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@System_Int32 : IElementReaderWriter<int>
	{
		// Token: 0x060053F8 RID: 21496 RVA: 0x00065ACD File Offset: 0x00063CCD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe int Read(byte* data, int index)
		{
			return *(int*)(data + index * 4);
		}

		// Token: 0x060053F9 RID: 21497 RVA: 0x00065932 File Offset: 0x00063B32
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref int ReadRef(byte* data, int index)
		{
			return ref *(int*)(data + index * 4);
		}

		// Token: 0x060053FA RID: 21498 RVA: 0x00065AD9 File Offset: 0x00063CD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, int val)
		{
			*(int*)(data + index * 4) = val;
		}

		// Token: 0x060053FB RID: 21499 RVA: 0x00038586 File Offset: 0x00036786
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x060053FC RID: 21500 RVA: 0x001C92C8 File Offset: 0x001C74C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(int val)
		{
			return val.GetHashCode();
		}

		// Token: 0x060053FD RID: 21501 RVA: 0x001C92DC File Offset: 0x001C74DC
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

		// Token: 0x04005651 RID: 22097
		[WeaverGenerated]
		public static IElementReaderWriter<int> Instance;
	}
}
