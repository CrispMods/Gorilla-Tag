using System;
using Photon.Pun;

// Token: 0x02000547 RID: 1351
public interface IGorillaSerializeable
{
	// Token: 0x060020D8 RID: 8408
	void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x060020D9 RID: 8409
	void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info);
}
