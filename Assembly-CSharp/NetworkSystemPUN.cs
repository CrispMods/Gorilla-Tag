using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using Fusion;
using GorillaTag;
using GorillaTag.Audio;
using KID.Model;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

// Token: 0x020002A4 RID: 676
[RequireComponent(typeof(PUNCallbackNotifier))]
public class NetworkSystemPUN : NetworkSystem
{
	// Token: 0x170001CD RID: 461
	// (get) Token: 0x0600105E RID: 4190 RVA: 0x0003B416 File Offset: 0x00039616
	public override NetPlayer[] AllNetPlayers
	{
		get
		{
			return this.m_allNetPlayers;
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x0600105F RID: 4191 RVA: 0x0003B41E File Offset: 0x0003961E
	public override NetPlayer[] PlayerListOthers
	{
		get
		{
			return this.m_otherNetPlayers;
		}
	}

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x06001060 RID: 4192 RVA: 0x0003B426 File Offset: 0x00039626
	public override VoiceConnection VoiceConnection
	{
		get
		{
			return this.punVoice;
		}
	}

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x06001061 RID: 4193 RVA: 0x000A94DC File Offset: 0x000A76DC
	private int lowestPingRegionIndex
	{
		get
		{
			int num = 9999;
			int result = -1;
			for (int i = 0; i < this.regionData.Length; i++)
			{
				if (this.regionData[i].pingToRegion < num)
				{
					num = this.regionData[i].pingToRegion;
					result = i;
				}
			}
			return result;
		}
	}

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x06001062 RID: 4194 RVA: 0x0003B42E File Offset: 0x0003962E
	// (set) Token: 0x06001063 RID: 4195 RVA: 0x0003B436 File Offset: 0x00039636
	private NetworkSystemPUN.InternalState internalState
	{
		get
		{
			return this.currentState;
		}
		set
		{
			this.currentState = value;
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x06001064 RID: 4196 RVA: 0x0003B43F File Offset: 0x0003963F
	public override string CurrentPhotonBackend
	{
		get
		{
			return "PUN";
		}
	}

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x06001065 RID: 4197 RVA: 0x0003B446 File Offset: 0x00039646
	public override bool IsOnline
	{
		get
		{
			return this.InRoom;
		}
	}

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x06001066 RID: 4198 RVA: 0x0003B44E File Offset: 0x0003964E
	public override bool InRoom
	{
		get
		{
			return PhotonNetwork.InRoom;
		}
	}

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x06001067 RID: 4199 RVA: 0x0003B455 File Offset: 0x00039655
	public override string RoomName
	{
		get
		{
			Room currentRoom = PhotonNetwork.CurrentRoom;
			return ((currentRoom != null) ? currentRoom.Name : null) ?? string.Empty;
		}
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x000A9528 File Offset: 0x000A7728
	public override string RoomStringStripped()
	{
		Room currentRoom = PhotonNetwork.CurrentRoom;
		NetworkSystem.reusableSB.Clear();
		NetworkSystem.reusableSB.AppendFormat("Room: '{0}' ", (currentRoom.Name.Length < 20) ? currentRoom.Name : currentRoom.Name.Remove(20));
		NetworkSystem.reusableSB.AppendFormat("{0},{1} {3}/{2} players.", new object[]
		{
			currentRoom.IsVisible ? "visible" : "hidden",
			currentRoom.IsOpen ? "open" : "closed",
			currentRoom.MaxPlayers,
			currentRoom.PlayerCount
		});
		NetworkSystem.reusableSB.Append("\ncustomProps: {");
		NetworkSystem.reusableSB.AppendFormat("joinedGameMode={0}, ", (RoomSystem.RoomGameMode.Length < 50) ? RoomSystem.RoomGameMode : RoomSystem.RoomGameMode.Remove(50));
		IDictionary customProperties = currentRoom.CustomProperties;
		if (customProperties.Contains("gameMode"))
		{
			object obj = customProperties["gameMode"];
			if (obj == null)
			{
				NetworkSystem.reusableSB.AppendFormat("gameMode=null}", Array.Empty<object>());
			}
			else
			{
				string text = obj as string;
				if (text != null)
				{
					NetworkSystem.reusableSB.AppendFormat("gameMode={0}", (text.Length < 50) ? text : text.Remove(50));
				}
			}
		}
		NetworkSystem.reusableSB.Append("}");
		Debug.Log(NetworkSystem.reusableSB.ToString());
		return NetworkSystem.reusableSB.ToString();
	}

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x06001069 RID: 4201 RVA: 0x000A96B0 File Offset: 0x000A78B0
	public override string GameModeString
	{
		get
		{
			object obj;
			PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gameMode", out obj);
			if (obj != null)
			{
				return obj.ToString();
			}
			return null;
		}
	}

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x0600106A RID: 4202 RVA: 0x0003B471 File Offset: 0x00039671
	public override string CurrentRegion
	{
		get
		{
			return PhotonNetwork.CloudRegion;
		}
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x0600106B RID: 4203 RVA: 0x0003B478 File Offset: 0x00039678
	public override bool SessionIsPrivate
	{
		get
		{
			Room currentRoom = PhotonNetwork.CurrentRoom;
			return currentRoom != null && !currentRoom.IsVisible;
		}
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x0600106C RID: 4204 RVA: 0x0003B48D File Offset: 0x0003968D
	public override int LocalPlayerID
	{
		get
		{
			return PhotonNetwork.LocalPlayer.ActorNumber;
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x0600106D RID: 4205 RVA: 0x0003B499 File Offset: 0x00039699
	public override int ServerTimestamp
	{
		get
		{
			return PhotonNetwork.ServerTimestamp;
		}
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x0600106E RID: 4206 RVA: 0x0003B4A0 File Offset: 0x000396A0
	public override double SimTime
	{
		get
		{
			return PhotonNetwork.Time;
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x0600106F RID: 4207 RVA: 0x0003B4A7 File Offset: 0x000396A7
	public override float SimDeltaTime
	{
		get
		{
			return Time.deltaTime;
		}
	}

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x06001070 RID: 4208 RVA: 0x0003B499 File Offset: 0x00039699
	public override int SimTick
	{
		get
		{
			return PhotonNetwork.ServerTimestamp;
		}
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06001071 RID: 4209 RVA: 0x0003B4AE File Offset: 0x000396AE
	public override int TickRate
	{
		get
		{
			return PhotonNetwork.SerializationRate;
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x06001072 RID: 4210 RVA: 0x0003B4B5 File Offset: 0x000396B5
	public override int RoomPlayerCount
	{
		get
		{
			return (int)PhotonNetwork.CurrentRoom.PlayerCount;
		}
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x06001073 RID: 4211 RVA: 0x0003B4C1 File Offset: 0x000396C1
	public override bool IsMasterClient
	{
		get
		{
			return PhotonNetwork.IsMasterClient;
		}
	}

	// Token: 0x06001074 RID: 4212 RVA: 0x000A96E0 File Offset: 0x000A78E0
	public override void Initialise()
	{
		NetworkSystemPUN.<Initialise>d__53 <Initialise>d__;
		<Initialise>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Initialise>d__.<>4__this = this;
		<Initialise>d__.<>1__state = -1;
		<Initialise>d__.<>t__builder.Start<NetworkSystemPUN.<Initialise>d__53>(ref <Initialise>d__);
	}

	// Token: 0x06001075 RID: 4213 RVA: 0x000A9718 File Offset: 0x000A7918
	private Task CacheRegionInfo()
	{
		NetworkSystemPUN.<CacheRegionInfo>d__54 <CacheRegionInfo>d__;
		<CacheRegionInfo>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<CacheRegionInfo>d__.<>4__this = this;
		<CacheRegionInfo>d__.<>1__state = -1;
		<CacheRegionInfo>d__.<>t__builder.Start<NetworkSystemPUN.<CacheRegionInfo>d__54>(ref <CacheRegionInfo>d__);
		return <CacheRegionInfo>d__.<>t__builder.Task;
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x0003B4C8 File Offset: 0x000396C8
	public override AuthenticationValues GetAuthenticationValues()
	{
		return PhotonNetwork.AuthValues;
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x0003B4CF File Offset: 0x000396CF
	public override void SetAuthenticationValues(AuthenticationValues authValues)
	{
		PhotonNetwork.AuthValues = authValues;
	}

	// Token: 0x06001078 RID: 4216 RVA: 0x0003B4D7 File Offset: 0x000396D7
	public override void FinishAuthenticating()
	{
		this.internalState = NetworkSystemPUN.InternalState.Authenticated;
	}

	// Token: 0x06001079 RID: 4217 RVA: 0x000A975C File Offset: 0x000A795C
	private Task WaitForState(CancellationToken ct, params NetworkSystemPUN.InternalState[] desiredStates)
	{
		NetworkSystemPUN.<WaitForState>d__58 <WaitForState>d__;
		<WaitForState>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<WaitForState>d__.<>4__this = this;
		<WaitForState>d__.ct = ct;
		<WaitForState>d__.desiredStates = desiredStates;
		<WaitForState>d__.<>1__state = -1;
		<WaitForState>d__.<>t__builder.Start<NetworkSystemPUN.<WaitForState>d__58>(ref <WaitForState>d__);
		return <WaitForState>d__.<>t__builder.Task;
	}

	// Token: 0x0600107A RID: 4218 RVA: 0x000A97B0 File Offset: 0x000A79B0
	private Task<bool> WaitForStateCheck(params NetworkSystemPUN.InternalState[] desiredStates)
	{
		NetworkSystemPUN.<WaitForStateCheck>d__59 <WaitForStateCheck>d__;
		<WaitForStateCheck>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<WaitForStateCheck>d__.<>4__this = this;
		<WaitForStateCheck>d__.desiredStates = desiredStates;
		<WaitForStateCheck>d__.<>1__state = -1;
		<WaitForStateCheck>d__.<>t__builder.Start<NetworkSystemPUN.<WaitForStateCheck>d__59>(ref <WaitForStateCheck>d__);
		return <WaitForStateCheck>d__.<>t__builder.Task;
	}

	// Token: 0x0600107B RID: 4219 RVA: 0x000A97FC File Offset: 0x000A79FC
	private Task<NetJoinResult> MakeOrFindRoom(string roomName, RoomConfig opts, int regionIndex = -1)
	{
		NetworkSystemPUN.<MakeOrFindRoom>d__60 <MakeOrFindRoom>d__;
		<MakeOrFindRoom>d__.<>t__builder = AsyncTaskMethodBuilder<NetJoinResult>.Create();
		<MakeOrFindRoom>d__.<>4__this = this;
		<MakeOrFindRoom>d__.roomName = roomName;
		<MakeOrFindRoom>d__.opts = opts;
		<MakeOrFindRoom>d__.regionIndex = regionIndex;
		<MakeOrFindRoom>d__.<>1__state = -1;
		<MakeOrFindRoom>d__.<>t__builder.Start<NetworkSystemPUN.<MakeOrFindRoom>d__60>(ref <MakeOrFindRoom>d__);
		return <MakeOrFindRoom>d__.<>t__builder.Task;
	}

	// Token: 0x0600107C RID: 4220 RVA: 0x000A9858 File Offset: 0x000A7A58
	private Task<bool> TryJoinRoom(string roomName, RoomConfig opts)
	{
		NetworkSystemPUN.<TryJoinRoom>d__61 <TryJoinRoom>d__;
		<TryJoinRoom>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<TryJoinRoom>d__.<>4__this = this;
		<TryJoinRoom>d__.roomName = roomName;
		<TryJoinRoom>d__.opts = opts;
		<TryJoinRoom>d__.<>1__state = -1;
		<TryJoinRoom>d__.<>t__builder.Start<NetworkSystemPUN.<TryJoinRoom>d__61>(ref <TryJoinRoom>d__);
		return <TryJoinRoom>d__.<>t__builder.Task;
	}

	// Token: 0x0600107D RID: 4221 RVA: 0x000A98AC File Offset: 0x000A7AAC
	private Task<bool> TryJoinRoomInRegion(string roomName, RoomConfig opts, int regionIndex)
	{
		NetworkSystemPUN.<TryJoinRoomInRegion>d__62 <TryJoinRoomInRegion>d__;
		<TryJoinRoomInRegion>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<TryJoinRoomInRegion>d__.<>4__this = this;
		<TryJoinRoomInRegion>d__.roomName = roomName;
		<TryJoinRoomInRegion>d__.opts = opts;
		<TryJoinRoomInRegion>d__.regionIndex = regionIndex;
		<TryJoinRoomInRegion>d__.<>1__state = -1;
		<TryJoinRoomInRegion>d__.<>t__builder.Start<NetworkSystemPUN.<TryJoinRoomInRegion>d__62>(ref <TryJoinRoomInRegion>d__);
		return <TryJoinRoomInRegion>d__.<>t__builder.Task;
	}

	// Token: 0x0600107E RID: 4222 RVA: 0x000A9908 File Offset: 0x000A7B08
	private Task<NetJoinResult> TryCreateRoom(string roomName, RoomConfig opts)
	{
		NetworkSystemPUN.<TryCreateRoom>d__63 <TryCreateRoom>d__;
		<TryCreateRoom>d__.<>t__builder = AsyncTaskMethodBuilder<NetJoinResult>.Create();
		<TryCreateRoom>d__.<>4__this = this;
		<TryCreateRoom>d__.roomName = roomName;
		<TryCreateRoom>d__.opts = opts;
		<TryCreateRoom>d__.<>1__state = -1;
		<TryCreateRoom>d__.<>t__builder.Start<NetworkSystemPUN.<TryCreateRoom>d__63>(ref <TryCreateRoom>d__);
		return <TryCreateRoom>d__.<>t__builder.Task;
	}

	// Token: 0x0600107F RID: 4223 RVA: 0x000A995C File Offset: 0x000A7B5C
	private Task<NetJoinResult> JoinRandomPublicRoom(RoomConfig opts)
	{
		NetworkSystemPUN.<JoinRandomPublicRoom>d__64 <JoinRandomPublicRoom>d__;
		<JoinRandomPublicRoom>d__.<>t__builder = AsyncTaskMethodBuilder<NetJoinResult>.Create();
		<JoinRandomPublicRoom>d__.<>4__this = this;
		<JoinRandomPublicRoom>d__.opts = opts;
		<JoinRandomPublicRoom>d__.<>1__state = -1;
		<JoinRandomPublicRoom>d__.<>t__builder.Start<NetworkSystemPUN.<JoinRandomPublicRoom>d__64>(ref <JoinRandomPublicRoom>d__);
		return <JoinRandomPublicRoom>d__.<>t__builder.Task;
	}

	// Token: 0x06001080 RID: 4224 RVA: 0x000A99A8 File Offset: 0x000A7BA8
	public override Task<NetJoinResult> ConnectToRoom(string roomName, RoomConfig opts, int regionIndex = -1)
	{
		NetworkSystemPUN.<ConnectToRoom>d__65 <ConnectToRoom>d__;
		<ConnectToRoom>d__.<>t__builder = AsyncTaskMethodBuilder<NetJoinResult>.Create();
		<ConnectToRoom>d__.<>4__this = this;
		<ConnectToRoom>d__.roomName = roomName;
		<ConnectToRoom>d__.opts = opts;
		<ConnectToRoom>d__.regionIndex = regionIndex;
		<ConnectToRoom>d__.<>1__state = -1;
		<ConnectToRoom>d__.<>t__builder.Start<NetworkSystemPUN.<ConnectToRoom>d__65>(ref <ConnectToRoom>d__);
		return <ConnectToRoom>d__.<>t__builder.Task;
	}

	// Token: 0x06001081 RID: 4225 RVA: 0x000A9A04 File Offset: 0x000A7C04
	public override Task JoinFriendsRoom(string userID, int actorIDToFollow, string keyToFollow, string shufflerToFollow)
	{
		NetworkSystemPUN.<JoinFriendsRoom>d__66 <JoinFriendsRoom>d__;
		<JoinFriendsRoom>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<JoinFriendsRoom>d__.<>4__this = this;
		<JoinFriendsRoom>d__.userID = userID;
		<JoinFriendsRoom>d__.actorIDToFollow = actorIDToFollow;
		<JoinFriendsRoom>d__.keyToFollow = keyToFollow;
		<JoinFriendsRoom>d__.shufflerToFollow = shufflerToFollow;
		<JoinFriendsRoom>d__.<>1__state = -1;
		<JoinFriendsRoom>d__.<>t__builder.Start<NetworkSystemPUN.<JoinFriendsRoom>d__66>(ref <JoinFriendsRoom>d__);
		return <JoinFriendsRoom>d__.<>t__builder.Task;
	}

	// Token: 0x06001082 RID: 4226 RVA: 0x000306DC File Offset: 0x0002E8DC
	public override void JoinPubWithFriends()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06001083 RID: 4227 RVA: 0x000A9A68 File Offset: 0x000A7C68
	public override string GetRandomWeightedRegion()
	{
		float value = UnityEngine.Random.value;
		int num = 0;
		for (int i = 0; i < this.regionData.Length; i++)
		{
			num += this.regionData[i].playersInRegion;
		}
		float num2 = 0f;
		int num3 = -1;
		while (num2 < value && num3 < this.regionData.Length - 1)
		{
			num3++;
			num2 += (float)this.regionData[num3].playersInRegion / (float)num;
		}
		return this.regionNames[num3];
	}

	// Token: 0x06001084 RID: 4228 RVA: 0x000A9AE0 File Offset: 0x000A7CE0
	public override Task ReturnToSinglePlayer()
	{
		NetworkSystemPUN.<ReturnToSinglePlayer>d__69 <ReturnToSinglePlayer>d__;
		<ReturnToSinglePlayer>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ReturnToSinglePlayer>d__.<>4__this = this;
		<ReturnToSinglePlayer>d__.<>1__state = -1;
		<ReturnToSinglePlayer>d__.<>t__builder.Start<NetworkSystemPUN.<ReturnToSinglePlayer>d__69>(ref <ReturnToSinglePlayer>d__);
		return <ReturnToSinglePlayer>d__.<>t__builder.Task;
	}

	// Token: 0x06001085 RID: 4229 RVA: 0x000A9B24 File Offset: 0x000A7D24
	private Task InternalDisconnect()
	{
		NetworkSystemPUN.<InternalDisconnect>d__70 <InternalDisconnect>d__;
		<InternalDisconnect>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<InternalDisconnect>d__.<>4__this = this;
		<InternalDisconnect>d__.<>1__state = -1;
		<InternalDisconnect>d__.<>t__builder.Start<NetworkSystemPUN.<InternalDisconnect>d__70>(ref <InternalDisconnect>d__);
		return <InternalDisconnect>d__.<>t__builder.Task;
	}

	// Token: 0x06001086 RID: 4230 RVA: 0x0003B4E0 File Offset: 0x000396E0
	private void AddVoice()
	{
		this.SetupVoice();
	}

	// Token: 0x06001087 RID: 4231 RVA: 0x000A9B68 File Offset: 0x000A7D68
	private void SetupVoice()
	{
		this.punVoice = PhotonVoiceNetwork.Instance;
		this.VoiceNetworkObject = this.punVoice.gameObject;
		this.VoiceNetworkObject.name = "VoiceNetworkObject";
		this.VoiceNetworkObject.transform.parent = base.transform;
		this.VoiceNetworkObject.transform.localPosition = Vector3.zero;
		this.punVoice.LogLevel = this.VoiceSettings.LogLevel;
		this.punVoice.GlobalRecordersLogLevel = this.VoiceSettings.GlobalRecordersLogLevel;
		this.punVoice.GlobalSpeakersLogLevel = this.VoiceSettings.GlobalSpeakersLogLevel;
		this.punVoice.AutoConnectAndJoin = this.VoiceSettings.AutoConnectAndJoin;
		this.punVoice.AutoLeaveAndDisconnect = this.VoiceSettings.AutoLeaveAndDisconnect;
		this.punVoice.WorkInOfflineMode = this.VoiceSettings.WorkInOfflineMode;
		this.punVoice.AutoCreateSpeakerIfNotFound = this.VoiceSettings.CreateSpeakerIfNotFound;
		AppSettings appSettings = new AppSettings();
		appSettings.AppIdRealtime = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
		appSettings.AppIdVoice = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice;
		this.punVoice.Settings = appSettings;
		this.remoteVoiceAddedCallbacks.ForEach(delegate(Action<RemoteVoiceLink> callback)
		{
			this.punVoice.RemoteVoiceAdded += callback;
		});
		this.localRecorder = this.VoiceNetworkObject.GetComponent<Recorder>();
		if (this.localRecorder == null)
		{
			this.localRecorder = this.VoiceNetworkObject.AddComponent<Recorder>();
		}
		this.localRecorder.LogLevel = this.VoiceSettings.LogLevel;
		this.localRecorder.RecordOnlyWhenEnabled = this.VoiceSettings.RecordOnlyWhenEnabled;
		this.localRecorder.RecordOnlyWhenJoined = this.VoiceSettings.RecordOnlyWhenJoined;
		this.localRecorder.StopRecordingWhenPaused = this.VoiceSettings.StopRecordingWhenPaused;
		this.localRecorder.TransmitEnabled = this.VoiceSettings.TransmitEnabled;
		this.localRecorder.AutoStart = this.VoiceSettings.AutoStart;
		this.localRecorder.Encrypt = this.VoiceSettings.Encrypt;
		this.localRecorder.FrameDuration = this.VoiceSettings.FrameDuration;
		this.localRecorder.SamplingRate = this.VoiceSettings.SamplingRate;
		this.localRecorder.InterestGroup = this.VoiceSettings.InterestGroup;
		this.localRecorder.SourceType = this.VoiceSettings.InputSourceType;
		this.localRecorder.MicrophoneType = this.VoiceSettings.MicrophoneType;
		this.localRecorder.UseMicrophoneTypeFallback = this.VoiceSettings.UseFallback;
		this.localRecorder.VoiceDetection = this.VoiceSettings.Detect;
		this.localRecorder.VoiceDetectionThreshold = this.VoiceSettings.Threshold;
		this.localRecorder.Bitrate = this.VoiceSettings.Bitrate;
		this.localRecorder.VoiceDetectionDelayMs = this.VoiceSettings.Delay;
		this.localRecorder.DebugEchoMode = this.VoiceSettings.DebugEcho;
		this.punVoice.PrimaryRecorder = this.localRecorder;
		this.VoiceNetworkObject.AddComponent<VoiceToLoudness>();
	}

	// Token: 0x06001088 RID: 4232 RVA: 0x0003A631 File Offset: 0x00038831
	public override void AddRemoteVoiceAddedCallback(Action<RemoteVoiceLink> callback)
	{
		this.remoteVoiceAddedCallbacks.Add(callback);
	}

	// Token: 0x06001089 RID: 4233 RVA: 0x0003B4E8 File Offset: 0x000396E8
	public override GameObject NetInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, bool isRoomObject = false)
	{
		if (PhotonNetwork.CurrentRoom == null)
		{
			return UnityEngine.Object.Instantiate<GameObject>(prefab, position, rotation);
		}
		if (isRoomObject)
		{
			return PhotonNetwork.InstantiateRoomObject(prefab.name, position, rotation, 0, null);
		}
		return PhotonNetwork.Instantiate(prefab.name, position, rotation, 0, null);
	}

	// Token: 0x0600108A RID: 4234 RVA: 0x0003B51D File Offset: 0x0003971D
	public override GameObject NetInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, int playerAuthID, bool isRoomObject = false)
	{
		return this.NetInstantiate(prefab, position, rotation, isRoomObject);
	}

	// Token: 0x0600108B RID: 4235 RVA: 0x0003B52A File Offset: 0x0003972A
	public override GameObject NetInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, bool isRoomObject, byte group = 0, object[] data = null, NetworkRunner.OnBeforeSpawned callback = null)
	{
		if (PhotonNetwork.CurrentRoom == null)
		{
			return UnityEngine.Object.Instantiate<GameObject>(prefab, position, rotation);
		}
		if (isRoomObject)
		{
			return PhotonNetwork.InstantiateRoomObject(prefab.name, position, rotation, group, data);
		}
		return PhotonNetwork.Instantiate(prefab.name, position, rotation, group, data);
	}

	// Token: 0x0600108C RID: 4236 RVA: 0x000A9E94 File Offset: 0x000A8094
	public override void NetDestroy(GameObject instance)
	{
		PhotonView photonView;
		if (instance.TryGetComponent<PhotonView>(out photonView) && photonView.AmOwner)
		{
			PhotonNetwork.Destroy(instance);
			return;
		}
		UnityEngine.Object.Destroy(instance);
	}

	// Token: 0x0600108D RID: 4237 RVA: 0x00030607 File Offset: 0x0002E807
	public override void SetPlayerObject(GameObject playerInstance, int? owningPlayerID = null)
	{
	}

	// Token: 0x0600108E RID: 4238 RVA: 0x000A9EC0 File Offset: 0x000A80C0
	public override void CallRPC(MonoBehaviour component, NetworkSystem.RPC rpcMethod, bool sendToSelf = true)
	{
		RpcTarget target = sendToSelf ? RpcTarget.All : RpcTarget.Others;
		PhotonView.Get(component).RPC(rpcMethod.Method.Name, target, new object[]
		{
			NetworkSystem.EmptyArgs
		});
	}

	// Token: 0x0600108F RID: 4239 RVA: 0x000A9EFC File Offset: 0x000A80FC
	public override void CallRPC<T>(MonoBehaviour component, NetworkSystem.RPC rpcMethod, RPCArgBuffer<T> args, bool sendToSelf = true)
	{
		RpcTarget target = sendToSelf ? RpcTarget.All : RpcTarget.Others;
		ref args.SerializeToRPCData<T>();
		PhotonView.Get(component).RPC(rpcMethod.Method.Name, target, new object[]
		{
			args.Data
		});
	}

	// Token: 0x06001090 RID: 4240 RVA: 0x000A9F40 File Offset: 0x000A8140
	public override void CallRPC(MonoBehaviour component, NetworkSystem.StringRPC rpcMethod, string message, bool sendToSelf = true)
	{
		RpcTarget target = sendToSelf ? RpcTarget.All : RpcTarget.Others;
		PhotonView.Get(component).RPC(rpcMethod.Method.Name, target, new object[]
		{
			message
		});
	}

	// Token: 0x06001091 RID: 4241 RVA: 0x000A9F78 File Offset: 0x000A8178
	public override void CallRPC(int targetPlayerID, MonoBehaviour component, NetworkSystem.RPC rpcMethod)
	{
		Player player = PhotonNetwork.CurrentRoom.GetPlayer(targetPlayerID, false);
		PhotonView.Get(component).RPC(rpcMethod.Method.Name, player, new object[]
		{
			NetworkSystem.EmptyArgs
		});
	}

	// Token: 0x06001092 RID: 4242 RVA: 0x000A9FB8 File Offset: 0x000A81B8
	public override void CallRPC<T>(int targetPlayerID, MonoBehaviour component, NetworkSystem.RPC rpcMethod, RPCArgBuffer<T> args)
	{
		Player player = PhotonNetwork.CurrentRoom.GetPlayer(targetPlayerID, false);
		ref args.SerializeToRPCData<T>();
		PhotonView.Get(component).RPC(rpcMethod.Method.Name, player, new object[]
		{
			args.Data
		});
	}

	// Token: 0x06001093 RID: 4243 RVA: 0x000AA000 File Offset: 0x000A8200
	public override void CallRPC(int targetPlayerID, MonoBehaviour component, NetworkSystem.StringRPC rpcMethod, string message)
	{
		Player player = PhotonNetwork.CurrentRoom.GetPlayer(targetPlayerID, false);
		PhotonView.Get(component).RPC(rpcMethod.Method.Name, player, new object[]
		{
			message
		});
	}

	// Token: 0x06001094 RID: 4244 RVA: 0x000AA03C File Offset: 0x000A823C
	public override Task AwaitSceneReady()
	{
		NetworkSystemPUN.<AwaitSceneReady>d__85 <AwaitSceneReady>d__;
		<AwaitSceneReady>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AwaitSceneReady>d__.<>1__state = -1;
		<AwaitSceneReady>d__.<>t__builder.Start<NetworkSystemPUN.<AwaitSceneReady>d__85>(ref <AwaitSceneReady>d__);
		return <AwaitSceneReady>d__.<>t__builder.Task;
	}

	// Token: 0x06001095 RID: 4245 RVA: 0x000AA078 File Offset: 0x000A8278
	public override NetPlayer GetLocalPlayer()
	{
		if (this.netPlayerCache.Count == 0)
		{
			base.UpdatePlayers();
		}
		foreach (NetPlayer netPlayer in this.netPlayerCache)
		{
			if (netPlayer.IsLocal)
			{
				return netPlayer;
			}
		}
		Debug.LogError("Somehow no local net players found. This shouldn't happen");
		return null;
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x000AA0F0 File Offset: 0x000A82F0
	public override NetPlayer GetPlayer(int PlayerID)
	{
		if (this.InRoom && !PhotonNetwork.CurrentRoom.Players.ContainsKey(PlayerID))
		{
			return null;
		}
		foreach (NetPlayer netPlayer in this.netPlayerCache)
		{
			if (netPlayer.ActorNumber == PlayerID)
			{
				return netPlayer;
			}
		}
		base.UpdatePlayers();
		foreach (NetPlayer netPlayer2 in this.netPlayerCache)
		{
			if (netPlayer2.ActorNumber == PlayerID)
			{
				return netPlayer2;
			}
		}
		GTDev.LogWarning<string>("There is no NetPlayer with this ID currently in game. Passed ID: " + PlayerID.ToString(), null);
		return null;
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x000AA1D0 File Offset: 0x000A83D0
	public override void SetMyNickName(string id)
	{
		ValueTuple<bool, Permission.ManagedByEnum> customNicknamePermissionStatus = KIDManager.Instance.GetCustomNicknamePermissionStatus();
		if (((!customNicknamePermissionStatus.Item1 && customNicknamePermissionStatus.Item2 != Permission.ManagedByEnum.PLAYER) || customNicknamePermissionStatus.Item2 == Permission.ManagedByEnum.PROHIBITED) && !id.StartsWith("gorilla"))
		{
			Debug.Log("[KID] Trying to set custom nickname but that permission has been disallowed");
			PhotonNetwork.LocalPlayer.NickName = "gorilla";
			return;
		}
		PlayerPrefs.SetString("playerName", id);
		PhotonNetwork.LocalPlayer.NickName = id;
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x0003B563 File Offset: 0x00039763
	public override string GetMyNickName()
	{
		return PhotonNetwork.LocalPlayer.NickName;
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x0003B56F File Offset: 0x0003976F
	public override string GetMyDefaultName()
	{
		return PhotonNetwork.LocalPlayer.DefaultName;
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x000AA248 File Offset: 0x000A8448
	public override string GetNickName(int playerID)
	{
		NetPlayer player = this.GetPlayer(playerID);
		if (player != null)
		{
			return player.NickName;
		}
		return null;
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x0003B57B File Offset: 0x0003977B
	public override string GetNickName(NetPlayer player)
	{
		return player.NickName;
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x000AA268 File Offset: 0x000A8468
	public override void SetMyTutorialComplete()
	{
		bool flag = PlayerPrefs.GetString("didTutorial", "nope") == "done";
		if (!flag)
		{
			PlayerPrefs.SetString("didTutorial", "done");
			PlayerPrefs.Save();
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add("didTutorial", flag);
		PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable, null, null);
	}

	// Token: 0x0600109D RID: 4253 RVA: 0x0003A7E7 File Offset: 0x000389E7
	public override bool GetMyTutorialCompletion()
	{
		return PlayerPrefs.GetString("didTutorial", "nope") == "done";
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x000AA2CC File Offset: 0x000A84CC
	public override bool GetPlayerTutorialCompletion(int playerID)
	{
		NetPlayer player = this.GetPlayer(playerID);
		if (player == null)
		{
			return false;
		}
		Player player2 = PhotonNetwork.CurrentRoom.GetPlayer(player.ActorNumber, false);
		if (player2 == null)
		{
			return false;
		}
		object obj;
		if (player2.CustomProperties.TryGetValue("didTutorial", out obj))
		{
			bool flag;
			bool flag2;
			if (obj is bool)
			{
				flag = (bool)obj;
				flag2 = (1 == 0);
			}
			else
			{
				flag2 = true;
			}
			return flag2 || flag;
		}
		return false;
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x0003B583 File Offset: 0x00039783
	public override string GetMyUserID()
	{
		return PhotonNetwork.LocalPlayer.UserId;
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x000AA32C File Offset: 0x000A852C
	public override string GetUserID(int playerID)
	{
		NetPlayer player = this.GetPlayer(playerID);
		if (player != null)
		{
			return player.UserId;
		}
		return null;
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x000AA34C File Offset: 0x000A854C
	public override string GetUserID(NetPlayer netPlayer)
	{
		Player playerRef = ((PunNetPlayer)netPlayer).PlayerRef;
		if (playerRef != null)
		{
			return playerRef.UserId;
		}
		return null;
	}

	// Token: 0x060010A2 RID: 4258 RVA: 0x000AA370 File Offset: 0x000A8570
	public override int GlobalPlayerCount()
	{
		int num = 0;
		foreach (NetworkRegionInfo networkRegionInfo in this.regionData)
		{
			num += networkRegionInfo.playersInRegion;
		}
		return num;
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x000AA3A4 File Offset: 0x000A85A4
	public override bool IsObjectLocallyOwned(GameObject obj)
	{
		PhotonView photonView;
		return !this.IsOnline || !obj.TryGetComponent<PhotonView>(out photonView) || photonView.IsMine;
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x000AA3D0 File Offset: 0x000A85D0
	protected override void UpdateNetPlayerList()
	{
		if (!this.IsOnline)
		{
			bool flag = false;
			PunNetPlayer punNetPlayer = null;
			if (this.netPlayerCache.Count > 0)
			{
				for (int i = 0; i < this.netPlayerCache.Count; i++)
				{
					NetPlayer netPlayer = this.netPlayerCache[i];
					if (netPlayer.IsLocal)
					{
						punNetPlayer = (PunNetPlayer)netPlayer;
						flag = true;
					}
					else
					{
						this.playerPool.Return((PunNetPlayer)netPlayer);
					}
				}
				this.netPlayerCache.Clear();
			}
			if (!flag)
			{
				punNetPlayer = this.playerPool.Take();
				punNetPlayer.InitPlayer(PhotonNetwork.LocalPlayer);
			}
			this.netPlayerCache.Add(punNetPlayer);
		}
		else
		{
			Dictionary<int, Player>.ValueCollection values = PhotonNetwork.CurrentRoom.Players.Values;
			foreach (Player player in values)
			{
				bool flag2 = false;
				for (int j = 0; j < this.netPlayerCache.Count; j++)
				{
					if (player == ((PunNetPlayer)this.netPlayerCache[j]).PlayerRef)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					PunNetPlayer punNetPlayer2 = this.playerPool.Take();
					punNetPlayer2.InitPlayer(player);
					this.netPlayerCache.Add(punNetPlayer2);
				}
			}
			for (int k = 0; k < this.netPlayerCache.Count; k++)
			{
				PunNetPlayer punNetPlayer3 = (PunNetPlayer)this.netPlayerCache[k];
				bool flag3 = false;
				using (Dictionary<int, Player>.ValueCollection.Enumerator enumerator = values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == punNetPlayer3.PlayerRef)
						{
							flag3 = true;
							break;
						}
					}
				}
				if (!flag3)
				{
					this.playerPool.Return(punNetPlayer3);
					this.netPlayerCache.Remove(punNetPlayer3);
				}
			}
		}
		this.m_allNetPlayers = this.netPlayerCache.ToArray();
		this.m_otherNetPlayers = new NetPlayer[this.m_allNetPlayers.Length - 1];
		int num = 0;
		for (int l = 0; l < this.m_allNetPlayers.Length; l++)
		{
			NetPlayer netPlayer2 = this.m_allNetPlayers[l];
			if (netPlayer2.IsLocal)
			{
				num++;
			}
			else
			{
				int num2 = l - num;
				if (num2 == this.m_otherNetPlayers.Length)
				{
					break;
				}
				this.m_otherNetPlayers[num2] = netPlayer2;
			}
		}
	}

	// Token: 0x060010A5 RID: 4261 RVA: 0x000AA63C File Offset: 0x000A883C
	public override bool IsObjectRoomObject(GameObject obj)
	{
		PhotonView component = obj.GetComponent<PhotonView>();
		if (component == null)
		{
			Debug.LogError("No photonview found on this Object, this shouldn't happen");
			return false;
		}
		return component.IsRoomView;
	}

	// Token: 0x060010A6 RID: 4262 RVA: 0x0003B58F File Offset: 0x0003978F
	public override bool ShouldUpdateObject(GameObject obj)
	{
		return this.IsObjectLocallyOwned(obj);
	}

	// Token: 0x060010A7 RID: 4263 RVA: 0x0003B58F File Offset: 0x0003978F
	public override bool ShouldWriteObjectData(GameObject obj)
	{
		return this.IsObjectLocallyOwned(obj);
	}

	// Token: 0x060010A8 RID: 4264 RVA: 0x000AA66C File Offset: 0x000A886C
	public override int GetOwningPlayerID(GameObject obj)
	{
		PhotonView photonView;
		if (obj.TryGetComponent<PhotonView>(out photonView) && photonView.Owner != null)
		{
			return photonView.Owner.ActorNumber;
		}
		return -1;
	}

	// Token: 0x060010A9 RID: 4265 RVA: 0x0003B598 File Offset: 0x00039798
	public override bool ShouldSpawnLocally(int playerID)
	{
		return this.LocalPlayerID == playerID || (playerID == -1 && PhotonNetwork.MasterClient.IsLocal);
	}

	// Token: 0x060010AA RID: 4266 RVA: 0x00030498 File Offset: 0x0002E698
	public override bool IsTotalAuthority()
	{
		return false;
	}

	// Token: 0x060010AB RID: 4267 RVA: 0x0003B5B5 File Offset: 0x000397B5
	public void OnConnectedtoMaster()
	{
		if (this.internalState == NetworkSystemPUN.InternalState.ConnectingToMaster)
		{
			this.internalState = NetworkSystemPUN.InternalState.ConnectedToMaster;
		}
		base.UpdatePlayers();
	}

	// Token: 0x060010AC RID: 4268 RVA: 0x0003B5CD File Offset: 0x000397CD
	public void OnJoinedRoom()
	{
		if (this.internalState == NetworkSystemPUN.InternalState.Searching_Joining)
		{
			this.internalState = NetworkSystemPUN.InternalState.Searching_Joined;
		}
		else if (this.internalState == NetworkSystemPUN.InternalState.Searching_Creating)
		{
			this.internalState = NetworkSystemPUN.InternalState.Searching_Created;
		}
		this.AddVoice();
		base.UpdatePlayers();
		base.JoinedNetworkRoom();
	}

	// Token: 0x060010AD RID: 4269 RVA: 0x0003B607 File Offset: 0x00039807
	public void OnJoinRoomFailed(short returnCode, string message)
	{
		Debug.Log("onJoinRoomFailed " + returnCode.ToString() + message);
		if (this.internalState == NetworkSystemPUN.InternalState.Searching_Joining)
		{
			if (returnCode == 32765)
			{
				this.internalState = NetworkSystemPUN.InternalState.Searching_JoinFailed_Full;
				return;
			}
			this.internalState = NetworkSystemPUN.InternalState.Searching_JoinFailed;
		}
	}

	// Token: 0x060010AE RID: 4270 RVA: 0x0003B643 File Offset: 0x00039843
	public void OnCreateRoomFailed(short returnCode, string message)
	{
		if (this.internalState == NetworkSystemPUN.InternalState.Searching_Creating)
		{
			this.internalState = NetworkSystemPUN.InternalState.Searching_CreateFailed;
		}
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x000AA698 File Offset: 0x000A8898
	public void OnPlayerEnteredRoom(Player newPlayer)
	{
		base.UpdatePlayers();
		NetPlayer player = base.GetPlayer(newPlayer);
		base.PlayerJoined(player);
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x000AA6BC File Offset: 0x000A88BC
	public void OnPlayerLeftRoom(Player otherPlayer)
	{
		NetPlayer player = base.GetPlayer(otherPlayer);
		base.UpdatePlayers();
		base.PlayerLeft(player);
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x000AA6E0 File Offset: 0x000A88E0
	public void OnDisconnected(DisconnectCause cause)
	{
		NetworkSystemPUN.<OnDisconnected>d__114 <OnDisconnected>d__;
		<OnDisconnected>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnDisconnected>d__.<>4__this = this;
		<OnDisconnected>d__.<>1__state = -1;
		<OnDisconnected>d__.<>t__builder.Start<NetworkSystemPUN.<OnDisconnected>d__114>(ref <OnDisconnected>d__);
	}

	// Token: 0x060010B2 RID: 4274 RVA: 0x0003B657 File Offset: 0x00039857
	public void OnMasterClientSwitched(Player newMasterClient)
	{
		base.OnMasterClientSwitchedCallback(newMasterClient);
	}

	// Token: 0x060010B3 RID: 4275 RVA: 0x000AA718 File Offset: 0x000A8918
	private ValueTuple<CancellationTokenSource, CancellationToken> GetCancellationToken()
	{
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		CancellationToken token = cancellationTokenSource.Token;
		this._taskCancelTokens.Add(cancellationTokenSource);
		return new ValueTuple<CancellationTokenSource, CancellationToken>(cancellationTokenSource, token);
	}

	// Token: 0x060010B4 RID: 4276 RVA: 0x000AA748 File Offset: 0x000A8948
	public void ResetSystem()
	{
		if (this.VoiceNetworkObject)
		{
			UnityEngine.Object.Destroy(this.VoiceNetworkObject);
		}
		PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = this.regionNames[this.lowestPingRegionIndex];
		this.currentRegionIndex = this.lowestPingRegionIndex;
		PhotonNetwork.Disconnect();
		this._taskCancelTokens.ForEach(delegate(CancellationTokenSource token)
		{
			token.Cancel();
			token.Dispose();
		});
		this._taskCancelTokens.Clear();
		this.internalState = NetworkSystemPUN.InternalState.Idle;
		base.netState = NetSystemState.Idle;
	}

	// Token: 0x060010B5 RID: 4277 RVA: 0x000AA7E0 File Offset: 0x000A89E0
	private void UpdateZoneInfo(bool roomIsPublic, string zoneName = null)
	{
		AuthenticationValues authenticationValues = this.GetAuthenticationValues();
		Dictionary<string, object> dictionary = ((authenticationValues != null) ? authenticationValues.AuthPostData : null) as Dictionary<string, object>;
		if (dictionary != null)
		{
			dictionary["Zone"] = ((zoneName != null) ? zoneName : ((ZoneManagement.instance.activeZones.Count > 0) ? ZoneManagement.instance.activeZones.First<GTZone>().GetName<GTZone>() : ""));
			dictionary["SubZone"] = GTSubZone.none.GetName<GTSubZone>();
			dictionary["IsPublic"] = roomIsPublic;
			authenticationValues.SetAuthPostData(dictionary);
			this.SetAuthenticationValues(authenticationValues);
		}
	}

	// Token: 0x04001271 RID: 4721
	private NetworkRegionInfo[] regionData;

	// Token: 0x04001272 RID: 4722
	private Task<NetJoinResult> roomTask;

	// Token: 0x04001273 RID: 4723
	private ObjectPool<PunNetPlayer> playerPool;

	// Token: 0x04001274 RID: 4724
	private NetPlayer[] m_allNetPlayers = new NetPlayer[0];

	// Token: 0x04001275 RID: 4725
	private NetPlayer[] m_otherNetPlayers = new NetPlayer[0];

	// Token: 0x04001276 RID: 4726
	private List<CancellationTokenSource> _taskCancelTokens = new List<CancellationTokenSource>();

	// Token: 0x04001277 RID: 4727
	private PhotonVoiceNetwork punVoice;

	// Token: 0x04001278 RID: 4728
	private GameObject VoiceNetworkObject;

	// Token: 0x04001279 RID: 4729
	private NetworkSystemPUN.InternalState currentState;

	// Token: 0x0400127A RID: 4730
	private bool firstRoomJoin;

	// Token: 0x020002A5 RID: 677
	private enum InternalState
	{
		// Token: 0x0400127C RID: 4732
		AwaitingAuth,
		// Token: 0x0400127D RID: 4733
		Authenticated,
		// Token: 0x0400127E RID: 4734
		PingGathering,
		// Token: 0x0400127F RID: 4735
		StateCheckFailed,
		// Token: 0x04001280 RID: 4736
		ConnectingToMaster,
		// Token: 0x04001281 RID: 4737
		ConnectedToMaster,
		// Token: 0x04001282 RID: 4738
		Idle,
		// Token: 0x04001283 RID: 4739
		Internal_Disconnecting,
		// Token: 0x04001284 RID: 4740
		Internal_Disconnected,
		// Token: 0x04001285 RID: 4741
		Searching_Connecting,
		// Token: 0x04001286 RID: 4742
		Searching_Connected,
		// Token: 0x04001287 RID: 4743
		Searching_Joining,
		// Token: 0x04001288 RID: 4744
		Searching_Joined,
		// Token: 0x04001289 RID: 4745
		Searching_JoinFailed,
		// Token: 0x0400128A RID: 4746
		Searching_JoinFailed_Full,
		// Token: 0x0400128B RID: 4747
		Searching_Creating,
		// Token: 0x0400128C RID: 4748
		Searching_Created,
		// Token: 0x0400128D RID: 4749
		Searching_CreateFailed,
		// Token: 0x0400128E RID: 4750
		Searching_Disconnecting,
		// Token: 0x0400128F RID: 4751
		Searching_Disconnected
	}
}
