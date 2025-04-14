using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D0E RID: 3342
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@UnityEngine_Vector3 : IElementReaderWriter<Vector3>
	{
		// Token: 0x060053CE RID: 21454 RVA: 0x0019C361 File Offset: 0x0019A561
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe Vector3 Read(byte* data, int index)
		{
			return *(Vector3*)(data + index * 12);
		}

		// Token: 0x060053CF RID: 21455 RVA: 0x0019C371 File Offset: 0x0019A571
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref Vector3 ReadRef(byte* data, int index)
		{
			return ref *(Vector3*)(data + index * 12);
		}

		// Token: 0x060053D0 RID: 21456 RVA: 0x0019C37C File Offset: 0x0019A57C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, Vector3 val)
		{
			*(Vector3*)(data + index * 12) = val;
		}

		// Token: 0x060053D1 RID: 21457 RVA: 0x000AC7BA File Offset: 0x000AA9BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 3;
		}

		// Token: 0x060053D2 RID: 21458 RVA: 0x0019C390 File Offset: 0x0019A590
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(Vector3 val)
		{
			return val.GetHashCode();
		}

		// Token: 0x060053D3 RID: 21459 RVA: 0x0019C3AC File Offset: 0x0019A5AC
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

		// Token: 0x04005628 RID: 22056
		[WeaverGenerated]
		public static IElementReaderWriter<Vector3> Instance;
	}
}
