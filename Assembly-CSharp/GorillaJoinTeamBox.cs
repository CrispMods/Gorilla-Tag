using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000577 RID: 1399
public class GorillaJoinTeamBox : GorillaTriggerBox
{
	// Token: 0x06002275 RID: 8821 RVA: 0x000AB4C9 File Offset: 0x000A96C9
	public override void OnBoxTriggered()
	{
		base.OnBoxTriggered();
		if (GameObject.FindGameObjectWithTag("GorillaGameManager").GetComponent<GorillaGameManager>() != null)
		{
			bool inRoom = PhotonNetwork.InRoom;
		}
	}

	// Token: 0x0400260D RID: 9741
	public bool joinRedTeam;
}
