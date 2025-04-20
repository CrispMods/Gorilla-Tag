using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000487 RID: 1159
public class GorillaTriggerBoxGameFlag : GorillaTriggerBox
{
	// Token: 0x06001C31 RID: 7217 RVA: 0x00043631 File Offset: 0x00041831
	public override void OnBoxTriggered()
	{
		base.OnBoxTriggered();
		PhotonView.Get(UnityEngine.Object.FindObjectOfType<GorillaGameManager>()).RPC(this.functionName, RpcTarget.MasterClient, null);
	}

	// Token: 0x04001F39 RID: 7993
	public string functionName;
}
