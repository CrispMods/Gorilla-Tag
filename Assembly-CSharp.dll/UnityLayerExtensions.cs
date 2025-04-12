using System;
using UnityEngine;

// Token: 0x02000204 RID: 516
public static class UnityLayerExtensions
{
	// Token: 0x06000C17 RID: 3095 RVA: 0x0003775C File Offset: 0x0003595C
	public static int ToLayerMask(this UnityLayer self)
	{
		return 1 << (int)self;
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x00037764 File Offset: 0x00035964
	public static int ToLayerIndex(this UnityLayer self)
	{
		return (int)self;
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x00037767 File Offset: 0x00035967
	public static bool IsOnLayer(this GameObject obj, UnityLayer layer)
	{
		return obj.layer == (int)layer;
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x00037772 File Offset: 0x00035972
	public static void SetLayer(this GameObject obj, UnityLayer layer)
	{
		obj.layer = (int)layer;
	}
}
