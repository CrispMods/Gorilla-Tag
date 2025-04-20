using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x020004AD RID: 1197
public class MonkeBallGoalZone : MonoBehaviour
{
	// Token: 0x06001D26 RID: 7462 RVA: 0x000E0054 File Offset: 0x000DE254
	private void Update()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		if (MonkeBallGame.Instance.GetGameState() == MonkeBallGame.GameState.Playing)
		{
			for (int i = 0; i < this.playersInGoalZone.Count; i++)
			{
				MonkeBallPlayer monkeBallPlayer = this.playersInGoalZone[i];
				if (monkeBallPlayer.gamePlayer.teamId != this.teamId)
				{
					GameBallId gameBallId = monkeBallPlayer.gamePlayer.GetGameBallId();
					if (gameBallId.IsValid())
					{
						MonkeBallGame.Instance.RequestScore(monkeBallPlayer.gamePlayer.teamId);
						GameBallId gameBallId2 = monkeBallPlayer.gamePlayer.GetGameBallId();
						int otherTeam = MonkeBallGame.Instance.GetOtherTeam(monkeBallPlayer.gamePlayer.teamId);
						if (MonkeBallGame.Instance.resetBallPositionOnScore)
						{
							MonkeBallGame.Instance.RequestResetBall(gameBallId2, otherTeam);
						}
						MonkeBallGame.Instance.RequestRestrictBallToTeamOnScore(gameBallId2, otherTeam);
						monkeBallPlayer.gamePlayer.ClearGrabbedIfHeld(gameBallId);
					}
				}
			}
		}
	}

	// Token: 0x06001D27 RID: 7463 RVA: 0x000E0134 File Offset: 0x000DE334
	private void OnTriggerEnter(Collider other)
	{
		GamePlayer gamePlayer = GamePlayer.GetGamePlayer(other, true);
		if (gamePlayer != null && gamePlayer.teamId != this.teamId)
		{
			MonkeBallPlayer component = gamePlayer.GetComponent<MonkeBallPlayer>();
			if (component != null)
			{
				component.currGoalZone = this;
				this.playersInGoalZone.Add(component);
			}
		}
	}

	// Token: 0x06001D28 RID: 7464 RVA: 0x000E0184 File Offset: 0x000DE384
	private void OnTriggerExit(Collider other)
	{
		GamePlayer gamePlayer = GamePlayer.GetGamePlayer(other, true);
		if (gamePlayer != null && gamePlayer.teamId != this.teamId)
		{
			MonkeBallPlayer component = gamePlayer.GetComponent<MonkeBallPlayer>();
			if (component != null)
			{
				component.currGoalZone = null;
				this.playersInGoalZone.Remove(component);
			}
		}
	}

	// Token: 0x06001D29 RID: 7465 RVA: 0x00043F15 File Offset: 0x00042115
	public void CleanupPlayer(MonkeBallPlayer player)
	{
		this.playersInGoalZone.Remove(player);
	}

	// Token: 0x0400200D RID: 8205
	public int teamId;

	// Token: 0x0400200E RID: 8206
	public List<MonkeBallPlayer> playersInGoalZone;
}
