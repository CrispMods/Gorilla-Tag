using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

// Token: 0x020004DC RID: 1244
public class BuilderTableDataRenderIndirectBatch
{
	// Token: 0x040021A4 RID: 8612
	public int totalInstances;

	// Token: 0x040021A5 RID: 8613
	public TransformAccessArray instanceTransform;

	// Token: 0x040021A6 RID: 8614
	public NativeArray<int> instanceTransformIndexToDataIndex;

	// Token: 0x040021A7 RID: 8615
	public NativeArray<Matrix4x4> instanceObjectToWorld;

	// Token: 0x040021A8 RID: 8616
	public NativeArray<int> instanceTexIndex;

	// Token: 0x040021A9 RID: 8617
	public NativeArray<float> instanceTint;

	// Token: 0x040021AA RID: 8618
	public NativeArray<int> instanceLodLevel;

	// Token: 0x040021AB RID: 8619
	public NativeArray<int> instanceLodLevelDirty;

	// Token: 0x040021AC RID: 8620
	public NativeList<BuilderTableMeshInstances> renderMeshes;

	// Token: 0x040021AD RID: 8621
	public GraphicsBuffer commandBuf;

	// Token: 0x040021AE RID: 8622
	public GraphicsBuffer matrixBuf;

	// Token: 0x040021AF RID: 8623
	public GraphicsBuffer texIndexBuf;

	// Token: 0x040021B0 RID: 8624
	public GraphicsBuffer tintBuf;

	// Token: 0x040021B1 RID: 8625
	public NativeArray<GraphicsBuffer.IndirectDrawIndexedArgs> commandData;

	// Token: 0x040021B2 RID: 8626
	public int commandCount;

	// Token: 0x040021B3 RID: 8627
	public RenderParams rp;
}
