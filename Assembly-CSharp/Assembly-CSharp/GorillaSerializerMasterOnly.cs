using System;
using Fusion;
using Photon.Pun;

// Token: 0x02000537 RID: 1335
[NetworkBehaviourWeaved(0)]
internal abstract class GorillaSerializerMasterOnly : GorillaWrappedSerializer
{
	// Token: 0x06002058 RID: 8280 RVA: 0x000A3143 File Offset: 0x000A1343
	protected override bool ValidOnSerialize(PhotonStream stream, in PhotonMessageInfo info)
	{
		return info.Sender == PhotonNetwork.MasterClient;
	}

	// Token: 0x0600205A RID: 8282 RVA: 0x000A315D File Offset: 0x000A135D
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x0600205B RID: 8283 RVA: 0x000A3169 File Offset: 0x000A1369
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}
}
