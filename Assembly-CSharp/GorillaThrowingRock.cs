using System;
using Photon.Pun;

// Token: 0x02000659 RID: 1625
public class GorillaThrowingRock : GorillaThrowable, IPunInstantiateMagicCallback
{
	// Token: 0x06002847 RID: 10311 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
	}

	// Token: 0x04002DA7 RID: 11687
	public float bonkSpeedMin = 1f;

	// Token: 0x04002DA8 RID: 11688
	public float bonkSpeedMax = 5f;

	// Token: 0x04002DA9 RID: 11689
	public VRRig hitRig;
}
