using System;
using Fusion;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x02000288 RID: 648
public struct PhotonMessageInfoWrapped
{
	// Token: 0x1700018B RID: 395
	// (get) Token: 0x06000F2E RID: 3886 RVA: 0x0003AA30 File Offset: 0x00038C30
	public NetPlayer Sender
	{
		get
		{
			return NetworkSystem.Instance.GetPlayer(this.senderID);
		}
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x06000F2F RID: 3887 RVA: 0x0003AA42 File Offset: 0x00038C42
	public double SentServerTime
	{
		get
		{
			return this.sentTick / 1000.0;
		}
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x0003AA56 File Offset: 0x00038C56
	public PhotonMessageInfoWrapped(PhotonMessageInfo info)
	{
		Player sender = info.Sender;
		this.senderID = ((sender != null) ? sender.ActorNumber : -1);
		this.sentTick = info.SentServerTimestamp;
		this.punInfo = info;
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x0003AA84 File Offset: 0x00038C84
	public PhotonMessageInfoWrapped(RpcInfo info)
	{
		this.senderID = info.Source.PlayerId;
		this.sentTick = info.Tick.Raw;
		this.punInfo = default(PhotonMessageInfo);
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x0003AAB5 File Offset: 0x00038CB5
	public PhotonMessageInfoWrapped(int playerID, int tick)
	{
		this.senderID = playerID;
		this.sentTick = tick;
		this.punInfo = default(PhotonMessageInfo);
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x0003AAD1 File Offset: 0x00038CD1
	public static implicit operator PhotonMessageInfoWrapped(PhotonMessageInfo info)
	{
		return new PhotonMessageInfoWrapped(info);
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x0003AAD9 File Offset: 0x00038CD9
	public static implicit operator PhotonMessageInfoWrapped(RpcInfo info)
	{
		return new PhotonMessageInfoWrapped(info);
	}

	// Token: 0x040011F4 RID: 4596
	public int senderID;

	// Token: 0x040011F5 RID: 4597
	public int sentTick;

	// Token: 0x040011F6 RID: 4598
	public PhotonMessageInfo punInfo;
}
