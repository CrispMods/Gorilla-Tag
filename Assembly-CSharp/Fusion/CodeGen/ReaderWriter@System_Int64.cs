using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D4F RID: 3407
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@System_Int64 : IElementReaderWriter<long>
	{
		// Token: 0x0600555A RID: 21850 RVA: 0x00067599 File Offset: 0x00065799
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe long Read(byte* data, int index)
		{
			return *(long*)(data + index * 8);
		}

		// Token: 0x0600555B RID: 21851 RVA: 0x000675A5 File Offset: 0x000657A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref long ReadRef(byte* data, int index)
		{
			return ref *(long*)(data + index * 8);
		}

		// Token: 0x0600555C RID: 21852 RVA: 0x000675B0 File Offset: 0x000657B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, long val)
		{
			*(long*)(data + index * 8) = val;
		}

		// Token: 0x0600555D RID: 21853 RVA: 0x00032182 File Offset: 0x00030382
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 2;
		}

		// Token: 0x0600555E RID: 21854 RVA: 0x001D13EC File Offset: 0x001CF5EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(long val)
		{
			return val.GetHashCode();
		}

		// Token: 0x0600555F RID: 21855 RVA: 0x001D1400 File Offset: 0x001CF600
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

		// Token: 0x0400581E RID: 22558
		[WeaverGenerated]
		public static IElementReaderWriter<long> Instance;
	}
}
