using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D1E RID: 3358
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@System_Int64 : IElementReaderWriter<long>
	{
		// Token: 0x060053F8 RID: 21496 RVA: 0x0019C5B1 File Offset: 0x0019A7B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe long Read(byte* data, int index)
		{
			return *(long*)(data + index * 8);
		}

		// Token: 0x060053F9 RID: 21497 RVA: 0x0019C5BD File Offset: 0x0019A7BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref long ReadRef(byte* data, int index)
		{
			return ref *(long*)(data + index * 8);
		}

		// Token: 0x060053FA RID: 21498 RVA: 0x0019C5C8 File Offset: 0x0019A7C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, long val)
		{
			*(long*)(data + index * 8) = val;
		}

		// Token: 0x060053FB RID: 21499 RVA: 0x00010B2F File Offset: 0x0000ED2F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 2;
		}

		// Token: 0x060053FC RID: 21500 RVA: 0x0019C5D8 File Offset: 0x0019A7D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(long val)
		{
			return val.GetHashCode();
		}

		// Token: 0x060053FD RID: 21501 RVA: 0x0019C5EC File Offset: 0x0019A7EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[WeaverGenerated]
		public static IElementReaderWriter<long> GetInstance()
		{
			if (ReaderWriter@System_Int64.Instance == null)
			{
				ReaderWriter@System_Int64.Instance = default(ReaderWriter@System_Int64);
			}
			return ReaderWriter@System_Int64.Instance;
		}

		// Token: 0x04005712 RID: 22290
		[WeaverGenerated]
		public static IElementReaderWriter<long> Instance;
	}
}
