using System;
using UnityEngine;

// Token: 0x020006BC RID: 1724
public static class Id128Ext
{
	// Token: 0x06002ABD RID: 10941 RVA: 0x000D48D8 File Offset: 0x000D2AD8
	public static Id128 ToId128(this Hash128 h)
	{
		return new Id128(h);
	}

	// Token: 0x06002ABE RID: 10942 RVA: 0x000D48D0 File Offset: 0x000D2AD0
	public static Id128 ToId128(this Guid g)
	{
		return new Id128(g);
	}
}
