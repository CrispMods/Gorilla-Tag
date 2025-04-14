using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Realtime;

// Token: 0x020007AE RID: 1966
public class PhotonUserCache : IInRoomCallbacks, IMatchmakingCallbacks
{
	// Token: 0x06003073 RID: 12403 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer)
	{
	}

	// Token: 0x06003074 RID: 12404 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnJoinedRoom()
	{
	}

	// Token: 0x06003075 RID: 12405 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnLeftRoom()
	{
	}

	// Token: 0x06003076 RID: 12406 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnPlayerLeftRoom(Player player)
	{
	}

	// Token: 0x06003077 RID: 12407 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)
	{
	}

	// Token: 0x06003078 RID: 12408 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
	{
	}

	// Token: 0x06003079 RID: 12409 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnCreatedRoom()
	{
	}

	// Token: 0x0600307A RID: 12410 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnPreLeavingRoom()
	{
	}

	// Token: 0x0600307B RID: 12411 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message)
	{
	}

	// Token: 0x0600307C RID: 12412 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnFriendListUpdate(List<FriendInfo> friendList)
	{
	}

	// Token: 0x0600307D RID: 12413 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnRoomPropertiesUpdate(Hashtable changedProperties)
	{
	}

	// Token: 0x0600307E RID: 12414 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player player, Hashtable changedProperties)
	{
	}

	// Token: 0x0600307F RID: 12415 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
	{
	}
}
