using System;
using UnityEngine;

// Token: 0x020006C0 RID: 1728
public class LerpScale : LerpComponent
{
	// Token: 0x06002ACF RID: 10959 RVA: 0x000D45F8 File Offset: 0x000D27F8
	protected override void OnLerp(float t)
	{
		this.current = Vector3.Lerp(this.start, this.end, this.scaleCurve.Evaluate(t));
		if (this.target)
		{
			this.target.localScale = this.current;
		}
	}

	// Token: 0x0400302C RID: 12332
	[Space]
	public Transform target;

	// Token: 0x0400302D RID: 12333
	[Space]
	public Vector3 start = Vector3.one;

	// Token: 0x0400302E RID: 12334
	public Vector3 end = Vector3.one;

	// Token: 0x0400302F RID: 12335
	public Vector3 current;

	// Token: 0x04003030 RID: 12336
	[SerializeField]
	private AnimationCurve scaleCurve = AnimationCurves.EaseInOutBounce;
}
