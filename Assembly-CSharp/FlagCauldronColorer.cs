using System;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class FlagCauldronColorer : MonoBehaviour
{
	// Token: 0x04000C55 RID: 3157
	public FlagCauldronColorer.ColorMode mode;

	// Token: 0x04000C56 RID: 3158
	public Transform colorPoint;

	// Token: 0x020001A8 RID: 424
	public enum ColorMode
	{
		// Token: 0x04000C58 RID: 3160
		None,
		// Token: 0x04000C59 RID: 3161
		Red,
		// Token: 0x04000C5A RID: 3162
		Green,
		// Token: 0x04000C5B RID: 3163
		Blue,
		// Token: 0x04000C5C RID: 3164
		Black,
		// Token: 0x04000C5D RID: 3165
		Clear
	}
}
