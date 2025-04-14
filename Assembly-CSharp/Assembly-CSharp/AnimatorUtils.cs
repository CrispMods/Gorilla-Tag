using System;
using UnityEngine;

// Token: 0x02000829 RID: 2089
public static class AnimatorUtils
{
	// Token: 0x06003310 RID: 13072 RVA: 0x000F47C4 File Offset: 0x000F29C4
	public static void ResetToEntryState(this Animator a)
	{
		if (a == null)
		{
			return;
		}
		a.Rebind();
		a.Update(0f);
	}
}
