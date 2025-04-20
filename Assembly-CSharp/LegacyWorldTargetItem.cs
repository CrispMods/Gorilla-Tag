using System;
using Photon.Realtime;

// Token: 0x0200038B RID: 907
public class LegacyWorldTargetItem
{
	// Token: 0x0600154F RID: 5455 RVA: 0x0003E656 File Offset: 0x0003C856
	public bool IsValid()
	{
		return this.itemIdx != -1 && this.owner != null;
	}

	// Token: 0x06001550 RID: 5456 RVA: 0x0003E66C File Offset: 0x0003C86C
	public void Invalidate()
	{
		this.itemIdx = -1;
		this.owner = null;
	}

	// Token: 0x0400178C RID: 6028
	public Player owner;

	// Token: 0x0400178D RID: 6029
	public int itemIdx;
}
