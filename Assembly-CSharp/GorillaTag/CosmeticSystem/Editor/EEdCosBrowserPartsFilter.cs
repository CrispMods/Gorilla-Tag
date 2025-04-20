using System;

namespace GorillaTag.CosmeticSystem.Editor
{
	// Token: 0x02000C2D RID: 3117
	[Flags]
	public enum EEdCosBrowserPartsFilter
	{
		// Token: 0x04005026 RID: 20518
		None = 0,
		// Token: 0x04005027 RID: 20519
		NoParts = 1,
		// Token: 0x04005028 RID: 20520
		Holdable = 2,
		// Token: 0x04005029 RID: 20521
		Functional = 4,
		// Token: 0x0400502A RID: 20522
		Wardrobe = 8,
		// Token: 0x0400502B RID: 20523
		Store = 16,
		// Token: 0x0400502C RID: 20524
		FirstPerson = 32,
		// Token: 0x0400502D RID: 20525
		All = 63
	}
}
