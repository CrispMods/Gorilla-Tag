using System;
using Photon.Realtime;

// Token: 0x02000380 RID: 896
public class LegacyWorldTargetItem
{
	// Token: 0x06001503 RID: 5379 RVA: 0x00066C98 File Offset: 0x00064E98
	public bool IsValid()
	{
		return this.itemIdx != -1 && this.owner != null;
	}

	// Token: 0x06001504 RID: 5380 RVA: 0x00066CAE File Offset: 0x00064EAE
	public void Invalidate()
	{
		this.itemIdx = -1;
		this.owner = null;
	}

	// Token: 0x04001744 RID: 5956
	public Player owner;

	// Token: 0x04001745 RID: 5957
	public int itemIdx;
}
