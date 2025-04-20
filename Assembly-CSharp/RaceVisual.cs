using System;
using TMPro;
using UnityEngine;

// Token: 0x020001E4 RID: 484
public class RaceVisual : MonoBehaviour
{
	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06000B4A RID: 2890 RVA: 0x00037E91 File Offset: 0x00036091
	// (set) Token: 0x06000B4B RID: 2891 RVA: 0x00037E99 File Offset: 0x00036099
	public int raceId { get; private set; }

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06000B4C RID: 2892 RVA: 0x00037EA2 File Offset: 0x000360A2
	// (set) Token: 0x06000B4D RID: 2893 RVA: 0x00037EAA File Offset: 0x000360AA
	public bool TickRunning { get; set; }

	// Token: 0x06000B4E RID: 2894 RVA: 0x00037EB3 File Offset: 0x000360B3
	private void Awake()
	{
		this.checkpoints = base.GetComponent<RaceCheckpointManager>();
		this.finishLineText.text = "";
		this.SetScoreboardText("", "");
		this.SetRaceStartScoreboardText("", "");
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x00037EF1 File Offset: 0x000360F1
	private void OnEnable()
	{
		RacingManager.instance.RegisterVisual(this);
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x00037EFE File Offset: 0x000360FE
	public void Button_StartRace(int laps)
	{
		RacingManager.instance.Button_StartRace(this.raceId, laps);
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x00037F11 File Offset: 0x00036111
	public void ShowFinishLineText(string text)
	{
		this.finishLineText.text = text;
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x00037F1F File Offset: 0x0003611F
	public void UpdateCountdown(int timeRemaining)
	{
		if (timeRemaining != this.lastDisplayedCountdown)
		{
			this.countdownText.text = timeRemaining.ToString();
			this.finishLineText.text = "";
			this.lastDisplayedCountdown = timeRemaining;
		}
	}

	// Token: 0x06000B53 RID: 2899 RVA: 0x0009A7A8 File Offset: 0x000989A8
	public void SetScoreboardText(string mainText, string timesText)
	{
		foreach (RacingScoreboard racingScoreboard in this.raceScoreboards)
		{
			racingScoreboard.mainDisplay.text = mainText;
			racingScoreboard.timesDisplay.text = timesText;
		}
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x00037F53 File Offset: 0x00036153
	public void SetRaceStartScoreboardText(string mainText, string timesText)
	{
		this.raceStartScoreboard.mainDisplay.text = mainText;
		this.raceStartScoreboard.timesDisplay.text = timesText;
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x00037F77 File Offset: 0x00036177
	public void ActivateStartingWall(bool enable)
	{
		this.startingWall.SetActive(enable);
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x00037F85 File Offset: 0x00036185
	public bool IsPlayerNearCheckpoint(VRRig player, int checkpoint)
	{
		return this.checkpoints.IsPlayerNearCheckpoint(player, checkpoint);
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x00037F94 File Offset: 0x00036194
	public void OnCountdownStart(int laps, float goAfterInterval)
	{
		this.raceConsoleVisual.ShowRaceInProgress(laps);
		this.countdownSoundPlayer.Play();
		this.countdownSoundPlayer.time = this.countdownSoundGoTime - goAfterInterval;
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x00037FC0 File Offset: 0x000361C0
	public void OnRaceStart()
	{
		this.finishLineText.text = "GO!";
		this.checkpoints.OnRaceStart();
		this.lastDisplayedCountdown = 0;
		this.startingWall.SetActive(false);
		this.isRaceEndSoundEnabled = false;
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x00037FF7 File Offset: 0x000361F7
	public void OnRaceEnded()
	{
		this.finishLineText.text = "";
		this.lastDisplayedCountdown = 0;
		this.checkpoints.OnRaceEnd();
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x0003801B File Offset: 0x0003621B
	public void OnRaceReset()
	{
		this.raceConsoleVisual.ShowCanStartRace();
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x00038028 File Offset: 0x00036228
	public void EnableRaceEndSound()
	{
		this.isRaceEndSoundEnabled = true;
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x00038031 File Offset: 0x00036231
	public void OnCheckpointPassed(int index, SoundBankPlayer checkpointSound)
	{
		if (index == 0 && this.isRaceEndSoundEnabled)
		{
			this.countdownSoundPlayer.PlayOneShot(this.raceEndSound);
		}
		else
		{
			checkpointSound.Play();
		}
		RacingManager.instance.OnCheckpointPassed(this.raceId, index);
	}

	// Token: 0x04000DAF RID: 3503
	[SerializeField]
	private TextMeshPro finishLineText;

	// Token: 0x04000DB0 RID: 3504
	[SerializeField]
	private TextMeshPro countdownText;

	// Token: 0x04000DB1 RID: 3505
	[SerializeField]
	private RacingScoreboard[] raceScoreboards;

	// Token: 0x04000DB2 RID: 3506
	[SerializeField]
	private RacingScoreboard raceStartScoreboard;

	// Token: 0x04000DB3 RID: 3507
	[SerializeField]
	private RaceConsoleVisual raceConsoleVisual;

	// Token: 0x04000DB4 RID: 3508
	private float nextVisualRefreshTimestamp;

	// Token: 0x04000DB5 RID: 3509
	private RaceCheckpointManager checkpoints;

	// Token: 0x04000DB6 RID: 3510
	[SerializeField]
	private AudioClip raceEndSound;

	// Token: 0x04000DB7 RID: 3511
	[SerializeField]
	private float countdownSoundGoTime;

	// Token: 0x04000DB8 RID: 3512
	[SerializeField]
	private AudioSource countdownSoundPlayer;

	// Token: 0x04000DB9 RID: 3513
	[SerializeField]
	private GameObject startingWall;

	// Token: 0x04000DBA RID: 3514
	private int lastDisplayedCountdown;

	// Token: 0x04000DBB RID: 3515
	private bool isRaceEndSoundEnabled;
}
