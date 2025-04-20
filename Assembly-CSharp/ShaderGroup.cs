using System;
using UnityEngine;

// Token: 0x020008FD RID: 2301
[Serializable]
public struct ShaderGroup
{
	// Token: 0x0600377D RID: 14205 RVA: 0x00054A4A File Offset: 0x00052C4A
	public ShaderGroup(Material material, Shader original, Shader gameplay, Shader baking)
	{
		this.material = material;
		this.originalShader = original;
		this.gameplayShader = gameplay;
		this.bakingShader = baking;
	}

	// Token: 0x04003A53 RID: 14931
	public Material material;

	// Token: 0x04003A54 RID: 14932
	public Shader originalShader;

	// Token: 0x04003A55 RID: 14933
	public Shader gameplayShader;

	// Token: 0x04003A56 RID: 14934
	public Shader bakingShader;
}
