using System;
using UnityEngine;

// Token: 0x020006C1 RID: 1729
public class LerpScale : LerpComponent
{
	// Token: 0x06002AD7 RID: 10967 RVA: 0x000D4A78 File Offset: 0x000D2C78
	protected override void OnLerp(float t)
	{
		this.current = Vector3.Lerp(this.start, this.end, this.scaleCurve.Evaluate(t));
		if (this.target)
		{
			this.target.localScale = this.current;
		}
	}

	// Token: 0x04003032 RID: 12338
	[Space]
	public Transform target;

	// Token: 0x04003033 RID: 12339
	[Space]
	public Vector3 start = Vector3.one;

	// Token: 0x04003034 RID: 12340
	public Vector3 end = Vector3.one;

	// Token: 0x04003035 RID: 12341
	public Vector3 current;

	// Token: 0x04003036 RID: 12342
	[SerializeField]
	private AnimationCurve scaleCurve = AnimationCurves.EaseInOutBounce;
}
