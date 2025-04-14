using System;
using UnityEngine;

// Token: 0x020000BB RID: 187
public class BakeBlendShape : MonoBehaviour
{
	// Token: 0x060004DC RID: 1244 RVA: 0x0001D0CC File Offset: 0x0001B2CC
	private void Update()
	{
		Mesh mesh = new Mesh();
		MeshCollider component = base.GetComponent<MeshCollider>();
		base.GetComponent<SkinnedMeshRenderer>().BakeMesh(mesh);
		component.sharedMesh = null;
		component.sharedMesh = mesh;
	}
}
