using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

// Token: 0x020004A4 RID: 1188
public class MonkeBallScoreboard : MonoBehaviour
{
	// Token: 0x06001CE2 RID: 7394 RVA: 0x0008D146 File Offset: 0x0008B346
	public void Setup(MonkeBallGame game)
	{
		this.game = game;
	}

	// Token: 0x06001CE3 RID: 7395 RVA: 0x0008D150 File Offset: 0x0008B350
	public void RefreshScore()
	{
		for (int i = 0; i < this.game.team.Count; i++)
		{
			this.teamDisplays[i].scoreLabel.text = this.game.team[i].score.ToString();
		}
	}

	// Token: 0x06001CE4 RID: 7396 RVA: 0x0008D1A5 File Offset: 0x0008B3A5
	public void RefreshTeamPlayers(int teamId, int numPlayers)
	{
		this.teamDisplays[teamId].playersLabel.text = string.Format("PLAYERS: {0}", Mathf.Clamp(numPlayers, 0, 99));
	}

	// Token: 0x06001CE5 RID: 7397 RVA: 0x0008D1D1 File Offset: 0x0008B3D1
	public void PlayScoreFx()
	{
		this.PlayFX(this.scoreSound, this.scoreSoundVolume);
	}

	// Token: 0x06001CE6 RID: 7398 RVA: 0x0008D1E5 File Offset: 0x0008B3E5
	public void PlayPlayerJoinFx()
	{
		this.PlayFX(this.playerJoinSound, 0.5f);
	}

	// Token: 0x06001CE7 RID: 7399 RVA: 0x0008D1F8 File Offset: 0x0008B3F8
	public void PlayPlayerLeaveFx()
	{
		this.PlayFX(this.playerLeaveSound, 0.5f);
	}

	// Token: 0x06001CE8 RID: 7400 RVA: 0x0008D20B File Offset: 0x0008B40B
	public void PlayGameStartFx()
	{
		this.PlayFX(this.gameStartSound, this.gameStartVolume);
	}

	// Token: 0x06001CE9 RID: 7401 RVA: 0x0008D21F File Offset: 0x0008B41F
	public void PlayGameEndFx()
	{
		this.PlayFX(this.gameEndSound, this.gameEndVolume);
	}

	// Token: 0x06001CEA RID: 7402 RVA: 0x0008D233 File Offset: 0x0008B433
	private void PlayFX(AudioClip clip, float volume)
	{
		if (this.audioSource != null)
		{
			this.audioSource.clip = clip;
			this.audioSource.volume = volume;
			this.audioSource.Play();
		}
	}

	// Token: 0x06001CEB RID: 7403 RVA: 0x0008D268 File Offset: 0x0008B468
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

	// Token: 0x04001FCD RID: 8141
	private MonkeBallGame game;

	// Token: 0x04001FCE RID: 8142
	public MonkeBallScoreboard.TeamDisplay[] teamDisplays;

	// Token: 0x04001FCF RID: 8143
	public TextMeshPro timeRemainingLabel;

	// Token: 0x04001FD0 RID: 8144
	public AudioSource audioSource;

	// Token: 0x04001FD1 RID: 8145
	public AudioClip scoreSound;

	// Token: 0x04001FD2 RID: 8146
	public float scoreSoundVolume;

	// Token: 0x04001FD3 RID: 8147
	public AudioClip playerJoinSound;

	// Token: 0x04001FD4 RID: 8148
	public AudioClip playerLeaveSound;

	// Token: 0x04001FD5 RID: 8149
	public AudioClip gameStartSound;

	// Token: 0x04001FD6 RID: 8150
	public float gameStartVolume;

	// Token: 0x04001FD7 RID: 8151
	public AudioClip gameEndSound;

	// Token: 0x04001FD8 RID: 8152
	public float gameEndVolume;

	// Token: 0x020004A5 RID: 1189
	[Serializable]
	public class TeamDisplay
	{
		// Token: 0x04001FD9 RID: 8153
		public TextMeshPro nameLabel;

		// Token: 0x04001FDA RID: 8154
		public TextMeshPro scoreLabel;

		// Token: 0x04001FDB RID: 8155
		public TextMeshPro playersLabel;
	}
}
