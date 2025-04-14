using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D11 RID: 3345
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@UnityEngine_Vector3 : IElementReaderWriter<Vector3>
	{
		// Token: 0x060053DA RID: 21466 RVA: 0x0019C929 File Offset: 0x0019AB29
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe Vector3 Read(byte* data, int index)
		{
			return *(Vector3*)(data + index * 12);
		}

		// Token: 0x060053DB RID: 21467 RVA: 0x0019C939 File Offset: 0x0019AB39
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref Vector3 ReadRef(byte* data, int index)
		{
			return ref *(Vector3*)(data + index * 12);
		}

		// Token: 0x060053DC RID: 21468 RVA: 0x0019C944 File Offset: 0x0019AB44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, Vector3 val)
		{
			*(Vector3*)(data + index * 12) = val;
		}

		// Token: 0x060053DD RID: 21469 RVA: 0x000ACC3A File Offset: 0x000AAE3A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 3;
		}

		// Token: 0x060053DE RID: 21470 RVA: 0x0019C958 File Offset: 0x0019AB58
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(Vector3 val)
		{
			return val.GetHashCode();
		}

		// Token: 0x060053DF RID: 21471 RVA: 0x0019C974 File Offset: 0x0019AB74
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

		// Token: 0x0400563A RID: 22074
		[WeaverGenerated]
		public static IElementReaderWriter<Vector3> Instance;
	}
}
