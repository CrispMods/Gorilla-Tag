using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000890 RID: 2192
public static class UnityObjectUtils
{
	// Token: 0x0600351D RID: 13597 RVA: 0x000FD54C File Offset: 0x000FB74C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T AsNull<T>(this T obj) where T : Object
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

	// Token: 0x0600351E RID: 13598 RVA: 0x00037516 File Offset: 0x00035716
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SafeDestroy(this Object obj)
	{
		Object.Destroy(obj);
	}
}
