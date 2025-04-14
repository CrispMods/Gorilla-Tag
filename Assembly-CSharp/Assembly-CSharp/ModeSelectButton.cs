using System;
using GameObjectScheduling;
using GorillaNetworking;
using TMPro;
using UnityEngine;

// Token: 0x020005EE RID: 1518
public class ModeSelectButton : GorillaPressableButton
{
	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x060025C6 RID: 9670 RVA: 0x000BAA6A File Offset: 0x000B8C6A
	// (set) Token: 0x060025C7 RID: 9671 RVA: 0x000BAA72 File Offset: 0x000B8C72
	public PartyGameModeWarning WarningScreen
	{
		get
		{
			return this.warningScreen;
		}
		set
		{
			this.warningScreen = value;
		}
	}

	// Token: 0x060025C8 RID: 9672 RVA: 0x000BAA7B File Offset: 0x000B8C7B
	public override void Start()
	{
		base.Start();
		GorillaComputer.instance.currentGameMode.AddCallback(new Action<string>(this.OnGameModeChanged), true);
	}

	// Token: 0x060025C9 RID: 9673 RVA: 0x000BAAA1 File Offset: 0x000B8CA1
	private void OnDestroy()
	{
		if (!ApplicationQuittingState.IsQuitting)
		{
			GorillaComputer.instance.currentGameMode.RemoveCallback(new Action<string>(this.OnGameModeChanged));
		}
	}

	// Token: 0x060025CA RID: 9674 RVA: 0x000BAAC7 File Offset: 0x000B8CC7
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivationWithHand(isLeftHand);
		if (this.warningScreen.ShouldShowWarning)
		{
			this.warningScreen.Show();
			return;
		}
		GorillaComputer.instance.OnModeSelectButtonPress(this.gameMode, isLeftHand);
	}

	// Token: 0x060025CB RID: 9675 RVA: 0x000BAAFC File Offset: 0x000B8CFC
	public void OnGameModeChanged(string newGameMode)
	{
		this.buttonRenderer.material = ((newGameMode.ToLower() == this.gameMode.ToLower()) ? this.pressedMaterial : this.unpressedMaterial);
	}

	// Token: 0x060025CC RID: 9676 RVA: 0x000BAB2F File Offset: 0x000B8D2F
	public void SetInfo(ModeSelectButtonInfoData info)
	{
		this.SetInfo(info.Mode, info.ModeTitle, info.NewMode, info.CountdownTo);
	}

	// Token: 0x060025CD RID: 9677 RVA: 0x000BAB50 File Offset: 0x000B8D50
	public void SetInfo(string Mode, string ModeTitle, bool NewMode, CountdownTextDate CountdownTo)
	{
		this.gameModeTitle.text = ModeTitle;
		this.gameMode = Mode;
		this.newModeSplash.SetActive(NewMode);
		this.limitedCountdown.gameObject.SetActive(false);
		if (CountdownTo == null)
		{
			return;
		}
		this.limitedCountdown.Countdown = CountdownTo;
		this.limitedCountdown.gameObject.SetActive(true);
	}

	// Token: 0x060025CE RID: 9678 RVA: 0x000BABB6 File Offset: 0x000B8DB6
	public void HideNewAndLimitedTimeInfo()
	{
		this.limitedCountdown.gameObject.SetActive(false);
		this.newModeSplash.SetActive(false);
	}

	// Token: 0x040029E2 RID: 10722
	[SerializeField]
	public string gameMode;

	// Token: 0x040029E3 RID: 10723
	[SerializeField]
	private PartyGameModeWarning warningScreen;

	// Token: 0x040029E4 RID: 10724
	[SerializeField]
	private TMP_Text gameModeTitle;

	// Token: 0x040029E5 RID: 10725
	[SerializeField]
	private GameObject newModeSplash;

	// Token: 0x040029E6 RID: 10726
	[SerializeField]
	private CountdownText limitedCountdown;
}
