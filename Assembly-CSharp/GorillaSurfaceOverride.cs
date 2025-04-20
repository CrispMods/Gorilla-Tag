using System;
using UnityEngine;

// Token: 0x02000484 RID: 1156
public class GorillaSurfaceOverride : MonoBehaviour
{
	// Token: 0x04001F32 RID: 7986
	[GorillaSoundLookup]
	public int overrideIndex;

	// Token: 0x04001F33 RID: 7987
	public float extraVelMultiplier = 1f;

	// Token: 0x04001F34 RID: 7988
	public float extraVelMaxMultiplier = 1f;

	// Token: 0x04001F35 RID: 7989
	[HideInInspector]
	[NonSerialized]
	public float slidePercentageOverride = -1f;

	// Token: 0x04001F36 RID: 7990
	public bool sendOnTapEvent;

	// Token: 0x04001F37 RID: 7991
	public bool disablePushBackEffect;
}
