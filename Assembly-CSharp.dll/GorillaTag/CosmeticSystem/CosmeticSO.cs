using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BF7 RID: 3063
	[CreateAssetMenu(fileName = "Untitled_CosmeticSO", menuName = "- Gorilla Tag/CosmeticSO", order = 0)]
	public class CosmeticSO : ScriptableObject
	{
		// Token: 0x06004CCD RID: 19661 RVA: 0x0006174E File Offset: 0x0005F94E
		public void OnEnable()
		{
			this.info.debugCosmeticSOName = base.name;
		}

		// Token: 0x04004EC3 RID: 20163
		public CosmeticInfoV2 info = new CosmeticInfoV2("UNNAMED");
	}
}
