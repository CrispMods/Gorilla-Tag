using System;
using UnityEngine;

// Token: 0x02000840 RID: 2112
public static class AnimatorUtils
{
	// Token: 0x060033BF RID: 13247 RVA: 0x0005208D File Offset: 0x0005028D
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
