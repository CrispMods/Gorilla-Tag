using System;
using UnityEngine;

// Token: 0x020001B2 RID: 434
public class FlagCauldronColorer : MonoBehaviour
{
	// Token: 0x04000C9B RID: 3227
	public FlagCauldronColorer.ColorMode mode;

	// Token: 0x04000C9C RID: 3228
	public Transform colorPoint;

	// Token: 0x020001B3 RID: 435
	public enum ColorMode
	{
		// Token: 0x04000C9E RID: 3230
		None,
		// Token: 0x04000C9F RID: 3231
		Red,
		// Token: 0x04000CA0 RID: 3232
		Green,
		// Token: 0x04000CA1 RID: 3233
		Blue,
		// Token: 0x04000CA2 RID: 3234
		Black,
		// Token: 0x04000CA3 RID: 3235
		Clear
	}
}
