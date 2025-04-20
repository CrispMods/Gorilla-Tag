using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x0200080A RID: 2058
internal class PaintbrawlRPCs : RPCNetworkBase
{
	// Token: 0x060032D6 RID: 13014 RVA: 0x00051921 File Offset: 0x0004FB21
	public override void SetClassTarget(IWrappedSerializable target, GorillaWrappedSerializer netHandler)
	{
		this.paintbrawlManager = (GorillaPaintbrawlManager)target;
		this.serializer = (GameModeSerializer)netHandler;
	}

	// Token: 0x060032D7 RID: 13015 RVA: 0x00138F00 File Offset: 0x00137100
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

	// Token: 0x04003665 RID: 13925
	private GameModeSerializer serializer;

	// Token: 0x04003666 RID: 13926
	private GorillaPaintbrawlManager paintbrawlManager;
}
