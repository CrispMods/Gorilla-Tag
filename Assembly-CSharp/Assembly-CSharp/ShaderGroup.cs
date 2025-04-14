using System;
using UnityEngine;

// Token: 0x020008E4 RID: 2276
[Serializable]
public struct ShaderGroup
{
	// Token: 0x060036C1 RID: 14017 RVA: 0x001036E3 File Offset: 0x001018E3
	public ShaderGroup(Material material, Shader original, Shader gameplay, Shader baking)
	{
		this.material = material;
		this.originalShader = original;
		this.gameplayShader = gameplay;
		this.bakingShader = baking;
	}

	// Token: 0x040039A4 RID: 14756
	public Material material;

	// Token: 0x040039A5 RID: 14757
	public Shader originalShader;

	// Token: 0x040039A6 RID: 14758
	public Shader gameplayShader;

	// Token: 0x040039A7 RID: 14759
	public Shader bakingShader;
}
