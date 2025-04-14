using System;
using GameObjectScheduling;
using UnityEngine;

// Token: 0x02000249 RID: 585
public class MeshMaterialReplacer : MonoBehaviour
{
	// Token: 0x06000D81 RID: 3457 RVA: 0x000457B4 File Offset: 0x000439B4
	private void Start()
	{
		MeshRenderer meshRenderer;
		if (base.TryGetComponent<MeshRenderer>(out meshRenderer))
		{
			base.GetComponent<MeshFilter>().mesh = this.meshMaterialReplacement.mesh;
			meshRenderer.materials = this.meshMaterialReplacement.materials;
			return;
		}
		SkinnedMeshRenderer skinnedMeshRenderer;
		if (base.TryGetComponent<SkinnedMeshRenderer>(out skinnedMeshRenderer))
		{
			skinnedMeshRenderer.sharedMesh = this.meshMaterialReplacement.mesh;
			skinnedMeshRenderer.materials = this.meshMaterialReplacement.materials;
		}
	}

	// Token: 0x040010A9 RID: 4265
	[SerializeField]
	private MeshMaterialReplacement meshMaterialReplacement;
}
