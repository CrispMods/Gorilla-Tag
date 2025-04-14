using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Fusion;
using Fusion.Sockets;
using GorillaExtensions;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020001EB RID: 491
internal class RequestableOwnershipGaurdHandler : IPunOwnershipCallbacks, IInRoomCallbacks, INetworkRunnerCallbacks
{
	// Token: 0x06000B68 RID: 2920 RVA: 0x0003D18C File Offset: 0x0003B38C
	static RequestableOwnershipGaurdHandler()
	{
		PhotonNetwork.AddCallbackTarget(RequestableOwnershipGaurdHandler.callbackInstance);
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x0003D1B6 File Offset: 0x0003B3B6
	internal static void RegisterView(NetworkView view, RequestableOwnershipGuard guard)
	{
		if (view == null || RequestableOwnershipGaurdHandler.gaurdedViews.Contains(view))
		{
			return;
		}
		RequestableOwnershipGaurdHandler.gaurdedViews.Add(view);
		RequestableOwnershipGaurdHandler.guardingLookup.Add(view, guard);
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x0003D1E7 File Offset: 0x0003B3E7
	internal static void RemoveView(NetworkView view)
	{
		if (view == null)
		{
			return;
		}
		RequestableOwnershipGaurdHandler.gaurdedViews.Remove(view);
		RequestableOwnershipGaurdHandler.guardingLookup.Remove(view);
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x0003D20C File Offset: 0x0003B40C
	internal static void RegisterViews(NetworkView[] views, RequestableOwnershipGuard guard)
	{
		for (int i = 0; i < views.Length; i++)
		{
			RequestableOwnershipGaurdHandler.RegisterView(views[i], guard);
		}
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x0003D234 File Offset: 0x0003B434
	public static void RemoveViews(NetworkView[] views, RequestableOwnershipGuard guard)
	{
		for (int i = 0; i < views.Length; i++)
		{
			RequestableOwnershipGaurdHandler.RemoveView(views[i]);
		}
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x0003D25C File Offset: 0x0003B45C
	void IPunOwnershipCallbacks.OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
	{
		if (RequestableOwnershipGaurdHandler.gaurdedViews.FirstOrDefault((NetworkView p) => p.GetView == targetView).IsNull())
		{
			return;
		}
		RequestableOwnershipGuard requestableOwnershipGuard;
		if (!this.FindTargetInLookup(targetView, out requestableOwnershipGuard))
		{
			return;
		}
		if (!object.Equals(previousOwner, requestableOwnershipGuard.currentOwner))
		{
			Debug.LogError("Ownership transferred but the previous owner didn't initiate the request, Switching back");
			targetView.OwnerActorNr = requestableOwnershipGuard.currentOwner.ActorNumber;
			targetView.ControllerActorNr = requestableOwnershipGuard.currentOwner.ActorNumber;
		}
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x0003D2EC File Offset: 0x0003B4EC
	private bool FindTargetInLookup(PhotonView targetView, out RequestableOwnershipGuard guard)
	{
		foreach (NetworkView networkView in RequestableOwnershipGaurdHandler.guardingLookup.Keys)
		{
			if (networkView.GetView == targetView)
			{
				guard = RequestableOwnershipGaurdHandler.guardingLookup[networkView];
				return true;
			}
		}
		guard = null;
		return false;
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x0003D364 File Offset: 0x0003B564
	void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
	{
		this.OnHostChangedShared();
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x0003D364 File Offset: 0x0003B564
	public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
	{
		this.OnHostChangedShared();
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x0003D36C File Offset: 0x0003B56C
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

	// Token: 0x06000B72 RID: 2930 RVA: 0x000023F4 File Offset: 0x000005F4
	void IPunOwnershipCallbacks.OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
	{
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x000023F4 File Offset: 0x000005F4
	void IPunOwnershipCallbacks.OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
	{
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnPlayerEnteredRoom(Player newPlayer)
	{
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnPlayerLeftRoom(Player otherPlayer)
	{
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
	{
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
	{
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
	{
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnInput(NetworkRunner runner, NetworkInput input)
	{
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
	{
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
	{
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnConnectedToServer(NetworkRunner runner)
	{
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
	{
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
	{
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
	{
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
	{
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
	{
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
	{
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
	{
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
	{
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnSceneLoadDone(NetworkRunner runner)
	{
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnSceneLoadStart(NetworkRunner runner)
	{
	}

	// Token: 0x04000E00 RID: 3584
	private static HashSet<NetworkView> gaurdedViews = new HashSet<NetworkView>();

	// Token: 0x04000E01 RID: 3585
	private static readonly RequestableOwnershipGaurdHandler callbackInstance = new RequestableOwnershipGaurdHandler();

	// Token: 0x04000E02 RID: 3586
	private static Dictionary<NetworkView, RequestableOwnershipGuard> guardingLookup = new Dictionary<NetworkView, RequestableOwnershipGuard>();
}
