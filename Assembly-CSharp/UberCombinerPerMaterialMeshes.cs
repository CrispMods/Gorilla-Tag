using System;
using UnityEngine;

// Token: 0x020008F0 RID: 2288
public class UberCombinerPerMaterialMeshes : MonoBehaviour
{
	// Token: 0x040039B5 RID: 14773
	public GameObject rootObject;

	// Token: 0x040039B6 RID: 14774
	public bool deleteSelfOnPrefabBake;

	// Token: 0x040039B7 RID: 14775
	[Space]
	public GameObject[] objects = new GameObject[0];

	// Token: 0x040039B8 RID: 14776
	public MeshRenderer[] renderers = new MeshRenderer[0];

	// Token: 0x040039B9 RID: 14777
	public MeshFilter[] filters = new MeshFilter[0];

	// Token: 0x040039BA RID: 14778
	public Material[] materials = new Material[0];
}
