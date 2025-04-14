using System;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000C8B RID: 3211
	[CreateAssetMenu(fileName = "New Mesh Material Replacement", menuName = "Game Object Scheduling/New Mesh Material Replacement", order = 1)]
	public class MeshMaterialReplacement : ScriptableObject
	{
		// Token: 0x04005358 RID: 21336
		public Mesh mesh;

		// Token: 0x04005359 RID: 21337
		public Material[] materials;
	}
}
