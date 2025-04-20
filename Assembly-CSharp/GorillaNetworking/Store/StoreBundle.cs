using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B28 RID: 2856
	[Serializable]
	public class StoreBundle
	{
		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x0600478A RID: 18314 RVA: 0x0005E988 File Offset: 0x0005CB88
		public string playfabBundleID
		{
			get
			{
				return this._storeBundleDataReference.playfabBundleID;
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x0600478B RID: 18315 RVA: 0x0005E995 File Offset: 0x0005CB95
		public string bundleSKU
		{
			get
			{
				return this._storeBundleDataReference.bundleSKU;
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x0600478C RID: 18316 RVA: 0x0005E9A2 File Offset: 0x0005CBA2
		public Sprite bundleImage
		{
			get
			{
				return this._storeBundleDataReference.bundleImage;
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x0600478D RID: 18317 RVA: 0x0005E9AF File Offset: 0x0005CBAF
		public string price
		{
			get
			{
				return this._price;
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x0600478E RID: 18318 RVA: 0x0018A304 File Offset: 0x00188504
		public string bundleName
		{
			get
			{
				if (this._bundleName.IsNullOrEmpty())
				{
					int num = CosmeticsController.instance.allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => this.playfabBundleID == x.itemName);
					if (num > -1)
					{
						if (!CosmeticsController.instance.allCosmetics[num].overrideDisplayName.IsNullOrEmpty())
						{
							this._bundleName = CosmeticsController.instance.allCosmetics[num].overrideDisplayName;
						}
						else
						{
							this._bundleName = CosmeticsController.instance.allCosmetics[num].displayName;
						}
					}
					else
					{
						this._bundleName = "NULL_BUNDLE_NAME";
					}
				}
				return this._bundleName;
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x0600478F RID: 18319 RVA: 0x0005E9B7 File Offset: 0x0005CBB7
		public bool HasPrice
		{
			get
			{
				return !string.IsNullOrEmpty(this.price) && this.price != StoreBundle.defaultPrice;
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06004790 RID: 18320 RVA: 0x0005E9D8 File Offset: 0x0005CBD8
		public string bundleDescriptionText
		{
			get
			{
				return this._storeBundleDataReference.bundleDescriptionText;
			}
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x0018A3B0 File Offset: 0x001885B0
		public StoreBundle()
		{
			this.isOwned = false;
			this.bundleStands = new List<BundleStand>();
		}

		// Token: 0x06004792 RID: 18322 RVA: 0x0018A404 File Offset: 0x00188604
		public StoreBundle(StoreBundleData data)
		{
			this.isOwned = false;
			this.bundleStands = new List<BundleStand>();
			this._storeBundleDataReference = data;
		}

		// Token: 0x06004793 RID: 18323 RVA: 0x0018A45C File Offset: 0x0018865C
		public void InitializebundleStands()
		{
			foreach (BundleStand bundleStand in this.bundleStands)
			{
				bundleStand.UpdateDescriptionText(this.bundleDescriptionText);
				bundleStand.InitializeEventListeners();
			}
		}

		// Token: 0x06004794 RID: 18324 RVA: 0x0018A4B8 File Offset: 0x001886B8
		public void TryUpdatePrice(uint bundlePrice)
		{
			this.TryUpdatePrice((bundlePrice / 100m).ToString());
		}

		// Token: 0x06004795 RID: 18325 RVA: 0x0018A4E8 File Offset: 0x001886E8
		public void TryUpdatePrice(string bundlePrice = null)
		{
			if (!string.IsNullOrEmpty(bundlePrice))
			{
				decimal num;
				this._price = (decimal.TryParse(bundlePrice, out num) ? (StoreBundle.defaultCurrencySymbol + bundlePrice) : bundlePrice);
			}
			this.UpdatePurchaseButtonText();
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x0018A524 File Offset: 0x00188724
		public void UpdatePurchaseButtonText()
		{
			this.purchaseButtonText = string.Format(this.purchaseButtonStringFormat, this.bundleName, this.price);
			foreach (BundleStand bundleStand in this.bundleStands)
			{
				bundleStand.UpdatePurchaseButtonText(this.purchaseButtonText);
			}
		}

		// Token: 0x06004797 RID: 18327 RVA: 0x0018A598 File Offset: 0x00188798
		public void ValidateBundleData()
		{
			if (this._storeBundleDataReference == null)
			{
				Debug.LogError("StoreBundleData is null");
				foreach (BundleStand bundleStand in this.bundleStands)
				{
					if (bundleStand == null)
					{
						Debug.LogError("BundleStand is null");
					}
					else if (bundleStand._bundleDataReference != null)
					{
						this._storeBundleDataReference = bundleStand._bundleDataReference;
						Debug.LogError("BundleStand StoreBundleData is not equal to StoreBundle StoreBundleData");
					}
				}
			}
			if (this._storeBundleDataReference == null)
			{
				Debug.LogError("StoreBundleData is null");
				return;
			}
			if (this._storeBundleDataReference.playfabBundleID.IsNullOrEmpty())
			{
				Debug.LogError("playfabBundleID is null");
			}
			if (this._storeBundleDataReference.bundleSKU.IsNullOrEmpty())
			{
				Debug.LogError("bundleSKU is null");
			}
			if (this._storeBundleDataReference.bundleImage == null)
			{
				Debug.LogError("bundleImage is null");
			}
			if (this._storeBundleDataReference.bundleDescriptionText.IsNullOrEmpty())
			{
				Debug.LogError("bundleDescriptionText is null");
			}
		}

		// Token: 0x040048C5 RID: 18629
		private static readonly string defaultPrice = "$--.--";

		// Token: 0x040048C6 RID: 18630
		private static readonly string defaultCurrencySymbol = "$";

		// Token: 0x040048C7 RID: 18631
		[NonSerialized]
		public string purchaseButtonStringFormat = "THE {0}\n{1}";

		// Token: 0x040048C8 RID: 18632
		[SerializeField]
		public List<BundleStand> bundleStands;

		// Token: 0x040048C9 RID: 18633
		public bool isOwned;

		// Token: 0x040048CA RID: 18634
		private string _price = StoreBundle.defaultPrice;

		// Token: 0x040048CB RID: 18635
		private string _bundleName = "";

		// Token: 0x040048CC RID: 18636
		public string purchaseButtonText = "";

		// Token: 0x040048CD RID: 18637
		[FormerlySerializedAs("storeBundleDataReference")]
		[SerializeField]
		[ReadOnly]
		private StoreBundleData _storeBundleDataReference;
	}
}
