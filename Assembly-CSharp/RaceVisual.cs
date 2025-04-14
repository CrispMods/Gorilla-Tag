using System;
using TMPro;
using UnityEngine;

// Token: 0x020001D9 RID: 473
public class RaceVisual : MonoBehaviour
{
	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000AFE RID: 2814 RVA: 0x0003B467 File Offset: 0x00039667
	// (set) Token: 0x06000AFF RID: 2815 RVA: 0x0003B46F File Offset: 0x0003966F
	public int raceId { get; private set; }

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06000B00 RID: 2816 RVA: 0x0003B478 File Offset: 0x00039678
	// (set) Token: 0x06000B01 RID: 2817 RVA: 0x0003B480 File Offset: 0x00039680
	public bool TickRunning { get; set; }

	// Token: 0x06000B02 RID: 2818 RVA: 0x0003B489 File Offset: 0x00039689
	private void Awake()
	{
		this.checkpoints = base.GetComponent<RaceCheckpointManager>();
		this.finishLineText.text = "";
		this.SetScoreboardText("", "");
		this.SetRaceStartScoreboardText("", "");
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x0003B4C7 File Offset: 0x000396C7
	private void OnEnable()
	{
		RacingManager.instance.RegisterVisual(this);
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x0003B4D4 File Offset: 0x000396D4
	public void Button_StartRace(int laps)
	{
		RacingManager.instance.Button_StartRace(this.raceId, laps);
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x0003B4E7 File Offset: 0x000396E7
	public void ShowFinishLineText(string text)
	{
		this.finishLineText.text = text;
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x0003B4F5 File Offset: 0x000396F5
	public void UpdateCountdown(int timeRemaining)
	{
		if (timeRemaining != this.lastDisplayedCountdown)
		{
			this.countdownText.text = timeRemaining.ToString();
			this.finishLineText.text = "";
			this.lastDisplayedCountdown = timeRemaining;
		}
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x0003B52C File Offset: 0x0003972C
	public void SetScoreboardText(string mainText, string timesText)
	{
		foreach (RacingScoreboard racingScoreboard in this.raceScoreboards)
		{
			racingScoreboard.mainDisplay.text = mainText;
			racingScoreboard.timesDisplay.text = timesText;
		}
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x0003B568 File Offset: 0x00039768
	public void SetRaceStartScoreboardText(string mainText, string timesText)
	{
		this.raceStartScoreboard.mainDisplay.text = mainText;
		this.raceStartScoreboard.timesDisplay.text = timesText;
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x0003B58C File Offset: 0x0003978C
	public void ActivateStartingWall(bool enable)
	{
		this.startingWall.SetActive(enable);
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x0003B59A File Offset: 0x0003979A
	public bool IsPlayerNearCheckpoint(VRRig player, int checkpoint)
	{
		return this.checkpoints.IsPlayerNearCheckpoint(player, checkpoint);
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x0003B5A9 File Offset: 0x000397A9
	public void OnCountdownStart(int laps, float goAfterInterval)
	{
		this.raceConsoleVisual.ShowRaceInProgress(laps);
		this.countdownSoundPlayer.Play();
		this.countdownSoundPlayer.time = this.countdownSoundGoTime - goAfterInterval;
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x0003B5D5 File Offset: 0x000397D5
	public void OnRaceStart()
	{
		this.finishLineText.text = "GO!";
		this.checkpoints.OnRaceStart();
		this.lastDisplayedCountdown = 0;
		this.startingWall.SetActive(false);
		this.isRaceEndSoundEnabled = false;
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x0003B60C File Offset: 0x0003980C
	public void OnRaceEnded()
	{
		this.finishLineText.text = "";
		this.lastDisplayedCountdown = 0;
		this.checkpoints.OnRaceEnd();
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x0003B630 File Offset: 0x00039830
	public void OnRaceReset()
	{
		this.raceConsoleVisual.ShowCanStartRace();
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0003B63D File Offset: 0x0003983D
	public void EnableRaceEndSound()
	{
		this.isRaceEndSoundEnabled = true;
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x0003B646 File Offset: 0x00039846
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

	// Token: 0x04000D69 RID: 3433
	[SerializeField]
	private TextMeshPro finishLineText;

	// Token: 0x04000D6A RID: 3434
	[SerializeField]
	private TextMeshPro countdownText;

	// Token: 0x04000D6B RID: 3435
	[SerializeField]
	private RacingScoreboard[] raceScoreboards;

	// Token: 0x04000D6C RID: 3436
	[SerializeField]
	private RacingScoreboard raceStartScoreboard;

	// Token: 0x04000D6D RID: 3437
	[SerializeField]
	private RaceConsoleVisual raceConsoleVisual;

	// Token: 0x04000D6E RID: 3438
	private float nextVisualRefreshTimestamp;

	// Token: 0x04000D6F RID: 3439
	private RaceCheckpointManager checkpoints;

	// Token: 0x04000D70 RID: 3440
	[SerializeField]
	private AudioClip raceEndSound;

	// Token: 0x04000D71 RID: 3441
	[SerializeField]
	private float countdownSoundGoTime;

	// Token: 0x04000D72 RID: 3442
	[SerializeField]
	private AudioSource countdownSoundPlayer;

	// Token: 0x04000D73 RID: 3443
	[SerializeField]
	private GameObject startingWall;

	// Token: 0x04000D74 RID: 3444
	private int lastDisplayedCountdown;

	// Token: 0x04000D75 RID: 3445
	private bool isRaceEndSoundEnabled;
}
