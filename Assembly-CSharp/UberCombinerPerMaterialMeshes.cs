using System;
using UnityEngine;

// Token: 0x020008D4 RID: 2260
public class UberCombinerPerMaterialMeshes : MonoBehaviour
{
	// Token: 0x040038F4 RID: 14580
	public GameObject rootObject;

	// Token: 0x040038F5 RID: 14581
	public bool deleteSelfOnPrefabBake;

	// Token: 0x040038F6 RID: 14582
	[Space]
	public GameObject[] objects = new GameObject[0];

	// Token: 0x040038F7 RID: 14583
	public MeshRenderer[] renderers = new MeshRenderer[0];

	// Token: 0x040038F8 RID: 14584
	public MeshFilter[] filters = new MeshFilter[0];

	// Token: 0x040038F9 RID: 14585
	public Material[] materials = new Material[0];
}
