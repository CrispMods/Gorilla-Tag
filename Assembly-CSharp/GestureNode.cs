using System;
using UnityEngine;

// Token: 0x02000159 RID: 345
[Serializable]
public class GestureNode
{
	// Token: 0x04000A75 RID: 2677
	public bool track;

	// Token: 0x04000A76 RID: 2678
	public GestureHandState state;

	// Token: 0x04000A77 RID: 2679
	public GestureDigitFlexion flexion;

	// Token: 0x04000A78 RID: 2680
	public GestureAlignment alignment;

	// Token: 0x04000A79 RID: 2681
	[Space]
	public GestureNodeFlags flags;
}
