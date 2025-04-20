using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

// Token: 0x020004B0 RID: 1200
public class MonkeBallScoreboard : MonoBehaviour
{
	// Token: 0x06001D33 RID: 7475 RVA: 0x00043F9E File Offset: 0x0004219E
	public void Setup(MonkeBallGame game)
	{
		this.game = game;
	}

	// Token: 0x06001D34 RID: 7476 RVA: 0x000E02C0 File Offset: 0x000DE4C0
	public void RefreshScore()
	{
		for (int i = 0; i < this.game.team.Count; i++)
		{
			this.teamDisplays[i].scoreLabel.text = this.game.team[i].score.ToString();
		}
	}

	// Token: 0x06001D35 RID: 7477 RVA: 0x00043FA7 File Offset: 0x000421A7
	public void RefreshTeamPlayers(int teamId, int numPlayers)
	{
		this.teamDisplays[teamId].playersLabel.text = string.Format("PLAYERS: {0}", Mathf.Clamp(numPlayers, 0, 99));
	}

	// Token: 0x06001D36 RID: 7478 RVA: 0x00043FD3 File Offset: 0x000421D3
	public void PlayScoreFx()
	{
		this.PlayFX(this.scoreSound, this.scoreSoundVolume);
	}

	// Token: 0x06001D37 RID: 7479 RVA: 0x00043FE7 File Offset: 0x000421E7
	public void PlayPlayerJoinFx()
	{
		this.PlayFX(this.playerJoinSound, 0.5f);
	}

	// Token: 0x06001D38 RID: 7480 RVA: 0x00043FFA File Offset: 0x000421FA
	public void PlayPlayerLeaveFx()
	{
		this.PlayFX(this.playerLeaveSound, 0.5f);
	}

	// Token: 0x06001D39 RID: 7481 RVA: 0x0004400D File Offset: 0x0004220D
	public void PlayGameStartFx()
	{
		this.PlayFX(this.gameStartSound, this.gameStartVolume);
	}

	// Token: 0x06001D3A RID: 7482 RVA: 0x00044021 File Offset: 0x00042221
	public void PlayGameEndFx()
	{
		this.PlayFX(this.gameEndSound, this.gameEndVolume);
	}

	// Token: 0x06001D3B RID: 7483 RVA: 0x00044035 File Offset: 0x00042235
	private void PlayFX(AudioClip clip, float volume)
	{
		if (this.audioSource != null)
		{
			this.audioSource.clip = clip;
			this.audioSource.volume = volume;
			this.audioSource.Play();
		}
	}

	// Token: 0x06001D3C RID: 7484 RVA: 0x000E0318 File Offset: 0x000DE518
	public void RefreshTime()
	{
		float a = (float)(this.game.gameEndTime - PhotonNetwork.Time);
		if (this.game.gameEndTime < 0.0)
		{
			a = 0f;
		}
		a = Mathf.Max(a, 0f);
		this.timeRemainingLabel.text = a.ToString("#00.00");
	}

	// Token: 0x0400201B RID: 8219
	private MonkeBallGame game;

	// Token: 0x0400201C RID: 8220
	public MonkeBallScoreboard.TeamDisplay[] teamDisplays;

	// Token: 0x0400201D RID: 8221
	public TextMeshPro timeRemainingLabel;

	// Token: 0x0400201E RID: 8222
	public AudioSource audioSource;

	// Token: 0x0400201F RID: 8223
	public AudioClip scoreSound;

	// Token: 0x04002020 RID: 8224
	public float scoreSoundVolume;

	// Token: 0x04002021 RID: 8225
	public AudioClip playerJoinSound;

	// Token: 0x04002022 RID: 8226
	public AudioClip playerLeaveSound;

	// Token: 0x04002023 RID: 8227
	public AudioClip gameStartSound;

	// Token: 0x04002024 RID: 8228
	public float gameStartVolume;

	// Token: 0x04002025 RID: 8229
	public AudioClip gameEndSound;

	// Token: 0x04002026 RID: 8230
	public float gameEndVolume;

	// Token: 0x020004B1 RID: 1201
	[Serializable]
	public class TeamDisplay
	{
		// Token: 0x04002027 RID: 8231
		public TextMeshPro nameLabel;

		// Token: 0x04002028 RID: 8232
		public TextMeshPro scoreLabel;

		// Token: 0x04002029 RID: 8233
		public TextMeshPro playersLabel;
	}
}
