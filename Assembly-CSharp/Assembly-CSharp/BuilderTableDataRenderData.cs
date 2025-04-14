using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

// Token: 0x020004DD RID: 1245
public class BuilderTableDataRenderData
{
	// Token: 0x040021B4 RID: 8628
	public const int NUM_SPLIT_MESH_INSTANCE_GROUPS = 1;

	// Token: 0x040021B5 RID: 8629
	public int texWidth;

	// Token: 0x040021B6 RID: 8630
	public int texHeight;

	// Token: 0x040021B7 RID: 8631
	public TextureFormat textureFormat;

	// Token: 0x040021B8 RID: 8632
	public Dictionary<Material, int> materialToIndex;

	// Token: 0x040021B9 RID: 8633
	public List<Material> materials;

	// Token: 0x040021BA RID: 8634
	public Material sharedMaterial;

	// Token: 0x040021BB RID: 8635
	public Material sharedMaterialIndirect;

	// Token: 0x040021BC RID: 8636
	public Dictionary<Texture2D, int> textureToIndex;

	// Token: 0x040021BD RID: 8637
	public List<Texture2D> textures;

	// Token: 0x040021BE RID: 8638
	public List<Material> perTextureMaterial;

	// Token: 0x040021BF RID: 8639
	public List<MaterialPropertyBlock> perTexturePropertyBlock;

	// Token: 0x040021C0 RID: 8640
	public Texture2DArray sharedTexArray;

	// Token: 0x040021C1 RID: 8641
	public Dictionary<Mesh, int> meshToIndex;

	// Token: 0x040021C2 RID: 8642
	public List<Mesh> meshes;

	// Token: 0x040021C3 RID: 8643
	public List<int> meshInstanceCount;

	// Token: 0x040021C4 RID: 8644
	public NativeList<BuilderTableSubMesh> subMeshes;

	// Token: 0x040021C5 RID: 8645
	public Mesh sharedMesh;

	// Token: 0x040021C6 RID: 8646
	public BuilderTableDataRenderIndirectBatch dynamicBatch;

	// Token: 0x040021C7 RID: 8647
	public BuilderTableDataRenderIndirectBatch staticBatch;

	// Token: 0x040021C8 RID: 8648
	public JobHandle setupInstancesJobs;
}
