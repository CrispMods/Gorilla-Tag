using System;
using Unity.Collections;
using UnityEngine.Jobs;

// Token: 0x020004DA RID: 1242
public struct BuilderTableMeshInstances
{
	// Token: 0x0400219E RID: 8606
	public TransformAccessArray transforms;

	// Token: 0x0400219F RID: 8607
	public NativeList<int> texIndex;

	// Token: 0x040021A0 RID: 8608
	public NativeList<float> tint;
}
