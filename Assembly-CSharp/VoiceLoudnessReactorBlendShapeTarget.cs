using System;
using UnityEngine;

// Token: 0x020008E5 RID: 2277
[Serializable]
public class VoiceLoudnessReactorBlendShapeTarget
{
	// Token: 0x040039B1 RID: 14769
	public SkinnedMeshRenderer SkinnedMeshRenderer;

	// Token: 0x040039B2 RID: 14770
	public int BlendShapeIndex;

	// Token: 0x040039B3 RID: 14771
	public float minValue;

	// Token: 0x040039B4 RID: 14772
	public float maxValue = 1f;

	// Token: 0x040039B5 RID: 14773
	public bool UseSmoothedLoudness;
}
