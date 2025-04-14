using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BF4 RID: 3060
	[CreateAssetMenu(fileName = "Untitled_CosmeticSO", menuName = "- Gorilla Tag/CosmeticSO", order = 0)]
	public class CosmeticSO : ScriptableObject
	{
		// Token: 0x06004CC1 RID: 19649 RVA: 0x00175487 File Offset: 0x00173687
		public void OnEnable()
		{
			this.info.debugCosmeticSOName = base.name;
		}

		// Token: 0x04004EB1 RID: 20145
		public CosmeticInfoV2 info = new CosmeticInfoV2("UNNAMED");
	}
}
