using System;
using GorillaTagScripts.ModIO;
using ModIO;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000611 RID: 1553
internal class ModIOSerializer : GorillaSerializer
{
	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x060026AD RID: 9901 RVA: 0x000A2DED File Offset: 0x000A0FED
	internal bool HasAuthority
	{
		get
		{
			return this.photonView.IsMine;
		}
	}

	// Token: 0x060026AE RID: 9902 RVA: 0x000BE8E8 File Offset: 0x000BCAE8
	protected void Start()
	{
		RoomSystem.PlayerLeftEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerLeftEvent, new Action<NetPlayer>(this.OnPlayerLeftRoom));
		NetworkSystem.Instance.OnMultiplayerStarted += this.OnJoinedRoom;
	}

	// Token: 0x060026AF RID: 9903 RVA: 0x000BE920 File Offset: 0x000BCB20
	private void OnPlayerLeftRoom(NetPlayer leavingPlayer)
	{
		if (leavingPlayer.ActorNumber == ModIOMapsTerminal.UpdateAndRetrieveLocalStatus().driverID)
		{
			ModIOMapsTerminal.ResetTerminalControl();
			if (!NetworkSystem.Instance.IsMasterClient)
			{
				return;
			}
			base.SendRPC("SetTerminalControlStatus_RPC", true, new object[]
			{
				false,
				-2
			});
		}
	}

	// Token: 0x060026B0 RID: 9904 RVA: 0x000BE976 File Offset: 0x000BCB76
	private void OnJoinedRoom()
	{
		if (NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		this.waitingForRoomInitialization = true;
		base.SendRPC("RequestRoomInitialization_RPC", false, Array.Empty<object>());
	}

	// Token: 0x060026B1 RID: 9905 RVA: 0x000BE9A0 File Offset: 0x000BCBA0
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
		bool flag = ModIOMapsTerminal.GetDriverID() != -2;
		base.SendRPC("InitializeRoom_RPC", info.Sender, new object[]
		{
			flag,
			ModIOMapsTerminal.GetDriverID(),
			ModIOMapsTerminal.UpdateAndRetrieveLocalStatus().PackData(true),
			ModIOMapLoader.LoadedMapModId
		});
	}

	// Token: 0x060026B2 RID: 9906 RVA: 0x000BEA3C File Offset: 0x000BCC3C
	[PunRPC]
	private void InitializeRoom_RPC(bool locked, int driverID, long[] statusData, long loadedMapModID, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "InitializeRoom_RPC");
		if (!info.Sender.IsMasterClient || !this.waitingForRoomInitialization)
		{
			return;
		}
		ModIOMapsTerminal.SetTerminalControlStatus(locked, locked ? driverID : -2, false);
		ModIOMapsTerminal.UpdateStatusFromDriver(statusData, false);
		if (loadedMapModID > 0L)
		{
			CustomMapManager.SetRoomMod(loadedMapModID);
		}
		this.waitingForRoomInitialization = false;
	}

	// Token: 0x060026B3 RID: 9907 RVA: 0x000BEA98 File Offset: 0x000BCC98
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

	// Token: 0x060026B4 RID: 9908 RVA: 0x000BEAE9 File Offset: 0x000BCCE9
	public void UnloadModSynced()
	{
		CustomMapManager.UnloadMod(true);
		CustomMapManager.ClearRoomMap();
		if (NetworkSystem.Instance.InRoom && NetworkSystem.Instance.SessionIsPrivate)
		{
			base.SendRPC("UnloadMod_RPC", true, Array.Empty<object>());
		}
	}

	// Token: 0x060026B5 RID: 9909 RVA: 0x000BEB20 File Offset: 0x000BCD20
	[PunRPC]
	private void SetRoomMap_RPC(long modId, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "SetRoomMap_RPC");
		if (modId <= 0L)
		{
			return;
		}
		if (info.Sender.ActorNumber != this.photonView.OwnerActorNr && info.Sender.ActorNumber != ModIOMapsTerminal.GetDriverID())
		{
			return;
		}
		if (modId != this.detailsScreen.currentModProfile.id.id)
		{
			return;
		}
		CustomMapManager.SetRoomMod(modId);
	}

	// Token: 0x060026B6 RID: 9910 RVA: 0x000BEB88 File Offset: 0x000BCD88
	[PunRPC]
	private void UnloadMod_RPC(PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "UnloadMod_RPC");
		if (!CustomMapManager.AreAllPlayersInVirtualStump())
		{
			return;
		}
		if (info.Sender.ActorNumber != ModIOMapsTerminal.GetDriverID())
		{
			return;
		}
		CustomMapManager.UnloadMod(true);
	}

	// Token: 0x060026B7 RID: 9911 RVA: 0x000BEBB7 File Offset: 0x000BCDB7
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

	// Token: 0x060026B8 RID: 9912 RVA: 0x000BEBE4 File Offset: 0x000BCDE4
	[PunRPC]
	private void RequestTerminalControlStatusChange_RPC(bool lockedStatus, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RequestTerminalControlStatusChange_RPC");
		if (!NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		if (!NetworkSystem.Instance.GetPlayer(info.Sender).IsNull && CustomMapManager.IsRemotePlayerInVirtualStump(info.Sender.UserId))
		{
			ModIOMapsTerminal.HandleTerminalControlStatusChangeRequest(lockedStatus, info.Sender.ActorNumber);
		}
	}

	// Token: 0x060026B9 RID: 9913 RVA: 0x000BEC43 File Offset: 0x000BCE43
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

	// Token: 0x060026BA RID: 9914 RVA: 0x000BEC82 File Offset: 0x000BCE82
	[PunRPC]
	private void SetTerminalControlStatus_RPC(bool locked, int driverID, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "SetTerminalControlStatus_RPC");
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		ModIOMapsTerminal.SetTerminalControlStatus(locked, driverID, false);
	}

	// Token: 0x060026BB RID: 9915 RVA: 0x000BECA8 File Offset: 0x000BCEA8
	public void SendTerminalStatus(bool sendFullModList = false, bool forceSearch = false)
	{
		if (!NetworkSystem.Instance.InRoom || !ModIOMapsTerminal.IsDriver)
		{
			return;
		}
		if (forceSearch)
		{
			base.SendRPC("ForceNewSearch_RPC", true, new object[]
			{
				ModIOMapsTerminal.UpdateAndRetrieveLocalStatus().PackData(sendFullModList)
			});
			return;
		}
		base.SendRPC("UpdateScreen_RPC", true, new object[]
		{
			ModIOMapsTerminal.UpdateAndRetrieveLocalStatus().PackData(sendFullModList)
		});
	}

	// Token: 0x060026BC RID: 9916 RVA: 0x000BED0D File Offset: 0x000BCF0D
	[PunRPC]
	private void UpdateScreen_RPC(long[] statusData, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "UpdateScreen_RPC");
		if (info.Sender.ActorNumber == ModIOMapsTerminal.GetDriverID() && CustomMapManager.IsRemotePlayerInVirtualStump(info.Sender.UserId))
		{
			ModIOMapsTerminal.UpdateStatusFromDriver(statusData, false);
		}
	}

	// Token: 0x060026BD RID: 9917 RVA: 0x000BED45 File Offset: 0x000BCF45
	[PunRPC]
	private void ForceNewSearch_RPC(long[] statusData, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "ForceNewSearch_RPC");
		if (info.Sender.ActorNumber == ModIOMapsTerminal.GetDriverID() && CustomMapManager.IsRemotePlayerInVirtualStump(info.Sender.UserId))
		{
			ModIOMapsTerminal.UpdateStatusFromDriver(statusData, true);
		}
	}

	// Token: 0x060026BE RID: 9918 RVA: 0x000BED7D File Offset: 0x000BCF7D
	public void RefreshDriverNickName()
	{
		if (!NetworkSystem.Instance.InRoom)
		{
			return;
		}
		base.SendRPC("RefreshDriverNickName_RPC", true, Array.Empty<object>());
	}

	// Token: 0x060026BF RID: 9919 RVA: 0x000BED9D File Offset: 0x000BCF9D
	[PunRPC]
	private void RefreshDriverNickName_RPC(PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RefreshDriverNickName_RPC");
		ModIOMapsTerminal.RefreshDriverNickName();
	}

	// Token: 0x04002A8E RID: 10894
	[SerializeField]
	private VirtualStumpBarrierSFX barrierSFX;

	// Token: 0x04002A8F RID: 10895
	[SerializeField]
	private ModIOModDetailsScreen detailsScreen;

	// Token: 0x04002A90 RID: 10896
	private bool waitingForRoomInitialization;
}
