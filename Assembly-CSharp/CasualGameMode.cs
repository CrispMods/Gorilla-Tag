using System;
using Fusion;
using GorillaGameModes;
using Photon.Pun;

// Token: 0x0200046C RID: 1132
public class CasualGameMode : GorillaGameManager
{
	// Token: 0x06001BCD RID: 7117 RVA: 0x000430F0 File Offset: 0x000412F0
	public override int MyMatIndex(NetPlayer player)
	{
		if (this.GetMyMaterial == null)
		{
			return 0;
		}
		return this.GetMyMaterial(player);
	}

	// Token: 0x06001BCE RID: 7118 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnSerializeRead(object newData)
	{
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x0003924B File Offset: 0x0003744B
	public override object OnSerializeWrite()
	{
		return null;
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06001BD1 RID: 7121 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x00030498 File Offset: 0x0002E698
	public override GameModeType GameType()
	{
		return GameModeType.Casual;
	}

	// Token: 0x06001BD3 RID: 7123 RVA: 0x00043108 File Offset: 0x00041308
	public override void AddFusionDataBehaviour(NetworkObject behaviour)
	{
		behaviour.AddBehaviour<CasualGameModeData>();
	}

	// Token: 0x06001BD4 RID: 7124 RVA: 0x00043111 File Offset: 0x00041311
	public override string GameModeName()
	{
		return "CASUAL";
	}

	// Token: 0x04001EC9 RID: 7881
	public CasualGameMode.MyMatDelegate GetMyMaterial;

	// Token: 0x0200046D RID: 1133
	// (Invoke) Token: 0x06001BD7 RID: 7127
	public delegate int MyMatDelegate(NetPlayer player);
}
