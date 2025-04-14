using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x020004A1 RID: 1185
public class MonkeBallGoalZone : MonoBehaviour
{
	// Token: 0x06001CD2 RID: 7378 RVA: 0x0008CACC File Offset: 0x0008ACCC
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

	// Token: 0x06001CD3 RID: 7379 RVA: 0x0008CBAC File Offset: 0x0008ADAC
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

	// Token: 0x06001CD4 RID: 7380 RVA: 0x0008CBFC File Offset: 0x0008ADFC
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

	// Token: 0x06001CD5 RID: 7381 RVA: 0x0008CC4C File Offset: 0x0008AE4C
	public void CleanupPlayer(MonkeBallPlayer player)
	{
		this.playersInGoalZone.Remove(player);
	}

	// Token: 0x04001FBE RID: 8126
	public int teamId;

	// Token: 0x04001FBF RID: 8127
	public List<MonkeBallPlayer> playersInGoalZone;
}
