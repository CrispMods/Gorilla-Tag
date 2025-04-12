using System;
using Photon.Pun;

// Token: 0x020007CF RID: 1999
[Serializable]
public struct PhotonSignalInfo
{
	// Token: 0x0600313C RID: 12604 RVA: 0x0004FBC5 File Offset: 0x0004DDC5
	public PhotonSignalInfo(NetPlayer sender, int timestamp)
	{
		this.sender = sender;
		this.timestamp = timestamp;
	}

	// Token: 0x17000511 RID: 1297
	// (get) Token: 0x0600313D RID: 12605 RVA: 0x0004FBD5 File Offset: 0x0004DDD5
	public double sentServerTime
	{
		get
		{
			return this.timestamp / 1000.0;
		}
	}

	// Token: 0x0600313E RID: 12606 RVA: 0x0004FBE9 File Offset: 0x0004DDE9
	public override string ToString()
	{
		return string.Format("[{0}: Sender = '{1}' sentTime = {2}]", "PhotonSignalInfo", this.sender.ActorNumber, this.sentServerTime);
	}

	// Token: 0x0600313F RID: 12607 RVA: 0x0004FC15 File Offset: 0x0004DE15
	public static implicit operator PhotonMessageInfo(PhotonSignalInfo psi)
	{
		return new PhotonMessageInfo(psi.sender.GetPlayerRef(), psi.timestamp, null);
	}

	// Token: 0x0400353E RID: 13630
	public readonly int timestamp;

	// Token: 0x0400353F RID: 13631
	public readonly NetPlayer sender;
}
