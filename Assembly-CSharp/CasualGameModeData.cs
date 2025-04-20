using System;
using Fusion;

// Token: 0x0200046F RID: 1135
[NetworkBehaviourWeaved(1)]
public class CasualGameModeData : FusionGameModeData
{
	// Token: 0x17000306 RID: 774
	// (get) Token: 0x06001BDA RID: 7130 RVA: 0x00043120 File Offset: 0x00041320
	// (set) Token: 0x06001BDB RID: 7131 RVA: 0x00030607 File Offset: 0x0002E807
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

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x06001BDC RID: 7132 RVA: 0x0004312D File Offset: 0x0004132D
	// (set) Token: 0x06001BDD RID: 7133 RVA: 0x00043157 File Offset: 0x00041357
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

	// Token: 0x06001BDF RID: 7135 RVA: 0x00043182 File Offset: 0x00041382
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.casualData = this._casualData;
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x0004319A File Offset: 0x0004139A
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._casualData = this.casualData;
	}

	// Token: 0x04001ECA RID: 7882
	[WeaverGenerated]
	[DefaultForProperty("casualData", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private CasualData _casualData;
}
