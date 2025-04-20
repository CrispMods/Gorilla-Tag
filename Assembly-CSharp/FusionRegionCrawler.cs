using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Fusion;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using UnityEngine;

// Token: 0x0200026B RID: 619
public class FusionRegionCrawler : MonoBehaviour, INetworkRunnerCallbacks
{
	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06000E77 RID: 3703 RVA: 0x0003A40F File Offset: 0x0003860F
	public int PlayerCountGlobal
	{
		get
		{
			return this.globalPlayerCount;
		}
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x0003A417 File Offset: 0x00038617
	public void Start()
	{
		this.regionRunner = base.gameObject.AddComponent<NetworkRunner>();
		this.regionRunner.AddCallbacks(new INetworkRunnerCallbacks[]
		{
			this
		});
		base.StartCoroutine(this.OccasionalUpdate());
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x0003A44C File Offset: 0x0003864C
	public IEnumerator OccasionalUpdate()
	{
		while (this.refreshPlayerCountAutomatically)
		{
			yield return this.UpdatePlayerCount();
			yield return new WaitForSeconds(this.UpdateFrequency);
		}
		yield break;
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x0003A45B File Offset: 0x0003865B
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

	// Token: 0x06000E7B RID: 3707 RVA: 0x0003A46A File Offset: 0x0003866A
	public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
	{
		if (this.waitingForSessionListUpdate)
		{
			this.sessionInfoCache = sessionList;
			this.waitingForSessionListUpdate = false;
		}
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x00030607 File Offset: 0x0002E807
	void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
	{
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x00030607 File Offset: 0x0002E807
	void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
	{
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x00030607 File Offset: 0x0002E807
	void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
	{
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x00030607 File Offset: 0x0002E807
	void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
	{
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x00030607 File Offset: 0x0002E807
	void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
	{
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x00030607 File Offset: 0x0002E807
	void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
	{
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x00030607 File Offset: 0x0002E807
	void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
	{
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x00030607 File Offset: 0x0002E807
	void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
	{
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x00030607 File Offset: 0x0002E807
	void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
	{
	}

	// Token: 0x06000E85 RID: 3717 RVA: 0x00030607 File Offset: 0x0002E807
	void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
	{
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x00030607 File Offset: 0x0002E807
	void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
	{
	}

	// Token: 0x06000E87 RID: 3719 RVA: 0x00030607 File Offset: 0x0002E807
	void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
	{
	}

	// Token: 0x06000E88 RID: 3720 RVA: 0x00030607 File Offset: 0x0002E807
	void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
	{
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x00030607 File Offset: 0x0002E807
	void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner)
	{
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
	{
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
	{
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
	{
	}

	// Token: 0x04001135 RID: 4405
	public FusionRegionCrawler.PlayerCountUpdated OnPlayerCountUpdated;

	// Token: 0x04001136 RID: 4406
	private NetworkRunner regionRunner;

	// Token: 0x04001137 RID: 4407
	private List<SessionInfo> sessionInfoCache;

	// Token: 0x04001138 RID: 4408
	private bool waitingForSessionListUpdate;

	// Token: 0x04001139 RID: 4409
	private int globalPlayerCount;

	// Token: 0x0400113A RID: 4410
	private float UpdateFrequency = 10f;

	// Token: 0x0400113B RID: 4411
	private bool refreshPlayerCountAutomatically = true;

	// Token: 0x0400113C RID: 4412
	private int tempSessionPlayerCount;

	// Token: 0x0200026C RID: 620
	// (Invoke) Token: 0x06000E91 RID: 3729
	public delegate void PlayerCountUpdated(int playerCount);
}
