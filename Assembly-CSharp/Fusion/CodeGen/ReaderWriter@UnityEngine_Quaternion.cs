using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D48 RID: 3400
	[WeaverGenerated]
	[PreserveInPlugin]
	internal struct ReaderWriter@UnityEngine_Quaternion : IElementReaderWriter<Quaternion>
	{
		// Token: 0x06005545 RID: 21829 RVA: 0x000674FE File Offset: 0x000656FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe Quaternion Read(byte* data, int index)
		{
			return *(Quaternion*)(data + index * 16);
		}

		// Token: 0x06005546 RID: 21830 RVA: 0x0006750E File Offset: 0x0006570E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe ref Quaternion ReadRef(byte* data, int index)
		{
			return ref *(Quaternion*)(data + index * 16);
		}

		// Token: 0x06005547 RID: 21831 RVA: 0x00067519 File Offset: 0x00065719
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public unsafe void Write(byte* data, int index, Quaternion val)
		{
			*(Quaternion*)(data + index * 16) = val;
		}

		// Token: 0x06005548 RID: 21832 RVA: 0x0005144C File Offset: 0x0004F64C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementWordCount()
		{
			return 4;
		}

		// Token: 0x06005549 RID: 21833 RVA: 0x001D1364 File Offset: 0x001CF564
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[PreserveInPlugin]
		[WeaverGenerated]
		public int GetElementHashCode(Quaternion val)
		{
			return val.GetHashCode();
		}

		// Token: 0x0600554A RID: 21834 RVA: 0x001D1380 File Offset: 0x001CF580
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

		// Token: 0x04005749 RID: 22345
		[WeaverGenerated]
		public static IElementReaderWriter<Quaternion> Instance;
	}
}
