using System;

// Token: 0x020007F8 RID: 2040
internal interface IUserCosmeticsCallback
{
	// Token: 0x0600323E RID: 12862
	bool OnGetUserCosmetics(string cosmetics);

	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x0600323F RID: 12863
	// (set) Token: 0x06003240 RID: 12864
	bool PendingUpdate { get; set; }
}
