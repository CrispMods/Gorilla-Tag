using System;
using UnityEngine;

// Token: 0x020006D0 RID: 1744
public static class Id128Ext
{
	// Token: 0x06002B4B RID: 11083 RVA: 0x0004D338 File Offset: 0x0004B538
	public static Id128 ToId128(this Hash128 h)
	{
		return new Id128(h);
	}

	// Token: 0x06002B4C RID: 11084 RVA: 0x0004D330 File Offset: 0x0004B530
	public static Id128 ToId128(this Guid g)
	{
		return new Id128(g);
	}
}
