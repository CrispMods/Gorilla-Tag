using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004E7 RID: 1255
public class BuilderRoom : MonoBehaviour
{
	// Token: 0x04002215 RID: 8725
	public List<GameObject> disableColliderRoots;

	// Token: 0x04002216 RID: 8726
	public List<GameObject> disableRenderRoots;

	// Token: 0x04002217 RID: 8727
	public List<GameObject> disableGameObjectsForScene;

	// Token: 0x04002218 RID: 8728
	public List<GameObject> disableObjectsForPersistent;

	// Token: 0x04002219 RID: 8729
	public List<MeshRenderer> disabledRenderersForPersistent;

	// Token: 0x0400221A RID: 8730
	public List<Collider> disabledCollidersForScene;
}
