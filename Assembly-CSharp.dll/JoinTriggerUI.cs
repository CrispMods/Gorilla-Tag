using System;
using GorillaNetworking;
using TMPro;
using UnityEngine;

// Token: 0x020001C4 RID: 452
public class JoinTriggerUI : MonoBehaviour
{
	// Token: 0x06000A95 RID: 2709 RVA: 0x00036799 File Offset: 0x00034999
	private void Awake()
	{
		this.joinTriggerRef.TryResolve<GorillaNetworkJoinTrigger>(out this.joinTrigger);
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x000367AD File Offset: 0x000349AD
	private void Start()
	{
		this.didStart = true;
		this.OnEnable();
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x000367BC File Offset: 0x000349BC
	private void OnEnable()
	{
		if (this.didStart)
		{
			this.joinTrigger.RegisterUI(this);
		}
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x000367D2 File Offset: 0x000349D2
	private void OnDisable()
	{
		this.joinTrigger.UnregisterUI(this);
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x000964A0 File Offset: 0x000946A0
	public void SetState(JoinTriggerVisualState state, Func<string> oldZone, Func<string> newZone, Func<string> oldGameMode, Func<string> newGameMode)
	{
		switch (state)
		{
		case JoinTriggerVisualState.ConnectionError:
			this.milestoneRenderer.sharedMaterial = this.template.Milestone_Error;
			this.screenBGRenderer.sharedMaterial = this.template.ScreenBG_Error;
			this.screenText.text = (this.template.showFullErrorMessages ? GorillaScoreboardTotalUpdater.instance.offlineTextErrorString : this.template.ScreenText_Error);
			return;
		case JoinTriggerVisualState.AlreadyInRoom:
			this.milestoneRenderer.sharedMaterial = this.template.Milestone_AlreadyInRoom;
			this.screenBGRenderer.sharedMaterial = this.template.ScreenBG_AlreadyInRoom;
			this.screenText.text = this.template.ScreenText_AlreadyInRoom.GetText(oldZone, newZone, oldGameMode, newGameMode);
			return;
		case JoinTriggerVisualState.InPrivateRoom:
			this.milestoneRenderer.sharedMaterial = this.template.Milestone_InPrivateRoom;
			this.screenBGRenderer.sharedMaterial = this.template.ScreenBG_InPrivateRoom;
			this.screenText.text = this.template.ScreenText_InPrivateRoom.GetText(oldZone, newZone, oldGameMode, newGameMode);
			return;
		case JoinTriggerVisualState.NotConnectedSoloJoin:
			this.milestoneRenderer.sharedMaterial = this.template.Milestone_NotConnectedSoloJoin;
			this.screenBGRenderer.sharedMaterial = this.template.ScreenBG_NotConnectedSoloJoin;
			this.screenText.text = this.template.ScreenText_NotConnectedSoloJoin.GetText(oldZone, newZone, oldGameMode, newGameMode);
			return;
		case JoinTriggerVisualState.LeaveRoomAndSoloJoin:
			this.milestoneRenderer.sharedMaterial = this.template.Milestone_LeaveRoomAndSoloJoin;
			this.screenBGRenderer.sharedMaterial = this.template.ScreenBG_LeaveRoomAndSoloJoin;
			this.screenText.text = this.template.ScreenText_LeaveRoomAndSoloJoin.GetText(oldZone, newZone, oldGameMode, newGameMode);
			return;
		case JoinTriggerVisualState.LeaveRoomAndPartyJoin:
			this.milestoneRenderer.sharedMaterial = this.template.Milestone_LeaveRoomAndGroupJoin;
			this.screenBGRenderer.sharedMaterial = this.template.ScreenBG_LeaveRoomAndGroupJoin;
			this.screenText.text = this.template.ScreenText_LeaveRoomAndGroupJoin.GetText(oldZone, newZone, oldGameMode, newGameMode);
			return;
		case JoinTriggerVisualState.AbandonPartyAndSoloJoin:
			this.milestoneRenderer.sharedMaterial = this.template.Milestone_AbandonPartyAndSoloJoin;
			this.screenBGRenderer.sharedMaterial = this.template.ScreenBG_AbandonPartyAndSoloJoin;
			this.screenText.text = this.template.ScreenText_AbandonPartyAndSoloJoin.GetText(oldZone, newZone, oldGameMode, newGameMode);
			return;
		case JoinTriggerVisualState.ChangingGameModeSoloJoin:
			this.milestoneRenderer.sharedMaterial = this.template.Milestone_ChangingGameModeSoloJoin;
			this.screenBGRenderer.sharedMaterial = this.template.ScreenBG_ChangingGameModeSoloJoin;
			this.screenText.text = this.template.ScreenText_ChangingGameModeSoloJoin.GetText(oldZone, newZone, oldGameMode, newGameMode);
			return;
		default:
			return;
		}
	}

	// Token: 0x04000CE5 RID: 3301
	[SerializeField]
	private XSceneRef joinTriggerRef;

	// Token: 0x04000CE6 RID: 3302
	private GorillaNetworkJoinTrigger joinTrigger;

	// Token: 0x04000CE7 RID: 3303
	[SerializeField]
	private MeshRenderer milestoneRenderer;

	// Token: 0x04000CE8 RID: 3304
	[SerializeField]
	private MeshRenderer screenBGRenderer;

	// Token: 0x04000CE9 RID: 3305
	[SerializeField]
	private TextMeshPro screenText;

	// Token: 0x04000CEA RID: 3306
	[SerializeField]
	private JoinTriggerUITemplate template;

	// Token: 0x04000CEB RID: 3307
	private bool didStart;
}
