using System;
using GameObjectScheduling;
using UnityEngine;

// Token: 0x02000249 RID: 585
public class MeshMaterialReplacer : MonoBehaviour
{
	// Token: 0x06000D83 RID: 3459 RVA: 0x000A02B0 File Offset: 0x0009E4B0
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

	// Token: 0x040010AA RID: 4266
	[SerializeField]
	private MeshMaterialReplacement meshMaterialReplacement;
}
