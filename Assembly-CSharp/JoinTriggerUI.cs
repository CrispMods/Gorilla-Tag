using System;
using GorillaNetworking;
using TMPro;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class JoinTriggerUI : MonoBehaviour
{
	// Token: 0x06000ADF RID: 2783 RVA: 0x00037A59 File Offset: 0x00035C59
	private void Awake()
	{
		this.joinTriggerRef.TryResolve<GorillaNetworkJoinTrigger>(out this.joinTrigger);
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x00037A6D File Offset: 0x00035C6D
	private void Start()
	{
		this.didStart = true;
		this.OnEnable();
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x00037A7C File Offset: 0x00035C7C
	private void OnEnable()
	{
		if (this.didStart)
		{
			this.joinTrigger.RegisterUI(this);
		}
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x00037A92 File Offset: 0x00035C92
	private void OnDisable()
	{
		this.joinTrigger.UnregisterUI(this);
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x00098D94 File Offset: 0x00096F94
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

	// Token: 0x04000D2A RID: 3370
	[SerializeField]
	private XSceneRef joinTriggerRef;

	// Token: 0x04000D2B RID: 3371
	private GorillaNetworkJoinTrigger joinTrigger;

	// Token: 0x04000D2C RID: 3372
	[SerializeField]
	private MeshRenderer milestoneRenderer;

	// Token: 0x04000D2D RID: 3373
	[SerializeField]
	private MeshRenderer screenBGRenderer;

	// Token: 0x04000D2E RID: 3374
	[SerializeField]
	private TextMeshPro screenText;

	// Token: 0x04000D2F RID: 3375
	[SerializeField]
	private JoinTriggerUITemplate template;

	// Token: 0x04000D30 RID: 3376
	private bool didStart;
}
