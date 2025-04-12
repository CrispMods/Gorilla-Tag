using System;
using System.IO;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B0D RID: 2829
	[Serializable]
	public class StoreItem
	{
		// Token: 0x060046B0 RID: 18096 RVA: 0x00185134 File Offset: 0x00183334
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

		// Token: 0x060046B1 RID: 18097 RVA: 0x00185188 File Offset: 0x00183388
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

		// Token: 0x04004839 RID: 18489
		public string itemName = "";

		// Token: 0x0400483A RID: 18490
		public int itemCategory;

		// Token: 0x0400483B RID: 18491
		public string itemPictureResourceString = "";

		// Token: 0x0400483C RID: 18492
		public string displayName = "";

		// Token: 0x0400483D RID: 18493
		public string overrideDisplayName = "";

		// Token: 0x0400483E RID: 18494
		public string[] bundledItems = new string[0];

		// Token: 0x0400483F RID: 18495
		public bool canTryOn;

		// Token: 0x04004840 RID: 18496
		public bool bothHandsHoldable;

		// Token: 0x04004841 RID: 18497
		public string AssetBundleName = "";

		// Token: 0x04004842 RID: 18498
		public bool bUsesMeshAtlas;

		// Token: 0x04004843 RID: 18499
		public string MeshAtlasResourceName = "";

		// Token: 0x04004844 RID: 18500
		public string MeshResourceName = "";

		// Token: 0x04004845 RID: 18501
		public string MaterialResrouceName = "";

		// Token: 0x04004846 RID: 18502
		public Vector3 translationOffset = Vector3.zero;

		// Token: 0x04004847 RID: 18503
		public Vector3 rotationOffset = Vector3.zero;

		// Token: 0x04004848 RID: 18504
		public Vector3 scale = Vector3.one;
	}
}
