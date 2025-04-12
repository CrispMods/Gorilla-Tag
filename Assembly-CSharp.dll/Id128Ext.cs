using System;
using UnityEngine;

// Token: 0x020006BC RID: 1724
public static class Id128Ext
{
	// Token: 0x06002ABD RID: 10941 RVA: 0x0004BFF3 File Offset: 0x0004A1F3
	public static Id128 ToId128(this Hash128 h)
	{
		return new Id128(h);
	}

	// Token: 0x06002ABE RID: 10942 RVA: 0x0004BFEB File Offset: 0x0004A1EB
	public static Id128 ToId128(this Guid g)
	{
		return new Id128(g);
	}
}
