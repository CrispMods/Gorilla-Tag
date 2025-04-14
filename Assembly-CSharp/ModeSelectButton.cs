using System;
using GameObjectScheduling;
using GorillaNetworking;
using TMPro;
using UnityEngine;

// Token: 0x020005ED RID: 1517
public class ModeSelectButton : GorillaPressableButton
{
	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x060025BE RID: 9662 RVA: 0x000BA5EA File Offset: 0x000B87EA
	// (set) Token: 0x060025BF RID: 9663 RVA: 0x000BA5F2 File Offset: 0x000B87F2
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

	// Token: 0x060025C0 RID: 9664 RVA: 0x000BA5FB File Offset: 0x000B87FB
	public override void Start()
	{
		base.Start();
		GorillaComputer.instance.currentGameMode.AddCallback(new Action<string>(this.OnGameModeChanged), true);
	}

	// Token: 0x060025C1 RID: 9665 RVA: 0x000BA621 File Offset: 0x000B8821
	private void OnDestroy()
	{
		if (!ApplicationQuittingState.IsQuitting)
		{
			GorillaComputer.instance.currentGameMode.RemoveCallback(new Action<string>(this.OnGameModeChanged));
		}
	}

	// Token: 0x060025C2 RID: 9666 RVA: 0x000BA647 File Offset: 0x000B8847
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

	// Token: 0x060025C3 RID: 9667 RVA: 0x000BA67C File Offset: 0x000B887C
	public void OnGameModeChanged(string newGameMode)
	{
		this.buttonRenderer.material = ((newGameMode.ToLower() == this.gameMode.ToLower()) ? this.pressedMaterial : this.unpressedMaterial);
	}

	// Token: 0x060025C4 RID: 9668 RVA: 0x000BA6AF File Offset: 0x000B88AF
	public void SetInfo(ModeSelectButtonInfoData info)
	{
		this.SetInfo(info.Mode, info.ModeTitle, info.NewMode, info.CountdownTo);
	}

	// Token: 0x060025C5 RID: 9669 RVA: 0x000BA6D0 File Offset: 0x000B88D0
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

	// Token: 0x060025C6 RID: 9670 RVA: 0x000BA736 File Offset: 0x000B8936
	public void HideNewAndLimitedTimeInfo()
	{
		this.limitedCountdown.gameObject.SetActive(false);
		this.newModeSplash.SetActive(false);
	}

	// Token: 0x040029DC RID: 10716
	[SerializeField]
	public string gameMode;

	// Token: 0x040029DD RID: 10717
	[SerializeField]
	private PartyGameModeWarning warningScreen;

	// Token: 0x040029DE RID: 10718
	[SerializeField]
	private TMP_Text gameModeTitle;

	// Token: 0x040029DF RID: 10719
	[SerializeField]
	private GameObject newModeSplash;

	// Token: 0x040029E0 RID: 10720
	[SerializeField]
	private CountdownText limitedCountdown;
}
