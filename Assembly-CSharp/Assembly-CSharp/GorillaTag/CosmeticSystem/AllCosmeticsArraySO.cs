using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BEE RID: 3054
	public class AllCosmeticsArraySO : ScriptableObject
	{
		// Token: 0x06004CC0 RID: 19648 RVA: 0x00175794 File Offset: 0x00173994
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

		// Token: 0x04004E95 RID: 20117
		[SerializeField]
		public GTDirectAssetRef<CosmeticSO>[] sturdyAssetRefs;
	}
}
