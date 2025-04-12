using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x020005A3 RID: 1443
public class GorillaTurning : GorillaTriggerBox
{
	// Token: 0x060023E3 RID: 9187 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void Awake()
	{
	}

	// Token: 0x040027DA RID: 10202
	public Material redMaterial;

	// Token: 0x040027DB RID: 10203
	public Material blueMaterial;

	// Token: 0x040027DC RID: 10204
	public Material greenMaterial;

	// Token: 0x040027DD RID: 10205
	public Material transparentBlueMaterial;

	// Token: 0x040027DE RID: 10206
	public Material transparentRedMaterial;

	// Token: 0x040027DF RID: 10207
	public Material transparentGreenMaterial;

	// Token: 0x040027E0 RID: 10208
	public MeshRenderer smoothTurnBox;

	// Token: 0x040027E1 RID: 10209
	public MeshRenderer snapTurnBox;

	// Token: 0x040027E2 RID: 10210
	public MeshRenderer noTurnBox;

	// Token: 0x040027E3 RID: 10211
	public GorillaSnapTurn snapTurn;

	// Token: 0x040027E4 RID: 10212
	public string currentChoice;

	// Token: 0x040027E5 RID: 10213
	public float currentSpeed;
}
