using System;
using Fusion;
using GorillaGameModes;
using Photon.Pun;

// Token: 0x02000460 RID: 1120
public class CasualGameMode : GorillaGameManager
{
	// Token: 0x06001B79 RID: 7033 RVA: 0x000872EF File Offset: 0x000854EF
	public override int MyMatIndex(NetPlayer player)
	{
		if (this.GetMyMaterial == null)
		{
			return 0;
		}
		return this.GetMyMaterial(player);
	}

	// Token: 0x06001B7A RID: 7034 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnSerializeRead(object newData)
	{
	}

	// Token: 0x06001B7B RID: 7035 RVA: 0x00042E31 File Offset: 0x00041031
	public override object OnSerializeWrite()
	{
		return null;
	}

	// Token: 0x06001B7C RID: 7036 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06001B7E RID: 7038 RVA: 0x00002076 File Offset: 0x00000276
	public override GameModeType GameType()
	{
		return GameModeType.Casual;
	}

	// Token: 0x06001B7F RID: 7039 RVA: 0x00087307 File Offset: 0x00085507
	public override void AddFusionDataBehaviour(NetworkObject behaviour)
	{
		behaviour.AddBehaviour<CasualGameModeData>();
	}

	// Token: 0x06001B80 RID: 7040 RVA: 0x00087310 File Offset: 0x00085510
	public override string GameModeName()
	{
		return "CASUAL";
	}

	// Token: 0x04001E7A RID: 7802
	public CasualGameMode.MyMatDelegate GetMyMaterial;

	// Token: 0x02000461 RID: 1121
	// (Invoke) Token: 0x06001B83 RID: 7043
	public delegate int MyMatDelegate(NetPlayer player);
}
