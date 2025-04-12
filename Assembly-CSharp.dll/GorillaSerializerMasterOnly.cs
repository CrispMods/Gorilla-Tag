using System;
using Fusion;
using Photon.Pun;

// Token: 0x02000537 RID: 1335
[NetworkBehaviourWeaved(0)]
internal abstract class GorillaSerializerMasterOnly : GorillaWrappedSerializer
{
	// Token: 0x06002058 RID: 8280 RVA: 0x0004510A File Offset: 0x0004330A
	protected override bool ValidOnSerialize(PhotonStream stream, in PhotonMessageInfo info)
	{
		return info.Sender == PhotonNetwork.MasterClient;
	}

	// Token: 0x0600205A RID: 8282 RVA: 0x00045124 File Offset: 0x00043324
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x0600205B RID: 8283 RVA: 0x00045130 File Offset: 0x00043330
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}
}
