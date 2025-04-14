using System;
using Fusion;
using Photon.Pun;

// Token: 0x0200053C RID: 1340
internal interface IWrappedSerializable : INetworkStruct
{
	// Token: 0x06002087 RID: 8327
	void OnSerializeRead(object newData);

	// Token: 0x06002088 RID: 8328
	void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x06002089 RID: 8329
	object OnSerializeWrite();

	// Token: 0x0600208A RID: 8330
	void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info);
}
