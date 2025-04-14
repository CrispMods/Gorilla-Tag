using System;

// Token: 0x020001F2 RID: 498
public interface IRequestableOwnershipGuardCallbacks
{
	// Token: 0x06000BC2 RID: 3010
	void OnOwnershipTransferred(NetPlayer toPlayer, NetPlayer fromPlayer);

	// Token: 0x06000BC3 RID: 3011
	bool OnOwnershipRequest(NetPlayer fromPlayer);

	// Token: 0x06000BC4 RID: 3012
	void OnMyOwnerLeft();

	// Token: 0x06000BC5 RID: 3013
	bool OnMasterClientAssistedTakeoverRequest(NetPlayer fromPlayer, NetPlayer toPlayer);

	// Token: 0x06000BC6 RID: 3014
	void OnMyCreatorLeft();
}
