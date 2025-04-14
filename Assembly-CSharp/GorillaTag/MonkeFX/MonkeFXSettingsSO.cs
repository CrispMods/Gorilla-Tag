using System;
using UnityEngine;

namespace GorillaTag.MonkeFX
{
	// Token: 0x02000BCE RID: 3022
	[CreateAssetMenu(fileName = "MeshGenerator", menuName = "ScriptableObjects/MeshGenerator", order = 1)]
	public class MonkeFXSettingsSO : ScriptableObject
	{
		// Token: 0x06004C41 RID: 19521 RVA: 0x00173596 File Offset: 0x00171796
		protected void Awake()
		{
			MonkeFX.Register(this);
		}

		// Token: 0x04004E2D RID: 20013
		public GTDirectAssetRef<Mesh>[] sourceMeshes;

		// Token: 0x04004E2E RID: 20014
		[HideInInspector]
		public Mesh combinedMesh;
	}
}
