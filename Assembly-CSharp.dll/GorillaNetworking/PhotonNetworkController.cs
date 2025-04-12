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
	// Token: 0x02000AD3 RID: 2771
	public class PhotonNetworkController : MonoBehaviour
	{
		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06004546 RID: 17734 RVA: 0x0005C5BF File Offset: 0x0005A7BF
		// (set) Token: 0x06004547 RID: 17735 RVA: 0x0005C5C7 File Offset: 0x0005A7C7
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

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06004548 RID: 17736 RVA: 0x0005C5D0 File Offset: 0x0005A7D0
		// (set) Token: 0x06004549 RID: 17737 RVA: 0x0005C5D8 File Offset: 0x0005A7D8
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

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x0600454A RID: 17738 RVA: 0x0005C5E1 File Offset: 0x0005A7E1
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

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x0600454B RID: 17739 RVA: 0x0005C5FF File Offset: 0x0005A7FF
		// (set) Token: 0x0600454C RID: 17740 RVA: 0x0005C607 File Offset: 0x0005A807
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

		// Token: 0x0600454D RID: 17741 RVA: 0x0017F6D8 File Offset: 0x0017D8D8
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

		// Token: 0x0600454E RID: 17742 RVA: 0x0017F748 File Offset: 0x0017D948
		public void Start()
		{
			base.StartCoroutine(this.DisableOnStart());
			NetworkSystem.Instance.OnJoinedRoomEvent += this.OnJoinedRoom;
			NetworkSystem.Instance.OnReturnedToSinglePlayer += this.OnDisconnected;
			PhotonNetwork.NetworkingClient.LoadBalancingPeer.ReuseEventInstance = true;
		}

		// Token: 0x0600454F RID: 17743 RVA: 0x0005C610 File Offset: 0x0005A810
		private IEnumerator DisableOnStart()
		{
			ZoneManagement.SetActiveZone(this.StartZone);
			yield break;
		}

		// Token: 0x06004550 RID: 17744 RVA: 0x0017F7A0 File Offset: 0x0017D9A0
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

		// Token: 0x06004551 RID: 17745 RVA: 0x0005C61F File Offset: 0x0005A81F
		public void DeferJoining(float duration)
		{
			this.partyJoinDeferredUntilTimestamp = Mathf.Max(this.partyJoinDeferredUntilTimestamp, Time.time + duration);
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x0005C639 File Offset: 0x0005A839
		public void ClearDeferredJoin()
		{
			this.partyJoinDeferredUntilTimestamp = 0f;
			this.deferredJoin = false;
		}

		// Token: 0x06004553 RID: 17747 RVA: 0x0005C64D File Offset: 0x0005A84D
		public void AttemptToJoinPublicRoom(GorillaNetworkJoinTrigger triggeredTrigger, JoinType roomJoinType = JoinType.Solo)
		{
			this.AttemptToJoinPublicRoomAsync(triggeredTrigger, roomJoinType);
		}

		// Token: 0x06004554 RID: 17748 RVA: 0x0017F9F0 File Offset: 0x0017DBF0
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

		// Token: 0x06004555 RID: 17749 RVA: 0x0017FA38 File Offset: 0x0017DC38
		private Task SendPartyFollowCommands()
		{
			PhotonNetworkController.<SendPartyFollowCommands>d__65 <SendPartyFollowCommands>d__;
			<SendPartyFollowCommands>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SendPartyFollowCommands>d__.<>1__state = -1;
			<SendPartyFollowCommands>d__.<>t__builder.Start<PhotonNetworkController.<SendPartyFollowCommands>d__65>(ref <SendPartyFollowCommands>d__);
			return <SendPartyFollowCommands>d__.<>t__builder.Task;
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x0005C657 File Offset: 0x0005A857
		public void AttemptToJoinSpecificRoom(string roomID, JoinType roomJoinType)
		{
			this.AttemptToJoinSpecificRoomAsync(roomID, roomJoinType, null);
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x0005C663 File Offset: 0x0005A863
		public void AttemptToJoinSpecificRoomWithCallback(string roomID, JoinType roomJoinType, Action<NetJoinResult> callback)
		{
			this.AttemptToJoinSpecificRoomAsync(roomID, roomJoinType, callback);
		}

		// Token: 0x06004558 RID: 17752 RVA: 0x0017FA74 File Offset: 0x0017DC74
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

		// Token: 0x06004559 RID: 17753 RVA: 0x0017FAD0 File Offset: 0x0017DCD0
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

		// Token: 0x0600455A RID: 17754 RVA: 0x0017FBB0 File Offset: 0x0017DDB0
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

		// Token: 0x0600455B RID: 17755 RVA: 0x0005C66F File Offset: 0x0005A86F
		public void RegisterJoinTrigger(GorillaNetworkJoinTrigger trigger)
		{
			this.allJoinTriggers.Add(trigger);
		}

		// Token: 0x0600455C RID: 17756 RVA: 0x0017FD30 File Offset: 0x0017DF30
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

		// Token: 0x0600455D RID: 17757 RVA: 0x0017FD9C File Offset: 0x0017DF9C
		public void UpdateTriggerScreens()
		{
			foreach (GorillaNetworkJoinTrigger gorillaNetworkJoinTrigger in this.allJoinTriggers)
			{
				gorillaNetworkJoinTrigger.UpdateUI();
			}
		}

		// Token: 0x0600455E RID: 17758 RVA: 0x0017FDEC File Offset: 0x0017DFEC
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

		// Token: 0x0600455F RID: 17759 RVA: 0x0005C67D File Offset: 0x0005A87D
		public void OnDisconnected()
		{
			this.DisconnectCleanup();
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x0005C685 File Offset: 0x0005A885
		public void OnApplicationQuit()
		{
			if (PhotonNetwork.IsConnected)
			{
				PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion != "dev";
			}
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x0005C6A8 File Offset: 0x0005A8A8
		private string ReturnRoomName()
		{
			if (this.isPrivate)
			{
				return this.customRoomID;
			}
			return this.RandomRoomName();
		}

		// Token: 0x06004562 RID: 17762 RVA: 0x0017FE50 File Offset: 0x0017E050
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

		// Token: 0x06004563 RID: 17763 RVA: 0x0005C6BF File Offset: 0x0005A8BF
		public byte GetRoomSize(string gameModeName)
		{
			return 10;
		}

		// Token: 0x06004564 RID: 17764 RVA: 0x0017FEA8 File Offset: 0x0017E0A8
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

		// Token: 0x06004565 RID: 17765 RVA: 0x0017FF28 File Offset: 0x0017E128
		public int TotalUsers()
		{
			int num = 0;
			foreach (int num2 in this.playersInRegion)
			{
				num += num2;
			}
			return num;
		}

		// Token: 0x06004566 RID: 17766 RVA: 0x0017FF58 File Offset: 0x0017E158
		public string CurrentState()
		{
			if (NetworkSystem.Instance == null)
			{
				Debug.Log("Null netsys!!!");
			}
			return NetworkSystem.Instance.netState.ToString();
		}

		// Token: 0x06004567 RID: 17767 RVA: 0x0017FF94 File Offset: 0x0017E194
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

		// Token: 0x06004568 RID: 17768 RVA: 0x0005C6C3 File Offset: 0x0005A8C3
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

		// Token: 0x040046C7 RID: 18119
		public static volatile PhotonNetworkController Instance;

		// Token: 0x040046C8 RID: 18120
		public int incrementCounter;

		// Token: 0x040046C9 RID: 18121
		public PlayFabAuthenticator playFabAuthenticator;

		// Token: 0x040046CA RID: 18122
		public string[] serverRegions;

		// Token: 0x040046CB RID: 18123
		public bool isPrivate;

		// Token: 0x040046CC RID: 18124
		public string customRoomID;

		// Token: 0x040046CD RID: 18125
		public GameObject playerOffset;

		// Token: 0x040046CE RID: 18126
		public SkinnedMeshRenderer[] offlineVRRig;

		// Token: 0x040046CF RID: 18127
		public bool attemptingToConnect;

		// Token: 0x040046D0 RID: 18128
		private int currentRegionIndex;

		// Token: 0x040046D1 RID: 18129
		public string currentGameType;

		// Token: 0x040046D2 RID: 18130
		public bool roomCosmeticsInitialized;

		// Token: 0x040046D3 RID: 18131
		public GameObject photonVoiceObjectPrefab;

		// Token: 0x040046D4 RID: 18132
		public Dictionary<string, bool> playerCosmeticsLookup = new Dictionary<string, bool>();

		// Token: 0x040046D5 RID: 18133
		private float lastHeadRightHandDistance;

		// Token: 0x040046D6 RID: 18134
		private float lastHeadLeftHandDistance;

		// Token: 0x040046D7 RID: 18135
		private float pauseTime;

		// Token: 0x040046D8 RID: 18136
		private float disconnectTime = 120f;

		// Token: 0x040046D9 RID: 18137
		public bool disableAFKKick;

		// Token: 0x040046DA RID: 18138
		private float headRightHandDistance;

		// Token: 0x040046DB RID: 18139
		private float headLeftHandDistance;

		// Token: 0x040046DC RID: 18140
		private Quaternion headQuat;

		// Token: 0x040046DD RID: 18141
		private Quaternion lastHeadQuat;

		// Token: 0x040046DE RID: 18142
		public GameObject[] disableOnStartup;

		// Token: 0x040046DF RID: 18143
		public GameObject[] enableOnStartup;

		// Token: 0x040046E0 RID: 18144
		public bool updatedName;

		// Token: 0x040046E1 RID: 18145
		private int[] playersInRegion;

		// Token: 0x040046E2 RID: 18146
		private int[] pingInRegion;

		// Token: 0x040046E3 RID: 18147
		public List<string> friendIDList = new List<string>();

		// Token: 0x040046E4 RID: 18148
		private JoinType currentJoinType;

		// Token: 0x040046E5 RID: 18149
		private string friendToFollow;

		// Token: 0x040046E6 RID: 18150
		private string keyToFollow;

		// Token: 0x040046E7 RID: 18151
		public string shuffler;

		// Token: 0x040046E8 RID: 18152
		public string keyStr;

		// Token: 0x040046E9 RID: 18153
		private string platformTag = "OTHER";

		// Token: 0x040046EA RID: 18154
		private string startLevel;

		// Token: 0x040046EB RID: 18155
		[SerializeField]
		private GTZone startZone;

		// Token: 0x040046EC RID: 18156
		private GorillaGeoHideShowTrigger startGeoTrigger;

		// Token: 0x040046ED RID: 18157
		public GorillaNetworkJoinTrigger privateTrigger;

		// Token: 0x040046EE RID: 18158
		internal string initialGameMode = "";

		// Token: 0x040046EF RID: 18159
		public GorillaNetworkJoinTrigger currentJoinTrigger;

		// Token: 0x040046F0 RID: 18160
		public string autoJoinRoom;

		// Token: 0x040046F1 RID: 18161
		private bool deferredJoin;

		// Token: 0x040046F2 RID: 18162
		private float partyJoinDeferredUntilTimestamp;

		// Token: 0x040046F3 RID: 18163
		private DateTime? timeWhenApplicationPaused;

		// Token: 0x040046F4 RID: 18164
		[NetworkPrefab]
		[SerializeField]
		private NetworkObject testPlayerPrefab;

		// Token: 0x040046F5 RID: 18165
		private List<GorillaNetworkJoinTrigger> allJoinTriggers = new List<GorillaNetworkJoinTrigger>();
	}
}
