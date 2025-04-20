using System;
using UnityEngine;

// Token: 0x020004B9 RID: 1209
[CreateAssetMenu(fileName = "BuilderPieceEffectInfo", menuName = "Gorilla Tag/Builder/EffectInfo", order = 0)]
public class BuilderPieceEffectInfo : ScriptableObject
{
	// Token: 0x04002065 RID: 8293
	public GameObject placeVFX;

	// Token: 0x04002066 RID: 8294
	public GameObject disconnectVFX;

	// Token: 0x04002067 RID: 8295
	public GameObject grabbedVFX;

	// Token: 0x04002068 RID: 8296
	public GameObject locationLockVFX;

	// Token: 0x04002069 RID: 8297
	public GameObject recycleVFX;

	// Token: 0x0400206A RID: 8298
	public GameObject tooHeavyVFX;
}
