using System;
using Fusion;
using GorillaGameModes;
using Photon.Pun;

// Token: 0x02000460 RID: 1120
public class CasualGameMode : GorillaGameManager
{
	// Token: 0x06001B7C RID: 7036 RVA: 0x00041DB7 File Offset: 0x0003FFB7
	public override int MyMatIndex(NetPlayer player)
	{
		if (this.GetMyMaterial == null)
		{
			return 0;
		}
		return this.GetMyMaterial(player);
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void OnSerializeRead(object newData)
	{
	}

	// Token: 0x06001B7E RID: 7038 RVA: 0x00037F8B File Offset: 0x0003618B
	public override object OnSerializeWrite()
	{
		return null;
	}

	// Token: 0x06001B7F RID: 7039 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06001B80 RID: 7040 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06001B81 RID: 7041 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
	public override GameModeType GameType()
	{
		return GameModeType.Casual;
	}

	// Token: 0x06001B82 RID: 7042 RVA: 0x00041DCF File Offset: 0x0003FFCF
	public override void AddFusionDataBehaviour(NetworkObject behaviour)
	{
		behaviour.AddBehaviour<CasualGameModeData>();
	}

	// Token: 0x06001B83 RID: 7043 RVA: 0x00041DD8 File Offset: 0x0003FFD8
	public override string GameModeName()
	{
		return "CASUAL";
	}

	// Token: 0x04001E7B RID: 7803
	public CasualGameMode.MyMatDelegate GetMyMaterial;

	// Token: 0x02000461 RID: 1121
	// (Invoke) Token: 0x06001B86 RID: 7046
	public delegate int MyMatDelegate(NetPlayer player);
}
