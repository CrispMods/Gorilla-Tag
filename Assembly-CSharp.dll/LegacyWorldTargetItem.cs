using System;
using Photon.Realtime;

// Token: 0x02000380 RID: 896
public class LegacyWorldTargetItem
{
	// Token: 0x06001506 RID: 5382 RVA: 0x0003D396 File Offset: 0x0003B596
	public bool IsValid()
	{
		return this.itemIdx != -1 && this.owner != null;
	}

	// Token: 0x06001507 RID: 5383 RVA: 0x0003D3AC File Offset: 0x0003B5AC
	public void Invalidate()
	{
		this.itemIdx = -1;
		this.owner = null;
	}

	// Token: 0x04001745 RID: 5957
	public Player owner;

	// Token: 0x04001746 RID: 5958
	public int itemIdx;
}
