using System;
using GorillaGameModes;
using GorillaTagScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaNetworking
{
	// Token: 0x02000ABB RID: 2747
	public class GorillaNetworkJoinTrigger : GorillaTriggerBox
	{
		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x060044F3 RID: 17651 RVA: 0x00147C90 File Offset: 0x00145E90
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

		// Token: 0x060044F4 RID: 17652 RVA: 0x00147CC0 File Offset: 0x00145EC0
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

		// Token: 0x060044F5 RID: 17653 RVA: 0x00147D40 File Offset: 0x00145F40
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

		// Token: 0x060044F6 RID: 17654 RVA: 0x00147D8C File Offset: 0x00145F8C
		public void UnregisterUI(JoinTriggerUI ui)
		{
			this.ui = null;
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x00147D95 File Offset: 0x00145F95
		private void OnDestroy()
		{
			if (this.didRegisterForCallbacks)
			{
				FriendshipGroupDetection.Instance.RemoveGroupZoneCallback(new Action<GroupJoinZoneAB>(this.OnGroupPositionsChanged));
			}
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x00147DB5 File Offset: 0x00145FB5
		private void OnGroupPositionsChanged(GroupJoinZoneAB groupZone)
		{
			this.UpdateUI();
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x00147DC0 File Offset: 0x00145FC0
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

		// Token: 0x060044FA RID: 17658 RVA: 0x0014800A File Offset: 0x0014620A
		private string GetActiveNetworkZone()
		{
			return PhotonNetworkController.Instance.currentJoinTrigger.networkZone.ToUpper();
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x00148022 File Offset: 0x00146222
		private string GetDesiredNetworkZone()
		{
			return this.networkZone.ToUpper();
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x0014802F File Offset: 0x0014622F
		public static string GetActiveGameType()
		{
			GorillaGameManager activeGameMode = GameMode.ActiveGameMode;
			return ((activeGameMode != null) ? activeGameMode.GameModeName() : null) ?? "";
		}

		// Token: 0x060044FD RID: 17661 RVA: 0x0014804C File Offset: 0x0014624C
		public string GetDesiredGameType()
		{
			return GameMode.GameModeZoneMapping.VerifyModeForZone(this.zone, Enum.Parse<GameModeType>(GorillaComputer.instance.currentGameMode.Value, true), NetworkSystem.Instance.SessionIsPrivate).ToString();
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x00148098 File Offset: 0x00146298
		public virtual string GetFullDesiredGameModeString()
		{
			return this.networkZone + GorillaComputer.instance.currentQueue + this.GetDesiredGameType();
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x001480B7 File Offset: 0x001462B7
		public bool CanPartyJoin()
		{
			return this.CanPartyJoin(FriendshipGroupDetection.Instance.partyZone);
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x001480C9 File Offset: 0x001462C9
		public bool CanPartyJoin(GroupJoinZoneAB zone)
		{
			return (this.groupJoinRequiredZonesAB & zone) == zone;
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x001480E0 File Offset: 0x001462E0
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

		// Token: 0x06004502 RID: 17666 RVA: 0x0014828F File Offset: 0x0014648F
		public static void DisableTriggerJoins()
		{
			Debug.Log("[GorillaNetworkJoinTrigger::DisableTriggerJoins] Disabling Trigger-based Room Joins...");
			GorillaNetworkJoinTrigger.triggerJoinsDisabled = true;
		}

		// Token: 0x06004503 RID: 17667 RVA: 0x001482A1 File Offset: 0x001464A1
		public static void EnableTriggerJoins()
		{
			Debug.Log("[GorillaNetworkJoinTrigger::EnableTriggerJoins] Enabling Trigger-based Room Joins...");
			GorillaNetworkJoinTrigger.triggerJoinsDisabled = false;
		}

		// Token: 0x04004674 RID: 18036
		public GameObject[] makeSureThisIsDisabled;

		// Token: 0x04004675 RID: 18037
		public GameObject[] makeSureThisIsEnabled;

		// Token: 0x04004676 RID: 18038
		public GTZone zone;

		// Token: 0x04004677 RID: 18039
		public GroupJoinZoneA groupJoinRequiredZones;

		// Token: 0x04004678 RID: 18040
		public GroupJoinZoneB groupJoinRequiredZonesB;

		// Token: 0x04004679 RID: 18041
		[FormerlySerializedAs("gameModeName")]
		public string networkZone;

		// Token: 0x0400467A RID: 18042
		public string componentTypeToAdd;

		// Token: 0x0400467B RID: 18043
		public GameObject componentTarget;

		// Token: 0x0400467C RID: 18044
		public GorillaFriendCollider myCollider;

		// Token: 0x0400467D RID: 18045
		public GorillaNetworkJoinTrigger primaryTriggerForMyZone;

		// Token: 0x0400467E RID: 18046
		public bool ignoredIfInParty;

		// Token: 0x0400467F RID: 18047
		private JoinTriggerUI ui;

		// Token: 0x04004680 RID: 18048
		private bool didRegisterForCallbacks;

		// Token: 0x04004681 RID: 18049
		private static bool triggerJoinsDisabled;
	}
}
