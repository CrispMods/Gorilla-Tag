using System;
using UnityEngine;

// Token: 0x020008D6 RID: 2262
public class UberCombinerAssets : ScriptableObject
{
	// Token: 0x17000591 RID: 1425
	// (get) Token: 0x060036AD RID: 13997 RVA: 0x00102ACF File Offset: 0x00100CCF
	public static UberCombinerAssets Instance
	{
		get
		{
			UberCombinerAssets.gInstance == null;
			return UberCombinerAssets.gInstance;
		}
	}

	// Token: 0x060036AE RID: 13998 RVA: 0x00102AE2 File Offset: 0x00100CE2
	private void OnEnable()
	{
		this.Setup();
	}

	// Token: 0x060036AF RID: 13999 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Setup()
	{
	}

	// Token: 0x060036B0 RID: 14000 RVA: 0x000023F4 File Offset: 0x000005F4
	public void ClearMaterialAssets()
	{
	}

	// Token: 0x060036B1 RID: 14001 RVA: 0x000023F4 File Offset: 0x000005F4
	public void ClearPrefabAssets()
	{
	}

	// Token: 0x040038FA RID: 14586
	[SerializeField]
	private Object _rootFolder;

	// Token: 0x040038FB RID: 14587
	[SerializeField]
	private Object _resourcesFolder;

	// Token: 0x040038FC RID: 14588
	[SerializeField]
	private Object _materialsFolder;

	// Token: 0x040038FD RID: 14589
	[SerializeField]
	private Object _prefabsFolder;

	// Token: 0x040038FE RID: 14590
	[Space]
	public Object MeshBakerDefaultCustomizer;

	// Token: 0x040038FF RID: 14591
	public Material ReferenceUberMaterial;

	// Token: 0x04003900 RID: 14592
	public Shader TextureArrayCapableShader;

	// Token: 0x04003901 RID: 14593
	[Space]
	public string RootFolderPath;

	// Token: 0x04003902 RID: 14594
	public string ResourcesFolderPath;

	// Token: 0x04003903 RID: 14595
	public string MaterialsFolderPath;

	// Token: 0x04003904 RID: 14596
	public string PrefabsFolderPath;

	// Token: 0x04003905 RID: 14597
	private static UberCombinerAssets gInstance;
}
