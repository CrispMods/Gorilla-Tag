using System;
using UnityEngine;

// Token: 0x020004B4 RID: 1204
public class MonkeBallTeamZoneSelector : MonoBehaviour
{
	// Token: 0x06001D48 RID: 7496 RVA: 0x000E0464 File Offset: 0x000DE664
	private void OnTriggerEnter(Collider other)
	{
		GamePlayer gamePlayer = GamePlayer.GetGamePlayer(other, true);
		if (gamePlayer != null && gamePlayer.IsLocalPlayer() && gamePlayer.teamId != this.teamId)
		{
			MonkeBallGame.Instance.RequestSetTeam(this.teamId);
		}
	}

	// Token: 0x04002032 RID: 8242
	public int teamId;
}
