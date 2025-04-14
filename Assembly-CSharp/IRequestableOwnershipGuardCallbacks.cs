using System;

// Token: 0x020001F2 RID: 498
public interface IRequestableOwnershipGuardCallbacks
{
	// Token: 0x06000BC0 RID: 3008
	void OnOwnershipTransferred(NetPlayer toPlayer, NetPlayer fromPlayer);

	// Token: 0x06000BC1 RID: 3009
	bool OnOwnershipRequest(NetPlayer fromPlayer);

	// Token: 0x06000BC2 RID: 3010
	void OnMyOwnerLeft();

	// Token: 0x06000BC3 RID: 3011
	bool OnMasterClientAssistedTakeoverRequest(NetPlayer fromPlayer, NetPlayer toPlayer);

	// Token: 0x06000BC4 RID: 3012
	void OnMyCreatorLeft();
}
