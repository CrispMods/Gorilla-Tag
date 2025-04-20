using System;
using UnityEngine;

// Token: 0x020000C6 RID: 198
public class FingerFlagTwirlTest : MonoBehaviour
{
	// Token: 0x0600051A RID: 1306 RVA: 0x000804C4 File Offset: 0x0007E6C4
	protected void FixedUpdate()
	{
		this.animTimes += Time.deltaTime * this.rotAnimDurations;
		this.animTimes.x = this.animTimes.x % 1f;
		this.animTimes.y = this.animTimes.y % 1f;
		this.animTimes.z = this.animTimes.z % 1f;
		base.transform.localRotation = Quaternion.Euler(this.rotXAnimCurve.Evaluate(this.animTimes.x) * this.rotAnimAmplitudes.x, this.rotYAnimCurve.Evaluate(this.animTimes.y) * this.rotAnimAmplitudes.y, this.rotZAnimCurve.Evaluate(this.animTimes.z) * this.rotAnimAmplitudes.z);
	}

	// Token: 0x040005E2 RID: 1506
	public Vector3 rotAnimDurations = new Vector3(0.2f, 0.1f, 0.5f);

	// Token: 0x040005E3 RID: 1507
	public Vector3 rotAnimAmplitudes = Vector3.one * 360f;

	// Token: 0x040005E4 RID: 1508
	public AnimationCurve rotXAnimCurve;

	// Token: 0x040005E5 RID: 1509
	public AnimationCurve rotYAnimCurve;

	// Token: 0x040005E6 RID: 1510
	public AnimationCurve rotZAnimCurve;

	// Token: 0x040005E7 RID: 1511
	private Vector3 animTimes = Vector3.zero;
}
