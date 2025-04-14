using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200047B RID: 1147
public class GorillaTriggerBoxGameFlag : GorillaTriggerBox
{
	// Token: 0x06001BDD RID: 7133 RVA: 0x00087D04 File Offset: 0x00085F04
	public override void OnBoxTriggered()
	{
		base.OnBoxTriggered();
		PhotonView.Get(Object.FindObjectOfType<GorillaGameManager>()).RPC(this.functionName, RpcTarget.MasterClient, null);
	}

	// Token: 0x04001EEA RID: 7914
	public string functionName;
}
