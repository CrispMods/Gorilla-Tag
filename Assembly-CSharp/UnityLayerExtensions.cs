using System;
using UnityEngine;

// Token: 0x02000204 RID: 516
public static class UnityLayerExtensions
{
	// Token: 0x06000C15 RID: 3093 RVA: 0x0003FF6A File Offset: 0x0003E16A
	public static int ToLayerMask(this UnityLayer self)
	{
		return 1 << (int)self;
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x0003FF72 File Offset: 0x0003E172
	public static int ToLayerIndex(this UnityLayer self)
	{
		return (int)self;
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x0003FF75 File Offset: 0x0003E175
	public static bool IsOnLayer(this GameObject obj, UnityLayer layer)
	{
		return obj.layer == (int)layer;
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x0003FF80 File Offset: 0x0003E180
	public static void SetLayer(this GameObject obj, UnityLayer layer)
	{
		obj.layer = (int)layer;
	}
}
