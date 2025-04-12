using System;
using Fusion;
using Photon.Pun;

// Token: 0x02000281 RID: 641
[NetworkBehaviourWeaved(0)]
public class NetworkComponentCallbacks : NetworkComponent
{
	// Token: 0x06000F25 RID: 3877 RVA: 0x00039A76 File Offset: 0x00037C76
	public override void ReadDataFusion()
	{
		this.ReadData();
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x00039A83 File Offset: 0x00037C83
	public override void WriteDataFusion()
	{
		this.WriteData();
	}

	// Token: 0x06000F27 RID: 3879 RVA: 0x00039A90 File Offset: 0x00037C90
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.ReadPunData(stream, info);
	}

	// Token: 0x06000F28 RID: 3880 RVA: 0x00039A9F File Offset: 0x00037C9F
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.WritePunData(stream, info);
	}

	// Token: 0x06000F2A RID: 3882 RVA: 0x0002F861 File Offset: 0x0002DA61
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x06000F2B RID: 3883 RVA: 0x0002F86D File Offset: 0x0002DA6D
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}

	// Token: 0x040011BB RID: 4539
	public Action ReadData;

	// Token: 0x040011BC RID: 4540
	public Action WriteData;

	// Token: 0x040011BD RID: 4541
	public Action<PhotonStream, PhotonMessageInfo> ReadPunData;

	// Token: 0x040011BE RID: 4542
	public Action<PhotonStream, PhotonMessageInfo> WritePunData;
}
