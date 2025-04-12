using System;
using UnityEngine;

// Token: 0x020008EA RID: 2282
[Serializable]
public class VoiceLoudnessReactorTransformRotationTarget
{
	// Token: 0x17000593 RID: 1427
	// (get) Token: 0x060036D2 RID: 14034 RVA: 0x000535F4 File Offset: 0x000517F4
	// (set) Token: 0x060036D3 RID: 14035 RVA: 0x000535FC File Offset: 0x000517FC
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

	// Token: 0x040039CD RID: 14797
	public Transform transform;

	// Token: 0x040039CE RID: 14798
	private Quaternion initial;

	// Token: 0x040039CF RID: 14799
	public Quaternion Max = Quaternion.identity;

	// Token: 0x040039D0 RID: 14800
	public float Scale = 1f;

	// Token: 0x040039D1 RID: 14801
	public bool UseSmoothedLoudness;
}
