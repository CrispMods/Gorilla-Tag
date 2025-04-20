using System;
using Fusion;
using Photon.Pun;

// Token: 0x02000549 RID: 1353
internal interface IWrappedSerializable : INetworkStruct
{
	// Token: 0x060020DD RID: 8413
	void OnSerializeRead(object newData);

	// Token: 0x060020DE RID: 8414
	void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x060020DF RID: 8415
	object OnSerializeWrite();

	// Token: 0x060020E0 RID: 8416
	void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info);
}
