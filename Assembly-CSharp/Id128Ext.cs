using System;
using UnityEngine;

// Token: 0x020006BB RID: 1723
public static class Id128Ext
{
	// Token: 0x06002AB5 RID: 10933 RVA: 0x000D4458 File Offset: 0x000D2658
	public static Id128 ToId128(this Hash128 h)
	{
		return new Id128(h);
	}

	// Token: 0x06002AB6 RID: 10934 RVA: 0x000D4450 File Offset: 0x000D2650
	public static Id128 ToId128(this Guid g)
	{
		return new Id128(g);
	}
}
