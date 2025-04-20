using System;
using UnityEngine;

// Token: 0x020006D5 RID: 1749
public class LerpScale : LerpComponent
{
	// Token: 0x06002B65 RID: 11109 RVA: 0x001205C8 File Offset: 0x0011E7C8
	protected override void OnLerp(float t)
	{
		this.current = Vector3.Lerp(this.start, this.end, this.scaleCurve.Evaluate(t));
		if (this.target)
		{
			this.target.localScale = this.current;
		}
	}

	// Token: 0x040030C9 RID: 12489
	[Space]
	public Transform target;

	// Token: 0x040030CA RID: 12490
	[Space]
	public Vector3 start = Vector3.one;

	// Token: 0x040030CB RID: 12491
	public Vector3 end = Vector3.one;

	// Token: 0x040030CC RID: 12492
	public Vector3 current;

	// Token: 0x040030CD RID: 12493
	[SerializeField]
	private AnimationCurve scaleCurve = AnimationCurves.EaseInOutBounce;
}
