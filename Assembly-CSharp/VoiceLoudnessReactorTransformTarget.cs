using System;
using UnityEngine;

// Token: 0x020008E6 RID: 2278
[Serializable]
public class VoiceLoudnessReactorTransformTarget
{
	// Token: 0x17000591 RID: 1425
	// (get) Token: 0x060036C3 RID: 14019 RVA: 0x00103BF9 File Offset: 0x00101DF9
	// (set) Token: 0x060036C4 RID: 14020 RVA: 0x00103C01 File Offset: 0x00101E01
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

	// Token: 0x040039B6 RID: 14774
	public Transform transform;

	// Token: 0x040039B7 RID: 14775
	private Vector3 initial;

	// Token: 0x040039B8 RID: 14776
	public Vector3 Max = Vector3.one;

	// Token: 0x040039B9 RID: 14777
	public float Scale = 1f;

	// Token: 0x040039BA RID: 14778
	public bool UseSmoothedLoudness;
}
