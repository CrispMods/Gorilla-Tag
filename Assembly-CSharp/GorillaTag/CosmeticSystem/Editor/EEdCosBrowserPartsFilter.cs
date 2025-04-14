using System;

namespace GorillaTag.CosmeticSystem.Editor
{
	// Token: 0x02000BFF RID: 3071
	[Flags]
	public enum EEdCosBrowserPartsFilter
	{
		// Token: 0x04004F30 RID: 20272
		None = 0,
		// Token: 0x04004F31 RID: 20273
		NoParts = 1,
		// Token: 0x04004F32 RID: 20274
		Holdable = 2,
		// Token: 0x04004F33 RID: 20275
		Functional = 4,
		// Token: 0x04004F34 RID: 20276
		Wardrobe = 8,
		// Token: 0x04004F35 RID: 20277
		Store = 16,
		// Token: 0x04004F36 RID: 20278
		FirstPerson = 32,
		// Token: 0x04004F37 RID: 20279
		All = 63
	}
}
