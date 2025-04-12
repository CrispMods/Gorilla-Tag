using System;
using UnityEngine;

// Token: 0x020004AD RID: 1197
[CreateAssetMenu(fileName = "BuilderPieceEffectInfo", menuName = "Gorilla Tag/Builder/EffectInfo", order = 0)]
public class BuilderPieceEffectInfo : ScriptableObject
{
	// Token: 0x04002017 RID: 8215
	public GameObject placeVFX;

	// Token: 0x04002018 RID: 8216
	public GameObject disconnectVFX;

	// Token: 0x04002019 RID: 8217
	public GameObject grabbedVFX;

	// Token: 0x0400201A RID: 8218
	public GameObject locationLockVFX;

	// Token: 0x0400201B RID: 8219
	public GameObject recycleVFX;

	// Token: 0x0400201C RID: 8220
	public GameObject tooHeavyVFX;
}
