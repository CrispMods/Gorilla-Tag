using System;
using UnityEngine;

// Token: 0x0200085D RID: 2141
public static class PoolUtils
{
	// Token: 0x060033FD RID: 13309 RVA: 0x000F81B8 File Offset: 0x000F63B8
	public static int GameObjHashCode(GameObject obj)
	{
		return obj.tag.GetHashCode();
	}
}
