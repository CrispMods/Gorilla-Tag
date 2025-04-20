using System;
using GorillaGameModes;
using GorillaTagScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaNetworking
{
	// Token: 0x02000AE5 RID: 2789
	public class GorillaNetworkJoinTrigger : GorillaTriggerBox
	{
		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x0600462C RID: 17964 RVA: 0x00185188 File Offset: 0x00183388
		public GroupJoinZoneAB groupJoinRequiredZonesAB
		{
			get
			{
				return new GroupJoinZoneAB
				{
					a = this.groupJoinRequiredZones,
					b = this.groupJoinRequiredZonesB
				};
			}
		}

		// Token: 0x0600462D RID: 17965 RVA: 0x001851B8 File Offset: 0x001833B8
		private void Start()
		{
			if (this.primaryTriggerForMyZone == null)
			{
				this.primaryTriggerForMyZone = this;
			}
			if (this.primaryTriggerForMyZone == this)
			{
				GorillaComputer.instance.RegisterPrimaryJoinTrigger(this);
			}
			PhotonNetworkController.Instance.RegisterJoinTrigger(this);
			if (!this.didRegisterForCallbacks && this.ui != null)
			{
				this.didRegisterForCallbacks = true;
				FriendshipGroupDetection.Instance.AddGroupZoneCallback(new Action<GroupJoinZoneAB>(this.OnGroupPositionsChanged));
			}
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x00185238 File Offset: 0x00183438
		public void RegisterUI(JoinTriggerUI ui)
		{
			this.ui = ui;
			if (!this.didRegisterForCallbacks && FriendshipGroupDetection.Instance != null)
			{
				this.didRegisterForCallbacks = true;
				FriendshipGroupDetection.Instance.AddGroupZoneCallback(new Action<GroupJoinZoneAB>(this.OnGroupPositionsChanged));
			}
			this.UpdateUI();
		}

		// Token: 0x0600462F RID: 17967 RVA: 0x0005DCC9 File Offset: 0x0005BEC9
		public void UnregisterUI(JoinTriggerUI ui)
		{
			this.ui = null;
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x0005DCD2 File Offset: 0x0005BED2
		private void OnDestroy()
		{
			if (this.didRegisterForCallbacks)
			{
				FriendshipGroupDetection.Instance.RemoveGroupZoneCallback(new Action<GroupJoinZoneAB>(this.OnGroupPositionsChanged));
			}
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x0005DCF2 File Offset: 0x0005BEF2
		private void OnGroupPositionsChanged(GroupJoinZoneAB groupZone)
		{
			this.UpdateUI();
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x00185284 File Offset: 0x00183484
		public void UpdateUI()
		{
			if (this.ui == null || NetworkSystem.Instance == null)
			{
				return;
			}
			if (GorillaScoreboardTotalUpdater.instance.offlineTextErrorString != null)
			{
				this.ui.SetState(JoinTriggerVisualState.ConnectionError, new Func<string>(this.GetActiveNetworkZone), new Func<string>(this.GetDesiredNetworkZone), new Func<string>(GorillaNetworkJoinTrigger.GetActiveGameType), new Func<string>(this.GetDesiredGameType));
				return;
			}
			if (NetworkSystem.Instance.SessionIsPrivate)
			{
				this.ui.SetState(JoinTriggerVisualState.InPrivateRoom, new Func<string>(this.GetActiveNetworkZone), new Func<string>(this.GetDesiredNetworkZone), new Func<string>(GorillaNetworkJoinTrigger.GetActiveGameType), new Func<string>(this.GetDesiredGameType));
				return;
			}
			if (NetworkSystem.Instance.InRoom && NetworkSystem.Instance.GameModeString == this.GetFullDesiredGameModeString())
			{
				this.ui.SetState(JoinTriggerVisualState.AlreadyInRoom, new Func<string>(this.GetActiveNetworkZone), new Func<string>(this.GetDesiredNetworkZone), new Func<string>(GorillaNetworkJoinTrigger.GetActiveGameType), new Func<string>(this.GetDesiredGameType));
				return;
			}
			if (FriendshipGroupDetection.Instance.IsInParty)
			{
				this.ui.SetState(this.CanPartyJoin() ? JoinTriggerVisualState.LeaveRoomAndPartyJoin : JoinTriggerVisualState.AbandonPartyAndSoloJoin, new Func<string>(this.GetActiveNetworkZone), new Func<string>(this.GetDesiredNetworkZone), new Func<string>(GorillaNetworkJoinTrigger.GetActiveGameType), new Func<string>(this.GetDesiredGameType));
				return;
			}
			if (!NetworkSystem.Instance.InRoom)
			{
				this.ui.SetState(JoinTriggerVisualState.NotConnectedSoloJoin, new Func<string>(this.GetActiveNetworkZone), new Func<string>(this.GetDesiredNetworkZone), new Func<string>(GorillaNetworkJoinTrigger.GetActiveGameType), new Func<string>(this.GetDesiredGameType));
				return;
			}
			if (PhotonNetworkController.Instance.currentJoinTrigger == this.primaryTriggerForMyZone)
			{
				this.ui.SetState(JoinTriggerVisualState.ChangingGameModeSoloJoin, new Func<string>(this.GetActiveNetworkZone), new Func<string>(this.GetDesiredNetworkZone), new Func<string>(GorillaNetworkJoinTrigger.GetActiveGameType), new Func<string>(this.GetDesiredGameType));
				return;
			}
			this.ui.SetState(JoinTriggerVisualState.LeaveRoomAndSoloJoin, new Func<string>(this.GetActiveNetworkZone), new Func<string>(this.GetDesiredNetworkZone), new Func<string>(GorillaNetworkJoinTrigger.GetActiveGameType), new Func<string>(this.GetDesiredGameType));
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x0005DCFA File Offset: 0x0005BEFA
		private string GetActiveNetworkZone()
		{
			return PhotonNetworkController.Instance.currentJoinTrigger.networkZone.ToUpper();
		}

		// Token: 0x06004634 RID: 17972 RVA: 0x0005DD12 File Offset: 0x0005BF12
		private string GetDesiredNetworkZone()
		{
			return this.networkZone.ToUpper();
		}

		// Token: 0x06004635 RID: 17973 RVA: 0x0005DD1F File Offset: 0x0005BF1F
		public static string GetActiveGameType()
		{
			GorillaGameManager activeGameMode = GameMode.ActiveGameMode;
			return ((activeGameMode != null) ? activeGameMode.GameModeName() : null) ?? "";
		}

		// Token: 0x06004636 RID: 17974 RVA: 0x001854D0 File Offset: 0x001836D0
		public string GetDesiredGameType()
		{
			return GameMode.GameModeZoneMapping.VerifyModeForZone(this.zone, Enum.Parse<GameModeType>(GorillaComputer.instance.currentGameMode.Value, true), NetworkSystem.Instance.SessionIsPrivate).ToString();
		}

		// Token: 0x06004637 RID: 17975 RVA: 0x0005DD3B File Offset: 0x0005BF3B
		public virtual string GetFullDesiredGameModeString()
		{
			return this.networkZone + GorillaComputer.instance.currentQueue + this.GetDesiredGameType();
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x0005DD5A File Offset: 0x0005BF5A
		public bool CanPartyJoin()
		{
			return this.CanPartyJoin(FriendshipGroupDetection.Instance.partyZone);
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x0005DD6C File Offset: 0x0005BF6C
		public bool CanPartyJoin(GroupJoinZoneAB zone)
		{
			return (this.groupJoinRequiredZonesAB & zone) == zone;
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x0018551C File Offset: 0x0018371C
		public override void OnBoxTriggered()
		{
			base.OnBoxTriggered();
			if (GorillaNetworkJoinTrigger.triggerJoinsDisabled)
			{
				Debug.Log("GorillaNetworkJoinTrigger::OnBoxTriggered - blocking join call");
				return;
			}
			GorillaComputer.instance.allowedMapsToJoin = this.myCollider.myAllowedMapsToJoin;
			if (NetworkSystem.Instance.groupJoinInProgress)
			{
				return;
			}
			if (FriendshipGroupDetection.Instance.IsInParty)
			{
				if (this.ignoredIfInParty)
				{
					return;
				}
				if (NetworkSystem.Instance.netState == NetSystemState.Connecting || NetworkSystem.Instance.netState == NetSystemState.Disconnecting || NetworkSystem.Instance.netState == NetSystemState.Initialization || NetworkSystem.Instance.netState == NetSystemState.PingRecon)
				{
					return;
				}
				if (NetworkSystem.Instance.InRoom)
				{
					if (NetworkSystem.Instance.GameModeString == this.GetFullDesiredGameModeString())
					{
						Debug.Log("JoinTrigger: Ignoring party join/leave because " + this.networkZone + " is already the game mode");
						return;
					}
					if (NetworkSystem.Instance.SessionIsPrivate)
					{
						Debug.Log("JoinTrigger: Ignoring party join/leave because we're in a private room");
						return;
					}
				}
				if (this.CanPartyJoin())
				{
					Debug.Log(string.Format("JoinTrigger: Attempting party join in 1 second! <{0}> accepts <{1}>", this.groupJoinRequiredZones, FriendshipGroupDetection.Instance.partyZone));
					PhotonNetworkController.Instance.DeferJoining(1f);
					FriendshipGroupDetection.Instance.SendAboutToGroupJoin();
					PhotonNetworkController.Instance.AttemptToJoinPublicRoom(this, JoinType.JoinWithParty);
					return;
				}
				Debug.Log(string.Format("JoinTrigger: LeaveGroup: Leaving party and will solo join, wanted <{0}> but got <{1}>", this.groupJoinRequiredZones, FriendshipGroupDetection.Instance.partyZone));
				FriendshipGroupDetection.Instance.LeaveParty();
				PhotonNetworkController.Instance.DeferJoining(1f);
			}
			else
			{
				Debug.Log("JoinTrigger: Solo join (not in a group)");
				PhotonNetworkController.Instance.ClearDeferredJoin();
			}
			PhotonNetworkController.Instance.AttemptToJoinPublicRoom(this, JoinType.Solo);
		}

		// Token: 0x0600463B RID: 17979 RVA: 0x0005DD80 File Offset: 0x0005BF80
		public static void DisableTriggerJoins()
		{
			Debug.Log("[GorillaNetworkJoinTrigger::DisableTriggerJoins] Disabling Trigger-based Room Joins...");
			GorillaNetworkJoinTrigger.triggerJoinsDisabled = true;
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x0005DD92 File Offset: 0x0005BF92
		public static void EnableTriggerJoins()
		{
			Debug.Log("[GorillaNetworkJoinTrigger::EnableTriggerJoins] Enabling Trigger-based Room Joins...");
			GorillaNetworkJoinTrigger.triggerJoinsDisabled = false;
		}

		// Token: 0x04004759 RID: 18265
		public GameObject[] makeSureThisIsDisabled;

		// Token: 0x0400475A RID: 18266
		public GameObject[] makeSureThisIsEnabled;

		// Token: 0x0400475B RID: 18267
		public GTZone zone;

		// Token: 0x0400475C RID: 18268
		public GroupJoinZoneA groupJoinRequiredZones;

		// Token: 0x0400475D RID: 18269
		public GroupJoinZoneB groupJoinRequiredZonesB;

		// Token: 0x0400475E RID: 18270
		[FormerlySerializedAs("gameModeName")]
		public string networkZone;

		// Token: 0x0400475F RID: 18271
		public string componentTypeToAdd;

		// Token: 0x04004760 RID: 18272
		public GameObject componentTarget;

		// Token: 0x04004761 RID: 18273
		public GorillaFriendCollider myCollider;

		// Token: 0x04004762 RID: 18274
		public GorillaNetworkJoinTrigger primaryTriggerForMyZone;

		// Token: 0x04004763 RID: 18275
		public bool ignoredIfInParty;

		// Token: 0x04004764 RID: 18276
		private JoinTriggerUI ui;

		// Token: 0x04004765 RID: 18277
		private bool didRegisterForCallbacks;

		// Token: 0x04004766 RID: 18278
		private static bool triggerJoinsDisabled;
	}
}
