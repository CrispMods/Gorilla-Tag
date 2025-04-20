using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Fusion;
using Fusion.Sockets;
using GorillaExtensions;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020001F6 RID: 502
internal class RequestableOwnershipGaurdHandler : IPunOwnershipCallbacks, IInRoomCallbacks, INetworkRunnerCallbacks
{
	// Token: 0x06000BB4 RID: 2996 RVA: 0x000384C1 File Offset: 0x000366C1
	static RequestableOwnershipGaurdHandler()
	{
		PhotonNetwork.AddCallbackTarget(RequestableOwnershipGaurdHandler.callbackInstance);
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x000384EB File Offset: 0x000366EB
	internal static void RegisterView(NetworkView view, RequestableOwnershipGuard guard)
	{
		if (view == null || RequestableOwnershipGaurdHandler.gaurdedViews.Contains(view))
		{
			return;
		}
		RequestableOwnershipGaurdHandler.gaurdedViews.Add(view);
		RequestableOwnershipGaurdHandler.guardingLookup.Add(view, guard);
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x0003851C File Offset: 0x0003671C
	internal static void RemoveView(NetworkView view)
	{
		if (view == null)
		{
			return;
		}
		RequestableOwnershipGaurdHandler.gaurdedViews.Remove(view);
		RequestableOwnershipGaurdHandler.guardingLookup.Remove(view);
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x0009BE9C File Offset: 0x0009A09C
	internal static void RegisterViews(NetworkView[] views, RequestableOwnershipGuard guard)
	{
		for (int i = 0; i < views.Length; i++)
		{
			RequestableOwnershipGaurdHandler.RegisterView(views[i], guard);
		}
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x0009BEC4 File Offset: 0x0009A0C4
	public static void RemoveViews(NetworkView[] views, RequestableOwnershipGuard guard)
	{
		for (int i = 0; i < views.Length; i++)
		{
			RequestableOwnershipGaurdHandler.RemoveView(views[i]);
		}
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x0009BEEC File Offset: 0x0009A0EC
	void IPunOwnershipCallbacks.OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
	{
		NetworkView networkView = RequestableOwnershipGaurdHandler.gaurdedViews.FirstOrDefault((NetworkView p) => p.GetView == targetView);
		RequestableOwnershipGuard requestableOwnershipGuard;
		if (networkView.IsNull() || !RequestableOwnershipGaurdHandler.guardingLookup.TryGetValue(networkView, out requestableOwnershipGuard) || requestableOwnershipGuard.IsNull())
		{
			return;
		}
		NetPlayer currentOwner = requestableOwnershipGuard.currentOwner;
		Player player = (currentOwner != null) ? currentOwner.GetPlayerRef() : null;
		int num = (player != null) ? player.ActorNumber : 0;
		if (num == 0 || previousOwner != player)
		{
			GTDev.LogError<string>("Ownership transferred but the previous owner didn't initiate the request, Switching back", null);
			targetView.OwnerActorNr = num;
			targetView.ControllerActorNr = num;
		}
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x00038540 File Offset: 0x00036740
	void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
	{
		this.OnHostChangedShared();
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x00038540 File Offset: 0x00036740
	public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
	{
		this.OnHostChangedShared();
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x0009BF8C File Offset: 0x0009A18C
	private void OnHostChangedShared()
	{
		foreach (NetworkView networkView in RequestableOwnershipGaurdHandler.gaurdedViews)
		{
			RequestableOwnershipGuard requestableOwnershipGuard;
			if (!RequestableOwnershipGaurdHandler.guardingLookup.TryGetValue(networkView, out requestableOwnershipGuard))
			{
				break;
			}
			if (networkView.Owner != null && requestableOwnershipGuard.currentOwner != null && !object.Equals(networkView.Owner, requestableOwnershipGuard.currentOwner))
			{
				networkView.OwnerActorNr = requestableOwnershipGuard.currentOwner.ActorNumber;
				networkView.ControllerActorNr = requestableOwnershipGuard.currentOwner.ActorNumber;
			}
		}
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x00030607 File Offset: 0x0002E807
	void IPunOwnershipCallbacks.OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
	{
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x00030607 File Offset: 0x0002E807
	void IPunOwnershipCallbacks.OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
	{
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnPlayerEnteredRoom(Player newPlayer)
	{
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnPlayerLeftRoom(Player otherPlayer)
	{
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
	{
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
	{
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
	{
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnInput(NetworkRunner runner, NetworkInput input)
	{
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
	{
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
	{
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnConnectedToServer(NetworkRunner runner)
	{
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
	{
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
	{
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
	{
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
	{
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
	{
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
	{
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
	{
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
	{
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnSceneLoadDone(NetworkRunner runner)
	{
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnSceneLoadStart(NetworkRunner runner)
	{
	}

	// Token: 0x04000E46 RID: 3654
	private static HashSet<NetworkView> gaurdedViews = new HashSet<NetworkView>();

	// Token: 0x04000E47 RID: 3655
	private static readonly RequestableOwnershipGaurdHandler callbackInstance = new RequestableOwnershipGaurdHandler();

	// Token: 0x04000E48 RID: 3656
	private static Dictionary<NetworkView, RequestableOwnershipGuard> guardingLookup = new Dictionary<NetworkView, RequestableOwnershipGuard>();
}
