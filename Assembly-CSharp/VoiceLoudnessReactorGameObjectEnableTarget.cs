using System;
using UnityEngine;

// Token: 0x02000905 RID: 2309
[Serializable]
public class VoiceLoudnessReactorGameObjectEnableTarget
{
	// Token: 0x04003A8C RID: 14988
	public GameObject GameObject;

	// Token: 0x04003A8D RID: 14989
	public float Threshold;

	// Token: 0x04003A8E RID: 14990
	public bool TurnOnAtThreshhold = true;

	// Token: 0x04003A8F RID: 14991
	public bool UseSmoothedLoudness;

	// Token: 0x04003A90 RID: 14992
	public float Scale = 1f;
}
