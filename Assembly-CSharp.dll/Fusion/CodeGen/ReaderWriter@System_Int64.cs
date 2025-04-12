using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D21 RID: 3361
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@System_Int64 : IElementReaderWriter<long>
	{
		// Token: 0x06005404 RID: 21508 RVA: 0x00065B23 File Offset: 0x00063D23
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe long Read(byte* data, int index)
		{
			return *(long*)(data + index * 8);
		}

		// Token: 0x06005405 RID: 21509 RVA: 0x00065B2F File Offset: 0x00063D2F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref long ReadRef(byte* data, int index)
		{
			return ref *(long*)(data + index * 8);
		}

		// Token: 0x06005406 RID: 21510 RVA: 0x00065B3A File Offset: 0x00063D3A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, long val)
		{
			*(long*)(data + index * 8) = val;
		}

		// Token: 0x06005407 RID: 21511 RVA: 0x00031018 File Offset: 0x0002F218
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 2;
		}

		// Token: 0x06005408 RID: 21512 RVA: 0x001C9308 File Offset: 0x001C7508
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(long val)
		{
			return val.GetHashCode();
		}

		// Token: 0x06005409 RID: 21513 RVA: 0x001C931C File Offset: 0x001C751C
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

		// Token: 0x04005724 RID: 22308
		[WeaverGenerated]
		public static IElementReaderWriter<long> Instance;
	}
}
