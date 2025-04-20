using System;
using UnityEngine;

namespace GorillaTag.MonkeFX
{
	// Token: 0x02000BFC RID: 3068
	[CreateAssetMenu(fileName = "MeshGenerator", menuName = "ScriptableObjects/MeshGenerator", order = 1)]
	public class MonkeFXSettingsSO : ScriptableObject
	{
		// Token: 0x06004D8D RID: 19853 RVA: 0x00062D8F File Offset: 0x00060F8F
		protected void Awake()
		{
			MonkeFX.Register(this);
		}

		// Token: 0x04004F23 RID: 20259
		public GTDirectAssetRef<Mesh>[] sourceMeshes;

		// Token: 0x04004F24 RID: 20260
		[HideInInspector]
		public Mesh combinedMesh;
	}
}
