using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

// Token: 0x020004EA RID: 1258
public class BuilderTableDataRenderData
{
	// Token: 0x04002206 RID: 8710
	public const int NUM_SPLIT_MESH_INSTANCE_GROUPS = 1;

	// Token: 0x04002207 RID: 8711
	public int texWidth;

	// Token: 0x04002208 RID: 8712
	public int texHeight;

	// Token: 0x04002209 RID: 8713
	public TextureFormat textureFormat;

	// Token: 0x0400220A RID: 8714
	public Dictionary<Material, int> materialToIndex;

	// Token: 0x0400220B RID: 8715
	public List<Material> materials;

	// Token: 0x0400220C RID: 8716
	public Material sharedMaterial;

	// Token: 0x0400220D RID: 8717
	public Material sharedMaterialIndirect;

	// Token: 0x0400220E RID: 8718
	public Dictionary<Texture2D, int> textureToIndex;

	// Token: 0x0400220F RID: 8719
	public List<Texture2D> textures;

	// Token: 0x04002210 RID: 8720
	public List<Material> perTextureMaterial;

	// Token: 0x04002211 RID: 8721
	public List<MaterialPropertyBlock> perTexturePropertyBlock;

	// Token: 0x04002212 RID: 8722
	public Texture2DArray sharedTexArray;

	// Token: 0x04002213 RID: 8723
	public Dictionary<Mesh, int> meshToIndex;

	// Token: 0x04002214 RID: 8724
	public List<Mesh> meshes;

	// Token: 0x04002215 RID: 8725
	public List<int> meshInstanceCount;

	// Token: 0x04002216 RID: 8726
	public NativeList<BuilderTableSubMesh> subMeshes;

	// Token: 0x04002217 RID: 8727
	public Mesh sharedMesh;

	// Token: 0x04002218 RID: 8728
	public BuilderTableDataRenderIndirectBatch dynamicBatch;

	// Token: 0x04002219 RID: 8729
	public BuilderTableDataRenderIndirectBatch staticBatch;

	// Token: 0x0400221A RID: 8730
	public JobHandle setupInstancesJobs;
}
