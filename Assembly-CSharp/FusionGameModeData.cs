using System;
using Fusion;

// Token: 0x02000464 RID: 1124
[NetworkBehaviourWeaved(0)]
public abstract class FusionGameModeData : NetworkBehaviour
{
	// Token: 0x17000301 RID: 769
	// (get) Token: 0x06001B8D RID: 7053
	// (set) Token: 0x06001B8E RID: 7054
	public abstract object Data { get; set; }

	// Token: 0x06001B90 RID: 7056 RVA: 0x000023F4 File Offset: 0x000005F4
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
	}

	// Token: 0x06001B91 RID: 7057 RVA: 0x000023F4 File Offset: 0x000005F4
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
	}

	// Token: 0x04001E7C RID: 7804
	protected INetworkStruct data;
}
