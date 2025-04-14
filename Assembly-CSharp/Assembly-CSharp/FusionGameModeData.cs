using System;
using Fusion;

// Token: 0x02000464 RID: 1124
[NetworkBehaviourWeaved(0)]
public abstract class FusionGameModeData : NetworkBehaviour
{
	// Token: 0x17000301 RID: 769
	// (get) Token: 0x06001B90 RID: 7056
	// (set) Token: 0x06001B91 RID: 7057
	public abstract object Data { get; set; }

	// Token: 0x06001B93 RID: 7059 RVA: 0x000023F4 File Offset: 0x000005F4
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
	}

	// Token: 0x06001B94 RID: 7060 RVA: 0x000023F4 File Offset: 0x000005F4
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
	}

	// Token: 0x04001E7D RID: 7805
	protected INetworkStruct data;
}
