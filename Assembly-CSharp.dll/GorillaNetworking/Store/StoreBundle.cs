using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaNetworking.Store
{
	// Token: 0x02000AFE RID: 2814
	[Serializable]
	public class StoreBundle
	{
		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x0600464E RID: 17998 RVA: 0x0005CF89 File Offset: 0x0005B189
		public string playfabBundleID
		{
			get
			{
				return this._storeBundleDataReference.playfabBundleID;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x0600464F RID: 17999 RVA: 0x0005CF96 File Offset: 0x0005B196
		public string bundleSKU
		{
			get
			{
				return this._storeBundleDataReference.bundleSKU;
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06004650 RID: 18000 RVA: 0x0005CFA3 File Offset: 0x0005B1A3
		public Sprite bundleImage
		{
			get
			{
				return this._storeBundleDataReference.bundleImage;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06004651 RID: 18001 RVA: 0x0005CFB0 File Offset: 0x0005B1B0
		public string price
		{
			get
			{
				return this._price;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06004652 RID: 18002 RVA: 0x0018340C File Offset: 0x0018160C
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

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06004653 RID: 18003 RVA: 0x0005CFB8 File Offset: 0x0005B1B8
		public bool HasPrice
		{
			get
			{
				return !string.IsNullOrEmpty(this.price) && this.price != StoreBundle.defaultPrice;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06004654 RID: 18004 RVA: 0x0005CFD9 File Offset: 0x0005B1D9
		public string bundleDescriptionText
		{
			get
			{
				return this._storeBundleDataReference.bundleDescriptionText;
			}
		}

		// Token: 0x06004655 RID: 18005 RVA: 0x001834B8 File Offset: 0x001816B8
		public StoreBundle()
		{
			this.isOwned = false;
			this.bundleStands = new List<BundleStand>();
		}

		// Token: 0x06004656 RID: 18006 RVA: 0x0018350C File Offset: 0x0018170C
		public StoreBundle(StoreBundleData data)
		{
			this.isOwned = false;
			this.bundleStands = new List<BundleStand>();
			this._storeBundleDataReference = data;
		}

		// Token: 0x06004657 RID: 18007 RVA: 0x00183564 File Offset: 0x00181764
		public void InitializebundleStands()
		{
			foreach (BundleStand bundleStand in this.bundleStands)
			{
				bundleStand.UpdateDescriptionText(this.bundleDescriptionText);
				bundleStand.InitializeEventListeners();
			}
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x001835C0 File Offset: 0x001817C0
		public void TryUpdatePrice(uint bundlePrice)
		{
			this.TryUpdatePrice((bundlePrice / 100m).ToString());
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x001835F0 File Offset: 0x001817F0
		public void TryUpdatePrice(string bundlePrice = null)
		{
			if (!string.IsNullOrEmpty(bundlePrice))
			{
				decimal num;
				this._price = (decimal.TryParse(bundlePrice, out num) ? (StoreBundle.defaultCurrencySymbol + bundlePrice) : bundlePrice);
			}
			this.UpdatePurchaseButtonText();
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x0018362C File Offset: 0x0018182C
		public void UpdatePurchaseButtonText()
		{
			this.purchaseButtonText = string.Format(this.purchaseButtonStringFormat, this.bundleName, this.price);
			foreach (BundleStand bundleStand in this.bundleStands)
			{
				bundleStand.UpdatePurchaseButtonText(this.purchaseButtonText);
			}
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x001836A0 File Offset: 0x001818A0
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

		// Token: 0x040047E2 RID: 18402
		private static readonly string defaultPrice = "$--.--";

		// Token: 0x040047E3 RID: 18403
		private static readonly string defaultCurrencySymbol = "$";

		// Token: 0x040047E4 RID: 18404
		[NonSerialized]
		public string purchaseButtonStringFormat = "THE {0}\n{1}";

		// Token: 0x040047E5 RID: 18405
		[SerializeField]
		public List<BundleStand> bundleStands;

		// Token: 0x040047E6 RID: 18406
		public bool isOwned;

		// Token: 0x040047E7 RID: 18407
		private string _price = StoreBundle.defaultPrice;

		// Token: 0x040047E8 RID: 18408
		private string _bundleName = "";

		// Token: 0x040047E9 RID: 18409
		public string purchaseButtonText = "";

		// Token: 0x040047EA RID: 18410
		[FormerlySerializedAs("storeBundleDataReference")]
		[SerializeField]
		[ReadOnly]
		private StoreBundleData _storeBundleDataReference;
	}
}
