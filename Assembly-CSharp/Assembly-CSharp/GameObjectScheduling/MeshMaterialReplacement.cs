using System;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000C8E RID: 3214
	[CreateAssetMenu(fileName = "New Mesh Material Replacement", menuName = "Game Object Scheduling/New Mesh Material Replacement", order = 1)]
	public class MeshMaterialReplacement : ScriptableObject
	{
		// Token: 0x0400536A RID: 21354
		public Mesh mesh;

		// Token: 0x0400536B RID: 21355
		public Material[] materials;
	}
}
