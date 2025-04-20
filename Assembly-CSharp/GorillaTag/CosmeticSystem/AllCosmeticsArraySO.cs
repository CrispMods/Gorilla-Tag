using System;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000C19 RID: 3097
	public class AllCosmeticsArraySO : ScriptableObject
	{
		// Token: 0x06004E00 RID: 19968 RVA: 0x001AD7C0 File Offset: 0x001AB9C0
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

		// Token: 0x04004F79 RID: 20345
		[SerializeField]
		public GTDirectAssetRef<CosmeticSO>[] sturdyAssetRefs;
	}
}
