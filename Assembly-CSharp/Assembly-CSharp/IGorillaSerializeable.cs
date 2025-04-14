using System;
using Photon.Pun;

// Token: 0x0200053A RID: 1338
public interface IGorillaSerializeable
{
	// Token: 0x06002082 RID: 8322
	void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x06002083 RID: 8323
	void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info);
}
