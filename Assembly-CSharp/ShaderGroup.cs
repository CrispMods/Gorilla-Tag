using System;
using UnityEngine;

// Token: 0x020008E1 RID: 2273
[Serializable]
public struct ShaderGroup
{
	// Token: 0x060036B5 RID: 14005 RVA: 0x0010311B File Offset: 0x0010131B
	public ShaderGroup(Material material, Shader original, Shader gameplay, Shader baking)
	{
		this.material = material;
		this.originalShader = original;
		this.gameplayShader = gameplay;
		this.bakingShader = baking;
	}

	// Token: 0x04003992 RID: 14738
	public Material material;

	// Token: 0x04003993 RID: 14739
	public Shader originalShader;

	// Token: 0x04003994 RID: 14740
	public Shader gameplayShader;

	// Token: 0x04003995 RID: 14741
	public Shader bakingShader;
}
