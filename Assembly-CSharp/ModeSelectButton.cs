using System;
using GameObjectScheduling;
using GorillaNetworking;
using TMPro;
using UnityEngine;

// Token: 0x020005FB RID: 1531
public class ModeSelectButton : GorillaPressableButton
{
	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x06002620 RID: 9760 RVA: 0x00049E62 File Offset: 0x00048062
	// (set) Token: 0x06002621 RID: 9761 RVA: 0x00049E6A File Offset: 0x0004806A
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

	// Token: 0x06002622 RID: 9762 RVA: 0x00049E73 File Offset: 0x00048073
	public override void Start()
	{
		base.Start();
		GorillaComputer.instance.currentGameMode.AddCallback(new Action<string>(this.OnGameModeChanged), true);
	}

	// Token: 0x06002623 RID: 9763 RVA: 0x00049E99 File Offset: 0x00048099
	private void OnDestroy()
	{
		if (!ApplicationQuittingState.IsQuitting)
		{
			GorillaComputer.instance.currentGameMode.RemoveCallback(new Action<string>(this.OnGameModeChanged));
		}
	}

	// Token: 0x06002624 RID: 9764 RVA: 0x00049EBF File Offset: 0x000480BF
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

	// Token: 0x06002625 RID: 9765 RVA: 0x00049EF4 File Offset: 0x000480F4
	public void OnGameModeChanged(string newGameMode)
	{
		this.buttonRenderer.material = ((newGameMode.ToLower() == this.gameMode.ToLower()) ? this.pressedMaterial : this.unpressedMaterial);
	}

	// Token: 0x06002626 RID: 9766 RVA: 0x00049F27 File Offset: 0x00048127
	public void SetInfo(ModeSelectButtonInfoData info)
	{
		this.SetInfo(info.Mode, info.ModeTitle, info.NewMode, info.CountdownTo);
	}

	// Token: 0x06002627 RID: 9767 RVA: 0x00107FE8 File Offset: 0x001061E8
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

	// Token: 0x06002628 RID: 9768 RVA: 0x00049F47 File Offset: 0x00048147
	public void HideNewAndLimitedTimeInfo()
	{
		this.limitedCountdown.gameObject.SetActive(false);
		this.newModeSplash.SetActive(false);
	}

	// Token: 0x04002A3B RID: 10811
	[SerializeField]
	public string gameMode;

	// Token: 0x04002A3C RID: 10812
	[SerializeField]
	private PartyGameModeWarning warningScreen;

	// Token: 0x04002A3D RID: 10813
	[SerializeField]
	private TMP_Text gameModeTitle;

	// Token: 0x04002A3E RID: 10814
	[SerializeField]
	private GameObject newModeSplash;

	// Token: 0x04002A3F RID: 10815
	[SerializeField]
	private CountdownText limitedCountdown;
}
