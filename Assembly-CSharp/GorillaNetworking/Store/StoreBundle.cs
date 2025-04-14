using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaNetworking.Store
{
	// Token: 0x02000AFB RID: 2811
	[Serializable]
	public class StoreBundle
	{
		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06004642 RID: 17986 RVA: 0x0014D50F File Offset: 0x0014B70F
		public string playfabBundleID
		{
			get
			{
				return this._storeBundleDataReference.playfabBundleID;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06004643 RID: 17987 RVA: 0x0014D51C File Offset: 0x0014B71C
		public string bundleSKU
		{
			get
			{
				return this._storeBundleDataReference.bundleSKU;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06004644 RID: 17988 RVA: 0x0014D529 File Offset: 0x0014B729
		public Sprite bundleImage
		{
			get
			{
				return this._storeBundleDataReference.bundleImage;
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06004645 RID: 17989 RVA: 0x0014D536 File Offset: 0x0014B736
		public string price
		{
			get
			{
				return this._price;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06004646 RID: 17990 RVA: 0x0014D540 File Offset: 0x0014B740
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

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06004647 RID: 17991 RVA: 0x0014D5EC File Offset: 0x0014B7EC
		public bool HasPrice
		{
			get
			{
				return !string.IsNullOrEmpty(this.price) && this.price != StoreBundle.defaultPrice;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06004648 RID: 17992 RVA: 0x0014D60D File Offset: 0x0014B80D
		public string bundleDescriptionText
		{
			get
			{
				return this._storeBundleDataReference.bundleDescriptionText;
			}
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x0014D61C File Offset: 0x0014B81C
		public StoreBundle()
		{
			this.isOwned = false;
			this.bundleStands = new List<BundleStand>();
		}

		// Token: 0x0600464A RID: 17994 RVA: 0x0014D670 File Offset: 0x0014B870
		public StoreBundle(StoreBundleData data)
		{
			this.isOwned = false;
			this.bundleStands = new List<BundleStand>();
			this._storeBundleDataReference = data;
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x0014D6C8 File Offset: 0x0014B8C8
		public void InitializebundleStands()
		{
			foreach (BundleStand bundleStand in this.bundleStands)
			{
				bundleStand.UpdateDescriptionText(this.bundleDescriptionText);
				bundleStand.InitializeEventListeners();
			}
		}

		// Token: 0x0600464C RID: 17996 RVA: 0x0014D724 File Offset: 0x0014B924
		public void TryUpdatePrice(uint bundlePrice)
		{
			this.TryUpdatePrice((bundlePrice / 100m).ToString());
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x0014D754 File Offset: 0x0014B954
		public void TryUpdatePrice(string bundlePrice = null)
		{
			if (!string.IsNullOrEmpty(bundlePrice))
			{
				decimal num;
				this._price = (decimal.TryParse(bundlePrice, out num) ? (StoreBundle.defaultCurrencySymbol + bundlePrice) : bundlePrice);
			}
			this.UpdatePurchaseButtonText();
		}

		// Token: 0x0600464E RID: 17998 RVA: 0x0014D790 File Offset: 0x0014B990
		public void UpdatePurchaseButtonText()
		{
			this.purchaseButtonText = string.Format(this.purchaseButtonStringFormat, this.bundleName, this.price);
			foreach (BundleStand bundleStand in this.bundleStands)
			{
				bundleStand.UpdatePurchaseButtonText(this.purchaseButtonText);
			}
		}

		// Token: 0x0600464F RID: 17999 RVA: 0x0014D804 File Offset: 0x0014BA04
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

		// Token: 0x040047D0 RID: 18384
		private static readonly string defaultPrice = "$--.--";

		// Token: 0x040047D1 RID: 18385
		private static readonly string defaultCurrencySymbol = "$";

		// Token: 0x040047D2 RID: 18386
		[NonSerialized]
		public string purchaseButtonStringFormat = "THE {0}\n{1}";

		// Token: 0x040047D3 RID: 18387
		[SerializeField]
		public List<BundleStand> bundleStands;

		// Token: 0x040047D4 RID: 18388
		public bool isOwned;

		// Token: 0x040047D5 RID: 18389
		private string _price = StoreBundle.defaultPrice;

		// Token: 0x040047D6 RID: 18390
		private string _bundleName = "";

		// Token: 0x040047D7 RID: 18391
		public string purchaseButtonText = "";

		// Token: 0x040047D8 RID: 18392
		[FormerlySerializedAs("storeBundleDataReference")]
		[SerializeField]
		[ReadOnly]
		private StoreBundleData _storeBundleDataReference;
	}
}
