using System;
using Fusion;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x0200027D RID: 637
public struct PhotonMessageInfoWrapped
{
	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x00039770 File Offset: 0x00037970
	public NetPlayer Sender
	{
		get
		{
			return NetworkSystem.Instance.GetPlayer(this.senderID);
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x06000EE6 RID: 3814 RVA: 0x00039782 File Offset: 0x00037982
	public double SentServerTime
	{
		get
		{
			return this.sentTick / 1000.0;
		}
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x00039796 File Offset: 0x00037996
	public PhotonMessageInfoWrapped(PhotonMessageInfo info)
	{
		Player sender = info.Sender;
		this.senderID = ((sender != null) ? sender.ActorNumber : -1);
		this.sentTick = info.SentServerTimestamp;
		this.punInfo = info;
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x000397C4 File Offset: 0x000379C4
	public PhotonMessageInfoWrapped(RpcInfo info)
	{
		this.senderID = info.Source.PlayerId;
		this.sentTick = info.Tick.Raw;
		this.punInfo = default(PhotonMessageInfo);
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x000397F5 File Offset: 0x000379F5
	public PhotonMessageInfoWrapped(int playerID, int tick)
	{
		this.senderID = playerID;
		this.sentTick = tick;
		this.punInfo = default(PhotonMessageInfo);
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x00039811 File Offset: 0x00037A11
	public static implicit operator PhotonMessageInfoWrapped(PhotonMessageInfo info)
	{
		return new PhotonMessageInfoWrapped(info);
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x00039819 File Offset: 0x00037A19
	public static implicit operator PhotonMessageInfoWrapped(RpcInfo info)
	{
		return new PhotonMessageInfoWrapped(info);
	}

	// Token: 0x040011AF RID: 4527
	public int senderID;

	// Token: 0x040011B0 RID: 4528
	public int sentTick;

	// Token: 0x040011B1 RID: 4529
	public PhotonMessageInfo punInfo;
}
