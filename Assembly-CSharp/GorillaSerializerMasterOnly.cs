using System;
using Fusion;
using Photon.Pun;

// Token: 0x02000537 RID: 1335
[NetworkBehaviourWeaved(0)]
internal abstract class GorillaSerializerMasterOnly : GorillaWrappedSerializer
{
	// Token: 0x06002055 RID: 8277 RVA: 0x000A2DBF File Offset: 0x000A0FBF
	protected override bool ValidOnSerialize(PhotonStream stream, in PhotonMessageInfo info)
	{
		return info.Sender == PhotonNetwork.MasterClient;
	}

	// Token: 0x06002057 RID: 8279 RVA: 0x000A2DD9 File Offset: 0x000A0FD9
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x06002058 RID: 8280 RVA: 0x000A2DE5 File Offset: 0x000A0FE5
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}
}
