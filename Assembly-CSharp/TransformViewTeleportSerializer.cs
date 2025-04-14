using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007B6 RID: 1974
[NetworkBehaviourWeaved(1)]
public class TransformViewTeleportSerializer : NetworkComponent
{
	// Token: 0x06003092 RID: 12434 RVA: 0x000E9DB9 File Offset: 0x000E7FB9
	protected override void Start()
	{
		base.Start();
		this.transformView = base.GetComponent<GorillaNetworkTransform>();
	}

	// Token: 0x06003093 RID: 12435 RVA: 0x000E9DCD File Offset: 0x000E7FCD
	public void SetWillTeleport()
	{
		this.willTeleport = true;
	}

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x06003094 RID: 12436 RVA: 0x000E9DD6 File Offset: 0x000E7FD6
	// (set) Token: 0x06003095 RID: 12437 RVA: 0x000E9E00 File Offset: 0x000E8000
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

	// Token: 0x06003096 RID: 12438 RVA: 0x000E9E2B File Offset: 0x000E802B
	public override void WriteDataFusion()
	{
		this.Data = this.willTeleport;
		this.willTeleport = false;
	}

	// Token: 0x06003097 RID: 12439 RVA: 0x000E9E45 File Offset: 0x000E8045
	public override void ReadDataFusion()
	{
		if (this.Data)
		{
			this.transformView.GTAddition_DoTeleport();
		}
	}

	// Token: 0x06003098 RID: 12440 RVA: 0x000E9E5F File Offset: 0x000E805F
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (this.transformView.RespectOwnership && info.Sender != info.photonView.Owner)
		{
			return;
		}
		stream.SendNext(this.willTeleport);
		this.willTeleport = false;
	}

	// Token: 0x06003099 RID: 12441 RVA: 0x000E9E9A File Offset: 0x000E809A
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

	// Token: 0x0600309B RID: 12443 RVA: 0x000E9ED5 File Offset: 0x000E80D5
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x0600309C RID: 12444 RVA: 0x000E9EED File Offset: 0x000E80ED
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x04003483 RID: 13443
	private bool willTeleport;

	// Token: 0x04003484 RID: 13444
	private GorillaNetworkTransform transformView;

	// Token: 0x04003485 RID: 13445
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private NetworkBool _Data;
}
