using System;
using GorillaTagScripts.ModIO;
using ModIO;
using Photon.Pun;
using UnityEngine;

// Token: 0x020006A7 RID: 1703
internal class VirtualStumpSerializer : GorillaSerializer
{
	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x06002A50 RID: 10832 RVA: 0x000464D7 File Offset: 0x000446D7
	internal bool HasAuthority
	{
		get
		{
			return this.photonView.IsMine;
		}
	}

	// Token: 0x06002A51 RID: 10833 RVA: 0x0004C8AE File Offset: 0x0004AAAE
	protected void Start()
	{
		NetworkSystem.Instance.OnMultiplayerStarted += this.OnJoinedRoom;
		NetworkSystem.Instance.OnPlayerLeft += this.OnPlayerLeftRoom;
	}

	// Token: 0x06002A52 RID: 10834 RVA: 0x0011C154 File Offset: 0x0011A354
	private void OnPlayerLeftRoom(NetPlayer leavingPlayer)
	{
		if (!NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		int driverID = CustomMapsTerminal.GetDriverID();
		if (leavingPlayer.ActorNumber == driverID)
		{
			CustomMapsTerminal.SetTerminalControlStatus(false, -2, true);
		}
	}

	// Token: 0x06002A53 RID: 10835 RVA: 0x0004C8DC File Offset: 0x0004AADC
	private void OnJoinedRoom()
	{
		if (NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		this.waitingForRoomInitialization = true;
		base.SendRPC("RequestRoomInitialization_RPC", false, Array.Empty<object>());
	}

	// Token: 0x06002A54 RID: 10836 RVA: 0x0011C188 File Offset: 0x0011A388
	[PunRPC]
	private void RequestRoomInitialization_RPC(PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RequestRoomInitialization_RPC");
		if (!NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		if (player.CheckSingleCallRPC(NetPlayer.SingleCallRPC.CMS_RequestRoomInitialization))
		{
			return;
		}
		player.ReceivedSingleCallRPC(NetPlayer.SingleCallRPC.CMS_RequestRoomInitialization);
		bool flag = CustomMapsTerminal.GetDriverID() != -2;
		base.SendRPC("InitializeRoom_RPC", info.Sender, new object[]
		{
			flag,
			CustomMapsTerminal.GetDriverID(),
			CustomMapsTerminal.UpdateAndRetrieveLocalStatus().PackData(true),
			CustomMapLoader.LoadedMapModId
		});
	}

	// Token: 0x06002A55 RID: 10837 RVA: 0x0011C224 File Offset: 0x0011A424
	[PunRPC]
	private void InitializeRoom_RPC(bool locked, int driverID, long[] statusData, long loadedMapModID, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "InitializeRoom_RPC");
		if (!info.Sender.IsMasterClient || !this.waitingForRoomInitialization)
		{
			return;
		}
		if (statusData.IsNullOrEmpty<long>())
		{
			return;
		}
		if (NetworkSystem.Instance.GetPlayer(driverID) == null)
		{
			return;
		}
		CustomMapsTerminal.ClearTags();
		CustomMapsTerminal.UpdateStatusFromDriver(statusData, driverID, false);
		if (loadedMapModID > 0L)
		{
			CustomMapManager.SetRoomMod(loadedMapModID);
		}
		this.waitingForRoomInitialization = false;
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x0011C28C File Offset: 0x0011A48C
	public void LoadModSynced(long modId)
	{
		CustomMapManager.SetRoomMod(modId);
		CustomMapManager.LoadMod(new ModId(modId));
		if (NetworkSystem.Instance.InRoom && NetworkSystem.Instance.SessionIsPrivate)
		{
			base.SendRPC("SetRoomMap_RPC", true, new object[]
			{
				modId
			});
		}
	}

	// Token: 0x06002A57 RID: 10839 RVA: 0x0004C903 File Offset: 0x0004AB03
	public void UnloadModSynced()
	{
		CustomMapManager.UnloadMod(true);
		CustomMapManager.ClearRoomMap();
		if (NetworkSystem.Instance.InRoom && NetworkSystem.Instance.SessionIsPrivate)
		{
			base.SendRPC("UnloadMod_RPC", true, Array.Empty<object>());
		}
	}

	// Token: 0x06002A58 RID: 10840 RVA: 0x0011C2E0 File Offset: 0x0011A4E0
	[PunRPC]
	private void SetRoomMap_RPC(long modId, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "SetRoomMap_RPC");
		if (modId <= 0L)
		{
			return;
		}
		if (info.Sender.ActorNumber != this.photonView.OwnerActorNr && info.Sender.ActorNumber != CustomMapsTerminal.GetDriverID())
		{
			return;
		}
		if (modId != this.detailsScreen.currentModProfile.id.id)
		{
			return;
		}
		CustomMapManager.SetRoomMod(modId);
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x0004C93A File Offset: 0x0004AB3A
	[PunRPC]
	private void UnloadMod_RPC(PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "UnloadMod_RPC");
		if (info.Sender.ActorNumber != CustomMapsTerminal.GetDriverID())
		{
			return;
		}
		if (!CustomMapManager.AreAllPlayersInVirtualStump())
		{
			return;
		}
		CustomMapManager.UnloadMod(true);
		CustomMapManager.ClearRoomMap();
	}

	// Token: 0x06002A5A RID: 10842 RVA: 0x0004C96E File Offset: 0x0004AB6E
	public void RequestTerminalControlStatusChange(bool lockedStatus)
	{
		if (!NetworkSystem.Instance.InRoom)
		{
			return;
		}
		base.SendRPC("RequestTerminalControlStatusChange_RPC", false, new object[]
		{
			lockedStatus
		});
	}

	// Token: 0x06002A5B RID: 10843 RVA: 0x0011C348 File Offset: 0x0011A548
	[PunRPC]
	private void RequestTerminalControlStatusChange_RPC(bool lockedStatus, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RequestTerminalControlStatusChange_RPC");
		if (!NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(player, out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[19].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		if (!player.IsNull && CustomMapManager.IsRemotePlayerInVirtualStump(info.Sender.UserId))
		{
			CustomMapsTerminal.HandleTerminalControlStatusChangeRequest(lockedStatus, info.Sender.ActorNumber);
		}
	}

	// Token: 0x06002A5C RID: 10844 RVA: 0x0004C998 File Offset: 0x0004AB98
	public void SetTerminalControlStatus(bool locked, int playerID)
	{
		if (!NetworkSystem.Instance.InRoom || !NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		base.SendRPC("SetTerminalControlStatus_RPC", true, new object[]
		{
			locked,
			playerID
		});
	}

	// Token: 0x06002A5D RID: 10845 RVA: 0x0011C3E0 File Offset: 0x0011A5E0
	[PunRPC]
	private void SetTerminalControlStatus_RPC(bool locked, int driverID, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "SetTerminalControlStatus_RPC");
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		if (NetworkSystem.Instance.GetPlayer(driverID) == null)
		{
			return;
		}
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(player, out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[16].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		CustomMapsTerminal.SetTerminalControlStatus(locked, driverID, false);
	}

	// Token: 0x06002A5E RID: 10846 RVA: 0x0011C464 File Offset: 0x0011A664
	public void SendTerminalStatus(bool sendFullModList = false, bool forceSearch = false)
	{
		if (!NetworkSystem.Instance.InRoom || !CustomMapsTerminal.IsDriver)
		{
			return;
		}
		base.SendRPC("UpdateScreen_RPC", true, new object[]
		{
			CustomMapsTerminal.UpdateAndRetrieveLocalStatus().PackData(sendFullModList),
			forceSearch,
			CustomMapsTerminal.GetDriverID()
		});
	}

	// Token: 0x06002A5F RID: 10847 RVA: 0x0011C4BC File Offset: 0x0011A6BC
	[PunRPC]
	private void UpdateScreen_RPC(long[] statusData, bool forceNewSearch, int driverID, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "UpdateScreen_RPC");
		if (info.Sender.ActorNumber != CustomMapsTerminal.GetDriverID() || !CustomMapManager.IsRemotePlayerInVirtualStump(info.Sender.UserId))
		{
			return;
		}
		if (statusData.IsNullOrEmpty<long>())
		{
			return;
		}
		if (NetworkSystem.Instance.GetPlayer(driverID) == null)
		{
			return;
		}
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(player, out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[17].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		CustomMapsTerminal.UpdateStatusFromDriver(statusData, driverID, forceNewSearch);
	}

	// Token: 0x06002A60 RID: 10848 RVA: 0x0004C9D7 File Offset: 0x0004ABD7
	public void RefreshDriverNickName()
	{
		if (!NetworkSystem.Instance.InRoom)
		{
			return;
		}
		base.SendRPC("RefreshDriverNickName_RPC", true, Array.Empty<object>());
	}

	// Token: 0x06002A61 RID: 10849 RVA: 0x0011C564 File Offset: 0x0011A764
	[PunRPC]
	private void RefreshDriverNickName_RPC(PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RefreshDriverNickName_RPC");
		if (info.Sender.ActorNumber != CustomMapsTerminal.GetDriverID())
		{
			return;
		}
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(player, out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[18].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		CustomMapsTerminal.RefreshDriverNickName();
	}

	// Token: 0x04002FE8 RID: 12264
	[SerializeField]
	private VirtualStumpBarrierSFX barrierSFX;

	// Token: 0x04002FE9 RID: 12265
	[SerializeField]
	private CustomMapsDetailsScreen detailsScreen;

	// Token: 0x04002FEA RID: 12266
	private bool waitingForRoomInitialization;
}
