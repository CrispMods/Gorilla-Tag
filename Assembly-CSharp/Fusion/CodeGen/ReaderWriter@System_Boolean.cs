using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D66 RID: 3430
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@System_Boolean : IElementReaderWriter<bool>
	{
		// Token: 0x0600558D RID: 21901 RVA: 0x00067763 File Offset: 0x00065963
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe bool Read(byte* data, int index)
		{
			return ReadWriteUtilsForWeaver.ReadBoolean((int*)(data + index * 4));
		}

		// Token: 0x0600558E RID: 21902 RVA: 0x00067773 File Offset: 0x00065973
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref bool ReadRef(byte* data, int index)
		{
			throw new NotSupportedException("Only supported for trivially copyable types. System.Boolean is not trivially copyable.");
		}

		// Token: 0x0600558F RID: 21903 RVA: 0x0006777F File Offset: 0x0006597F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, bool val)
		{
			ReadWriteUtilsForWeaver.WriteBoolean((int*)(data + index * 4), val);
		}

		// Token: 0x06005590 RID: 21904 RVA: 0x00039846 File Offset: 0x00037A46
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x06005591 RID: 21905 RVA: 0x001D14FC File Offset: 0x001CF6FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(bool val)
		{
			return val.GetHashCode();
		}

		// Token: 0x06005592 RID: 21906 RVA: 0x001D1510 File Offset: 0x001CF710
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

		// Token: 0x04005A4E RID: 23118
		[WeaverGenerated]
		public static IElementReaderWriter<bool> Instance;
	}
}
