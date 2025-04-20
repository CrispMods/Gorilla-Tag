using System;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000CBC RID: 3260
	[CreateAssetMenu(fileName = "New Mesh Material Replacement", menuName = "Game Object Scheduling/New Mesh Material Replacement", order = 1)]
	public class MeshMaterialReplacement : ScriptableObject
	{
		// Token: 0x04005464 RID: 21604
		public Mesh mesh;

		// Token: 0x04005465 RID: 21605
		public Material[] materials;
	}
}
