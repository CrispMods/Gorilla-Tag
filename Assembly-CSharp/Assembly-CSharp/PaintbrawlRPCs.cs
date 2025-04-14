using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020007F3 RID: 2035
internal class PaintbrawlRPCs : RPCNetworkBase
{
	// Token: 0x0600322C RID: 12844 RVA: 0x000F11E9 File Offset: 0x000EF3E9
	public override void SetClassTarget(IWrappedSerializable target, GorillaWrappedSerializer netHandler)
	{
		this.paintbrawlManager = (GorillaPaintbrawlManager)target;
		this.serializer = (GameModeSerializer)netHandler;
	}

	// Token: 0x0600322D RID: 12845 RVA: 0x000F1204 File Offset: 0x000EF404
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

	// Token: 0x040035BC RID: 13756
	private GameModeSerializer serializer;

	// Token: 0x040035BD RID: 13757
	private GorillaPaintbrawlManager paintbrawlManager;
}
