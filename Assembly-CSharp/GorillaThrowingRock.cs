using System;
using Photon.Pun;

// Token: 0x0200068F RID: 1679
public class GorillaThrowingRock : GorillaThrowable, IPunInstantiateMagicCallback
{
	// Token: 0x060029C8 RID: 10696 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
	}

	// Token: 0x04002F37 RID: 12087
	public float bonkSpeedMin = 1f;

	// Token: 0x04002F38 RID: 12088
	public float bonkSpeedMax = 5f;

	// Token: 0x04002F39 RID: 12089
	public VRRig hitRig;
}
