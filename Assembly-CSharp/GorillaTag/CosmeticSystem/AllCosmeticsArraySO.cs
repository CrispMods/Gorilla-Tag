using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BEB RID: 3051
	public class AllCosmeticsArraySO : ScriptableObject
	{
		// Token: 0x06004CB4 RID: 19636 RVA: 0x001751CC File Offset: 0x001733CC
		public CosmeticSO SearchForCosmeticSO(string playfabId)
		{
			GTDirectAssetRef<CosmeticSO>[] array = this.sturdyAssetRefs;
			for (int i = 0; i < array.Length; i++)
			{
				CosmeticSO cosmeticSO = array[i];
				if (cosmeticSO.info.playFabID == playfabId)
				{
					return cosmeticSO;
				}
			}
			Debug.LogWarning("AllCosmeticsArraySO - SearchForCosmeticSO - No Cosmetic found with playfabId: " + playfabId, this);
			return null;
		}

		// Token: 0x04004E83 RID: 20099
		[SerializeField]
		public GTDirectAssetRef<CosmeticSO>[] sturdyAssetRefs;
	}
}
