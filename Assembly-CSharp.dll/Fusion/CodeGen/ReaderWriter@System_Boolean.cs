using System;
using System.Runtime.CompilerServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D38 RID: 3384
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@System_Boolean : IElementReaderWriter<bool>
	{
		// Token: 0x06005437 RID: 21559 RVA: 0x00065CED File Offset: 0x00063EED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe bool Read(byte* data, int index)
		{
			return ReadWriteUtilsForWeaver.ReadBoolean((int*)(data + index * 4));
		}

		// Token: 0x06005438 RID: 21560 RVA: 0x00065CFD File Offset: 0x00063EFD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref bool ReadRef(byte* data, int index)
		{
			throw new NotSupportedException("Only supported for trivially copyable types. System.Boolean is not trivially copyable.");
		}

		// Token: 0x06005439 RID: 21561 RVA: 0x00065D09 File Offset: 0x00063F09
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, bool val)
		{
			ReadWriteUtilsForWeaver.WriteBoolean((int*)(data + index * 4), val);
		}

		// Token: 0x0600543A RID: 21562 RVA: 0x00038586 File Offset: 0x00036786
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 1;
		}

		// Token: 0x0600543B RID: 21563 RVA: 0x001C9418 File Offset: 0x001C7618
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(bool val)
		{
			return val.GetHashCode();
		}

		// Token: 0x0600543C RID: 21564 RVA: 0x001C942C File Offset: 0x001C762C
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

		// Token: 0x04005954 RID: 22868
		[WeaverGenerated]
		public static IElementReaderWriter<bool> Instance;
	}
}
