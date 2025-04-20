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

// Token: 0x02000271 RID: 625
public class NetworkSystemFusion : NetworkSystem
{
	// Token: 0x17000179 RID: 377
	// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x0003A4F2 File Offset: 0x000386F2
	// (set) Token: 0x06000EA3 RID: 3747 RVA: 0x0003A4FA File Offset: 0x000386FA
	public NetworkRunner runner { get; private set; }

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x0003A503 File Offset: 0x00038703
	public override bool IsOnline
	{
		get
		{
			return this.runner != null && !this.runner.IsSinglePlayer;
		}
	}

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x0003A523 File Offset: 0x00038723
	public override bool InRoom
	{
		get
		{
			return this.runner != null && this.runner.State != NetworkRunner.States.Shutdown && !this.runner.IsSinglePlayer && this.runner.IsConnectedToServer;
		}
	}

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x0003A55B File Offset: 0x0003875B
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

	// Token: 0x06000EA7 RID: 3751 RVA: 0x000A4C14 File Offset: 0x000A2E14
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

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x000A4DB0 File Offset: 0x000A2FB0
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

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x0003A573 File Offset: 0x00038773
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

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06000EAA RID: 3754 RVA: 0x000A4DEC File Offset: 0x000A2FEC
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

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x06000EAB RID: 3755 RVA: 0x000A4E38 File Offset: 0x000A3038
	public override int LocalPlayerID
	{
		get
		{
			return this.runner.LocalPlayer.PlayerId;
		}
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x06000EAC RID: 3756 RVA: 0x0003A58B File Offset: 0x0003878B
	public override string CurrentPhotonBackend
	{
		get
		{
			return "Fusion";
		}
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x06000EAD RID: 3757 RVA: 0x0003A592 File Offset: 0x00038792
	public override double SimTime
	{
		get
		{
			return (double)this.runner.SimulationTime;
		}
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06000EAE RID: 3758 RVA: 0x0003A5A0 File Offset: 0x000387A0
	public override float SimDeltaTime
	{
		get
		{
			return this.runner.DeltaTime;
		}
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06000EAF RID: 3759 RVA: 0x0003A5AD File Offset: 0x000387AD
	public override int SimTick
	{
		get
		{
			return this.runner.Tick.Raw;
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x0003A5BF File Offset: 0x000387BF
	public override int TickRate
	{
		get
		{
			return this.runner.TickRate;
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x06000EB1 RID: 3761 RVA: 0x0003A5AD File Offset: 0x000387AD
	public override int ServerTimestamp
	{
		get
		{
			return this.runner.Tick.Raw;
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x0003A5CC File Offset: 0x000387CC
	public override int RoomPlayerCount
	{
		get
		{
			return this.runner.SessionInfo.PlayerCount;
		}
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x0003A5DE File Offset: 0x000387DE
	public override VoiceConnection VoiceConnection
	{
		get
		{
			return this.FusionVoice;
		}
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x0003A5E6 File Offset: 0x000387E6
	public override bool IsMasterClient
	{
		get
		{
			NetworkRunner runner = this.runner;
			return runner == null || runner.IsSharedModeMasterClient;
		}
	}

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x000A4E58 File Offset: 0x000A3058
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

	// Token: 0x06000EB6 RID: 3766 RVA: 0x000A4EB8 File Offset: 0x000A30B8
	public override void Initialise()
	{
		NetworkSystemFusion.<Initialise>d__54 <Initialise>d__;
		<Initialise>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Initialise>d__.<>4__this = this;
		<Initialise>d__.<>1__state = -1;
		<Initialise>d__.<>t__builder.Start<NetworkSystemFusion.<Initialise>d__54>(ref <Initialise>d__);
	}

	// Token: 0x06000EB7 RID: 3767 RVA: 0x000A4EF0 File Offset: 0x000A30F0
	private void CreateRegionCrawler()
	{
		GameObject gameObject = new GameObject("[Network Crawler]");
		gameObject.transform.SetParent(base.transform);
		this.regionCrawler = gameObject.AddComponent<FusionRegionCrawler>();
	}

	// Token: 0x06000EB8 RID: 3768 RVA: 0x000A4F28 File Offset: 0x000A3128
	private Task AwaitAuth()
	{
		NetworkSystemFusion.<AwaitAuth>d__56 <AwaitAuth>d__;
		<AwaitAuth>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AwaitAuth>d__.<>4__this = this;
		<AwaitAuth>d__.<>1__state = -1;
		<AwaitAuth>d__.<>t__builder.Start<NetworkSystemFusion.<AwaitAuth>d__56>(ref <AwaitAuth>d__);
		return <AwaitAuth>d__.<>t__builder.Task;
	}

	// Token: 0x06000EB9 RID: 3769 RVA: 0x0003A5F9 File Offset: 0x000387F9
	public override void FinishAuthenticating()
	{
		if (this.cachedPlayfabAuth != null)
		{
			Debug.Log("AUTHED");
			return;
		}
		Debug.LogError("Authentication Failed");
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x000A4F6C File Offset: 0x000A316C
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

	// Token: 0x06000EBB RID: 3771 RVA: 0x000A4FC0 File Offset: 0x000A31C0
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

	// Token: 0x06000EBC RID: 3772 RVA: 0x000A501C File Offset: 0x000A321C
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

	// Token: 0x06000EBD RID: 3773 RVA: 0x000A5070 File Offset: 0x000A3270
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

	// Token: 0x06000EBE RID: 3774 RVA: 0x000A50BC File Offset: 0x000A32BC
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

	// Token: 0x06000EBF RID: 3775 RVA: 0x000306DC File Offset: 0x0002E8DC
	public override void JoinPubWithFriends()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x000A5120 File Offset: 0x000A3320
	public override Task ReturnToSinglePlayer()
	{
		NetworkSystemFusion.<ReturnToSinglePlayer>d__64 <ReturnToSinglePlayer>d__;
		<ReturnToSinglePlayer>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ReturnToSinglePlayer>d__.<>4__this = this;
		<ReturnToSinglePlayer>d__.<>1__state = -1;
		<ReturnToSinglePlayer>d__.<>t__builder.Start<NetworkSystemFusion.<ReturnToSinglePlayer>d__64>(ref <ReturnToSinglePlayer>d__);
		return <ReturnToSinglePlayer>d__.<>t__builder.Task;
	}

	// Token: 0x06000EC1 RID: 3777 RVA: 0x000A5164 File Offset: 0x000A3364
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

	// Token: 0x06000EC2 RID: 3778 RVA: 0x000A51B0 File Offset: 0x000A33B0
	public void MigrateHost(NetworkRunner runner, HostMigrationToken hostMigrationToken)
	{
		NetworkSystemFusion.<MigrateHost>d__66 <MigrateHost>d__;
		<MigrateHost>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<MigrateHost>d__.<>4__this = this;
		<MigrateHost>d__.<>1__state = -1;
		<MigrateHost>d__.<>t__builder.Start<NetworkSystemFusion.<MigrateHost>d__66>(ref <MigrateHost>d__);
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x000A51E8 File Offset: 0x000A33E8
	public void ResetSystem()
	{
		NetworkSystemFusion.<ResetSystem>d__67 <ResetSystem>d__;
		<ResetSystem>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<ResetSystem>d__.<>4__this = this;
		<ResetSystem>d__.<>1__state = -1;
		<ResetSystem>d__.<>t__builder.Start<NetworkSystemFusion.<ResetSystem>d__67>(ref <ResetSystem>d__);
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x0003A618 File Offset: 0x00038818
	private void AddVoice()
	{
		this.SetupVoice();
		this.FusionVoiceBridge = this.volatileNetObj.AddComponent<FusionVoiceBridge>();
	}

	// Token: 0x06000EC5 RID: 3781 RVA: 0x000A5220 File Offset: 0x000A3420
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

	// Token: 0x06000EC6 RID: 3782 RVA: 0x0003A631 File Offset: 0x00038831
	public override void AddRemoteVoiceAddedCallback(Action<RemoteVoiceLink> callback)
	{
		this.remoteVoiceAddedCallbacks.Add(callback);
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x0003A63F File Offset: 0x0003883F
	private void AttachCallbackTargets()
	{
		this.runner.AddCallbacks(this.objectsThatNeedCallbacks.ToArray());
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x0003A657 File Offset: 0x00038857
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

	// Token: 0x06000EC9 RID: 3785 RVA: 0x000A54C4 File Offset: 0x000A36C4
	private void AttachSceneObjects(bool onlyCached = false)
	{
		NetworkSystemFusion.<AttachSceneObjects>d__75 <AttachSceneObjects>d__;
		<AttachSceneObjects>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<AttachSceneObjects>d__.<>4__this = this;
		<AttachSceneObjects>d__.onlyCached = onlyCached;
		<AttachSceneObjects>d__.<>1__state = -1;
		<AttachSceneObjects>d__.<>t__builder.Start<NetworkSystemFusion.<AttachSceneObjects>d__75>(ref <AttachSceneObjects>d__);
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x000A5504 File Offset: 0x000A3704
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

	// Token: 0x06000ECB RID: 3787 RVA: 0x000A555C File Offset: 0x000A375C
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

	// Token: 0x06000ECC RID: 3788 RVA: 0x000A5658 File Offset: 0x000A3858
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

	// Token: 0x06000ECD RID: 3789 RVA: 0x000A56C8 File Offset: 0x000A38C8
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

	// Token: 0x06000ECE RID: 3790 RVA: 0x000A5774 File Offset: 0x000A3974
	public override GameObject NetInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, bool isRoomObject, byte group = 0, object[] data = null, NetworkRunner.OnBeforeSpawned callback = null)
	{
		Utils.Log("Net instantiate Fusion: " + prefab.name);
		return this.runner.Spawn(prefab, new Vector3?(position), new Quaternion?(rotation), new PlayerRef?(this.runner.LocalPlayer), callback, (NetworkSpawnFlags)0).gameObject;
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x000A57C8 File Offset: 0x000A39C8
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

	// Token: 0x06000ED0 RID: 3792 RVA: 0x000A57F4 File Offset: 0x000A39F4
	public override bool ShouldSpawnLocally(int playerID)
	{
		if (this.runner.GameMode == Fusion.GameMode.Shared)
		{
			return this.runner.LocalPlayer.PlayerId == playerID || (playerID == -1 && this.runner.IsSharedModeMasterClient);
		}
		return this.runner.GameMode != Fusion.GameMode.Client;
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x000A584C File Offset: 0x000A3A4C
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

	// Token: 0x06000ED2 RID: 3794 RVA: 0x000A58C4 File Offset: 0x000A3AC4
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

	// Token: 0x06000ED3 RID: 3795 RVA: 0x000A5944 File Offset: 0x000A3B44
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

	// Token: 0x06000ED4 RID: 3796 RVA: 0x0003A696 File Offset: 0x00038896
	public override void CallRPC(int targetPlayerID, MonoBehaviour component, NetworkSystem.RPC rpcMethod)
	{
		this.GetPlayerRef(targetPlayerID);
		Utils.Log(rpcMethod.GetDelegateName() + "RPC called!");
	}

	// Token: 0x06000ED5 RID: 3797 RVA: 0x0003A6B5 File Offset: 0x000388B5
	public override void CallRPC<T>(int targetPlayerID, MonoBehaviour component, NetworkSystem.RPC rpcMethod, RPCArgBuffer<T> args)
	{
		Utils.Log(rpcMethod.GetDelegateName() + "RPC called!");
		this.GetPlayerRef(targetPlayerID);
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x0003A6D4 File Offset: 0x000388D4
	public override void CallRPC(int targetPlayerID, MonoBehaviour component, NetworkSystem.StringRPC rpcMethod, string message)
	{
		this.GetPlayerRef(targetPlayerID);
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x000A59A8 File Offset: 0x000A3BA8
	public override void NetRaiseEventReliable(byte eventCode, object data)
	{
		byte[] byteData = data.ByteSerialize();
		FusionCallbackHandler.RPC_OnEventRaisedReliable(this.runner, eventCode, byteData, false, null, default(RpcInfo));
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x000A59D4 File Offset: 0x000A3BD4
	public override void NetRaiseEventUnreliable(byte eventCode, object data)
	{
		byte[] byteData = data.ByteSerialize();
		FusionCallbackHandler.RPC_OnEventRaisedUnreliable(this.runner, eventCode, byteData, false, null, default(RpcInfo));
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x000A5A00 File Offset: 0x000A3C00
	public override void NetRaiseEventReliable(byte eventCode, object data, NetEventOptions opts)
	{
		byte[] byteData = data.ByteSerialize();
		byte[] netOptsData = opts.ByteSerialize();
		FusionCallbackHandler.RPC_OnEventRaisedReliable(this.runner, eventCode, byteData, true, netOptsData, default(RpcInfo));
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x000A5A34 File Offset: 0x000A3C34
	public override void NetRaiseEventUnreliable(byte eventCode, object data, NetEventOptions opts)
	{
		byte[] byteData = data.ByteSerialize();
		byte[] netOptsData = opts.ByteSerialize();
		FusionCallbackHandler.RPC_OnEventRaisedUnreliable(this.runner, eventCode, byteData, true, netOptsData, default(RpcInfo));
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x000306DC File Offset: 0x0002E8DC
	public override string GetRandomWeightedRegion()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x000A5A68 File Offset: 0x000A3C68
	public override Task AwaitSceneReady()
	{
		NetworkSystemFusion.<AwaitSceneReady>d__94 <AwaitSceneReady>d__;
		<AwaitSceneReady>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AwaitSceneReady>d__.<>4__this = this;
		<AwaitSceneReady>d__.<>1__state = -1;
		<AwaitSceneReady>d__.<>t__builder.Start<NetworkSystemFusion.<AwaitSceneReady>d__94>(ref <AwaitSceneReady>d__);
		return <AwaitSceneReady>d__.<>t__builder.Task;
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnJoinedSession()
	{
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x0003A6DE File Offset: 0x000388DE
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

	// Token: 0x06000EDF RID: 3807 RVA: 0x0003A6FC File Offset: 0x000388FC
	public void OnDisconnectedFromSession()
	{
		Utils.Log("On Disconnected");
		this.internalState = NetworkSystemFusion.InternalState.Disconnected;
		base.UpdatePlayers();
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x0003A716 File Offset: 0x00038916
	public void OnRunnerShutDown()
	{
		Utils.Log("Runner shutdown callback");
		if (this.internalState == NetworkSystemFusion.InternalState.Disconnecting)
		{
			this.internalState = NetworkSystemFusion.InternalState.Disconnected;
		}
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x0003A734 File Offset: 0x00038934
	public void OnFusionPlayerJoined(PlayerRef player)
	{
		this.AwaitJoiningPlayerClientReady(player);
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x000A5AAC File Offset: 0x000A3CAC
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

	// Token: 0x06000EE3 RID: 3811 RVA: 0x000A5AF8 File Offset: 0x000A3CF8
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

	// Token: 0x06000EE4 RID: 3812 RVA: 0x000A5B68 File Offset: 0x000A3D68
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

	// Token: 0x06000EE5 RID: 3813 RVA: 0x000A5E24 File Offset: 0x000A4024
	public override void SetPlayerObject(GameObject playerInstance, int? owningPlayerID = null)
	{
		PlayerRef player = this.runner.LocalPlayer;
		if (owningPlayerID != null)
		{
			player = this.GetPlayerRef(owningPlayerID.Value);
		}
		this.runner.SetPlayerObject(player, playerInstance.GetComponent<NetworkObject>());
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x000A5E68 File Offset: 0x000A4068
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

	// Token: 0x06000EE7 RID: 3815 RVA: 0x000A5F04 File Offset: 0x000A4104
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

	// Token: 0x06000EE8 RID: 3816 RVA: 0x000A5F9C File Offset: 0x000A419C
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

	// Token: 0x06000EE9 RID: 3817 RVA: 0x000A6090 File Offset: 0x000A4290
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

	// Token: 0x06000EEA RID: 3818 RVA: 0x0003A73E File Offset: 0x0003893E
	public override string GetMyNickName()
	{
		return PlayerPrefs.GetString("playerName");
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x000A6150 File Offset: 0x000A4350
	public override string GetMyDefaultName()
	{
		return "gorilla" + UnityEngine.Random.Range(0, 9999).ToString().PadLeft(4, '0');
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x000A6184 File Offset: 0x000A4384
	public override string GetNickName(int playerID)
	{
		NetPlayer player = this.GetPlayer(playerID);
		return this.GetNickName(player);
	}

	// Token: 0x06000EED RID: 3821 RVA: 0x000A61A0 File Offset: 0x000A43A0
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

	// Token: 0x06000EEE RID: 3822 RVA: 0x0003A74A File Offset: 0x0003894A
	public override string GetMyUserID()
	{
		return this.runner.GetPlayerUserId(this.runner.LocalPlayer);
	}

	// Token: 0x06000EEF RID: 3823 RVA: 0x0003A762 File Offset: 0x00038962
	public override string GetUserID(int playerID)
	{
		if (this.runner == null)
		{
			return string.Empty;
		}
		return this.runner.GetPlayerUserId(this.GetPlayerRef(playerID));
	}

	// Token: 0x06000EF0 RID: 3824 RVA: 0x0003A78A File Offset: 0x0003898A
	public override string GetUserID(NetPlayer player)
	{
		if (this.runner == null)
		{
			return string.Empty;
		}
		return this.runner.GetPlayerUserId(((FusionNetPlayer)player).PlayerRef);
	}

	// Token: 0x06000EF1 RID: 3825 RVA: 0x0003A7B6 File Offset: 0x000389B6
	public override void SetMyTutorialComplete()
	{
		if (!(PlayerPrefs.GetString("didTutorial", "nope") == "done"))
		{
			PlayerPrefs.SetString("didTutorial", "done");
			PlayerPrefs.Save();
		}
	}

	// Token: 0x06000EF2 RID: 3826 RVA: 0x0003A7E7 File Offset: 0x000389E7
	public override bool GetMyTutorialCompletion()
	{
		return PlayerPrefs.GetString("didTutorial", "nope") == "done";
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x000A6240 File Offset: 0x000A4440
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

	// Token: 0x06000EF4 RID: 3828 RVA: 0x0003A802 File Offset: 0x00038A02
	public override int GlobalPlayerCount()
	{
		if (this.regionCrawler == null)
		{
			return 0;
		}
		return this.regionCrawler.PlayerCountGlobal;
	}

	// Token: 0x06000EF5 RID: 3829 RVA: 0x000A62B8 File Offset: 0x000A44B8
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

	// Token: 0x06000EF6 RID: 3830 RVA: 0x000A62FC File Offset: 0x000A44FC
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

	// Token: 0x06000EF7 RID: 3831 RVA: 0x0003A81F File Offset: 0x00038A1F
	public override bool IsTotalAuthority()
	{
		return this.runner.Mode == SimulationModes.Server || this.runner.Mode == SimulationModes.Host || this.runner.GameMode == Fusion.GameMode.Single || this.runner.IsSharedModeMasterClient;
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x000A6350 File Offset: 0x000A4550
	public override bool ShouldWriteObjectData(GameObject obj)
	{
		NetworkObject networkObject;
		return obj.TryGetComponent<NetworkObject>(out networkObject) && networkObject.HasStateAuthority;
	}

	// Token: 0x06000EF9 RID: 3833 RVA: 0x000A6370 File Offset: 0x000A4570
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

	// Token: 0x06000EFA RID: 3834 RVA: 0x000A63D8 File Offset: 0x000A45D8
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

	// Token: 0x06000EFB RID: 3835 RVA: 0x000A63FC File Offset: 0x000A45FC
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

	// Token: 0x04001159 RID: 4441
	private NetworkSystemFusion.InternalState internalState;

	// Token: 0x0400115A RID: 4442
	private FusionInternalRPCs internalRPCProvider;

	// Token: 0x0400115B RID: 4443
	private FusionCallbackHandler callbackHandler;

	// Token: 0x0400115C RID: 4444
	private FusionRegionCrawler regionCrawler;

	// Token: 0x0400115D RID: 4445
	private GameObject volatileNetObj;

	// Token: 0x0400115E RID: 4446
	private Fusion.Photon.Realtime.AuthenticationValues cachedPlayfabAuth;

	// Token: 0x0400115F RID: 4447
	private const string playerPropertiesPath = "P_FusionProperties";

	// Token: 0x04001160 RID: 4448
	private bool lastConnectAttempt_WasFull;

	// Token: 0x04001161 RID: 4449
	private FusionVoiceBridge FusionVoiceBridge;

	// Token: 0x04001162 RID: 4450
	private VoiceConnection FusionVoice;

	// Token: 0x04001163 RID: 4451
	private CustomObjectProvider myObjectProvider;

	// Token: 0x04001164 RID: 4452
	private ObjectPool<FusionNetPlayer> playerPool;

	// Token: 0x04001165 RID: 4453
	public List<NetworkObject> cachedNetSceneObjects = new List<NetworkObject>();

	// Token: 0x04001166 RID: 4454
	private List<INetworkRunnerCallbacks> objectsThatNeedCallbacks = new List<INetworkRunnerCallbacks>();

	// Token: 0x04001167 RID: 4455
	private Queue<NetworkObject> registrationQueue = new Queue<NetworkObject>();

	// Token: 0x04001168 RID: 4456
	private bool isProcessingQueue;

	// Token: 0x02000272 RID: 626
	private enum InternalState
	{
		// Token: 0x0400116A RID: 4458
		AwaitingAuth,
		// Token: 0x0400116B RID: 4459
		Idle,
		// Token: 0x0400116C RID: 4460
		Searching_Joining,
		// Token: 0x0400116D RID: 4461
		Searching_Joined,
		// Token: 0x0400116E RID: 4462
		Searching_JoinFailed,
		// Token: 0x0400116F RID: 4463
		Searching_Disconnecting,
		// Token: 0x04001170 RID: 4464
		Searching_Disconnected,
		// Token: 0x04001171 RID: 4465
		ConnectingToRoom,
		// Token: 0x04001172 RID: 4466
		ConnectedToRoom,
		// Token: 0x04001173 RID: 4467
		JoinRoomFailed,
		// Token: 0x04001174 RID: 4468
		Disconnecting,
		// Token: 0x04001175 RID: 4469
		Disconnected,
		// Token: 0x04001176 RID: 4470
		StateCheckFailed
	}
}
