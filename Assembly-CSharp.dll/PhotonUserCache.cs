using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Realtime;

// Token: 0x020007AF RID: 1967
public class PhotonUserCache : IInRoomCallbacks, IMatchmakingCallbacks
{
	// Token: 0x0600307B RID: 12411 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer)
	{
	}

	// Token: 0x0600307C RID: 12412 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IMatchmakingCallbacks.OnJoinedRoom()
	{
	}

	// Token: 0x0600307D RID: 12413 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IMatchmakingCallbacks.OnLeftRoom()
	{
	}

	// Token: 0x0600307E RID: 12414 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IInRoomCallbacks.OnPlayerLeftRoom(Player player)
	{
	}

	// Token: 0x0600307F RID: 12415 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)
	{
	}

	// Token: 0x06003080 RID: 12416 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
	{
	}

	// Token: 0x06003081 RID: 12417 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IMatchmakingCallbacks.OnCreatedRoom()
	{
	}

	// Token: 0x06003082 RID: 12418 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IMatchmakingCallbacks.OnPreLeavingRoom()
	{
	}

	// Token: 0x06003083 RID: 12419 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message)
	{
	}

	// Token: 0x06003084 RID: 12420 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IMatchmakingCallbacks.OnFriendListUpdate(List<FriendInfo> friendList)
	{
	}

	// Token: 0x06003085 RID: 12421 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IInRoomCallbacks.OnRoomPropertiesUpdate(Hashtable changedProperties)
	{
	}

	// Token: 0x06003086 RID: 12422 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player player, Hashtable changedProperties)
	{
	}

	// Token: 0x06003087 RID: 12423 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
	{
	}
}
