using System;
using GorillaTagScripts.ModIO;
using ModIO;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000612 RID: 1554
internal class ModIOSerializer : GorillaSerializer
{
	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x060026B5 RID: 9909 RVA: 0x000A3171 File Offset: 0x000A1371
	internal bool HasAuthority
	{
		get
		{
			return this.photonView.IsMine;
		}
	}

	// Token: 0x060026B6 RID: 9910 RVA: 0x000BED68 File Offset: 0x000BCF68
	protected void Start()
	{
		RoomSystem.PlayerLeftEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerLeftEvent, new Action<NetPlayer>(this.OnPlayerLeftRoom));
		NetworkSystem.Instance.OnMultiplayerStarted += this.OnJoinedRoom;
	}

	// Token: 0x060026B7 RID: 9911 RVA: 0x000BEDA0 File Offset: 0x000BCFA0
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

	// Token: 0x060026B8 RID: 9912 RVA: 0x000BEDF6 File Offset: 0x000BCFF6
	private void OnJoinedRoom()
	{
		if (NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		this.waitingForRoomInitialization = true;
		base.SendRPC("RequestRoomInitialization_RPC", false, Array.Empty<object>());
	}

	// Token: 0x060026B9 RID: 9913 RVA: 0x000BEE20 File Offset: 0x000BD020
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

	// Token: 0x060026BA RID: 9914 RVA: 0x000BEEBC File Offset: 0x000BD0BC
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

	// Token: 0x060026BB RID: 9915 RVA: 0x000BEF18 File Offset: 0x000BD118
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

	// Token: 0x060026BC RID: 9916 RVA: 0x000BEF69 File Offset: 0x000BD169
	public void UnloadModSynced()
	{
		CustomMapManager.UnloadMod(true);
		CustomMapManager.ClearRoomMap();
		if (NetworkSystem.Instance.InRoom && NetworkSystem.Instance.SessionIsPrivate)
		{
			base.SendRPC("UnloadMod_RPC", true, Array.Empty<object>());
		}
	}

	// Token: 0x060026BD RID: 9917 RVA: 0x000BEFA0 File Offset: 0x000BD1A0
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

	// Token: 0x060026BE RID: 9918 RVA: 0x000BF008 File Offset: 0x000BD208
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

	// Token: 0x060026BF RID: 9919 RVA: 0x000BF037 File Offset: 0x000BD237
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

	// Token: 0x060026C0 RID: 9920 RVA: 0x000BF064 File Offset: 0x000BD264
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

	// Token: 0x060026C1 RID: 9921 RVA: 0x000BF0C3 File Offset: 0x000BD2C3
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

	// Token: 0x060026C2 RID: 9922 RVA: 0x000BF102 File Offset: 0x000BD302
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

	// Token: 0x060026C3 RID: 9923 RVA: 0x000BF128 File Offset: 0x000BD328
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

	// Token: 0x060026C4 RID: 9924 RVA: 0x000BF18D File Offset: 0x000BD38D
	[PunRPC]
	private void UpdateScreen_RPC(long[] statusData, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "UpdateScreen_RPC");
		if (info.Sender.ActorNumber == ModIOMapsTerminal.GetDriverID() && CustomMapManager.IsRemotePlayerInVirtualStump(info.Sender.UserId))
		{
			ModIOMapsTerminal.UpdateStatusFromDriver(statusData, false);
		}
	}

	// Token: 0x060026C5 RID: 9925 RVA: 0x000BF1C5 File Offset: 0x000BD3C5
	[PunRPC]
	private void ForceNewSearch_RPC(long[] statusData, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "ForceNewSearch_RPC");
		if (info.Sender.ActorNumber == ModIOMapsTerminal.GetDriverID() && CustomMapManager.IsRemotePlayerInVirtualStump(info.Sender.UserId))
		{
			ModIOMapsTerminal.UpdateStatusFromDriver(statusData, true);
		}
	}

	// Token: 0x060026C6 RID: 9926 RVA: 0x000BF1FD File Offset: 0x000BD3FD
	public void RefreshDriverNickName()
	{
		if (!NetworkSystem.Instance.InRoom)
		{
			return;
		}
		base.SendRPC("RefreshDriverNickName_RPC", true, Array.Empty<object>());
	}

	// Token: 0x060026C7 RID: 9927 RVA: 0x000BF21D File Offset: 0x000BD41D
	[PunRPC]
	private void RefreshDriverNickName_RPC(PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RefreshDriverNickName_RPC");
		ModIOMapsTerminal.RefreshDriverNickName();
	}

	// Token: 0x04002A94 RID: 10900
	[SerializeField]
	private VirtualStumpBarrierSFX barrierSFX;

	// Token: 0x04002A95 RID: 10901
	[SerializeField]
	private ModIOModDetailsScreen detailsScreen;

	// Token: 0x04002A96 RID: 10902
	private bool waitingForRoomInitialization;
}
