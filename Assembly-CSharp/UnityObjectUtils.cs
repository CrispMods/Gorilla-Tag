using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020008AC RID: 2220
public static class UnityObjectUtils
{
	// Token: 0x060035E9 RID: 13801 RVA: 0x00143D0C File Offset: 0x00141F0C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T AsNull<T>(this T obj) where T : UnityEngine.Object
	{
		if (obj == null)
		{
			return default(T);
		}
		if (!(obj == null))
		{
			return obj;
		}
		return default(T);
	}

	// Token: 0x060035EA RID: 13802 RVA: 0x000373D8 File Offset: 0x000355D8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SafeDestroy(this UnityEngine.Object obj)
	{
		UnityEngine.Object.Destroy(obj);
	}
}
