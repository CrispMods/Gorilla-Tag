using System;
using Fusion;
using Photon.Pun;

// Token: 0x0200053C RID: 1340
internal interface IWrappedSerializable : INetworkStruct
{
	// Token: 0x06002084 RID: 8324
	void OnSerializeRead(object newData);

	// Token: 0x06002085 RID: 8325
	void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x06002086 RID: 8326
	object OnSerializeWrite();

	// Token: 0x06002087 RID: 8327
	void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info);
}
