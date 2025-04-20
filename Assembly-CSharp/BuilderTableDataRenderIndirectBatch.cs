using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

// Token: 0x020004E9 RID: 1257
public class BuilderTableDataRenderIndirectBatch
{
	// Token: 0x040021F6 RID: 8694
	public int totalInstances;

	// Token: 0x040021F7 RID: 8695
	public TransformAccessArray instanceTransform;

	// Token: 0x040021F8 RID: 8696
	public NativeArray<int> instanceTransformIndexToDataIndex;

	// Token: 0x040021F9 RID: 8697
	public NativeArray<Matrix4x4> instanceObjectToWorld;

	// Token: 0x040021FA RID: 8698
	public NativeArray<int> instanceTexIndex;

	// Token: 0x040021FB RID: 8699
	public NativeArray<float> instanceTint;

	// Token: 0x040021FC RID: 8700
	public NativeArray<int> instanceLodLevel;

	// Token: 0x040021FD RID: 8701
	public NativeArray<int> instanceLodLevelDirty;

	// Token: 0x040021FE RID: 8702
	public NativeList<BuilderTableMeshInstances> renderMeshes;

	// Token: 0x040021FF RID: 8703
	public GraphicsBuffer commandBuf;

	// Token: 0x04002200 RID: 8704
	public GraphicsBuffer matrixBuf;

	// Token: 0x04002201 RID: 8705
	public GraphicsBuffer texIndexBuf;

	// Token: 0x04002202 RID: 8706
	public GraphicsBuffer tintBuf;

	// Token: 0x04002203 RID: 8707
	public NativeArray<GraphicsBuffer.IndirectDrawIndexedArgs> commandData;

	// Token: 0x04002204 RID: 8708
	public int commandCount;

	// Token: 0x04002205 RID: 8709
	public RenderParams rp;
}
