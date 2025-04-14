using System;
using UnityEngine;

// Token: 0x020008ED RID: 2285
[Serializable]
public class VoiceLoudnessReactorAnimatorTarget
{
	// Token: 0x040039E2 RID: 14818
	public Animator animator;

	// Token: 0x040039E3 RID: 14819
	public bool useSmoothedLoudness;

	// Token: 0x040039E4 RID: 14820
	public float animatorSpeedToLoudness = 1f;
}
