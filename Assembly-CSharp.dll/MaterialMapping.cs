using System;
using UnityEngine;

// Token: 0x020008E3 RID: 2275
public class MaterialMapping : ScriptableObject
{
	// Token: 0x060036BE RID: 14014 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void CleanUpData()
	{
	}

	// Token: 0x0400399E RID: 14750
	private static string path = "Assets/UberShaderConversion/MaterialMap.asset";

	// Token: 0x0400399F RID: 14751
	public static string materialDirectory = "Assets/UberShaderConversion/Materials/";

	// Token: 0x040039A0 RID: 14752
	private static MaterialMapping instance;

	// Token: 0x040039A1 RID: 14753
	public ShaderGroup[] map;

	// Token: 0x040039A2 RID: 14754
	public Material mirrorMat;

	// Token: 0x040039A3 RID: 14755
	public RenderTexture mirrorTexture;
}
