using System;
using Photon.Pun;

// Token: 0x0200053A RID: 1338
public interface IGorillaSerializeable
{
	// Token: 0x0600207F RID: 8319
	void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x06002080 RID: 8320
	void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info);
}
