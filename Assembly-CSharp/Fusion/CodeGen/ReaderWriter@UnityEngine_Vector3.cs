using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D3F RID: 3391
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@UnityEngine_Vector3 : IElementReaderWriter<Vector3>
	{
		// Token: 0x06005530 RID: 21808 RVA: 0x00067463 File Offset: 0x00065663
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe Vector3 Read(byte* data, int index)
		{
			return *(Vector3*)(data + index * 12);
		}

		// Token: 0x06005531 RID: 21809 RVA: 0x00067473 File Offset: 0x00065673
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref Vector3 ReadRef(byte* data, int index)
		{
			return ref *(Vector3*)(data + index * 12);
		}

		// Token: 0x06005532 RID: 21810 RVA: 0x0006747E File Offset: 0x0006567E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, Vector3 val)
		{
			*(Vector3*)(data + index * 12) = val;
		}

		// Token: 0x06005533 RID: 21811 RVA: 0x00047CA2 File Offset: 0x00045EA2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 3;
		}

		// Token: 0x06005534 RID: 21812 RVA: 0x001D12DC File Offset: 0x001CF4DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(Vector3 val)
		{
			return val.GetHashCode();
		}

		// Token: 0x06005535 RID: 21813 RVA: 0x001D12F8 File Offset: 0x001CF4F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[WeaverGenerated]
		public static IElementReaderWriter<Vector3> GetInstance()
		{
			if (ReaderWriter@UnityEngine_Vector3.Instance == null)
			{
				ReaderWriter@UnityEngine_Vector3.Instance = default(ReaderWriter@UnityEngine_Vector3);
			}
			return ReaderWriter@UnityEngine_Vector3.Instance;
		}

		// Token: 0x04005734 RID: 22324
		[WeaverGenerated]
		public static IElementReaderWriter<Vector3> Instance;
	}
}
