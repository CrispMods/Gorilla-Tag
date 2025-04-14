using System;
using Photon.Pun;

// Token: 0x020007CE RID: 1998
[Serializable]
public struct PhotonSignalInfo
{
	// Token: 0x06003134 RID: 12596 RVA: 0x000EDADC File Offset: 0x000EBCDC
	public PhotonSignalInfo(NetPlayer sender, int timestamp)
	{
		this.sender = sender;
		this.timestamp = timestamp;
	}

	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x06003135 RID: 12597 RVA: 0x000EDAEC File Offset: 0x000EBCEC
	public double sentServerTime
	{
		get
		{
			return this.timestamp / 1000.0;
		}
	}

	// Token: 0x06003136 RID: 12598 RVA: 0x000EDB00 File Offset: 0x000EBD00
	public override string ToString()
	{
		return string.Format("[{0}: Sender = '{1}' sentTime = {2}]", "PhotonSignalInfo", this.sender.ActorNumber, this.sentServerTime);
	}

	// Token: 0x06003137 RID: 12599 RVA: 0x000EDB2C File Offset: 0x000EBD2C
	public static implicit operator PhotonMessageInfo(PhotonSignalInfo psi)
	{
		return new PhotonMessageInfo(psi.sender.GetPlayerRef(), psi.timestamp, null);
	}

	// Token: 0x04003538 RID: 13624
	public readonly int timestamp;

	// Token: 0x04003539 RID: 13625
	public readonly NetPlayer sender;
}
