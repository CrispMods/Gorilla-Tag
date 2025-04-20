using System;
using UnityEngine;

// Token: 0x02000901 RID: 2305
[Serializable]
public class VoiceLoudnessReactorBlendShapeTarget
{
	// Token: 0x04003A72 RID: 14962
	public SkinnedMeshRenderer SkinnedMeshRenderer;

	// Token: 0x04003A73 RID: 14963
	public int BlendShapeIndex;

	// Token: 0x04003A74 RID: 14964
	public float minValue;

	// Token: 0x04003A75 RID: 14965
	public float maxValue = 1f;

	// Token: 0x04003A76 RID: 14966
	public bool UseSmoothedLoudness;
}
