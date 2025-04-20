using System;
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
	// Token: 0x02000AD3 RID: 2771
	public class GorillaComputer : MonoBehaviour, IMatchmakingCallbacks, IGorillaSliceableSimple
	{
		// Token: 0x06004557 RID: 17751 RVA: 0x0005D36F File Offset: 0x0005B56F
		public DateTime GetServerTime()
		{
			return this.startupTime + TimeSpan.FromSeconds((double)Time.realtimeSinceStartup);
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06004558 RID: 17752 RVA: 0x0005D387 File Offset: 0x0005B587
		public string VStumpRoomPrepend
		{
			get
			{
				return this.virtualStumpRoomPrepend;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06004559 RID: 17753 RVA: 0x00180B7C File Offset: 0x0017ED7C
		public GorillaComputer.ComputerState currentState
		{
			get
			{
				GorillaComputer.ComputerState result;
				this.stateStack.TryPeek(out result);
				return result;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x0600455A RID: 17754 RVA: 0x0005D38F File Offset: 0x0005B58F
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

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x0600455B RID: 17755 RVA: 0x0005D3C6 File Offset: 0x0005B5C6
		// (set) Token: 0x0600455C RID: 17756 RVA: 0x0005D3CE File Offset: 0x0005B5CE
		public bool NametagsEnabled { get; private set; }

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x0600455D RID: 17757 RVA: 0x0005D3D7 File Offset: 0x0005B5D7
		public TMP_Text offlineVRRigNametagText
		{
			get
			{
				return this._offlineVRRigNametagText[0];
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x0600455E RID: 17758 RVA: 0x0005D3E1 File Offset: 0x0005B5E1
		// (set) Token: 0x0600455F RID: 17759 RVA: 0x0005D3E9 File Offset: 0x0005B5E9
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

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06004560 RID: 17760 RVA: 0x0005D3F8 File Offset: 0x0005B5F8
		// (set) Token: 0x06004561 RID: 17761 RVA: 0x0005D400 File Offset: 0x0005B600
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

		// Token: 0x06004562 RID: 17762 RVA: 0x00180B98 File Offset: 0x0017ED98
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

		// Token: 0x06004563 RID: 17763 RVA: 0x0005D409 File Offset: 0x0005B609
		private void Start()
		{
			Debug.Log("Computer Init");
			this.Initialise();
		}

		// Token: 0x06004564 RID: 17764 RVA: 0x0005D41B File Offset: 0x0005B61B
		public void OnEnable()
		{
			KIDManager.RegisterSessionUpdatedCallback_VoiceChat(new Action<bool, Permission.ManagedByEnum>(this.SetVoiceChatBySafety));
			KIDManager.RegisterSessionUpdatedCallback_CustomUsernames(new Action<bool, Permission.ManagedByEnum>(this.OnKIDSessionUpdated_CustomNicknames));
			GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06004565 RID: 17765 RVA: 0x0005D446 File Offset: 0x0005B646
		public void OnDisable()
		{
			KIDManager.UnregisterSessionUpdatedCallback_VoiceChat(new Action<bool, Permission.ManagedByEnum>(this.SetVoiceChatBySafety));
			KIDManager.UnregisterSessionUpdatedCallback_CustomUsernames(new Action<bool, Permission.ManagedByEnum>(this.OnKIDSessionUpdated_CustomNicknames));
			GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06004566 RID: 17766 RVA: 0x0005D471 File Offset: 0x0005B671
		protected void OnDestroy()
		{
			if (GorillaComputer.instance == this)
			{
				GorillaComputer.hasInstance = false;
				GorillaComputer.instance = null;
			}
			KIDManager.UnregisterSessionUpdateCallback_AnyPermission(new Action(this.OnSessionUpdate_GorillaComputer));
		}

		// Token: 0x06004567 RID: 17767 RVA: 0x00180BEC File Offset: 0x0017EDEC
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

		// Token: 0x06004568 RID: 17768 RVA: 0x00180CB0 File Offset: 0x0017EEB0
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

		// Token: 0x06004569 RID: 17769 RVA: 0x00180D94 File Offset: 0x0017EF94
		private void InitialiseRoomScreens()
		{
			this.screenText.Initialize(this.computerScreenRenderer.materials, this.wrongVersionMaterial, GameEvents.ScreenTextChangedEvent, GameEvents.ScreenTextMaterialsEvent);
			this.functionSelectText.Initialize(this.computerScreenRenderer.materials, this.wrongVersionMaterial, GameEvents.FunctionSelectTextChangedEvent, null);
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x00180DEC File Offset: 0x0017EFEC
		private void InitialiseStrings()
		{
			this.roomToJoin = "";
			this.redText = "";
			this.blueText = "";
			this.greenText = "";
			this.currentName = "";
			this.savedName = "";
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x00180E3C File Offset: 0x0017F03C
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

		// Token: 0x0600456C RID: 17772 RVA: 0x00030607 File Offset: 0x0002E807
		private void InitializeStartupState()
		{
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x00030607 File Offset: 0x0002E807
		private void InitializeRoomState()
		{
		}

		// Token: 0x0600456E RID: 17774 RVA: 0x00180EB8 File Offset: 0x0017F0B8
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

		// Token: 0x0600456F RID: 17775 RVA: 0x00180F84 File Offset: 0x0017F184
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

		// Token: 0x06004570 RID: 17776 RVA: 0x00181154 File Offset: 0x0017F354
		private void InitializeTurnState()
		{
			this.gorillaTurn = GorillaTagger.Instance.GetComponent<GorillaSnapTurn>();
			string defaultValue = (Application.platform == RuntimePlatform.Android) ? "NONE" : "SNAP";
			this.turnType = PlayerPrefs.GetString("stickTurning", defaultValue);
			this.turnValue = PlayerPrefs.GetInt("turnFactor", 4);
			this.gorillaTurn.ChangeTurnMode(this.turnType, this.turnValue);
		}

		// Token: 0x06004571 RID: 17777 RVA: 0x0005D4A1 File Offset: 0x0005B6A1
		private void InitializeMicState()
		{
			this.pttType = PlayerPrefs.GetString("pttType", "ALL CHAT");
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x001811C0 File Offset: 0x0017F3C0
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

		// Token: 0x06004573 RID: 17779 RVA: 0x00181208 File Offset: 0x0017F408
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

		// Token: 0x06004574 RID: 17780 RVA: 0x0005D4B8 File Offset: 0x0005B6B8
		private void InitializeGroupState()
		{
			this.groupMapJoin = PlayerPrefs.GetString("groupMapJoin", "FOREST");
			this.groupMapJoinIndex = PlayerPrefs.GetInt("groupMapJoinIndex", 0);
			this.allowedMapsToJoin = this.friendJoinCollider.myAllowedMapsToJoin;
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x00181278 File Offset: 0x0017F478
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

		// Token: 0x06004576 RID: 17782 RVA: 0x0005D4F1 File Offset: 0x0005B6F1
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

		// Token: 0x06004577 RID: 17783 RVA: 0x00181328 File Offset: 0x0017F528
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

		// Token: 0x06004578 RID: 17784 RVA: 0x001813D4 File Offset: 0x0017F5D4
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

		// Token: 0x06004579 RID: 17785 RVA: 0x00030607 File Offset: 0x0002E807
		private void InitializeCreditsState()
		{
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x0005D500 File Offset: 0x0005B700
		private void InitializeTimeState()
		{
			BetterDayNightManager.instance.currentSetting = TimeSettings.Normal;
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x0005D50F File Offset: 0x0005B70F
		private void InitializeSupportState()
		{
			this.displaySupport = false;
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x00181494 File Offset: 0x0017F694
		private void InitializeVisualsState()
		{
			this.disableParticles = (PlayerPrefs.GetString("disableParticles", "FALSE") == "TRUE");
			GorillaTagger.Instance.ShowCosmeticParticles(!this.disableParticles);
			this.instrumentVolume = PlayerPrefs.GetFloat("instrumentVolume", 0.1f);
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x0005D518 File Offset: 0x0005B718
		private void InitializeRedeemState()
		{
			this.RedemptionStatus = GorillaComputer.RedemptionResult.Empty;
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x0005D521 File Offset: 0x0005B721
		private bool CheckInternetConnection()
		{
			return Application.internetReachability > NetworkReachability.NotReachable;
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x001814E8 File Offset: 0x0017F6E8
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

		// Token: 0x06004580 RID: 17792 RVA: 0x001815B0 File Offset: 0x0017F7B0
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

		// Token: 0x06004581 RID: 17793 RVA: 0x001816EC File Offset: 0x0017F8EC
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

		// Token: 0x06004582 RID: 17794 RVA: 0x0018175C File Offset: 0x0017F95C
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

		// Token: 0x06004583 RID: 17795 RVA: 0x001818A0 File Offset: 0x0017FAA0
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

		// Token: 0x06004584 RID: 17796 RVA: 0x0005D52B File Offset: 0x0005B72B
		public void SetGameModeWithoutButton(string gameMode)
		{
			this.currentGameMode.Value = gameMode;
			this.UpdateGameModeText();
			PhotonNetworkController.Instance.UpdateTriggerScreens();
		}

		// Token: 0x06004585 RID: 17797 RVA: 0x0005D54B File Offset: 0x0005B74B
		public void RegisterPrimaryJoinTrigger(GorillaNetworkJoinTrigger trigger)
		{
			this.primaryTriggersByZone[trigger.networkZone] = trigger;
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x00181904 File Offset: 0x0017FB04
		private GorillaNetworkJoinTrigger GetSelectedMapJoinTrigger()
		{
			GorillaNetworkJoinTrigger result;
			this.primaryTriggersByZone.TryGetValue(this.allowedMapsToJoin[Mathf.Min(this.allowedMapsToJoin.Length - 1, this.groupMapJoinIndex)], out result);
			return result;
		}

		// Token: 0x06004587 RID: 17799 RVA: 0x0018193C File Offset: 0x0017FB3C
		public GorillaNetworkJoinTrigger GetJoinTriggerForZone(string zone)
		{
			GorillaNetworkJoinTrigger result;
			this.primaryTriggersByZone.TryGetValue(zone, out result);
			return result;
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x0018195C File Offset: 0x0017FB5C
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

		// Token: 0x06004589 RID: 17801 RVA: 0x001819C4 File Offset: 0x0017FBC4
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

		// Token: 0x0600458A RID: 17802 RVA: 0x0005D55F File Offset: 0x0005B75F
		public void CompQueueUnlockButtonPress()
		{
			this.allowedInCompetitive = true;
			PlayerPrefs.SetInt("allowedInCompetitive", 1);
			PlayerPrefs.Save();
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x00181BB4 File Offset: 0x0017FDB4
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

		// Token: 0x0600458C RID: 17804 RVA: 0x0005D578 File Offset: 0x0005B778
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

		// Token: 0x0600458D RID: 17805 RVA: 0x0005D5B1 File Offset: 0x0005B7B1
		private void SwitchToWarningState()
		{
			this.warningConfirmationInputString = string.Empty;
			this.SwitchState(GorillaComputer.ComputerState.NameWarning, false);
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x0005D5C7 File Offset: 0x0005B7C7
		private void SwitchToLoadingState()
		{
			this.SwitchState(GorillaComputer.ComputerState.Loading, false);
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x0005D5D2 File Offset: 0x0005B7D2
		private void ProcessStartupState(GorillaKeyboardBindings buttonPressed)
		{
			this.SwitchState(this.GetState(this.currentStateIndex), true);
		}

		// Token: 0x06004590 RID: 17808 RVA: 0x00181C10 File Offset: 0x0017FE10
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

		// Token: 0x06004591 RID: 17809 RVA: 0x00181D70 File Offset: 0x0017FF70
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

		// Token: 0x06004592 RID: 17810 RVA: 0x00181ED8 File Offset: 0x001800D8
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

		// Token: 0x06004593 RID: 17811 RVA: 0x00182064 File Offset: 0x00180264
		private void DisconnectAfterDelay(float seconds)
		{
			GorillaComputer.<DisconnectAfterDelay>d__173 <DisconnectAfterDelay>d__;
			<DisconnectAfterDelay>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<DisconnectAfterDelay>d__.seconds = seconds;
			<DisconnectAfterDelay>d__.<>1__state = -1;
			<DisconnectAfterDelay>d__.<>t__builder.Start<GorillaComputer.<DisconnectAfterDelay>d__173>(ref <DisconnectAfterDelay>d__);
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x0018209C File Offset: 0x0018029C
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

		// Token: 0x06004595 RID: 17813 RVA: 0x001821A4 File Offset: 0x001803A4
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

		// Token: 0x06004596 RID: 17814 RVA: 0x0018222C File Offset: 0x0018042C
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

		// Token: 0x06004597 RID: 17815 RVA: 0x00182280 File Offset: 0x00180480
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

		// Token: 0x06004598 RID: 17816 RVA: 0x0005D5E7 File Offset: 0x0005B7E7
		public void JoinTroopQueue()
		{
			if (this.IsValidTroopName(this.troopName))
			{
				this.currentTroopPopulation = -1;
				this.JoinQueue(this.GetQueueNameForTroop(this.troopName), true);
				this.RequestTroopPopulation(true);
			}
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x001822EC File Offset: 0x001804EC
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

		// Token: 0x0600459A RID: 17818 RVA: 0x0005D618 File Offset: 0x0005B818
		public void JoinDefaultQueue()
		{
			this.JoinQueue("DEFAULT", false);
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x0018236C File Offset: 0x0018056C
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

		// Token: 0x0600459C RID: 17820 RVA: 0x0005D626 File Offset: 0x0005B826
		public string GetCurrentTroop()
		{
			if (this.troopQueueActive)
			{
				return this.troopName;
			}
			return this.currentQueue;
		}

		// Token: 0x0600459D RID: 17821 RVA: 0x0005D63D File Offset: 0x0005B83D
		public int GetCurrentTroopPopulation()
		{
			if (this.troopQueueActive)
			{
				return this.currentTroopPopulation;
			}
			return -1;
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x001823C8 File Offset: 0x001805C8
		private void JoinQueue(string queueName, bool isTroopQueue = false)
		{
			this.currentQueue = queueName;
			this.troopQueueActive = isTroopQueue;
			this.currentTroopPopulation = -1;
			PlayerPrefs.SetString("currentQueue", this.currentQueue);
			PlayerPrefs.SetInt("troopQueueActive", this.troopQueueActive ? 1 : 0);
			PlayerPrefs.Save();
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x00182418 File Offset: 0x00180618
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

		// Token: 0x060045A0 RID: 17824 RVA: 0x0018259C File Offset: 0x0018079C
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

		// Token: 0x060045A1 RID: 17825 RVA: 0x0005D64F File Offset: 0x0005B84F
		private bool IsValidTroopName(string troop)
		{
			return !string.IsNullOrEmpty(troop) && troop.Length <= 12 && (this.allowedInCompetitive || troop != "COMPETITIVE");
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x0003F031 File Offset: 0x0003D231
		private string GetQueueNameForTroop(string troop)
		{
			return troop;
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x001826E4 File Offset: 0x001808E4
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

		// Token: 0x060045A4 RID: 17828 RVA: 0x00182794 File Offset: 0x00180994
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

		// Token: 0x060045A5 RID: 17829 RVA: 0x00182824 File Offset: 0x00180A24
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

		// Token: 0x060045A6 RID: 17830 RVA: 0x0005D67A File Offset: 0x0005B87A
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

		// Token: 0x060045A7 RID: 17831 RVA: 0x0005D6A4 File Offset: 0x0005B8A4
		private void ProcessSupportState(GorillaKeyboardBindings buttonPressed)
		{
			if (buttonPressed == GorillaKeyboardBindings.enter)
			{
				this.displaySupport = true;
			}
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x001828C4 File Offset: 0x00180AC4
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

		// Token: 0x060045A9 RID: 17833 RVA: 0x001829B0 File Offset: 0x00180BB0
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

		// Token: 0x060045AA RID: 17834 RVA: 0x00182A3C File Offset: 0x00180C3C
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

		// Token: 0x060045AB RID: 17835 RVA: 0x0005D6B2 File Offset: 0x0005B8B2
		private void LoadingScreen()
		{
			this.screenText.Text = "LOADING";
			this.LoadingRoutine = base.StartCoroutine(this.<LoadingScreen>g__LoadingScreenLocal|199_0());
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x00182B70 File Offset: 0x00180D70
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

		// Token: 0x060045AD RID: 17837 RVA: 0x00182BE4 File Offset: 0x00180DE4
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

		// Token: 0x060045AE RID: 17838 RVA: 0x00182CF8 File Offset: 0x00180EF8
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

		// Token: 0x060045AF RID: 17839 RVA: 0x0005D6D6 File Offset: 0x0005B8D6
		private void CreditsScreen()
		{
			this.screenText.Text = this.creditsView.GetScreenText();
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x00182D68 File Offset: 0x00180F68
		private void VisualsScreen()
		{
			this.screenText.Text = "UPDATE ITEMS SETTINGS. PRESS OPTION 1 TO ENABLE ITEM PARTICLES. PRESS OPTION 2 TO DISABLE ITEM PARTICLES. PRESS 1-10 TO CHANGE INSTRUMENT VOLUME FOR OTHER PLAYERS.\n\nITEM PARTICLES ON: " + (this.disableParticles ? "FALSE" : "TRUE") + "\nINSTRUMENT VOLUME: " + Mathf.CeilToInt(this.instrumentVolume * 50f).ToString();
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x00182DBC File Offset: 0x00180FBC
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

		// Token: 0x060045B2 RID: 17842 RVA: 0x0005D6EE File Offset: 0x0005B8EE
		private void AutomuteScreen()
		{
			this.screenText.Text = "AUTOMOD AUTOMATICALLY MUTES PLAYERS WHEN THEY JOIN YOUR ROOM IF A LOT OF OTHER PLAYERS HAVE MUTED THEM\nPRESS OPTION 1 FOR AGGRESSIVE MUTING\nPRESS OPTION 2 FOR MODERATE MUTING\nPRESS OPTION 3 TO TURN AUTOMOD OFF\n\nCURRENT AUTOMOD LEVEL: " + this.autoMuteType;
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x00182E5C File Offset: 0x0018105C
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

		// Token: 0x060045B4 RID: 17844 RVA: 0x0005D70B File Offset: 0x0005B90B
		private void MicScreen()
		{
			if (KIDManager.Instance.GetVoiceChatPermissionStatus().Item2 == Permission.ManagedByEnum.PROHIBITED)
			{
				this.MicScreen_KIdProhibited();
				return;
			}
			this.screenText.Text = "PRESS OPTION 1 = ALL CHAT.\nPRESS OPTION 2 = PUSH TO TALK.\nPRESS OPTION 3 = PUSH TO MUTE.\n\nCURRENT MIC SETTING: " + this.pttType + "\n\nPUSH TO TALK AND PUSH TO MUTE WORK WITH ANY FACE BUTTON";
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x00182EF8 File Offset: 0x001810F8
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

		// Token: 0x060045B6 RID: 17846 RVA: 0x00182F50 File Offset: 0x00181150
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

		// Token: 0x060045B7 RID: 17847 RVA: 0x0005D746 File Offset: 0x0005B946
		private void TurnScreen()
		{
			this.screenText.Text = "PRESS OPTION 1 TO USE SNAP TURN. PRESS OPTION 2 TO USE SMOOTH TURN. PRESS OPTION 3 TO USE NO ARTIFICIAL TURNING. PRESS THE NUMBER KEYS TO CHOOSE A TURNING SPEED.\n CURRENT TURN TYPE: " + this.turnType + "\nCURRENT TURN SPEED: " + this.turnValue.ToString();
		}

		// Token: 0x060045B8 RID: 17848 RVA: 0x00183188 File Offset: 0x00181388
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

		// Token: 0x060045B9 RID: 17849 RVA: 0x0018324C File Offset: 0x0018144C
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

		// Token: 0x060045BA RID: 17850 RVA: 0x001832DC File Offset: 0x001814DC
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

		// Token: 0x060045BB RID: 17851 RVA: 0x001833D4 File Offset: 0x001815D4
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

		// Token: 0x060045BC RID: 17852 RVA: 0x00183644 File Offset: 0x00181844
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

		// Token: 0x060045BD RID: 17853 RVA: 0x00183714 File Offset: 0x00181914
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

		// Token: 0x060045BE RID: 17854 RVA: 0x0005D773 File Offset: 0x0005B973
		private void UpdateFunctionScreen()
		{
			this.functionSelectText.Text = this.GetOrderListForScreen(this.currentState);
		}

		// Token: 0x060045BF RID: 17855 RVA: 0x0005D78C File Offset: 0x0005B98C
		private void CheckAutoBanListForRoomName(string nameToCheck)
		{
			this.SwitchToLoadingState();
			this.AutoBanPlayfabFunction(nameToCheck, true, new Action<ExecuteFunctionResult>(this.OnRoomNameChecked));
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x0005D7A8 File Offset: 0x0005B9A8
		private void CheckAutoBanListForPlayerName(string nameToCheck)
		{
			this.SwitchToLoadingState();
			this.AutoBanPlayfabFunction(nameToCheck, false, new Action<ExecuteFunctionResult>(this.OnPlayerNameChecked));
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x0005D7C4 File Offset: 0x0005B9C4
		private void CheckAutoBanListForTroopName(string nameToCheck)
		{
			if (this.IsValidTroopName(this.troopToJoin))
			{
				this.SwitchToLoadingState();
				this.AutoBanPlayfabFunction(nameToCheck, false, new Action<ExecuteFunctionResult>(this.OnTroopNameChecked));
			}
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x0005D7EE File Offset: 0x0005B9EE
		private void AutoBanPlayfabFunction(string nameToCheck, bool forRoom, Action<ExecuteFunctionResult> resultCallback)
		{
			GorillaServer.Instance.CheckForBadName(new CheckForBadNameRequest
			{
				name = nameToCheck,
				forRoom = forRoom
			}, resultCallback, new Action<PlayFabError>(this.OnErrorNameCheck));
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x0018376C File Offset: 0x0018196C
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

		// Token: 0x060045C4 RID: 17860 RVA: 0x00183894 File Offset: 0x00181A94
		private void OnPlayerNameChecked(ExecuteFunctionResult result)
		{
			object obj;
			if (((JsonObject)result.FunctionResult).TryGetValue("result", out obj))
			{
				switch (int.Parse(obj.ToString()))
				{
				case 0:
					NetworkSystem.Instance.SetMyNickName(this.currentName);
					CustomMapsTerminal.RequestDriverNickNameRefresh();
					break;
				case 1:
					NetworkSystem.Instance.SetMyNickName("gorilla");
					CustomMapsTerminal.RequestDriverNickNameRefresh();
					this.currentName = "gorilla";
					this.SwitchToWarningState();
					break;
				case 2:
					NetworkSystem.Instance.SetMyNickName("gorilla");
					CustomMapsTerminal.RequestDriverNickNameRefresh();
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

		// Token: 0x060045C5 RID: 17861 RVA: 0x001839D0 File Offset: 0x00181BD0
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

		// Token: 0x060045C6 RID: 17862 RVA: 0x0005D81C File Offset: 0x0005BA1C
		private void OnErrorNameCheck(PlayFabError error)
		{
			if (this.currentState == GorillaComputer.ComputerState.Loading)
			{
				this.PopState();
			}
			GorillaComputer.OnErrorShared(error);
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x00183A58 File Offset: 0x00181C58
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

		// Token: 0x060045C8 RID: 17864 RVA: 0x00183B18 File Offset: 0x00181D18
		public void UpdateColor(float red, float green, float blue)
		{
			this.redValue = Mathf.Clamp(red, 0f, 1f);
			this.greenValue = Mathf.Clamp(green, 0f, 1f);
			this.blueValue = Mathf.Clamp(blue, 0f, 1f);
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x0005D834 File Offset: 0x0005BA34
		public void UpdateFailureText(string failMessage)
		{
			GorillaScoreboardTotalUpdater.instance.SetOfflineFailureText(failMessage);
			PhotonNetworkController.Instance.UpdateTriggerScreens();
			this.screenText.EnableFailedState(failMessage);
			this.functionSelectText.EnableFailedState(failMessage);
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x0005D865 File Offset: 0x0005BA65
		private void RestoreFromFailureState()
		{
			GorillaScoreboardTotalUpdater.instance.ClearOfflineFailureText();
			PhotonNetworkController.Instance.UpdateTriggerScreens();
			this.screenText.DisableFailedState();
			this.functionSelectText.DisableFailedState();
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x0005D893 File Offset: 0x0005BA93
		public void GeneralFailureMessage(string failMessage)
		{
			this.isConnectedToMaster = false;
			NetworkSystem.Instance.SetWrongVersion();
			this.UpdateFailureText(failMessage);
			this.UpdateScreen();
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x00183B68 File Offset: 0x00181D68
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

		// Token: 0x060045CD RID: 17869 RVA: 0x00183DC0 File Offset: 0x00181FC0
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

		// Token: 0x060045CE RID: 17870 RVA: 0x00183E24 File Offset: 0x00182024
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

		// Token: 0x060045CF RID: 17871 RVA: 0x00183E88 File Offset: 0x00182088
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

		// Token: 0x060045D0 RID: 17872 RVA: 0x00183ED0 File Offset: 0x001820D0
		public int GetStateIndex(GorillaComputer.ComputerState state)
		{
			return this._activeOrderList.FindIndex((GorillaComputer.StateOrderItem s) => s.State == state);
		}

		// Token: 0x060045D1 RID: 17873 RVA: 0x00183F04 File Offset: 0x00182104
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

		// Token: 0x060045D2 RID: 17874 RVA: 0x0005D8B3 File Offset: 0x0005BAB3
		private void GetCurrentTime()
		{
			this.tryGetTimeAgain = true;
			PlayFabClientAPI.GetTime(new GetTimeRequest(), new Action<GetTimeResult>(this.OnGetTimeSuccess), new Action<PlayFabError>(this.OnGetTimeFailure), null, null);
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x00183F74 File Offset: 0x00182174
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

		// Token: 0x060045D4 RID: 17876 RVA: 0x00183FDC File Offset: 0x001821DC
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

		// Token: 0x060045D5 RID: 17877 RVA: 0x0005D8E0 File Offset: 0x0005BAE0
		private void PlayerCountChangedCallback(NetPlayer player)
		{
			this.UpdateScreen();
		}

		// Token: 0x060045D6 RID: 17878 RVA: 0x00184070 File Offset: 0x00182270
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

		// Token: 0x060045D7 RID: 17879 RVA: 0x0018414C File Offset: 0x0018234C
		private void SetNameTagText(string newName)
		{
			for (int i = 0; i < this._offlineVRRigNametagText.Length; i++)
			{
				this._offlineVRRigNametagText[i].text = newName;
			}
		}

		// Token: 0x060045D8 RID: 17880 RVA: 0x0018417C File Offset: 0x0018237C
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

		// Token: 0x060045D9 RID: 17881 RVA: 0x00184310 File Offset: 0x00182510
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

		// Token: 0x060045DA RID: 17882 RVA: 0x0005D8E8 File Offset: 0x0005BAE8
		public static void RegisterOnNametagSettingChanged(Action<bool> callback)
		{
			GorillaComputer.onNametagSettingChangedAction = (Action<bool>)Delegate.Combine(GorillaComputer.onNametagSettingChangedAction, callback);
		}

		// Token: 0x060045DB RID: 17883 RVA: 0x0005D8FF File Offset: 0x0005BAFF
		public static void UnregisterOnNametagSettingChanged(Action<bool> callback)
		{
			GorillaComputer.onNametagSettingChangedAction = (Action<bool>)Delegate.Remove(GorillaComputer.onNametagSettingChangedAction, callback);
		}

		// Token: 0x060045DC RID: 17884 RVA: 0x001843DC File Offset: 0x001825DC
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

		// Token: 0x060045DD RID: 17885 RVA: 0x00030607 File Offset: 0x0002E807
		void IMatchmakingCallbacks.OnFriendListUpdate(List<Photon.Realtime.FriendInfo> friendList)
		{
		}

		// Token: 0x060045DE RID: 17886 RVA: 0x00030607 File Offset: 0x0002E807
		void IMatchmakingCallbacks.OnCreatedRoom()
		{
		}

		// Token: 0x060045DF RID: 17887 RVA: 0x00030607 File Offset: 0x0002E807
		void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)
		{
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x00030607 File Offset: 0x0002E807
		void IMatchmakingCallbacks.OnJoinedRoom()
		{
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x00030607 File Offset: 0x0002E807
		void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message)
		{
		}

		// Token: 0x060045E2 RID: 17890 RVA: 0x00030607 File Offset: 0x0002E807
		void IMatchmakingCallbacks.OnLeftRoom()
		{
		}

		// Token: 0x060045E3 RID: 17891 RVA: 0x00030607 File Offset: 0x0002E807
		void IMatchmakingCallbacks.OnPreLeavingRoom()
		{
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x0005D916 File Offset: 0x0005BB16
		void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
		{
			if (returnCode == 32765)
			{
				this.roomFull = true;
			}
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x0005D927 File Offset: 0x0005BB27
		public void SetInVirtualStump(bool inVirtualStump)
		{
			this.playerInVirtualStump = inVirtualStump;
			this.roomToJoin = (this.playerInVirtualStump ? (this.virtualStumpRoomPrepend + this.roomToJoin) : this.roomToJoin.RemoveAll(this.virtualStumpRoomPrepend, StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x060045E6 RID: 17894 RVA: 0x0005D963 File Offset: 0x0005BB63
		public bool IsPlayerInVirtualStump()
		{
			return this.playerInVirtualStump;
		}

		// Token: 0x060045E7 RID: 17895 RVA: 0x0005D96B File Offset: 0x0005BB6B
		private void InitializeKIdState()
		{
			KIDManager.RegisterSessionUpdateCallback_AnyPermission(new Action(this.OnSessionUpdate_GorillaComputer));
			KIDIntegration.RegisterOnInitialisationComplete(new Action(this.UpdateKidState));
		}

		// Token: 0x060045E8 RID: 17896 RVA: 0x0005D98F File Offset: 0x0005BB8F
		private void UpdateKidState()
		{
			this._currentScreentState = GorillaComputer.EKidScreenState.Ready;
			if (!KIDIntegration.HasFoundSession)
			{
				this._currentScreentState = GorillaComputer.EKidScreenState.Show_OTP;
			}
		}

		// Token: 0x060045E9 RID: 17897 RVA: 0x0005D9A6 File Offset: 0x0005BBA6
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

		// Token: 0x060045EA RID: 17898 RVA: 0x0018443C File Offset: 0x0018263C
		private void UpdateSession()
		{
			GorillaComputer.<UpdateSession>d__273 <UpdateSession>d__;
			<UpdateSession>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<UpdateSession>d__.<>4__this = this;
			<UpdateSession>d__.<>1__state = -1;
			<UpdateSession>d__.<>t__builder.Start<GorillaComputer.<UpdateSession>d__273>(ref <UpdateSession>d__);
		}

		// Token: 0x060045EB RID: 17899 RVA: 0x0005D9D4 File Offset: 0x0005BBD4
		private void OnSessionUpdate_GorillaComputer()
		{
			this.UpdateKidState();
			this.UpdateScreen();
		}

		// Token: 0x060045EC RID: 17900 RVA: 0x0005D9E2 File Offset: 0x0005BBE2
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

		// Token: 0x060045ED RID: 17901 RVA: 0x00184474 File Offset: 0x00182674
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

		// Token: 0x060045EE RID: 17902 RVA: 0x0005DA18 File Offset: 0x0005BC18
		private void ProhibitedMessage(string verb)
		{
			this.screenText.Text = "";
			GorillaText gorillaText = this.screenText;
			gorillaText.Text = gorillaText.Text + "\n\nYOU ARE NOT ALLOWED TO " + verb + " IN YOUR JURISDICTION.";
		}

		// Token: 0x060045EF RID: 17903 RVA: 0x0018462C File Offset: 0x0018282C
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

		// Token: 0x060045F0 RID: 17904 RVA: 0x0005DA4B File Offset: 0x0005BC4B
		private void RoomScreen_KIdProhibited()
		{
			this.ProhibitedMessage("CREATE OR JOIN PRIVATE ROOMS");
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x0018467C File Offset: 0x0018287C
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

		// Token: 0x060045F2 RID: 17906 RVA: 0x0005DA58 File Offset: 0x0005BC58
		private void VoiceScreen_KIdProhibited()
		{
			this.ProhibitedMessage("USE THE VOICE CHAT");
		}

		// Token: 0x060045F3 RID: 17907 RVA: 0x0005DA65 File Offset: 0x0005BC65
		private void MicScreen_KIdPermission()
		{
			this.screenText.Text = "";
			this.GuardianConsentMessage("OPTION 3", "ENABLE HUMAN VOICE CHAT");
		}

		// Token: 0x060045F4 RID: 17908 RVA: 0x0005DA88 File Offset: 0x0005BC88
		private void MicScreen_KIdProhibited()
		{
			this.VoiceScreen_KIdProhibited();
		}

		// Token: 0x060045F5 RID: 17909 RVA: 0x001846D4 File Offset: 0x001828D4
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

		// Token: 0x060045F6 RID: 17910 RVA: 0x0005DA90 File Offset: 0x0005BC90
		private void NameScreen_KIdProhibited()
		{
			this.ProhibitedMessage("SET CUSTOM NICKNAMES");
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x00184724 File Offset: 0x00182924
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

		// Token: 0x060045F8 RID: 17912 RVA: 0x0005DA9D File Offset: 0x0005BC9D
		private void TroopScreen_KIdPermission()
		{
			this.screenText.Text = "";
			this.GuardianConsentMessage("OPTION 3", "JOIN TROOPS");
		}

		// Token: 0x060045F9 RID: 17913 RVA: 0x0005DAC0 File Offset: 0x0005BCC0
		private void TroopScreen_KIdProhibited()
		{
			this.ProhibitedMessage("CREATE OR JOIN TROOPS");
		}

		// Token: 0x060045FA RID: 17914 RVA: 0x0005DACD File Offset: 0x0005BCCD
		private void ProcessKIdState(GorillaKeyboardBindings buttonPressed)
		{
			if (buttonPressed == GorillaKeyboardBindings.option1 && this._currentScreentState == GorillaComputer.EKidScreenState.Ready)
			{
				this.RequestUpdatedPermissions();
			}
		}

		// Token: 0x060045FB RID: 17915 RVA: 0x0005DAE2 File Offset: 0x0005BCE2
		private void KIdScreen()
		{
			if (KIDManager.KIDIntegration == null || this._currentScreentState == GorillaComputer.EKidScreenState.Show_OTP || !KIDIntegration.HasFoundSession)
			{
				this.GuardianConsentMessage("OPTION 3", "");
				return;
			}
			this.KIdScreen_DisplayPermissions();
		}

		// Token: 0x060045FC RID: 17916 RVA: 0x001847E8 File Offset: 0x001829E8
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

		// Token: 0x060045FD RID: 17917 RVA: 0x00184960 File Offset: 0x00182B60
		private void KIdScreen_ShowOTP()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("k-ID Permission Setup");
			stringBuilder.AppendLine("\nEnter this code on the Family Portal:\n\t\t" + KIDManager.KIDIntegration.GetOneTimePasscode());
			stringBuilder.AppendLine("\nFamily Portal: k-id.com/code");
			this.screenText.Text = stringBuilder.ToString();
		}

		// Token: 0x060045FF RID: 17919 RVA: 0x00032105 File Offset: 0x00030305
		bool IGorillaSliceableSimple.get_isActiveAndEnabled()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x06004602 RID: 17922 RVA: 0x0005DB2C File Offset: 0x0005BD2C
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

		// Token: 0x04004675 RID: 18037
		private const bool HIDE_SCREENS = false;

		// Token: 0x04004676 RID: 18038
		public const string NAMETAG_PLAYER_PREF_KEY = "nameTagsOn";

		// Token: 0x04004677 RID: 18039
		[OnEnterPlay_SetNull]
		public static volatile GorillaComputer instance;

		// Token: 0x04004678 RID: 18040
		[OnEnterPlay_Set(false)]
		public static bool hasInstance;

		// Token: 0x04004679 RID: 18041
		[OnEnterPlay_SetNull]
		private static Action<bool> onNametagSettingChangedAction;

		// Token: 0x0400467A RID: 18042
		public bool tryGetTimeAgain;

		// Token: 0x0400467B RID: 18043
		public Material unpressedMaterial;

		// Token: 0x0400467C RID: 18044
		public Material pressedMaterial;

		// Token: 0x0400467D RID: 18045
		public string currentTextField;

		// Token: 0x0400467E RID: 18046
		public float buttonFadeTime;

		// Token: 0x0400467F RID: 18047
		public string offlineTextInitialString;

		// Token: 0x04004680 RID: 18048
		public GorillaText screenText;

		// Token: 0x04004681 RID: 18049
		public GorillaText functionSelectText;

		// Token: 0x04004682 RID: 18050
		public GorillaText wallScreenText;

		// Token: 0x04004683 RID: 18051
		[SerializeField]
		[FormerlySerializedAs("offlineVRRigNametagText")]
		private TMP_Text[] _offlineVRRigNametagText;

		// Token: 0x04004684 RID: 18052
		public string versionMismatch = "PLEASE UPDATE TO THE LATEST VERSION OF GORILLA TAG. YOU'RE ON AN OLD VERSION. FEEL FREE TO RUN AROUND, BUT YOU WON'T BE ABLE TO PLAY WITH ANYONE ELSE.";

		// Token: 0x04004685 RID: 18053
		public string unableToConnect = "UNABLE TO CONNECT TO THE INTERNET. PLEASE CHECK YOUR CONNECTION AND RESTART THE GAME.";

		// Token: 0x04004686 RID: 18054
		public Material wrongVersionMaterial;

		// Token: 0x04004687 RID: 18055
		public MeshRenderer wallScreenRenderer;

		// Token: 0x04004688 RID: 18056
		public MeshRenderer computerScreenRenderer;

		// Token: 0x04004689 RID: 18057
		public long startupMillis;

		// Token: 0x0400468A RID: 18058
		public DateTime startupTime;

		// Token: 0x0400468B RID: 18059
		public string lastPressedGameMode;

		// Token: 0x0400468C RID: 18060
		public WatchableStringSO currentGameMode;

		// Token: 0x0400468D RID: 18061
		public WatchableStringSO currentGameModeText;

		// Token: 0x0400468E RID: 18062
		public int includeUpdatedServerSynchTest;

		// Token: 0x0400468F RID: 18063
		public PhotonNetworkController networkController;

		// Token: 0x04004690 RID: 18064
		public float updateCooldown = 1f;

		// Token: 0x04004691 RID: 18065
		public float lastUpdateTime;

		// Token: 0x04004692 RID: 18066
		public bool isConnectedToMaster;

		// Token: 0x04004693 RID: 18067
		public bool internetFailure;

		// Token: 0x04004694 RID: 18068
		public string[] allowedMapsToJoin;

		// Token: 0x04004695 RID: 18069
		[Header("State vars")]
		public bool stateUpdated;

		// Token: 0x04004696 RID: 18070
		public bool screenChanged;

		// Token: 0x04004697 RID: 18071
		public bool initialized;

		// Token: 0x04004698 RID: 18072
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

		// Token: 0x04004699 RID: 18073
		public string Pointer = "<-";

		// Token: 0x0400469A RID: 18074
		public int highestCharacterCount;

		// Token: 0x0400469B RID: 18075
		public List<string> FunctionNames = new List<string>();

		// Token: 0x0400469C RID: 18076
		public int FunctionsCount;

		// Token: 0x0400469D RID: 18077
		[Header("Room vars")]
		public string roomToJoin;

		// Token: 0x0400469E RID: 18078
		public bool roomFull;

		// Token: 0x0400469F RID: 18079
		public bool roomNotAllowed;

		// Token: 0x040046A0 RID: 18080
		[Header("Mic vars")]
		public string pttType;

		// Token: 0x040046A1 RID: 18081
		[Header("Automute vars")]
		public string autoMuteType;

		// Token: 0x040046A2 RID: 18082
		[Header("Queue vars")]
		public string currentQueue;

		// Token: 0x040046A3 RID: 18083
		public bool allowedInCompetitive;

		// Token: 0x040046A4 RID: 18084
		[Header("Group Vars")]
		public string groupMapJoin;

		// Token: 0x040046A5 RID: 18085
		public int groupMapJoinIndex;

		// Token: 0x040046A6 RID: 18086
		public GorillaFriendCollider friendJoinCollider;

		// Token: 0x040046A7 RID: 18087
		[Header("Troop vars")]
		public string troopName;

		// Token: 0x040046A8 RID: 18088
		public bool troopQueueActive;

		// Token: 0x040046A9 RID: 18089
		public string troopToJoin;

		// Token: 0x040046AA RID: 18090
		[Header("Join Triggers")]
		public Dictionary<string, GorillaNetworkJoinTrigger> primaryTriggersByZone = new Dictionary<string, GorillaNetworkJoinTrigger>();

		// Token: 0x040046AB RID: 18091
		public string voiceChatOn;

		// Token: 0x040046AC RID: 18092
		[Header("Mode select vars")]
		public ModeSelectButton[] modeSelectButtons;

		// Token: 0x040046AD RID: 18093
		public string version;

		// Token: 0x040046AE RID: 18094
		public string buildDate;

		// Token: 0x040046AF RID: 18095
		public string buildCode;

		// Token: 0x040046B0 RID: 18096
		[Header("Cosmetics")]
		public bool disableParticles;

		// Token: 0x040046B1 RID: 18097
		public float instrumentVolume;

		// Token: 0x040046B2 RID: 18098
		[Header("Credits")]
		public CreditsView creditsView;

		// Token: 0x040046B3 RID: 18099
		[Header("Handedness")]
		public bool leftHanded;

		// Token: 0x040046B4 RID: 18100
		[Header("Name state vars")]
		public string savedName;

		// Token: 0x040046B5 RID: 18101
		public string currentName;

		// Token: 0x040046B6 RID: 18102
		public TextAsset exactOneWeekFile;

		// Token: 0x040046B7 RID: 18103
		public TextAsset anywhereOneWeekFile;

		// Token: 0x040046B8 RID: 18104
		public TextAsset anywhereTwoWeekFile;

		// Token: 0x040046B9 RID: 18105
		private List<GorillaComputer.ComputerState> _filteredStates = new List<GorillaComputer.ComputerState>();

		// Token: 0x040046BA RID: 18106
		private List<GorillaComputer.StateOrderItem> _activeOrderList = new List<GorillaComputer.StateOrderItem>();

		// Token: 0x040046BB RID: 18107
		private Stack<GorillaComputer.ComputerState> stateStack = new Stack<GorillaComputer.ComputerState>();

		// Token: 0x040046BC RID: 18108
		private GorillaComputer.ComputerState currentComputerState;

		// Token: 0x040046BD RID: 18109
		private GorillaComputer.ComputerState previousComputerState;

		// Token: 0x040046BE RID: 18110
		private int currentStateIndex;

		// Token: 0x040046BF RID: 18111
		private int usersBanned;

		// Token: 0x040046C0 RID: 18112
		private float redValue;

		// Token: 0x040046C1 RID: 18113
		private string redText;

		// Token: 0x040046C2 RID: 18114
		private float blueValue;

		// Token: 0x040046C3 RID: 18115
		private string blueText;

		// Token: 0x040046C4 RID: 18116
		private float greenValue;

		// Token: 0x040046C5 RID: 18117
		private string greenText;

		// Token: 0x040046C6 RID: 18118
		private int colorCursorLine;

		// Token: 0x040046C7 RID: 18119
		private int turnValue;

		// Token: 0x040046C8 RID: 18120
		private string turnType;

		// Token: 0x040046C9 RID: 18121
		private GorillaSnapTurn gorillaTurn;

		// Token: 0x040046CA RID: 18122
		private string warningConfirmationInputString = string.Empty;

		// Token: 0x040046CB RID: 18123
		private bool displaySupport;

		// Token: 0x040046CC RID: 18124
		private string[] exactOneWeek;

		// Token: 0x040046CD RID: 18125
		private string[] anywhereOneWeek;

		// Token: 0x040046CE RID: 18126
		private string[] anywhereTwoWeek;

		// Token: 0x040046CF RID: 18127
		private GorillaComputer.RedemptionResult redemptionResult;

		// Token: 0x040046D0 RID: 18128
		private string redemptionCode = "";

		// Token: 0x040046D1 RID: 18129
		private bool playerInVirtualStump;

		// Token: 0x040046D2 RID: 18130
		private string virtualStumpRoomPrepend = "";

		// Token: 0x040046D3 RID: 18131
		private WaitForSeconds waitOneSecond = new WaitForSeconds(1f);

		// Token: 0x040046D4 RID: 18132
		private Coroutine LoadingRoutine;

		// Token: 0x040046D5 RID: 18133
		private List<string> topTroops = new List<string>();

		// Token: 0x040046D6 RID: 18134
		private bool hasRequestedInitialTroopPopulation;

		// Token: 0x040046D7 RID: 18135
		private int currentTroopPopulation = -1;

		// Token: 0x040046D9 RID: 18137
		private float lastCheckedWifi;

		// Token: 0x040046DA RID: 18138
		private float checkIfDisconnectedSeconds = 10f;

		// Token: 0x040046DB RID: 18139
		private float checkIfConnectedSeconds = 1f;

		// Token: 0x040046DC RID: 18140
		private float troopPopulationCheckCooldown = 3f;

		// Token: 0x040046DD RID: 18141
		private float nextPopulationCheckTime;

		// Token: 0x040046DE RID: 18142
		public Action OnServerTimeUpdated;

		// Token: 0x040046DF RID: 18143
		private const string ENABLED_COLOUR = "#85ffa5";

		// Token: 0x040046E0 RID: 18144
		private const string DISABLED_COLOUR = "\"RED\"";

		// Token: 0x040046E1 RID: 18145
		private const string FAMILY_PORTAL_URL = "k-id.com/code";

		// Token: 0x040046E2 RID: 18146
		private float _updateAttemptCooldown = 60f;

		// Token: 0x040046E3 RID: 18147
		private float _nextUpdateAttemptTime;

		// Token: 0x040046E4 RID: 18148
		private bool _waitingForUpdatedSession;

		// Token: 0x040046E5 RID: 18149
		private GorillaComputer.EKidScreenState _currentScreentState = GorillaComputer.EKidScreenState.Show_OTP;

		// Token: 0x040046E6 RID: 18150
		private string[] _interestedKIDPermissionNames = new string[]
		{
			"custom-username",
			"voice-chat",
			"join-groups"
		};

		// Token: 0x02000AD4 RID: 2772
		public enum ComputerState
		{
			// Token: 0x040046E8 RID: 18152
			Startup,
			// Token: 0x040046E9 RID: 18153
			Color,
			// Token: 0x040046EA RID: 18154
			Name,
			// Token: 0x040046EB RID: 18155
			Turn,
			// Token: 0x040046EC RID: 18156
			Mic,
			// Token: 0x040046ED RID: 18157
			Room,
			// Token: 0x040046EE RID: 18158
			Queue,
			// Token: 0x040046EF RID: 18159
			Group,
			// Token: 0x040046F0 RID: 18160
			Voice,
			// Token: 0x040046F1 RID: 18161
			AutoMute,
			// Token: 0x040046F2 RID: 18162
			Credits,
			// Token: 0x040046F3 RID: 18163
			Visuals,
			// Token: 0x040046F4 RID: 18164
			Time,
			// Token: 0x040046F5 RID: 18165
			NameWarning,
			// Token: 0x040046F6 RID: 18166
			Loading,
			// Token: 0x040046F7 RID: 18167
			Support,
			// Token: 0x040046F8 RID: 18168
			Troop,
			// Token: 0x040046F9 RID: 18169
			KID,
			// Token: 0x040046FA RID: 18170
			Redemption
		}

		// Token: 0x02000AD5 RID: 2773
		private enum NameCheckResult
		{
			// Token: 0x040046FC RID: 18172
			Success,
			// Token: 0x040046FD RID: 18173
			Warning,
			// Token: 0x040046FE RID: 18174
			Ban
		}

		// Token: 0x02000AD6 RID: 2774
		public enum RedemptionResult
		{
			// Token: 0x04004700 RID: 18176
			Empty,
			// Token: 0x04004701 RID: 18177
			Invalid,
			// Token: 0x04004702 RID: 18178
			Checking,
			// Token: 0x04004703 RID: 18179
			AlreadyUsed,
			// Token: 0x04004704 RID: 18180
			Success
		}

		// Token: 0x02000AD7 RID: 2775
		[Serializable]
		public class StateOrderItem
		{
			// Token: 0x06004604 RID: 17924 RVA: 0x0005DB3B File Offset: 0x0005BD3B
			public StateOrderItem()
			{
			}

			// Token: 0x06004605 RID: 17925 RVA: 0x0005DB4E File Offset: 0x0005BD4E
			public StateOrderItem(GorillaComputer.ComputerState state)
			{
				this.State = state;
			}

			// Token: 0x06004606 RID: 17926 RVA: 0x0005DB68 File Offset: 0x0005BD68
			public StateOrderItem(GorillaComputer.ComputerState state, string overrideName)
			{
				this.State = state;
				this.OverrideName = overrideName;
			}

			// Token: 0x06004607 RID: 17927 RVA: 0x0005DB89 File Offset: 0x0005BD89
			public string GetName()
			{
				if (!string.IsNullOrEmpty(this.OverrideName))
				{
					return this.OverrideName.ToUpper();
				}
				return this.State.ToString().ToUpper();
			}

			// Token: 0x04004705 RID: 18181
			public GorillaComputer.ComputerState State;

			// Token: 0x04004706 RID: 18182
			[Tooltip("Case not important - ToUpper applied at runtime")]
			public string OverrideName = "";
		}

		// Token: 0x02000AD8 RID: 2776
		private enum EKidScreenState
		{
			// Token: 0x04004708 RID: 18184
			Ready,
			// Token: 0x04004709 RID: 18185
			Show_OTP,
			// Token: 0x0400470A RID: 18186
			Show_Setup_Screen
		}
	}
}
