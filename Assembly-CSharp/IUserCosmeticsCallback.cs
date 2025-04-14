using System;

// Token: 0x020007F5 RID: 2037
internal interface IUserCosmeticsCallback
{
	// Token: 0x06003232 RID: 12850
	bool OnGetUserCosmetics(string cosmetics);

	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x06003233 RID: 12851
	// (set) Token: 0x06003234 RID: 12852
	bool PendingUpdate { get; set; }
}
