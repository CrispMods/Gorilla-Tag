using System;
using UnityEngine;

// Token: 0x0200020F RID: 527
public static class UnityLayerExtensions
{
	// Token: 0x06000C60 RID: 3168 RVA: 0x00038A1C File Offset: 0x00036C1C
	public static int ToLayerMask(this UnityLayer self)
	{
		return 1 << (int)self;
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x00038A24 File Offset: 0x00036C24
	public static int ToLayerIndex(this UnityLayer self)
	{
		return (int)self;
	}

	// Token: 0x06000C62 RID: 3170 RVA: 0x00038A27 File Offset: 0x00036C27
	public static bool IsOnLayer(this GameObject obj, UnityLayer layer)
	{
		return obj.layer == (int)layer;
	}

	// Token: 0x06000C63 RID: 3171 RVA: 0x00038A32 File Offset: 0x00036C32
	public static void SetLayer(this GameObject obj, UnityLayer layer)
	{
		obj.layer = (int)layer;
	}
}
