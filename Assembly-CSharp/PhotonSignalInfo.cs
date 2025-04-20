using System;
using Photon.Pun;

// Token: 0x020007E6 RID: 2022
[Serializable]
public struct PhotonSignalInfo
{
	// Token: 0x060031E6 RID: 12774 RVA: 0x00050FC7 File Offset: 0x0004F1C7
	public PhotonSignalInfo(NetPlayer sender, int timestamp)
	{
		this.sender = sender;
		this.timestamp = timestamp;
	}

	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x060031E7 RID: 12775 RVA: 0x00050FD7 File Offset: 0x0004F1D7
	public double sentServerTime
	{
		get
		{
			return this.timestamp / 1000.0;
		}
	}

	// Token: 0x060031E8 RID: 12776 RVA: 0x00050FEB File Offset: 0x0004F1EB
	public override string ToString()
	{
		return string.Format("[{0}: Sender = '{1}' sentTime = {2}]", "PhotonSignalInfo", this.sender.ActorNumber, this.sentServerTime);
	}

	// Token: 0x060031E9 RID: 12777 RVA: 0x00051017 File Offset: 0x0004F217
	public static implicit operator PhotonMessageInfo(PhotonSignalInfo psi)
	{
		return new PhotonMessageInfo(psi.sender.GetPlayerRef(), psi.timestamp, null);
	}

	// Token: 0x040035E2 RID: 13794
	public readonly int timestamp;

	// Token: 0x040035E3 RID: 13795
	public readonly NetPlayer sender;
}
