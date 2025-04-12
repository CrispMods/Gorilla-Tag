using System;
using Fusion;

// Token: 0x02000463 RID: 1123
[NetworkBehaviourWeaved(1)]
public class CasualGameModeData : FusionGameModeData
{
	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06001B89 RID: 7049 RVA: 0x00041DE7 File Offset: 0x0003FFE7
	// (set) Token: 0x06001B8A RID: 7050 RVA: 0x0002F75F File Offset: 0x0002D95F
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
	// (get) Token: 0x06001B8B RID: 7051 RVA: 0x00041DF4 File Offset: 0x0003FFF4
	// (set) Token: 0x06001B8C RID: 7052 RVA: 0x00041E1E File Offset: 0x0004001E
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

	// Token: 0x06001B8E RID: 7054 RVA: 0x00041E49 File Offset: 0x00040049
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.casualData = this._casualData;
	}

	// Token: 0x06001B8F RID: 7055 RVA: 0x00041E61 File Offset: 0x00040061
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._casualData = this.casualData;
	}

	// Token: 0x04001E7C RID: 7804
	[WeaverGenerated]
	[DefaultForProperty("casualData", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private CasualData _casualData;
}
