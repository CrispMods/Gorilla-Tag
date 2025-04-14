using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BF7 RID: 3063
	[CreateAssetMenu(fileName = "Untitled_CosmeticSO", menuName = "- Gorilla Tag/CosmeticSO", order = 0)]
	public class CosmeticSO : ScriptableObject
	{
		// Token: 0x06004CCD RID: 19661 RVA: 0x00175A4F File Offset: 0x00173C4F
		public void OnEnable()
		{
			this.info.debugCosmeticSOName = base.name;
		}

		// Token: 0x04004EC3 RID: 20163
		public CosmeticInfoV2 info = new CosmeticInfoV2("UNNAMED");
	}
}
