using System;
using Fusion;

// Token: 0x02000470 RID: 1136
[NetworkBehaviourWeaved(0)]
public abstract class FusionGameModeData : NetworkBehaviour
{
	// Token: 0x17000308 RID: 776
	// (get) Token: 0x06001BE1 RID: 7137
	// (set) Token: 0x06001BE2 RID: 7138
	public abstract object Data { get; set; }

	// Token: 0x06001BE4 RID: 7140 RVA: 0x00030607 File Offset: 0x0002E807
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
	}

	// Token: 0x06001BE5 RID: 7141 RVA: 0x00030607 File Offset: 0x0002E807
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
	}

	// Token: 0x04001ECB RID: 7883
	protected INetworkStruct data;
}
