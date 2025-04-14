using System;
using UnityEngine;

// Token: 0x020008E7 RID: 2279
[Serializable]
public class VoiceLoudnessReactorTransformRotationTarget
{
	// Token: 0x17000592 RID: 1426
	// (get) Token: 0x060036C6 RID: 14022 RVA: 0x00103C28 File Offset: 0x00101E28
	// (set) Token: 0x060036C7 RID: 14023 RVA: 0x00103C30 File Offset: 0x00101E30
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

	// Token: 0x040039BB RID: 14779
	public Transform transform;

	// Token: 0x040039BC RID: 14780
	private Quaternion initial;

	// Token: 0x040039BD RID: 14781
	public Quaternion Max = Quaternion.identity;

	// Token: 0x040039BE RID: 14782
	public float Scale = 1f;

	// Token: 0x040039BF RID: 14783
	public bool UseSmoothedLoudness;
}
