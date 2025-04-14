﻿using System;
using Fusion;

// Token: 0x02000466 RID: 1126
[NetworkBehaviourWeaved(23)]
public class HuntGameModeData : FusionGameModeData
{
	// Token: 0x17000304 RID: 772
	// (get) Token: 0x06001B97 RID: 7063 RVA: 0x00087783 File Offset: 0x00085983
	// (set) Token: 0x06001B98 RID: 7064 RVA: 0x00087790 File Offset: 0x00085990
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
	// (get) Token: 0x06001B99 RID: 7065 RVA: 0x0008779E File Offset: 0x0008599E
	// (set) Token: 0x06001B9A RID: 7066 RVA: 0x000877C8 File Offset: 0x000859C8
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

	// Token: 0x06001B9C RID: 7068 RVA: 0x000877F3 File Offset: 0x000859F3
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.huntdata = this._huntdata;
	}

	// Token: 0x06001B9D RID: 7069 RVA: 0x0008780B File Offset: 0x00085A0B
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._huntdata = this.huntdata;
	}

	// Token: 0x04001E83 RID: 7811
	[WeaverGenerated]
	[DefaultForProperty("huntdata", 0, 23)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private HuntData _huntdata;
}
