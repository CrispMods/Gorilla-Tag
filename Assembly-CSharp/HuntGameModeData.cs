using System;
using Fusion;

// Token: 0x02000466 RID: 1126
[NetworkBehaviourWeaved(23)]
public class HuntGameModeData : FusionGameModeData
{
	// Token: 0x17000304 RID: 772
	// (get) Token: 0x06001B94 RID: 7060 RVA: 0x000873FF File Offset: 0x000855FF
	// (set) Token: 0x06001B95 RID: 7061 RVA: 0x0008740C File Offset: 0x0008560C
	public override object Data
	{
		get
		{
			return this.huntdata;
		}
		set
		{
			this.huntdata = (HuntData)value;
		}
	}

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x06001B96 RID: 7062 RVA: 0x0008741A File Offset: 0x0008561A
	// (set) Token: 0x06001B97 RID: 7063 RVA: 0x00087444 File Offset: 0x00085644
	[Networked]
	[NetworkedWeaved(0, 23)]
	private unsafe HuntData huntdata
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing HuntGameModeData.huntdata. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(HuntData*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing HuntGameModeData.huntdata. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(HuntData*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x06001B99 RID: 7065 RVA: 0x0008746F File Offset: 0x0008566F
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.huntdata = this._huntdata;
	}

	// Token: 0x06001B9A RID: 7066 RVA: 0x00087487 File Offset: 0x00085687
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._huntdata = this.huntdata;
	}

	// Token: 0x04001E82 RID: 7810
	[WeaverGenerated]
	[DefaultForProperty("huntdata", 0, 23)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private HuntData _huntdata;
}
