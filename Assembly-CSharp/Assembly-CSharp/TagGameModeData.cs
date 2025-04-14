using System;
using Fusion;

// Token: 0x02000468 RID: 1128
[NetworkBehaviourWeaved(12)]
public class TagGameModeData : FusionGameModeData
{
	// Token: 0x17000308 RID: 776
	// (get) Token: 0x06001BA1 RID: 7073 RVA: 0x00087858 File Offset: 0x00085A58
	// (set) Token: 0x06001BA2 RID: 7074 RVA: 0x00087865 File Offset: 0x00085A65
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
	// (get) Token: 0x06001BA3 RID: 7075 RVA: 0x00087873 File Offset: 0x00085A73
	// (set) Token: 0x06001BA4 RID: 7076 RVA: 0x0008789D File Offset: 0x00085A9D
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

	// Token: 0x06001BA6 RID: 7078 RVA: 0x000878C8 File Offset: 0x00085AC8
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.tagData = this._tagData;
	}

	// Token: 0x06001BA7 RID: 7079 RVA: 0x000878E0 File Offset: 0x00085AE0
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._tagData = this.tagData;
	}

	// Token: 0x04001E87 RID: 7815
	[WeaverGenerated]
	[DefaultForProperty("tagData", 0, 12)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private TagData _tagData;
}
