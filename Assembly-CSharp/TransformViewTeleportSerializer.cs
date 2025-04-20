using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007CE RID: 1998
[NetworkBehaviourWeaved(1)]
public class TransformViewTeleportSerializer : NetworkComponent
{
	// Token: 0x06003144 RID: 12612 RVA: 0x0005096C File Offset: 0x0004EB6C
	protected override void Start()
	{
		base.Start();
		this.transformView = base.GetComponent<GorillaNetworkTransform>();
	}

	// Token: 0x06003145 RID: 12613 RVA: 0x00050980 File Offset: 0x0004EB80
	public void SetWillTeleport()
	{
		this.willTeleport = true;
	}

	// Token: 0x17000515 RID: 1301
	// (get) Token: 0x06003146 RID: 12614 RVA: 0x00050989 File Offset: 0x0004EB89
	// (set) Token: 0x06003147 RID: 12615 RVA: 0x000509B3 File Offset: 0x0004EBB3
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

	// Token: 0x06003148 RID: 12616 RVA: 0x000509DE File Offset: 0x0004EBDE
	public override void WriteDataFusion()
	{
		this.Data = this.willTeleport;
		this.willTeleport = false;
	}

	// Token: 0x06003149 RID: 12617 RVA: 0x000509F8 File Offset: 0x0004EBF8
	public override void ReadDataFusion()
	{
		if (this.Data)
		{
			this.transformView.GTAddition_DoTeleport();
		}
	}

	// Token: 0x0600314A RID: 12618 RVA: 0x00050A12 File Offset: 0x0004EC12
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (this.transformView.RespectOwnership && info.Sender != info.photonView.Owner)
		{
			return;
		}
		stream.SendNext(this.willTeleport);
		this.willTeleport = false;
	}

	// Token: 0x0600314B RID: 12619 RVA: 0x00050A4D File Offset: 0x0004EC4D
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

	// Token: 0x0600314D RID: 12621 RVA: 0x00050A88 File Offset: 0x0004EC88
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x0600314E RID: 12622 RVA: 0x00050AA0 File Offset: 0x0004ECA0
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x0400352D RID: 13613
	private bool willTeleport;

	// Token: 0x0400352E RID: 13614
	private GorillaNetworkTransform transformView;

	// Token: 0x0400352F RID: 13615
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private NetworkBool _Data;
}
