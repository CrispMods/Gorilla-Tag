using System;
using UnityEngine;

// Token: 0x020008D3 RID: 2259
public class UberCombinerAssets : ScriptableObject
{
	// Token: 0x17000590 RID: 1424
	// (get) Token: 0x060036A1 RID: 13985 RVA: 0x00102507 File Offset: 0x00100707
	public static UberCombinerAssets Instance
	{
		get
		{
			UberCombinerAssets.gInstance == null;
			return UberCombinerAssets.gInstance;
		}
	}

	// Token: 0x060036A2 RID: 13986 RVA: 0x0010251A File Offset: 0x0010071A
	private void OnEnable()
	{
		this.Setup();
	}

	// Token: 0x060036A3 RID: 13987 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Setup()
	{
	}

	// Token: 0x060036A4 RID: 13988 RVA: 0x000023F4 File Offset: 0x000005F4
	public void ClearMaterialAssets()
	{
	}

	// Token: 0x060036A5 RID: 13989 RVA: 0x000023F4 File Offset: 0x000005F4
	public void ClearPrefabAssets()
	{
	}

	// Token: 0x040038E8 RID: 14568
	[SerializeField]
	private Object _rootFolder;

	// Token: 0x040038E9 RID: 14569
	[SerializeField]
	private Object _resourcesFolder;

	// Token: 0x040038EA RID: 14570
	[SerializeField]
	private Object _materialsFolder;

	// Token: 0x040038EB RID: 14571
	[SerializeField]
	private Object _prefabsFolder;

	// Token: 0x040038EC RID: 14572
	[Space]
	public Object MeshBakerDefaultCustomizer;

	// Token: 0x040038ED RID: 14573
	public Material ReferenceUberMaterial;

	// Token: 0x040038EE RID: 14574
	public Shader TextureArrayCapableShader;

	// Token: 0x040038EF RID: 14575
	[Space]
	public string RootFolderPath;

	// Token: 0x040038F0 RID: 14576
	public string ResourcesFolderPath;

	// Token: 0x040038F1 RID: 14577
	public string MaterialsFolderPath;

	// Token: 0x040038F2 RID: 14578
	public string PrefabsFolderPath;

	// Token: 0x040038F3 RID: 14579
	private static UberCombinerAssets gInstance;
}
