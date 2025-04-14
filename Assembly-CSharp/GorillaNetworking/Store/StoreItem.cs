using System;
using System.IO;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B0A RID: 2826
	[Serializable]
	public class StoreItem
	{
		// Token: 0x060046A4 RID: 18084 RVA: 0x0014F608 File Offset: 0x0014D808
		public static void SerializeItemsAsJSON(StoreItem[] items)
		{
			string text = "";
			foreach (StoreItem obj in items)
			{
				text = text + JsonUtility.ToJson(obj) + ";";
			}
			Debug.LogError(text);
			File.WriteAllText(Application.dataPath + "/Resources/StoreItems/FeaturedStoreItemsList.json", text);
		}

		// Token: 0x060046A5 RID: 18085 RVA: 0x0014F65C File Offset: 0x0014D85C
		public static void ConvertCosmeticItemToSToreItem(CosmeticsController.CosmeticItem cosmeticItem, ref StoreItem storeItem)
		{
			storeItem.itemName = cosmeticItem.itemName;
			storeItem.itemCategory = (int)cosmeticItem.itemCategory;
			storeItem.itemPictureResourceString = cosmeticItem.itemPictureResourceString;
			storeItem.displayName = cosmeticItem.displayName;
			storeItem.overrideDisplayName = cosmeticItem.overrideDisplayName;
			storeItem.bundledItems = cosmeticItem.bundledItems;
			storeItem.canTryOn = cosmeticItem.canTryOn;
			storeItem.bothHandsHoldable = cosmeticItem.bothHandsHoldable;
			storeItem.AssetBundleName = "";
			storeItem.bUsesMeshAtlas = cosmeticItem.bUsesMeshAtlas;
			storeItem.MeshResourceName = cosmeticItem.meshResourceString;
			storeItem.MeshAtlasResourceName = cosmeticItem.meshAtlasResourceString;
			storeItem.MaterialResrouceName = cosmeticItem.materialResourceString;
		}

		// Token: 0x04004827 RID: 18471
		public string itemName = "";

		// Token: 0x04004828 RID: 18472
		public int itemCategory;

		// Token: 0x04004829 RID: 18473
		public string itemPictureResourceString = "";

		// Token: 0x0400482A RID: 18474
		public string displayName = "";

		// Token: 0x0400482B RID: 18475
		public string overrideDisplayName = "";

		// Token: 0x0400482C RID: 18476
		public string[] bundledItems = new string[0];

		// Token: 0x0400482D RID: 18477
		public bool canTryOn;

		// Token: 0x0400482E RID: 18478
		public bool bothHandsHoldable;

		// Token: 0x0400482F RID: 18479
		public string AssetBundleName = "";

		// Token: 0x04004830 RID: 18480
		public bool bUsesMeshAtlas;

		// Token: 0x04004831 RID: 18481
		public string MeshAtlasResourceName = "";

		// Token: 0x04004832 RID: 18482
		public string MeshResourceName = "";

		// Token: 0x04004833 RID: 18483
		public string MaterialResrouceName = "";

		// Token: 0x04004834 RID: 18484
		public Vector3 translationOffset = Vector3.zero;

		// Token: 0x04004835 RID: 18485
		public Vector3 rotationOffset = Vector3.zero;

		// Token: 0x04004836 RID: 18486
		public Vector3 scale = Vector3.one;
	}
}
