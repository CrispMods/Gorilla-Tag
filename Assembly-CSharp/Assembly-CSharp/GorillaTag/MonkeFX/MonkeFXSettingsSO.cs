using System;
using UnityEngine;

namespace GorillaTag.MonkeFX
{
	// Token: 0x02000BD1 RID: 3025
	[CreateAssetMenu(fileName = "MeshGenerator", menuName = "ScriptableObjects/MeshGenerator", order = 1)]
	public class MonkeFXSettingsSO : ScriptableObject
	{
		// Token: 0x06004C4D RID: 19533 RVA: 0x00173B5E File Offset: 0x00171D5E
		protected void Awake()
		{
			MonkeFX.Register(this);
		}

		// Token: 0x04004E3F RID: 20031
		public GTDirectAssetRef<Mesh>[] sourceMeshes;

		// Token: 0x04004E40 RID: 20032
		[HideInInspector]
		public Mesh combinedMesh;
	}
}
