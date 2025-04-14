using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x020005A2 RID: 1442
public class GorillaTurning : GorillaTriggerBox
{
	// Token: 0x060023DB RID: 9179 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Awake()
	{
	}

	// Token: 0x040027D4 RID: 10196
	public Material redMaterial;

	// Token: 0x040027D5 RID: 10197
	public Material blueMaterial;

	// Token: 0x040027D6 RID: 10198
	public Material greenMaterial;

	// Token: 0x040027D7 RID: 10199
	public Material transparentBlueMaterial;

	// Token: 0x040027D8 RID: 10200
	public Material transparentRedMaterial;

	// Token: 0x040027D9 RID: 10201
	public Material transparentGreenMaterial;

	// Token: 0x040027DA RID: 10202
	public MeshRenderer smoothTurnBox;

	// Token: 0x040027DB RID: 10203
	public MeshRenderer snapTurnBox;

	// Token: 0x040027DC RID: 10204
	public MeshRenderer noTurnBox;

	// Token: 0x040027DD RID: 10205
	public GorillaSnapTurn snapTurn;

	// Token: 0x040027DE RID: 10206
	public string currentChoice;

	// Token: 0x040027DF RID: 10207
	public float currentSpeed;
}
