using System;
using UnityEngine;

// Token: 0x02000826 RID: 2086
public static class AnimatorUtils
{
	// Token: 0x06003304 RID: 13060 RVA: 0x000F41FC File Offset: 0x000F23FC
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
