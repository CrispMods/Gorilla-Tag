using System;
using UnityEngine;

// Token: 0x020008FC RID: 2300
public class MaterialMapping : ScriptableObject
{
	// Token: 0x0600377A RID: 14202 RVA: 0x00030607 File Offset: 0x0002E807
	public void CleanUpData()
	{
	}

	// Token: 0x04003A4D RID: 14925
	private static string path = "Assets/UberShaderConversion/MaterialMap.asset";

	// Token: 0x04003A4E RID: 14926
	public static string materialDirectory = "Assets/UberShaderConversion/Materials/";

	// Token: 0x04003A4F RID: 14927
	private static MaterialMapping instance;

	// Token: 0x04003A50 RID: 14928
	public ShaderGroup[] map;

	// Token: 0x04003A51 RID: 14929
	public Material mirrorMat;

	// Token: 0x04003A52 RID: 14930
	public RenderTexture mirrorTexture;
}
