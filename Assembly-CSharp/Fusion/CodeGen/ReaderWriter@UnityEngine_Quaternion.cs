using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D17 RID: 3351
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@UnityEngine_Quaternion : IElementReaderWriter<Quaternion>
	{
		// Token: 0x060053E3 RID: 21475 RVA: 0x0019C489 File Offset: 0x0019A689
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe Quaternion Read(byte* data, int index)
		{
			return *(Quaternion*)(data + index * 16);
		}

		// Token: 0x060053E4 RID: 21476 RVA: 0x0019C499 File Offset: 0x0019A699
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref Quaternion ReadRef(byte* data, int index)
		{
			return ref *(Quaternion*)(data + index * 16);
		}

		// Token: 0x060053E5 RID: 21477 RVA: 0x0019C4A4 File Offset: 0x0019A6A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, Quaternion val)
		{
			*(Quaternion*)(data + index * 16) = val;
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x000EE9C3 File Offset: 0x000ECBC3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 4;
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x0019C4B8 File Offset: 0x0019A6B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(Quaternion val)
		{
			return val.GetHashCode();
		}

		// Token: 0x060053E8 RID: 21480 RVA: 0x0019C4D4 File Offset: 0x0019A6D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[WeaverGenerated]
		public static IElementReaderWriter<Quaternion> GetInstance()
		{
			if (ReaderWriter@UnityEngine_Quaternion.Instance == null)
			{
				ReaderWriter@UnityEngine_Quaternion.Instance = default(ReaderWriter@UnityEngine_Quaternion);
			}
			return ReaderWriter@UnityEngine_Quaternion.Instance;
		}

		// Token: 0x0400563D RID: 22077
		[WeaverGenerated]
		public static IElementReaderWriter<Quaternion> Instance;
	}
}
