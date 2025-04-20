using System;
using UnityEngine;

// Token: 0x020008EF RID: 2287
public class UberCombinerAssets : ScriptableObject
{
	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x06003769 RID: 14185 RVA: 0x00054994 File Offset: 0x00052B94
	public static UberCombinerAssets Instance
	{
		get
		{
			UberCombinerAssets.gInstance == null;
			return UberCombinerAssets.gInstance;
		}
	}

	// Token: 0x0600376A RID: 14186 RVA: 0x000549A7 File Offset: 0x00052BA7
	private void OnEnable()
	{
		this.Setup();
	}

	// Token: 0x0600376B RID: 14187 RVA: 0x00030607 File Offset: 0x0002E807
	private void Setup()
	{
	}

	// Token: 0x0600376C RID: 14188 RVA: 0x00030607 File Offset: 0x0002E807
	public void ClearMaterialAssets()
	{
	}

	// Token: 0x0600376D RID: 14189 RVA: 0x00030607 File Offset: 0x0002E807
	public void ClearPrefabAssets()
	{
	}

	// Token: 0x040039A9 RID: 14761
	[SerializeField]
	private UnityEngine.Object _rootFolder;

	// Token: 0x040039AA RID: 14762
	[SerializeField]
	private UnityEngine.Object _resourcesFolder;

	// Token: 0x040039AB RID: 14763
	[SerializeField]
	private UnityEngine.Object _materialsFolder;

	// Token: 0x040039AC RID: 14764
	[SerializeField]
	private UnityEngine.Object _prefabsFolder;

	// Token: 0x040039AD RID: 14765
	[Space]
	public UnityEngine.Object MeshBakerDefaultCustomizer;

	// Token: 0x040039AE RID: 14766
	public Material ReferenceUberMaterial;

	// Token: 0x040039AF RID: 14767
	public Shader TextureArrayCapableShader;

	// Token: 0x040039B0 RID: 14768
	[Space]
	public string RootFolderPath;

	// Token: 0x040039B1 RID: 14769
	public string ResourcesFolderPath;

	// Token: 0x040039B2 RID: 14770
	public string MaterialsFolderPath;

	// Token: 0x040039B3 RID: 14771
	public string PrefabsFolderPath;

	// Token: 0x040039B4 RID: 14772
	private static UberCombinerAssets gInstance;
}
