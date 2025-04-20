using System;
using Unity.Collections;
using UnityEngine.Jobs;

// Token: 0x020004E7 RID: 1255
public struct BuilderTableMeshInstances
{
	// Token: 0x040021F0 RID: 8688
	public TransformAccessArray transforms;

	// Token: 0x040021F1 RID: 8689
	public NativeList<int> texIndex;

	// Token: 0x040021F2 RID: 8690
	public NativeList<float> tint;
}
