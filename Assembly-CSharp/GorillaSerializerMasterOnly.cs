using System;
using Fusion;
using Photon.Pun;

// Token: 0x02000544 RID: 1348
[NetworkBehaviourWeaved(0)]
internal abstract class GorillaSerializerMasterOnly : GorillaWrappedSerializer
{
	// Token: 0x060020AE RID: 8366 RVA: 0x000464A9 File Offset: 0x000446A9
	protected override bool ValidOnSerialize(PhotonStream stream, in PhotonMessageInfo info)
	{
		return info.Sender == PhotonNetwork.MasterClient;
	}

	// Token: 0x060020B0 RID: 8368 RVA: 0x000464C3 File Offset: 0x000446C3
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x060020B1 RID: 8369 RVA: 0x000464CF File Offset: 0x000446CF
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}
}
