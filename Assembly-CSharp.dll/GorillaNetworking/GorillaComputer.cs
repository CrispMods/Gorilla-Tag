﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using GorillaGameModes;
using GorillaTagScripts;
using GorillaTagScripts.ModIO;
using KID.Model;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using PlayFab.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

namespace GorillaNetworking
{
	// Token: 0x02000AAA RID: 2730
	public class GorillaComputer : MonoBehaviour, IMatchmakingCallbacks, IGorillaSliceableSimple
	{
		// Token: 0x06004420 RID: 17440 RVA: 0x0005B998 File Offset: 0x00059B98
		public DateTime GetServerTime()
		{
			return this.startupTime + TimeSpan.FromSeconds((double)Time.realtimeSinceStartup);
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06004421 RID: 17441 RVA: 0x0005B9B0 File Offset: 0x00059BB0
		public string VStumpRoomPrepend
		{
			get
			{
				return this.virtualStumpRoomPrepend;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06004422 RID: 17442 RVA: 0x00179CA4 File Offset: 0x00177EA4
		public GorillaComputer.ComputerState currentState
		{
			get
			{
				GorillaComputer.ComputerState result;
				this.stateStack.TryPeek(out result);
				return result;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06004423 RID: 17443 RVA: 0x0005B9B8 File Offset: 0x00059BB8
		public string NameTagPlayerPref
		{
			get
			{
				if (PlayFabAuthenticator.instance == null)
				{
					Debug.LogError("Trying to access PlayFab Authenticator Instance, but it is null. Will use a shared key for the nametag instead");
					return "nameTagsOn";
				}
				return "nameTagsOn-" + PlayFabAuthenticator.instance.GetPlayFabPlayerId();
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06004424 RID: 17444 RVA: 0x0005B9EF File Offset: 0x00059BEF
		// (set) Token: 0x06004425 RID: 17445 RVA: 0x0005B9F7 File Offset: 0x00059BF7
		public bool NametagsEnabled { get; private set; }

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06004426 RID: 17446 RVA: 0x0005BA00 File Offset: 0x00059C00
		public TMP_Text offlineVRRigNametagText
		{
			get
			{
				return this._offlineVRRigNametagText[0];
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06004427 RID: 17447 RVA: 0x0005BA0A File Offset: 0x00059C0A
		// (set) Token: 0x06004428 RID: 17448 RVA: 0x0005BA12 File Offset: 0x00059C12
		public GorillaComputer.RedemptionResult RedemptionStatus
		{
			get
			{
				return this.redemptionResult;
			}
			set
			{
				this.redemptionResult = value;
				this.UpdateScreen();
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06004429 RID: 17449 RVA: 0x0005BA21 File Offset: 0x00059C21
		// (set) Token: 0x0600442A RID: 17450 RVA: 0x0005BA29 File Offset: 0x00059C29
		public string RedemptionCode
		{
			get
			{
				return this.redemptionCode;
			}
			set
			{
				this.redemptionCode = value;
			}
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x00179CC0 File Offset: 0x00177EC0
		private void Awake()
		{
			if (GorillaComputer.instance == null)
			{
				GorillaComputer.instance = this;
				GorillaComputer.hasInstance = true;
			}
			else if (GorillaComputer.instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			this._activeOrderList = this.OrderList;
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x0005BA32 File Offset: 0x00059C32
		private void Start()
		{
			Debug.Log("Computer Init");
			this.Initialise();
		}

		// Token: 0x0600442D RID: 17453 RVA: 0x0005BA44 File Offset: 0x00059C44
		public void OnEnable()
		{
			KIDManager.RegisterSessionUpdatedCallback_VoiceChat(new Action<bool, Permission.ManagedByEnum>(this.SetVoiceChatBySafety));
			KIDManager.RegisterSessionUpdatedCallback_CustomUsernames(new Action<bool, Permission.ManagedByEnum>(this.OnKIDSessionUpdated_CustomNicknames));
			GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x0600442E RID: 17454 RVA: 0x0005BA6F File Offset: 0x00059C6F
		public void OnDisable()
		{
			KIDManager.UnregisterSessionUpdatedCallback_VoiceChat(new Action<bool, Permission.ManagedByEnum>(this.SetVoiceChatBySafety));
			KIDManager.UnregisterSessionUpdatedCallback_CustomUsernames(new Action<bool, Permission.ManagedByEnum>(this.OnKIDSessionUpdated_CustomNicknames));
			GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x0005BA9A File Offset: 0x00059C9A
		protected void OnDestroy()
		{
			if (GorillaComputer.instance == this)
			{
				GorillaComputer.hasInstance = false;
				GorillaComputer.instance = null;
			}
			KIDManager.UnregisterSessionUpdateCallback_AnyPermission(new Action(this.OnSessionUpdate_GorillaComputer));
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x00179D14 File Offset: 0x00177F14
		public void SliceUpdate()
		{
			if ((this.internetFailure && Time.time < this.lastCheckedWifi + this.checkIfConnectedSeconds) || (!this.internetFailure && Time.time < this.lastCheckedWifi + this.checkIfDisconnectedSeconds))
			{
				return;
			}
			this.lastCheckedWifi = Time.time;
			this.stateUpdated = false;
			if (!this.CheckInternetConnection())
			{
				this.UpdateFailureText("NO WIFI OR LAN CONNECTION DETECTED.");
				this.internetFailure = true;
				return;
			}
			if (this.internetFailure)
			{
				if (this.CheckInternetConnection())
				{
					this.internetFailure = false;
				}
				this.RestoreFromFailureState();
				this.UpdateScreen();
				return;
			}
			if (this.isConnectedToMaster && Time.time > this.lastUpdateTime + this.updateCooldown)
			{
				this.lastUpdateTime = Time.time;
				this.UpdateScreen();
			}
		}

		// Token: 0x06004431 RID: 17457 RVA: 0x00179DD8 File Offset: 0x00177FD8
		private void Initialise()
		{
			GameEvents.OnGorrillaKeyboardButtonPressedEvent.AddListener(new UnityAction<GorillaKeyboardBindings>(this.PressButton));
			RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.UpdateScreen));
			RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.UpdateScreen));
			RoomSystem.PlayerJoinedEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerJoinedEvent, new Action<NetPlayer>(this.PlayerCountChangedCallback));
			RoomSystem.PlayerLeftEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerLeftEvent, new Action<NetPlayer>(this.PlayerCountChangedCallback));
			this.InitialiseRoomScreens();
			this.InitialiseStrings();
			this.InitialiseAllRoomStates();
			this.UpdateScreen();
			byte[] bytes = new byte[]
			{
				Convert.ToByte(64)
			};
			this.virtualStumpRoomPrepend = Encoding.ASCII.GetString(bytes);
			this.initialized = true;
		}

		// Token: 0x06004432 RID: 17458 RVA: 0x00179EBC File Offset: 0x001780BC
		private void InitialiseRoomScreens()
		{
			this.screenText.Initialize(this.computerScreenRenderer.materials, this.wrongVersionMaterial, GameEvents.ScreenTextChangedEvent, GameEvents.ScreenTextMaterialsEvent);
			this.functionSelectText.Initialize(this.computerScreenRenderer.materials, this.wrongVersionMaterial, GameEvents.FunctionSelectTextChangedEvent, null);
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x00179F14 File Offset: 0x00178114
		private void InitialiseStrings()
		{
			this.roomToJoin = "";
			this.redText = "";
			this.blueText = "";
			this.greenText = "";
			this.currentName = "";
			this.savedName = "";
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x00179F64 File Offset: 0x00178164
		private void InitialiseAllRoomStates()
		{
			this.SwitchState(GorillaComputer.ComputerState.Startup, true);
			this.InitializeNameState();
			this.InitializeRoomState();
			this.InitializeTurnState();
			this.InitializeStartupState();
			this.InitializeQueueState();
			this.InitializeMicState();
			this.InitializeGroupState();
			this.InitializeVoiceState();
			this.InitializeAutoMuteState();
			this.InitializeGameMode();
			this.InitializeVisualsState();
			this.InitializeCreditsState();
			this.InitializeTimeState();
			this.InitializeSupportState();
			this.InitializeTroopState();
			this.InitializeKIdState();
			this.InitializeRedeemState();
		}

		// Token: 0x06004435 RID: 17461 RVA: 0x0002F75F File Offset: 0x0002D95F
		private void InitializeStartupState()
		{
		}

		// Token: 0x06004436 RID: 17462 RVA: 0x0002F75F File Offset: 0x0002D95F
		private void InitializeRoomState()
		{
		}

		// Token: 0x06004437 RID: 17463 RVA: 0x00179FE0 File Offset: 0x001781E0
		private void InitializeColorState()
		{
			this.redValue = PlayerPrefs.GetFloat("redValue", 0f);
			this.greenValue = PlayerPrefs.GetFloat("greenValue", 0f);
			this.blueValue = PlayerPrefs.GetFloat("blueValue", 0f);
			this.blueText = Mathf.Floor(this.blueValue * 9f).ToString();
			this.redText = Mathf.Floor(this.redValue * 9f).ToString();
			this.greenText = Mathf.Floor(this.greenValue * 9f).ToString();
			this.colorCursorLine = 0;
			GorillaTagger.Instance.UpdateColor(this.redValue, this.greenValue, this.blueValue);
		}

		// Token: 0x06004438 RID: 17464 RVA: 0x0017A0AC File Offset: 0x001782AC
		private void InitializeNameState()
		{
			int @int = PlayerPrefs.GetInt("nameTagsOn", -1);
			ValueTuple<bool, Permission.ManagedByEnum> customNicknamePermissionStatus = KIDManager.Instance.GetCustomNicknamePermissionStatus();
			switch (customNicknamePermissionStatus.Item2)
			{
			case Permission.ManagedByEnum.PLAYER:
				if (@int == -1)
				{
					this.NametagsEnabled = customNicknamePermissionStatus.Item1;
				}
				else
				{
					this.NametagsEnabled = (@int > 0);
				}
				break;
			case Permission.ManagedByEnum.GUARDIAN:
				this.NametagsEnabled = (customNicknamePermissionStatus.Item1 && @int > 0);
				break;
			case Permission.ManagedByEnum.PROHIBITED:
				this.NametagsEnabled = false;
				break;
			}
			this.savedName = PlayerPrefs.GetString("playerName", "gorilla");
			NetworkSystem.Instance.SetMyNickName(this.savedName);
			this.currentName = this.savedName;
			VRRigCache.Instance.localRig.Rig.UpdateName();
			this.exactOneWeek = this.exactOneWeekFile.text.Split('\n', StringSplitOptions.None);
			this.anywhereOneWeek = this.anywhereOneWeekFile.text.Split('\n', StringSplitOptions.None);
			this.anywhereTwoWeek = this.anywhereTwoWeekFile.text.Split('\n', StringSplitOptions.None);
			for (int i = 0; i < this.exactOneWeek.Length; i++)
			{
				this.exactOneWeek[i] = this.exactOneWeek[i].ToLower().TrimEnd(new char[]
				{
					'\r',
					'\n'
				});
			}
			for (int j = 0; j < this.anywhereOneWeek.Length; j++)
			{
				this.anywhereOneWeek[j] = this.anywhereOneWeek[j].ToLower().TrimEnd(new char[]
				{
					'\r',
					'\n'
				});
			}
			for (int k = 0; k < this.anywhereTwoWeek.Length; k++)
			{
				this.anywhereTwoWeek[k] = this.anywhereTwoWeek[k].ToLower().TrimEnd(new char[]
				{
					'\r',
					'\n'
				});
			}
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x0017A27C File Offset: 0x0017847C
		private void InitializeTurnState()
		{
			this.gorillaTurn = GorillaTagger.Instance.GetComponent<GorillaSnapTurn>();
			string defaultValue = (Application.platform == RuntimePlatform.Android) ? "NONE" : "SNAP";
			this.turnType = PlayerPrefs.GetString("stickTurning", defaultValue);
			this.turnValue = PlayerPrefs.GetInt("turnFactor", 4);
			this.gorillaTurn.ChangeTurnMode(this.turnType, this.turnValue);
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x0005BACA File Offset: 0x00059CCA
		private void InitializeMicState()
		{
			this.pttType = PlayerPrefs.GetString("pttType", "ALL CHAT");
		}

		// Token: 0x0600443B RID: 17467 RVA: 0x0017A2E8 File Offset: 0x001784E8
		private void InitializeAutoMuteState()
		{
			int @int = PlayerPrefs.GetInt("autoMute", 1);
			if (@int == 0)
			{
				this.autoMuteType = "OFF";
				return;
			}
			if (@int == 1)
			{
				this.autoMuteType = "MODERATE";
				return;
			}
			if (@int == 2)
			{
				this.autoMuteType = "AGGRESSIVE";
			}
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x0017A330 File Offset: 0x00178530
		private void InitializeQueueState()
		{
			this.currentQueue = PlayerPrefs.GetString("currentQueue", "DEFAULT");
			this.allowedInCompetitive = (PlayerPrefs.GetInt("allowedInCompetitive", 0) == 1);
			if (!this.allowedInCompetitive && this.currentQueue == "COMPETITIVE")
			{
				PlayerPrefs.SetString("currentQueue", "DEFAULT");
				PlayerPrefs.Save();
				this.currentQueue = "DEFAULT";
			}
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x0005BAE1 File Offset: 0x00059CE1
		private void InitializeGroupState()
		{
			this.groupMapJoin = PlayerPrefs.GetString("groupMapJoin", "FOREST");
			this.groupMapJoinIndex = PlayerPrefs.GetInt("groupMapJoinIndex", 0);
			this.allowedMapsToJoin = this.friendJoinCollider.myAllowedMapsToJoin;
		}

		// Token: 0x0600443E RID: 17470 RVA: 0x0017A3A0 File Offset: 0x001785A0
		private void InitializeTroopState()
		{
			bool flag = false;
			this.troopToJoin = (this.troopName = PlayerPrefs.GetString("troopName", string.Empty));
			this.troopQueueActive = (PlayerPrefs.GetInt("troopQueueActive", 0) == 1);
			if (this.troopQueueActive && !this.IsValidTroopName(this.troopName))
			{
				this.troopQueueActive = false;
				PlayerPrefs.SetInt("troopQueueActive", this.troopQueueActive ? 1 : 0);
				this.currentQueue = "DEFAULT";
				PlayerPrefs.SetString("currentQueue", this.currentQueue);
				flag = true;
			}
			if (this.troopQueueActive)
			{
				base.StartCoroutine(this.HandleInitialTroopQueueState());
			}
			if (flag)
			{
				PlayerPrefs.Save();
			}
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x0005BB1A File Offset: 0x00059D1A
		private IEnumerator HandleInitialTroopQueueState()
		{
			Debug.Log("HandleInitialTroopQueueState()");
			while (!PlayFabCloudScriptAPI.IsEntityLoggedIn())
			{
				yield return null;
			}
			this.RequestTroopPopulation(false);
			while (this.currentTroopPopulation < 0)
			{
				yield return null;
			}
			if (this.currentTroopPopulation < 2)
			{
				Debug.Log("Low population - starting in DEFAULT queue");
				this.JoinDefaultQueue();
			}
			yield break;
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x0017A450 File Offset: 0x00178650
		private void InitializeVoiceState()
		{
			ValueTuple<bool, Permission.ManagedByEnum> voiceChatPermissionStatus = KIDManager.Instance.GetVoiceChatPermissionStatus();
			string text = PlayerPrefs.GetString("voiceChatOn", "");
			string defaultValue = "FALSE";
			switch (voiceChatPermissionStatus.Item2)
			{
			case Permission.ManagedByEnum.PLAYER:
				if (string.IsNullOrEmpty(text))
				{
					defaultValue = (voiceChatPermissionStatus.Item1 ? "TRUE" : "FALSE");
				}
				else
				{
					defaultValue = text;
				}
				break;
			case Permission.ManagedByEnum.GUARDIAN:
				if (voiceChatPermissionStatus.Item1)
				{
					text = (string.IsNullOrEmpty(text) ? "FALSE" : text);
					defaultValue = text;
				}
				else
				{
					defaultValue = "FALSE";
				}
				break;
			case Permission.ManagedByEnum.PROHIBITED:
				defaultValue = "FALSE";
				break;
			}
			this.voiceChatOn = PlayerPrefs.GetString("voiceChatOn", defaultValue);
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x0017A4FC File Offset: 0x001786FC
		private void InitializeGameMode()
		{
			string text = PlayerPrefs.GetString("currentGameMode", GameModeType.Infection.ToString());
			GameModeType gameModeType;
			try
			{
				gameModeType = Enum.Parse<GameModeType>(text, true);
			}
			catch
			{
				gameModeType = GameModeType.Infection;
				text = GameModeType.Infection.ToString();
			}
			if (gameModeType != GameModeType.Casual && gameModeType != GameModeType.Infection && gameModeType != GameModeType.Hunt && gameModeType != GameModeType.Paintbrawl && gameModeType != GameModeType.Ambush)
			{
				PlayerPrefs.SetString("currentGameMode", GameModeType.Infection.ToString());
				PlayerPrefs.Save();
				text = GameModeType.Infection.ToString();
			}
			this.leftHanded = (PlayerPrefs.GetInt("leftHanded", 0) == 1);
			this.OnModeSelectButtonPress(text, this.leftHanded);
			GameModePages.SetSelectedGameModeShared(text);
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x0002F75F File Offset: 0x0002D95F
		private void InitializeCreditsState()
		{
		}

		// Token: 0x06004443 RID: 17475 RVA: 0x0005BB29 File Offset: 0x00059D29
		private void InitializeTimeState()
		{
			BetterDayNightManager.instance.currentSetting = TimeSettings.Normal;
		}

		// Token: 0x06004444 RID: 17476 RVA: 0x0005BB38 File Offset: 0x00059D38
		private void InitializeSupportState()
		{
			this.displaySupport = false;
		}

		// Token: 0x06004445 RID: 17477 RVA: 0x0017A5BC File Offset: 0x001787BC
		private void InitializeVisualsState()
		{
			this.disableParticles = (PlayerPrefs.GetString("disableParticles", "FALSE") == "TRUE");
			GorillaTagger.Instance.ShowCosmeticParticles(!this.disableParticles);
			this.instrumentVolume = PlayerPrefs.GetFloat("instrumentVolume", 0.1f);
		}

		// Token: 0x06004446 RID: 17478 RVA: 0x0005BB41 File Offset: 0x00059D41
		private void InitializeRedeemState()
		{
			this.RedemptionStatus = GorillaComputer.RedemptionResult.Empty;
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x0005BB4A File Offset: 0x00059D4A
		private bool CheckInternetConnection()
		{
			return Application.internetReachability > NetworkReachability.NotReachable;
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x0017A610 File Offset: 0x00178810
		public void OnConnectedToMasterStuff()
		{
			if (!this.isConnectedToMaster)
			{
				this.isConnectedToMaster = true;
				GorillaServer.Instance.ReturnCurrentVersion(new ReturnCurrentVersionRequest
				{
					CurrentVersion = NetworkSystemConfig.AppVersionStripped,
					UpdatedSynchTest = new int?(this.includeUpdatedServerSynchTest)
				}, new Action<ExecuteFunctionResult>(this.OnReturnCurrentVersion), new Action<PlayFabError>(GorillaComputer.OnErrorShared));
				if (this.startupMillis == 0L && !this.tryGetTimeAgain)
				{
					this.GetCurrentTime();
				}
				RuntimePlatform platform = Application.platform;
				this.SaveModAccountData();
				bool safety = PlayFabAuthenticator.instance.GetSafety();
				if (!KIDIntegration.IsReady && !KIDIntegration.HasFoundSession && safety)
				{
					this.SetComputerSettingsBySafety(safety, new GorillaComputer.ComputerState[]
					{
						GorillaComputer.ComputerState.Voice,
						GorillaComputer.ComputerState.AutoMute,
						GorillaComputer.ComputerState.Name,
						GorillaComputer.ComputerState.Group
					}, false);
				}
			}
		}

		// Token: 0x06004449 RID: 17481 RVA: 0x0017A6D8 File Offset: 0x001788D8
		private void OnReturnCurrentVersion(ExecuteFunctionResult result)
		{
			JsonObject jsonObject = (JsonObject)result.FunctionResult;
			if (jsonObject == null)
			{
				this.GeneralFailureMessage(this.versionMismatch);
				return;
			}
			object obj;
			if (jsonObject.TryGetValue("SynchTime", out obj))
			{
				Debug.Log("message value is: " + (string)obj);
			}
			if (jsonObject.TryGetValue("Fail", out obj) && (bool)obj)
			{
				this.GeneralFailureMessage(this.versionMismatch);
				return;
			}
			if (jsonObject.TryGetValue("ResultCode", out obj) && (ulong)obj != 0UL)
			{
				this.GeneralFailureMessage(this.versionMismatch);
				return;
			}
			if (jsonObject.TryGetValue("QueueStats", out obj) && ((JsonObject)obj).TryGetValue("TopTroops", out obj))
			{
				this.topTroops.Clear();
				foreach (object obj2 in ((JsonArray)obj))
				{
					this.topTroops.Add(obj2.ToString());
				}
			}
			if (jsonObject.TryGetValue("BannedUsers", out obj))
			{
				this.usersBanned = int.Parse((string)obj);
			}
			this.UpdateScreen();
		}

		// Token: 0x0600444A RID: 17482 RVA: 0x0017A814 File Offset: 0x00178A14
		public void SaveModAccountData()
		{
			string path = Application.persistentDataPath + "/DoNotShareWithAnyoneEVERNoMatterWhatTheySay.txt";
			if (File.Exists(path))
			{
				return;
			}
			GorillaServer.Instance.ReturnMyOculusHash(delegate(ExecuteFunctionResult result)
			{
				object obj;
				if (((JsonObject)result.FunctionResult).TryGetValue("oculusHash", out obj))
				{
					StreamWriter streamWriter = new StreamWriter(path);
					streamWriter.Write(PlayFabAuthenticator.instance.GetPlayFabPlayerId() + "." + (string)obj);
					streamWriter.Close();
				}
			}, delegate(PlayFabError error)
			{
				if (error.Error == PlayFabErrorCode.NotAuthenticated)
				{
					PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
					return;
				}
				if (error.Error == PlayFabErrorCode.AccountBanned)
				{
					GorillaGameManager.ForceStopGame_DisconnectAndDestroy();
				}
			});
		}

		// Token: 0x0600444B RID: 17483 RVA: 0x0017A884 File Offset: 0x00178A84
		public void PressButton(GorillaKeyboardBindings buttonPressed)
		{
			if (this.currentState == GorillaComputer.ComputerState.Startup)
			{
				this.ProcessStartupState(buttonPressed);
				this.UpdateScreen();
				return;
			}
			this.RequestTroopPopulation(false);
			bool flag = true;
			if (buttonPressed == GorillaKeyboardBindings.up)
			{
				flag = false;
				this.DecreaseState();
			}
			else if (buttonPressed == GorillaKeyboardBindings.down)
			{
				flag = false;
				this.IncreaseState();
			}
			if (flag)
			{
				switch (this.currentState)
				{
				case GorillaComputer.ComputerState.Color:
					this.ProcessColorState(buttonPressed);
					break;
				case GorillaComputer.ComputerState.Name:
					this.ProcessNameState(buttonPressed);
					break;
				case GorillaComputer.ComputerState.Turn:
					this.ProcessTurnState(buttonPressed);
					break;
				case GorillaComputer.ComputerState.Mic:
					this.ProcessMicState(buttonPressed);
					break;
				case GorillaComputer.ComputerState.Room:
					this.ProcessRoomState(buttonPressed);
					break;
				case GorillaComputer.ComputerState.Queue:
					this.ProcessQueueState(buttonPressed);
					break;
				case GorillaComputer.ComputerState.Group:
					this.ProcessGroupState(buttonPressed);
					break;
				case GorillaComputer.ComputerState.Voice:
					this.ProcessVoiceState(buttonPressed);
					break;
				case GorillaComputer.ComputerState.AutoMute:
					this.ProcessAutoMuteState(buttonPressed);
					break;
				case GorillaComputer.ComputerState.Credits:
					this.ProcessCreditsState(buttonPressed);
					break;
				case GorillaComputer.ComputerState.Visuals:
					this.ProcessVisualsState(buttonPressed);
					break;
				case GorillaComputer.ComputerState.NameWarning:
					this.ProcessNameWarningState(buttonPressed);
					break;
				case GorillaComputer.ComputerState.Support:
					this.ProcessSupportState(buttonPressed);
					break;
				case GorillaComputer.ComputerState.Troop:
					this.ProcessTroopState(buttonPressed);
					break;
				case GorillaComputer.ComputerState.KID:
					this.ProcessKIdState(buttonPressed);
					break;
				case GorillaComputer.ComputerState.Redemption:
					this.ProcessRedemptionState(buttonPressed);
					break;
				}
			}
			this.UpdateScreen();
		}

		// Token: 0x0600444C RID: 17484 RVA: 0x0017A9C8 File Offset: 0x00178BC8
		public void OnModeSelectButtonPress(string gameMode, bool leftHand)
		{
			this.lastPressedGameMode = gameMode;
			PlayerPrefs.SetString("currentGameMode", gameMode);
			if (leftHand != this.leftHanded)
			{
				PlayerPrefs.SetInt("leftHanded", leftHand ? 1 : 0);
				this.leftHanded = leftHand;
			}
			PlayerPrefs.Save();
			if (FriendshipGroupDetection.Instance.IsInParty)
			{
				FriendshipGroupDetection.Instance.SendRequestPartyGameMode(gameMode);
				return;
			}
			this.SetGameModeWithoutButton(gameMode);
		}

		// Token: 0x0600444D RID: 17485 RVA: 0x0005BB54 File Offset: 0x00059D54
		public void SetGameModeWithoutButton(string gameMode)
		{
			this.currentGameMode.Value = gameMode;
			this.UpdateGameModeText();
			PhotonNetworkController.Instance.UpdateTriggerScreens();
		}

		// Token: 0x0600444E RID: 17486 RVA: 0x0005BB74 File Offset: 0x00059D74
		public void RegisterPrimaryJoinTrigger(GorillaNetworkJoinTrigger trigger)
		{
			this.primaryTriggersByZone[trigger.networkZone] = trigger;
		}

		// Token: 0x0600444F RID: 17487 RVA: 0x0017AA2C File Offset: 0x00178C2C
		private GorillaNetworkJoinTrigger GetSelectedMapJoinTrigger()
		{
			GorillaNetworkJoinTrigger result;
			this.primaryTriggersByZone.TryGetValue(this.allowedMapsToJoin[Mathf.Min(this.allowedMapsToJoin.Length - 1, this.groupMapJoinIndex)], out result);
			return result;
		}

		// Token: 0x06004450 RID: 17488 RVA: 0x0017AA64 File Offset: 0x00178C64
		public GorillaNetworkJoinTrigger GetJoinTriggerForZone(string zone)
		{
			GorillaNetworkJoinTrigger result;
			this.primaryTriggersByZone.TryGetValue(zone, out result);
			return result;
		}

		// Token: 0x06004451 RID: 17489 RVA: 0x0017AA84 File Offset: 0x00178C84
		public GorillaNetworkJoinTrigger GetJoinTriggerFromFullGameModeString(string gameModeString)
		{
			foreach (KeyValuePair<string, GorillaNetworkJoinTrigger> keyValuePair in this.primaryTriggersByZone)
			{
				if (gameModeString.StartsWith(keyValuePair.Key))
				{
					return keyValuePair.Value;
				}
			}
			return null;
		}

		// Token: 0x06004452 RID: 17490 RVA: 0x0017AAEC File Offset: 0x00178CEC
		public void OnGroupJoinButtonPress(int mapJoinIndex, GorillaFriendCollider chosenFriendJoinCollider)
		{
			Debug.Log("On Group button press. Map:" + mapJoinIndex.ToString() + " - collider: " + chosenFriendJoinCollider.name);
			if (mapJoinIndex >= this.allowedMapsToJoin.Length)
			{
				this.roomNotAllowed = true;
				this.currentStateIndex = 0;
				this.SwitchState(this.GetState(this.currentStateIndex), true);
				return;
			}
			GorillaNetworkJoinTrigger selectedMapJoinTrigger = this.GetSelectedMapJoinTrigger();
			if (!FriendshipGroupDetection.Instance.IsInParty)
			{
				if (NetworkSystem.Instance.InRoom && NetworkSystem.Instance.SessionIsPrivate)
				{
					PhotonNetworkController.Instance.friendIDList = new List<string>(chosenFriendJoinCollider.playerIDsCurrentlyTouching);
					foreach (string str in this.networkController.friendIDList)
					{
						Debug.Log("Friend ID:" + str);
					}
					PhotonNetworkController.Instance.shuffler = UnityEngine.Random.Range(0, 99).ToString().PadLeft(2, '0') + UnityEngine.Random.Range(0, 99999999).ToString().PadLeft(8, '0');
					PhotonNetworkController.Instance.keyStr = UnityEngine.Random.Range(0, 99999999).ToString().PadLeft(8, '0');
					RoomSystem.SendNearbyFollowCommand(chosenFriendJoinCollider, PhotonNetworkController.Instance.shuffler, PhotonNetworkController.Instance.keyStr);
					PhotonNetwork.SendAllOutgoingCommands();
					PhotonNetworkController.Instance.AttemptToJoinPublicRoom(selectedMapJoinTrigger, JoinType.JoinWithNearby);
					this.currentStateIndex = 0;
					this.SwitchState(this.GetState(this.currentStateIndex), true);
				}
				return;
			}
			if (selectedMapJoinTrigger != null && selectedMapJoinTrigger.CanPartyJoin())
			{
				PhotonNetworkController.Instance.AttemptToJoinPublicRoom(selectedMapJoinTrigger, JoinType.ForceJoinWithParty);
				this.currentStateIndex = 0;
				this.SwitchState(this.GetState(this.currentStateIndex), true);
				return;
			}
			this.UpdateScreen();
		}

		// Token: 0x06004453 RID: 17491 RVA: 0x0005BB88 File Offset: 0x00059D88
		public void CompQueueUnlockButtonPress()
		{
			this.allowedInCompetitive = true;
			PlayerPrefs.SetInt("allowedInCompetitive", 1);
			PlayerPrefs.Save();
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x0017ACDC File Offset: 0x00178EDC
		private void SwitchState(GorillaComputer.ComputerState newState, bool clearStack = true)
		{
			if (this.previousComputerState != this.currentComputerState)
			{
				this.previousComputerState = this.currentComputerState;
			}
			this.currentComputerState = newState;
			if (this.LoadingRoutine != null)
			{
				base.StopCoroutine(this.LoadingRoutine);
			}
			if (clearStack)
			{
				this.stateStack.Clear();
			}
			this.stateStack.Push(newState);
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x0005BBA1 File Offset: 0x00059DA1
		private void PopState()
		{
			this.currentComputerState = this.previousComputerState;
			if (this.stateStack.Count <= 1)
			{
				Debug.LogError("Can't pop into an empty stack");
				return;
			}
			this.stateStack.Pop();
			this.UpdateScreen();
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x0005BBDA File Offset: 0x00059DDA
		private void SwitchToWarningState()
		{
			this.warningConfirmationInputString = string.Empty;
			this.SwitchState(GorillaComputer.ComputerState.NameWarning, false);
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x0005BBF0 File Offset: 0x00059DF0
		private void SwitchToLoadingState()
		{
			this.SwitchState(GorillaComputer.ComputerState.Loading, false);
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x0005BBFB File Offset: 0x00059DFB
		private void ProcessStartupState(GorillaKeyboardBindings buttonPressed)
		{
			this.SwitchState(this.GetState(this.currentStateIndex), true);
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x0017AD38 File Offset: 0x00178F38
		private void ProcessColorState(GorillaKeyboardBindings buttonPressed)
		{
			switch (buttonPressed)
			{
			case GorillaKeyboardBindings.enter:
				return;
			case GorillaKeyboardBindings.option1:
				this.colorCursorLine = 0;
				return;
			case GorillaKeyboardBindings.option2:
				this.colorCursorLine = 1;
				return;
			case GorillaKeyboardBindings.option3:
				this.colorCursorLine = 2;
				return;
			default:
			{
				int num = (int)buttonPressed;
				if (num < 10)
				{
					switch (this.colorCursorLine)
					{
					case 0:
						this.redText = num.ToString();
						this.redValue = (float)num / 9f;
						PlayerPrefs.SetFloat("redValue", this.redValue);
						break;
					case 1:
						this.greenText = num.ToString();
						this.greenValue = (float)num / 9f;
						PlayerPrefs.SetFloat("greenValue", this.greenValue);
						break;
					case 2:
						this.blueText = num.ToString();
						this.blueValue = (float)num / 9f;
						PlayerPrefs.SetFloat("blueValue", this.blueValue);
						break;
					}
					GorillaTagger.Instance.UpdateColor(this.redValue, this.greenValue, this.blueValue);
					PlayerPrefs.Save();
					if (NetworkSystem.Instance.InRoom)
					{
						GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", RpcTarget.All, new object[]
						{
							this.redValue,
							this.greenValue,
							this.blueValue
						});
					}
				}
				return;
			}
			}
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x0017AE98 File Offset: 0x00179098
		public void ProcessNameState(GorillaKeyboardBindings buttonPressed)
		{
			ValueTuple<bool, Permission.ManagedByEnum> customNicknamePermissionStatus = KIDManager.Instance.GetCustomNicknamePermissionStatus();
			if ((customNicknamePermissionStatus.Item1 || customNicknamePermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && customNicknamePermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED)
			{
				switch (buttonPressed)
				{
				case GorillaKeyboardBindings.delete:
					if (this.currentName.Length > 0 && this.NametagsEnabled)
					{
						this.currentName = this.currentName.Substring(0, this.currentName.Length - 1);
						return;
					}
					break;
				case GorillaKeyboardBindings.enter:
					if (this.currentName != this.savedName && this.currentName != "" && this.NametagsEnabled)
					{
						this.CheckAutoBanListForPlayerName(this.currentName);
						return;
					}
					break;
				case GorillaKeyboardBindings.option1:
					this.UpdateNametagSetting(!this.NametagsEnabled);
					return;
				default:
					if (this.NametagsEnabled && this.currentName.Length < 12 && (buttonPressed < GorillaKeyboardBindings.up || buttonPressed > GorillaKeyboardBindings.option3))
					{
						string str = this.currentName;
						string str2;
						if (buttonPressed >= GorillaKeyboardBindings.up)
						{
							str2 = buttonPressed.ToString();
						}
						else
						{
							int num = (int)buttonPressed;
							str2 = num.ToString();
						}
						this.currentName = str + str2;
						return;
					}
					break;
				}
			}
			else if (buttonPressed != GorillaKeyboardBindings.option2)
			{
				if (buttonPressed != GorillaKeyboardBindings.option3)
				{
					return;
				}
				if (this._currentScreentState != GorillaComputer.EKidScreenState.Show_OTP)
				{
					return;
				}
				this.ProcessScreen_SetupKID();
			}
			else
			{
				if (this._currentScreentState != GorillaComputer.EKidScreenState.Ready)
				{
					this.ProcessScreen_SetupKID();
					return;
				}
				this.RequestUpdatedPermissions();
				return;
			}
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x0017B000 File Offset: 0x00179200
		private void ProcessRoomState(GorillaKeyboardBindings buttonPressed)
		{
			ValueTuple<bool, Permission.ManagedByEnum> privateRoomPermissionStatus = KIDManager.Instance.GetPrivateRoomPermissionStatus();
			bool flag = (privateRoomPermissionStatus.Item1 || privateRoomPermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && privateRoomPermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED;
			switch (buttonPressed)
			{
			case GorillaKeyboardBindings.delete:
				if (flag && ((this.playerInVirtualStump && this.roomToJoin.Length > 1) || (!this.playerInVirtualStump && this.roomToJoin.Length > 0)))
				{
					this.roomToJoin = this.roomToJoin.Substring(0, this.roomToJoin.Length - 1);
					return;
				}
				break;
			case GorillaKeyboardBindings.enter:
				if (flag && ((!this.playerInVirtualStump && this.roomToJoin != "") || (this.playerInVirtualStump && this.roomToJoin.Length > 1)))
				{
					this.CheckAutoBanListForRoomName(this.roomToJoin);
					return;
				}
				break;
			case GorillaKeyboardBindings.option1:
				if (FriendshipGroupDetection.Instance.IsInParty)
				{
					FriendshipGroupDetection.Instance.LeaveParty();
					this.DisconnectAfterDelay(1f);
					return;
				}
				NetworkSystem.Instance.ReturnToSinglePlayer();
				return;
			case GorillaKeyboardBindings.option2:
				if (this._currentScreentState != GorillaComputer.EKidScreenState.Ready)
				{
					this.ProcessScreen_SetupKID();
					return;
				}
				this.RequestUpdatedPermissions();
				return;
			case GorillaKeyboardBindings.option3:
				if (this._currentScreentState != GorillaComputer.EKidScreenState.Show_OTP)
				{
					return;
				}
				this.ProcessScreen_SetupKID();
				return;
			default:
				if (flag && this.roomToJoin.Length < 10)
				{
					string str = this.roomToJoin;
					string str2;
					if (buttonPressed >= GorillaKeyboardBindings.up)
					{
						str2 = buttonPressed.ToString();
					}
					else
					{
						int num = (int)buttonPressed;
						str2 = num.ToString();
					}
					this.roomToJoin = str + str2;
				}
				break;
			}
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x0017B18C File Offset: 0x0017938C
		private void DisconnectAfterDelay(float seconds)
		{
			GorillaComputer.<DisconnectAfterDelay>d__173 <DisconnectAfterDelay>d__;
			<DisconnectAfterDelay>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<DisconnectAfterDelay>d__.seconds = seconds;
			<DisconnectAfterDelay>d__.<>1__state = -1;
			<DisconnectAfterDelay>d__.<>t__builder.Start<GorillaComputer.<DisconnectAfterDelay>d__173>(ref <DisconnectAfterDelay>d__);
		}

		// Token: 0x0600445D RID: 17501 RVA: 0x0017B1C4 File Offset: 0x001793C4
		private void ProcessTurnState(GorillaKeyboardBindings buttonPressed)
		{
			if (buttonPressed < GorillaKeyboardBindings.up)
			{
				this.turnValue = (int)buttonPressed;
				PlayerPrefs.SetInt("turnFactor", this.turnValue);
				PlayerPrefs.Save();
				this.gorillaTurn.ChangeTurnMode(this.turnType, this.turnValue);
				return;
			}
			switch (buttonPressed)
			{
			case GorillaKeyboardBindings.option1:
				this.turnType = "SNAP";
				PlayerPrefs.SetString("stickTurning", this.turnType);
				PlayerPrefs.Save();
				this.gorillaTurn.ChangeTurnMode(this.turnType, this.turnValue);
				return;
			case GorillaKeyboardBindings.option2:
				this.turnType = "SMOOTH";
				PlayerPrefs.SetString("stickTurning", this.turnType);
				PlayerPrefs.Save();
				this.gorillaTurn.ChangeTurnMode(this.turnType, this.turnValue);
				return;
			case GorillaKeyboardBindings.option3:
				this.turnType = "NONE";
				PlayerPrefs.SetString("stickTurning", this.turnType);
				PlayerPrefs.Save();
				this.gorillaTurn.ChangeTurnMode(this.turnType, this.turnValue);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600445E RID: 17502 RVA: 0x0017B2CC File Offset: 0x001794CC
		private void ProcessMicState(GorillaKeyboardBindings buttonPressed)
		{
			switch (buttonPressed)
			{
			case GorillaKeyboardBindings.option1:
				this.pttType = "ALL CHAT";
				PlayerPrefs.SetString("pttType", this.pttType);
				PlayerPrefs.Save();
				return;
			case GorillaKeyboardBindings.option2:
				this.pttType = "PUSH TO TALK";
				PlayerPrefs.SetString("pttType", this.pttType);
				PlayerPrefs.Save();
				return;
			case GorillaKeyboardBindings.option3:
				this.pttType = "PUSH TO MUTE";
				PlayerPrefs.SetString("pttType", this.pttType);
				PlayerPrefs.Save();
				return;
			default:
				return;
			}
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x0017B354 File Offset: 0x00179554
		private void ProcessQueueState(GorillaKeyboardBindings buttonPressed)
		{
			switch (buttonPressed)
			{
			case GorillaKeyboardBindings.option1:
				this.JoinQueue("DEFAULT", false);
				return;
			case GorillaKeyboardBindings.option2:
				this.JoinQueue("MINIGAMES", false);
				return;
			case GorillaKeyboardBindings.option3:
				if (this.allowedInCompetitive)
				{
					this.JoinQueue("COMPETITIVE", false);
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x0017B3A8 File Offset: 0x001795A8
		public void JoinTroop(string newTroopName)
		{
			if (this.IsValidTroopName(newTroopName))
			{
				this.currentTroopPopulation = -1;
				this.troopName = newTroopName;
				PlayerPrefs.SetString("troopName", this.troopName);
				if (this.troopQueueActive)
				{
					this.currentQueue = this.GetQueueNameForTroop(this.troopName);
					PlayerPrefs.SetString("currentQueue", this.currentQueue);
				}
				PlayerPrefs.Save();
				this.JoinTroopQueue();
			}
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x0005BC10 File Offset: 0x00059E10
		public void JoinTroopQueue()
		{
			if (this.IsValidTroopName(this.troopName))
			{
				this.currentTroopPopulation = -1;
				this.JoinQueue(this.GetQueueNameForTroop(this.troopName), true);
				this.RequestTroopPopulation(true);
			}
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x0017B414 File Offset: 0x00179614
		private void RequestTroopPopulation(bool forceUpdate = false)
		{
			if (!PlayFabCloudScriptAPI.IsEntityLoggedIn())
			{
				return;
			}
			if (!this.hasRequestedInitialTroopPopulation || forceUpdate)
			{
				if (this.nextPopulationCheckTime > Time.time)
				{
					return;
				}
				this.nextPopulationCheckTime = Time.time + this.troopPopulationCheckCooldown;
				this.hasRequestedInitialTroopPopulation = true;
				GorillaServer.Instance.ReturnQueueStats(new ReturnQueueStatsRequest
				{
					queueName = this.troopName
				}, delegate(ExecuteFunctionResult result)
				{
					Debug.Log("Troop pop received");
					object obj;
					if (((JsonObject)result.FunctionResult).TryGetValue("PlayerCount", out obj))
					{
						this.currentTroopPopulation = int.Parse(obj.ToString());
						if (this.currentComputerState == GorillaComputer.ComputerState.Queue)
						{
							this.UpdateScreen();
							return;
						}
					}
					else
					{
						this.currentTroopPopulation = 0;
					}
				}, delegate(PlayFabError error)
				{
					Debug.LogError(string.Format("Error requesting troop population: {0}", error));
					this.currentTroopPopulation = -1;
				});
			}
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x0005BC41 File Offset: 0x00059E41
		public void JoinDefaultQueue()
		{
			this.JoinQueue("DEFAULT", false);
		}

		// Token: 0x06004464 RID: 17508 RVA: 0x0017B494 File Offset: 0x00179694
		public void LeaveTroop()
		{
			if (this.IsValidTroopName(this.troopName))
			{
				this.troopToJoin = this.troopName;
			}
			this.currentTroopPopulation = -1;
			this.troopName = string.Empty;
			PlayerPrefs.SetString("troopName", this.troopName);
			if (this.troopQueueActive)
			{
				this.JoinDefaultQueue();
			}
			PlayerPrefs.Save();
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x0005BC4F File Offset: 0x00059E4F
		public string GetCurrentTroop()
		{
			if (this.troopQueueActive)
			{
				return this.troopName;
			}
			return this.currentQueue;
		}

		// Token: 0x06004466 RID: 17510 RVA: 0x0005BC66 File Offset: 0x00059E66
		public int GetCurrentTroopPopulation()
		{
			if (this.troopQueueActive)
			{
				return this.currentTroopPopulation;
			}
			return -1;
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x0017B4F0 File Offset: 0x001796F0
		private void JoinQueue(string queueName, bool isTroopQueue = false)
		{
			this.currentQueue = queueName;
			this.troopQueueActive = isTroopQueue;
			this.currentTroopPopulation = -1;
			PlayerPrefs.SetString("currentQueue", this.currentQueue);
			PlayerPrefs.SetInt("troopQueueActive", this.troopQueueActive ? 1 : 0);
			PlayerPrefs.Save();
		}

		// Token: 0x06004468 RID: 17512 RVA: 0x0017B540 File Offset: 0x00179740
		private void ProcessGroupState(GorillaKeyboardBindings buttonPressed)
		{
			switch (buttonPressed)
			{
			case GorillaKeyboardBindings.one:
				this.groupMapJoin = "FOREST";
				this.groupMapJoinIndex = 0;
				PlayerPrefs.SetString("groupMapJoin", this.groupMapJoin);
				PlayerPrefs.SetInt("groupMapJoinIndex", this.groupMapJoinIndex);
				PlayerPrefs.Save();
				break;
			case GorillaKeyboardBindings.two:
				this.groupMapJoin = "CAVE";
				this.groupMapJoinIndex = 1;
				PlayerPrefs.SetString("groupMapJoin", this.groupMapJoin);
				PlayerPrefs.SetInt("groupMapJoinIndex", this.groupMapJoinIndex);
				PlayerPrefs.Save();
				break;
			case GorillaKeyboardBindings.three:
				this.groupMapJoin = "CANYON";
				this.groupMapJoinIndex = 2;
				PlayerPrefs.SetString("groupMapJoin", this.groupMapJoin);
				PlayerPrefs.SetInt("groupMapJoinIndex", this.groupMapJoinIndex);
				PlayerPrefs.Save();
				break;
			case GorillaKeyboardBindings.four:
				this.groupMapJoin = "CITY";
				this.groupMapJoinIndex = 3;
				PlayerPrefs.SetString("groupMapJoin", this.groupMapJoin);
				PlayerPrefs.SetInt("groupMapJoinIndex", this.groupMapJoinIndex);
				PlayerPrefs.Save();
				break;
			case GorillaKeyboardBindings.five:
				this.groupMapJoin = "CLOUDS";
				this.groupMapJoinIndex = 4;
				PlayerPrefs.SetString("groupMapJoin", this.groupMapJoin);
				PlayerPrefs.SetInt("groupMapJoinIndex", this.groupMapJoinIndex);
				PlayerPrefs.Save();
				break;
			default:
				if (buttonPressed == GorillaKeyboardBindings.enter)
				{
					this.OnGroupJoinButtonPress(Mathf.Min(this.allowedMapsToJoin.Length - 1, this.groupMapJoinIndex), this.friendJoinCollider);
				}
				break;
			}
			this.roomFull = false;
		}

		// Token: 0x06004469 RID: 17513 RVA: 0x0017B6C4 File Offset: 0x001798C4
		private void ProcessTroopState(GorillaKeyboardBindings buttonPressed)
		{
			ValueTuple<bool, Permission.ManagedByEnum> privateRoomPermissionStatus = KIDManager.Instance.GetPrivateRoomPermissionStatus();
			bool flag = (privateRoomPermissionStatus.Item1 || privateRoomPermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && privateRoomPermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED;
			bool flag2 = this.IsValidTroopName(this.troopName);
			if (flag)
			{
				switch (buttonPressed)
				{
				case GorillaKeyboardBindings.delete:
					if (!flag2 && this.troopToJoin.Length > 0)
					{
						this.troopToJoin = this.troopToJoin.Substring(0, this.troopToJoin.Length - 1);
						return;
					}
					break;
				case GorillaKeyboardBindings.enter:
					if (!flag2)
					{
						this.CheckAutoBanListForTroopName(this.troopToJoin);
						return;
					}
					break;
				case GorillaKeyboardBindings.option1:
					this.JoinTroopQueue();
					return;
				case GorillaKeyboardBindings.option2:
					this.JoinDefaultQueue();
					return;
				case GorillaKeyboardBindings.option3:
					this.LeaveTroop();
					return;
				default:
					if (!flag2 && this.troopToJoin.Length < 12)
					{
						string str = this.troopToJoin;
						string str2;
						if (buttonPressed >= GorillaKeyboardBindings.up)
						{
							str2 = buttonPressed.ToString();
						}
						else
						{
							int num = (int)buttonPressed;
							str2 = num.ToString();
						}
						this.troopToJoin = str + str2;
						return;
					}
					break;
				}
			}
			else
			{
				switch (buttonPressed)
				{
				case GorillaKeyboardBindings.option1:
					break;
				case GorillaKeyboardBindings.option2:
					if (this._currentScreentState != GorillaComputer.EKidScreenState.Ready)
					{
						this.ProcessScreen_SetupKID();
						return;
					}
					this.RequestUpdatedPermissions();
					return;
				case GorillaKeyboardBindings.option3:
					if (this._currentScreentState != GorillaComputer.EKidScreenState.Show_OTP)
					{
						return;
					}
					this.ProcessScreen_SetupKID();
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600446A RID: 17514 RVA: 0x0005BC78 File Offset: 0x00059E78
		private bool IsValidTroopName(string troop)
		{
			return !string.IsNullOrEmpty(troop) && troop.Length <= 12 && (this.allowedInCompetitive || troop != "COMPETITIVE");
		}

		// Token: 0x0600446B RID: 17515 RVA: 0x0003DD71 File Offset: 0x0003BF71
		private string GetQueueNameForTroop(string troop)
		{
			return troop;
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x0017B80C File Offset: 0x00179A0C
		private void ProcessVoiceState(GorillaKeyboardBindings buttonPressed)
		{
			ValueTuple<bool, Permission.ManagedByEnum> voiceChatPermissionStatus = KIDManager.Instance.GetVoiceChatPermissionStatus();
			if ((voiceChatPermissionStatus.Item1 || voiceChatPermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && voiceChatPermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED)
			{
				if (buttonPressed != GorillaKeyboardBindings.option1)
				{
					if (buttonPressed == GorillaKeyboardBindings.option2)
					{
						this.voiceChatOn = "FALSE";
					}
				}
				else
				{
					this.voiceChatOn = "TRUE";
				}
				PlayerPrefs.SetString("voiceChatOn", this.voiceChatOn);
				PlayerPrefs.Save();
			}
			else if (buttonPressed != GorillaKeyboardBindings.option2)
			{
				if (buttonPressed == GorillaKeyboardBindings.option3)
				{
					if (this._currentScreentState != GorillaComputer.EKidScreenState.Show_OTP)
					{
						return;
					}
					this.ProcessScreen_SetupKID();
				}
			}
			else if (this._currentScreentState != GorillaComputer.EKidScreenState.Ready)
			{
				this.ProcessScreen_SetupKID();
			}
			else
			{
				this.RequestUpdatedPermissions();
			}
			RigContainer.RefreshAllRigVoices();
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x0017B8BC File Offset: 0x00179ABC
		private void ProcessAutoMuteState(GorillaKeyboardBindings buttonPressed)
		{
			switch (buttonPressed)
			{
			case GorillaKeyboardBindings.option1:
				this.autoMuteType = "AGGRESSIVE";
				PlayerPrefs.SetInt("autoMute", 2);
				PlayerPrefs.Save();
				RigContainer.RefreshAllRigVoices();
				break;
			case GorillaKeyboardBindings.option2:
				this.autoMuteType = "MODERATE";
				PlayerPrefs.SetInt("autoMute", 1);
				PlayerPrefs.Save();
				RigContainer.RefreshAllRigVoices();
				break;
			case GorillaKeyboardBindings.option3:
				this.autoMuteType = "OFF";
				PlayerPrefs.SetInt("autoMute", 0);
				PlayerPrefs.Save();
				RigContainer.RefreshAllRigVoices();
				break;
			}
			this.UpdateScreen();
		}

		// Token: 0x0600446E RID: 17518 RVA: 0x0017B94C File Offset: 0x00179B4C
		private void ProcessVisualsState(GorillaKeyboardBindings buttonPressed)
		{
			if (buttonPressed < GorillaKeyboardBindings.up)
			{
				this.instrumentVolume = (float)buttonPressed / 50f;
				PlayerPrefs.SetFloat("instrumentVolume", this.instrumentVolume);
				PlayerPrefs.Save();
				return;
			}
			if (buttonPressed == GorillaKeyboardBindings.option1)
			{
				this.disableParticles = false;
				PlayerPrefs.SetString("disableParticles", "FALSE");
				PlayerPrefs.Save();
				GorillaTagger.Instance.ShowCosmeticParticles(!this.disableParticles);
				return;
			}
			if (buttonPressed != GorillaKeyboardBindings.option2)
			{
				return;
			}
			this.disableParticles = true;
			PlayerPrefs.SetString("disableParticles", "TRUE");
			PlayerPrefs.Save();
			GorillaTagger.Instance.ShowCosmeticParticles(!this.disableParticles);
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x0005BCA3 File Offset: 0x00059EA3
		private void ProcessCreditsState(GorillaKeyboardBindings buttonPressed)
		{
			if (buttonPressed == GorillaKeyboardBindings.enter)
			{
				this.creditsView.ProcessButtonPress(buttonPressed);
				return;
			}
			if (buttonPressed != GorillaKeyboardBindings.option1)
			{
				return;
			}
			Debug.Log("[KID] REFRESH TOKEN TEST");
			KIDIntegrationRouter.RefreshToken();
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x0005BCCD File Offset: 0x00059ECD
		private void ProcessSupportState(GorillaKeyboardBindings buttonPressed)
		{
			if (buttonPressed == GorillaKeyboardBindings.enter)
			{
				this.displaySupport = true;
			}
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x0017B9EC File Offset: 0x00179BEC
		private void ProcessRedemptionState(GorillaKeyboardBindings buttonPressed)
		{
			if (this.RedemptionStatus == GorillaComputer.RedemptionResult.Checking)
			{
				return;
			}
			if (buttonPressed != GorillaKeyboardBindings.delete)
			{
				if (buttonPressed == GorillaKeyboardBindings.enter)
				{
					if (this.redemptionCode != "")
					{
						if (this.redemptionCode.Length < 8)
						{
							this.RedemptionStatus = GorillaComputer.RedemptionResult.Invalid;
							return;
						}
						CodeRedemption.Instance.HandleCodeRedemption(this.redemptionCode);
						this.RedemptionStatus = GorillaComputer.RedemptionResult.Checking;
						return;
					}
					else if (this.RedemptionStatus != GorillaComputer.RedemptionResult.Success)
					{
						this.RedemptionStatus = GorillaComputer.RedemptionResult.Empty;
						return;
					}
				}
				else if (this.redemptionCode.Length < 8 && (buttonPressed < GorillaKeyboardBindings.up || buttonPressed > GorillaKeyboardBindings.option3))
				{
					string str = this.redemptionCode;
					string str2;
					if (buttonPressed >= GorillaKeyboardBindings.up)
					{
						str2 = buttonPressed.ToString();
					}
					else
					{
						int num = (int)buttonPressed;
						str2 = num.ToString();
					}
					this.redemptionCode = str + str2;
				}
			}
			else if (this.redemptionCode.Length > 0)
			{
				this.redemptionCode = this.redemptionCode.Substring(0, this.redemptionCode.Length - 1);
				return;
			}
		}

		// Token: 0x06004472 RID: 17522 RVA: 0x0017BAD8 File Offset: 0x00179CD8
		private void ProcessNameWarningState(GorillaKeyboardBindings buttonPressed)
		{
			if (this.warningConfirmationInputString.ToLower() == "yes")
			{
				this.PopState();
				return;
			}
			if (buttonPressed == GorillaKeyboardBindings.delete)
			{
				if (this.warningConfirmationInputString.Length > 0)
				{
					this.warningConfirmationInputString = this.warningConfirmationInputString.Substring(0, this.warningConfirmationInputString.Length - 1);
					return;
				}
			}
			else if (this.warningConfirmationInputString.Length < 3)
			{
				this.warningConfirmationInputString += buttonPressed.ToString();
			}
		}

		// Token: 0x06004473 RID: 17523 RVA: 0x0017BB64 File Offset: 0x00179D64
		public void UpdateScreen()
		{
			if (NetworkSystem.Instance != null && !NetworkSystem.Instance.WrongVersion)
			{
				this.UpdateFunctionScreen();
				switch (this.currentState)
				{
				case GorillaComputer.ComputerState.Startup:
					this.StartupScreen();
					break;
				case GorillaComputer.ComputerState.Color:
					this.ColourScreen();
					break;
				case GorillaComputer.ComputerState.Name:
					this.NameScreen();
					break;
				case GorillaComputer.ComputerState.Turn:
					this.TurnScreen();
					break;
				case GorillaComputer.ComputerState.Mic:
					this.MicScreen();
					break;
				case GorillaComputer.ComputerState.Room:
					this.RoomScreen();
					break;
				case GorillaComputer.ComputerState.Queue:
					this.QueueScreen();
					break;
				case GorillaComputer.ComputerState.Group:
					this.GroupScreen();
					break;
				case GorillaComputer.ComputerState.Voice:
					this.VoiceScreen();
					break;
				case GorillaComputer.ComputerState.AutoMute:
					this.AutomuteScreen();
					break;
				case GorillaComputer.ComputerState.Credits:
					this.CreditsScreen();
					break;
				case GorillaComputer.ComputerState.Visuals:
					this.VisualsScreen();
					break;
				case GorillaComputer.ComputerState.Time:
					this.TimeScreen();
					break;
				case GorillaComputer.ComputerState.NameWarning:
					this.NameWarningScreen();
					break;
				case GorillaComputer.ComputerState.Loading:
					this.LoadingScreen();
					break;
				case GorillaComputer.ComputerState.Support:
					this.SupportScreen();
					break;
				case GorillaComputer.ComputerState.Troop:
					this.TroopScreen();
					break;
				case GorillaComputer.ComputerState.KID:
					this.KIdScreen();
					break;
				case GorillaComputer.ComputerState.Redemption:
					this.RedemptionScreen();
					break;
				}
			}
			this.UpdateGameModeText();
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x0005BCDB File Offset: 0x00059EDB
		private void LoadingScreen()
		{
			this.screenText.Text = "LOADING";
			this.LoadingRoutine = base.StartCoroutine(this.<LoadingScreen>g__LoadingScreenLocal|199_0());
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x0017BC98 File Offset: 0x00179E98
		private void NameWarningScreen()
		{
			this.screenText.Text = "<color=red>WARNING: PLEASE CHOOSE A BETTER NAME\n\nENTERING ANOTHER BAD NAME WILL RESULT IN A BAN</color>";
			if (this.warningConfirmationInputString.ToLower() == "yes")
			{
				GorillaText gorillaText = this.screenText;
				gorillaText.Text += "\n\nPRESS ANY KEY TO CONTINUE";
				return;
			}
			GorillaText gorillaText2 = this.screenText;
			gorillaText2.Text = gorillaText2.Text + "\n\nTYPE 'YES' TO CONFIRM: " + this.warningConfirmationInputString;
		}

		// Token: 0x06004476 RID: 17526 RVA: 0x0017BD0C File Offset: 0x00179F0C
		private void SupportScreen()
		{
			if (this.displaySupport)
			{
				string text = PlayFabAuthenticator.instance.platform.ToString().ToUpper();
				string text2;
				if (text == "PC")
				{
					text2 = "OCULUS PC";
				}
				else
				{
					text2 = text;
				}
				text = text2;
				this.screenText.Text = string.Concat(new string[]
				{
					"SUPPORT\n\nPLAYERID   ",
					PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
					"\nVERSION    ",
					this.version.ToUpper(),
					"\nPLATFORM   ",
					text,
					"\nBUILD DATE ",
					this.buildDate,
					"\n"
				});
				return;
			}
			this.screenText.Text = "SUPPORT\n\n";
			GorillaText gorillaText = this.screenText;
			gorillaText.Text += "PRESS ENTER TO DISPLAY SUPPORT AND ACCOUNT INFORMATION\n\n\n\n";
			GorillaText gorillaText2 = this.screenText;
			gorillaText2.Text += "<color=red>DO NOT SHARE ACCOUNT INFORMATION WITH ANYONE OTHER ";
			GorillaText gorillaText3 = this.screenText;
			gorillaText3.Text += "THAN ANOTHER AXIOM SUPPORT</color>";
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x0017BE20 File Offset: 0x0017A020
		private void TimeScreen()
		{
			this.screenText.Text = string.Concat(new string[]
			{
				"UPDATE TIME SETTINGS. (LOCALLY ONLY). \nPRESS OPTION 1 FOR NORMAL MODE. \nPRESS OPTION 2 FOR STATIC MODE. \nPRESS 1-10 TO CHANGE TIME OF DAY. \nCURRENT MODE: ",
				BetterDayNightManager.instance.currentSetting.ToString().ToUpper(),
				". \nTIME OF DAY: ",
				BetterDayNightManager.instance.currentTimeOfDay.ToUpper(),
				". \n"
			});
		}

		// Token: 0x06004478 RID: 17528 RVA: 0x0005BCFF File Offset: 0x00059EFF
		private void CreditsScreen()
		{
			this.screenText.Text = this.creditsView.GetScreenText();
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x0017BE90 File Offset: 0x0017A090
		private void VisualsScreen()
		{
			this.screenText.Text = "UPDATE ITEMS SETTINGS. PRESS OPTION 1 TO ENABLE ITEM PARTICLES. PRESS OPTION 2 TO DISABLE ITEM PARTICLES. PRESS 1-10 TO CHANGE INSTRUMENT VOLUME FOR OTHER PLAYERS.\n\nITEM PARTICLES ON: " + (this.disableParticles ? "FALSE" : "TRUE") + "\nINSTRUMENT VOLUME: " + Mathf.CeilToInt(this.instrumentVolume * 50f).ToString();
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x0017BEE4 File Offset: 0x0017A0E4
		private void VoiceScreen()
		{
			ValueTuple<bool, Permission.ManagedByEnum> voiceChatPermissionStatus = KIDManager.Instance.GetVoiceChatPermissionStatus();
			if ((voiceChatPermissionStatus.Item1 || voiceChatPermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && voiceChatPermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED)
			{
				this.screenText.Text = "CHOOSE WHICH TYPE OF VOICE YOU WANT TO HEAR AND SPEAK. \nPRESS OPTION 1 = HUMAN VOICES. \nPRESS OPTION 2 = MONKE VOICES. \n\nVOICE TYPE: " + ((this.voiceChatOn == "TRUE") ? "HUMAN" : ((this.voiceChatOn == "FALSE") ? "MONKE" : "OFF"));
				return;
			}
			if (voiceChatPermissionStatus.Item2 == Permission.ManagedByEnum.PROHIBITED)
			{
				this.VoiceScreen_KIdProhibited();
				return;
			}
			this.VoiceScreen_KIdPermission();
		}

		// Token: 0x0600447B RID: 17531 RVA: 0x0005BD17 File Offset: 0x00059F17
		private void AutomuteScreen()
		{
			this.screenText.Text = "AUTOMOD AUTOMATICALLY MUTES PLAYERS WHEN THEY JOIN YOUR ROOM IF A LOT OF OTHER PLAYERS HAVE MUTED THEM\nPRESS OPTION 1 FOR AGGRESSIVE MUTING\nPRESS OPTION 2 FOR MODERATE MUTING\nPRESS OPTION 3 TO TURN AUTOMOD OFF\n\nCURRENT AUTOMOD LEVEL: " + this.autoMuteType;
		}

		// Token: 0x0600447C RID: 17532 RVA: 0x0017BF84 File Offset: 0x0017A184
		private void GroupScreen()
		{
			string str = (this.allowedMapsToJoin.Length > 1) ? this.groupMapJoin : this.allowedMapsToJoin[0].ToUpper();
			string str2 = (this.allowedMapsToJoin.Length > 1) ? "\n\nUSE NUMBER KEYS TO SELECT DESTINATION\n1: FOREST, 2: CAVE, 3: CANYON, 4: CITY, 5: CLOUDS." : "";
			if (FriendshipGroupDetection.Instance.IsInParty)
			{
				string str3 = this.GetSelectedMapJoinTrigger().CanPartyJoin() ? "" : "\n\n<color=red>CANNOT JOIN BECAUSE YOUR GROUP IS NOT HERE</color>";
				this.screenText.Text = "PRESS ENTER TO JOIN A PUBLIC GAME WITH YOUR FRIENDSHIP GROUP.\n\nACTIVE ZONE WILL BE: " + str + str2 + str3;
				return;
			}
			this.screenText.Text = "PRESS ENTER TO JOIN A PUBLIC GAME AND BRING EVERYONE IN THIS ROOM WITH YOU.\n\nACTIVE ZONE WILL BE: " + str + str2;
		}

		// Token: 0x0600447D RID: 17533 RVA: 0x0005BD34 File Offset: 0x00059F34
		private void MicScreen()
		{
			if (KIDManager.Instance.GetVoiceChatPermissionStatus().Item2 == Permission.ManagedByEnum.PROHIBITED)
			{
				this.MicScreen_KIdProhibited();
				return;
			}
			this.screenText.Text = "PRESS OPTION 1 = ALL CHAT.\nPRESS OPTION 2 = PUSH TO TALK.\nPRESS OPTION 3 = PUSH TO MUTE.\n\nCURRENT MIC SETTING: " + this.pttType + "\n\nPUSH TO TALK AND PUSH TO MUTE WORK WITH ANY FACE BUTTON";
		}

		// Token: 0x0600447E RID: 17534 RVA: 0x0017C020 File Offset: 0x0017A220
		private void QueueScreen()
		{
			if (this.allowedInCompetitive)
			{
				this.screenText.Text = "THIS OPTION AFFECTS WHO YOU PLAY WITH. DEFAULT IS FOR ANYONE TO PLAY NORMALLY. MINIGAMES IS FOR PEOPLE LOOKING TO PLAY WITH THEIR OWN MADE UP RULES. COMPETITIVE IS FOR PLAYERS WHO WANT TO PLAY THE GAME AND TRY AS HARD AS THEY CAN. PRESS OPTION 1 FOR DEFAULT, OPTION 2 FOR MINIGAMES, OR OPTION 3 FOR COMPETITIVE.";
			}
			else
			{
				this.screenText.Text = "THIS OPTION AFFECTS WHO YOU PLAY WITH. DEFAULT IS FOR ANYONE TO PLAY NORMALLY. MINIGAMES IS FOR PEOPLE LOOKING TO PLAY WITH THEIR OWN MADE UP RULES. BEAT THE OBSTACLE COURSE IN CITY TO ALLOW COMPETITIVE PLAY. PRESS OPTION 1 FOR DEFAULT, OR OPTION 2 FOR MINIGAMES.";
			}
			GorillaText gorillaText = this.screenText;
			gorillaText.Text = gorillaText.Text + "\n\nCURRENT QUEUE: " + this.currentQueue;
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x0017C078 File Offset: 0x0017A278
		private void TroopScreen()
		{
			ValueTuple<bool, Permission.ManagedByEnum> privateRoomPermissionStatus = KIDManager.Instance.GetPrivateRoomPermissionStatus();
			bool flag = (privateRoomPermissionStatus.Item1 || privateRoomPermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && privateRoomPermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED;
			bool flag2 = this.IsValidTroopName(this.troopName);
			this.screenText.Text = string.Empty;
			if (flag)
			{
				this.screenText.Text = "PLAY WITH A PERSISTENT GROUP ACROSS MULTIPLE ROOMS.";
				if (!flag2)
				{
					GorillaText gorillaText = this.screenText;
					gorillaText.Text += " PRESS ENTER TO JOIN OR CREATE A TROOP.";
				}
			}
			GorillaText gorillaText2 = this.screenText;
			gorillaText2.Text += "\n\nCURRENT TROOP: ";
			if (flag2)
			{
				GorillaText gorillaText3 = this.screenText;
				gorillaText3.Text = gorillaText3.Text + this.troopName + "\n";
				if (flag)
				{
					bool flag3 = this.currentTroopPopulation > -1;
					if (this.troopQueueActive)
					{
						GorillaText gorillaText4 = this.screenText;
						gorillaText4.Text += "  -IN TROOP QUEUE-\n";
						if (flag3)
						{
							GorillaText gorillaText5 = this.screenText;
							gorillaText5.Text += string.Format("\nPLAYERS IN TROOP: {0}\n", Mathf.Max(1, this.currentTroopPopulation));
						}
						GorillaText gorillaText6 = this.screenText;
						gorillaText6.Text += "\nPRESS OPTION 2 FOR DEFAULT QUEUE.\n";
					}
					else
					{
						GorillaText gorillaText7 = this.screenText;
						gorillaText7.Text = gorillaText7.Text + "  -IN " + this.currentQueue + " QUEUE-\n";
						if (flag3)
						{
							GorillaText gorillaText8 = this.screenText;
							gorillaText8.Text += string.Format("\nPLAYERS IN TROOP: {0}\n", Mathf.Max(1, this.currentTroopPopulation));
						}
						GorillaText gorillaText9 = this.screenText;
						gorillaText9.Text += "\nPRESS OPTION 1 FOR TROOP QUEUE.\n";
					}
					GorillaText gorillaText10 = this.screenText;
					gorillaText10.Text += "PRESS OPTION 3 TO LEAVE YOUR TROOP.";
				}
			}
			else
			{
				GorillaText gorillaText11 = this.screenText;
				gorillaText11.Text += "-NOT IN TROOP-";
			}
			if (flag)
			{
				if (!flag2)
				{
					GorillaText gorillaText12 = this.screenText;
					gorillaText12.Text = gorillaText12.Text + "\n\nTROOP TO JOIN: " + this.troopToJoin;
					return;
				}
			}
			else
			{
				if (privateRoomPermissionStatus.Item2 == Permission.ManagedByEnum.PROHIBITED)
				{
					this.TroopScreen_KIdProhibited();
					return;
				}
				this.TroopScreen_KIdPermission();
			}
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x0005BD6F File Offset: 0x00059F6F
		private void TurnScreen()
		{
			this.screenText.Text = "PRESS OPTION 1 TO USE SNAP TURN. PRESS OPTION 2 TO USE SMOOTH TURN. PRESS OPTION 3 TO USE NO ARTIFICIAL TURNING. PRESS THE NUMBER KEYS TO CHOOSE A TURNING SPEED.\n CURRENT TURN TYPE: " + this.turnType + "\nCURRENT TURN SPEED: " + this.turnValue.ToString();
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x0017C2B0 File Offset: 0x0017A4B0
		private void NameScreen()
		{
			ValueTuple<bool, Permission.ManagedByEnum> customNicknamePermissionStatus = KIDManager.Instance.GetCustomNicknamePermissionStatus();
			if ((customNicknamePermissionStatus.Item1 || customNicknamePermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && customNicknamePermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED)
			{
				this.screenText.Text = "PRESS ENTER TO CHANGE YOUR NAME TO THE ENTERED NEW NAME.\n\nCURRENT NAME: " + this.savedName;
				if (this.NametagsEnabled)
				{
					GorillaText gorillaText = this.screenText;
					gorillaText.Text = gorillaText.Text + "\n\n    NEW NAME: " + this.currentName;
				}
				GorillaText gorillaText2 = this.screenText;
				gorillaText2.Text = gorillaText2.Text + "\n\nPRESS OPTION 1 TO TOGGLE NAMETAGS.\nCURRENTLY NAMETAGS ARE: " + (this.NametagsEnabled ? "ON" : "OFF");
				return;
			}
			if (customNicknamePermissionStatus.Item2 == Permission.ManagedByEnum.PROHIBITED)
			{
				this.NameScreen_KIdProhibited();
				return;
			}
			this.NameScreen_KIdPermission();
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x0017C374 File Offset: 0x0017A574
		private void StartupScreen()
		{
			string text = string.Empty;
			if (KIDManager.Instance.GetActiveAccountStatus() != AgeStatusType.LEGALADULT)
			{
				text = "YOU ARE PLAYING ON A MANAGED ACCOUNT\nSOME SETTINGS ARE DISABLED\n\n";
			}
			string empty = string.Empty;
			this.screenText.Text = string.Concat(new string[]
			{
				"GORILLA OS\n\n",
				text,
				NetworkSystem.Instance.GlobalPlayerCount().ToString(),
				" PLAYERS ONLINE\n\n",
				this.usersBanned.ToString(),
				" USERS BANNED YESTERDAY\n\n",
				empty,
				"PRESS ANY KEY TO BEGIN"
			});
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x0017C404 File Offset: 0x0017A604
		private void ColourScreen()
		{
			this.screenText.Text = "USE THE OPTIONS BUTTONS TO SELECT THE COLOR TO UPDATE, THEN PRESS 0-9 TO SET A NEW VALUE.";
			GorillaText gorillaText = this.screenText;
			gorillaText.Text = gorillaText.Text + "\n\n  RED: " + Mathf.FloorToInt(this.redValue * 9f).ToString() + ((this.colorCursorLine == 0) ? "<--" : "");
			GorillaText gorillaText2 = this.screenText;
			gorillaText2.Text = gorillaText2.Text + "\n\nGREEN: " + Mathf.FloorToInt(this.greenValue * 9f).ToString() + ((this.colorCursorLine == 1) ? "<--" : "");
			GorillaText gorillaText3 = this.screenText;
			gorillaText3.Text = gorillaText3.Text + "\n\n BLUE: " + Mathf.FloorToInt(this.blueValue * 9f).ToString() + ((this.colorCursorLine == 2) ? "<--" : "");
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x0017C4FC File Offset: 0x0017A6FC
		private void RoomScreen()
		{
			ValueTuple<bool, Permission.ManagedByEnum> privateRoomPermissionStatus = KIDManager.Instance.GetPrivateRoomPermissionStatus();
			object obj = (privateRoomPermissionStatus.Item1 || privateRoomPermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && privateRoomPermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED;
			this.screenText.Text = "";
			object obj2 = obj;
			if (obj2 != null)
			{
				GorillaText gorillaText = this.screenText;
				gorillaText.Text += "PRESS ENTER TO JOIN OR CREATE A CUSTOM ROOM WITH THE ENTERED CODE. ";
			}
			GorillaText gorillaText2 = this.screenText;
			gorillaText2.Text += "PRESS OPTION 1 TO DISCONNECT FROM THE CURRENT ROOM. ";
			if (FriendshipGroupDetection.Instance.IsInParty)
			{
				if (FriendshipGroupDetection.Instance.IsPartyWithinCollider(this.friendJoinCollider))
				{
					GorillaText gorillaText3 = this.screenText;
					gorillaText3.Text += "YOUR GROUP WILL TRAVEL WITH YOU. ";
				}
				else
				{
					GorillaText gorillaText4 = this.screenText;
					gorillaText4.Text += "<color=red>YOU WILL LEAVE YOUR PARTY UNLESS YOU GATHER THEM HERE FIRST!</color> ";
				}
			}
			GorillaText gorillaText5 = this.screenText;
			gorillaText5.Text += "\n\nCURRENT ROOM: ";
			if (NetworkSystem.Instance.InRoom)
			{
				GorillaText gorillaText6 = this.screenText;
				gorillaText6.Text += NetworkSystem.Instance.RoomName;
				if (NetworkSystem.Instance.SessionIsPrivate)
				{
					GorillaGameManager activeGameMode = GameMode.ActiveGameMode;
					string text = (activeGameMode != null) ? activeGameMode.GameModeName() : null;
					if (text != null && text != this.currentGameMode.Value)
					{
						GorillaText gorillaText7 = this.screenText;
						gorillaText7.Text = gorillaText7.Text + " (" + text + " GAME)";
					}
				}
				GorillaText gorillaText8 = this.screenText;
				gorillaText8.Text = gorillaText8.Text + "\n\nPLAYERS IN ROOM: " + NetworkSystem.Instance.RoomPlayerCount.ToString();
			}
			else
			{
				GorillaText gorillaText9 = this.screenText;
				gorillaText9.Text += "-NOT IN ROOM-";
				GorillaText gorillaText10 = this.screenText;
				gorillaText10.Text = gorillaText10.Text + "\n\nPLAYERS ONLINE: " + NetworkSystem.Instance.GlobalPlayerCount().ToString();
			}
			if (obj2 != null)
			{
				GorillaText gorillaText11 = this.screenText;
				gorillaText11.Text = gorillaText11.Text + "\n\nROOM TO JOIN: " + this.roomToJoin;
				if (this.roomFull)
				{
					GorillaText gorillaText12 = this.screenText;
					gorillaText12.Text += "\n\nROOM FULL. JOIN ROOM FAILED.";
					return;
				}
				if (this.roomNotAllowed)
				{
					GorillaText gorillaText13 = this.screenText;
					gorillaText13.Text += "\n\nCANNOT JOIN ROOM TYPE FROM HERE.";
					return;
				}
			}
			else
			{
				if (privateRoomPermissionStatus.Item2 == Permission.ManagedByEnum.PROHIBITED)
				{
					this.RoomScreen_KIdProhibited();
					return;
				}
				this.RoomScreen_KIdPermission();
			}
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x0017C76C File Offset: 0x0017A96C
		private void RedemptionScreen()
		{
			this.screenText.Text = "TYPE REDEMPTION CODE AND PRESS ENTER";
			GorillaText gorillaText = this.screenText;
			gorillaText.Text = gorillaText.Text + "\n\nCODE: " + this.redemptionCode;
			switch (this.RedemptionStatus)
			{
			case GorillaComputer.RedemptionResult.Empty:
				break;
			case GorillaComputer.RedemptionResult.Invalid:
			{
				GorillaText gorillaText2 = this.screenText;
				gorillaText2.Text += "\n\nINVALID CODE";
				return;
			}
			case GorillaComputer.RedemptionResult.Checking:
			{
				GorillaText gorillaText3 = this.screenText;
				gorillaText3.Text += "\n\nVALIDATING...";
				return;
			}
			case GorillaComputer.RedemptionResult.AlreadyUsed:
			{
				GorillaText gorillaText4 = this.screenText;
				gorillaText4.Text += "\n\nCODE ALREADY CLAIMED";
				return;
			}
			case GorillaComputer.RedemptionResult.Success:
			{
				GorillaText gorillaText5 = this.screenText;
				gorillaText5.Text += "\n\nSUCCESSFULLY CLAIMED!";
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x0017C83C File Offset: 0x0017AA3C
		private void UpdateGameModeText()
		{
			if (NetworkSystem.Instance.InRoom)
			{
				if (GorillaGameManager.instance != null)
				{
					this.currentGameModeText.Value = "CURRENT MODE\n" + GorillaGameManager.instance.GameModeName();
					return;
				}
				this.currentGameModeText.Value = "CURRENT MODE\n-NOT IN ROOM-";
			}
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x0005BD9C File Offset: 0x00059F9C
		private void UpdateFunctionScreen()
		{
			this.functionSelectText.Text = this.GetOrderListForScreen(this.currentState);
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x0005BDB5 File Offset: 0x00059FB5
		private void CheckAutoBanListForRoomName(string nameToCheck)
		{
			this.SwitchToLoadingState();
			this.AutoBanPlayfabFunction(nameToCheck, true, new Action<ExecuteFunctionResult>(this.OnRoomNameChecked));
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x0005BDD1 File Offset: 0x00059FD1
		private void CheckAutoBanListForPlayerName(string nameToCheck)
		{
			this.SwitchToLoadingState();
			this.AutoBanPlayfabFunction(nameToCheck, false, new Action<ExecuteFunctionResult>(this.OnPlayerNameChecked));
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x0005BDED File Offset: 0x00059FED
		private void CheckAutoBanListForTroopName(string nameToCheck)
		{
			if (this.IsValidTroopName(this.troopToJoin))
			{
				this.SwitchToLoadingState();
				this.AutoBanPlayfabFunction(nameToCheck, false, new Action<ExecuteFunctionResult>(this.OnTroopNameChecked));
			}
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x0005BE17 File Offset: 0x0005A017
		private void AutoBanPlayfabFunction(string nameToCheck, bool forRoom, Action<ExecuteFunctionResult> resultCallback)
		{
			GorillaServer.Instance.CheckForBadName(new CheckForBadNameRequest
			{
				name = nameToCheck,
				forRoom = forRoom
			}, resultCallback, new Action<PlayFabError>(this.OnErrorNameCheck));
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x0017C894 File Offset: 0x0017AA94
		private void OnRoomNameChecked(ExecuteFunctionResult result)
		{
			object obj;
			if (((JsonObject)result.FunctionResult).TryGetValue("result", out obj))
			{
				switch (int.Parse(obj.ToString()))
				{
				case 0:
					if (FriendshipGroupDetection.Instance.IsInParty && !FriendshipGroupDetection.Instance.IsPartyWithinCollider(this.friendJoinCollider))
					{
						FriendshipGroupDetection.Instance.LeaveParty();
					}
					if (this.playerInVirtualStump)
					{
						CustomMapManager.UnloadMod(false);
					}
					this.networkController.AttemptToJoinSpecificRoom(this.roomToJoin, FriendshipGroupDetection.Instance.IsInParty ? JoinType.JoinWithParty : JoinType.Solo);
					break;
				case 1:
					this.roomToJoin = "";
					this.roomToJoin += (this.playerInVirtualStump ? this.virtualStumpRoomPrepend : "");
					this.SwitchToWarningState();
					break;
				case 2:
					this.roomToJoin = "";
					this.roomToJoin += (this.playerInVirtualStump ? this.virtualStumpRoomPrepend : "");
					GorillaGameManager.ForceStopGame_DisconnectAndDestroy();
					break;
				}
			}
			if (this.currentState == GorillaComputer.ComputerState.Loading)
			{
				this.PopState();
			}
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x0017C9BC File Offset: 0x0017ABBC
		private void OnPlayerNameChecked(ExecuteFunctionResult result)
		{
			object obj;
			if (((JsonObject)result.FunctionResult).TryGetValue("result", out obj))
			{
				switch (int.Parse(obj.ToString()))
				{
				case 0:
					NetworkSystem.Instance.SetMyNickName(this.currentName);
					ModIOMapsTerminal.RequestDriverNickNameRefresh();
					break;
				case 1:
					NetworkSystem.Instance.SetMyNickName("gorilla");
					ModIOMapsTerminal.RequestDriverNickNameRefresh();
					this.currentName = "gorilla";
					this.SwitchToWarningState();
					break;
				case 2:
					NetworkSystem.Instance.SetMyNickName("gorilla");
					ModIOMapsTerminal.RequestDriverNickNameRefresh();
					this.currentName = "gorilla";
					GorillaGameManager.ForceStopGame_DisconnectAndDestroy();
					break;
				}
			}
			this.SetNameTagText(this.currentName);
			this.savedName = this.currentName;
			PlayerPrefs.SetString("playerName", this.currentName);
			PlayerPrefs.Save();
			if (NetworkSystem.Instance.InRoom)
			{
				GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", RpcTarget.All, new object[]
				{
					this.redValue,
					this.greenValue,
					this.blueValue
				});
			}
			if (this.currentState == GorillaComputer.ComputerState.Loading)
			{
				this.PopState();
			}
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x0017CAF8 File Offset: 0x0017ACF8
		private void OnTroopNameChecked(ExecuteFunctionResult result)
		{
			object obj;
			if (((JsonObject)result.FunctionResult).TryGetValue("result", out obj))
			{
				switch (int.Parse(obj.ToString()))
				{
				case 0:
					this.JoinTroop(this.troopToJoin);
					break;
				case 1:
					this.troopToJoin = string.Empty;
					this.SwitchToWarningState();
					break;
				case 2:
					this.troopToJoin = string.Empty;
					GorillaGameManager.ForceStopGame_DisconnectAndDestroy();
					break;
				}
			}
			if (this.currentState == GorillaComputer.ComputerState.Loading)
			{
				this.PopState();
			}
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x0005BE45 File Offset: 0x0005A045
		private void OnErrorNameCheck(PlayFabError error)
		{
			if (this.currentState == GorillaComputer.ComputerState.Loading)
			{
				this.PopState();
			}
			GorillaComputer.OnErrorShared(error);
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x0017CB80 File Offset: 0x0017AD80
		public bool CheckAutoBanListForName(string nameToCheck)
		{
			nameToCheck = nameToCheck.ToLower();
			nameToCheck = new string(Array.FindAll<char>(nameToCheck.ToCharArray(), (char c) => char.IsLetterOrDigit(c)));
			foreach (string value in this.anywhereTwoWeek)
			{
				if (nameToCheck.IndexOf(value) >= 0)
				{
					return false;
				}
			}
			foreach (string value2 in this.anywhereOneWeek)
			{
				if (nameToCheck.IndexOf(value2) >= 0 && !nameToCheck.Contains("fagol"))
				{
					return false;
				}
			}
			string[] array = this.exactOneWeek;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == nameToCheck)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x0017CC40 File Offset: 0x0017AE40
		public void UpdateColor(float red, float green, float blue)
		{
			this.redValue = Mathf.Clamp(red, 0f, 1f);
			this.greenValue = Mathf.Clamp(green, 0f, 1f);
			this.blueValue = Mathf.Clamp(blue, 0f, 1f);
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x0005BE5D File Offset: 0x0005A05D
		public void UpdateFailureText(string failMessage)
		{
			GorillaScoreboardTotalUpdater.instance.SetOfflineFailureText(failMessage);
			PhotonNetworkController.Instance.UpdateTriggerScreens();
			this.screenText.EnableFailedState(failMessage);
			this.functionSelectText.EnableFailedState(failMessage);
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x0005BE8E File Offset: 0x0005A08E
		private void RestoreFromFailureState()
		{
			GorillaScoreboardTotalUpdater.instance.ClearOfflineFailureText();
			PhotonNetworkController.Instance.UpdateTriggerScreens();
			this.screenText.DisableFailedState();
			this.functionSelectText.DisableFailedState();
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x0005BEBC File Offset: 0x0005A0BC
		public void GeneralFailureMessage(string failMessage)
		{
			this.isConnectedToMaster = false;
			NetworkSystem.Instance.SetWrongVersion();
			this.UpdateFailureText(failMessage);
			this.UpdateScreen();
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x0017CC90 File Offset: 0x0017AE90
		private static void OnErrorShared(PlayFabError error)
		{
			if (error.Error == PlayFabErrorCode.NotAuthenticated)
			{
				PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
			}
			else if (error.Error == PlayFabErrorCode.AccountBanned)
			{
				GorillaGameManager.ForceStopGame_DisconnectAndDestroy();
			}
			if (error.ErrorMessage == "The account making this request is currently banned")
			{
				using (Dictionary<string, List<string>>.Enumerator enumerator = error.ErrorDetails.GetEnumerator())
				{
					if (!enumerator.MoveNext())
					{
						return;
					}
					KeyValuePair<string, List<string>> keyValuePair = enumerator.Current;
					if (keyValuePair.Value[0] != "Indefinite")
					{
						GorillaComputer.instance.GeneralFailureMessage(string.Concat(new string[]
						{
							"YOUR ACCOUNT ",
							PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
							" HAS BEEN BANNED. YOU WILL NOT BE ABLE TO PLAY UNTIL THE BAN EXPIRES.\nREASON: ",
							keyValuePair.Key,
							"\nHOURS LEFT: ",
							((int)((DateTime.Parse(keyValuePair.Value[0]) - DateTime.UtcNow).TotalHours + 1.0)).ToString()
						}));
						return;
					}
					GorillaComputer.instance.GeneralFailureMessage("YOUR ACCOUNT " + PlayFabAuthenticator.instance.GetPlayFabPlayerId() + " HAS BEEN BANNED INDEFINITELY.\nREASON: " + keyValuePair.Key);
					return;
				}
			}
			if (error.ErrorMessage == "The IP making this request is currently banned")
			{
				using (Dictionary<string, List<string>>.Enumerator enumerator = error.ErrorDetails.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						KeyValuePair<string, List<string>> keyValuePair2 = enumerator.Current;
						if (keyValuePair2.Value[0] != "Indefinite")
						{
							GorillaComputer.instance.GeneralFailureMessage("THIS IP HAS BEEN BANNED. YOU WILL NOT BE ABLE TO PLAY UNTIL THE BAN EXPIRES.\nREASON: " + keyValuePair2.Key + "\nHOURS LEFT: " + ((int)((DateTime.Parse(keyValuePair2.Value[0]) - DateTime.UtcNow).TotalHours + 1.0)).ToString());
						}
						else
						{
							GorillaComputer.instance.GeneralFailureMessage("THIS IP HAS BEEN BANNED INDEFINITELY.\nREASON: " + keyValuePair2.Key);
						}
					}
				}
			}
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x0017CEE8 File Offset: 0x0017B0E8
		private void DecreaseState()
		{
			this.currentStateIndex--;
			if (this.GetState(this.currentStateIndex) == GorillaComputer.ComputerState.Time)
			{
				this.currentStateIndex--;
			}
			if (this.currentStateIndex < 0)
			{
				this.currentStateIndex = this.FunctionsCount - 1;
			}
			this.SwitchState(this.GetState(this.currentStateIndex), true);
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x0017CF4C File Offset: 0x0017B14C
		private void IncreaseState()
		{
			this.currentStateIndex++;
			if (this.GetState(this.currentStateIndex) == GorillaComputer.ComputerState.Time)
			{
				this.currentStateIndex++;
			}
			if (this.currentStateIndex >= this.FunctionsCount)
			{
				this.currentStateIndex = 0;
			}
			this.SwitchState(this.GetState(this.currentStateIndex), true);
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x0017CFB0 File Offset: 0x0017B1B0
		public GorillaComputer.ComputerState GetState(int index)
		{
			GorillaComputer.ComputerState state;
			try
			{
				state = this._activeOrderList[index].State;
			}
			catch
			{
				state = this._activeOrderList[0].State;
			}
			return state;
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x0017CFF8 File Offset: 0x0017B1F8
		public int GetStateIndex(GorillaComputer.ComputerState state)
		{
			return this._activeOrderList.FindIndex((GorillaComputer.StateOrderItem s) => s.State == state);
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x0017D02C File Offset: 0x0017B22C
		public string GetOrderListForScreen(GorillaComputer.ComputerState currentState)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int stateIndex = this.GetStateIndex(currentState);
			for (int i = 0; i < this.FunctionsCount; i++)
			{
				stringBuilder.Append(this.FunctionNames[i]);
				if (i == stateIndex)
				{
					stringBuilder.Append(this.Pointer);
				}
				if (i < this.FunctionsCount - 1)
				{
					stringBuilder.Append("\n");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600449B RID: 17563 RVA: 0x0005BEDC File Offset: 0x0005A0DC
		private void GetCurrentTime()
		{
			this.tryGetTimeAgain = true;
			PlayFabClientAPI.GetTime(new GetTimeRequest(), new Action<GetTimeResult>(this.OnGetTimeSuccess), new Action<PlayFabError>(this.OnGetTimeFailure), null, null);
		}

		// Token: 0x0600449C RID: 17564 RVA: 0x0017D09C File Offset: 0x0017B29C
		private void OnGetTimeSuccess(GetTimeResult result)
		{
			this.startupMillis = (long)(TimeSpan.FromTicks(result.Time.Ticks).TotalMilliseconds - (double)(Time.realtimeSinceStartup * 1000f));
			this.startupTime = result.Time - TimeSpan.FromSeconds((double)Time.realtimeSinceStartup);
			Action onServerTimeUpdated = this.OnServerTimeUpdated;
			if (onServerTimeUpdated == null)
			{
				return;
			}
			onServerTimeUpdated();
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x0017D104 File Offset: 0x0017B304
		private void OnGetTimeFailure(PlayFabError error)
		{
			this.startupMillis = (long)(TimeSpan.FromTicks(DateTime.UtcNow.Ticks).TotalMilliseconds - (double)(Time.realtimeSinceStartup * 1000f));
			this.startupTime = DateTime.UtcNow - TimeSpan.FromSeconds((double)Time.realtimeSinceStartup);
			Action onServerTimeUpdated = this.OnServerTimeUpdated;
			if (onServerTimeUpdated != null)
			{
				onServerTimeUpdated();
			}
			if (error.Error == PlayFabErrorCode.NotAuthenticated)
			{
				PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
				return;
			}
			if (error.Error == PlayFabErrorCode.AccountBanned)
			{
				GorillaGameManager.ForceStopGame_DisconnectAndDestroy();
			}
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x0005BF09 File Offset: 0x0005A109
		private void PlayerCountChangedCallback(NetPlayer player)
		{
			this.UpdateScreen();
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x0017D198 File Offset: 0x0017B398
		public void SetNameBySafety(bool isSafety)
		{
			if (!isSafety)
			{
				return;
			}
			PlayerPrefs.SetString("playerNameBackup", this.currentName);
			this.currentName = "gorilla" + UnityEngine.Random.Range(0, 9999).ToString().PadLeft(4, '0');
			this.savedName = this.currentName;
			NetworkSystem.Instance.SetMyNickName(this.currentName);
			this.SetNameTagText(this.currentName);
			PlayerPrefs.SetString("playerName", this.currentName);
			PlayerPrefs.Save();
			if (NetworkSystem.Instance.InRoom)
			{
				GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", RpcTarget.All, new object[]
				{
					this.redValue,
					this.greenValue,
					this.blueValue
				});
			}
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x0017D274 File Offset: 0x0017B474
		private void SetNameTagText(string newName)
		{
			for (int i = 0; i < this._offlineVRRigNametagText.Length; i++)
			{
				this._offlineVRRigNametagText[i].text = newName;
			}
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x0017D2A4 File Offset: 0x0017B4A4
		public void SetComputerSettingsBySafety(bool isSafety, GorillaComputer.ComputerState[] toFilterOut, bool shouldHide)
		{
			this._activeOrderList = this.OrderList;
			if (!isSafety)
			{
				this._activeOrderList = this.OrderList;
				if (this._filteredStates.Count > 0 && toFilterOut.Length != 0)
				{
					for (int i = 0; i < toFilterOut.Length; i++)
					{
						if (this._filteredStates.Contains(toFilterOut[i]))
						{
							this._filteredStates.Remove(toFilterOut[i]);
						}
					}
				}
			}
			else if (shouldHide)
			{
				for (int j = 0; j < toFilterOut.Length; j++)
				{
					if (!this._filteredStates.Contains(toFilterOut[j]))
					{
						this._filteredStates.Add(toFilterOut[j]);
					}
				}
			}
			if (this._filteredStates.Count > 0)
			{
				int k = 0;
				int num = this._activeOrderList.Count;
				while (k < num)
				{
					if (this._filteredStates.Contains(this._activeOrderList[k].State))
					{
						this._activeOrderList.RemoveAt(k);
						k--;
						num--;
					}
					k++;
				}
			}
			this.FunctionsCount = this._activeOrderList.Count;
			this.FunctionNames.Clear();
			this._activeOrderList.ForEach(delegate(GorillaComputer.StateOrderItem s)
			{
				string name = s.GetName();
				if (name.Length > this.highestCharacterCount)
				{
					this.highestCharacterCount = name.Length;
				}
				this.FunctionNames.Add(name);
			});
			for (int l = 0; l < this.FunctionsCount; l++)
			{
				int num2 = this.highestCharacterCount - this.FunctionNames[l].Length;
				for (int m = 0; m < num2; m++)
				{
					List<string> functionNames = this.FunctionNames;
					int index = l;
					functionNames[index] += " ";
				}
			}
			this.UpdateScreen();
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x0017D438 File Offset: 0x0017B638
		private void SetVoiceChatBySafety(bool voiceChatEnabled, Permission.ManagedByEnum managedBy)
		{
			bool isSafety = !voiceChatEnabled;
			this.SetComputerSettingsBySafety(isSafety, new GorillaComputer.ComputerState[]
			{
				GorillaComputer.ComputerState.Voice,
				GorillaComputer.ComputerState.AutoMute,
				GorillaComputer.ComputerState.Mic
			}, false);
			string @string = PlayerPrefs.GetString("voiceChatOn", "");
			switch (managedBy)
			{
			case Permission.ManagedByEnum.PLAYER:
				if (string.IsNullOrEmpty(@string))
				{
					this.voiceChatOn = (voiceChatEnabled ? "TRUE" : "FALSE");
				}
				else
				{
					this.voiceChatOn = @string;
				}
				break;
			case Permission.ManagedByEnum.GUARDIAN:
				if (string.IsNullOrEmpty(@string))
				{
					this.voiceChatOn = "FALSE";
				}
				else
				{
					this.voiceChatOn = (voiceChatEnabled ? @string : "FALSE");
				}
				break;
			case Permission.ManagedByEnum.PROHIBITED:
				this.voiceChatOn = "FALSE";
				break;
			}
			RigContainer.RefreshAllRigVoices();
			Debug.Log("[KID] On Session Update - Voice Chat Permission changed - Has enabled voiceChat? [" + voiceChatEnabled.ToString() + "]");
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x0005BF11 File Offset: 0x0005A111
		public static void RegisterOnNametagSettingChanged(Action<bool> callback)
		{
			GorillaComputer.onNametagSettingChangedAction = (Action<bool>)Delegate.Combine(GorillaComputer.onNametagSettingChangedAction, callback);
		}

		// Token: 0x060044A4 RID: 17572 RVA: 0x0005BF28 File Offset: 0x0005A128
		public static void UnregisterOnNametagSettingChanged(Action<bool> callback)
		{
			GorillaComputer.onNametagSettingChangedAction = (Action<bool>)Delegate.Remove(GorillaComputer.onNametagSettingChangedAction, callback);
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x0017D504 File Offset: 0x0017B704
		private void UpdateNametagSetting(bool newSettingValue)
		{
			this.NametagsEnabled = newSettingValue;
			if (this.NametagsEnabled)
			{
				NetworkSystem.Instance.SetMyNickName(this.savedName);
			}
			Action<bool> action = GorillaComputer.onNametagSettingChangedAction;
			if (action != null)
			{
				action(this.NametagsEnabled);
			}
			int value = this.NametagsEnabled ? 1 : 0;
			PlayerPrefs.SetInt(this.NameTagPlayerPref, value);
			PlayerPrefs.Save();
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x0002F75F File Offset: 0x0002D95F
		void IMatchmakingCallbacks.OnFriendListUpdate(List<Photon.Realtime.FriendInfo> friendList)
		{
		}

		// Token: 0x060044A7 RID: 17575 RVA: 0x0002F75F File Offset: 0x0002D95F
		void IMatchmakingCallbacks.OnCreatedRoom()
		{
		}

		// Token: 0x060044A8 RID: 17576 RVA: 0x0002F75F File Offset: 0x0002D95F
		void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)
		{
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x0002F75F File Offset: 0x0002D95F
		void IMatchmakingCallbacks.OnJoinedRoom()
		{
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x0002F75F File Offset: 0x0002D95F
		void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message)
		{
		}

		// Token: 0x060044AB RID: 17579 RVA: 0x0002F75F File Offset: 0x0002D95F
		void IMatchmakingCallbacks.OnLeftRoom()
		{
		}

		// Token: 0x060044AC RID: 17580 RVA: 0x0002F75F File Offset: 0x0002D95F
		void IMatchmakingCallbacks.OnPreLeavingRoom()
		{
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x0005BF3F File Offset: 0x0005A13F
		void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
		{
			if (returnCode == 32765)
			{
				this.roomFull = true;
			}
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x0005BF50 File Offset: 0x0005A150
		public void SetInVirtualStump(bool inVirtualStump)
		{
			this.playerInVirtualStump = inVirtualStump;
			this.roomToJoin = (this.playerInVirtualStump ? (this.virtualStumpRoomPrepend + this.roomToJoin) : this.roomToJoin.RemoveAll(this.virtualStumpRoomPrepend, StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x060044AF RID: 17583 RVA: 0x0005BF8C File Offset: 0x0005A18C
		public bool IsPlayerInVirtualStump()
		{
			return this.playerInVirtualStump;
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x0005BF94 File Offset: 0x0005A194
		private void InitializeKIdState()
		{
			KIDManager.RegisterSessionUpdateCallback_AnyPermission(new Action(this.OnSessionUpdate_GorillaComputer));
			KIDIntegration.RegisterOnInitialisationComplete(new Action(this.UpdateKidState));
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x0005BFB8 File Offset: 0x0005A1B8
		private void UpdateKidState()
		{
			this._currentScreentState = GorillaComputer.EKidScreenState.Ready;
			if (!KIDIntegration.HasFoundSession)
			{
				this._currentScreentState = GorillaComputer.EKidScreenState.Show_OTP;
			}
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x0005BFCF File Offset: 0x0005A1CF
		private void RequestUpdatedPermissions()
		{
			if (!KIDManager.KidEnabled)
			{
				return;
			}
			if (this._waitingForUpdatedSession)
			{
				return;
			}
			if (Time.time < this._nextUpdateAttemptTime)
			{
				return;
			}
			this._waitingForUpdatedSession = true;
			this.UpdateSession();
		}

		// Token: 0x060044B3 RID: 17587 RVA: 0x0017D564 File Offset: 0x0017B764
		private void UpdateSession()
		{
			GorillaComputer.<UpdateSession>d__273 <UpdateSession>d__;
			<UpdateSession>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<UpdateSession>d__.<>4__this = this;
			<UpdateSession>d__.<>1__state = -1;
			<UpdateSession>d__.<>t__builder.Start<GorillaComputer.<UpdateSession>d__273>(ref <UpdateSession>d__);
		}

		// Token: 0x060044B4 RID: 17588 RVA: 0x0005BFFD File Offset: 0x0005A1FD
		private void OnSessionUpdate_GorillaComputer()
		{
			this.UpdateKidState();
			this.UpdateScreen();
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x0005C00B File Offset: 0x0005A20B
		private void ProcessScreen_SetupKID()
		{
			if (KIDManager.KIDIntegration == null)
			{
				Debug.LogError("[KID] Unable to start k-ID Flow. Something went wrong previously and the k-ID Integration is NULL");
				return;
			}
			if (KIDIntegration.ChallengeStatus == AwaitChallengeResponse.StatusEnum.POLLTIMEOUT || KIDIntegration.ChallengeStatus == AwaitChallengeResponse.StatusEnum.FAIL)
			{
				KIDManager.KIDIntegration.RestartChallengeAwait();
				return;
			}
			this._currentScreentState = GorillaComputer.EKidScreenState.Show_OTP;
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x0017D59C File Offset: 0x0017B79C
		private bool GuardianConsentMessage(string setupKIDButtonName, string featureDescription)
		{
			GorillaText gorillaText = this.screenText;
			gorillaText.Text = gorillaText.Text + "PARENT/GUARDIAN PERMISSION REQUIRED TO " + featureDescription.ToUpper() + "!";
			if (this._waitingForUpdatedSession)
			{
				GorillaText gorillaText2 = this.screenText;
				gorillaText2.Text += "\n\nWAITING FOR PARENT/GUARDIAN CONSENT!";
				return true;
			}
			if (this._currentScreentState != GorillaComputer.EKidScreenState.Show_OTP)
			{
				if (Time.time >= this._nextUpdateAttemptTime)
				{
					GorillaText gorillaText3 = this.screenText;
					gorillaText3.Text += "\n\nPRESS OPTION 2 TO REFRESH PERMISSIONS!";
				}
				else
				{
					GorillaText gorillaText4 = this.screenText;
					gorillaText4.Text = gorillaText4.Text + "\n\nCHECK AGAIN IN " + ((int)(this._nextUpdateAttemptTime - Time.time)).ToString() + " SECONDS!";
				}
				return false;
			}
			KIDIntegration kidintegration = KIDManager.KIDIntegration;
			string text = ((kidintegration != null) ? kidintegration.GetOneTimePasscode() : null) ?? "INVALID PASSCODE";
			text == "INVALID PASSCODE";
			if (KIDManager.KIDIntegration == null)
			{
				GorillaText gorillaText5 = this.screenText;
				gorillaText5.Text += "\n\nUNABLE TO REACH K-ID!";
				GorillaText gorillaText6 = this.screenText;
				gorillaText6.Text += "\nRESTART THE GAME AND TRY AGAIN LATER";
				return true;
			}
			GorillaText gorillaText7 = this.screenText;
			gorillaText7.Text = gorillaText7.Text + "\n\nONE TIME PASSCODE IS:\t" + text;
			GorillaText gorillaText8 = this.screenText;
			gorillaText8.Text += "\n\nGIVE IT TO A PARENT/GUARDIAN TO ENTER IT AT: k-id.com/code";
			int num = (int)KIDManager.KIDIntegration.ChallengeTimeoutRemaining;
			if (num > 0)
			{
				GorillaText gorillaText9 = this.screenText;
				gorillaText9.Text = gorillaText9.Text + "\n\nYOU HAVE " + num.ToString() + " SECONDS BEFORE IT TIMES OUT!";
			}
			else
			{
				GorillaText gorillaText10 = this.screenText;
				gorillaText10.Text += "\nPRESS OPTION 2 TO REFRESH PERMISSIONS!";
			}
			return true;
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x0005C041 File Offset: 0x0005A241
		private void ProhibitedMessage(string verb)
		{
			this.screenText.Text = "";
			GorillaText gorillaText = this.screenText;
			gorillaText.Text = gorillaText.Text + "\n\nYOU ARE NOT ALLOWED TO " + verb + " IN YOUR JURISDICTION.";
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x0017D754 File Offset: 0x0017B954
		private void RoomScreen_KIdPermission()
		{
			if (!GorillaServer.Instance.UseKID())
			{
				this.screenText.Text = "YOU CANNOT USE THE PRIVATE ROOM FEATURE RIGHT NOW";
				return;
			}
			this.screenText.Text = "";
			this.GuardianConsentMessage("OPTION 3", "JOIN PRIVATE ROOMS");
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x0005C074 File Offset: 0x0005A274
		private void RoomScreen_KIdProhibited()
		{
			this.ProhibitedMessage("CREATE OR JOIN PRIVATE ROOMS");
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x0017D7A4 File Offset: 0x0017B9A4
		private void VoiceScreen_KIdPermission()
		{
			this.screenText.Text = "VOICE TYPE: \"MONKE\"\n\n";
			if (!GorillaServer.Instance.UseKID())
			{
				GorillaText gorillaText = this.screenText;
				gorillaText.Text += "YOU CANNOT USE THE HUMAN VOICE TYPE FEATURE RIGHT NOW";
				return;
			}
			this.GuardianConsentMessage("OPTION 3", "ENABLE HUMAN VOICE CHAT");
		}

		// Token: 0x060044BB RID: 17595 RVA: 0x0005C081 File Offset: 0x0005A281
		private void VoiceScreen_KIdProhibited()
		{
			this.ProhibitedMessage("USE THE VOICE CHAT");
		}

		// Token: 0x060044BC RID: 17596 RVA: 0x0005C08E File Offset: 0x0005A28E
		private void MicScreen_KIdPermission()
		{
			this.screenText.Text = "";
			this.GuardianConsentMessage("OPTION 3", "ENABLE HUMAN VOICE CHAT");
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x0005C0B1 File Offset: 0x0005A2B1
		private void MicScreen_KIdProhibited()
		{
			this.VoiceScreen_KIdProhibited();
		}

		// Token: 0x060044BE RID: 17598 RVA: 0x0017D7FC File Offset: 0x0017B9FC
		private void NameScreen_KIdPermission()
		{
			if (!GorillaServer.Instance.UseKID())
			{
				this.screenText.Text = "YOU CANNOT USE THE CUSTOM NICKNAME FEATURE RIGHT NOW";
				return;
			}
			this.screenText.Text = "";
			this.GuardianConsentMessage("OPTION 3", "SET CUSTOM NICKNAMES");
		}

		// Token: 0x060044BF RID: 17599 RVA: 0x0005C0B9 File Offset: 0x0005A2B9
		private void NameScreen_KIdProhibited()
		{
			this.ProhibitedMessage("SET CUSTOM NICKNAMES");
		}

		// Token: 0x060044C0 RID: 17600 RVA: 0x0017D84C File Offset: 0x0017BA4C
		private void OnKIDSessionUpdated_CustomNicknames(bool showCustomNames, Permission.ManagedByEnum managedBy)
		{
			bool flag = (showCustomNames || managedBy == Permission.ManagedByEnum.PLAYER) && managedBy != Permission.ManagedByEnum.PROHIBITED;
			this.SetComputerSettingsBySafety(!flag, new GorillaComputer.ComputerState[]
			{
				GorillaComputer.ComputerState.Name
			}, false);
			int @int = PlayerPrefs.GetInt(this.NameTagPlayerPref, -1);
			bool flag2 = @int > 0;
			switch (managedBy)
			{
			case Permission.ManagedByEnum.PLAYER:
				if (showCustomNames)
				{
					this.NametagsEnabled = (@int == -1 || flag2);
				}
				else
				{
					this.NametagsEnabled = (@int != -1 && flag2);
				}
				break;
			case Permission.ManagedByEnum.GUARDIAN:
				this.NametagsEnabled = (showCustomNames && flag2);
				break;
			case Permission.ManagedByEnum.PROHIBITED:
				this.NametagsEnabled = false;
				break;
			}
			if (this.NametagsEnabled)
			{
				NetworkSystem.Instance.SetMyNickName(this.savedName);
			}
			Action<bool> action = GorillaComputer.onNametagSettingChangedAction;
			if (action == null)
			{
				return;
			}
			action(this.NametagsEnabled);
		}

		// Token: 0x060044C1 RID: 17601 RVA: 0x0005C0C6 File Offset: 0x0005A2C6
		private void TroopScreen_KIdPermission()
		{
			this.screenText.Text = "";
			this.GuardianConsentMessage("OPTION 3", "JOIN TROOPS");
		}

		// Token: 0x060044C2 RID: 17602 RVA: 0x0005C0E9 File Offset: 0x0005A2E9
		private void TroopScreen_KIdProhibited()
		{
			this.ProhibitedMessage("CREATE OR JOIN TROOPS");
		}

		// Token: 0x060044C3 RID: 17603 RVA: 0x0005C0F6 File Offset: 0x0005A2F6
		private void ProcessKIdState(GorillaKeyboardBindings buttonPressed)
		{
			if (buttonPressed == GorillaKeyboardBindings.option1 && this._currentScreentState == GorillaComputer.EKidScreenState.Ready)
			{
				this.RequestUpdatedPermissions();
			}
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x0005C10B File Offset: 0x0005A30B
		private void KIdScreen()
		{
			if (KIDManager.KIDIntegration == null || this._currentScreentState == GorillaComputer.EKidScreenState.Show_OTP || !KIDIntegration.HasFoundSession)
			{
				this.GuardianConsentMessage("OPTION 3", "");
				return;
			}
			this.KIdScreen_DisplayPermissions();
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x0017D910 File Offset: 0x0017BB10
		private void KIdScreen_DisplayPermissions()
		{
			AgeStatusType activeAccountStatus = KIDManager.Instance.GetActiveAccountStatus();
			string str = (activeAccountStatus == (AgeStatusType)0) ? "NOT READY" : activeAccountStatus.ToString();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("k-ID Account Status:\t" + str);
			if (activeAccountStatus == (AgeStatusType)0)
			{
				stringBuilder.AppendLine("\nPress 'OPTION 1' to get permissions!");
				this.screenText.Text = stringBuilder.ToString();
				return;
			}
			if (this._waitingForUpdatedSession)
			{
				stringBuilder.AppendLine("\nWAITING FOR PARENT/GUARDIAN CONSENT!");
				this.screenText.Text = stringBuilder.ToString();
				return;
			}
			stringBuilder.AppendLine("\nPermissions:");
			List<SKIDPermissionData> allPermissionsData = KIDManager.Instance.GetAllPermissionsData();
			int count = allPermissionsData.Count;
			int num = 1;
			for (int i = 0; i < count; i++)
			{
				if (this._interestedKIDPermissionNames.Contains(allPermissionsData[i].PermissionName))
				{
					string text = allPermissionsData[i].PermissionEnabled ? "<color=#85ffa5>" : "<color=\"RED\">";
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"[",
						num.ToString(),
						"] ",
						text,
						allPermissionsData[i].PermissionName,
						"</color>"
					}));
					num++;
				}
			}
			stringBuilder.AppendLine("\nTO REFRESH PERMISSIONS PRESS OPTION 1!");
			this.screenText.Text = stringBuilder.ToString();
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x0017DA88 File Offset: 0x0017BC88
		private void KIdScreen_ShowOTP()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("k-ID Permission Setup");
			stringBuilder.AppendLine("\nEnter this code on the Family Portal:\n\t\t" + KIDManager.KIDIntegration.GetOneTimePasscode());
			stringBuilder.AppendLine("\nFamily Portal: k-id.com/code");
			this.screenText.Text = stringBuilder.ToString();
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x00030F9B File Offset: 0x0002F19B
		bool IGorillaSliceableSimple.get_isActiveAndEnabled()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x060044CB RID: 17611 RVA: 0x0005C155 File Offset: 0x0005A355
		[CompilerGenerated]
		private IEnumerator <LoadingScreen>g__LoadingScreenLocal|199_0()
		{
			int dotsCount = 0;
			while (this.currentState == GorillaComputer.ComputerState.Loading)
			{
				int num = dotsCount;
				dotsCount = num + 1;
				if (dotsCount == 3)
				{
					dotsCount = 0;
				}
				this.screenText.Text = "LOADING";
				for (int i = 0; i < dotsCount; i++)
				{
					GorillaText gorillaText = this.screenText;
					gorillaText.Text += ". ";
				}
				yield return this.waitOneSecond;
			}
			yield break;
		}

		// Token: 0x04004590 RID: 17808
		private const bool HIDE_SCREENS = false;

		// Token: 0x04004591 RID: 17809
		public const string NAMETAG_PLAYER_PREF_KEY = "nameTagsOn";

		// Token: 0x04004592 RID: 17810
		[OnEnterPlay_SetNull]
		public static volatile GorillaComputer instance;

		// Token: 0x04004593 RID: 17811
		[OnEnterPlay_Set(false)]
		public static bool hasInstance;

		// Token: 0x04004594 RID: 17812
		[OnEnterPlay_SetNull]
		private static Action<bool> onNametagSettingChangedAction;

		// Token: 0x04004595 RID: 17813
		public bool tryGetTimeAgain;

		// Token: 0x04004596 RID: 17814
		public Material unpressedMaterial;

		// Token: 0x04004597 RID: 17815
		public Material pressedMaterial;

		// Token: 0x04004598 RID: 17816
		public string currentTextField;

		// Token: 0x04004599 RID: 17817
		public float buttonFadeTime;

		// Token: 0x0400459A RID: 17818
		public string offlineTextInitialString;

		// Token: 0x0400459B RID: 17819
		public GorillaText screenText;

		// Token: 0x0400459C RID: 17820
		public GorillaText functionSelectText;

		// Token: 0x0400459D RID: 17821
		public GorillaText wallScreenText;

		// Token: 0x0400459E RID: 17822
		[SerializeField]
		[FormerlySerializedAs("offlineVRRigNametagText")]
		private TMP_Text[] _offlineVRRigNametagText;

		// Token: 0x0400459F RID: 17823
		public string versionMismatch = "PLEASE UPDATE TO THE LATEST VERSION OF GORILLA TAG. YOU'RE ON AN OLD VERSION. FEEL FREE TO RUN AROUND, BUT YOU WON'T BE ABLE TO PLAY WITH ANYONE ELSE.";

		// Token: 0x040045A0 RID: 17824
		public string unableToConnect = "UNABLE TO CONNECT TO THE INTERNET. PLEASE CHECK YOUR CONNECTION AND RESTART THE GAME.";

		// Token: 0x040045A1 RID: 17825
		public Material wrongVersionMaterial;

		// Token: 0x040045A2 RID: 17826
		public MeshRenderer wallScreenRenderer;

		// Token: 0x040045A3 RID: 17827
		public MeshRenderer computerScreenRenderer;

		// Token: 0x040045A4 RID: 17828
		public long startupMillis;

		// Token: 0x040045A5 RID: 17829
		public DateTime startupTime;

		// Token: 0x040045A6 RID: 17830
		public string lastPressedGameMode;

		// Token: 0x040045A7 RID: 17831
		public WatchableStringSO currentGameMode;

		// Token: 0x040045A8 RID: 17832
		public WatchableStringSO currentGameModeText;

		// Token: 0x040045A9 RID: 17833
		public int includeUpdatedServerSynchTest;

		// Token: 0x040045AA RID: 17834
		public PhotonNetworkController networkController;

		// Token: 0x040045AB RID: 17835
		public float updateCooldown = 1f;

		// Token: 0x040045AC RID: 17836
		public float lastUpdateTime;

		// Token: 0x040045AD RID: 17837
		public bool isConnectedToMaster;

		// Token: 0x040045AE RID: 17838
		public bool internetFailure;

		// Token: 0x040045AF RID: 17839
		public string[] allowedMapsToJoin;

		// Token: 0x040045B0 RID: 17840
		[Header("State vars")]
		public bool stateUpdated;

		// Token: 0x040045B1 RID: 17841
		public bool screenChanged;

		// Token: 0x040045B2 RID: 17842
		public bool initialized;

		// Token: 0x040045B3 RID: 17843
		public List<GorillaComputer.StateOrderItem> OrderList = new List<GorillaComputer.StateOrderItem>
		{
			new GorillaComputer.StateOrderItem(GorillaComputer.ComputerState.Room),
			new GorillaComputer.StateOrderItem(GorillaComputer.ComputerState.Name),
			new GorillaComputer.StateOrderItem(GorillaComputer.ComputerState.Color),
			new GorillaComputer.StateOrderItem(GorillaComputer.ComputerState.Turn),
			new GorillaComputer.StateOrderItem(GorillaComputer.ComputerState.Mic),
			new GorillaComputer.StateOrderItem(GorillaComputer.ComputerState.Queue),
			new GorillaComputer.StateOrderItem(GorillaComputer.ComputerState.Troop),
			new GorillaComputer.StateOrderItem(GorillaComputer.ComputerState.Group),
			new GorillaComputer.StateOrderItem(GorillaComputer.ComputerState.Voice),
			new GorillaComputer.StateOrderItem(GorillaComputer.ComputerState.AutoMute, "Automod"),
			new GorillaComputer.StateOrderItem(GorillaComputer.ComputerState.Visuals, "Items"),
			new GorillaComputer.StateOrderItem(GorillaComputer.ComputerState.Credits),
			new GorillaComputer.StateOrderItem(GorillaComputer.ComputerState.Support)
		};

		// Token: 0x040045B4 RID: 17844
		public string Pointer = "<-";

		// Token: 0x040045B5 RID: 17845
		public int highestCharacterCount;

		// Token: 0x040045B6 RID: 17846
		public List<string> FunctionNames = new List<string>();

		// Token: 0x040045B7 RID: 17847
		public int FunctionsCount;

		// Token: 0x040045B8 RID: 17848
		[Header("Room vars")]
		public string roomToJoin;

		// Token: 0x040045B9 RID: 17849
		public bool roomFull;

		// Token: 0x040045BA RID: 17850
		public bool roomNotAllowed;

		// Token: 0x040045BB RID: 17851
		[Header("Mic vars")]
		public string pttType;

		// Token: 0x040045BC RID: 17852
		[Header("Automute vars")]
		public string autoMuteType;

		// Token: 0x040045BD RID: 17853
		[Header("Queue vars")]
		public string currentQueue;

		// Token: 0x040045BE RID: 17854
		public bool allowedInCompetitive;

		// Token: 0x040045BF RID: 17855
		[Header("Group Vars")]
		public string groupMapJoin;

		// Token: 0x040045C0 RID: 17856
		public int groupMapJoinIndex;

		// Token: 0x040045C1 RID: 17857
		public GorillaFriendCollider friendJoinCollider;

		// Token: 0x040045C2 RID: 17858
		[Header("Troop vars")]
		public string troopName;

		// Token: 0x040045C3 RID: 17859
		public bool troopQueueActive;

		// Token: 0x040045C4 RID: 17860
		public string troopToJoin;

		// Token: 0x040045C5 RID: 17861
		[Header("Join Triggers")]
		public Dictionary<string, GorillaNetworkJoinTrigger> primaryTriggersByZone = new Dictionary<string, GorillaNetworkJoinTrigger>();

		// Token: 0x040045C6 RID: 17862
		public string voiceChatOn;

		// Token: 0x040045C7 RID: 17863
		[Header("Mode select vars")]
		public ModeSelectButton[] modeSelectButtons;

		// Token: 0x040045C8 RID: 17864
		public string version;

		// Token: 0x040045C9 RID: 17865
		public string buildDate;

		// Token: 0x040045CA RID: 17866
		public string buildCode;

		// Token: 0x040045CB RID: 17867
		[Header("Cosmetics")]
		public bool disableParticles;

		// Token: 0x040045CC RID: 17868
		public float instrumentVolume;

		// Token: 0x040045CD RID: 17869
		[Header("Credits")]
		public CreditsView creditsView;

		// Token: 0x040045CE RID: 17870
		[Header("Handedness")]
		public bool leftHanded;

		// Token: 0x040045CF RID: 17871
		[Header("Name state vars")]
		public string savedName;

		// Token: 0x040045D0 RID: 17872
		public string currentName;

		// Token: 0x040045D1 RID: 17873
		public TextAsset exactOneWeekFile;

		// Token: 0x040045D2 RID: 17874
		public TextAsset anywhereOneWeekFile;

		// Token: 0x040045D3 RID: 17875
		public TextAsset anywhereTwoWeekFile;

		// Token: 0x040045D4 RID: 17876
		private List<GorillaComputer.ComputerState> _filteredStates = new List<GorillaComputer.ComputerState>();

		// Token: 0x040045D5 RID: 17877
		private List<GorillaComputer.StateOrderItem> _activeOrderList = new List<GorillaComputer.StateOrderItem>();

		// Token: 0x040045D6 RID: 17878
		private Stack<GorillaComputer.ComputerState> stateStack = new Stack<GorillaComputer.ComputerState>();

		// Token: 0x040045D7 RID: 17879
		private GorillaComputer.ComputerState currentComputerState;

		// Token: 0x040045D8 RID: 17880
		private GorillaComputer.ComputerState previousComputerState;

		// Token: 0x040045D9 RID: 17881
		private int currentStateIndex;

		// Token: 0x040045DA RID: 17882
		private int usersBanned;

		// Token: 0x040045DB RID: 17883
		private float redValue;

		// Token: 0x040045DC RID: 17884
		private string redText;

		// Token: 0x040045DD RID: 17885
		private float blueValue;

		// Token: 0x040045DE RID: 17886
		private string blueText;

		// Token: 0x040045DF RID: 17887
		private float greenValue;

		// Token: 0x040045E0 RID: 17888
		private string greenText;

		// Token: 0x040045E1 RID: 17889
		private int colorCursorLine;

		// Token: 0x040045E2 RID: 17890
		private int turnValue;

		// Token: 0x040045E3 RID: 17891
		private string turnType;

		// Token: 0x040045E4 RID: 17892
		private GorillaSnapTurn gorillaTurn;

		// Token: 0x040045E5 RID: 17893
		private string warningConfirmationInputString = string.Empty;

		// Token: 0x040045E6 RID: 17894
		private bool displaySupport;

		// Token: 0x040045E7 RID: 17895
		private string[] exactOneWeek;

		// Token: 0x040045E8 RID: 17896
		private string[] anywhereOneWeek;

		// Token: 0x040045E9 RID: 17897
		private string[] anywhereTwoWeek;

		// Token: 0x040045EA RID: 17898
		private GorillaComputer.RedemptionResult redemptionResult;

		// Token: 0x040045EB RID: 17899
		private string redemptionCode = "";

		// Token: 0x040045EC RID: 17900
		private bool playerInVirtualStump;

		// Token: 0x040045ED RID: 17901
		private string virtualStumpRoomPrepend = "";

		// Token: 0x040045EE RID: 17902
		private WaitForSeconds waitOneSecond = new WaitForSeconds(1f);

		// Token: 0x040045EF RID: 17903
		private Coroutine LoadingRoutine;

		// Token: 0x040045F0 RID: 17904
		private List<string> topTroops = new List<string>();

		// Token: 0x040045F1 RID: 17905
		private bool hasRequestedInitialTroopPopulation;

		// Token: 0x040045F2 RID: 17906
		private int currentTroopPopulation = -1;

		// Token: 0x040045F4 RID: 17908
		private float lastCheckedWifi;

		// Token: 0x040045F5 RID: 17909
		private float checkIfDisconnectedSeconds = 10f;

		// Token: 0x040045F6 RID: 17910
		private float checkIfConnectedSeconds = 1f;

		// Token: 0x040045F7 RID: 17911
		private float troopPopulationCheckCooldown = 3f;

		// Token: 0x040045F8 RID: 17912
		private float nextPopulationCheckTime;

		// Token: 0x040045F9 RID: 17913
		public Action OnServerTimeUpdated;

		// Token: 0x040045FA RID: 17914
		private const string ENABLED_COLOUR = "#85ffa5";

		// Token: 0x040045FB RID: 17915
		private const string DISABLED_COLOUR = "\"RED\"";

		// Token: 0x040045FC RID: 17916
		private const string FAMILY_PORTAL_URL = "k-id.com/code";

		// Token: 0x040045FD RID: 17917
		private float _updateAttemptCooldown = 60f;

		// Token: 0x040045FE RID: 17918
		private float _nextUpdateAttemptTime;

		// Token: 0x040045FF RID: 17919
		private bool _waitingForUpdatedSession;

		// Token: 0x04004600 RID: 17920
		private GorillaComputer.EKidScreenState _currentScreentState = GorillaComputer.EKidScreenState.Show_OTP;

		// Token: 0x04004601 RID: 17921
		private string[] _interestedKIDPermissionNames = new string[]
		{
			"custom-username",
			"voice-chat",
			"join-groups"
		};

		// Token: 0x02000AAB RID: 2731
		public enum ComputerState
		{
			// Token: 0x04004603 RID: 17923
			Startup,
			// Token: 0x04004604 RID: 17924
			Color,
			// Token: 0x04004605 RID: 17925
			Name,
			// Token: 0x04004606 RID: 17926
			Turn,
			// Token: 0x04004607 RID: 17927
			Mic,
			// Token: 0x04004608 RID: 17928
			Room,
			// Token: 0x04004609 RID: 17929
			Queue,
			// Token: 0x0400460A RID: 17930
			Group,
			// Token: 0x0400460B RID: 17931
			Voice,
			// Token: 0x0400460C RID: 17932
			AutoMute,
			// Token: 0x0400460D RID: 17933
			Credits,
			// Token: 0x0400460E RID: 17934
			Visuals,
			// Token: 0x0400460F RID: 17935
			Time,
			// Token: 0x04004610 RID: 17936
			NameWarning,
			// Token: 0x04004611 RID: 17937
			Loading,
			// Token: 0x04004612 RID: 17938
			Support,
			// Token: 0x04004613 RID: 17939
			Troop,
			// Token: 0x04004614 RID: 17940
			KID,
			// Token: 0x04004615 RID: 17941
			Redemption
		}

		// Token: 0x02000AAC RID: 2732
		private enum NameCheckResult
		{
			// Token: 0x04004617 RID: 17943
			Success,
			// Token: 0x04004618 RID: 17944
			Warning,
			// Token: 0x04004619 RID: 17945
			Ban
		}

		// Token: 0x02000AAD RID: 2733
		public enum RedemptionResult
		{
			// Token: 0x0400461B RID: 17947
			Empty,
			// Token: 0x0400461C RID: 17948
			Invalid,
			// Token: 0x0400461D RID: 17949
			Checking,
			// Token: 0x0400461E RID: 17950
			AlreadyUsed,
			// Token: 0x0400461F RID: 17951
			Success
		}

		// Token: 0x02000AAE RID: 2734
		[Serializable]
		public class StateOrderItem
		{
			// Token: 0x060044CD RID: 17613 RVA: 0x0005C164 File Offset: 0x0005A364
			public StateOrderItem()
			{
			}

			// Token: 0x060044CE RID: 17614 RVA: 0x0005C177 File Offset: 0x0005A377
			public StateOrderItem(GorillaComputer.ComputerState state)
			{
				this.State = state;
			}

			// Token: 0x060044CF RID: 17615 RVA: 0x0005C191 File Offset: 0x0005A391
			public StateOrderItem(GorillaComputer.ComputerState state, string overrideName)
			{
				this.State = state;
				this.OverrideName = overrideName;
			}

			// Token: 0x060044D0 RID: 17616 RVA: 0x0005C1B2 File Offset: 0x0005A3B2
			public string GetName()
			{
				if (!string.IsNullOrEmpty(this.OverrideName))
				{
					return this.OverrideName.ToUpper();
				}
				return this.State.ToString().ToUpper();
			}

			// Token: 0x04004620 RID: 17952
			public GorillaComputer.ComputerState State;

			// Token: 0x04004621 RID: 17953
			[Tooltip("Case not important - ToUpper applied at runtime")]
			public string OverrideName = "";
		}

		// Token: 0x02000AAF RID: 2735
		private enum EKidScreenState
		{
			// Token: 0x04004623 RID: 17955
			Ready,
			// Token: 0x04004624 RID: 17956
			Show_OTP,
			// Token: 0x04004625 RID: 17957
			Show_Setup_Screen
		}
	}
}
