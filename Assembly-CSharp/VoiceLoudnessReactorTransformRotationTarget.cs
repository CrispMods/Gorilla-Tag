using System;
using UnityEngine;

// Token: 0x02000903 RID: 2307
[Serializable]
public class VoiceLoudnessReactorTransformRotationTarget
{
	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x0600378E RID: 14222 RVA: 0x00054B11 File Offset: 0x00052D11
	// (set) Token: 0x0600378F RID: 14223 RVA: 0x00054B19 File Offset: 0x00052D19
	public Quaternion Initial
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

	// Token: 0x04003A7C RID: 14972
	public Transform transform;

	// Token: 0x04003A7D RID: 14973
	private Quaternion initial;

	// Token: 0x04003A7E RID: 14974
	public Quaternion Max = Quaternion.identity;

	// Token: 0x04003A7F RID: 14975
	public float Scale = 1f;

	// Token: 0x04003A80 RID: 14976
	public bool UseSmoothedLoudness;
}
