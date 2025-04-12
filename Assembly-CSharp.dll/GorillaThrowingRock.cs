using System;
using Photon.Pun;

// Token: 0x02000690 RID: 1680
public class GorillaThrowingRock : GorillaThrowable, IPunInstantiateMagicCallback
{
	// Token: 0x060029D0 RID: 10704 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
	}

	// Token: 0x04002F3D RID: 12093
	public float bonkSpeedMin = 1f;

	// Token: 0x04002F3E RID: 12094
	public float bonkSpeedMax = 5f;

	// Token: 0x04002F3F RID: 12095
	public VRRig hitRig;
}
