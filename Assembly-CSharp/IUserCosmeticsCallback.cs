using System;

// Token: 0x0200080F RID: 2063
internal interface IUserCosmeticsCallback
{
	// Token: 0x060032E8 RID: 13032
	bool OnGetUserCosmetics(string cosmetics);

	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x060032E9 RID: 13033
	// (set) Token: 0x060032EA RID: 13034
	bool PendingUpdate { get; set; }
}
