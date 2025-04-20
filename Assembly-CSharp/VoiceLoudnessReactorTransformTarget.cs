using System;
using UnityEngine;

// Token: 0x02000902 RID: 2306
[Serializable]
public class VoiceLoudnessReactorTransformTarget
{
	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x0600378B RID: 14219 RVA: 0x00054AE2 File Offset: 0x00052CE2
	// (set) Token: 0x0600378C RID: 14220 RVA: 0x00054AEA File Offset: 0x00052CEA
	public Vector3 Initial
	{
		get
		{
			return this.initial;
		}
		set
		{
			this.initial = value;
		}
	}

	// Token: 0x04003A77 RID: 14967
	public Transform transform;

	// Token: 0x04003A78 RID: 14968
	private Vector3 initial;

	// Token: 0x04003A79 RID: 14969
	public Vector3 Max = Vector3.one;

	// Token: 0x04003A7A RID: 14970
	public float Scale = 1f;

	// Token: 0x04003A7B RID: 14971
	public bool UseSmoothedLoudness;
}
