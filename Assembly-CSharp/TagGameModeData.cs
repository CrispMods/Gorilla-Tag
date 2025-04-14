using System;
using Fusion;

// Token: 0x02000468 RID: 1128
[NetworkBehaviourWeaved(12)]
public class TagGameModeData : FusionGameModeData
{
	// Token: 0x17000308 RID: 776
	// (get) Token: 0x06001B9E RID: 7070 RVA: 0x000874D4 File Offset: 0x000856D4
	// (set) Token: 0x06001B9F RID: 7071 RVA: 0x000874E1 File Offset: 0x000856E1
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

	// Token: 0x17000309 RID: 777
	// (get) Token: 0x06001BA0 RID: 7072 RVA: 0x000874EF File Offset: 0x000856EF
	// (set) Token: 0x06001BA1 RID: 7073 RVA: 0x00087519 File Offset: 0x00085719
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

	// Token: 0x06001BA3 RID: 7075 RVA: 0x00087544 File Offset: 0x00085744
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.tagData = this._tagData;
	}

	// Token: 0x06001BA4 RID: 7076 RVA: 0x0008755C File Offset: 0x0008575C
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._tagData = this.tagData;
	}

	// Token: 0x04001E86 RID: 7814
	[WeaverGenerated]
	[DefaultForProperty("tagData", 0, 12)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private TagData _tagData;
}
