using System;
using Fusion;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x0200027D RID: 637
public struct PhotonMessageInfoWrapped
{
	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x0004BD81 File Offset: 0x00049F81
	public NetPlayer Sender
	{
		get
		{
			return NetworkSystem.Instance.GetPlayer(this.senderID);
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x06000EE4 RID: 3812 RVA: 0x0004BD93 File Offset: 0x00049F93
	public double SentServerTime
	{
		get
		{
			return this.sentTick / 1000.0;
		}
	}

	// Token: 0x06000EE5 RID: 3813 RVA: 0x0004BDA7 File Offset: 0x00049FA7
	public PhotonMessageInfoWrapped(PhotonMessageInfo info)
	{
		Player sender = info.Sender;
		this.senderID = ((sender != null) ? sender.ActorNumber : -1);
		this.sentTick = info.SentServerTimestamp;
		this.punInfo = info;
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x0004BDD5 File Offset: 0x00049FD5
	public PhotonMessageInfoWrapped(RpcInfo info)
	{
		this.senderID = info.Source.PlayerId;
		this.sentTick = info.Tick.Raw;
		this.punInfo = default(PhotonMessageInfo);
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x0004BE06 File Offset: 0x0004A006
	public PhotonMessageInfoWrapped(int playerID, int tick)
	{
		this.senderID = playerID;
		this.sentTick = tick;
		this.punInfo = default(PhotonMessageInfo);
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x0004BE22 File Offset: 0x0004A022
	public static implicit operator PhotonMessageInfoWrapped(PhotonMessageInfo info)
	{
		return new PhotonMessageInfoWrapped(info);
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x0004BE2A File Offset: 0x0004A02A
	public static implicit operator PhotonMessageInfoWrapped(RpcInfo info)
	{
		return new PhotonMessageInfoWrapped(info);
	}

	// Token: 0x040011AE RID: 4526
	public int senderID;

	// Token: 0x040011AF RID: 4527
	public int sentTick;

	// Token: 0x040011B0 RID: 4528
	public PhotonMessageInfo punInfo;
}
