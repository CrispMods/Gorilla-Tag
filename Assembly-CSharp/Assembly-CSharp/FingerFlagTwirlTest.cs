using System;
using UnityEngine;

// Token: 0x020000BC RID: 188
public class FingerFlagTwirlTest : MonoBehaviour
{
	// Token: 0x060004E0 RID: 1248 RVA: 0x0001D424 File Offset: 0x0001B624
	protected void FixedUpdate()
	{
		this.animTimes += Time.deltaTime * this.rotAnimDurations;
		this.animTimes.x = this.animTimes.x % 1f;
		this.animTimes.y = this.animTimes.y % 1f;
		this.animTimes.z = this.animTimes.z % 1f;
		base.transform.localRotation = Quaternion.Euler(this.rotXAnimCurve.Evaluate(this.animTimes.x) * this.rotAnimAmplitudes.x, this.rotYAnimCurve.Evaluate(this.animTimes.y) * this.rotAnimAmplitudes.y, this.rotZAnimCurve.Evaluate(this.animTimes.z) * this.rotAnimAmplitudes.z);
	}

	// Token: 0x040005A3 RID: 1443
	public Vector3 rotAnimDurations = new Vector3(0.2f, 0.1f, 0.5f);

	// Token: 0x040005A4 RID: 1444
	public Vector3 rotAnimAmplitudes = Vector3.one * 360f;

	// Token: 0x040005A5 RID: 1445
	public AnimationCurve rotXAnimCurve;

	// Token: 0x040005A6 RID: 1446
	public AnimationCurve rotYAnimCurve;

	// Token: 0x040005A7 RID: 1447
	public AnimationCurve rotZAnimCurve;

	// Token: 0x040005A8 RID: 1448
	private Vector3 animTimes = Vector3.zero;
}
