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
	// Token: 0x02000AF3 RID: 2803
	public class BundleManager : MonoBehaviour
	{
		// Token: 0x06004608 RID: 17928 RVA: 0x0014C63A File Offset: 0x0014A83A
		private IEnumerable GetStoreBundles()
		{
			List<StoreBundleData> list = new List<StoreBundleData>();
			list.Add(this.nullBundleData);
			list.AddRange(this._bundleScriptableObjects);
			return list;
		}

		// Token: 0x06004609 RID: 17929 RVA: 0x0014C659 File Offset: 0x0014A859
		public void Awake()
		{
			if (BundleManager.instance == null)
			{
				BundleManager.instance = this;
				return;
			}
			if (BundleManager.instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
		}

		// Token: 0x0600460A RID: 17930 RVA: 0x0014C68E File Offset: 0x0014A88E
		private void Start()
		{
			this.GenerateBundleDictionaries();
			this.Initialize();
		}

		// Token: 0x0600460B RID: 17931 RVA: 0x0014C69C File Offset: 0x0014A89C
		private void Initialize()
		{
			foreach (StoreBundle storeBundle in this._storeBundles)
			{
				storeBundle.InitializebundleStands();
			}
		}

		// Token: 0x0600460C RID: 17932 RVA: 0x0014C6EC File Offset: 0x0014A8EC
		private void ValidateBundleData()
		{
			foreach (StoreBundle storeBundle in this._storeBundles)
			{
				storeBundle.ValidateBundleData();
			}
		}

		// Token: 0x0600460D RID: 17933 RVA: 0x0014C73C File Offset: 0x0014A93C
		private void SpawnBundleStands()
		{
			foreach (StoreBundle storeBundle in this._storeBundles)
			{
				foreach (BundleStand bundleStand in storeBundle.bundleStands)
				{
					if (bundleStand != null)
					{
						Object.DestroyImmediate(bundleStand.gameObject);
					}
				}
			}
			this._spawnedBundleStands.Clear();
			this.storeBundlesById.Clear();
			this.storeBundlesBySKU.Clear();
			this._storeBundles.Clear();
			this._bundleScriptableObjects.Clear();
			BundleStand[] array = Object.FindObjectsOfType<BundleStand>();
			for (int i = 0; i < array.Length; i++)
			{
				Object.DestroyImmediate(array[i].gameObject);
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

		// Token: 0x0600460E RID: 17934 RVA: 0x0014C950 File Offset: 0x0014AB50
		public void ClearEverything()
		{
			foreach (StoreBundle storeBundle in this._storeBundles)
			{
				foreach (BundleStand bundleStand in storeBundle.bundleStands)
				{
					if (bundleStand != null)
					{
						Object.DestroyImmediate(bundleStand.gameObject);
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
			BundleStand[] array = Object.FindObjectsOfType<BundleStand>();
			for (int i = 0; i < array.Length; i++)
			{
				Object.DestroyImmediate(array[i].gameObject);
			}
		}

		// Token: 0x0600460F RID: 17935 RVA: 0x000023F4 File Offset: 0x000005F4
		public void GenerateAllStoreBundleReferences()
		{
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x0014CA84 File Offset: 0x0014AC84
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

		// Token: 0x06004611 RID: 17937 RVA: 0x0014CB1C File Offset: 0x0014AD1C
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

		// Token: 0x06004612 RID: 17938 RVA: 0x0014CBA4 File Offset: 0x0014ADA4
		public void BundlePurchaseButtonPressed(string playFabItemName)
		{
			CosmeticsController.instance.PurchaseBundle(this.storeBundlesById[playFabItemName]);
		}

		// Token: 0x06004613 RID: 17939 RVA: 0x0014CBC0 File Offset: 0x0014ADC0
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
			BundleStand[] array = Object.FindObjectsOfType<BundleStand>();
			for (int j = 0; j < array.Length; j++)
			{
				BundleStand bundle = array[j];
				if (this._spawnedBundleStands.Any((SpawnedBundle x) => x.spawnLocationPath == bundle.transform.parent.gameObject.GetPath(3)))
				{
					SpawnedBundle spawnedBundle = this._spawnedBundleStands.First((SpawnedBundle x) => x.spawnLocationPath == bundle.transform.parent.gameObject.GetPath(3));
					if (spawnedBundle != null && spawnedBundle.bundleStand != bundle)
					{
						Object.DestroyImmediate(spawnedBundle.bundleStand.gameObject);
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

		// Token: 0x06004614 RID: 17940 RVA: 0x0014CCEB File Offset: 0x0014AEEB
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

		// Token: 0x06004615 RID: 17941 RVA: 0x0014CD20 File Offset: 0x0014AF20
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

		// Token: 0x06004616 RID: 17942 RVA: 0x0014CD80 File Offset: 0x0014AF80
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

		// Token: 0x06004617 RID: 17943 RVA: 0x0014CDE0 File Offset: 0x0014AFE0
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

		// Token: 0x06004618 RID: 17944 RVA: 0x0014CE5C File Offset: 0x0014B05C
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

		// Token: 0x06004619 RID: 17945 RVA: 0x0014CED8 File Offset: 0x0014B0D8
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

		// Token: 0x0600461A RID: 17946 RVA: 0x0014CF70 File Offset: 0x0014B170
		public void PressTryOnBundleButton(TryOnBundleButton pressedTryOnBundleButton, bool isLeftHand)
		{
			if (this._tryOnBundlesStand.IsNotNull())
			{
				this._tryOnBundlesStand.PressTryOnBundleButton(pressedTryOnBundleButton, isLeftHand);
			}
		}

		// Token: 0x0600461B RID: 17947 RVA: 0x0014CF8C File Offset: 0x0014B18C
		public void PressPurchaseTryOnBundleButton()
		{
			this._tryOnBundlesStand.PurchaseButtonPressed();
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x0014CF99 File Offset: 0x0014B199
		public void UpdateBundlePrice(string productSku, string productFormattedPrice)
		{
			if (this.storeBundlesBySKU.ContainsKey(productSku))
			{
				this.storeBundlesBySKU[productSku].TryUpdatePrice(productFormattedPrice);
			}
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x0014CFBC File Offset: 0x0014B1BC
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

		// Token: 0x040047AE RID: 18350
		public static volatile BundleManager instance;

		// Token: 0x040047AF RID: 18351
		[FormerlySerializedAs("_TryOnBundlesStand")]
		public TryOnBundlesStand _tryOnBundlesStand;

		// Token: 0x040047B0 RID: 18352
		[SerializeField]
		private StoreBundleData nullBundleData;

		// Token: 0x040047B1 RID: 18353
		private List<StoreBundleData> _bundleScriptableObjects = new List<StoreBundleData>();

		// Token: 0x040047B2 RID: 18354
		[SerializeField]
		private List<StoreBundle> _storeBundles = new List<StoreBundle>();

		// Token: 0x040047B3 RID: 18355
		[FormerlySerializedAs("_SpawnedBundleStands")]
		[SerializeField]
		private List<SpawnedBundle> _spawnedBundleStands = new List<SpawnedBundle>();

		// Token: 0x040047B4 RID: 18356
		public Dictionary<string, StoreBundle> storeBundlesById = new Dictionary<string, StoreBundle>();

		// Token: 0x040047B5 RID: 18357
		public Dictionary<string, StoreBundle> storeBundlesBySKU = new Dictionary<string, StoreBundle>();

		// Token: 0x040047B6 RID: 18358
		[Header("Enable Advanced Search window in your settings to easily see all bundle prefabs")]
		[SerializeField]
		private List<BundleManager.BundleStandSpawn> BundleStands = new List<BundleManager.BundleStandSpawn>();

		// Token: 0x040047B7 RID: 18359
		[SerializeField]
		private StoreBundleData tryOnBundleButton1;

		// Token: 0x040047B8 RID: 18360
		[SerializeField]
		private StoreBundleData tryOnBundleButton2;

		// Token: 0x040047B9 RID: 18361
		[SerializeField]
		private StoreBundleData tryOnBundleButton3;

		// Token: 0x040047BA RID: 18362
		[SerializeField]
		private StoreBundleData tryOnBundleButton4;

		// Token: 0x040047BB RID: 18363
		[SerializeField]
		private StoreBundleData tryOnBundleButton5;

		// Token: 0x02000AF4 RID: 2804
		[Serializable]
		public class BundleStandSpawn
		{
			// Token: 0x0600461F RID: 17951 RVA: 0x0014D07D File Offset: 0x0014B27D
			private static IEnumerable GetEndCapSpawnPoints()
			{
				return from x in Object.FindObjectsOfType<EndCapSpawnPoint>()
				select new ValueDropdownItem(string.Concat(new string[]
				{
					x.transform.parent.parent.name,
					"/",
					x.transform.parent.name,
					"/",
					x.name
				}), x);
			}

			// Token: 0x040047BC RID: 18364
			public EndCapSpawnPoint spawnLocation;

			// Token: 0x040047BD RID: 18365
			public BundleStand bundleStand;
		}
	}
}
