﻿using System;
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

// Token: 0x02000299 RID: 665
[RequireComponent(typeof(PUNCallbackNotifier))]
public class NetworkSystemPUN : NetworkSystem
{
	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06001015 RID: 4117 RVA: 0x0003A156 File Offset: 0x00038356
	public override NetPlayer[] AllNetPlayers
	{
		get
		{
			return this.m_allNetPlayers;
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06001016 RID: 4118 RVA: 0x0003A15E File Offset: 0x0003835E
	public override NetPlayer[] PlayerListOthers
	{
		get
		{
			return this.m_otherNetPlayers;
		}
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06001017 RID: 4119 RVA: 0x0003A166 File Offset: 0x00038366
	public override VoiceConnection VoiceConnection
	{
		get
		{
			return this.punVoice;
		}
	}

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x06001018 RID: 4120 RVA: 0x000A6C50 File Offset: 0x000A4E50
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

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x06001019 RID: 4121 RVA: 0x0003A16E File Offset: 0x0003836E
	// (set) Token: 0x0600101A RID: 4122 RVA: 0x0003A176 File Offset: 0x00038376
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

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x0600101B RID: 4123 RVA: 0x0003A17F File Offset: 0x0003837F
	public override string CurrentPhotonBackend
	{
		get
		{
			return "PUN";
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x0600101C RID: 4124 RVA: 0x0003A186 File Offset: 0x00038386
	public override bool IsOnline
	{
		get
		{
			return this.InRoom;
		}
	}

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x0600101D RID: 4125 RVA: 0x0003A18E File Offset: 0x0003838E
	public override bool InRoom
	{
		get
		{
			return PhotonNetwork.InRoom;
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x0600101E RID: 4126 RVA: 0x0003A195 File Offset: 0x00038395
	public override string RoomName
	{
		get
		{
			Room currentRoom = PhotonNetwork.CurrentRoom;
			return ((currentRoom != null) ? currentRoom.Name : null) ?? string.Empty;
		}
	}

	// Token: 0x0600101F RID: 4127 RVA: 0x000A6C9C File Offset: 0x000A4E9C
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

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x06001020 RID: 4128 RVA: 0x000A6E24 File Offset: 0x000A5024
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

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x06001021 RID: 4129 RVA: 0x0003A1B1 File Offset: 0x000383B1
	public override string CurrentRegion
	{
		get
		{
			return PhotonNetwork.CloudRegion;
		}
	}

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x06001022 RID: 4130 RVA: 0x0003A1B8 File Offset: 0x000383B8
	public override bool SessionIsPrivate
	{
		get
		{
			Room currentRoom = PhotonNetwork.CurrentRoom;
			return currentRoom != null && !currentRoom.IsVisible;
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x06001023 RID: 4131 RVA: 0x0003A1CD File Offset: 0x000383CD
	public override int LocalPlayerID
	{
		get
		{
			return PhotonNetwork.LocalPlayer.ActorNumber;
		}
	}

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x06001024 RID: 4132 RVA: 0x0003A1D9 File Offset: 0x000383D9
	public override int ServerTimestamp
	{
		get
		{
			return PhotonNetwork.ServerTimestamp;
		}
	}

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x06001025 RID: 4133 RVA: 0x0003A1E0 File Offset: 0x000383E0
	public override double SimTime
	{
		get
		{
			return PhotonNetwork.Time;
		}
	}

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x06001026 RID: 4134 RVA: 0x0003A1E7 File Offset: 0x000383E7
	public override float SimDeltaTime
	{
		get
		{
			return Time.deltaTime;
		}
	}

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x06001027 RID: 4135 RVA: 0x0003A1D9 File Offset: 0x000383D9
	public override int SimTick
	{
		get
		{
			return PhotonNetwork.ServerTimestamp;
		}
	}

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x06001028 RID: 4136 RVA: 0x0003A1EE File Offset: 0x000383EE
	public override int TickRate
	{
		get
		{
			return PhotonNetwork.SerializationRate;
		}
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06001029 RID: 4137 RVA: 0x0003A1F5 File Offset: 0x000383F5
	public override int RoomPlayerCount
	{
		get
		{
			return (int)PhotonNetwork.CurrentRoom.PlayerCount;
		}
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x0600102A RID: 4138 RVA: 0x0003A201 File Offset: 0x00038401
	public override bool IsMasterClient
	{
		get
		{
			return PhotonNetwork.IsMasterClient;
		}
	}

	// Token: 0x0600102B RID: 4139 RVA: 0x000A6E54 File Offset: 0x000A5054
	public override void Initialise()
	{
		NetworkSystemPUN.<Initialise>d__53 <Initialise>d__;
		<Initialise>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Initialise>d__.<>4__this = this;
		<Initialise>d__.<>1__state = -1;
		<Initialise>d__.<>t__builder.Start<NetworkSystemPUN.<Initialise>d__53>(ref <Initialise>d__);
	}

	// Token: 0x0600102C RID: 4140 RVA: 0x000A6E8C File Offset: 0x000A508C
	private Task CacheRegionInfo()
	{
		NetworkSystemPUN.<CacheRegionInfo>d__54 <CacheRegionInfo>d__;
		<CacheRegionInfo>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<CacheRegionInfo>d__.<>4__this = this;
		<CacheRegionInfo>d__.<>1__state = -1;
		<CacheRegionInfo>d__.<>t__builder.Start<NetworkSystemPUN.<CacheRegionInfo>d__54>(ref <CacheRegionInfo>d__);
		return <CacheRegionInfo>d__.<>t__builder.Task;
	}

	// Token: 0x0600102D RID: 4141 RVA: 0x0003A208 File Offset: 0x00038408
	public override AuthenticationValues GetAuthenticationValues()
	{
		return PhotonNetwork.AuthValues;
	}

	// Token: 0x0600102E RID: 4142 RVA: 0x0003A20F File Offset: 0x0003840F
	public override void SetAuthenticationValues(AuthenticationValues authValues)
	{
		PhotonNetwork.AuthValues = authValues;
	}

	// Token: 0x0600102F RID: 4143 RVA: 0x0003A217 File Offset: 0x00038417
	public override void FinishAuthenticating()
	{
		this.internalState = NetworkSystemPUN.InternalState.Authenticated;
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x000A6ED0 File Offset: 0x000A50D0
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

	// Token: 0x06001031 RID: 4145 RVA: 0x000A6F24 File Offset: 0x000A5124
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

	// Token: 0x06001032 RID: 4146 RVA: 0x000A6F70 File Offset: 0x000A5170
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

	// Token: 0x06001033 RID: 4147 RVA: 0x000A6FCC File Offset: 0x000A51CC
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

	// Token: 0x06001034 RID: 4148 RVA: 0x000A7020 File Offset: 0x000A5220
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

	// Token: 0x06001035 RID: 4149 RVA: 0x000A707C File Offset: 0x000A527C
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

	// Token: 0x06001036 RID: 4150 RVA: 0x000A70D0 File Offset: 0x000A52D0
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

	// Token: 0x06001037 RID: 4151 RVA: 0x000A711C File Offset: 0x000A531C
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

	// Token: 0x06001038 RID: 4152 RVA: 0x000A7178 File Offset: 0x000A5378
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

	// Token: 0x06001039 RID: 4153 RVA: 0x0002F834 File Offset: 0x0002DA34
	public override void JoinPubWithFriends()
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x000A71DC File Offset: 0x000A53DC
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

	// Token: 0x0600103B RID: 4155 RVA: 0x000A7254 File Offset: 0x000A5454
	public override Task ReturnToSinglePlayer()
	{
		NetworkSystemPUN.<ReturnToSinglePlayer>d__69 <ReturnToSinglePlayer>d__;
		<ReturnToSinglePlayer>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ReturnToSinglePlayer>d__.<>4__this = this;
		<ReturnToSinglePlayer>d__.<>1__state = -1;
		<ReturnToSinglePlayer>d__.<>t__builder.Start<NetworkSystemPUN.<ReturnToSinglePlayer>d__69>(ref <ReturnToSinglePlayer>d__);
		return <ReturnToSinglePlayer>d__.<>t__builder.Task;
	}

	// Token: 0x0600103C RID: 4156 RVA: 0x000A7298 File Offset: 0x000A5498
	private Task InternalDisconnect()
	{
		NetworkSystemPUN.<InternalDisconnect>d__70 <InternalDisconnect>d__;
		<InternalDisconnect>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<InternalDisconnect>d__.<>4__this = this;
		<InternalDisconnect>d__.<>1__state = -1;
		<InternalDisconnect>d__.<>t__builder.Start<NetworkSystemPUN.<InternalDisconnect>d__70>(ref <InternalDisconnect>d__);
		return <InternalDisconnect>d__.<>t__builder.Task;
	}

	// Token: 0x0600103D RID: 4157 RVA: 0x0003A220 File Offset: 0x00038420
	private void AddVoice()
	{
		this.SetupVoice();
	}

	// Token: 0x0600103E RID: 4158 RVA: 0x000A72DC File Offset: 0x000A54DC
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

	// Token: 0x0600103F RID: 4159 RVA: 0x00039371 File Offset: 0x00037571
	public override void AddRemoteVoiceAddedCallback(Action<RemoteVoiceLink> callback)
	{
		this.remoteVoiceAddedCallbacks.Add(callback);
	}

	// Token: 0x06001040 RID: 4160 RVA: 0x0003A228 File Offset: 0x00038428
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

	// Token: 0x06001041 RID: 4161 RVA: 0x0003A25D File Offset: 0x0003845D
	public override GameObject NetInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, int playerAuthID, bool isRoomObject = false)
	{
		return this.NetInstantiate(prefab, position, rotation, isRoomObject);
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x0003A26A File Offset: 0x0003846A
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

	// Token: 0x06001043 RID: 4163 RVA: 0x000A7608 File Offset: 0x000A5808
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

	// Token: 0x06001044 RID: 4164 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void SetPlayerObject(GameObject playerInstance, int? owningPlayerID = null)
	{
	}

	// Token: 0x06001045 RID: 4165 RVA: 0x000A7634 File Offset: 0x000A5834
	public override void CallRPC(MonoBehaviour component, NetworkSystem.RPC rpcMethod, bool sendToSelf = true)
	{
		RpcTarget target = sendToSelf ? RpcTarget.All : RpcTarget.Others;
		PhotonView.Get(component).RPC(rpcMethod.Method.Name, target, new object[]
		{
			NetworkSystem.EmptyArgs
		});
	}

	// Token: 0x06001046 RID: 4166 RVA: 0x000A7670 File Offset: 0x000A5870
	public override void CallRPC<T>(MonoBehaviour component, NetworkSystem.RPC rpcMethod, RPCArgBuffer<T> args, bool sendToSelf = true)
	{
		RpcTarget target = sendToSelf ? RpcTarget.All : RpcTarget.Others;
		ref args.SerializeToRPCData<T>();
		PhotonView.Get(component).RPC(rpcMethod.Method.Name, target, new object[]
		{
			args.Data
		});
	}

	// Token: 0x06001047 RID: 4167 RVA: 0x000A76B4 File Offset: 0x000A58B4
	public override void CallRPC(MonoBehaviour component, NetworkSystem.StringRPC rpcMethod, string message, bool sendToSelf = true)
	{
		RpcTarget target = sendToSelf ? RpcTarget.All : RpcTarget.Others;
		PhotonView.Get(component).RPC(rpcMethod.Method.Name, target, new object[]
		{
			message
		});
	}

	// Token: 0x06001048 RID: 4168 RVA: 0x000A76EC File Offset: 0x000A58EC
	public override void CallRPC(int targetPlayerID, MonoBehaviour component, NetworkSystem.RPC rpcMethod)
	{
		Player player = PhotonNetwork.CurrentRoom.GetPlayer(targetPlayerID, false);
		PhotonView.Get(component).RPC(rpcMethod.Method.Name, player, new object[]
		{
			NetworkSystem.EmptyArgs
		});
	}

	// Token: 0x06001049 RID: 4169 RVA: 0x000A772C File Offset: 0x000A592C
	public override void CallRPC<T>(int targetPlayerID, MonoBehaviour component, NetworkSystem.RPC rpcMethod, RPCArgBuffer<T> args)
	{
		Player player = PhotonNetwork.CurrentRoom.GetPlayer(targetPlayerID, false);
		ref args.SerializeToRPCData<T>();
		PhotonView.Get(component).RPC(rpcMethod.Method.Name, player, new object[]
		{
			args.Data
		});
	}

	// Token: 0x0600104A RID: 4170 RVA: 0x000A7774 File Offset: 0x000A5974
	public override void CallRPC(int targetPlayerID, MonoBehaviour component, NetworkSystem.StringRPC rpcMethod, string message)
	{
		Player player = PhotonNetwork.CurrentRoom.GetPlayer(targetPlayerID, false);
		PhotonView.Get(component).RPC(rpcMethod.Method.Name, player, new object[]
		{
			message
		});
	}

	// Token: 0x0600104B RID: 4171 RVA: 0x000A77B0 File Offset: 0x000A59B0
	public override Task AwaitSceneReady()
	{
		NetworkSystemPUN.<AwaitSceneReady>d__85 <AwaitSceneReady>d__;
		<AwaitSceneReady>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AwaitSceneReady>d__.<>1__state = -1;
		<AwaitSceneReady>d__.<>t__builder.Start<NetworkSystemPUN.<AwaitSceneReady>d__85>(ref <AwaitSceneReady>d__);
		return <AwaitSceneReady>d__.<>t__builder.Task;
	}

	// Token: 0x0600104C RID: 4172 RVA: 0x000A77EC File Offset: 0x000A59EC
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

	// Token: 0x0600104D RID: 4173 RVA: 0x000A7864 File Offset: 0x000A5A64
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

	// Token: 0x0600104E RID: 4174 RVA: 0x000A7944 File Offset: 0x000A5B44
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

	// Token: 0x0600104F RID: 4175 RVA: 0x0003A2A3 File Offset: 0x000384A3
	public override string GetMyNickName()
	{
		return PhotonNetwork.LocalPlayer.NickName;
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x0003A2AF File Offset: 0x000384AF
	public override string GetMyDefaultName()
	{
		return PhotonNetwork.LocalPlayer.DefaultName;
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x000A79BC File Offset: 0x000A5BBC
	public override string GetNickName(int playerID)
	{
		NetPlayer player = this.GetPlayer(playerID);
		if (player != null)
		{
			return player.NickName;
		}
		return null;
	}

	// Token: 0x06001052 RID: 4178 RVA: 0x0003A2BB File Offset: 0x000384BB
	public override string GetNickName(NetPlayer player)
	{
		return player.NickName;
	}

	// Token: 0x06001053 RID: 4179 RVA: 0x000A79DC File Offset: 0x000A5BDC
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

	// Token: 0x06001054 RID: 4180 RVA: 0x00039527 File Offset: 0x00037727
	public override bool GetMyTutorialCompletion()
	{
		return PlayerPrefs.GetString("didTutorial", "nope") == "done";
	}

	// Token: 0x06001055 RID: 4181 RVA: 0x000A7A40 File Offset: 0x000A5C40
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

	// Token: 0x06001056 RID: 4182 RVA: 0x0003A2C3 File Offset: 0x000384C3
	public override string GetMyUserID()
	{
		return PhotonNetwork.LocalPlayer.UserId;
	}

	// Token: 0x06001057 RID: 4183 RVA: 0x000A7AA0 File Offset: 0x000A5CA0
	public override string GetUserID(int playerID)
	{
		NetPlayer player = this.GetPlayer(playerID);
		if (player != null)
		{
			return player.UserId;
		}
		return null;
	}

	// Token: 0x06001058 RID: 4184 RVA: 0x000A7AC0 File Offset: 0x000A5CC0
	public override string GetUserID(NetPlayer netPlayer)
	{
		Player playerRef = ((PunNetPlayer)netPlayer).PlayerRef;
		if (playerRef != null)
		{
			return playerRef.UserId;
		}
		return null;
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x000A7AE4 File Offset: 0x000A5CE4
	public override int GlobalPlayerCount()
	{
		int num = 0;
		foreach (NetworkRegionInfo networkRegionInfo in this.regionData)
		{
			num += networkRegionInfo.playersInRegion;
		}
		return num;
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x000A7B18 File Offset: 0x000A5D18
	public override bool IsObjectLocallyOwned(GameObject obj)
	{
		PhotonView photonView;
		return !this.IsOnline || !obj.TryGetComponent<PhotonView>(out photonView) || photonView.IsMine;
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x000A7B44 File Offset: 0x000A5D44
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

	// Token: 0x0600105C RID: 4188 RVA: 0x000A7DB0 File Offset: 0x000A5FB0
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

	// Token: 0x0600105D RID: 4189 RVA: 0x0003A2CF File Offset: 0x000384CF
	public override bool ShouldUpdateObject(GameObject obj)
	{
		return this.IsObjectLocallyOwned(obj);
	}

	// Token: 0x0600105E RID: 4190 RVA: 0x0003A2CF File Offset: 0x000384CF
	public override bool ShouldWriteObjectData(GameObject obj)
	{
		return this.IsObjectLocallyOwned(obj);
	}

	// Token: 0x0600105F RID: 4191 RVA: 0x000A7DE0 File Offset: 0x000A5FE0
	public override int GetOwningPlayerID(GameObject obj)
	{
		PhotonView photonView;
		if (obj.TryGetComponent<PhotonView>(out photonView) && photonView.Owner != null)
		{
			return photonView.Owner.ActorNumber;
		}
		return -1;
	}

	// Token: 0x06001060 RID: 4192 RVA: 0x0003A2D8 File Offset: 0x000384D8
	public override bool ShouldSpawnLocally(int playerID)
	{
		return this.LocalPlayerID == playerID || (playerID == -1 && PhotonNetwork.MasterClient.IsLocal);
	}

	// Token: 0x06001061 RID: 4193 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
	public override bool IsTotalAuthority()
	{
		return false;
	}

	// Token: 0x06001062 RID: 4194 RVA: 0x0003A2F5 File Offset: 0x000384F5
	public void OnConnectedtoMaster()
	{
		if (this.internalState == NetworkSystemPUN.InternalState.ConnectingToMaster)
		{
			this.internalState = NetworkSystemPUN.InternalState.ConnectedToMaster;
		}
		base.UpdatePlayers();
	}

	// Token: 0x06001063 RID: 4195 RVA: 0x0003A30D File Offset: 0x0003850D
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

	// Token: 0x06001064 RID: 4196 RVA: 0x0003A347 File Offset: 0x00038547
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

	// Token: 0x06001065 RID: 4197 RVA: 0x0003A383 File Offset: 0x00038583
	public void OnCreateRoomFailed(short returnCode, string message)
	{
		if (this.internalState == NetworkSystemPUN.InternalState.Searching_Creating)
		{
			this.internalState = NetworkSystemPUN.InternalState.Searching_CreateFailed;
		}
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x000A7E0C File Offset: 0x000A600C
	public void OnPlayerEnteredRoom(Player newPlayer)
	{
		base.UpdatePlayers();
		NetPlayer player = base.GetPlayer(newPlayer);
		base.PlayerJoined(player);
	}

	// Token: 0x06001067 RID: 4199 RVA: 0x000A7E30 File Offset: 0x000A6030
	public void OnPlayerLeftRoom(Player otherPlayer)
	{
		NetPlayer player = base.GetPlayer(otherPlayer);
		base.UpdatePlayers();
		base.PlayerLeft(player);
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x000A7E54 File Offset: 0x000A6054
	public void OnDisconnected(DisconnectCause cause)
	{
		NetworkSystemPUN.<OnDisconnected>d__114 <OnDisconnected>d__;
		<OnDisconnected>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnDisconnected>d__.<>4__this = this;
		<OnDisconnected>d__.<>1__state = -1;
		<OnDisconnected>d__.<>t__builder.Start<NetworkSystemPUN.<OnDisconnected>d__114>(ref <OnDisconnected>d__);
	}

	// Token: 0x06001069 RID: 4201 RVA: 0x0003A397 File Offset: 0x00038597
	public void OnMasterClientSwitched(Player newMasterClient)
	{
		base.OnMasterClientSwitchedCallback(newMasterClient);
	}

	// Token: 0x0600106A RID: 4202 RVA: 0x000A7E8C File Offset: 0x000A608C
	private ValueTuple<CancellationTokenSource, CancellationToken> GetCancellationToken()
	{
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		CancellationToken token = cancellationTokenSource.Token;
		this._taskCancelTokens.Add(cancellationTokenSource);
		return new ValueTuple<CancellationTokenSource, CancellationToken>(cancellationTokenSource, token);
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x000A7EBC File Offset: 0x000A60BC
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

	// Token: 0x0600106C RID: 4204 RVA: 0x000A7F54 File Offset: 0x000A6154
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

	// Token: 0x0400122A RID: 4650
	private NetworkRegionInfo[] regionData;

	// Token: 0x0400122B RID: 4651
	private Task<NetJoinResult> roomTask;

	// Token: 0x0400122C RID: 4652
	private ObjectPool<PunNetPlayer> playerPool;

	// Token: 0x0400122D RID: 4653
	private NetPlayer[] m_allNetPlayers = new NetPlayer[0];

	// Token: 0x0400122E RID: 4654
	private NetPlayer[] m_otherNetPlayers = new NetPlayer[0];

	// Token: 0x0400122F RID: 4655
	private List<CancellationTokenSource> _taskCancelTokens = new List<CancellationTokenSource>();

	// Token: 0x04001230 RID: 4656
	private PhotonVoiceNetwork punVoice;

	// Token: 0x04001231 RID: 4657
	private GameObject VoiceNetworkObject;

	// Token: 0x04001232 RID: 4658
	private NetworkSystemPUN.InternalState currentState;

	// Token: 0x04001233 RID: 4659
	private bool firstRoomJoin;

	// Token: 0x0200029A RID: 666
	private enum InternalState
	{
		// Token: 0x04001235 RID: 4661
		AwaitingAuth,
		// Token: 0x04001236 RID: 4662
		Authenticated,
		// Token: 0x04001237 RID: 4663
		PingGathering,
		// Token: 0x04001238 RID: 4664
		StateCheckFailed,
		// Token: 0x04001239 RID: 4665
		ConnectingToMaster,
		// Token: 0x0400123A RID: 4666
		ConnectedToMaster,
		// Token: 0x0400123B RID: 4667
		Idle,
		// Token: 0x0400123C RID: 4668
		Internal_Disconnecting,
		// Token: 0x0400123D RID: 4669
		Internal_Disconnected,
		// Token: 0x0400123E RID: 4670
		Searching_Connecting,
		// Token: 0x0400123F RID: 4671
		Searching_Connected,
		// Token: 0x04001240 RID: 4672
		Searching_Joining,
		// Token: 0x04001241 RID: 4673
		Searching_Joined,
		// Token: 0x04001242 RID: 4674
		Searching_JoinFailed,
		// Token: 0x04001243 RID: 4675
		Searching_JoinFailed_Full,
		// Token: 0x04001244 RID: 4676
		Searching_Creating,
		// Token: 0x04001245 RID: 4677
		Searching_Created,
		// Token: 0x04001246 RID: 4678
		Searching_CreateFailed,
		// Token: 0x04001247 RID: 4679
		Searching_Disconnecting,
		// Token: 0x04001248 RID: 4680
		Searching_Disconnected
	}
}
