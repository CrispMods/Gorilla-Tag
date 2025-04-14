using System;
using UnityEngine;

// Token: 0x02000860 RID: 2144
public static class PoolUtils
{
	// Token: 0x06003409 RID: 13321 RVA: 0x000F8780 File Offset: 0x000F6980
	public static int GameObjHashCode(GameObject obj)
	{
		return obj.tag.GetHashCode();
	}
}
