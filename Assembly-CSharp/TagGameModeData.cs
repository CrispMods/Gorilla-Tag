using System;
using Fusion;

// Token: 0x02000474 RID: 1140
[NetworkBehaviourWeaved(12)]
public class TagGameModeData : FusionGameModeData
{
	// Token: 0x1700030F RID: 783
	// (get) Token: 0x06001BF2 RID: 7154 RVA: 0x000432AF File Offset: 0x000414AF
	// (set) Token: 0x06001BF3 RID: 7155 RVA: 0x000432BC File Offset: 0x000414BC
	public override object Data
	{
		get
		{
			return this.tagData;
		}
		set
		{
			this.tagData = (TagData)value;
		}
	}

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x06001BF4 RID: 7156 RVA: 0x000432CA File Offset: 0x000414CA
	// (set) Token: 0x06001BF5 RID: 7157 RVA: 0x000432F4 File Offset: 0x000414F4
	[Networked]
	[NetworkedWeaved(0, 12)]
	private unsafe TagData tagData
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing TagGameModeData.tagData. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(TagData*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing TagGameModeData.tagData. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(TagData*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x06001BF7 RID: 7159 RVA: 0x0004331F File Offset: 0x0004151F
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.tagData = this._tagData;
	}

	// Token: 0x06001BF8 RID: 7160 RVA: 0x00043337 File Offset: 0x00041537
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._tagData = this.tagData;
	}

	// Token: 0x04001ED5 RID: 7893
	[WeaverGenerated]
	[DefaultForProperty("tagData", 0, 12)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private TagData _tagData;
}
