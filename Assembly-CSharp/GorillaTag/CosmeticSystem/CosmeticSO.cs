using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000C22 RID: 3106
	[CreateAssetMenu(fileName = "Untitled_CosmeticSO", menuName = "- Gorilla Tag/CosmeticSO", order = 0)]
	public class CosmeticSO : ScriptableObject
	{
		// Token: 0x06004E0D RID: 19981 RVA: 0x0006310F File Offset: 0x0006130F
		public void OnEnable()
		{
			this.info.debugCosmeticSOName = base.name;
		}

		// Token: 0x04004FA7 RID: 20391
		public CosmeticInfoV2 info = new CosmeticInfoV2("UNNAMED");
	}
}
