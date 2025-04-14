using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200047B RID: 1147
public class GorillaTriggerBoxGameFlag : GorillaTriggerBox
{
	// Token: 0x06001BE0 RID: 7136 RVA: 0x00088088 File Offset: 0x00086288
	public override void OnBoxTriggered()
	{
		base.OnBoxTriggered();
		PhotonView.Get(Object.FindObjectOfType<GorillaGameManager>()).RPC(this.functionName, RpcTarget.MasterClient, null);
	}

	// Token: 0x04001EEB RID: 7915
	public string functionName;
}
