using System;
using UnityEngine;

// Token: 0x020008EC RID: 2284
[Serializable]
public class VoiceLoudnessReactorGameObjectEnableTarget
{
	// Token: 0x040039DD RID: 14813
	public GameObject GameObject;

	// Token: 0x040039DE RID: 14814
	public float Threshold;

	// Token: 0x040039DF RID: 14815
	public bool TurnOnAtThreshhold = true;

	// Token: 0x040039E0 RID: 14816
	public bool UseSmoothedLoudness;

	// Token: 0x040039E1 RID: 14817
	public float Scale = 1f;
}
