using System;
using UnityEngine;

// Token: 0x020008E9 RID: 2281
[Serializable]
public class VoiceLoudnessReactorTransformTarget
{
	// Token: 0x17000592 RID: 1426
	// (get) Token: 0x060036CF RID: 14031 RVA: 0x000535C5 File Offset: 0x000517C5
	// (set) Token: 0x060036D0 RID: 14032 RVA: 0x000535CD File Offset: 0x000517CD
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

	// Token: 0x040039C8 RID: 14792
	public Transform transform;

	// Token: 0x040039C9 RID: 14793
	private Vector3 initial;

	// Token: 0x040039CA RID: 14794
	public Vector3 Max = Vector3.one;

	// Token: 0x040039CB RID: 14795
	public float Scale = 1f;

	// Token: 0x040039CC RID: 14796
	public bool UseSmoothedLoudness;
}
