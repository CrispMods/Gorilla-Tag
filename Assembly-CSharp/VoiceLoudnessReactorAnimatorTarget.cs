using System;
using UnityEngine;

// Token: 0x02000906 RID: 2310
[Serializable]
public class VoiceLoudnessReactorAnimatorTarget
{
	// Token: 0x04003A91 RID: 14993
	public Animator animator;

	// Token: 0x04003A92 RID: 14994
	public bool useSmoothedLoudness;

	// Token: 0x04003A93 RID: 14995
	public float animatorSpeedToLoudness = 1f;
}
