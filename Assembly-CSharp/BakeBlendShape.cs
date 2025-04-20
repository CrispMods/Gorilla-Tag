using System;
using UnityEngine;

// Token: 0x020000C5 RID: 197
public class BakeBlendShape : MonoBehaviour
{
	// Token: 0x06000518 RID: 1304 RVA: 0x00080490 File Offset: 0x0007E690
	private void Update()
	{
		Mesh mesh = new Mesh();
		MeshCollider component = base.GetComponent<MeshCollider>();
		base.GetComponent<SkinnedMeshRenderer>().BakeMesh(mesh);
		component.sharedMesh = null;
		component.sharedMesh = mesh;
	}
}
