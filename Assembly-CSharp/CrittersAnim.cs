using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
[Serializable]
public class CrittersAnim
{
	// Token: 0x06000132 RID: 306 RVA: 0x0006D87C File Offset: 0x0006BA7C
	public bool IsModified()
	{
		return (this.squashAmount != null && this.squashAmount.length > 1) || (this.forwardOffset != null && this.forwardOffset.length > 1) || (this.horizontalOffset != null && this.horizontalOffset.length > 1) || (this.verticalOffset != null && this.verticalOffset.length > 1);
	}

	// Token: 0x06000133 RID: 307 RVA: 0x0003112D File Offset: 0x0002F32D
	public static bool IsModified(CrittersAnim anim)
	{
		return anim != null && anim.IsModified();
	}

	// Token: 0x04000164 RID: 356
	public AnimationCurve squashAmount;

	// Token: 0x04000165 RID: 357
	public AnimationCurve forwardOffset;

	// Token: 0x04000166 RID: 358
	public AnimationCurve horizontalOffset;

	// Token: 0x04000167 RID: 359
	public AnimationCurve verticalOffset;

	// Token: 0x04000168 RID: 360
	public float playSpeed;
}
