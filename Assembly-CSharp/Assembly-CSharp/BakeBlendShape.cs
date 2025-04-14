using System;
using UnityEngine;

// Token: 0x020000BB RID: 187
public class BakeBlendShape : MonoBehaviour
{
	// Token: 0x060004DE RID: 1246 RVA: 0x0001D3F0 File Offset: 0x0001B5F0
	private void Update()
	{
		Mesh mesh = new Mesh();
		MeshCollider component = base.GetComponent<MeshCollider>();
		base.GetComponent<SkinnedMeshRenderer>().BakeMesh(mesh);
		component.sharedMesh = null;
		component.sharedMesh = mesh;
	}
}
