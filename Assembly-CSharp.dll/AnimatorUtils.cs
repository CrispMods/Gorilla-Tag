using System;
using UnityEngine;

// Token: 0x02000829 RID: 2089
public static class AnimatorUtils
{
	// Token: 0x06003310 RID: 13072 RVA: 0x00050C7F File Offset: 0x0004EE7F
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
