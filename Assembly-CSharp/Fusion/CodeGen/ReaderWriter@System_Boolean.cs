using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D35 RID: 3381
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@System_Boolean : IElementReaderWriter<bool>
	{
		// Token: 0x0600542B RID: 21547 RVA: 0x0019C894 File Offset: 0x0019AA94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe bool Read(byte* data, int index)
		{
			return ReadWriteUtilsForWeaver.ReadBoolean((int*)(data + index * 4));
		}

		// Token: 0x0600542C RID: 21548 RVA: 0x0019C8A4 File Offset: 0x0019AAA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref bool ReadRef(byte* data, int index)
		{
			throw new NotSupportedException("Only supported for trivially copyable types. System.Boolean is not trivially copyable.");
		}

		// Token: 0x0600542D RID: 21549 RVA: 0x0019C8B0 File Offset: 0x0019AAB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, bool val)
		{
			ReadWriteUtilsForWeaver.WriteBoolean((int*)(data + index * 4), val);
		}

		// Token: 0x0600542E RID: 21550 RVA: 0x000444E2 File Offset: 0x000426E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x0600542F RID: 21551 RVA: 0x0019C8C4 File Offset: 0x0019AAC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(bool val)
		{
			return val.GetHashCode();
		}

		// Token: 0x06005430 RID: 21552 RVA: 0x0019C8D8 File Offset: 0x0019AAD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[WeaverGenerated]
		public static IElementReaderWriter<bool> GetInstance()
		{
			if (ReaderWriter@System_Boolean.Instance == null)
			{
				ReaderWriter@System_Boolean.Instance = default(ReaderWriter@System_Boolean);
			}
			return ReaderWriter@System_Boolean.Instance;
		}

		// Token: 0x04005942 RID: 22850
		[WeaverGenerated]
		public static IElementReaderWriter<bool> Instance;
	}
}
