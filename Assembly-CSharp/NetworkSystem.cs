using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using GorillaNetworking;
using Photon.Realtime;
using Photon.Voice.Unity;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using Steamworks;
using UnityEngine;

// Token: 0x02000292 RID: 658
public abstract class NetworkSystem : MonoBehaviour
{
	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06000F87 RID: 3975 RVA: 0x0003AE1E File Offset: 0x0003901E
	// (set) Token: 0x06000F88 RID: 3976 RVA: 0x0003AE26 File Offset: 0x00039026
	public bool groupJoinInProgress { get; protected set; }

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06000F89 RID: 3977 RVA: 0x0003AE2F File Offset: 0x0003902F
	// (set) Token: 0x06000F8A RID: 3978 RVA: 0x0003AE37 File Offset: 0x00039037
	public NetSystemState netState
	{
		get
		{
			return this.testState;
		}
		protected set
		{
			Debug.Log("netstate set to:" + value.ToString());
			this.testState = value;
		}
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x06000F8B RID: 3979 RVA: 0x0003AE5C File Offset: 0x0003905C
	public NetPlayer LocalPlayer
	{
		get
		{
			return this.netPlayerCache.Find((NetPlayer p) => p.IsLocal);
		}
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x06000F8C RID: 3980 RVA: 0x0003AE88 File Offset: 0x00039088
	public virtual bool IsMasterClient { get; }

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06000F8D RID: 3981 RVA: 0x0003AE90 File Offset: 0x00039090
	public virtual NetPlayer MasterClient
	{
		get
		{
			return this.netPlayerCache.Find((NetPlayer p) => p.IsMasterClient);
		}
	}

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x06000F8E RID: 3982 RVA: 0x0003AEBC File Offset: 0x000390BC
	public Recorder LocalRecorder
	{
		get
		{
			return this.localRecorder;
		}
	}

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06000F8F RID: 3983 RVA: 0x0003AEC4 File Offset: 0x000390C4
	// (set) Token: 0x06000F90 RID: 3984 RVA: 0x0003AECC File Offset: 0x000390CC
	public virtual Speaker LocalSpeaker { get; set; }

	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06000F91 RID: 3985 RVA: 0x000A863C File Offset: 0x000A683C
	// (remove) Token: 0x06000F92 RID: 3986 RVA: 0x000A8674 File Offset: 0x000A6874
	public event Action OnJoinedRoomEvent;

	// Token: 0x06000F93 RID: 3987 RVA: 0x0003AED5 File Offset: 0x000390D5
	protected void JoinedNetworkRoom()
	{
		VRRigCache.Instance.OnJoinedRoom();
		Action onJoinedRoomEvent = this.OnJoinedRoomEvent;
		if (onJoinedRoomEvent == null)
		{
			return;
		}
		onJoinedRoomEvent();
	}

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x06000F94 RID: 3988 RVA: 0x000A86AC File Offset: 0x000A68AC
	// (remove) Token: 0x06000F95 RID: 3989 RVA: 0x000A86E4 File Offset: 0x000A68E4
	public event Action OnMultiplayerStarted;

	// Token: 0x06000F96 RID: 3990 RVA: 0x0003AEF1 File Offset: 0x000390F1
	internal void MultiplayerStarted()
	{
		Action onMultiplayerStarted = this.OnMultiplayerStarted;
		if (onMultiplayerStarted == null)
		{
			return;
		}
		onMultiplayerStarted();
	}

	// Token: 0x1400002B RID: 43
	// (add) Token: 0x06000F97 RID: 3991 RVA: 0x000A871C File Offset: 0x000A691C
	// (remove) Token: 0x06000F98 RID: 3992 RVA: 0x000A8754 File Offset: 0x000A6954
	public event Action OnReturnedToSinglePlayer;

	// Token: 0x06000F99 RID: 3993 RVA: 0x000A878C File Offset: 0x000A698C
	protected void SinglePlayerStarted()
	{
		try
		{
			Action onReturnedToSinglePlayer = this.OnReturnedToSinglePlayer;
			if (onReturnedToSinglePlayer != null)
			{
				onReturnedToSinglePlayer();
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		VRRigCache.Instance.OnLeftRoom();
	}

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x06000F9A RID: 3994 RVA: 0x000A87D0 File Offset: 0x000A69D0
	// (remove) Token: 0x06000F9B RID: 3995 RVA: 0x000A8808 File Offset: 0x000A6A08
	public event Action<NetPlayer> OnPlayerJoined;

	// Token: 0x06000F9C RID: 3996 RVA: 0x0003AF03 File Offset: 0x00039103
	protected void PlayerJoined(NetPlayer netPlayer)
	{
		if (this.IsOnline)
		{
			VRRigCache.Instance.OnPlayerEnteredRoom(netPlayer);
			Action<NetPlayer> onPlayerJoined = this.OnPlayerJoined;
			if (onPlayerJoined == null)
			{
				return;
			}
			onPlayerJoined(netPlayer);
		}
	}

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x06000F9D RID: 3997 RVA: 0x000A8840 File Offset: 0x000A6A40
	// (remove) Token: 0x06000F9E RID: 3998 RVA: 0x000A8878 File Offset: 0x000A6A78
	public event Action<NetPlayer> OnPlayerLeft;

	// Token: 0x06000F9F RID: 3999 RVA: 0x000A88B0 File Offset: 0x000A6AB0
	protected void PlayerLeft(NetPlayer netPlayer)
	{
		try
		{
			Action<NetPlayer> onPlayerLeft = this.OnPlayerLeft;
			if (onPlayerLeft != null)
			{
				onPlayerLeft(netPlayer);
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		VRRigCache.Instance.OnPlayerLeftRoom(netPlayer);
	}

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06000FA0 RID: 4000 RVA: 0x000A88F4 File Offset: 0x000A6AF4
	// (remove) Token: 0x06000FA1 RID: 4001 RVA: 0x000A892C File Offset: 0x000A6B2C
	internal event Action<NetPlayer> OnMasterClientSwitchedEvent;

	// Token: 0x06000FA2 RID: 4002 RVA: 0x0003AF29 File Offset: 0x00039129
	protected void OnMasterClientSwitchedCallback(NetPlayer nMaster)
	{
		Action<NetPlayer> onMasterClientSwitchedEvent = this.OnMasterClientSwitchedEvent;
		if (onMasterClientSwitchedEvent == null)
		{
			return;
		}
		onMasterClientSwitchedEvent(nMaster);
	}

	// Token: 0x1400002F RID: 47
	// (add) Token: 0x06000FA3 RID: 4003 RVA: 0x000A8964 File Offset: 0x000A6B64
	// (remove) Token: 0x06000FA4 RID: 4004 RVA: 0x000A899C File Offset: 0x000A6B9C
	public event Action<byte, object, int> OnRaiseEvent;

	// Token: 0x06000FA5 RID: 4005 RVA: 0x0003AF3C File Offset: 0x0003913C
	internal void RaiseEvent(byte eventCode, object data, int source)
	{
		Action<byte, object, int> onRaiseEvent = this.OnRaiseEvent;
		if (onRaiseEvent == null)
		{
			return;
		}
		onRaiseEvent(eventCode, data, source);
	}

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x06000FA6 RID: 4006 RVA: 0x000A89D4 File Offset: 0x000A6BD4
	// (remove) Token: 0x06000FA7 RID: 4007 RVA: 0x000A8A0C File Offset: 0x000A6C0C
	public event Action<Dictionary<string, object>> OnCustomAuthenticationResponse;

	// Token: 0x06000FA8 RID: 4008 RVA: 0x0003AF51 File Offset: 0x00039151
	internal void CustomAuthenticationResponse(Dictionary<string, object> response)
	{
		Action<Dictionary<string, object>> onCustomAuthenticationResponse = this.OnCustomAuthenticationResponse;
		if (onCustomAuthenticationResponse == null)
		{
			return;
		}
		onCustomAuthenticationResponse(response);
	}

	// Token: 0x06000FA9 RID: 4009 RVA: 0x0003AF64 File Offset: 0x00039164
	public virtual void Initialise()
	{
		Debug.Log("INITIALISING NETWORKSYSTEMS");
		if (NetworkSystem.Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		NetworkSystem.Instance = this;
		NetCrossoverUtils.Prewarm();
	}

	// Token: 0x06000FAA RID: 4010 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void Update()
	{
	}

	// Token: 0x06000FAB RID: 4011 RVA: 0x0003AF93 File Offset: 0x00039193
	public void RegisterSceneNetworkItem(GameObject item)
	{
		if (!this.SceneObjectsToAttach.Contains(item))
		{
			this.SceneObjectsToAttach.Add(item);
		}
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x0003AFAF File Offset: 0x000391AF
	public virtual void AttachObjectInGame(GameObject item)
	{
		this.RegisterSceneNetworkItem(item);
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void DetatchSceneObjectInGame(GameObject item)
	{
	}

	// Token: 0x06000FAE RID: 4014 RVA: 0x0003AFB8 File Offset: 0x000391B8
	public virtual AuthenticationValues GetAuthenticationValues()
	{
		Debug.LogWarning("NetworkSystem.GetAuthenticationValues should be overridden");
		return new AuthenticationValues();
	}

	// Token: 0x06000FAF RID: 4015 RVA: 0x0003AFC9 File Offset: 0x000391C9
	public virtual void SetAuthenticationValues(AuthenticationValues authValues)
	{
		Debug.LogWarning("NetworkSystem.SetAuthenticationValues should be overridden");
	}

	// Token: 0x06000FB0 RID: 4016
	public abstract void FinishAuthenticating();

	// Token: 0x06000FB1 RID: 4017
	public abstract Task<NetJoinResult> ConnectToRoom(string roomName, RoomConfig opts, int regionIndex = -1);

	// Token: 0x06000FB2 RID: 4018
	public abstract Task JoinFriendsRoom(string userID, int actorID, string keyToFollow, string shufflerToFollow);

	// Token: 0x06000FB3 RID: 4019
	public abstract Task ReturnToSinglePlayer();

	// Token: 0x06000FB4 RID: 4020
	public abstract void JoinPubWithFriends();

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x0003AFD5 File Offset: 0x000391D5
	public bool WrongVersion
	{
		get
		{
			return this.isWrongVersion;
		}
	}

	// Token: 0x06000FB6 RID: 4022 RVA: 0x0003AFDD File Offset: 0x000391DD
	public void SetWrongVersion()
	{
		this.isWrongVersion = true;
	}

	// Token: 0x06000FB7 RID: 4023 RVA: 0x0003AFE6 File Offset: 0x000391E6
	public GameObject NetInstantiate(GameObject prefab, bool isRoomObject = false)
	{
		return this.NetInstantiate(prefab, Vector3.zero, Quaternion.identity, false);
	}

	// Token: 0x06000FB8 RID: 4024 RVA: 0x0003AFFA File Offset: 0x000391FA
	public GameObject NetInstantiate(GameObject prefab, Vector3 position, bool isRoomObject = false)
	{
		return this.NetInstantiate(prefab, position, Quaternion.identity, false);
	}

	// Token: 0x06000FB9 RID: 4025
	public abstract GameObject NetInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, bool isRoomObject = false);

	// Token: 0x06000FBA RID: 4026
	public abstract GameObject NetInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, int playerAuthID, bool isRoomObject = false);

	// Token: 0x06000FBB RID: 4027
	public abstract GameObject NetInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, bool isRoomObject, byte group = 0, object[] data = null, NetworkRunner.OnBeforeSpawned callback = null);

	// Token: 0x06000FBC RID: 4028
	public abstract void SetPlayerObject(GameObject playerInstance, int? owningPlayerID = null);

	// Token: 0x06000FBD RID: 4029
	public abstract void NetDestroy(GameObject instance);

	// Token: 0x06000FBE RID: 4030
	public abstract void CallRPC(MonoBehaviour component, NetworkSystem.RPC rpcMethod, bool sendToSelf = true);

	// Token: 0x06000FBF RID: 4031
	public abstract void CallRPC<T>(MonoBehaviour component, NetworkSystem.RPC rpcMethod, RPCArgBuffer<T> args, bool sendToSelf = true) where T : struct;

	// Token: 0x06000FC0 RID: 4032
	public abstract void CallRPC(MonoBehaviour component, NetworkSystem.StringRPC rpcMethod, string message, bool sendToSelf = true);

	// Token: 0x06000FC1 RID: 4033
	public abstract void CallRPC(int targetPlayerID, MonoBehaviour component, NetworkSystem.RPC rpcMethod);

	// Token: 0x06000FC2 RID: 4034
	public abstract void CallRPC<T>(int targetPlayerID, MonoBehaviour component, NetworkSystem.RPC rpcMethod, RPCArgBuffer<T> args) where T : struct;

	// Token: 0x06000FC3 RID: 4035
	public abstract void CallRPC(int targetPlayerID, MonoBehaviour component, NetworkSystem.StringRPC rpcMethod, string message);

	// Token: 0x06000FC4 RID: 4036 RVA: 0x000A8A44 File Offset: 0x000A6C44
	public static string GetRandomRoomName()
	{
		string text = "";
		for (int i = 0; i < 4; i++)
		{
			text += "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".Substring(UnityEngine.Random.Range(0, "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".Length), 1);
		}
		if (GorillaComputer.instance.IsPlayerInVirtualStump())
		{
			text = GorillaComputer.instance.VStumpRoomPrepend + text;
		}
		if (GorillaComputer.instance.CheckAutoBanListForName(text))
		{
			return text;
		}
		return NetworkSystem.GetRandomRoomName();
	}

	// Token: 0x06000FC5 RID: 4037
	public abstract string GetRandomWeightedRegion();

	// Token: 0x06000FC6 RID: 4038 RVA: 0x000A8ABC File Offset: 0x000A6CBC
	protected Task RefreshNonce()
	{
		NetworkSystem.<RefreshNonce>d__103 <RefreshNonce>d__;
		<RefreshNonce>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<RefreshNonce>d__.<>4__this = this;
		<RefreshNonce>d__.<>1__state = -1;
		<RefreshNonce>d__.<>t__builder.Start<NetworkSystem.<RefreshNonce>d__103>(ref <RefreshNonce>d__);
		return <RefreshNonce>d__.<>t__builder.Task;
	}

	// Token: 0x06000FC7 RID: 4039 RVA: 0x000A8B00 File Offset: 0x000A6D00
	private void GetSteamAuthTicketSuccessCallback(string ticket)
	{
		AuthenticationValues authenticationValues = this.GetAuthenticationValues();
		Dictionary<string, object> dictionary = ((authenticationValues != null) ? authenticationValues.AuthPostData : null) as Dictionary<string, object>;
		if (dictionary != null)
		{
			dictionary["Nonce"] = ticket;
			authenticationValues.SetAuthPostData(dictionary);
			this.SetAuthenticationValues(authenticationValues);
			this.nonceRefreshed = true;
		}
	}

	// Token: 0x06000FC8 RID: 4040 RVA: 0x0003B00A File Offset: 0x0003920A
	private void GetSteamAuthTicketFailureCallback(EResult result)
	{
		base.StartCoroutine(this.ReGetNonce());
	}

	// Token: 0x06000FC9 RID: 4041 RVA: 0x0003B019 File Offset: 0x00039219
	private IEnumerator ReGetNonce()
	{
		yield return new WaitForSeconds(3f);
		PlayFabAuthenticator.instance.RefreshSteamAuthTicketForPhoton(new Action<string>(this.GetSteamAuthTicketSuccessCallback), new Action<EResult>(this.GetSteamAuthTicketFailureCallback));
		yield return null;
		yield break;
	}

	// Token: 0x06000FCA RID: 4042 RVA: 0x000A8B4C File Offset: 0x000A6D4C
	public void BroadcastMyRoom(bool create, string key, string shuffler)
	{
		string text = NetworkSystem.ShuffleRoomName(NetworkSystem.Instance.RoomName, shuffler.Substring(2, 8), true) + "|" + NetworkSystem.ShuffleRoomName("ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".Substring(NetworkSystem.Instance.currentRegionIndex, 1), shuffler.Substring(0, 2), true);
		Debug.Log(string.Format("Broadcasting room {0} region {1}({2}). Create: {3} key: {4} (shuffler {5}) shuffled: {6}", new object[]
		{
			NetworkSystem.Instance.RoomName,
			NetworkSystem.Instance.currentRegionIndex,
			NetworkSystem.Instance.regionNames[NetworkSystem.Instance.currentRegionIndex],
			create,
			key,
			shuffler,
			text
		}));
		GorillaServer instance = GorillaServer.Instance;
		BroadcastMyRoomRequest broadcastMyRoomRequest = new BroadcastMyRoomRequest();
		broadcastMyRoomRequest.KeyToFollow = key;
		broadcastMyRoomRequest.RoomToJoin = text;
		broadcastMyRoomRequest.Set = create;
		instance.BroadcastMyRoom(broadcastMyRoomRequest, delegate(ExecuteFunctionResult result)
		{
		}, delegate(PlayFabError error)
		{
		});
	}

	// Token: 0x06000FCB RID: 4043 RVA: 0x000A8C64 File Offset: 0x000A6E64
	public bool InstantCheckGroupData(string userID, string keyToFollow)
	{
		bool success = false;
		PlayFab.ClientModels.GetSharedGroupDataRequest getSharedGroupDataRequest = new PlayFab.ClientModels.GetSharedGroupDataRequest();
		getSharedGroupDataRequest.Keys = new List<string>
		{
			keyToFollow
		};
		getSharedGroupDataRequest.SharedGroupId = userID;
		PlayFabClientAPI.GetSharedGroupData(getSharedGroupDataRequest, delegate(GetSharedGroupDataResult result)
		{
			Debug.Log("Get Shared Group Data returned a success");
			Debug.Log(result.Data.ToStringFull());
			if (result.Data.Count > 0)
			{
				success = true;
				return;
			}
			Debug.Log("RESULT returned but no DATA");
		}, delegate(PlayFabError error)
		{
			Debug.Log("ERROR - no group data found");
		}, null, null);
		return success;
	}

	// Token: 0x06000FCC RID: 4044 RVA: 0x000A8CD4 File Offset: 0x000A6ED4
	public NetPlayer GetNetPlayerByID(int playerActorNumber)
	{
		return this.netPlayerCache.Find((NetPlayer a) => a.ActorNumber == playerActorNumber);
	}

	// Token: 0x06000FCD RID: 4045 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void NetRaiseEventReliable(byte eventCode, object data)
	{
	}

	// Token: 0x06000FCE RID: 4046 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void NetRaiseEventUnreliable(byte eventCode, object data)
	{
	}

	// Token: 0x06000FCF RID: 4047 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void NetRaiseEventReliable(byte eventCode, object data, NetEventOptions options)
	{
	}

	// Token: 0x06000FD0 RID: 4048 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void NetRaiseEventUnreliable(byte eventCode, object data, NetEventOptions options)
	{
	}

	// Token: 0x06000FD1 RID: 4049 RVA: 0x000A8D08 File Offset: 0x000A6F08
	public static string ShuffleRoomName(string room, string shuffle, bool encode)
	{
		NetworkSystem.shuffleStringBuilder.Clear();
		int num;
		if (!int.TryParse(shuffle, out num))
		{
			Debug.Log("Shuffle room failed");
			return "";
		}
		for (int i = 0; i < room.Length; i++)
		{
			int num2 = int.Parse(shuffle.Substring(i * 2 % (shuffle.Length - 1), 2));
			int index = NetworkSystem.mod("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".IndexOf(room[i]) + (encode ? num2 : (-num2)), "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".Length);
			NetworkSystem.shuffleStringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"[index]);
		}
		return NetworkSystem.shuffleStringBuilder.ToString();
	}

	// Token: 0x06000FD2 RID: 4050 RVA: 0x000343C7 File Offset: 0x000325C7
	public static int mod(int x, int m)
	{
		return (x % m + m) % m;
	}

	// Token: 0x06000FD3 RID: 4051
	public abstract Task AwaitSceneReady();

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000FD4 RID: 4052
	public abstract string CurrentPhotonBackend { get; }

	// Token: 0x06000FD5 RID: 4053
	public abstract NetPlayer GetLocalPlayer();

	// Token: 0x06000FD6 RID: 4054
	public abstract NetPlayer GetPlayer(int PlayerID);

	// Token: 0x06000FD7 RID: 4055 RVA: 0x000A8DB0 File Offset: 0x000A6FB0
	public NetPlayer GetPlayer(Player punPlayer)
	{
		if (punPlayer == null)
		{
			return null;
		}
		NetPlayer netPlayer = this.FindPlayer(punPlayer);
		if (netPlayer == null)
		{
			this.UpdatePlayers();
			netPlayer = this.FindPlayer(punPlayer);
			if (netPlayer == null)
			{
				Debug.LogError(string.Format("There is no NetPlayer with this ID currently in game. Passed ID: {0} nickname {1}", punPlayer.ActorNumber, punPlayer.NickName));
				return null;
			}
		}
		return netPlayer;
	}

	// Token: 0x06000FD8 RID: 4056 RVA: 0x000A8E04 File Offset: 0x000A7004
	private NetPlayer FindPlayer(Player punPlayer)
	{
		for (int i = 0; i < this.netPlayerCache.Count; i++)
		{
			if (this.netPlayerCache[i].GetPlayerRef() == punPlayer)
			{
				return this.netPlayerCache[i];
			}
		}
		return null;
	}

	// Token: 0x06000FD9 RID: 4057 RVA: 0x0003924B File Offset: 0x0003744B
	public NetPlayer GetPlayer(PlayerRef playerRef)
	{
		return null;
	}

	// Token: 0x06000FDA RID: 4058
	public abstract void SetMyNickName(string name);

	// Token: 0x06000FDB RID: 4059
	public abstract string GetMyNickName();

	// Token: 0x06000FDC RID: 4060
	public abstract string GetMyDefaultName();

	// Token: 0x06000FDD RID: 4061
	public abstract string GetNickName(int playerID);

	// Token: 0x06000FDE RID: 4062
	public abstract string GetNickName(NetPlayer player);

	// Token: 0x06000FDF RID: 4063
	public abstract string GetMyUserID();

	// Token: 0x06000FE0 RID: 4064
	public abstract string GetUserID(int playerID);

	// Token: 0x06000FE1 RID: 4065
	public abstract string GetUserID(NetPlayer player);

	// Token: 0x06000FE2 RID: 4066
	public abstract void SetMyTutorialComplete();

	// Token: 0x06000FE3 RID: 4067
	public abstract bool GetMyTutorialCompletion();

	// Token: 0x06000FE4 RID: 4068
	public abstract bool GetPlayerTutorialCompletion(int playerID);

	// Token: 0x06000FE5 RID: 4069 RVA: 0x0003B028 File Offset: 0x00039228
	public void AddVoiceSettings(SO_NetworkVoiceSettings settings)
	{
		this.VoiceSettings = settings;
	}

	// Token: 0x06000FE6 RID: 4070
	public abstract void AddRemoteVoiceAddedCallback(Action<RemoteVoiceLink> callback);

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000FE7 RID: 4071
	public abstract VoiceConnection VoiceConnection { get; }

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000FE8 RID: 4072
	public abstract bool IsOnline { get; }

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06000FE9 RID: 4073
	public abstract bool InRoom { get; }

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x06000FEA RID: 4074
	public abstract string RoomName { get; }

	// Token: 0x06000FEB RID: 4075
	public abstract string RoomStringStripped();

	// Token: 0x06000FEC RID: 4076 RVA: 0x000A8E4C File Offset: 0x000A704C
	public string RoomString()
	{
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", new object[]
		{
			this.RoomName,
			this.CurrentRoom.isPublic ? "visible" : "hidden",
			this.CurrentRoom.isJoinable ? "open" : "closed",
			this.CurrentRoom.MaxPlayers,
			this.RoomPlayerCount,
			this.CurrentRoom.CustomProps.ToStringFull()
		});
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06000FED RID: 4077
	public abstract string GameModeString { get; }

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06000FEE RID: 4078
	public abstract string CurrentRegion { get; }

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000FEF RID: 4079
	public abstract bool SessionIsPrivate { get; }

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x06000FF0 RID: 4080
	public abstract int LocalPlayerID { get; }

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x0003B031 File Offset: 0x00039231
	public virtual NetPlayer[] AllNetPlayers
	{
		get
		{
			return this.netPlayerCache.ToArray();
		}
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x0003B03E File Offset: 0x0003923E
	public virtual NetPlayer[] PlayerListOthers
	{
		get
		{
			return this.netPlayerCache.FindAll((NetPlayer p) => !p.IsLocal).ToArray();
		}
	}

	// Token: 0x06000FF3 RID: 4083
	protected abstract void UpdateNetPlayerList();

	// Token: 0x06000FF4 RID: 4084 RVA: 0x0003B06F File Offset: 0x0003926F
	public void UpdatePlayers()
	{
		this.UpdateNetPlayerList();
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x06000FF5 RID: 4085
	public abstract double SimTime { get; }

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x06000FF6 RID: 4086
	public abstract float SimDeltaTime { get; }

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x06000FF7 RID: 4087
	public abstract int SimTick { get; }

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x06000FF8 RID: 4088
	public abstract int TickRate { get; }

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06000FF9 RID: 4089
	public abstract int ServerTimestamp { get; }

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06000FFA RID: 4090
	public abstract int RoomPlayerCount { get; }

	// Token: 0x06000FFB RID: 4091
	public abstract int GlobalPlayerCount();

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06000FFC RID: 4092 RVA: 0x0003B077 File Offset: 0x00039277
	// (set) Token: 0x06000FFD RID: 4093 RVA: 0x0003B07F File Offset: 0x0003927F
	public RoomConfig CurrentRoom { get; protected set; }

	// Token: 0x06000FFE RID: 4094
	public abstract bool IsObjectLocallyOwned(GameObject obj);

	// Token: 0x06000FFF RID: 4095
	public abstract bool IsObjectRoomObject(GameObject obj);

	// Token: 0x06001000 RID: 4096
	public abstract bool ShouldUpdateObject(GameObject obj);

	// Token: 0x06001001 RID: 4097
	public abstract bool ShouldWriteObjectData(GameObject obj);

	// Token: 0x06001002 RID: 4098
	public abstract int GetOwningPlayerID(GameObject obj);

	// Token: 0x06001003 RID: 4099
	public abstract bool ShouldSpawnLocally(int playerID);

	// Token: 0x06001004 RID: 4100
	public abstract bool IsTotalAuthority();

	// Token: 0x0400121A RID: 4634
	public static NetworkSystem Instance;

	// Token: 0x0400121B RID: 4635
	public NetworkSystemConfig config;

	// Token: 0x0400121C RID: 4636
	public bool changingSceneManually;

	// Token: 0x0400121D RID: 4637
	public string[] regionNames;

	// Token: 0x0400121E RID: 4638
	public int currentRegionIndex;

	// Token: 0x04001220 RID: 4640
	private bool nonceRefreshed;

	// Token: 0x04001221 RID: 4641
	protected bool isWrongVersion;

	// Token: 0x04001222 RID: 4642
	private NetSystemState testState;

	// Token: 0x04001223 RID: 4643
	protected List<NetPlayer> netPlayerCache = new List<NetPlayer>();

	// Token: 0x04001224 RID: 4644
	protected Recorder localRecorder;

	// Token: 0x04001225 RID: 4645
	protected Speaker localSpeaker;

	// Token: 0x04001227 RID: 4647
	public List<GameObject> SceneObjectsToAttach = new List<GameObject>();

	// Token: 0x04001229 RID: 4649
	protected SO_NetworkVoiceSettings VoiceSettings;

	// Token: 0x0400122A RID: 4650
	protected List<Action<RemoteVoiceLink>> remoteVoiceAddedCallbacks = new List<Action<RemoteVoiceLink>>();

	// Token: 0x04001233 RID: 4659
	protected static readonly byte[] EmptyArgs = new byte[0];

	// Token: 0x04001234 RID: 4660
	public const string roomCharacters = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";

	// Token: 0x04001235 RID: 4661
	public const string shuffleCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

	// Token: 0x04001236 RID: 4662
	private static StringBuilder shuffleStringBuilder = new StringBuilder(4);

	// Token: 0x04001237 RID: 4663
	protected static StringBuilder reusableSB = new StringBuilder();

	// Token: 0x02000293 RID: 659
	// (Invoke) Token: 0x06001008 RID: 4104
	public delegate void RPC(byte[] data);

	// Token: 0x02000294 RID: 660
	// (Invoke) Token: 0x0600100C RID: 4108
	public delegate void StringRPC(string message);

	// Token: 0x02000295 RID: 661
	// (Invoke) Token: 0x06001010 RID: 4112
	public delegate void StaticRPC(byte[] data);

	// Token: 0x02000296 RID: 662
	// (Invoke) Token: 0x06001014 RID: 4116
	public delegate void StaticRPCPlaceholder(byte[] args);
}
