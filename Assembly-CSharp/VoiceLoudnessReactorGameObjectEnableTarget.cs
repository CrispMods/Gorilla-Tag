using System;
using UnityEngine;

// Token: 0x020008E9 RID: 2281
[Serializable]
public class VoiceLoudnessReactorGameObjectEnableTarget
{
	// Token: 0x040039CB RID: 14795
	public GameObject GameObject;

	// Token: 0x040039CC RID: 14796
	public float Threshold;

	// Token: 0x040039CD RID: 14797
	public bool TurnOnAtThreshhold = true;

	// Token: 0x040039CE RID: 14798
	public bool UseSmoothedLoudness;

	// Token: 0x040039CF RID: 14799
	public float Scale = 1f;
}
