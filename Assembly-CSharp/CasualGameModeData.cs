using System;
using Fusion;

// Token: 0x02000463 RID: 1123
[NetworkBehaviourWeaved(1)]
public class CasualGameModeData : FusionGameModeData
{
	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06001B86 RID: 7046 RVA: 0x0008731F File Offset: 0x0008551F
	// (set) Token: 0x06001B87 RID: 7047 RVA: 0x000023F4 File Offset: 0x000005F4
	public override object Data
	{
		get
		{
			return this.casualData;
		}
		set
		{
		}
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x06001B88 RID: 7048 RVA: 0x0008732C File Offset: 0x0008552C
	// (set) Token: 0x06001B89 RID: 7049 RVA: 0x00087356 File Offset: 0x00085556
	[Networked]
	[NetworkedWeaved(0, 1)]
	private unsafe CasualData casualData
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing CasualGameModeData.casualData. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(CasualData*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing CasualGameModeData.casualData. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(CasualData*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x06001B8B RID: 7051 RVA: 0x00087381 File Offset: 0x00085581
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.casualData = this._casualData;
	}

	// Token: 0x06001B8C RID: 7052 RVA: 0x00087399 File Offset: 0x00085599
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._casualData = this.casualData;
	}

	// Token: 0x04001E7B RID: 7803
	[WeaverGenerated]
	[DefaultForProperty("casualData", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private CasualData _casualData;
}
