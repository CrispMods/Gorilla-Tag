using System;
using GameObjectScheduling;
using GorillaNetworking;
using TMPro;
using UnityEngine;

// Token: 0x020005EE RID: 1518
public class ModeSelectButton : GorillaPressableButton
{
	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x060025C6 RID: 9670 RVA: 0x00048A8B File Offset: 0x00046C8B
	// (set) Token: 0x060025C7 RID: 9671 RVA: 0x00048A93 File Offset: 0x00046C93
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

	// Token: 0x060025C8 RID: 9672 RVA: 0x00048A9C File Offset: 0x00046C9C
	public override void Start()
	{
		base.Start();
		GorillaComputer.instance.currentGameMode.AddCallback(new Action<string>(this.OnGameModeChanged), true);
	}

	// Token: 0x060025C9 RID: 9673 RVA: 0x00048AC2 File Offset: 0x00046CC2
	private void OnDestroy()
	{
		if (!ApplicationQuittingState.IsQuitting)
		{
			GorillaComputer.instance.currentGameMode.RemoveCallback(new Action<string>(this.OnGameModeChanged));
		}
	}

	// Token: 0x060025CA RID: 9674 RVA: 0x00048AE8 File Offset: 0x00046CE8
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

	// Token: 0x060025CB RID: 9675 RVA: 0x00048B1D File Offset: 0x00046D1D
	public void OnGameModeChanged(string newGameMode)
	{
		this.buttonRenderer.material = ((newGameMode.ToLower() == this.gameMode.ToLower()) ? this.pressedMaterial : this.unpressedMaterial);
	}

	// Token: 0x060025CC RID: 9676 RVA: 0x00048B50 File Offset: 0x00046D50
	public void SetInfo(ModeSelectButtonInfoData info)
	{
		this.SetInfo(info.Mode, info.ModeTitle, info.NewMode, info.CountdownTo);
	}

	// Token: 0x060025CD RID: 9677 RVA: 0x001050AC File Offset: 0x001032AC
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

	// Token: 0x060025CE RID: 9678 RVA: 0x00048B70 File Offset: 0x00046D70
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
