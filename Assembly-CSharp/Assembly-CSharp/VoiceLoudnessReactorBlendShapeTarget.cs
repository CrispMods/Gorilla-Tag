using System;
using UnityEngine;

// Token: 0x020008E8 RID: 2280
[Serializable]
public class VoiceLoudnessReactorBlendShapeTarget
{
	// Token: 0x040039C3 RID: 14787
	public SkinnedMeshRenderer SkinnedMeshRenderer;

	// Token: 0x040039C4 RID: 14788
	public int BlendShapeIndex;

	// Token: 0x040039C5 RID: 14789
	public float minValue;

	// Token: 0x040039C6 RID: 14790
	public float maxValue = 1f;

	// Token: 0x040039C7 RID: 14791
	public bool UseSmoothedLoudness;
}
