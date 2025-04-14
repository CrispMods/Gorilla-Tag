using System;

// Token: 0x0200053B RID: 1339
internal interface IGorillaSerializeableScene : IGorillaSerializeable
{
	// Token: 0x06002084 RID: 8324
	void OnSceneLinking(GorillaSerializerScene serializer);

	// Token: 0x06002085 RID: 8325
	void OnNetworkObjectDisable();

	// Token: 0x06002086 RID: 8326
	void OnNetworkObjectEnable();
}
