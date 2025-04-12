using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007B7 RID: 1975
[NetworkBehaviourWeaved(1)]
public class TransformViewTeleportSerializer : NetworkComponent
{
	// Token: 0x0600309A RID: 12442 RVA: 0x0004F56A File Offset: 0x0004D76A
	protected override void Start()
	{
		base.Start();
		this.transformView = base.GetComponent<GorillaNetworkTransform>();
	}

	// Token: 0x0600309B RID: 12443 RVA: 0x0004F57E File Offset: 0x0004D77E
	public void SetWillTeleport()
	{
		this.willTeleport = true;
	}

	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x0600309C RID: 12444 RVA: 0x0004F587 File Offset: 0x0004D787
	// (set) Token: 0x0600309D RID: 12445 RVA: 0x0004F5B1 File Offset: 0x0004D7B1
	[Networked]
	[NetworkedWeaved(0, 1)]
	public unsafe NetworkBool Data
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing TransformViewTeleportSerializer.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(NetworkBool*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing TransformViewTeleportSerializer.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(NetworkBool*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x0600309E RID: 12446 RVA: 0x0004F5DC File Offset: 0x0004D7DC
	public override void WriteDataFusion()
	{
		this.Data = this.willTeleport;
		this.willTeleport = false;
	}

	// Token: 0x0600309F RID: 12447 RVA: 0x0004F5F6 File Offset: 0x0004D7F6
	public override void ReadDataFusion()
	{
		if (this.Data)
		{
			this.transformView.GTAddition_DoTeleport();
		}
	}

	// Token: 0x060030A0 RID: 12448 RVA: 0x0004F610 File Offset: 0x0004D810
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (this.transformView.RespectOwnership && info.Sender != info.photonView.Owner)
		{
			return;
		}
		stream.SendNext(this.willTeleport);
		this.willTeleport = false;
	}

	// Token: 0x060030A1 RID: 12449 RVA: 0x0004F64B File Offset: 0x0004D84B
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (this.transformView.RespectOwnership && info.Sender != info.photonView.Owner)
		{
			return;
		}
		if ((bool)stream.ReceiveNext())
		{
			this.transformView.GTAddition_DoTeleport();
		}
	}

	// Token: 0x060030A3 RID: 12451 RVA: 0x0004F686 File Offset: 0x0004D886
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x060030A4 RID: 12452 RVA: 0x0004F69E File Offset: 0x0004D89E
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x04003489 RID: 13449
	private bool willTeleport;

	// Token: 0x0400348A RID: 13450
	private GorillaNetworkTransform transformView;

	// Token: 0x0400348B RID: 13451
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private NetworkBool _Data;
}
