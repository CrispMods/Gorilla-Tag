using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Realtime;

// Token: 0x020007AF RID: 1967
public class PhotonUserCache : IInRoomCallbacks, IMatchmakingCallbacks
{
	// Token: 0x0600307B RID: 12411 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer)
	{
	}

	// Token: 0x0600307C RID: 12412 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnJoinedRoom()
	{
	}

	// Token: 0x0600307D RID: 12413 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnLeftRoom()
	{
	}

	// Token: 0x0600307E RID: 12414 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnPlayerLeftRoom(Player player)
	{
	}

	// Token: 0x0600307F RID: 12415 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)
	{
	}

	// Token: 0x06003080 RID: 12416 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
	{
	}

	// Token: 0x06003081 RID: 12417 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnCreatedRoom()
	{
	}

	// Token: 0x06003082 RID: 12418 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnPreLeavingRoom()
	{
	}

	// Token: 0x06003083 RID: 12419 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message)
	{
	}

	// Token: 0x06003084 RID: 12420 RVA: 0x000023F4 File Offset: 0x000005F4
	void IMatchmakingCallbacks.OnFriendListUpdate(List<FriendInfo> friendList)
	{
	}

	// Token: 0x06003085 RID: 12421 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnRoomPropertiesUpdate(Hashtable changedProperties)
	{
	}

	// Token: 0x06003086 RID: 12422 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player player, Hashtable changedProperties)
	{
	}

	// Token: 0x06003087 RID: 12423 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
	{
	}
}
