using System;

// Token: 0x020001F8 RID: 504
public enum NetworkingState
{
	// Token: 0x04000E4B RID: 3659
	IsOwner,
	// Token: 0x04000E4C RID: 3660
	IsBlindClient,
	// Token: 0x04000E4D RID: 3661
	IsClient,
	// Token: 0x04000E4E RID: 3662
	ForcefullyTakingOver,
	// Token: 0x04000E4F RID: 3663
	RequestingOwnership,
	// Token: 0x04000E50 RID: 3664
	RequestingOwnershipWaitingForSight,
	// Token: 0x04000E51 RID: 3665
	ForcefullyTakingOverWaitingForSight
}
