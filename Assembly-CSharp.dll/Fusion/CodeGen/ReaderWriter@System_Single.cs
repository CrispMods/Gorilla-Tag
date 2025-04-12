using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D13 RID: 3347
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@System_Single : IElementReaderWriter<float>
	{
		// Token: 0x060053E3 RID: 21475 RVA: 0x00065A32 File Offset: 0x00063C32
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe float Read(byte* data, int index)
		{
			return *(float*)(data + index * 4);
		}

		// Token: 0x060053E4 RID: 21476 RVA: 0x00065932 File Offset: 0x00063B32
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref float ReadRef(byte* data, int index)
		{
			return ref *(float*)(data + index * 4);
		}

		// Token: 0x060053E5 RID: 21477 RVA: 0x00065A3E File Offset: 0x00063C3E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, float val)
		{
			*(float*)(data + index * 4) = val;
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x00038586 File Offset: 0x00036786
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x001C9240 File Offset: 0x001C7440
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(float val)
		{
			return val.GetHashCode();
		}

		// Token: 0x060053E8 RID: 21480 RVA: 0x001C9254 File Offset: 0x001C7454
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

		// Token: 0x0400563C RID: 22076
		[WeaverGenerated]
		public static IElementReaderWriter<float> Instance;
	}
}
