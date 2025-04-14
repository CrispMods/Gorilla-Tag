using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000578 RID: 1400
public class GorillaJoinTeamBox : GorillaTriggerBox
{
	// Token: 0x0600227D RID: 8829 RVA: 0x000AB949 File Offset: 0x000A9B49
	public override void OnBoxTriggered()
	{
		base.OnBoxTriggered();
		if (GameObject.FindGameObjectWithTag("GorillaGameManager").GetComponent<GorillaGameManager>() != null)
		{
			bool inRoom = PhotonNetwork.InRoom;
		}
	}

	// Token: 0x04002613 RID: 9747
	public bool joinRedTeam;
}
