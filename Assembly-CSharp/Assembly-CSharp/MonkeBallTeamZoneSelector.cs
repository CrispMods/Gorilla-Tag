using System;
using UnityEngine;

// Token: 0x020004A8 RID: 1192
public class MonkeBallTeamZoneSelector : MonoBehaviour
{
	// Token: 0x06001CF7 RID: 7415 RVA: 0x0008D42C File Offset: 0x0008B62C
	private void OnTriggerEnter(Collider other)
	{
		GamePlayer gamePlayer = GamePlayer.GetGamePlayer(other, true);
		if (gamePlayer != null && gamePlayer.IsLocalPlayer() && gamePlayer.teamId != this.teamId)
		{
			MonkeBallGame.Instance.RequestSetTeam(this.teamId);
		}
	}

	// Token: 0x04001FE4 RID: 8164
	public int teamId;
}
