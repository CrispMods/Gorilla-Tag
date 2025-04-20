using System;
using System.IO;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B37 RID: 2871
	[Serializable]
	public class StoreItem
	{
		// Token: 0x060047ED RID: 18413 RVA: 0x0018C0A8 File Offset: 0x0018A2A8
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

		// Token: 0x060047EE RID: 18414 RVA: 0x0018C0FC File Offset: 0x0018A2FC
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

		// Token: 0x0400491C RID: 18716
		public string itemName = "";

		// Token: 0x0400491D RID: 18717
		public int itemCategory;

		// Token: 0x0400491E RID: 18718
		public string itemPictureResourceString = "";

		// Token: 0x0400491F RID: 18719
		public string displayName = "";

		// Token: 0x04004920 RID: 18720
		public string overrideDisplayName = "";

		// Token: 0x04004921 RID: 18721
		public string[] bundledItems = new string[0];

		// Token: 0x04004922 RID: 18722
		public bool canTryOn;

		// Token: 0x04004923 RID: 18723
		public bool bothHandsHoldable;

		// Token: 0x04004924 RID: 18724
		public string AssetBundleName = "";

		// Token: 0x04004925 RID: 18725
		public bool bUsesMeshAtlas;

		// Token: 0x04004926 RID: 18726
		public string MeshAtlasResourceName = "";

		// Token: 0x04004927 RID: 18727
		public string MeshResourceName = "";

		// Token: 0x04004928 RID: 18728
		public string MaterialResrouceName = "";

		// Token: 0x04004929 RID: 18729
		public Vector3 translationOffset = Vector3.zero;

		// Token: 0x0400492A RID: 18730
		public Vector3 rotationOffset = Vector3.zero;

		// Token: 0x0400492B RID: 18731
		public Vector3 scale = Vector3.one;
	}
}
