using System;
using TMPro;
using UnityEngine;

// Token: 0x020001D9 RID: 473
public class RaceVisual : MonoBehaviour
{
	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000B00 RID: 2816 RVA: 0x00036BD1 File Offset: 0x00034DD1
	// (set) Token: 0x06000B01 RID: 2817 RVA: 0x00036BD9 File Offset: 0x00034DD9
	public int raceId { get; private set; }

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06000B02 RID: 2818 RVA: 0x00036BE2 File Offset: 0x00034DE2
	// (set) Token: 0x06000B03 RID: 2819 RVA: 0x00036BEA File Offset: 0x00034DEA
	public bool TickRunning { get; set; }

	// Token: 0x06000B04 RID: 2820 RVA: 0x00036BF3 File Offset: 0x00034DF3
	private void Awake()
	{
		this.checkpoints = base.GetComponent<RaceCheckpointManager>();
		this.finishLineText.text = "";
		this.SetScoreboardText("", "");
		this.SetRaceStartScoreboardText("", "");
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x00036C31 File Offset: 0x00034E31
	private void OnEnable()
	{
		RacingManager.instance.RegisterVisual(this);
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x00036C3E File Offset: 0x00034E3E
	public void Button_StartRace(int laps)
	{
		RacingManager.instance.Button_StartRace(this.raceId, laps);
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x00036C51 File Offset: 0x00034E51
	public void ShowFinishLineText(string text)
	{
		this.finishLineText.text = text;
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x00036C5F File Offset: 0x00034E5F
	public void UpdateCountdown(int timeRemaining)
	{
		if (timeRemaining != this.lastDisplayedCountdown)
		{
			this.countdownText.text = timeRemaining.ToString();
			this.finishLineText.text = "";
			this.lastDisplayedCountdown = timeRemaining;
		}
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x00097EB4 File Offset: 0x000960B4
	public void SetScoreboardText(string mainText, string timesText)
	{
		foreach (RacingScoreboard racingScoreboard in this.raceScoreboards)
		{
			racingScoreboard.mainDisplay.text = mainText;
			racingScoreboard.timesDisplay.text = timesText;
		}
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x00036C93 File Offset: 0x00034E93
	public void SetRaceStartScoreboardText(string mainText, string timesText)
	{
		this.raceStartScoreboard.mainDisplay.text = mainText;
		this.raceStartScoreboard.timesDisplay.text = timesText;
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x00036CB7 File Offset: 0x00034EB7
	public void ActivateStartingWall(bool enable)
	{
		this.startingWall.SetActive(enable);
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x00036CC5 File Offset: 0x00034EC5
	public bool IsPlayerNearCheckpoint(VRRig player, int checkpoint)
	{
		return this.checkpoints.IsPlayerNearCheckpoint(player, checkpoint);
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x00036CD4 File Offset: 0x00034ED4
	public void OnCountdownStart(int laps, float goAfterInterval)
	{
		this.raceConsoleVisual.ShowRaceInProgress(laps);
		this.countdownSoundPlayer.Play();
		this.countdownSoundPlayer.time = this.countdownSoundGoTime - goAfterInterval;
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x00036D00 File Offset: 0x00034F00
	public void OnRaceStart()
	{
		this.finishLineText.text = "GO!";
		this.checkpoints.OnRaceStart();
		this.lastDisplayedCountdown = 0;
		this.startingWall.SetActive(false);
		this.isRaceEndSoundEnabled = false;
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x00036D37 File Offset: 0x00034F37
	public void OnRaceEnded()
	{
		this.finishLineText.text = "";
		this.lastDisplayedCountdown = 0;
		this.checkpoints.OnRaceEnd();
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x00036D5B File Offset: 0x00034F5B
	public void OnRaceReset()
	{
		this.raceConsoleVisual.ShowCanStartRace();
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x00036D68 File Offset: 0x00034F68
	public void EnableRaceEndSound()
	{
		this.isRaceEndSoundEnabled = true;
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x00036D71 File Offset: 0x00034F71
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

	// Token: 0x04000D6A RID: 3434
	[SerializeField]
	private TextMeshPro finishLineText;

	// Token: 0x04000D6B RID: 3435
	[SerializeField]
	private TextMeshPro countdownText;

	// Token: 0x04000D6C RID: 3436
	[SerializeField]
	private RacingScoreboard[] raceScoreboards;

	// Token: 0x04000D6D RID: 3437
	[SerializeField]
	private RacingScoreboard raceStartScoreboard;

	// Token: 0x04000D6E RID: 3438
	[SerializeField]
	private RaceConsoleVisual raceConsoleVisual;

	// Token: 0x04000D6F RID: 3439
	private float nextVisualRefreshTimestamp;

	// Token: 0x04000D70 RID: 3440
	private RaceCheckpointManager checkpoints;

	// Token: 0x04000D71 RID: 3441
	[SerializeField]
	private AudioClip raceEndSound;

	// Token: 0x04000D72 RID: 3442
	[SerializeField]
	private float countdownSoundGoTime;

	// Token: 0x04000D73 RID: 3443
	[SerializeField]
	private AudioSource countdownSoundPlayer;

	// Token: 0x04000D74 RID: 3444
	[SerializeField]
	private GameObject startingWall;

	// Token: 0x04000D75 RID: 3445
	private int lastDisplayedCountdown;

	// Token: 0x04000D76 RID: 3446
	private bool isRaceEndSoundEnabled;
}
