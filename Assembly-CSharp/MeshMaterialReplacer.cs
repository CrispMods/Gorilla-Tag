using System;
using GameObjectScheduling;
using UnityEngine;

// Token: 0x02000254 RID: 596
public class MeshMaterialReplacer : MonoBehaviour
{
	// Token: 0x06000DCC RID: 3532 RVA: 0x000A2B3C File Offset: 0x000A0D3C
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

	// Token: 0x040010EF RID: 4335
	[SerializeField]
	private MeshMaterialReplacement meshMaterialReplacement;
}
