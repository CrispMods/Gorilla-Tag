using System;
using UnityEngine;

// Token: 0x02000879 RID: 2169
public static class PoolUtils
{
	// Token: 0x060034C9 RID: 13513 RVA: 0x00052C7E File Offset: 0x00050E7E
	public static int GameObjHashCode(GameObject obj)
	{
		return obj.tag.GetHashCode();
	}
}
