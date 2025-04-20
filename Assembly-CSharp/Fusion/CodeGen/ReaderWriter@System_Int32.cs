using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D4A RID: 3402
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@System_Int32 : IElementReaderWriter<int>
	{
		// Token: 0x0600554E RID: 21838 RVA: 0x00067543 File Offset: 0x00065743
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe int Read(byte* data, int index)
		{
			return *(int*)(data + index * 4);
		}

		// Token: 0x0600554F RID: 21839 RVA: 0x000673A8 File Offset: 0x000655A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref int ReadRef(byte* data, int index)
		{
			return ref *(int*)(data + index * 4);
		}

		// Token: 0x06005550 RID: 21840 RVA: 0x0006754F File Offset: 0x0006574F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, int val)
		{
			*(int*)(data + index * 4) = val;
		}

		// Token: 0x06005551 RID: 21841 RVA: 0x00039846 File Offset: 0x00037A46
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x06005552 RID: 21842 RVA: 0x001D13AC File Offset: 0x001CF5AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(int val)
		{
			return val.GetHashCode();
		}

		// Token: 0x06005553 RID: 21843 RVA: 0x001D13C0 File Offset: 0x001CF5C0
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

		// Token: 0x0400574B RID: 22347
		[WeaverGenerated]
		public static IElementReaderWriter<int> Instance;
	}
}
