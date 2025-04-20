using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004F4 RID: 1268
public class BuilderRoom : MonoBehaviour
{
	// Token: 0x04002268 RID: 8808
	public List<GameObject> disableColliderRoots;

	// Token: 0x04002269 RID: 8809
	public List<GameObject> disableRenderRoots;

	// Token: 0x0400226A RID: 8810
	public List<GameObject> disableGameObjectsForScene;

	// Token: 0x0400226B RID: 8811
	public List<GameObject> disableObjectsForPersistent;

	// Token: 0x0400226C RID: 8812
	public List<MeshRenderer> disabledRenderersForPersistent;

	// Token: 0x0400226D RID: 8813
	public List<Collider> disabledCollidersForScene;
}
