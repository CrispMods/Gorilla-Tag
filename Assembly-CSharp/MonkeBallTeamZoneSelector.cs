using System;
using UnityEngine;

// Token: 0x020004A8 RID: 1192
public class MonkeBallTeamZoneSelector : MonoBehaviour
{
	// Token: 0x06001CF4 RID: 7412 RVA: 0x0008D0A8 File Offset: 0x0008B2A8
	private void OnTriggerEnter(Collider other)
	{
		GamePlayer gamePlayer = GamePlayer.GetGamePlayer(other, true);
		if (gamePlayer != null && gamePlayer.IsLocalPlayer() && gamePlayer.teamId != this.teamId)
		{
			MonkeBallGame.Instance.RequestSetTeam(this.teamId);
		}
	}

	// Token: 0x04001FE3 RID: 8163
	public int teamId;
}
