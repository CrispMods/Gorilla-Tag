using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GorillaExtensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B20 RID: 2848
	public class BundleManager : MonoBehaviour
	{
		// Token: 0x06004750 RID: 18256 RVA: 0x0005E6B8 File Offset: 0x0005C8B8
		private IEnumerable GetStoreBundles()
		{
			List<StoreBundleData> list = new List<StoreBundleData>();
			list.Add(this.nullBundleData);
			list.AddRange(this._bundleScriptableObjects);
			return list;
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x0005E6D7 File Offset: 0x0005C8D7
		public void Awake()
		{
			if (BundleManager.instance == null)
			{
				BundleManager.instance = this;
				return;
			}
			if (BundleManager.instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
		}

		// Token: 0x06004752 RID: 18258 RVA: 0x0005E70C File Offset: 0x0005C90C
		private void Start()
		{
			this.GenerateBundleDictionaries();
			this.Initialize();
		}

		// Token: 0x06004753 RID: 18259 RVA: 0x00189700 File Offset: 0x00187900
		private void Initialize()
		{
			foreach (StoreBundle storeBundle in this._storeBundles)
			{
				storeBundle.InitializebundleStands();
			}
		}

		// Token: 0x06004754 RID: 18260 RVA: 0x00189750 File Offset: 0x00187950
		private void ValidateBundleData()
		{
			foreach (StoreBundle storeBundle in this._storeBundles)
			{
				storeBundle.ValidateBundleData();
			}
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x001897A0 File Offset: 0x001879A0
		private void SpawnBundleStands()
		{
			foreach (StoreBundle storeBundle in this._storeBundles)
			{
				foreach (BundleStand bundleStand in storeBundle.bundleStands)
				{
					if (bundleStand != null)
					{
						UnityEngine.Object.DestroyImmediate(bundleStand.gameObject);
					}
				}
			}
			this._spawnedBundleStands.Clear();
			this.storeBundlesById.Clear();
			this.storeBundlesBySKU.Clear();
			this._storeBundles.Clear();
			this._bundleScriptableObjects.Clear();
			BundleStand[] array = UnityEngine.Object.FindObjectsOfType<BundleStand>();
			for (int i = 0; i < array.Length; i++)
			{
				UnityEngine.Object.DestroyImmediate(array[i].gameObject);
			}
			for (int j = 0; j < this.BundleStands.Count; j++)
			{
				if (this.BundleStands[j].spawnLocation == null)
				{
					Debug.LogError("No spawn location set for Bundle Stand " + j.ToString());
				}
				else if (this.BundleStands[j].bundleStand == null)
				{
					Debug.LogError("No Bundle Stand set for Bundle Stand " + j.ToString());
				}
			}
			this.GenerateAllStoreBundleReferences();
			if (!this._bundleScriptableObjects.Contains(this.tryOnBundleButton1))
			{
				this.tryOnBundleButton1 = this.nullBundleData;
			}
			if (!this._bundleScriptableObjects.Contains(this.tryOnBundleButton2))
			{
				this.tryOnBundleButton2 = this.nullBundleData;
			}
			if (!this._bundleScriptableObjects.Contains(this.tryOnBundleButton3))
			{
				this.tryOnBundleButton3 = this.nullBundleData;
			}
			if (!this._bundleScriptableObjects.Contains(this.tryOnBundleButton4))
			{
				this.tryOnBundleButton4 = this.nullBundleData;
			}
			if (!this._bundleScriptableObjects.Contains(this.tryOnBundleButton5))
			{
				this.tryOnBundleButton4 = this.nullBundleData;
			}
		}

		// Token: 0x06004756 RID: 18262 RVA: 0x001899B4 File Offset: 0x00187BB4
		public void ClearEverything()
		{
			foreach (StoreBundle storeBundle in this._storeBundles)
			{
				foreach (BundleStand bundleStand in storeBundle.bundleStands)
				{
					if (bundleStand != null)
					{
						UnityEngine.Object.DestroyImmediate(bundleStand.gameObject);
					}
				}
			}
			this._spawnedBundleStands.Clear();
			this.storeBundlesById.Clear();
			this.storeBundlesBySKU.Clear();
			this._storeBundles.Clear();
			this._bundleScriptableObjects.Clear();
			this.tryOnBundleButton1 = this.nullBundleData;
			this.tryOnBundleButton2 = this.nullBundleData;
			this.tryOnBundleButton3 = this.nullBundleData;
			this.tryOnBundleButton4 = this.nullBundleData;
			this.tryOnBundleButton5 = this.nullBundleData;
			BundleStand[] array = UnityEngine.Object.FindObjectsOfType<BundleStand>();
			for (int i = 0; i < array.Length; i++)
			{
				UnityEngine.Object.DestroyImmediate(array[i].gameObject);
			}
		}

		// Token: 0x06004757 RID: 18263 RVA: 0x00030607 File Offset: 0x0002E807
		public void GenerateAllStoreBundleReferences()
		{
		}

		// Token: 0x06004758 RID: 18264 RVA: 0x00189AE8 File Offset: 0x00187CE8
		private void AddNewBundleStand(BundleStand bundleStand)
		{
			foreach (StoreBundle storeBundle in this._storeBundles)
			{
				if (storeBundle.playfabBundleID == bundleStand._bundleDataReference.playfabBundleID)
				{
					storeBundle.bundleStands.Add(bundleStand);
					return;
				}
			}
			StoreBundle storeBundle2 = new StoreBundle(bundleStand._bundleDataReference);
			storeBundle2.bundleStands.Add(bundleStand);
			this._storeBundles.Add(storeBundle2);
		}

		// Token: 0x06004759 RID: 18265 RVA: 0x00189B80 File Offset: 0x00187D80
		public void GenerateBundleDictionaries()
		{
			this.storeBundlesById.Clear();
			this.storeBundlesBySKU.Clear();
			foreach (StoreBundle storeBundle in this._storeBundles)
			{
				this.storeBundlesById.Add(storeBundle.playfabBundleID, storeBundle);
				this.storeBundlesBySKU.Add(storeBundle.bundleSKU, storeBundle);
			}
		}

		// Token: 0x0600475A RID: 18266 RVA: 0x0005E71A File Offset: 0x0005C91A
		public void BundlePurchaseButtonPressed(string playFabItemName)
		{
			CosmeticsController.instance.PurchaseBundle(this.storeBundlesById[playFabItemName]);
		}

		// Token: 0x0600475B RID: 18267 RVA: 0x00189C08 File Offset: 0x00187E08
		public void FixBundles()
		{
			this._storeBundles.Clear();
			for (int i = this._spawnedBundleStands.Count - 1; i >= 0; i--)
			{
				if (this._spawnedBundleStands[i].bundleStand == null)
				{
					this._spawnedBundleStands.RemoveAt(i);
				}
			}
			BundleStand[] array = UnityEngine.Object.FindObjectsOfType<BundleStand>();
			for (int j = 0; j < array.Length; j++)
			{
				BundleStand bundle = array[j];
				if (this._spawnedBundleStands.Any((SpawnedBundle x) => x.spawnLocationPath == bundle.transform.parent.gameObject.GetPath(3)))
				{
					SpawnedBundle spawnedBundle = this._spawnedBundleStands.First((SpawnedBundle x) => x.spawnLocationPath == bundle.transform.parent.gameObject.GetPath(3));
					if (spawnedBundle != null && spawnedBundle.bundleStand != bundle)
					{
						UnityEngine.Object.DestroyImmediate(spawnedBundle.bundleStand.gameObject);
						spawnedBundle.bundleStand = bundle;
					}
				}
				else
				{
					this._spawnedBundleStands.Add(new SpawnedBundle
					{
						spawnLocationPath = bundle.transform.parent.gameObject.GetPath(3),
						bundleStand = bundle
					});
				}
			}
			this.GenerateAllStoreBundleReferences();
		}

		// Token: 0x0600475C RID: 18268 RVA: 0x0005E734 File Offset: 0x0005C934
		public StoreBundleData[] GetTryOnButtons()
		{
			return new StoreBundleData[]
			{
				this.tryOnBundleButton1,
				this.tryOnBundleButton2,
				this.tryOnBundleButton3,
				this.tryOnBundleButton4,
				this.tryOnBundleButton5
			};
		}

		// Token: 0x0600475D RID: 18269 RVA: 0x00189D34 File Offset: 0x00187F34
		public void NotifyBundleOfErrorByPlayFabID(string ItemId)
		{
			StoreBundle storeBundle;
			if (this.storeBundlesById.TryGetValue(ItemId, out storeBundle))
			{
				foreach (BundleStand bundleStand in storeBundle.bundleStands)
				{
					bundleStand.ErrorHappened();
				}
			}
		}

		// Token: 0x0600475E RID: 18270 RVA: 0x00189D94 File Offset: 0x00187F94
		public void NotifyBundleOfErrorBySKU(string ItemSKU)
		{
			StoreBundle storeBundle;
			if (this.storeBundlesBySKU.TryGetValue(ItemSKU, out storeBundle))
			{
				foreach (BundleStand bundleStand in storeBundle.bundleStands)
				{
					bundleStand.ErrorHappened();
				}
			}
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x00189DF4 File Offset: 0x00187FF4
		public void MarkBundleOwnedByPlayFabID(string ItemId)
		{
			if (this.storeBundlesById.ContainsKey(ItemId))
			{
				this.storeBundlesById[ItemId].isOwned = true;
				foreach (BundleStand bundleStand in this.storeBundlesById[ItemId].bundleStands)
				{
					bundleStand.NotifyAlreadyOwn();
				}
			}
		}

		// Token: 0x06004760 RID: 18272 RVA: 0x00189E70 File Offset: 0x00188070
		public void MarkBundleOwnedBySKU(string SKU)
		{
			if (this.storeBundlesBySKU.ContainsKey(SKU))
			{
				this.storeBundlesBySKU[SKU].isOwned = true;
				foreach (BundleStand bundleStand in this.storeBundlesBySKU[SKU].bundleStands)
				{
					bundleStand.NotifyAlreadyOwn();
				}
			}
		}

		// Token: 0x06004761 RID: 18273 RVA: 0x00189EEC File Offset: 0x001880EC
		public void CheckIfBundlesOwned()
		{
			foreach (StoreBundle storeBundle in this.storeBundlesById.Values)
			{
				if (storeBundle.isOwned)
				{
					foreach (BundleStand bundleStand in storeBundle.bundleStands)
					{
						bundleStand.NotifyAlreadyOwn();
					}
				}
			}
		}

		// Token: 0x06004762 RID: 18274 RVA: 0x0005E769 File Offset: 0x0005C969
		public void PressTryOnBundleButton(TryOnBundleButton pressedTryOnBundleButton, bool isLeftHand)
		{
			if (this._tryOnBundlesStand.IsNotNull())
			{
				this._tryOnBundlesStand.PressTryOnBundleButton(pressedTryOnBundleButton, isLeftHand);
			}
		}

		// Token: 0x06004763 RID: 18275 RVA: 0x0005E785 File Offset: 0x0005C985
		public void PressPurchaseTryOnBundleButton()
		{
			this._tryOnBundlesStand.PurchaseButtonPressed();
		}

		// Token: 0x06004764 RID: 18276 RVA: 0x0005E792 File Offset: 0x0005C992
		public void UpdateBundlePrice(string productSku, string productFormattedPrice)
		{
			if (this.storeBundlesBySKU.ContainsKey(productSku))
			{
				this.storeBundlesBySKU[productSku].TryUpdatePrice(productFormattedPrice);
			}
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x00189F84 File Offset: 0x00188184
		public void CheckForNoPriceBundlesAndDefaultPrice()
		{
			foreach (KeyValuePair<string, StoreBundle> keyValuePair in this.storeBundlesBySKU)
			{
				string text;
				StoreBundle storeBundle;
				keyValuePair.Deconstruct(out text, out storeBundle);
				StoreBundle storeBundle2 = storeBundle;
				if (!storeBundle2.HasPrice)
				{
					storeBundle2.TryUpdatePrice(null);
				}
			}
		}

		// Token: 0x040048A3 RID: 18595
		public static volatile BundleManager instance;

		// Token: 0x040048A4 RID: 18596
		[FormerlySerializedAs("_TryOnBundlesStand")]
		public TryOnBundlesStand _tryOnBundlesStand;

		// Token: 0x040048A5 RID: 18597
		[SerializeField]
		private StoreBundleData nullBundleData;

		// Token: 0x040048A6 RID: 18598
		private List<StoreBundleData> _bundleScriptableObjects = new List<StoreBundleData>();

		// Token: 0x040048A7 RID: 18599
		[SerializeField]
		private List<StoreBundle> _storeBundles = new List<StoreBundle>();

		// Token: 0x040048A8 RID: 18600
		[FormerlySerializedAs("_SpawnedBundleStands")]
		[SerializeField]
		private List<SpawnedBundle> _spawnedBundleStands = new List<SpawnedBundle>();

		// Token: 0x040048A9 RID: 18601
		public Dictionary<string, StoreBundle> storeBundlesById = new Dictionary<string, StoreBundle>();

		// Token: 0x040048AA RID: 18602
		public Dictionary<string, StoreBundle> storeBundlesBySKU = new Dictionary<string, StoreBundle>();

		// Token: 0x040048AB RID: 18603
		[Header("Enable Advanced Search window in your settings to easily see all bundle prefabs")]
		[SerializeField]
		private List<BundleManager.BundleStandSpawn> BundleStands = new List<BundleManager.BundleStandSpawn>();

		// Token: 0x040048AC RID: 18604
		[SerializeField]
		private StoreBundleData tryOnBundleButton1;

		// Token: 0x040048AD RID: 18605
		[SerializeField]
		private StoreBundleData tryOnBundleButton2;

		// Token: 0x040048AE RID: 18606
		[SerializeField]
		private StoreBundleData tryOnBundleButton3;

		// Token: 0x040048AF RID: 18607
		[SerializeField]
		private StoreBundleData tryOnBundleButton4;

		// Token: 0x040048B0 RID: 18608
		[SerializeField]
		private StoreBundleData tryOnBundleButton5;

		// Token: 0x02000B21 RID: 2849
		[Serializable]
		public class BundleStandSpawn
		{
			// Token: 0x06004767 RID: 18279 RVA: 0x0005E7B4 File Offset: 0x0005C9B4
			private static IEnumerable GetEndCapSpawnPoints()
			{
				return from x in UnityEngine.Object.FindObjectsOfType<EndCapSpawnPoint>()
				select new ValueDropdownItem(string.Concat(new string[]
				{
					x.transform.parent.parent.name,
					"/",
					x.transform.parent.name,
					"/",
					x.name
				}), x);
			}

			// Token: 0x040048B1 RID: 18609
			public EndCapSpawnPoint spawnLocation;

			// Token: 0x040048B2 RID: 18610
			public BundleStand bundleStand;
		}
	}
}
