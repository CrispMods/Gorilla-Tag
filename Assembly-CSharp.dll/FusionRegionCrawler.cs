using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Fusion;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using UnityEngine;

// Token: 0x02000260 RID: 608
public class FusionRegionCrawler : MonoBehaviour, INetworkRunnerCallbacks
{
	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06000E2E RID: 3630 RVA: 0x0003914F File Offset: 0x0003734F
	public int PlayerCountGlobal
	{
		get
		{
			return this.globalPlayerCount;
		}
	}

	// Token: 0x06000E2F RID: 3631 RVA: 0x00039157 File Offset: 0x00037357
	public void Start()
	{
		this.regionRunner = base.gameObject.AddComponent<NetworkRunner>();
		this.regionRunner.AddCallbacks(new INetworkRunnerCallbacks[]
		{
			this
		});
		base.StartCoroutine(this.OccasionalUpdate());
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x0003918C File Offset: 0x0003738C
	public IEnumerator OccasionalUpdate()
	{
		while (this.refreshPlayerCountAutomatically)
		{
			yield return this.UpdatePlayerCount();
			yield return new WaitForSeconds(this.UpdateFrequency);
		}
		yield break;
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x0003919B File Offset: 0x0003739B
	public IEnumerator UpdatePlayerCount()
	{
		int tempGlobalPlayerCount = 0;
		StartGameArgs startGameArgs = default(StartGameArgs);
		foreach (string fixedRegion in NetworkSystem.Instance.regionNames)
		{
			startGameArgs.CustomPhotonAppSettings = new FusionAppSettings();
			startGameArgs.CustomPhotonAppSettings.FixedRegion = fixedRegion;
			this.waitingForSessionListUpdate = true;
			this.regionRunner.JoinSessionLobby(SessionLobby.ClientServer, startGameArgs.CustomPhotonAppSettings.FixedRegion, null, null, new bool?(false), default(CancellationToken), false);
			while (this.waitingForSessionListUpdate)
			{
				yield return new WaitForEndOfFrame();
			}
			foreach (SessionInfo sessionInfo in this.sessionInfoCache)
			{
				tempGlobalPlayerCount += sessionInfo.PlayerCount;
			}
			tempGlobalPlayerCount += this.tempSessionPlayerCount;
		}
		string[] array = null;
		this.globalPlayerCount = tempGlobalPlayerCount;
		FusionRegionCrawler.PlayerCountUpdated onPlayerCountUpdated = this.OnPlayerCountUpdated;
		if (onPlayerCountUpdated != null)
		{
			onPlayerCountUpdated(this.globalPlayerCount);
		}
		yield break;
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x000391AA File Offset: 0x000373AA
	public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
	{
		if (this.waitingForSessionListUpdate)
		{
			this.sessionInfoCache = sessionList;
			this.waitingForSessionListUpdate = false;
		}
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x0002F75F File Offset: 0x0002D95F
	void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
	{
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x0002F75F File Offset: 0x0002D95F
	void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
	{
	}

	// Token: 0x06000E35 RID: 3637 RVA: 0x0002F75F File Offset: 0x0002D95F
	void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
	{
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x0002F75F File Offset: 0x0002D95F
	void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
	{
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x0002F75F File Offset: 0x0002D95F
	void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
	{
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x0002F75F File Offset: 0x0002D95F
	void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
	{
	}

	// Token: 0x06000E39 RID: 3641 RVA: 0x0002F75F File Offset: 0x0002D95F
	void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
	{
	}

	// Token: 0x06000E3A RID: 3642 RVA: 0x0002F75F File Offset: 0x0002D95F
	void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
	{
	}

	// Token: 0x06000E3B RID: 3643 RVA: 0x0002F75F File Offset: 0x0002D95F
	void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
	{
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x0002F75F File Offset: 0x0002D95F
	void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
	{
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x0002F75F File Offset: 0x0002D95F
	void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
	{
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x0002F75F File Offset: 0x0002D95F
	void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
	{
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x0002F75F File Offset: 0x0002D95F
	void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
	{
	}

	// Token: 0x06000E40 RID: 3648 RVA: 0x0002F75F File Offset: 0x0002D95F
	void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner)
	{
	}

	// Token: 0x06000E41 RID: 3649 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{
	}

	// Token: 0x06000E42 RID: 3650 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{
	}

	// Token: 0x06000E43 RID: 3651 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
	{
	}

	// Token: 0x06000E44 RID: 3652 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
	{
	}

	// Token: 0x06000E45 RID: 3653 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
	{
	}

	// Token: 0x040010F0 RID: 4336
	public FusionRegionCrawler.PlayerCountUpdated OnPlayerCountUpdated;

	// Token: 0x040010F1 RID: 4337
	private NetworkRunner regionRunner;

	// Token: 0x040010F2 RID: 4338
	private List<SessionInfo> sessionInfoCache;

	// Token: 0x040010F3 RID: 4339
	private bool waitingForSessionListUpdate;

	// Token: 0x040010F4 RID: 4340
	private int globalPlayerCount;

	// Token: 0x040010F5 RID: 4341
	private float UpdateFrequency = 10f;

	// Token: 0x040010F6 RID: 4342
	private bool refreshPlayerCountAutomatically = true;

	// Token: 0x040010F7 RID: 4343
	private int tempSessionPlayerCount;

	// Token: 0x02000261 RID: 609
	// (Invoke) Token: 0x06000E48 RID: 3656
	public delegate void PlayerCountUpdated(int playerCount);
}
