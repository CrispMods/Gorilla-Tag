using System;
using UnityEngine;

// Token: 0x02000860 RID: 2144
public static class PoolUtils
{
	// Token: 0x06003409 RID: 13321 RVA: 0x00051771 File Offset: 0x0004F971
	public static int GameObjHashCode(GameObject obj)
	{
		return obj.tag.GetHashCode();
	}
}
