using System;

// Token: 0x0200053B RID: 1339
internal interface IGorillaSerializeableScene : IGorillaSerializeable
{
	// Token: 0x06002081 RID: 8321
	void OnSceneLinking(GorillaSerializerScene serializer);

	// Token: 0x06002082 RID: 8322
	void OnNetworkObjectDisable();

	// Token: 0x06002083 RID: 8323
	void OnNetworkObjectEnable();
}
