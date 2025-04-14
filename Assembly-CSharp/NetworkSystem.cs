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

// Token: 0x02000287 RID: 647
public abstract class NetworkSystem : MonoBehaviour
{
	// Token: 0x1700019E RID: 414
	// (get) Token: 0x06000F3C RID: 3900 RVA: 0x0004C308 File Offset: 0x0004A508
	// (set) Token: 0x06000F3D RID: 3901 RVA: 0x0004C310 File Offset: 0x0004A510
	public bool groupJoinInProgress { get; protected set; }

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x06000F3E RID: 3902 RVA: 0x0004C319 File Offset: 0x0004A519
	// (set) Token: 0x06000F3F RID: 3903 RVA: 0x0004C321 File Offset: 0x0004A521
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

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06000F40 RID: 3904 RVA: 0x0004C346 File Offset: 0x0004A546
	public NetPlayer LocalPlayer
	{
		get
		{
			return this.netPlayerCache.Find((NetPlayer p) => p.IsLocal);
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06000F41 RID: 3905 RVA: 0x0004C372 File Offset: 0x0004A572
	public virtual bool IsMasterClient { get; }

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x06000F42 RID: 3906 RVA: 0x0004C37A File Offset: 0x0004A57A
	public virtual NetPlayer MasterClient
	{
		get
		{
			return this.netPlayerCache.Find((NetPlayer p) => p.IsMasterClient);
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06000F43 RID: 3907 RVA: 0x0004C3A6 File Offset: 0x0004A5A6
	public Recorder LocalRecorder
	{
		get
		{
			return this.localRecorder;
		}
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06000F44 RID: 3908 RVA: 0x0004C3AE File Offset: 0x0004A5AE
	// (set) Token: 0x06000F45 RID: 3909 RVA: 0x0004C3B6 File Offset: 0x0004A5B6
	public virtual Speaker LocalSpeaker { get; set; }

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x06000F46 RID: 3910 RVA: 0x0004C3C0 File Offset: 0x0004A5C0
	// (remove) Token: 0x06000F47 RID: 3911 RVA: 0x0004C3F8 File Offset: 0x0004A5F8
	public event Action OnJoinedRoomEvent;

	// Token: 0x06000F48 RID: 3912 RVA: 0x0004C42D File Offset: 0x0004A62D
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

	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06000F49 RID: 3913 RVA: 0x0004C44C File Offset: 0x0004A64C
	// (remove) Token: 0x06000F4A RID: 3914 RVA: 0x0004C484 File Offset: 0x0004A684
	public event Action OnMultiplayerStarted;

	// Token: 0x06000F4B RID: 3915 RVA: 0x0004C4B9 File Offset: 0x0004A6B9
	internal void MultiplayerStarted()
	{
		Action onMultiplayerStarted = this.OnMultiplayerStarted;
		if (onMultiplayerStarted == null)
		{
			return;
		}
		onMultiplayerStarted();
	}

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x06000F4C RID: 3916 RVA: 0x0004C4CC File Offset: 0x0004A6CC
	// (remove) Token: 0x06000F4D RID: 3917 RVA: 0x0004C504 File Offset: 0x0004A704
	public event Action OnReturnedToSinglePlayer;

	// Token: 0x06000F4E RID: 3918 RVA: 0x0004C53C File Offset: 0x0004A73C
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

	// Token: 0x1400002B RID: 43
	// (add) Token: 0x06000F4F RID: 3919 RVA: 0x0004C580 File Offset: 0x0004A780
	// (remove) Token: 0x06000F50 RID: 3920 RVA: 0x0004C5B8 File Offset: 0x0004A7B8
	public event Action<NetPlayer> OnPlayerJoined;

	// Token: 0x06000F51 RID: 3921 RVA: 0x0004C5ED File Offset: 0x0004A7ED
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

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x06000F52 RID: 3922 RVA: 0x0004C614 File Offset: 0x0004A814
	// (remove) Token: 0x06000F53 RID: 3923 RVA: 0x0004C64C File Offset: 0x0004A84C
	public event Action<NetPlayer> OnPlayerLeft;

	// Token: 0x06000F54 RID: 3924 RVA: 0x0004C684 File Offset: 0x0004A884
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

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x06000F55 RID: 3925 RVA: 0x0004C6C8 File Offset: 0x0004A8C8
	// (remove) Token: 0x06000F56 RID: 3926 RVA: 0x0004C700 File Offset: 0x0004A900
	internal event Action<NetPlayer> OnMasterClientSwitchedEvent;

	// Token: 0x06000F57 RID: 3927 RVA: 0x0004C735 File Offset: 0x0004A935
	protected void OnMasterClientSwitchedCallback(NetPlayer nMaster)
	{
		Action<NetPlayer> onMasterClientSwitchedEvent = this.OnMasterClientSwitchedEvent;
		if (onMasterClientSwitchedEvent == null)
		{
			return;
		}
		onMasterClientSwitchedEvent(nMaster);
	}

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06000F58 RID: 3928 RVA: 0x0004C748 File Offset: 0x0004A948
	// (remove) Token: 0x06000F59 RID: 3929 RVA: 0x0004C780 File Offset: 0x0004A980
	public event Action<byte, object, int> OnRaiseEvent;

	// Token: 0x06000F5A RID: 3930 RVA: 0x0004C7B5 File Offset: 0x0004A9B5
	internal void RaiseEvent(byte eventCode, object data, int source)
	{
		Action<byte, object, int> onRaiseEvent = this.OnRaiseEvent;
		if (onRaiseEvent == null)
		{
			return;
		}
		onRaiseEvent(eventCode, data, source);
	}

	// Token: 0x1400002F RID: 47
	// (add) Token: 0x06000F5B RID: 3931 RVA: 0x0004C7CC File Offset: 0x0004A9CC
	// (remove) Token: 0x06000F5C RID: 3932 RVA: 0x0004C804 File Offset: 0x0004AA04
	public event Action<Dictionary<string, object>> OnCustomAuthenticationResponse;

	// Token: 0x06000F5D RID: 3933 RVA: 0x0004C839 File Offset: 0x0004AA39
	internal void CustomAuthenticationResponse(Dictionary<string, object> response)
	{
		Action<Dictionary<string, object>> onCustomAuthenticationResponse = this.OnCustomAuthenticationResponse;
		if (onCustomAuthenticationResponse == null)
		{
			return;
		}
		onCustomAuthenticationResponse(response);
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x0004C84C File Offset: 0x0004AA4C
	public virtual void Initialise()
	{
		Debug.Log("INITIALISING NETWORKSYSTEMS");
		if (NetworkSystem.Instance)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		NetworkSystem.Instance = this;
		NetCrossoverUtils.Prewarm();
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void Update()
	{
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x0004C87B File Offset: 0x0004AA7B
	public void RegisterSceneNetworkItem(GameObject item)
	{
		if (!this.SceneObjectsToAttach.Contains(item))
		{
			this.SceneObjectsToAttach.Add(item);
		}
	}

	// Token: 0x06000F61 RID: 3937 RVA: 0x0004C897 File Offset: 0x0004AA97
	public virtual void AttachObjectInGame(GameObject item)
	{
		this.RegisterSceneNetworkItem(item);
	}

	// Token: 0x06000F62 RID: 3938 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void DetatchSceneObjectInGame(GameObject item)
	{
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x0004C8A0 File Offset: 0x0004AAA0
	public virtual AuthenticationValues GetAuthenticationValues()
	{
		Debug.LogWarning("NetworkSystem.GetAuthenticationValues should be overridden");
		return new AuthenticationValues();
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x0004C8B1 File Offset: 0x0004AAB1
	public virtual void SetAuthenticationValues(AuthenticationValues authValues)
	{
		Debug.LogWarning("NetworkSystem.SetAuthenticationValues should be overridden");
	}

	// Token: 0x06000F65 RID: 3941
	public abstract void FinishAuthenticating();

	// Token: 0x06000F66 RID: 3942
	public abstract Task<NetJoinResult> ConnectToRoom(string roomName, RoomConfig opts, int regionIndex = -1);

	// Token: 0x06000F67 RID: 3943
	public abstract Task JoinFriendsRoom(string userID, int actorID, string keyToFollow, string shufflerToFollow);

	// Token: 0x06000F68 RID: 3944
	public abstract Task ReturnToSinglePlayer();

	// Token: 0x06000F69 RID: 3945
	public abstract void JoinPubWithFriends();

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06000F6A RID: 3946 RVA: 0x0004C8BD File Offset: 0x0004AABD
	public bool WrongVersion
	{
		get
		{
			return this.isWrongVersion;
		}
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x0004C8C5 File Offset: 0x0004AAC5
	public void SetWrongVersion()
	{
		this.isWrongVersion = true;
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x0004C8CE File Offset: 0x0004AACE
	public GameObject NetInstantiate(GameObject prefab, bool isRoomObject = false)
	{
		return this.NetInstantiate(prefab, Vector3.zero, Quaternion.identity, false);
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x0004C8E2 File Offset: 0x0004AAE2
	public GameObject NetInstantiate(GameObject prefab, Vector3 position, bool isRoomObject = false)
	{
		return this.NetInstantiate(prefab, position, Quaternion.identity, false);
	}

	// Token: 0x06000F6E RID: 3950
	public abstract GameObject NetInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, bool isRoomObject = false);

	// Token: 0x06000F6F RID: 3951
	public abstract GameObject NetInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, int playerAuthID, bool isRoomObject = false);

	// Token: 0x06000F70 RID: 3952
	public abstract GameObject NetInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, bool isRoomObject, byte group = 0, object[] data = null, NetworkRunner.OnBeforeSpawned callback = null);

	// Token: 0x06000F71 RID: 3953
	public abstract void SetPlayerObject(GameObject playerInstance, int? owningPlayerID = null);

	// Token: 0x06000F72 RID: 3954
	public abstract void NetDestroy(GameObject instance);

	// Token: 0x06000F73 RID: 3955
	public abstract void CallRPC(MonoBehaviour component, NetworkSystem.RPC rpcMethod, bool sendToSelf = true);

	// Token: 0x06000F74 RID: 3956
	public abstract void CallRPC<T>(MonoBehaviour component, NetworkSystem.RPC rpcMethod, RPCArgBuffer<T> args, bool sendToSelf = true) where T : struct;

	// Token: 0x06000F75 RID: 3957
	public abstract void CallRPC(MonoBehaviour component, NetworkSystem.StringRPC rpcMethod, string message, bool sendToSelf = true);

	// Token: 0x06000F76 RID: 3958
	public abstract void CallRPC(int targetPlayerID, MonoBehaviour component, NetworkSystem.RPC rpcMethod);

	// Token: 0x06000F77 RID: 3959
	public abstract void CallRPC<T>(int targetPlayerID, MonoBehaviour component, NetworkSystem.RPC rpcMethod, RPCArgBuffer<T> args) where T : struct;

	// Token: 0x06000F78 RID: 3960
	public abstract void CallRPC(int targetPlayerID, MonoBehaviour component, NetworkSystem.StringRPC rpcMethod, string message);

	// Token: 0x06000F79 RID: 3961 RVA: 0x0004C8F4 File Offset: 0x0004AAF4
	public static string GetRandomRoomName()
	{
		string text = "";
		for (int i = 0; i < 4; i++)
		{
			text += "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".Substring(Random.Range(0, "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".Length), 1);
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

	// Token: 0x06000F7A RID: 3962
	public abstract string GetRandomWeightedRegion();

	// Token: 0x06000F7B RID: 3963 RVA: 0x0004C96C File Offset: 0x0004AB6C
	protected Task RefreshNonce()
	{
		NetworkSystem.<RefreshNonce>d__103 <RefreshNonce>d__;
		<RefreshNonce>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<RefreshNonce>d__.<>4__this = this;
		<RefreshNonce>d__.<>1__state = -1;
		<RefreshNonce>d__.<>t__builder.Start<NetworkSystem.<RefreshNonce>d__103>(ref <RefreshNonce>d__);
		return <RefreshNonce>d__.<>t__builder.Task;
	}

	// Token: 0x06000F7C RID: 3964 RVA: 0x0004C9B0 File Offset: 0x0004ABB0
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

	// Token: 0x06000F7D RID: 3965 RVA: 0x0004C9FA File Offset: 0x0004ABFA
	private void GetSteamAuthTicketFailureCallback(EResult result)
	{
		base.StartCoroutine(this.ReGetNonce());
	}

	// Token: 0x06000F7E RID: 3966 RVA: 0x0004CA09 File Offset: 0x0004AC09
	private IEnumerator ReGetNonce()
	{
		yield return new WaitForSeconds(3f);
		PlayFabAuthenticator.instance.RefreshSteamAuthTicketForPhoton(new Action<string>(this.GetSteamAuthTicketSuccessCallback), new Action<EResult>(this.GetSteamAuthTicketFailureCallback));
		yield return null;
		yield break;
	}

	// Token: 0x06000F7F RID: 3967 RVA: 0x0004CA18 File Offset: 0x0004AC18
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

	// Token: 0x06000F80 RID: 3968 RVA: 0x0004CB30 File Offset: 0x0004AD30
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

	// Token: 0x06000F81 RID: 3969 RVA: 0x0004CBA0 File Offset: 0x0004ADA0
	public NetPlayer GetNetPlayerByID(int playerActorNumber)
	{
		return this.netPlayerCache.Find((NetPlayer a) => a.ActorNumber == playerActorNumber);
	}

	// Token: 0x06000F82 RID: 3970 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void NetRaiseEventReliable(byte eventCode, object data)
	{
	}

	// Token: 0x06000F83 RID: 3971 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void NetRaiseEventUnreliable(byte eventCode, object data)
	{
	}

	// Token: 0x06000F84 RID: 3972 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void NetRaiseEventReliable(byte eventCode, object data, NetEventOptions options)
	{
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void NetRaiseEventUnreliable(byte eventCode, object data, NetEventOptions options)
	{
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x0004CBD4 File Offset: 0x0004ADD4
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

	// Token: 0x06000F87 RID: 3975 RVA: 0x000204AD File Offset: 0x0001E6AD
	public static int mod(int x, int m)
	{
		return (x % m + m) % m;
	}

	// Token: 0x06000F88 RID: 3976
	public abstract Task AwaitSceneReady();

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06000F89 RID: 3977
	public abstract string CurrentPhotonBackend { get; }

	// Token: 0x06000F8A RID: 3978
	public abstract NetPlayer GetLocalPlayer();

	// Token: 0x06000F8B RID: 3979
	public abstract NetPlayer GetPlayer(int PlayerID);

	// Token: 0x06000F8C RID: 3980 RVA: 0x0004CC7C File Offset: 0x0004AE7C
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

	// Token: 0x06000F8D RID: 3981 RVA: 0x0004CCD0 File Offset: 0x0004AED0
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

	// Token: 0x06000F8E RID: 3982 RVA: 0x00042E31 File Offset: 0x00041031
	public NetPlayer GetPlayer(PlayerRef playerRef)
	{
		return null;
	}

	// Token: 0x06000F8F RID: 3983
	public abstract void SetMyNickName(string name);

	// Token: 0x06000F90 RID: 3984
	public abstract string GetMyNickName();

	// Token: 0x06000F91 RID: 3985
	public abstract string GetMyDefaultName();

	// Token: 0x06000F92 RID: 3986
	public abstract string GetNickName(int playerID);

	// Token: 0x06000F93 RID: 3987
	public abstract string GetNickName(NetPlayer player);

	// Token: 0x06000F94 RID: 3988
	public abstract string GetMyUserID();

	// Token: 0x06000F95 RID: 3989
	public abstract string GetUserID(int playerID);

	// Token: 0x06000F96 RID: 3990
	public abstract string GetUserID(NetPlayer player);

	// Token: 0x06000F97 RID: 3991
	public abstract void SetMyTutorialComplete();

	// Token: 0x06000F98 RID: 3992
	public abstract bool GetMyTutorialCompletion();

	// Token: 0x06000F99 RID: 3993
	public abstract bool GetPlayerTutorialCompletion(int playerID);

	// Token: 0x06000F9A RID: 3994 RVA: 0x0004CD15 File Offset: 0x0004AF15
	public void AddVoiceSettings(SO_NetworkVoiceSettings settings)
	{
		this.VoiceSettings = settings;
	}

	// Token: 0x06000F9B RID: 3995
	public abstract void AddRemoteVoiceAddedCallback(Action<RemoteVoiceLink> callback);

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x06000F9C RID: 3996
	public abstract VoiceConnection VoiceConnection { get; }

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x06000F9D RID: 3997
	public abstract bool IsOnline { get; }

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06000F9E RID: 3998
	public abstract bool InRoom { get; }

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x06000F9F RID: 3999
	public abstract string RoomName { get; }

	// Token: 0x06000FA0 RID: 4000
	public abstract string RoomStringStripped();

	// Token: 0x06000FA1 RID: 4001 RVA: 0x0004CD20 File Offset: 0x0004AF20
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

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06000FA2 RID: 4002
	public abstract string GameModeString { get; }

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06000FA3 RID: 4003
	public abstract string CurrentRegion { get; }

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000FA4 RID: 4004
	public abstract bool SessionIsPrivate { get; }

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000FA5 RID: 4005
	public abstract int LocalPlayerID { get; }

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x0004CDB2 File Offset: 0x0004AFB2
	public virtual NetPlayer[] AllNetPlayers
	{
		get
		{
			return this.netPlayerCache.ToArray();
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06000FA7 RID: 4007 RVA: 0x0004CDBF File Offset: 0x0004AFBF
	public virtual NetPlayer[] PlayerListOthers
	{
		get
		{
			return this.netPlayerCache.FindAll((NetPlayer p) => !p.IsLocal).ToArray();
		}
	}

	// Token: 0x06000FA8 RID: 4008
	protected abstract void UpdateNetPlayerList();

	// Token: 0x06000FA9 RID: 4009 RVA: 0x0004CDF0 File Offset: 0x0004AFF0
	public void UpdatePlayers()
	{
		this.UpdateNetPlayerList();
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x06000FAA RID: 4010
	public abstract double SimTime { get; }

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06000FAB RID: 4011
	public abstract float SimDeltaTime { get; }

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06000FAC RID: 4012
	public abstract int SimTick { get; }

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000FAD RID: 4013
	public abstract int TickRate { get; }

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x06000FAE RID: 4014
	public abstract int ServerTimestamp { get; }

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x06000FAF RID: 4015
	public abstract int RoomPlayerCount { get; }

	// Token: 0x06000FB0 RID: 4016
	public abstract int GlobalPlayerCount();

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x0004CDF8 File Offset: 0x0004AFF8
	// (set) Token: 0x06000FB2 RID: 4018 RVA: 0x0004CE00 File Offset: 0x0004B000
	public RoomConfig CurrentRoom { get; protected set; }

	// Token: 0x06000FB3 RID: 4019
	public abstract bool IsObjectLocallyOwned(GameObject obj);

	// Token: 0x06000FB4 RID: 4020
	public abstract bool IsObjectRoomObject(GameObject obj);

	// Token: 0x06000FB5 RID: 4021
	public abstract bool ShouldUpdateObject(GameObject obj);

	// Token: 0x06000FB6 RID: 4022
	public abstract bool ShouldWriteObjectData(GameObject obj);

	// Token: 0x06000FB7 RID: 4023
	public abstract int GetOwningPlayerID(GameObject obj);

	// Token: 0x06000FB8 RID: 4024
	public abstract bool ShouldSpawnLocally(int playerID);

	// Token: 0x06000FB9 RID: 4025
	public abstract bool IsTotalAuthority();

	// Token: 0x040011D2 RID: 4562
	public static NetworkSystem Instance;

	// Token: 0x040011D3 RID: 4563
	public NetworkSystemConfig config;

	// Token: 0x040011D4 RID: 4564
	public bool changingSceneManually;

	// Token: 0x040011D5 RID: 4565
	public string[] regionNames;

	// Token: 0x040011D6 RID: 4566
	public int currentRegionIndex;

	// Token: 0x040011D8 RID: 4568
	private bool nonceRefreshed;

	// Token: 0x040011D9 RID: 4569
	protected bool isWrongVersion;

	// Token: 0x040011DA RID: 4570
	private NetSystemState testState;

	// Token: 0x040011DB RID: 4571
	protected List<NetPlayer> netPlayerCache = new List<NetPlayer>();

	// Token: 0x040011DC RID: 4572
	protected Recorder localRecorder;

	// Token: 0x040011DD RID: 4573
	protected Speaker localSpeaker;

	// Token: 0x040011DF RID: 4575
	public List<GameObject> SceneObjectsToAttach = new List<GameObject>();

	// Token: 0x040011E1 RID: 4577
	protected SO_NetworkVoiceSettings VoiceSettings;

	// Token: 0x040011E2 RID: 4578
	protected List<Action<RemoteVoiceLink>> remoteVoiceAddedCallbacks = new List<Action<RemoteVoiceLink>>();

	// Token: 0x040011EB RID: 4587
	protected static readonly byte[] EmptyArgs = new byte[0];

	// Token: 0x040011EC RID: 4588
	public const string roomCharacters = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";

	// Token: 0x040011ED RID: 4589
	public const string shuffleCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

	// Token: 0x040011EE RID: 4590
	private static StringBuilder shuffleStringBuilder = new StringBuilder(4);

	// Token: 0x040011EF RID: 4591
	protected static StringBuilder reusableSB = new StringBuilder();

	// Token: 0x02000288 RID: 648
	// (Invoke) Token: 0x06000FBD RID: 4029
	public delegate void RPC(byte[] data);

	// Token: 0x02000289 RID: 649
	// (Invoke) Token: 0x06000FC1 RID: 4033
	public delegate void StringRPC(string message);

	// Token: 0x0200028A RID: 650
	// (Invoke) Token: 0x06000FC5 RID: 4037
	public delegate void StaticRPC(byte[] data);

	// Token: 0x0200028B RID: 651
	// (Invoke) Token: 0x06000FC9 RID: 4041
	public delegate void StaticRPCPlaceholder(byte[] args);
}
