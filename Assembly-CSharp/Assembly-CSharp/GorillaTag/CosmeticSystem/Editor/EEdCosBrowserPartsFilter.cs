using System;

namespace GorillaTag.CosmeticSystem.Editor
{
	// Token: 0x02000C02 RID: 3074
	[Flags]
	public enum EEdCosBrowserPartsFilter
	{
		// Token: 0x04004F42 RID: 20290
		None = 0,
		// Token: 0x04004F43 RID: 20291
		NoParts = 1,
		// Token: 0x04004F44 RID: 20292
		Holdable = 2,
		// Token: 0x04004F45 RID: 20293
		Functional = 4,
		// Token: 0x04004F46 RID: 20294
		Wardrobe = 8,
		// Token: 0x04004F47 RID: 20295
		Store = 16,
		// Token: 0x04004F48 RID: 20296
		FirstPerson = 32,
		// Token: 0x04004F49 RID: 20297
		All = 63
	}
}
