using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020007F0 RID: 2032
internal class PaintbrawlRPCs : RPCNetworkBase
{
	// Token: 0x06003220 RID: 12832 RVA: 0x000F0C21 File Offset: 0x000EEE21
	public override void SetClassTarget(IWrappedSerializable target, GorillaWrappedSerializer netHandler)
	{
		this.paintbrawlManager = (GorillaPaintbrawlManager)target;
		this.serializer = (GameModeSerializer)netHandler;
	}

	// Token: 0x06003221 RID: 12833 RVA: 0x000F0C3C File Offset: 0x000EEE3C
	[PunRPC]
	public void RPC_ReportSlingshotHit(Player taggedPlayer, Vector3 hitLocation, int projectileCount, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RPC_ReportSlingshotHit");
		if (!NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		NetPlayer player = NetworkSystem.Instance.GetPlayer(taggedPlayer);
		PhotonMessageInfoWrapped info2 = new PhotonMessageInfoWrapped(info);
		this.paintbrawlManager.ReportSlingshotHit(player, hitLocation, projectileCount, info2);
	}

	// Token: 0x040035AA RID: 13738
	private GameModeSerializer serializer;

	// Token: 0x040035AB RID: 13739
	private GorillaPaintbrawlManager paintbrawlManager;
}
