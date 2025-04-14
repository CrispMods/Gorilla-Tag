using System;
using UnityEngine;

// Token: 0x02000478 RID: 1144
public class GorillaSurfaceOverride : MonoBehaviour
{
	// Token: 0x04001EE3 RID: 7907
	[GorillaSoundLookup]
	public int overrideIndex;

	// Token: 0x04001EE4 RID: 7908
	public float extraVelMultiplier = 1f;

	// Token: 0x04001EE5 RID: 7909
	public float extraVelMaxMultiplier = 1f;

	// Token: 0x04001EE6 RID: 7910
	[HideInInspector]
	[NonSerialized]
	public float slidePercentageOverride = -1f;

	// Token: 0x04001EE7 RID: 7911
	public bool sendOnTapEvent;

	// Token: 0x04001EE8 RID: 7912
	public bool disablePushBackEffect;
}
