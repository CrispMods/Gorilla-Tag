using System;
using UnityEngine;

// Token: 0x02000164 RID: 356
[Serializable]
public class GestureNode
{
	// Token: 0x04000ABC RID: 2748
	public bool track;

	// Token: 0x04000ABD RID: 2749
	public GestureHandState state;

	// Token: 0x04000ABE RID: 2750
	public GestureDigitFlexion flexion;

	// Token: 0x04000ABF RID: 2751
	public GestureAlignment alignment;

	// Token: 0x04000AC0 RID: 2752
	[Space]
	public GestureNodeFlags flags;
}
