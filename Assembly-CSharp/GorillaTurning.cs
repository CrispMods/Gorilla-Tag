using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x020005B0 RID: 1456
public class GorillaTurning : GorillaTriggerBox
{
	// Token: 0x0600243B RID: 9275 RVA: 0x00030607 File Offset: 0x0002E807
	private void Awake()
	{
	}

	// Token: 0x04002830 RID: 10288
	public Material redMaterial;

	// Token: 0x04002831 RID: 10289
	public Material blueMaterial;

	// Token: 0x04002832 RID: 10290
	public Material greenMaterial;

	// Token: 0x04002833 RID: 10291
	public Material transparentBlueMaterial;

	// Token: 0x04002834 RID: 10292
	public Material transparentRedMaterial;

	// Token: 0x04002835 RID: 10293
	public Material transparentGreenMaterial;

	// Token: 0x04002836 RID: 10294
	public MeshRenderer smoothTurnBox;

	// Token: 0x04002837 RID: 10295
	public MeshRenderer snapTurnBox;

	// Token: 0x04002838 RID: 10296
	public MeshRenderer noTurnBox;

	// Token: 0x04002839 RID: 10297
	public GorillaSnapTurn snapTurn;

	// Token: 0x0400283A RID: 10298
	public string currentChoice;

	// Token: 0x0400283B RID: 10299
	public float currentSpeed;
}
