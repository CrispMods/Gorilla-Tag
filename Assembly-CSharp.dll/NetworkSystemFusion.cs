using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Fusion;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using GorillaGameModes;
using GorillaTag;
using GorillaTag.Audio;
using KID.Model;
using Photon.Realtime;
using Photon.Voice.Fusion;
using Photon.Voice.Unity;
using PlayFab;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000266 RID: 614
public class NetworkSystemFusion : NetworkSystem
{
	// Token: 0x17000172 RID: 370
	// (get) Token: 0x06000E59 RID: 3673 RVA: 0x00039232 File Offset: 0x00037432
	// (set) Token: 0x06000E5A RID: 3674 RVA: 0x0003923A File Offset: 0x0003743A
	public NetworkRunner runner { get; private set; }

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06000E5B RID: 3675 RVA: 0x00039243 File Offset: 0x00037443
	public override bool IsOnline
	{
		get
		{
			return this.runner != null && !this.runner.IsSinglePlayer;
		}
	}

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06000E5C RID: 3676 RVA: 0x00039263 File Offset: 0x00037463
	public override bool InRoom
	{
		get
		{
			return this.runner != null && this.runner.State != NetworkRunner.States.Shutdown && !this.runner.IsSinglePlayer && this.runner.IsConnectedToServer;
		}
	}

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06000E5D RID: 3677 RVA: 0x0003929B File Offset: 0x0003749B
	public override string RoomName
	{
		get
		{
			SessionInfo sessionInfo = this.runner.SessionInfo;
			if (sessionInfo == null)
			{
				return null;
			}
			return sessionInfo.Name;
		}
	}

	// Token: 0x06000E5E RID: 3678 RVA: 0x000A2388 File Offset: 0x000A0588
	public override string RoomStringStripped()
	{
		SessionInfo sessionInfo = this.runner.SessionInfo;
		NetworkSystem.reusableSB.Clear();
		NetworkSystem.reusableSB.AppendFormat("Room: '{0}' ", (sessionInfo.Name.Length < 20) ? sessionInfo.Name : sessionInfo.Name.Remove(20));
		NetworkSystem.reusableSB.AppendFormat("{0},{1} {3}/{2} players.", new object[]
		{
			sessionInfo.IsVisible ? "visible" : "hidden",
			sessionInfo.IsOpen ? "open" : "closed",
			sessionInfo.MaxPlayers,
			sessionInfo.PlayerCount
		});
		NetworkSystem.reusableSB.Append("\ncustomProps: {");
		NetworkSystem.reusableSB.AppendFormat("joinedGameMode={0}, ", (RoomSystem.RoomGameMode.Length < 50) ? RoomSystem.RoomGameMode : RoomSystem.RoomGameMode.Remove(50));
		IDictionary properties = sessionInfo.Properties;
		Debug.Log(RoomSystem.RoomGameMode.ToString());
		if (properties.Contains("gameMode"))
		{
			object obj = properties["gameMode"];
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

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x06000E5F RID: 3679 RVA: 0x000A2524 File Offset: 0x000A0724
	public override string GameModeString
	{
		get
		{
			SessionProperty sessionProperty;
			this.runner.SessionInfo.Properties.TryGetValue("gameMode", out sessionProperty);
			if (sessionProperty != null)
			{
				return (string)sessionProperty.PropertyValue;
			}
			return null;
		}
	}

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x06000E60 RID: 3680 RVA: 0x000392B3 File Offset: 0x000374B3
	public override string CurrentRegion
	{
		get
		{
			SessionInfo sessionInfo = this.runner.SessionInfo;
			if (sessionInfo == null)
			{
				return null;
			}
			return sessionInfo.Region;
		}
	}

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x06000E61 RID: 3681 RVA: 0x000A2560 File Offset: 0x000A0760
	public override bool SessionIsPrivate
	{
		get
		{
			NetworkRunner runner = this.runner;
			bool? flag;
			if (runner == null)
			{
				flag = null;
			}
			else
			{
				SessionInfo sessionInfo = runner.SessionInfo;
				flag = ((sessionInfo != null) ? new bool?(!sessionInfo.IsVisible) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}
	}

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x06000E62 RID: 3682 RVA: 0x000A25AC File Offset: 0x000A07AC
	public override int LocalPlayerID
	{
		get
		{
			return this.runner.LocalPlayer.PlayerId;
		}
	}

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x06000E63 RID: 3683 RVA: 0x000392CB File Offset: 0x000374CB
	public override string CurrentPhotonBackend
	{
		get
		{
			return "Fusion";
		}
	}

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x06000E64 RID: 3684 RVA: 0x000392D2 File Offset: 0x000374D2
	public override double SimTime
	{
		get
		{
			return (double)this.runner.SimulationTime;
		}
	}

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06000E65 RID: 3685 RVA: 0x000392E0 File Offset: 0x000374E0
	public override float SimDeltaTime
	{
		get
		{
			return this.runner.DeltaTime;
		}
	}

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06000E66 RID: 3686 RVA: 0x000392ED File Offset: 0x000374ED
	public override int SimTick
	{
		get
		{
			return this.runner.Tick.Raw;
		}
	}

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06000E67 RID: 3687 RVA: 0x000392FF File Offset: 0x000374FF
	public override int TickRate
	{
		get
		{
			return this.runner.TickRate;
		}
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06000E68 RID: 3688 RVA: 0x000392ED File Offset: 0x000374ED
	public override int ServerTimestamp
	{
		get
		{
			return this.runner.Tick.Raw;
		}
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x06000E69 RID: 3689 RVA: 0x0003930C File Offset: 0x0003750C
	public override int RoomPlayerCount
	{
		get
		{
			return this.runner.SessionInfo.PlayerCount;
		}
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x06000E6A RID: 3690 RVA: 0x0003931E File Offset: 0x0003751E
	public override VoiceConnection VoiceConnection
	{
		get
		{
			return this.FusionVoice;
		}
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x06000E6B RID: 3691 RVA: 0x00039326 File Offset: 0x00037526
	public override bool IsMasterClient
	{
		get
		{
			NetworkRunner runner = this.runner;
			return runner == null || runner.IsSharedModeMasterClient;
		}
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06000E6C RID: 3692 RVA: 0x000A25CC File Offset: 0x000A07CC
	public override NetPlayer MasterClient
	{
		get
		{
			if (this.runner != null && this.runner.IsSharedModeMasterClient)
			{
				return base.GetPlayer(this.runner.LocalPlayer);
			}
			if (!(GorillaGameModes.GameMode.ActiveNetworkHandler != null))
			{
				return null;
			}
			return base.GetPlayer(GorillaGameModes.GameMode.ActiveNetworkHandler.Object.StateAuthority);
		}
	}

	// Token: 0x06000E6D RID: 3693 RVA: 0x000A262C File Offset: 0x000A082C
	public override void Initialise()
	{
		NetworkSystemFusion.<Initialise>d__54 <Initialise>d__;
		<Initialise>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Initialise>d__.<>4__this = this;
		<Initialise>d__.<>1__state = -1;
		<Initialise>d__.<>t__builder.Start<NetworkSystemFusion.<Initialise>d__54>(ref <Initialise>d__);
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x000A2664 File Offset: 0x000A0864
	private void CreateRegionCrawler()
	{
		GameObject gameObject = new GameObject("[Network Crawler]");
		gameObject.transform.SetParent(base.transform);
		this.regionCrawler = gameObject.AddComponent<FusionRegionCrawler>();
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x000A269C File Offset: 0x000A089C
	private Task AwaitAuth()
	{
		NetworkSystemFusion.<AwaitAuth>d__56 <AwaitAuth>d__;
		<AwaitAuth>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AwaitAuth>d__.<>4__this = this;
		<AwaitAuth>d__.<>1__state = -1;
		<AwaitAuth>d__.<>t__builder.Start<NetworkSystemFusion.<AwaitAuth>d__56>(ref <AwaitAuth>d__);
		return <AwaitAuth>d__.<>t__builder.Task;
	}

	// Token: 0x06000E70 RID: 3696 RVA: 0x00039339 File Offset: 0x00037539
	public override void FinishAuthenticating()
	{
		if (this.cachedPlayfabAuth != null)
		{
			Debug.Log("AUTHED");
			return;
		}
		Debug.LogError("Authentication Failed");
	}

	// Token: 0x06000E71 RID: 3697 RVA: 0x000A26E0 File Offset: 0x000A08E0
	public override Task<NetJoinResult> ConnectToRoom(string roomName, RoomConfig opts, int regionIndex = -1)
	{
		NetworkSystemFusion.<ConnectToRoom>d__58 <ConnectToRoom>d__;
		<ConnectToRoom>d__.<>t__builder = AsyncTaskMethodBuilder<NetJoinResult>.Create();
		<ConnectToRoom>d__.<>4__this = this;
		<ConnectToRoom>d__.roomName = roomName;
		<ConnectToRoom>d__.opts = opts;
		<ConnectToRoom>d__.<>1__state = -1;
		<ConnectToRoom>d__.<>t__builder.Start<NetworkSystemFusion.<ConnectToRoom>d__58>(ref <ConnectToRoom>d__);
		return <ConnectToRoom>d__.<>t__builder.Task;
	}

	// Token: 0x06000E72 RID: 3698 RVA: 0x000A2734 File Offset: 0x000A0934
	private Task<bool> Connect(Fusion.GameMode mode, string targetSessionName, RoomConfig opts)
	{
		NetworkSystemFusion.<Connect>d__59 <Connect>d__;
		<Connect>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<Connect>d__.<>4__this = this;
		<Connect>d__.mode = mode;
		<Connect>d__.targetSessionName = targetSessionName;
		<Connect>d__.opts = opts;
		<Connect>d__.<>1__state = -1;
		<Connect>d__.<>t__builder.Start<NetworkSystemFusion.<Connect>d__59>(ref <Connect>d__);
		return <Connect>d__.<>t__builder.Task;
	}

	// Token: 0x06000E73 RID: 3699 RVA: 0x000A2790 File Offset: 0x000A0990
	private Task<NetJoinResult> MakeOrJoinRoom(string roomName, RoomConfig opts)
	{
		NetworkSystemFusion.<MakeOrJoinRoom>d__60 <MakeOrJoinRoom>d__;
		<MakeOrJoinRoom>d__.<>t__builder = AsyncTaskMethodBuilder<NetJoinResult>.Create();
		<MakeOrJoinRoom>d__.<>4__this = this;
		<MakeOrJoinRoom>d__.roomName = roomName;
		<MakeOrJoinRoom>d__.opts = opts;
		<MakeOrJoinRoom>d__.<>1__state = -1;
		<MakeOrJoinRoom>d__.<>t__builder.Start<NetworkSystemFusion.<MakeOrJoinRoom>d__60>(ref <MakeOrJoinRoom>d__);
		return <MakeOrJoinRoom>d__.<>t__builder.Task;
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x000A27E4 File Offset: 0x000A09E4
	private Task<NetJoinResult> JoinRandomPublicRoom(RoomConfig opts)
	{
		NetworkSystemFusion.<JoinRandomPublicRoom>d__61 <JoinRandomPublicRoom>d__;
		<JoinRandomPublicRoom>d__.<>t__builder = AsyncTaskMethodBuilder<NetJoinResult>.Create();
		<JoinRandomPublicRoom>d__.<>4__this = this;
		<JoinRandomPublicRoom>d__.opts = opts;
		<JoinRandomPublicRoom>d__.<>1__state = -1;
		<JoinRandomPublicRoom>d__.<>t__builder.Start<NetworkSystemFusion.<JoinRandomPublicRoom>d__61>(ref <JoinRandomPublicRoom>d__);
		return <JoinRandomPublicRoom>d__.<>t__builder.Task;
	}

	// Token: 0x06000E75 RID: 3701 RVA: 0x000A2830 File Offset: 0x000A0A30
	public override Task JoinFriendsRoom(string userID, int actorIDToFollow, string keyToFollow, string shufflerToFollow)
	{
		NetworkSystemFusion.<JoinFriendsRoom>d__62 <JoinFriendsRoom>d__;
		<JoinFriendsRoom>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<JoinFriendsRoom>d__.<>4__this = this;
		<JoinFriendsRoom>d__.userID = userID;
		<JoinFriendsRoom>d__.actorIDToFollow = actorIDToFollow;
		<JoinFriendsRoom>d__.keyToFollow = keyToFollow;
		<JoinFriendsRoom>d__.shufflerToFollow = shufflerToFollow;
		<JoinFriendsRoom>d__.<>1__state = -1;
		<JoinFriendsRoom>d__.<>t__builder.Start<NetworkSystemFusion.<JoinFriendsRoom>d__62>(ref <JoinFriendsRoom>d__);
		return <JoinFriendsRoom>d__.<>t__builder.Task;
	}

	// Token: 0x06000E76 RID: 3702 RVA: 0x0002F834 File Offset: 0x0002DA34
	public override void JoinPubWithFriends()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x000A2894 File Offset: 0x000A0A94
	public override Task ReturnToSinglePlayer()
	{
		NetworkSystemFusion.<ReturnToSinglePlayer>d__64 <ReturnToSinglePlayer>d__;
		<ReturnToSinglePlayer>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ReturnToSinglePlayer>d__.<>4__this = this;
		<ReturnToSinglePlayer>d__.<>1__state = -1;
		<ReturnToSinglePlayer>d__.<>t__builder.Start<NetworkSystemFusion.<ReturnToSinglePlayer>d__64>(ref <ReturnToSinglePlayer>d__);
		return <ReturnToSinglePlayer>d__.<>t__builder.Task;
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x000A28D8 File Offset: 0x000A0AD8
	private Task CloseRunner(ShutdownReason reason = ShutdownReason.Ok)
	{
		NetworkSystemFusion.<CloseRunner>d__65 <CloseRunner>d__;
		<CloseRunner>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<CloseRunner>d__.<>4__this = this;
		<CloseRunner>d__.reason = reason;
		<CloseRunner>d__.<>1__state = -1;
		<CloseRunner>d__.<>t__builder.Start<NetworkSystemFusion.<CloseRunner>d__65>(ref <CloseRunner>d__);
		return <CloseRunner>d__.<>t__builder.Task;
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x000A2924 File Offset: 0x000A0B24
	public void MigrateHost(NetworkRunner runner, HostMigrationToken hostMigrationToken)
	{
		NetworkSystemFusion.<MigrateHost>d__66 <MigrateHost>d__;
		<MigrateHost>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<MigrateHost>d__.<>4__this = this;
		<MigrateHost>d__.<>1__state = -1;
		<MigrateHost>d__.<>t__builder.Start<NetworkSystemFusion.<MigrateHost>d__66>(ref <MigrateHost>d__);
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x000A295C File Offset: 0x000A0B5C
	public void ResetSystem()
	{
		NetworkSystemFusion.<ResetSystem>d__67 <ResetSystem>d__;
		<ResetSystem>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<ResetSystem>d__.<>4__this = this;
		<ResetSystem>d__.<>1__state = -1;
		<ResetSystem>d__.<>t__builder.Start<NetworkSystemFusion.<ResetSystem>d__67>(ref <ResetSystem>d__);
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x00039358 File Offset: 0x00037558
	private void AddVoice()
	{
		this.SetupVoice();
		this.FusionVoiceBridge = this.volatileNetObj.AddComponent<FusionVoiceBridge>();
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x000A2994 File Offset: 0x000A0B94
	private void SetupVoice()
	{
		Utils.Log("<color=orange>Adding Voice Stuff</color>");
		this.FusionVoice = this.volatileNetObj.AddComponent<VoiceConnection>();
		this.FusionVoice.LogLevel = this.VoiceSettings.LogLevel;
		this.FusionVoice.GlobalRecordersLogLevel = this.VoiceSettings.GlobalRecordersLogLevel;
		this.FusionVoice.GlobalSpeakersLogLevel = this.VoiceSettings.GlobalSpeakersLogLevel;
		this.FusionVoice.AutoCreateSpeakerIfNotFound = this.VoiceSettings.CreateSpeakerIfNotFound;
		Photon.Realtime.AppSettings appSettings = new Photon.Realtime.AppSettings();
		appSettings.AppIdFusion = PhotonAppSettings.Global.AppSettings.AppIdFusion;
		appSettings.AppIdVoice = PhotonAppSettings.Global.AppSettings.AppIdVoice;
		this.FusionVoice.Settings = appSettings;
		this.remoteVoiceAddedCallbacks.ForEach(delegate(Action<RemoteVoiceLink> callback)
		{
			this.FusionVoice.RemoteVoiceAdded += callback;
		});
		this.localRecorder = this.volatileNetObj.AddComponent<Recorder>();
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
		this.localRecorder.UserData = this.runner.UserId;
		this.FusionVoice.PrimaryRecorder = this.localRecorder;
		this.volatileNetObj.AddComponent<VoiceToLoudness>();
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x00039371 File Offset: 0x00037571
	public override void AddRemoteVoiceAddedCallback(Action<RemoteVoiceLink> callback)
	{
		this.remoteVoiceAddedCallbacks.Add(callback);
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x0003937F File Offset: 0x0003757F
	private void AttachCallbackTargets()
	{
		this.runner.AddCallbacks(this.objectsThatNeedCallbacks.ToArray());
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x00039397 File Offset: 0x00037597
	public void RegisterForNetworkCallbacks(INetworkRunnerCallbacks callbacks)
	{
		if (!this.objectsThatNeedCallbacks.Contains(callbacks))
		{
			this.objectsThatNeedCallbacks.Add(callbacks);
		}
		if (this.runner != null)
		{
			this.runner.AddCallbacks(new INetworkRunnerCallbacks[]
			{
				callbacks
			});
		}
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x000A2C38 File Offset: 0x000A0E38
	private void AttachSceneObjects(bool onlyCached = false)
	{
		NetworkSystemFusion.<AttachSceneObjects>d__75 <AttachSceneObjects>d__;
		<AttachSceneObjects>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<AttachSceneObjects>d__.<>4__this = this;
		<AttachSceneObjects>d__.onlyCached = onlyCached;
		<AttachSceneObjects>d__.<>1__state = -1;
		<AttachSceneObjects>d__.<>t__builder.Start<NetworkSystemFusion.<AttachSceneObjects>d__75>(ref <AttachSceneObjects>d__);
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x000A2C78 File Offset: 0x000A0E78
	public override void AttachObjectInGame(GameObject item)
	{
		base.AttachObjectInGame(item);
		NetworkObject component = item.GetComponent<NetworkObject>();
		if ((component != null && !this.cachedNetSceneObjects.Contains(component)) || !component.IsValid)
		{
			this.cachedNetSceneObjects.AddIfNew(component);
			this.registrationQueue.Enqueue(component);
			this.ProcessRegistrationQueue();
		}
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x000A2CD0 File Offset: 0x000A0ED0
	private void ProcessRegistrationQueue()
	{
		if (this.isProcessingQueue)
		{
			Debug.LogError("Queue is still processing");
			return;
		}
		this.isProcessingQueue = true;
		List<NetworkObject> list = new List<NetworkObject>();
		SceneRef scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
		while (this.registrationQueue.Count > 0)
		{
			NetworkObject networkObject = this.registrationQueue.Dequeue();
			if (this.InRoom && !networkObject.IsValid && !networkObject.Id.IsValid && networkObject.Runner == null)
			{
				try
				{
					list.Add(networkObject);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					this.isProcessingQueue = false;
					this.runner.RegisterSceneObjects(scene, list.ToArray(), default(NetworkSceneLoadId));
					this.ProcessRegistrationQueue();
					break;
				}
			}
		}
		this.runner.RegisterSceneObjects(scene, list.ToArray(), default(NetworkSceneLoadId));
		this.isProcessingQueue = false;
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x000A2DCC File Offset: 0x000A0FCC
	public override GameObject NetInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, bool isRoomObject = false)
	{
		Utils.Log("Net instantiate Fusion: " + prefab.name);
		try
		{
			return this.runner.Spawn(prefab, new Vector3?(position), new Quaternion?(rotation), new PlayerRef?(this.runner.LocalPlayer), null, (NetworkSpawnFlags)0).gameObject;
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return null;
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x000A2E3C File Offset: 0x000A103C
	public override GameObject NetInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, int playerAuthID, bool isRoomObject = false)
	{
		foreach (PlayerRef value in this.runner.ActivePlayers)
		{
			if (value.PlayerId == playerAuthID)
			{
				Utils.Log("Net instantiate Fusion: " + prefab.name);
				return this.runner.Spawn(prefab, new Vector3?(position), new Quaternion?(rotation), new PlayerRef?(value), null, (NetworkSpawnFlags)0).gameObject;
			}
		}
		Debug.LogError(string.Format("Couldn't find player with ID: {0}, cancelling requested spawn...", playerAuthID));
		return null;
	}

	// Token: 0x06000E85 RID: 3717 RVA: 0x000A2EE8 File Offset: 0x000A10E8
	public override GameObject NetInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, bool isRoomObject, byte group = 0, object[] data = null, NetworkRunner.OnBeforeSpawned callback = null)
	{
		Utils.Log("Net instantiate Fusion: " + prefab.name);
		return this.runner.Spawn(prefab, new Vector3?(position), new Quaternion?(rotation), new PlayerRef?(this.runner.LocalPlayer), callback, (NetworkSpawnFlags)0).gameObject;
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x000A2F3C File Offset: 0x000A113C
	public override void NetDestroy(GameObject instance)
	{
		NetworkObject networkObject;
		if (instance.TryGetComponent<NetworkObject>(out networkObject))
		{
			this.runner.Despawn(networkObject);
			return;
		}
		UnityEngine.Object.Destroy(instance);
	}

	// Token: 0x06000E87 RID: 3719 RVA: 0x000A2F68 File Offset: 0x000A1168
	public override bool ShouldSpawnLocally(int playerID)
	{
		if (this.runner.GameMode == Fusion.GameMode.Shared)
		{
			return this.runner.LocalPlayer.PlayerId == playerID || (playerID == -1 && this.runner.IsSharedModeMasterClient);
		}
		return this.runner.GameMode != Fusion.GameMode.Client;
	}

	// Token: 0x06000E88 RID: 3720 RVA: 0x000A2FC0 File Offset: 0x000A11C0
	public override void CallRPC(MonoBehaviour component, NetworkSystem.RPC rpcMethod, bool sendToSelf = true)
	{
		Utils.Log(rpcMethod.GetDelegateName() + "RPC called!");
		foreach (PlayerRef a in this.runner.ActivePlayers)
		{
			if (!sendToSelf)
			{
				a != this.runner.LocalPlayer;
			}
		}
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x000A3038 File Offset: 0x000A1238
	public override void CallRPC<T>(MonoBehaviour component, NetworkSystem.RPC rpcMethod, RPCArgBuffer<T> args, bool sendToSelf = true)
	{
		Utils.Log(rpcMethod.GetDelegateName() + "RPC called!");
		ref args.SerializeToRPCData<T>();
		foreach (PlayerRef a in this.runner.ActivePlayers)
		{
			if (!sendToSelf)
			{
				a != this.runner.LocalPlayer;
			}
		}
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x000A30B8 File Offset: 0x000A12B8
	public override void CallRPC(MonoBehaviour component, NetworkSystem.StringRPC rpcMethod, string message, bool sendToSelf = true)
	{
		foreach (PlayerRef a in this.runner.ActivePlayers)
		{
			if (!sendToSelf)
			{
				a != this.runner.LocalPlayer;
			}
		}
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x000393D6 File Offset: 0x000375D6
	public override void CallRPC(int targetPlayerID, MonoBehaviour component, NetworkSystem.RPC rpcMethod)
	{
		this.GetPlayerRef(targetPlayerID);
		Utils.Log(rpcMethod.GetDelegateName() + "RPC called!");
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x000393F5 File Offset: 0x000375F5
	public override void CallRPC<T>(int targetPlayerID, MonoBehaviour component, NetworkSystem.RPC rpcMethod, RPCArgBuffer<T> args)
	{
		Utils.Log(rpcMethod.GetDelegateName() + "RPC called!");
		this.GetPlayerRef(targetPlayerID);
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x00039414 File Offset: 0x00037614
	public override void CallRPC(int targetPlayerID, MonoBehaviour component, NetworkSystem.StringRPC rpcMethod, string message)
	{
		this.GetPlayerRef(targetPlayerID);
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x000A311C File Offset: 0x000A131C
	public override void NetRaiseEventReliable(byte eventCode, object data)
	{
		byte[] byteData = data.ByteSerialize();
		FusionCallbackHandler.RPC_OnEventRaisedReliable(this.runner, eventCode, byteData, false, null, default(RpcInfo));
	}

	// Token: 0x06000E8F RID: 3727 RVA: 0x000A3148 File Offset: 0x000A1348
	public override void NetRaiseEventUnreliable(byte eventCode, object data)
	{
		byte[] byteData = data.ByteSerialize();
		FusionCallbackHandler.RPC_OnEventRaisedUnreliable(this.runner, eventCode, byteData, false, null, default(RpcInfo));
	}

	// Token: 0x06000E90 RID: 3728 RVA: 0x000A3174 File Offset: 0x000A1374
	public override void NetRaiseEventReliable(byte eventCode, object data, NetEventOptions opts)
	{
		byte[] byteData = data.ByteSerialize();
		byte[] netOptsData = opts.ByteSerialize();
		FusionCallbackHandler.RPC_OnEventRaisedReliable(this.runner, eventCode, byteData, true, netOptsData, default(RpcInfo));
	}

	// Token: 0x06000E91 RID: 3729 RVA: 0x000A31A8 File Offset: 0x000A13A8
	public override void NetRaiseEventUnreliable(byte eventCode, object data, NetEventOptions opts)
	{
		byte[] byteData = data.ByteSerialize();
		byte[] netOptsData = opts.ByteSerialize();
		FusionCallbackHandler.RPC_OnEventRaisedUnreliable(this.runner, eventCode, byteData, true, netOptsData, default(RpcInfo));
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x0002F834 File Offset: 0x0002DA34
	public override string GetRandomWeightedRegion()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x000A31DC File Offset: 0x000A13DC
	public override Task AwaitSceneReady()
	{
		NetworkSystemFusion.<AwaitSceneReady>d__94 <AwaitSceneReady>d__;
		<AwaitSceneReady>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AwaitSceneReady>d__.<>4__this = this;
		<AwaitSceneReady>d__.<>1__state = -1;
		<AwaitSceneReady>d__.<>t__builder.Start<NetworkSystemFusion.<AwaitSceneReady>d__94>(ref <AwaitSceneReady>d__);
		return <AwaitSceneReady>d__.<>t__builder.Task;
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnJoinedSession()
	{
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x0003941E File Offset: 0x0003761E
	public void OnJoinFailed(NetConnectFailedReason reason)
	{
		switch (reason)
		{
		case NetConnectFailedReason.Timeout:
		case NetConnectFailedReason.ServerRefused:
			break;
		case NetConnectFailedReason.ServerFull:
			this.lastConnectAttempt_WasFull = true;
			break;
		default:
			return;
		}
	}

	// Token: 0x06000E96 RID: 3734 RVA: 0x0003943C File Offset: 0x0003763C
	public void OnDisconnectedFromSession()
	{
		Utils.Log("On Disconnected");
		this.internalState = NetworkSystemFusion.InternalState.Disconnected;
		base.UpdatePlayers();
	}

	// Token: 0x06000E97 RID: 3735 RVA: 0x00039456 File Offset: 0x00037656
	public void OnRunnerShutDown()
	{
		Utils.Log("Runner shutdown callback");
		if (this.internalState == NetworkSystemFusion.InternalState.Disconnecting)
		{
			this.internalState = NetworkSystemFusion.InternalState.Disconnected;
		}
	}

	// Token: 0x06000E98 RID: 3736 RVA: 0x00039474 File Offset: 0x00037674
	public void OnFusionPlayerJoined(PlayerRef player)
	{
		this.AwaitJoiningPlayerClientReady(player);
	}

	// Token: 0x06000E99 RID: 3737 RVA: 0x000A3220 File Offset: 0x000A1420
	private Task AwaitJoiningPlayerClientReady(PlayerRef player)
	{
		NetworkSystemFusion.<AwaitJoiningPlayerClientReady>d__100 <AwaitJoiningPlayerClientReady>d__;
		<AwaitJoiningPlayerClientReady>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AwaitJoiningPlayerClientReady>d__.<>4__this = this;
		<AwaitJoiningPlayerClientReady>d__.player = player;
		<AwaitJoiningPlayerClientReady>d__.<>1__state = -1;
		<AwaitJoiningPlayerClientReady>d__.<>t__builder.Start<NetworkSystemFusion.<AwaitJoiningPlayerClientReady>d__100>(ref <AwaitJoiningPlayerClientReady>d__);
		return <AwaitJoiningPlayerClientReady>d__.<>t__builder.Task;
	}

	// Token: 0x06000E9A RID: 3738 RVA: 0x000A326C File Offset: 0x000A146C
	public void OnFusionPlayerLeft(PlayerRef player)
	{
		if (this.IsTotalAuthority())
		{
			NetworkObject playerObject = this.runner.GetPlayerObject(player);
			if (playerObject != null)
			{
				Utils.Log("Destroying player object for leaving player!");
				this.NetDestroy(playerObject.gameObject);
			}
			else
			{
				Utils.Log("Player left without destroying an avatar for it somehow?");
			}
		}
		NetPlayer player2 = base.GetPlayer(player);
		if (player2 == null)
		{
			Debug.LogError("Joining player doesnt have a NetPlayer somehow, this shouldnt happen");
		}
		base.PlayerLeft(player2);
		base.UpdatePlayers();
	}

	// Token: 0x06000E9B RID: 3739 RVA: 0x000A32DC File Offset: 0x000A14DC
	protected override void UpdateNetPlayerList()
	{
		if (this.runner == null)
		{
			if (this.netPlayerCache.Count <= 1)
			{
				if (this.netPlayerCache.Exists((NetPlayer p) => p.IsLocal))
				{
					goto IL_84;
				}
			}
			this.netPlayerCache.ForEach(delegate(NetPlayer p)
			{
				this.playerPool.Return((FusionNetPlayer)p);
			});
			this.netPlayerCache.Clear();
			this.netPlayerCache.Add(new FusionNetPlayer(default(PlayerRef)));
			return;
		}
		IL_84:
		NetPlayer[] array;
		if (this.runner.IsSinglePlayer)
		{
			if (this.netPlayerCache.Count == 1 && this.netPlayerCache[0].IsLocal)
			{
				return;
			}
			bool flag = false;
			array = this.netPlayerCache.ToArray();
			if (this.netPlayerCache.Count > 0)
			{
				foreach (NetPlayer netPlayer in array)
				{
					if (((FusionNetPlayer)netPlayer).PlayerRef == this.runner.LocalPlayer)
					{
						flag = true;
					}
					else
					{
						this.playerPool.Return((FusionNetPlayer)netPlayer);
						this.netPlayerCache.Remove(netPlayer);
					}
				}
			}
			if (!flag)
			{
				FusionNetPlayer fusionNetPlayer = this.playerPool.Take();
				fusionNetPlayer.InitPlayer(this.runner.LocalPlayer);
				this.netPlayerCache.Add(fusionNetPlayer);
			}
		}
		foreach (PlayerRef playerRef in this.runner.ActivePlayers)
		{
			bool flag2 = false;
			for (int j = 0; j < this.netPlayerCache.Count; j++)
			{
				if (playerRef == ((FusionNetPlayer)this.netPlayerCache[j]).PlayerRef)
				{
					flag2 = true;
				}
			}
			if (!flag2)
			{
				FusionNetPlayer fusionNetPlayer2 = this.playerPool.Take();
				fusionNetPlayer2.InitPlayer(playerRef);
				this.netPlayerCache.Add(fusionNetPlayer2);
			}
		}
		array = this.netPlayerCache.ToArray();
		foreach (NetPlayer netPlayer2 in array)
		{
			bool flag3 = false;
			using (IEnumerator<PlayerRef> enumerator = this.runner.ActivePlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == ((FusionNetPlayer)netPlayer2).PlayerRef)
					{
						flag3 = true;
					}
				}
			}
			if (!flag3)
			{
				this.playerPool.Return((FusionNetPlayer)netPlayer2);
				this.netPlayerCache.Remove(netPlayer2);
			}
		}
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x000A3598 File Offset: 0x000A1798
	public override void SetPlayerObject(GameObject playerInstance, int? owningPlayerID = null)
	{
		PlayerRef player = this.runner.LocalPlayer;
		if (owningPlayerID != null)
		{
			player = this.GetPlayerRef(owningPlayerID.Value);
		}
		this.runner.SetPlayerObject(player, playerInstance.GetComponent<NetworkObject>());
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x000A35DC File Offset: 0x000A17DC
	private PlayerRef GetPlayerRef(int playerID)
	{
		if (this.runner == null)
		{
			Debug.LogWarning("There is no runner yet - returning default player ref");
			return default(PlayerRef);
		}
		foreach (PlayerRef result in this.runner.ActivePlayers)
		{
			if (result.PlayerId == playerID)
			{
				return result;
			}
		}
		Debug.LogWarning(string.Format("GetPlayerRef - Couldn't find active player with ID #{0}", playerID));
		return default(PlayerRef);
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x000A3678 File Offset: 0x000A1878
	public override NetPlayer GetLocalPlayer()
	{
		if (this.netPlayerCache.Count == 0 || this.netPlayerCache.Count != this.runner.SessionInfo.PlayerCount)
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
		Debug.LogError("Somehow there is no local NetPlayer. This shoulnd't happen.");
		return null;
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x000A3710 File Offset: 0x000A1910
	public override NetPlayer GetPlayer(int PlayerID)
	{
		if (PlayerID == -1)
		{
			Debug.LogWarning("Attempting to get NetPlayer for local -1 ID.");
			return null;
		}
		foreach (NetPlayer netPlayer in this.netPlayerCache)
		{
			if (netPlayer.ActorNumber == PlayerID)
			{
				return netPlayer;
			}
		}
		if (this.netPlayerCache.Count == 0 || this.netPlayerCache.Count != this.runner.SessionInfo.PlayerCount)
		{
			base.UpdatePlayers();
			foreach (NetPlayer netPlayer2 in this.netPlayerCache)
			{
				if (netPlayer2.ActorNumber == PlayerID)
				{
					return netPlayer2;
				}
			}
		}
		Debug.LogError("Failed to find the player, before and after resyncing the player cache, this probably shoulnd't happen...");
		return null;
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x000A3804 File Offset: 0x000A1A04
	public override void SetMyNickName(string name)
	{
		ValueTuple<bool, Permission.ManagedByEnum> customNicknamePermissionStatus = KIDManager.Instance.GetCustomNicknamePermissionStatus();
		if (((!customNicknamePermissionStatus.Item1 && customNicknamePermissionStatus.Item2 != Permission.ManagedByEnum.PLAYER) || customNicknamePermissionStatus.Item2 == Permission.ManagedByEnum.PROHIBITED) && !name.StartsWith("gorilla"))
		{
			Debug.Log("[KID] Trying to set custom nickname but that permission has been disallowed");
			if (this.InRoom && GorillaTagger.Instance.rigSerializer != null)
			{
				GorillaTagger.Instance.rigSerializer.nickName = "gorilla";
			}
			return;
		}
		PlayerPrefs.SetString("playerName", name);
		if (this.InRoom && GorillaTagger.Instance.rigSerializer != null)
		{
			GorillaTagger.Instance.rigSerializer.nickName = name;
		}
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x0003947E File Offset: 0x0003767E
	public override string GetMyNickName()
	{
		return PlayerPrefs.GetString("playerName");
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x000A38C4 File Offset: 0x000A1AC4
	public override string GetMyDefaultName()
	{
		return "gorilla" + UnityEngine.Random.Range(0, 9999).ToString().PadLeft(4, '0');
	}

	// Token: 0x06000EA3 RID: 3747 RVA: 0x000A38F8 File Offset: 0x000A1AF8
	public override string GetNickName(int playerID)
	{
		NetPlayer player = this.GetPlayer(playerID);
		return this.GetNickName(player);
	}

	// Token: 0x06000EA4 RID: 3748 RVA: 0x000A3914 File Offset: 0x000A1B14
	public override string GetNickName(NetPlayer player)
	{
		if (player == null)
		{
			Debug.LogError("Cant get nick name as playerID doesnt have a NetPlayer...");
			return "";
		}
		RigContainer rigContainer;
		VRRigCache.Instance.TryGetVrrig(player, out rigContainer);
		ValueTuple<bool, Permission.ManagedByEnum> customNicknamePermissionStatus = KIDManager.Instance.GetCustomNicknamePermissionStatus();
		if ((!customNicknamePermissionStatus.Item1 && customNicknamePermissionStatus.Item2 != Permission.ManagedByEnum.PLAYER) || customNicknamePermissionStatus.Item2 == Permission.ManagedByEnum.PROHIBITED)
		{
			return rigContainer.Rig.rigSerializer.defaultName.Value ?? "";
		}
		return rigContainer.Rig.rigSerializer.nickName.Value ?? "";
	}

	// Token: 0x06000EA5 RID: 3749 RVA: 0x0003948A File Offset: 0x0003768A
	public override string GetMyUserID()
	{
		return this.runner.GetPlayerUserId(this.runner.LocalPlayer);
	}

	// Token: 0x06000EA6 RID: 3750 RVA: 0x000394A2 File Offset: 0x000376A2
	public override string GetUserID(int playerID)
	{
		if (this.runner == null)
		{
			return string.Empty;
		}
		return this.runner.GetPlayerUserId(this.GetPlayerRef(playerID));
	}

	// Token: 0x06000EA7 RID: 3751 RVA: 0x000394CA File Offset: 0x000376CA
	public override string GetUserID(NetPlayer player)
	{
		if (this.runner == null)
		{
			return string.Empty;
		}
		return this.runner.GetPlayerUserId(((FusionNetPlayer)player).PlayerRef);
	}

	// Token: 0x06000EA8 RID: 3752 RVA: 0x000394F6 File Offset: 0x000376F6
	public override void SetMyTutorialComplete()
	{
		if (!(PlayerPrefs.GetString("didTutorial", "nope") == "done"))
		{
			PlayerPrefs.SetString("didTutorial", "done");
			PlayerPrefs.Save();
		}
	}

	// Token: 0x06000EA9 RID: 3753 RVA: 0x00039527 File Offset: 0x00037727
	public override bool GetMyTutorialCompletion()
	{
		return PlayerPrefs.GetString("didTutorial", "nope") == "done";
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x000A39B4 File Offset: 0x000A1BB4
	public override bool GetPlayerTutorialCompletion(int playerID)
	{
		NetPlayer player = this.GetPlayer(playerID);
		if (player == null)
		{
			Debug.LogError("Player not found");
			return false;
		}
		RigContainer rigContainer;
		VRRigCache.Instance.TryGetVrrig(player, out rigContainer);
		if (rigContainer == null)
		{
			Debug.LogError("VRRig not found for player");
			return false;
		}
		if (rigContainer.Rig.rigSerializer == null)
		{
			Debug.LogWarning("Vr rig serializer is not set up on the rig yet");
			return false;
		}
		return rigContainer.Rig.rigSerializer.tutorialComplete;
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x00039542 File Offset: 0x00037742
	public override int GlobalPlayerCount()
	{
		if (this.regionCrawler == null)
		{
			return 0;
		}
		return this.regionCrawler.PlayerCountGlobal;
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x000A3A2C File Offset: 0x000A1C2C
	public override int GetOwningPlayerID(GameObject obj)
	{
		NetworkObject networkObject;
		if (!obj.TryGetComponent<NetworkObject>(out networkObject))
		{
			return -1;
		}
		if (this.runner.GameMode == Fusion.GameMode.Shared)
		{
			return networkObject.StateAuthority.PlayerId;
		}
		return networkObject.InputAuthority.PlayerId;
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x000A3A70 File Offset: 0x000A1C70
	public override bool IsObjectLocallyOwned(GameObject obj)
	{
		NetworkObject networkObject;
		if (!obj.TryGetComponent<NetworkObject>(out networkObject))
		{
			return false;
		}
		if (this.runner.GameMode == Fusion.GameMode.Shared)
		{
			return networkObject.StateAuthority == this.runner.LocalPlayer;
		}
		return networkObject.InputAuthority == this.runner.LocalPlayer;
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x0003955F File Offset: 0x0003775F
	public override bool IsTotalAuthority()
	{
		return this.runner.Mode == SimulationModes.Server || this.runner.Mode == SimulationModes.Host || this.runner.GameMode == Fusion.GameMode.Single || this.runner.IsSharedModeMasterClient;
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x000A3AC4 File Offset: 0x000A1CC4
	public override bool ShouldWriteObjectData(GameObject obj)
	{
		NetworkObject networkObject;
		return obj.TryGetComponent<NetworkObject>(out networkObject) && networkObject.HasStateAuthority;
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x000A3AE4 File Offset: 0x000A1CE4
	public override bool ShouldUpdateObject(GameObject obj)
	{
		NetworkObject networkObject;
		if (!obj.TryGetComponent<NetworkObject>(out networkObject))
		{
			return true;
		}
		if (this.IsTotalAuthority())
		{
			return true;
		}
		if (networkObject.InputAuthority.IsRealPlayer && !networkObject.InputAuthority.IsRealPlayer)
		{
			return networkObject.InputAuthority == this.runner.LocalPlayer;
		}
		return this.runner.IsSharedModeMasterClient;
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x000A3B4C File Offset: 0x000A1D4C
	public override bool IsObjectRoomObject(GameObject obj)
	{
		NetworkObject networkObject;
		if (obj.TryGetComponent<NetworkObject>(out networkObject))
		{
			Debug.LogWarning("Fusion currently automatically passes false for roomobject check.");
			return false;
		}
		return false;
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x000A3B70 File Offset: 0x000A1D70
	private void OnMasterSwitch(NetPlayer player)
	{
		if (this.runner.IsSharedModeMasterClient)
		{
			Dictionary<string, SessionProperty> customProperties = new Dictionary<string, SessionProperty>
			{
				{
					"MasterClient",
					base.LocalPlayer.ActorNumber
				}
			};
			this.runner.SessionInfo.UpdateCustomProperties(customProperties);
		}
	}

	// Token: 0x04001114 RID: 4372
	private NetworkSystemFusion.InternalState internalState;

	// Token: 0x04001115 RID: 4373
	private FusionInternalRPCs internalRPCProvider;

	// Token: 0x04001116 RID: 4374
	private FusionCallbackHandler callbackHandler;

	// Token: 0x04001117 RID: 4375
	private FusionRegionCrawler regionCrawler;

	// Token: 0x04001118 RID: 4376
	private GameObject volatileNetObj;

	// Token: 0x04001119 RID: 4377
	private Fusion.Photon.Realtime.AuthenticationValues cachedPlayfabAuth;

	// Token: 0x0400111A RID: 4378
	private const string playerPropertiesPath = "P_FusionProperties";

	// Token: 0x0400111B RID: 4379
	private bool lastConnectAttempt_WasFull;

	// Token: 0x0400111C RID: 4380
	private FusionVoiceBridge FusionVoiceBridge;

	// Token: 0x0400111D RID: 4381
	private VoiceConnection FusionVoice;

	// Token: 0x0400111E RID: 4382
	private CustomObjectProvider myObjectProvider;

	// Token: 0x0400111F RID: 4383
	private ObjectPool<FusionNetPlayer> playerPool;

	// Token: 0x04001120 RID: 4384
	public List<NetworkObject> cachedNetSceneObjects = new List<NetworkObject>();

	// Token: 0x04001121 RID: 4385
	private List<INetworkRunnerCallbacks> objectsThatNeedCallbacks = new List<INetworkRunnerCallbacks>();

	// Token: 0x04001122 RID: 4386
	private Queue<NetworkObject> registrationQueue = new Queue<NetworkObject>();

	// Token: 0x04001123 RID: 4387
	private bool isProcessingQueue;

	// Token: 0x02000267 RID: 615
	private enum InternalState
	{
		// Token: 0x04001125 RID: 4389
		AwaitingAuth,
		// Token: 0x04001126 RID: 4390
		Idle,
		// Token: 0x04001127 RID: 4391
		Searching_Joining,
		// Token: 0x04001128 RID: 4392
		Searching_Joined,
		// Token: 0x04001129 RID: 4393
		Searching_JoinFailed,
		// Token: 0x0400112A RID: 4394
		Searching_Disconnecting,
		// Token: 0x0400112B RID: 4395
		Searching_Disconnected,
		// Token: 0x0400112C RID: 4396
		ConnectingToRoom,
		// Token: 0x0400112D RID: 4397
		ConnectedToRoom,
		// Token: 0x0400112E RID: 4398
		JoinRoomFailed,
		// Token: 0x0400112F RID: 4399
		Disconnecting,
		// Token: 0x04001130 RID: 4400
		Disconnected,
		// Token: 0x04001131 RID: 4401
		StateCheckFailed
	}
}
