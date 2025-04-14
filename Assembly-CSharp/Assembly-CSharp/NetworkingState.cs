using System;

// Token: 0x020001ED RID: 493
public enum NetworkingState
{
	// Token: 0x04000E06 RID: 3590
	IsOwner,
	// Token: 0x04000E07 RID: 3591
	IsBlindClient,
	// Token: 0x04000E08 RID: 3592
	IsClient,
	// Token: 0x04000E09 RID: 3593
	ForcefullyTakingOver,
	// Token: 0x04000E0A RID: 3594
	RequestingOwnership,
	// Token: 0x04000E0B RID: 3595
	RequestingOwnershipWaitingForSight,
	// Token: 0x04000E0C RID: 3596
	ForcefullyTakingOverWaitingForSight
}
