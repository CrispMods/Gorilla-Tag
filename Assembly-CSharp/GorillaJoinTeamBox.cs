using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000585 RID: 1413
public class GorillaJoinTeamBox : GorillaTriggerBox
{
	// Token: 0x060022D3 RID: 8915 RVA: 0x00047934 File Offset: 0x00045B34
	public override void OnBoxTriggered()
	{
		base.OnBoxTriggered();
		if (GameObject.FindGameObjectWithTag("GorillaGameManager").GetComponent<GorillaGameManager>() != null)
		{
			bool inRoom = PhotonNetwork.InRoom;
		}
	}

	// Token: 0x04002665 RID: 9829
	public bool joinRedTeam;
}
