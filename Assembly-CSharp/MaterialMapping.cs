using System;
using UnityEngine;

// Token: 0x020008E0 RID: 2272
public class MaterialMapping : ScriptableObject
{
	// Token: 0x060036B2 RID: 14002 RVA: 0x000023F4 File Offset: 0x000005F4
	public void CleanUpData()
	{
	}

	// Token: 0x0400398C RID: 14732
	private static string path = "Assets/UberShaderConversion/MaterialMap.asset";

	// Token: 0x0400398D RID: 14733
	public static string materialDirectory = "Assets/UberShaderConversion/Materials/";

	// Token: 0x0400398E RID: 14734
	private static MaterialMapping instance;

	// Token: 0x0400398F RID: 14735
	public ShaderGroup[] map;

	// Token: 0x04003990 RID: 14736
	public Material mirrorMat;

	// Token: 0x04003991 RID: 14737
	public RenderTexture mirrorTexture;
}
