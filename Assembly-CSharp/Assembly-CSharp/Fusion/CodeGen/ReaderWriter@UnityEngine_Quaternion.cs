using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D1A RID: 3354
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@UnityEngine_Quaternion : IElementReaderWriter<Quaternion>
	{
		// Token: 0x060053EF RID: 21487 RVA: 0x0019CA51 File Offset: 0x0019AC51
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe Quaternion Read(byte* data, int index)
		{
			return *(Quaternion*)(data + index * 16);
		}

		// Token: 0x060053F0 RID: 21488 RVA: 0x0019CA61 File Offset: 0x0019AC61
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref Quaternion ReadRef(byte* data, int index)
		{
			return ref *(Quaternion*)(data + index * 16);
		}

		// Token: 0x060053F1 RID: 21489 RVA: 0x0019CA6C File Offset: 0x0019AC6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, Quaternion val)
		{
			*(Quaternion*)(data + index * 16) = val;
		}

		// Token: 0x060053F2 RID: 21490 RVA: 0x000EEE43 File Offset: 0x000ED043
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 4;
		}

		// Token: 0x060053F3 RID: 21491 RVA: 0x0019CA80 File Offset: 0x0019AC80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(Quaternion val)
		{
			return val.GetHashCode();
		}

		// Token: 0x060053F4 RID: 21492 RVA: 0x0019CA9C File Offset: 0x0019AC9C
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

		// Token: 0x0400564F RID: 22095
		[WeaverGenerated]
		public static IElementReaderWriter<Quaternion> Instance;
	}
}
