using System;
using UnityEngine;

// Token: 0x020008EA RID: 2282
[Serializable]
public class VoiceLoudnessReactorAnimatorTarget
{
	// Token: 0x040039D0 RID: 14800
	public Animator animator;

	// Token: 0x040039D1 RID: 14801
	public bool useSmoothedLoudness;

	// Token: 0x040039D2 RID: 14802
	public float animatorSpeedToLoudness = 1f;
}
