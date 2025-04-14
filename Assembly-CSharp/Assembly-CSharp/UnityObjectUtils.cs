using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000893 RID: 2195
public static class UnityObjectUtils
{
	// Token: 0x06003529 RID: 13609 RVA: 0x000FDB14 File Offset: 0x000FBD14
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

	// Token: 0x0600352A RID: 13610 RVA: 0x0003783A File Offset: 0x00035A3A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SafeDestroy(this Object obj)
	{
		Object.Destroy(obj);
	}
}
