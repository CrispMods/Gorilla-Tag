using System;
using Fusion;
using Photon.Pun;

// Token: 0x0200028C RID: 652
[NetworkBehaviourWeaved(0)]
public class NetworkComponentCallbacks : NetworkComponent
{
	// Token: 0x06000F6E RID: 3950 RVA: 0x0003AD36 File Offset: 0x00038F36
	public override void ReadDataFusion()
	{
		this.ReadData();
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x0003AD43 File Offset: 0x00038F43
	public override void WriteDataFusion()
	{
		this.WriteData();
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x0003AD50 File Offset: 0x00038F50
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.ReadPunData(stream, info);
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x0003AD5F File Offset: 0x00038F5F
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.WritePunData(stream, info);
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x00030709 File Offset: 0x0002E909
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x06000F74 RID: 3956 RVA: 0x00030715 File Offset: 0x0002E915
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}

	// Token: 0x04001202 RID: 4610
	public Action ReadData;

	// Token: 0x04001203 RID: 4611
	public Action WriteData;

	// Token: 0x04001204 RID: 4612
	public Action<PhotonStream, PhotonMessageInfo> ReadPunData;

	// Token: 0x04001205 RID: 4613
	public Action<PhotonStream, PhotonMessageInfo> WritePunData;
}
