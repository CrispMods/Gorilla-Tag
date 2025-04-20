using System;

// Token: 0x02000548 RID: 1352
internal interface IGorillaSerializeableScene : IGorillaSerializeable
{
	// Token: 0x060020DA RID: 8410
	void OnSceneLinking(GorillaSerializerScene serializer);

	// Token: 0x060020DB RID: 8411
	void OnNetworkObjectDisable();

	// Token: 0x060020DC RID: 8412
	void OnNetworkObjectEnable();
}
