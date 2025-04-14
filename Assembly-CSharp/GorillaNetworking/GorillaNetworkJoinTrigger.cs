using System;
using GorillaGameModes;
using GorillaTagScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaNetworking
{
	// Token: 0x02000AB8 RID: 2744
	public class GorillaNetworkJoinTrigger : GorillaTriggerBox
	{
		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060044E7 RID: 17639 RVA: 0x001476C8 File Offset: 0x001458C8
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

		// Token: 0x060044E8 RID: 17640 RVA: 0x001476F8 File Offset: 0x001458F8
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

		// Token: 0x060044E9 RID: 17641 RVA: 0x00147778 File Offset: 0x00145978
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

		// Token: 0x060044EA RID: 17642 RVA: 0x001477C4 File Offset: 0x001459C4
		public void UnregisterUI(JoinTriggerUI ui)
		{
			this.ui = null;
		}

		// Token: 0x060044EB RID: 17643 RVA: 0x001477CD File Offset: 0x001459CD
		private void OnDestroy()
		{
			if (this.didRegisterForCallbacks)
			{
				FriendshipGroupDetection.Instance.RemoveGroupZoneCallback(new Action<GroupJoinZoneAB>(this.OnGroupPositionsChanged));
			}
		}

		// Token: 0x060044EC RID: 17644 RVA: 0x001477ED File Offset: 0x001459ED
		private void OnGroupPositionsChanged(GroupJoinZoneAB groupZone)
		{
			this.UpdateUI();
		}

		// Token: 0x060044ED RID: 17645 RVA: 0x001477F8 File Offset: 0x001459F8
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

		// Token: 0x060044EE RID: 17646 RVA: 0x00147A42 File Offset: 0x00145C42
		private string GetActiveNetworkZone()
		{
			return PhotonNetworkController.Instance.currentJoinTrigger.networkZone.ToUpper();
		}

		// Token: 0x060044EF RID: 17647 RVA: 0x00147A5A File Offset: 0x00145C5A
		private string GetDesiredNetworkZone()
		{
			return this.networkZone.ToUpper();
		}

		// Token: 0x060044F0 RID: 17648 RVA: 0x00147A67 File Offset: 0x00145C67
		public static string GetActiveGameType()
		{
			GorillaGameManager activeGameMode = GameMode.ActiveGameMode;
			return ((activeGameMode != null) ? activeGameMode.GameModeName() : null) ?? "";
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x00147A84 File Offset: 0x00145C84
		public string GetDesiredGameType()
		{
			return GameMode.GameModeZoneMapping.VerifyModeForZone(this.zone, Enum.Parse<GameModeType>(GorillaComputer.instance.currentGameMode.Value, true), NetworkSystem.Instance.SessionIsPrivate).ToString();
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x00147AD0 File Offset: 0x00145CD0
		public virtual string GetFullDesiredGameModeString()
		{
			return this.networkZone + GorillaComputer.instance.currentQueue + this.GetDesiredGameType();
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x00147AEF File Offset: 0x00145CEF
		public bool CanPartyJoin()
		{
			return this.CanPartyJoin(FriendshipGroupDetection.Instance.partyZone);
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x00147B01 File Offset: 0x00145D01
		public bool CanPartyJoin(GroupJoinZoneAB zone)
		{
			return (this.groupJoinRequiredZonesAB & zone) == zone;
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x00147B18 File Offset: 0x00145D18
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

		// Token: 0x060044F6 RID: 17654 RVA: 0x00147CC7 File Offset: 0x00145EC7
		public static void DisableTriggerJoins()
		{
			Debug.Log("[GorillaNetworkJoinTrigger::DisableTriggerJoins] Disabling Trigger-based Room Joins...");
			GorillaNetworkJoinTrigger.triggerJoinsDisabled = true;
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x00147CD9 File Offset: 0x00145ED9
		public static void EnableTriggerJoins()
		{
			Debug.Log("[GorillaNetworkJoinTrigger::EnableTriggerJoins] Enabling Trigger-based Room Joins...");
			GorillaNetworkJoinTrigger.triggerJoinsDisabled = false;
		}

		// Token: 0x04004662 RID: 18018
		public GameObject[] makeSureThisIsDisabled;

		// Token: 0x04004663 RID: 18019
		public GameObject[] makeSureThisIsEnabled;

		// Token: 0x04004664 RID: 18020
		public GTZone zone;

		// Token: 0x04004665 RID: 18021
		public GroupJoinZoneA groupJoinRequiredZones;

		// Token: 0x04004666 RID: 18022
		public GroupJoinZoneB groupJoinRequiredZonesB;

		// Token: 0x04004667 RID: 18023
		[FormerlySerializedAs("gameModeName")]
		public string networkZone;

		// Token: 0x04004668 RID: 18024
		public string componentTypeToAdd;

		// Token: 0x04004669 RID: 18025
		public GameObject componentTarget;

		// Token: 0x0400466A RID: 18026
		public GorillaFriendCollider myCollider;

		// Token: 0x0400466B RID: 18027
		public GorillaNetworkJoinTrigger primaryTriggerForMyZone;

		// Token: 0x0400466C RID: 18028
		public bool ignoredIfInParty;

		// Token: 0x0400466D RID: 18029
		private JoinTriggerUI ui;

		// Token: 0x0400466E RID: 18030
		private bool didRegisterForCallbacks;

		// Token: 0x0400466F RID: 18031
		private static bool triggerJoinsDisabled;
	}
}
