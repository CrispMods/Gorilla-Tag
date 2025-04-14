using System;
using Fusion;
using Photon.Pun;

// Token: 0x02000281 RID: 641
[NetworkBehaviourWeaved(0)]
public class NetworkComponentCallbacks : NetworkComponent
{
	// Token: 0x06000F23 RID: 3875 RVA: 0x0004C100 File Offset: 0x0004A300
	public override void ReadDataFusion()
	{
		this.ReadData();
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x0004C10D File Offset: 0x0004A30D
	public override void WriteDataFusion()
	{
		this.WriteData();
	}

	// Token: 0x06000F25 RID: 3877 RVA: 0x0004C11A File Offset: 0x0004A31A
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.ReadPunData(stream, info);
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x0004C129 File Offset: 0x0004A329
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.WritePunData(stream, info);
	}

	// Token: 0x06000F28 RID: 3880 RVA: 0x00002655 File Offset: 0x00000855
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x06000F29 RID: 3881 RVA: 0x00002661 File Offset: 0x00000861
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}

	// Token: 0x040011BA RID: 4538
	public Action ReadData;

	// Token: 0x040011BB RID: 4539
	public Action WriteData;

	// Token: 0x040011BC RID: 4540
	public Action<PhotonStream, PhotonMessageInfo> ReadPunData;

	// Token: 0x040011BD RID: 4541
	public Action<PhotonStream, PhotonMessageInfo> WritePunData;
}
