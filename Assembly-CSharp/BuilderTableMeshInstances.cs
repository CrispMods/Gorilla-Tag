using System;
using Unity.Collections;
using UnityEngine.Jobs;

// Token: 0x020004DA RID: 1242
public struct BuilderTableMeshInstances
{
	// Token: 0x0400219D RID: 8605
	public TransformAccessArray transforms;

	// Token: 0x0400219E RID: 8606
	public NativeList<int> texIndex;

	// Token: 0x0400219F RID: 8607
	public NativeList<float> tint;
}
