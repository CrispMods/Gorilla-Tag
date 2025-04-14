using System;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class FlagCauldronColorer : MonoBehaviour
{
	// Token: 0x04000C56 RID: 3158
	public FlagCauldronColorer.ColorMode mode;

	// Token: 0x04000C57 RID: 3159
	public Transform colorPoint;

	// Token: 0x020001A8 RID: 424
	public enum ColorMode
	{
		// Token: 0x04000C59 RID: 3161
		None,
		// Token: 0x04000C5A RID: 3162
		Red,
		// Token: 0x04000C5B RID: 3163
		Green,
		// Token: 0x04000C5C RID: 3164
		Blue,
		// Token: 0x04000C5D RID: 3165
		Black,
		// Token: 0x04000C5E RID: 3166
		Clear
	}
}
