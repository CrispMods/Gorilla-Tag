﻿using System;
using UnityEngine;

// Token: 0x02000038 RID: 56
[Serializable]
public class CrittersAnim
{
	// Token: 0x0600011D RID: 285 RVA: 0x000082A8 File Offset: 0x000064A8
	public bool IsModified()
	{
		return (this.squashAmount != null && this.squashAmount.length > 1) || (this.forwardOffset != null && this.forwardOffset.length > 1) || (this.horizontalOffset != null && this.horizontalOffset.length > 1) || (this.verticalOffset != null && this.verticalOffset.length > 1);
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00008311 File Offset: 0x00006511
	public static bool IsModified(CrittersAnim anim)
	{
		return anim != null && anim.IsModified();
	}

	// Token: 0x04000153 RID: 339
	public AnimationCurve squashAmount;

	// Token: 0x04000154 RID: 340
	public AnimationCurve forwardOffset;

	// Token: 0x04000155 RID: 341
	public AnimationCurve horizontalOffset;

	// Token: 0x04000156 RID: 342
	public AnimationCurve verticalOffset;

	// Token: 0x04000157 RID: 343
	public float playSpeed;
}
