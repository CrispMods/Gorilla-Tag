using System;

// Token: 0x020001FD RID: 509
public interface IRequestableOwnershipGuardCallbacks
{
	// Token: 0x06000C0B RID: 3083
	void OnOwnershipTransferred(NetPlayer toPlayer, NetPlayer fromPlayer);

	// Token: 0x06000C0C RID: 3084
	bool OnOwnershipRequest(NetPlayer fromPlayer);

	// Token: 0x06000C0D RID: 3085
	void OnMyOwnerLeft();

	// Token: 0x06000C0E RID: 3086
	bool OnMasterClientAssistedTakeoverRequest(NetPlayer fromPlayer, NetPlayer toPlayer);

	// Token: 0x06000C0F RID: 3087
	void OnMyCreatorLeft();
}
