using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Realtime;

// Token: 0x020007C6 RID: 1990
public class PhotonUserCache : IInRoomCallbacks, IMatchmakingCallbacks
{
	// Token: 0x06003125 RID: 12581 RVA: 0x00030607 File Offset: 0x0002E807
	void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer)
	{
	}

	// Token: 0x06003126 RID: 12582 RVA: 0x00030607 File Offset: 0x0002E807
	void IMatchmakingCallbacks.OnJoinedRoom()
	{
	}

	// Token: 0x06003127 RID: 12583 RVA: 0x00030607 File Offset: 0x0002E807
	void IMatchmakingCallbacks.OnLeftRoom()
	{
	}

	// Token: 0x06003128 RID: 12584 RVA: 0x00030607 File Offset: 0x0002E807
	void IInRoomCallbacks.OnPlayerLeftRoom(Player player)
	{
	}

	// Token: 0x06003129 RID: 12585 RVA: 0x00030607 File Offset: 0x0002E807
	void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)
	{
	}

	// Token: 0x0600312A RID: 12586 RVA: 0x00030607 File Offset: 0x0002E807
	void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
	{
	}

	// Token: 0x0600312B RID: 12587 RVA: 0x00030607 File Offset: 0x0002E807
	void IMatchmakingCallbacks.OnCreatedRoom()
	{
	}

	// Token: 0x0600312C RID: 12588 RVA: 0x00030607 File Offset: 0x0002E807
	void IMatchmakingCallbacks.OnPreLeavingRoom()
	{
	}

	// Token: 0x0600312D RID: 12589 RVA: 0x00030607 File Offset: 0x0002E807
	void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message)
	{
	}

	// Token: 0x0600312E RID: 12590 RVA: 0x00030607 File Offset: 0x0002E807
	void IMatchmakingCallbacks.OnFriendListUpdate(List<FriendInfo> friendList)
	{
	}

	// Token: 0x0600312F RID: 12591 RVA: 0x00030607 File Offset: 0x0002E807
	void IInRoomCallbacks.OnRoomPropertiesUpdate(Hashtable changedProperties)
	{
	}

	// Token: 0x06003130 RID: 12592 RVA: 0x00030607 File Offset: 0x0002E807
	void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player player, Hashtable changedProperties)
	{
	}

	// Token: 0x06003131 RID: 12593 RVA: 0x00030607 File Offset: 0x0002E807
	void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
	{
	}
}
