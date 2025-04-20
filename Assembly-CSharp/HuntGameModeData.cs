using System;
using Fusion;

// Token: 0x02000472 RID: 1138
[NetworkBehaviourWeaved(23)]
public class HuntGameModeData : FusionGameModeData
{
	// Token: 0x1700030B RID: 779
	// (get) Token: 0x06001BE8 RID: 7144 RVA: 0x000431E6 File Offset: 0x000413E6
	// (set) Token: 0x06001BE9 RID: 7145 RVA: 0x000431F3 File Offset: 0x000413F3
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

	// Token: 0x1700030C RID: 780
	// (get) Token: 0x06001BEA RID: 7146 RVA: 0x00043201 File Offset: 0x00041401
	// (set) Token: 0x06001BEB RID: 7147 RVA: 0x0004322B File Offset: 0x0004142B
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

	// Token: 0x06001BED RID: 7149 RVA: 0x00043256 File Offset: 0x00041456
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.huntdata = this._huntdata;
	}

	// Token: 0x06001BEE RID: 7150 RVA: 0x0004326E File Offset: 0x0004146E
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._huntdata = this.huntdata;
	}

	// Token: 0x04001ED1 RID: 7889
	[WeaverGenerated]
	[DefaultForProperty("huntdata", 0, 23)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private HuntData _huntdata;
}
