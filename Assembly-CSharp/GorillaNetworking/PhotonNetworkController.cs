using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Fusion;
using GorillaGameModes;
using GorillaLocomotion;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AFD RID: 2813
	public class PhotonNetworkController : MonoBehaviour
	{
		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06004682 RID: 18050 RVA: 0x0005DFBE File Offset: 0x0005C1BE
		// (set) Token: 0x06004683 RID: 18051 RVA: 0x0005DFC6 File Offset: 0x0005C1C6
		public string StartLevel
		{
			get
			{
				return this.startLevel;
			}
			set
			{
				this.startLevel = value;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06004684 RID: 18052 RVA: 0x0005DFCF File Offset: 0x0005C1CF
		// (set) Token: 0x06004685 RID: 18053 RVA: 0x0005DFD7 File Offset: 0x0005C1D7
		public GTZone StartZone
		{
			get
			{
				return this.startZone;
			}
			set
			{
				this.startZone = value;
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06004686 RID: 18054 RVA: 0x0005DFE0 File Offset: 0x0005C1E0
		public GTZone CurrentRoomZone
		{
			get
			{
				if (!(this.currentJoinTrigger != null))
				{
					return GTZone.none;
				}
				return this.currentJoinTrigger.zone;
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06004687 RID: 18055 RVA: 0x0005DFFE File Offset: 0x0005C1FE
		// (set) Token: 0x06004688 RID: 18056 RVA: 0x0005E006 File Offset: 0x0005C206
		public GorillaGeoHideShowTrigger StartGeoTrigger
		{
			get
			{
				return this.startGeoTrigger;
			}
			set
			{
				this.startGeoTrigger = value;
			}
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x001865E4 File Offset: 0x001847E4
		public void Awake()
		{
			if (PhotonNetworkController.Instance == null)
			{
				PhotonNetworkController.Instance = this;
			}
			else if (PhotonNetworkController.Instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			this.updatedName = false;
			this.playersInRegion = new int[this.serverRegions.Length];
			this.pingInRegion = new int[this.serverRegions.Length];
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x00186654 File Offset: 0x00184854
		public void Start()
		{
			base.StartCoroutine(this.DisableOnStart());
			NetworkSystem.Instance.OnJoinedRoomEvent += this.OnJoinedRoom;
			NetworkSystem.Instance.OnReturnedToSinglePlayer += this.OnDisconnected;
			PhotonNetwork.NetworkingClient.LoadBalancingPeer.ReuseEventInstance = true;
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x0005E00F File Offset: 0x0005C20F
		private IEnumerator DisableOnStart()
		{
			ZoneManagement.SetActiveZone(this.StartZone);
			yield break;
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x001866AC File Offset: 0x001848AC
		public void FixedUpdate()
		{
			this.headRightHandDistance = (GTPlayer.Instance.headCollider.transform.position - GTPlayer.Instance.rightControllerTransform.position).magnitude;
			this.headLeftHandDistance = (GTPlayer.Instance.headCollider.transform.position - GTPlayer.Instance.leftControllerTransform.position).magnitude;
			this.headQuat = GTPlayer.Instance.headCollider.transform.rotation;
			if (!this.disableAFKKick && Quaternion.Angle(this.headQuat, this.lastHeadQuat) <= 0.01f && Mathf.Abs(this.headRightHandDistance - this.lastHeadRightHandDistance) < 0.001f && Mathf.Abs(this.headLeftHandDistance - this.lastHeadLeftHandDistance) < 0.001f && this.pauseTime + this.disconnectTime < Time.realtimeSinceStartup)
			{
				this.pauseTime = Time.realtimeSinceStartup;
				NetworkSystem.Instance.ReturnToSinglePlayer();
			}
			else if (Quaternion.Angle(this.headQuat, this.lastHeadQuat) > 0.01f || Mathf.Abs(this.headRightHandDistance - this.lastHeadRightHandDistance) >= 0.001f || Mathf.Abs(this.headLeftHandDistance - this.lastHeadLeftHandDistance) >= 0.001f)
			{
				this.pauseTime = Time.realtimeSinceStartup;
			}
			this.lastHeadRightHandDistance = this.headRightHandDistance;
			this.lastHeadLeftHandDistance = this.headLeftHandDistance;
			this.lastHeadQuat = this.headQuat;
			if (this.deferredJoin && Time.time >= this.partyJoinDeferredUntilTimestamp)
			{
				if ((this.partyJoinDeferredUntilTimestamp != 0f || NetworkSystem.Instance.netState == NetSystemState.Idle) && this.currentJoinTrigger != null)
				{
					this.deferredJoin = false;
					this.partyJoinDeferredUntilTimestamp = 0f;
					if (this.currentJoinTrigger == this.privateTrigger)
					{
						this.AttemptToJoinSpecificRoom(this.customRoomID, FriendshipGroupDetection.Instance.IsInParty ? JoinType.JoinWithParty : JoinType.Solo);
						return;
					}
					this.AttemptToJoinPublicRoom(this.currentJoinTrigger, this.currentJoinType);
					return;
				}
				else if (NetworkSystem.Instance.netState != NetSystemState.PingRecon && NetworkSystem.Instance.netState != NetSystemState.Initialization)
				{
					this.deferredJoin = false;
					this.partyJoinDeferredUntilTimestamp = 0f;
				}
			}
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x0005E01E File Offset: 0x0005C21E
		public void DeferJoining(float duration)
		{
			this.partyJoinDeferredUntilTimestamp = Mathf.Max(this.partyJoinDeferredUntilTimestamp, Time.time + duration);
		}

		// Token: 0x0600468E RID: 18062 RVA: 0x0005E038 File Offset: 0x0005C238
		public void ClearDeferredJoin()
		{
			this.partyJoinDeferredUntilTimestamp = 0f;
			this.deferredJoin = false;
		}

		// Token: 0x0600468F RID: 18063 RVA: 0x0005E04C File Offset: 0x0005C24C
		public void AttemptToJoinPublicRoom(GorillaNetworkJoinTrigger triggeredTrigger, JoinType roomJoinType = JoinType.Solo)
		{
			this.AttemptToJoinPublicRoomAsync(triggeredTrigger, roomJoinType);
		}

		// Token: 0x06004690 RID: 18064 RVA: 0x001868FC File Offset: 0x00184AFC
		private void AttemptToJoinPublicRoomAsync(GorillaNetworkJoinTrigger triggeredTrigger, JoinType roomJoinType)
		{
			PhotonNetworkController.<AttemptToJoinPublicRoomAsync>d__64 <AttemptToJoinPublicRoomAsync>d__;
			<AttemptToJoinPublicRoomAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<AttemptToJoinPublicRoomAsync>d__.<>4__this = this;
			<AttemptToJoinPublicRoomAsync>d__.triggeredTrigger = triggeredTrigger;
			<AttemptToJoinPublicRoomAsync>d__.roomJoinType = roomJoinType;
			<AttemptToJoinPublicRoomAsync>d__.<>1__state = -1;
			<AttemptToJoinPublicRoomAsync>d__.<>t__builder.Start<PhotonNetworkController.<AttemptToJoinPublicRoomAsync>d__64>(ref <AttemptToJoinPublicRoomAsync>d__);
		}

		// Token: 0x06004691 RID: 18065 RVA: 0x00186944 File Offset: 0x00184B44
		private Task SendPartyFollowCommands()
		{
			PhotonNetworkController.<SendPartyFollowCommands>d__65 <SendPartyFollowCommands>d__;
			<SendPartyFollowCommands>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SendPartyFollowCommands>d__.<>1__state = -1;
			<SendPartyFollowCommands>d__.<>t__builder.Start<PhotonNetworkController.<SendPartyFollowCommands>d__65>(ref <SendPartyFollowCommands>d__);
			return <SendPartyFollowCommands>d__.<>t__builder.Task;
		}

		// Token: 0x06004692 RID: 18066 RVA: 0x0005E056 File Offset: 0x0005C256
		public void AttemptToJoinSpecificRoom(string roomID, JoinType roomJoinType)
		{
			this.AttemptToJoinSpecificRoomAsync(roomID, roomJoinType, null);
		}

		// Token: 0x06004693 RID: 18067 RVA: 0x0005E062 File Offset: 0x0005C262
		public void AttemptToJoinSpecificRoomWithCallback(string roomID, JoinType roomJoinType, Action<NetJoinResult> callback)
		{
			this.AttemptToJoinSpecificRoomAsync(roomID, roomJoinType, callback);
		}

		// Token: 0x06004694 RID: 18068 RVA: 0x00186980 File Offset: 0x00184B80
		public Task AttemptToJoinSpecificRoomAsync(string roomID, JoinType roomJoinType, Action<NetJoinResult> callback)
		{
			PhotonNetworkController.<AttemptToJoinSpecificRoomAsync>d__68 <AttemptToJoinSpecificRoomAsync>d__;
			<AttemptToJoinSpecificRoomAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AttemptToJoinSpecificRoomAsync>d__.<>4__this = this;
			<AttemptToJoinSpecificRoomAsync>d__.roomID = roomID;
			<AttemptToJoinSpecificRoomAsync>d__.roomJoinType = roomJoinType;
			<AttemptToJoinSpecificRoomAsync>d__.callback = callback;
			<AttemptToJoinSpecificRoomAsync>d__.<>1__state = -1;
			<AttemptToJoinSpecificRoomAsync>d__.<>t__builder.Start<PhotonNetworkController.<AttemptToJoinSpecificRoomAsync>d__68>(ref <AttemptToJoinSpecificRoomAsync>d__);
			return <AttemptToJoinSpecificRoomAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x001869DC File Offset: 0x00184BDC
		private void DisconnectCleanup()
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			if (GorillaParent.instance != null)
			{
				GorillaScoreboardSpawner[] componentsInChildren = GorillaParent.instance.GetComponentsInChildren<GorillaScoreboardSpawner>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].OnLeftRoom();
				}
			}
			this.attemptingToConnect = true;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.offlineVRRig)
			{
				if (skinnedMeshRenderer != null)
				{
					skinnedMeshRenderer.enabled = true;
				}
			}
			if (GorillaComputer.instance != null && !ApplicationQuittingState.IsQuitting)
			{
				this.UpdateTriggerScreens();
			}
			GTPlayer.Instance.maxJumpSpeed = 6.5f;
			GTPlayer.Instance.jumpMultiplier = 1.1f;
			GorillaNot.instance.currentMasterClient = null;
			GorillaTagger.Instance.offlineVRRig.huntComputer.SetActive(false);
			this.initialGameMode = "";
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x00186ABC File Offset: 0x00184CBC
		public void OnJoinedRoom()
		{
			if (NetworkSystem.Instance.GameModeString.IsNullOrEmpty())
			{
				NetworkSystem.Instance.ReturnToSinglePlayer();
			}
			this.initialGameMode = NetworkSystem.Instance.GameModeString;
			if (NetworkSystem.Instance.SessionIsPrivate)
			{
				this.currentJoinTrigger = this.privateTrigger;
				PhotonNetworkController.Instance.UpdateTriggerScreens();
			}
			else if (this.currentJoinType != JoinType.FollowingParty)
			{
				bool flag = false;
				for (int i = 0; i < GorillaComputer.instance.allowedMapsToJoin.Length; i++)
				{
					if (NetworkSystem.Instance.GameModeString.StartsWith(GorillaComputer.instance.allowedMapsToJoin[i]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					GorillaComputer.instance.roomNotAllowed = true;
					NetworkSystem.Instance.ReturnToSinglePlayer();
					return;
				}
			}
			NetworkSystem.Instance.SetMyTutorialComplete();
			VRRigCache.Instance.InstantiateNetworkObject();
			if (NetworkSystem.Instance.IsMasterClient)
			{
				GorillaGameModes.GameMode.LoadGameModeFromProperty(this.initialGameMode);
			}
			GorillaComputer.instance.roomFull = false;
			GorillaComputer.instance.roomNotAllowed = false;
			if (this.currentJoinType == JoinType.JoinWithParty || this.currentJoinType == JoinType.JoinWithNearby || this.currentJoinType == JoinType.ForceJoinWithParty)
			{
				this.keyToFollow = NetworkSystem.Instance.LocalPlayer.UserId + this.keyStr;
				NetworkSystem.Instance.BroadcastMyRoom(true, this.keyToFollow, this.shuffler);
			}
			GorillaNot.instance.currentMasterClient = null;
			this.UpdateCurrentJoinTrigger();
			this.UpdateTriggerScreens();
			NetworkSystem.Instance.MultiplayerStarted();
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x0005E06E File Offset: 0x0005C26E
		public void RegisterJoinTrigger(GorillaNetworkJoinTrigger trigger)
		{
			this.allJoinTriggers.Add(trigger);
		}

		// Token: 0x06004698 RID: 18072 RVA: 0x00186C3C File Offset: 0x00184E3C
		private void UpdateCurrentJoinTrigger()
		{
			GorillaNetworkJoinTrigger joinTriggerFromFullGameModeString = GorillaComputer.instance.GetJoinTriggerFromFullGameModeString(NetworkSystem.Instance.GameModeString);
			if (joinTriggerFromFullGameModeString != null)
			{
				this.currentJoinTrigger = joinTriggerFromFullGameModeString;
				return;
			}
			if (NetworkSystem.Instance.SessionIsPrivate)
			{
				if (this.currentJoinTrigger != this.privateTrigger)
				{
					Debug.LogError("IN a private game but private trigger isnt current");
					return;
				}
			}
			else
			{
				Debug.LogError("Not in private room and unabel tp update jointrigger.");
			}
		}

		// Token: 0x06004699 RID: 18073 RVA: 0x00186CA8 File Offset: 0x00184EA8
		public void UpdateTriggerScreens()
		{
			foreach (GorillaNetworkJoinTrigger gorillaNetworkJoinTrigger in this.allJoinTriggers)
			{
				gorillaNetworkJoinTrigger.UpdateUI();
			}
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x00186CF8 File Offset: 0x00184EF8
		public void AttemptToFollowIntoPub(string userIDToFollow, int actorNumberToFollow, string newKeyStr, string shufflerStr, JoinType joinType)
		{
			this.friendToFollow = userIDToFollow;
			this.keyToFollow = userIDToFollow + newKeyStr;
			this.shuffler = shufflerStr;
			this.currentJoinType = joinType;
			this.ClearDeferredJoin();
			if (NetworkSystem.Instance.InRoom)
			{
				NetworkSystem.Instance.JoinFriendsRoom(this.friendToFollow, actorNumberToFollow, this.keyToFollow, this.shuffler);
			}
		}

		// Token: 0x0600469B RID: 18075 RVA: 0x0005E07C File Offset: 0x0005C27C
		public void OnDisconnected()
		{
			this.DisconnectCleanup();
		}

		// Token: 0x0600469C RID: 18076 RVA: 0x0005E084 File Offset: 0x0005C284
		public void OnApplicationQuit()
		{
			if (PhotonNetwork.IsConnected)
			{
				PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion != "dev";
			}
		}

		// Token: 0x0600469D RID: 18077 RVA: 0x0005E0A7 File Offset: 0x0005C2A7
		private string ReturnRoomName()
		{
			if (this.isPrivate)
			{
				return this.customRoomID;
			}
			return this.RandomRoomName();
		}

		// Token: 0x0600469E RID: 18078 RVA: 0x00186D5C File Offset: 0x00184F5C
		private string RandomRoomName()
		{
			string text = "";
			for (int i = 0; i < 4; i++)
			{
				text += "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".Substring(UnityEngine.Random.Range(0, "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".Length), 1);
			}
			if (GorillaComputer.instance.CheckAutoBanListForName(text))
			{
				return text;
			}
			return this.RandomRoomName();
		}

		// Token: 0x0600469F RID: 18079 RVA: 0x0005E0BE File Offset: 0x0005C2BE
		public byte GetRoomSize(string gameModeName)
		{
			return 10;
		}

		// Token: 0x060046A0 RID: 18080 RVA: 0x00186DB4 File Offset: 0x00184FB4
		private string GetRegionWithLowestPing()
		{
			int num = 10000;
			int num2 = 0;
			for (int i = 0; i < this.serverRegions.Length; i++)
			{
				Debug.Log("ping in region " + this.serverRegions[i] + " is " + this.pingInRegion[i].ToString());
				if (this.pingInRegion[i] < num && this.pingInRegion[i] > 0)
				{
					num = this.pingInRegion[i];
					num2 = i;
				}
			}
			return this.serverRegions[num2];
		}

		// Token: 0x060046A1 RID: 18081 RVA: 0x00186E34 File Offset: 0x00185034
		public int TotalUsers()
		{
			int num = 0;
			foreach (int num2 in this.playersInRegion)
			{
				num += num2;
			}
			return num;
		}

		// Token: 0x060046A2 RID: 18082 RVA: 0x00186E64 File Offset: 0x00185064
		public string CurrentState()
		{
			if (NetworkSystem.Instance == null)
			{
				Debug.Log("Null netsys!!!");
			}
			return NetworkSystem.Instance.netState.ToString();
		}

		// Token: 0x060046A3 RID: 18083 RVA: 0x00186EA0 File Offset: 0x001850A0
		private void OnApplicationPause(bool pause)
		{
			if (pause)
			{
				this.timeWhenApplicationPaused = new DateTime?(DateTime.Now);
				return;
			}
			if ((DateTime.Now - (this.timeWhenApplicationPaused ?? DateTime.Now)).TotalSeconds > (double)this.disconnectTime)
			{
				this.timeWhenApplicationPaused = null;
				NetworkSystem instance = NetworkSystem.Instance;
				if (instance != null)
				{
					instance.ReturnToSinglePlayer();
				}
			}
			if (NetworkSystem.Instance != null && !NetworkSystem.Instance.InRoom && NetworkSystem.Instance.netState == NetSystemState.InGame)
			{
				NetworkSystem instance2 = NetworkSystem.Instance;
				if (instance2 == null)
				{
					return;
				}
				instance2.ReturnToSinglePlayer();
			}
		}

		// Token: 0x060046A4 RID: 18084 RVA: 0x0005E0C2 File Offset: 0x0005C2C2
		private void OnApplicationFocus(bool focus)
		{
			if (!focus && NetworkSystem.Instance != null && !NetworkSystem.Instance.InRoom && NetworkSystem.Instance.netState == NetSystemState.InGame)
			{
				NetworkSystem instance = NetworkSystem.Instance;
				if (instance == null)
				{
					return;
				}
				instance.ReturnToSinglePlayer();
			}
		}

		// Token: 0x040047AC RID: 18348
		public static volatile PhotonNetworkController Instance;

		// Token: 0x040047AD RID: 18349
		public int incrementCounter;

		// Token: 0x040047AE RID: 18350
		public PlayFabAuthenticator playFabAuthenticator;

		// Token: 0x040047AF RID: 18351
		public string[] serverRegions;

		// Token: 0x040047B0 RID: 18352
		public bool isPrivate;

		// Token: 0x040047B1 RID: 18353
		public string customRoomID;

		// Token: 0x040047B2 RID: 18354
		public GameObject playerOffset;

		// Token: 0x040047B3 RID: 18355
		public SkinnedMeshRenderer[] offlineVRRig;

		// Token: 0x040047B4 RID: 18356
		public bool attemptingToConnect;

		// Token: 0x040047B5 RID: 18357
		private int currentRegionIndex;

		// Token: 0x040047B6 RID: 18358
		public string currentGameType;

		// Token: 0x040047B7 RID: 18359
		public bool roomCosmeticsInitialized;

		// Token: 0x040047B8 RID: 18360
		public GameObject photonVoiceObjectPrefab;

		// Token: 0x040047B9 RID: 18361
		public Dictionary<string, bool> playerCosmeticsLookup = new Dictionary<string, bool>();

		// Token: 0x040047BA RID: 18362
		private float lastHeadRightHandDistance;

		// Token: 0x040047BB RID: 18363
		private float lastHeadLeftHandDistance;

		// Token: 0x040047BC RID: 18364
		private float pauseTime;

		// Token: 0x040047BD RID: 18365
		private float disconnectTime = 120f;

		// Token: 0x040047BE RID: 18366
		public bool disableAFKKick;

		// Token: 0x040047BF RID: 18367
		private float headRightHandDistance;

		// Token: 0x040047C0 RID: 18368
		private float headLeftHandDistance;

		// Token: 0x040047C1 RID: 18369
		private Quaternion headQuat;

		// Token: 0x040047C2 RID: 18370
		private Quaternion lastHeadQuat;

		// Token: 0x040047C3 RID: 18371
		public GameObject[] disableOnStartup;

		// Token: 0x040047C4 RID: 18372
		public GameObject[] enableOnStartup;

		// Token: 0x040047C5 RID: 18373
		public bool updatedName;

		// Token: 0x040047C6 RID: 18374
		private int[] playersInRegion;

		// Token: 0x040047C7 RID: 18375
		private int[] pingInRegion;

		// Token: 0x040047C8 RID: 18376
		public List<string> friendIDList = new List<string>();

		// Token: 0x040047C9 RID: 18377
		private JoinType currentJoinType;

		// Token: 0x040047CA RID: 18378
		private string friendToFollow;

		// Token: 0x040047CB RID: 18379
		private string keyToFollow;

		// Token: 0x040047CC RID: 18380
		public string shuffler;

		// Token: 0x040047CD RID: 18381
		public string keyStr;

		// Token: 0x040047CE RID: 18382
		private string platformTag = "OTHER";

		// Token: 0x040047CF RID: 18383
		private string startLevel;

		// Token: 0x040047D0 RID: 18384
		[SerializeField]
		private GTZone startZone;

		// Token: 0x040047D1 RID: 18385
		private GorillaGeoHideShowTrigger startGeoTrigger;

		// Token: 0x040047D2 RID: 18386
		public GorillaNetworkJoinTrigger privateTrigger;

		// Token: 0x040047D3 RID: 18387
		internal string initialGameMode = "";

		// Token: 0x040047D4 RID: 18388
		public GorillaNetworkJoinTrigger currentJoinTrigger;

		// Token: 0x040047D5 RID: 18389
		public string autoJoinRoom;

		// Token: 0x040047D6 RID: 18390
		private bool deferredJoin;

		// Token: 0x040047D7 RID: 18391
		private float partyJoinDeferredUntilTimestamp;

		// Token: 0x040047D8 RID: 18392
		private DateTime? timeWhenApplicationPaused;

		// Token: 0x040047D9 RID: 18393
		[NetworkPrefab]
		[SerializeField]
		private NetworkObject testPlayerPrefab;

		// Token: 0x040047DA RID: 18394
		private List<GorillaNetworkJoinTrigger> allJoinTriggers = new List<GorillaNetworkJoinTrigger>();
	}
}
