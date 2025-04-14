using System;
using UnityEngine;

// Token: 0x020008D7 RID: 2263
public class UberCombinerPerMaterialMeshes : MonoBehaviour
{
	// Token: 0x04003906 RID: 14598
	public GameObject rootObject;

	// Token: 0x04003907 RID: 14599
	public bool deleteSelfOnPrefabBake;

	// Token: 0x04003908 RID: 14600
	[Space]
	public GameObject[] objects = new GameObject[0];

	// Token: 0x04003909 RID: 14601
	public MeshRenderer[] renderers = new MeshRenderer[0];

	// Token: 0x0400390A RID: 14602
	public MeshFilter[] filters = new MeshFilter[0];

	// Token: 0x0400390B RID: 14603
	public Material[] materials = new Material[0];
}
