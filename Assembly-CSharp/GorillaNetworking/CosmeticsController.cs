using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ExitGames.Client.Photon;
using GorillaLocomotion;
using GorillaNetworking.Store;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using Steamworks;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GorillaNetworking
{
	// Token: 0x02000AB3 RID: 2739
	public class CosmeticsController : MonoBehaviour, IGorillaSliceableSimple, IBuildValidation
	{
		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x0600445E RID: 17502 RVA: 0x0005C9A2 File Offset: 0x0005ABA2
		// (set) Token: 0x0600445F RID: 17503 RVA: 0x0005C9AA File Offset: 0x0005ABAA
		public CosmeticInfoV2[] v2_allCosmetics { get; private set; }

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06004460 RID: 17504 RVA: 0x0005C9B3 File Offset: 0x0005ABB3
		// (set) Token: 0x06004461 RID: 17505 RVA: 0x0005C9BB File Offset: 0x0005ABBB
		public bool v2_allCosmeticsInfoAssetRef_isLoaded { get; private set; }

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06004462 RID: 17506 RVA: 0x0005C9C4 File Offset: 0x0005ABC4
		// (set) Token: 0x06004463 RID: 17507 RVA: 0x0005C9CC File Offset: 0x0005ABCC
		public bool v2_isGetCosmeticsPlayCatalogDataWaitingForCallback { get; private set; }

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06004464 RID: 17508 RVA: 0x0005C9D5 File Offset: 0x0005ABD5
		// (set) Token: 0x06004465 RID: 17509 RVA: 0x0005C9DD File Offset: 0x0005ABDD
		public bool v2_isCosmeticPlayFabCatalogDataLoaded { get; private set; }

		// Token: 0x06004466 RID: 17510 RVA: 0x0005C9E6 File Offset: 0x0005ABE6
		private void V2Awake()
		{
			this._allCosmetics = null;
			base.StartCoroutine(this.V2_allCosmeticsInfoAssetRefSO_LoadCoroutine());
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x0005C9FC File Offset: 0x0005ABFC
		private IEnumerator V2_allCosmeticsInfoAssetRefSO_LoadCoroutine()
		{
			while (!PlayFabAuthenticator.instance)
			{
				yield return new WaitForSeconds(1f);
			}
			float[] retryWaitTimes = new float[]
			{
				1f,
				2f,
				4f,
				4f,
				10f,
				10f,
				10f,
				10f,
				10f,
				10f,
				10f,
				10f,
				10f,
				10f,
				30f
			};
			int retryCount = 0;
			AsyncOperationHandle<AllCosmeticsArraySO> newSysAllCosmeticsAsyncOp;
			for (;;)
			{
				Debug.Log(string.Format("Attempting to load runtime key \"{0}\" ", this.v2_allCosmeticsInfoAssetRef.RuntimeKey) + string.Format("(Attempt: {0})", retryCount + 1));
				newSysAllCosmeticsAsyncOp = this.v2_allCosmeticsInfoAssetRef.LoadAssetAsync();
				yield return newSysAllCosmeticsAsyncOp;
				if (ApplicationQuittingState.IsQuitting)
				{
					break;
				}
				if (!newSysAllCosmeticsAsyncOp.IsValid())
				{
					Debug.LogError("`newSysAllCosmeticsAsyncOp` (should never happen) became invalid some how.");
				}
				if (newSysAllCosmeticsAsyncOp.Status == AsyncOperationStatus.Succeeded)
				{
					goto Block_4;
				}
				Debug.LogError(string.Format("Failed to load \"{0}\". ", this.v2_allCosmeticsInfoAssetRef.RuntimeKey) + "Error: " + newSysAllCosmeticsAsyncOp.OperationException.Message);
				float time = retryWaitTimes[Mathf.Min(retryCount, retryWaitTimes.Length - 1)];
				yield return new WaitForSecondsRealtime(time);
				int num = retryCount;
				retryCount = num + 1;
				newSysAllCosmeticsAsyncOp = default(AsyncOperationHandle<AllCosmeticsArraySO>);
			}
			yield break;
			Block_4:
			this.V2_allCosmeticsInfoAssetRef_LoadSucceeded(newSysAllCosmeticsAsyncOp.Result);
			yield break;
		}

		// Token: 0x06004468 RID: 17512 RVA: 0x0017B350 File Offset: 0x00179550
		private void V2_allCosmeticsInfoAssetRef_LoadSucceeded(AllCosmeticsArraySO allCosmeticsSO)
		{
			this.v2_allCosmetics = new CosmeticInfoV2[allCosmeticsSO.sturdyAssetRefs.Length];
			for (int i = 0; i < allCosmeticsSO.sturdyAssetRefs.Length; i++)
			{
				this.v2_allCosmetics[i] = allCosmeticsSO.sturdyAssetRefs[i].obj.info;
			}
			this._allCosmetics = new List<CosmeticsController.CosmeticItem>(allCosmeticsSO.sturdyAssetRefs.Length);
			for (int j = 0; j < this.v2_allCosmetics.Length; j++)
			{
				CosmeticInfoV2 cosmeticInfoV = this.v2_allCosmetics[j];
				string playFabID = cosmeticInfoV.playFabID;
				this._allCosmeticsDictV2[playFabID] = cosmeticInfoV;
				CosmeticsController.CosmeticItem item = new CosmeticsController.CosmeticItem
				{
					itemName = playFabID,
					itemCategory = cosmeticInfoV.category,
					isHoldable = cosmeticInfoV.hasHoldableParts,
					displayName = playFabID,
					itemPicture = cosmeticInfoV.icon,
					overrideDisplayName = cosmeticInfoV.displayName,
					bothHandsHoldable = cosmeticInfoV.usesBothHandSlots,
					isNullItem = false
				};
				this._allCosmetics.Add(item);
			}
			this.v2_allCosmeticsInfoAssetRef_isLoaded = true;
			Action v2_allCosmeticsInfoAssetRef_OnPostLoad = this.V2_allCosmeticsInfoAssetRef_OnPostLoad;
			if (v2_allCosmeticsInfoAssetRef_OnPostLoad == null)
			{
				return;
			}
			v2_allCosmeticsInfoAssetRef_OnPostLoad();
		}

		// Token: 0x06004469 RID: 17513 RVA: 0x0005CA0B File Offset: 0x0005AC0B
		public bool TryGetCosmeticInfoV2(string playFabId, out CosmeticInfoV2 cosmeticInfo)
		{
			return this._allCosmeticsDictV2.TryGetValue(playFabId, out cosmeticInfo);
		}

		// Token: 0x0600446A RID: 17514 RVA: 0x0005CA1A File Offset: 0x0005AC1A
		private void V2_ConformCosmeticItemV1DisplayName(ref CosmeticsController.CosmeticItem cosmetic)
		{
			if (cosmetic.itemName == cosmetic.displayName)
			{
				return;
			}
			cosmetic.overrideDisplayName = cosmetic.displayName;
			cosmetic.displayName = cosmetic.itemName;
		}

		// Token: 0x0600446B RID: 17515 RVA: 0x0017B484 File Offset: 0x00179684
		internal void InitializeCosmeticStands()
		{
			foreach (CosmeticStand cosmeticStand in this.cosmeticStands)
			{
				if (cosmeticStand != null)
				{
					cosmeticStand.InitializeCosmetic();
				}
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x0600446C RID: 17516 RVA: 0x0005CA48 File Offset: 0x0005AC48
		// (set) Token: 0x0600446D RID: 17517 RVA: 0x0005CA4F File Offset: 0x0005AC4F
		public static bool hasInstance { get; private set; }

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x0600446E RID: 17518 RVA: 0x0005CA57 File Offset: 0x0005AC57
		// (set) Token: 0x0600446F RID: 17519 RVA: 0x0005CA5F File Offset: 0x0005AC5F
		public List<CosmeticsController.CosmeticItem> allCosmetics
		{
			get
			{
				return this._allCosmetics;
			}
			set
			{
				this._allCosmetics = value;
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06004470 RID: 17520 RVA: 0x0005CA68 File Offset: 0x0005AC68
		// (set) Token: 0x06004471 RID: 17521 RVA: 0x0005CA70 File Offset: 0x0005AC70
		public bool allCosmeticsDict_isInitialized { get; private set; }

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06004472 RID: 17522 RVA: 0x0005CA79 File Offset: 0x0005AC79
		public Dictionary<string, CosmeticsController.CosmeticItem> allCosmeticsDict
		{
			get
			{
				return this._allCosmeticsDict;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06004473 RID: 17523 RVA: 0x0005CA81 File Offset: 0x0005AC81
		// (set) Token: 0x06004474 RID: 17524 RVA: 0x0005CA89 File Offset: 0x0005AC89
		public bool allCosmeticsItemIDsfromDisplayNamesDict_isInitialized { get; private set; }

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06004475 RID: 17525 RVA: 0x0005CA92 File Offset: 0x0005AC92
		public Dictionary<string, string> allCosmeticsItemIDsfromDisplayNamesDict
		{
			get
			{
				return this._allCosmeticsItemIDsfromDisplayNamesDict;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06004476 RID: 17526 RVA: 0x0005CA9A File Offset: 0x0005AC9A
		// (set) Token: 0x06004477 RID: 17527 RVA: 0x0005CAA2 File Offset: 0x0005ACA2
		public bool isHidingCosmeticsFromRemotePlayers { get; private set; }

		// Token: 0x06004478 RID: 17528 RVA: 0x0005CAAB File Offset: 0x0005ACAB
		public void AddWardrobeInstance(WardrobeInstance instance)
		{
			this.wardrobes.Add(instance);
			if (CosmeticsV2Spawner_Dirty.allPartsInstantiated)
			{
				this.UpdateWardrobeModelsAndButtons();
			}
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x0005CAC6 File Offset: 0x0005ACC6
		public void RemoveWardrobeInstance(WardrobeInstance instance)
		{
			this.wardrobes.Remove(instance);
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x0600447A RID: 17530 RVA: 0x0005CAD5 File Offset: 0x0005ACD5
		public int CurrencyBalance
		{
			get
			{
				return this.currencyBalance;
			}
		}

		// Token: 0x0600447B RID: 17531 RVA: 0x0017B4BC File Offset: 0x001796BC
		public void Awake()
		{
			if (CosmeticsController.instance == null)
			{
				CosmeticsController.instance = this;
				CosmeticsController.hasInstance = true;
			}
			else if (CosmeticsController.instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			this.V2Awake();
			if (base.gameObject.activeSelf)
			{
				this.catalog = "DLC";
				this.currencyName = "SR";
				this.nullItem = default(CosmeticsController.CosmeticItem);
				this.nullItem.itemName = "null";
				this.nullItem.displayName = "NOTHING";
				this.nullItem.itemPicture = Resources.Load<Sprite>("CosmeticNull_Icon");
				this.nullItem.itemPictureResourceString = "";
				this.nullItem.overrideDisplayName = "NOTHING";
				this.nullItem.meshAtlasResourceString = "";
				this.nullItem.meshResourceString = "";
				this.nullItem.materialResourceString = "";
				this.nullItem.isNullItem = true;
				this._allCosmeticsDict[this.nullItem.itemName] = this.nullItem;
				this._allCosmeticsItemIDsfromDisplayNamesDict[this.nullItem.displayName] = this.nullItem.itemName;
				for (int i = 0; i < 16; i++)
				{
					this.tryOnSet.items[i] = this.nullItem;
				}
				this.cosmeticsPages[0] = 0;
				this.cosmeticsPages[1] = 0;
				this.cosmeticsPages[2] = 0;
				this.cosmeticsPages[3] = 0;
				this.cosmeticsPages[4] = 0;
				this.cosmeticsPages[5] = 0;
				this.cosmeticsPages[6] = 0;
				this.cosmeticsPages[7] = 0;
				this.cosmeticsPages[8] = 0;
				this.cosmeticsPages[9] = 0;
				this.cosmeticsPages[10] = 0;
				this.itemLists[0] = this.unlockedHats;
				this.itemLists[1] = this.unlockedFaces;
				this.itemLists[2] = this.unlockedBadges;
				this.itemLists[3] = this.unlockedPaws;
				this.itemLists[4] = this.unlockedFurs;
				this.itemLists[5] = this.unlockedShirts;
				this.itemLists[6] = this.unlockedPants;
				this.itemLists[7] = this.unlockedArms;
				this.itemLists[8] = this.unlockedBacks;
				this.itemLists[9] = this.unlockedChests;
				this.itemLists[10] = this.unlockedTagFX;
				this.updateCosmeticsRetries = 0;
				this.maxUpdateCosmeticsRetries = 5;
				this.inventoryStringList.Clear();
				this.inventoryStringList.Add("Inventory");
				base.StartCoroutine(this.CheckCanGetDaily());
			}
		}

		// Token: 0x0600447C RID: 17532 RVA: 0x0017B764 File Offset: 0x00179964
		public void Start()
		{
			PlayFabTitleDataCache.Instance.GetTitleData("BundleData", delegate(string data)
			{
				this.bundleList.FromJson(data);
			}, delegate(PlayFabError e)
			{
				Debug.LogError(string.Format("Error getting bundle data: {0}", e));
			});
			this.anchorOverrides = GorillaTagger.Instance.offlineVRRig.GetComponent<VRRigAnchorOverrides>();
		}

		// Token: 0x0600447D RID: 17533 RVA: 0x0005CADD File Offset: 0x0005ACDD
		public void OnEnable()
		{
			GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
			if (SteamManager.Initialized && this._steamMicroTransactionAuthorizationResponse == null)
			{
				this._steamMicroTransactionAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(new Callback<MicroTxnAuthorizationResponse_t>.DispatchDelegate(this.ProcessSteamCallback));
			}
		}

		// Token: 0x0600447E RID: 17534 RVA: 0x0005CB0C File Offset: 0x0005AD0C
		public void OnDisable()
		{
			Callback<MicroTxnAuthorizationResponse_t> steamMicroTransactionAuthorizationResponse = this._steamMicroTransactionAuthorizationResponse;
			if (steamMicroTransactionAuthorizationResponse != null)
			{
				steamMicroTransactionAuthorizationResponse.Unregister();
			}
			GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x00030607 File Offset: 0x0002E807
		public void SliceUpdate()
		{
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x0017B7C0 File Offset: 0x001799C0
		public static bool CompareCategoryToSavedCosmeticSlots(CosmeticsController.CosmeticCategory category, CosmeticsController.CosmeticSlots slot)
		{
			switch (category)
			{
			case CosmeticsController.CosmeticCategory.Hat:
				return slot == CosmeticsController.CosmeticSlots.Hat;
			case CosmeticsController.CosmeticCategory.Badge:
				return CosmeticsController.CosmeticSlots.Badge == slot;
			case CosmeticsController.CosmeticCategory.Face:
				return CosmeticsController.CosmeticSlots.Face == slot;
			case CosmeticsController.CosmeticCategory.Paw:
				return slot == CosmeticsController.CosmeticSlots.HandRight || slot == CosmeticsController.CosmeticSlots.HandLeft;
			case CosmeticsController.CosmeticCategory.Chest:
				return CosmeticsController.CosmeticSlots.Chest == slot;
			case CosmeticsController.CosmeticCategory.Fur:
				return CosmeticsController.CosmeticSlots.Fur == slot;
			case CosmeticsController.CosmeticCategory.Shirt:
				return CosmeticsController.CosmeticSlots.Shirt == slot;
			case CosmeticsController.CosmeticCategory.Back:
				return slot == CosmeticsController.CosmeticSlots.BackLeft || slot == CosmeticsController.CosmeticSlots.BackRight;
			case CosmeticsController.CosmeticCategory.Arms:
				return slot == CosmeticsController.CosmeticSlots.ArmLeft || slot == CosmeticsController.CosmeticSlots.ArmRight;
			case CosmeticsController.CosmeticCategory.Pants:
				return CosmeticsController.CosmeticSlots.Pants == slot;
			case CosmeticsController.CosmeticCategory.TagEffect:
				return CosmeticsController.CosmeticSlots.TagEffect == slot;
			default:
				return false;
			}
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x0017B854 File Offset: 0x00179A54
		public static CosmeticsController.CosmeticSlots CategoryToNonTransferrableSlot(CosmeticsController.CosmeticCategory category)
		{
			switch (category)
			{
			case CosmeticsController.CosmeticCategory.Hat:
				return CosmeticsController.CosmeticSlots.Hat;
			case CosmeticsController.CosmeticCategory.Badge:
				return CosmeticsController.CosmeticSlots.Badge;
			case CosmeticsController.CosmeticCategory.Face:
				return CosmeticsController.CosmeticSlots.Face;
			case CosmeticsController.CosmeticCategory.Paw:
				return CosmeticsController.CosmeticSlots.HandRight;
			case CosmeticsController.CosmeticCategory.Chest:
				return CosmeticsController.CosmeticSlots.Chest;
			case CosmeticsController.CosmeticCategory.Fur:
				return CosmeticsController.CosmeticSlots.Fur;
			case CosmeticsController.CosmeticCategory.Shirt:
				return CosmeticsController.CosmeticSlots.Shirt;
			case CosmeticsController.CosmeticCategory.Back:
				return CosmeticsController.CosmeticSlots.Back;
			case CosmeticsController.CosmeticCategory.Arms:
				return CosmeticsController.CosmeticSlots.Arms;
			case CosmeticsController.CosmeticCategory.Pants:
				return CosmeticsController.CosmeticSlots.Pants;
			case CosmeticsController.CosmeticCategory.TagEffect:
				return CosmeticsController.CosmeticSlots.TagEffect;
			default:
				return CosmeticsController.CosmeticSlots.Count;
			}
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x0005CB26 File Offset: 0x0005AD26
		private CosmeticsController.CosmeticSlots DropPositionToCosmeticSlot(BodyDockPositions.DropPositions pos)
		{
			switch (pos)
			{
			case BodyDockPositions.DropPositions.LeftArm:
				return CosmeticsController.CosmeticSlots.ArmLeft;
			case BodyDockPositions.DropPositions.RightArm:
				return CosmeticsController.CosmeticSlots.ArmRight;
			case BodyDockPositions.DropPositions.LeftArm | BodyDockPositions.DropPositions.RightArm:
				break;
			case BodyDockPositions.DropPositions.Chest:
				return CosmeticsController.CosmeticSlots.Chest;
			default:
				if (pos == BodyDockPositions.DropPositions.LeftBack)
				{
					return CosmeticsController.CosmeticSlots.BackLeft;
				}
				if (pos == BodyDockPositions.DropPositions.RightBack)
				{
					return CosmeticsController.CosmeticSlots.BackRight;
				}
				break;
			}
			return CosmeticsController.CosmeticSlots.Count;
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x0005CB58 File Offset: 0x0005AD58
		private static BodyDockPositions.DropPositions CosmeticSlotToDropPosition(CosmeticsController.CosmeticSlots slot)
		{
			switch (slot)
			{
			case CosmeticsController.CosmeticSlots.ArmLeft:
				return BodyDockPositions.DropPositions.LeftArm;
			case CosmeticsController.CosmeticSlots.ArmRight:
				return BodyDockPositions.DropPositions.RightArm;
			case CosmeticsController.CosmeticSlots.BackLeft:
				return BodyDockPositions.DropPositions.LeftBack;
			case CosmeticsController.CosmeticSlots.BackRight:
				return BodyDockPositions.DropPositions.RightBack;
			case CosmeticsController.CosmeticSlots.Chest:
				return BodyDockPositions.DropPositions.Chest;
			}
			return BodyDockPositions.DropPositions.None;
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x0005CB8C File Offset: 0x0005AD8C
		private void SaveItemPreference(CosmeticsController.CosmeticSlots slot, int slotIdx, CosmeticsController.CosmeticItem newItem)
		{
			PlayerPrefs.SetString(CosmeticsController.CosmeticSet.SlotPlayerPreferenceName(slot), newItem.itemName);
			PlayerPrefs.Save();
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x0017B8B8 File Offset: 0x00179AB8
		public void SaveCurrentItemPreferences()
		{
			for (int i = 0; i < 16; i++)
			{
				CosmeticsController.CosmeticSlots slot = (CosmeticsController.CosmeticSlots)i;
				this.SaveItemPreference(slot, i, this.currentWornSet.items[i]);
			}
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x0017B8F0 File Offset: 0x00179AF0
		private void ApplyCosmeticToSet(CosmeticsController.CosmeticSet set, CosmeticsController.CosmeticItem newItem, int slotIdx, CosmeticsController.CosmeticSlots slot, bool applyToPlayerPrefs, List<CosmeticsController.CosmeticSlots> appliedSlots)
		{
			CosmeticsController.CosmeticItem cosmeticItem = (set.items[slotIdx].itemName == newItem.itemName) ? this.nullItem : newItem;
			set.items[slotIdx] = cosmeticItem;
			if (applyToPlayerPrefs)
			{
				this.SaveItemPreference(slot, slotIdx, cosmeticItem);
			}
			appliedSlots.Add(slot);
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x0017B94C File Offset: 0x00179B4C
		private void PrivApplyCosmeticItemToSet(CosmeticsController.CosmeticSet set, CosmeticsController.CosmeticItem newItem, bool isLeftHand, bool applyToPlayerPrefs, List<CosmeticsController.CosmeticSlots> appliedSlots)
		{
			if (newItem.isNullItem)
			{
				return;
			}
			if (CosmeticsController.CosmeticSet.IsHoldable(newItem))
			{
				BodyDockPositions.DockingResult dockingResult = GorillaTagger.Instance.offlineVRRig.GetComponent<BodyDockPositions>().ToggleWithHandedness(newItem.displayName, isLeftHand, newItem.bothHandsHoldable);
				foreach (BodyDockPositions.DropPositions pos in dockingResult.positionsDisabled)
				{
					CosmeticsController.CosmeticSlots cosmeticSlots = this.DropPositionToCosmeticSlot(pos);
					if (cosmeticSlots != CosmeticsController.CosmeticSlots.Count)
					{
						int num = (int)cosmeticSlots;
						set.items[num] = this.nullItem;
						if (applyToPlayerPrefs)
						{
							this.SaveItemPreference(cosmeticSlots, num, this.nullItem);
						}
					}
				}
				using (List<BodyDockPositions.DropPositions>.Enumerator enumerator = dockingResult.dockedPosition.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BodyDockPositions.DropPositions dropPositions = enumerator.Current;
						if (dropPositions != BodyDockPositions.DropPositions.None)
						{
							CosmeticsController.CosmeticSlots cosmeticSlots2 = this.DropPositionToCosmeticSlot(dropPositions);
							int num2 = (int)cosmeticSlots2;
							set.items[num2] = newItem;
							if (applyToPlayerPrefs)
							{
								this.SaveItemPreference(cosmeticSlots2, num2, newItem);
							}
							appliedSlots.Add(cosmeticSlots2);
						}
					}
					return;
				}
			}
			if (newItem.itemCategory == CosmeticsController.CosmeticCategory.Paw)
			{
				CosmeticsController.CosmeticSlots cosmeticSlots3 = isLeftHand ? CosmeticsController.CosmeticSlots.HandLeft : CosmeticsController.CosmeticSlots.HandRight;
				int slotIdx = (int)cosmeticSlots3;
				this.ApplyCosmeticToSet(set, newItem, slotIdx, cosmeticSlots3, applyToPlayerPrefs, appliedSlots);
				CosmeticsController.CosmeticSlots cosmeticSlots4 = CosmeticsController.CosmeticSet.OppositeSlot(cosmeticSlots3);
				int num3 = (int)cosmeticSlots4;
				if (newItem.bothHandsHoldable)
				{
					this.ApplyCosmeticToSet(set, this.nullItem, num3, cosmeticSlots4, applyToPlayerPrefs, appliedSlots);
					return;
				}
				if (set.items[num3].itemName == newItem.itemName)
				{
					this.ApplyCosmeticToSet(set, this.nullItem, num3, cosmeticSlots4, applyToPlayerPrefs, appliedSlots);
				}
				if (set.items[num3].bothHandsHoldable)
				{
					this.ApplyCosmeticToSet(set, this.nullItem, num3, cosmeticSlots4, applyToPlayerPrefs, appliedSlots);
					return;
				}
			}
			else
			{
				CosmeticsController.CosmeticSlots cosmeticSlots5 = CosmeticsController.CategoryToNonTransferrableSlot(newItem.itemCategory);
				int slotIdx2 = (int)cosmeticSlots5;
				this.ApplyCosmeticToSet(set, newItem, slotIdx2, cosmeticSlots5, applyToPlayerPrefs, appliedSlots);
			}
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x0005CBA4 File Offset: 0x0005ADA4
		public void ApplyCosmeticItemToSet(CosmeticsController.CosmeticSet set, CosmeticsController.CosmeticItem newItem, bool isLeftHand, bool applyToPlayerPrefs)
		{
			this.ApplyCosmeticItemToSet(set, newItem, isLeftHand, applyToPlayerPrefs, CosmeticsController._g_default_outAppliedSlotsList_for_applyCosmeticItemToSet);
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x0017BB50 File Offset: 0x00179D50
		public void ApplyCosmeticItemToSet(CosmeticsController.CosmeticSet set, CosmeticsController.CosmeticItem newItem, bool isLeftHand, bool applyToPlayerPrefs, List<CosmeticsController.CosmeticSlots> outAppliedSlotsList)
		{
			outAppliedSlotsList.Clear();
			if (newItem.itemCategory == CosmeticsController.CosmeticCategory.Set)
			{
				bool flag = false;
				Dictionary<CosmeticsController.CosmeticItem, bool> dictionary = new Dictionary<CosmeticsController.CosmeticItem, bool>();
				foreach (string itemID in newItem.bundledItems)
				{
					CosmeticsController.CosmeticItem itemFromDict = this.GetItemFromDict(itemID);
					if (this.AnyMatch(set, itemFromDict))
					{
						flag = true;
						dictionary.Add(itemFromDict, true);
					}
					else
					{
						dictionary.Add(itemFromDict, false);
					}
				}
				using (Dictionary<CosmeticsController.CosmeticItem, bool>.Enumerator enumerator = dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<CosmeticsController.CosmeticItem, bool> keyValuePair = enumerator.Current;
						if (flag)
						{
							if (keyValuePair.Value)
							{
								this.PrivApplyCosmeticItemToSet(set, keyValuePair.Key, isLeftHand, applyToPlayerPrefs, outAppliedSlotsList);
							}
						}
						else
						{
							this.PrivApplyCosmeticItemToSet(set, keyValuePair.Key, isLeftHand, applyToPlayerPrefs, outAppliedSlotsList);
						}
					}
					return;
				}
			}
			this.PrivApplyCosmeticItemToSet(set, newItem, isLeftHand, applyToPlayerPrefs, outAppliedSlotsList);
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x0017BC3C File Offset: 0x00179E3C
		public void RemoveCosmeticItemFromSet(CosmeticsController.CosmeticSet set, string itemName, bool applyToPlayerPrefs)
		{
			this.cachedSet.CopyItems(set);
			for (int i = 0; i < 16; i++)
			{
				if (set.items[i].displayName == itemName)
				{
					set.items[i] = this.nullItem;
					if (applyToPlayerPrefs)
					{
						this.SaveItemPreference((CosmeticsController.CosmeticSlots)i, i, this.nullItem);
					}
				}
			}
			VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
			BodyDockPositions component = offlineVRRig.GetComponent<BodyDockPositions>();
			set.ActivateCosmetics(this.cachedSet, offlineVRRig, component, offlineVRRig.cosmeticsObjectRegistry);
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x0005CBB6 File Offset: 0x0005ADB6
		public void PressFittingRoomButton(FittingRoomButton pressedFittingRoomButton, bool isLeftHand)
		{
			BundleManager.instance._tryOnBundlesStand.ClearSelectedBundle();
			this.ApplyCosmeticItemToSet(this.tryOnSet, pressedFittingRoomButton.currentCosmeticItem, isLeftHand, false);
			this.UpdateShoppingCart();
			this.UpdateWornCosmetics(true);
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x0017BCC4 File Offset: 0x00179EC4
		public CosmeticsController.EWearingCosmeticSet CheckIfCosmeticSetMatchesItemSet(CosmeticsController.CosmeticSet set, string itemName)
		{
			CosmeticsController.EWearingCosmeticSet ewearingCosmeticSet = CosmeticsController.EWearingCosmeticSet.NotASet;
			CosmeticsController.CosmeticItem cosmeticItem = this.allCosmeticsDict[itemName];
			if (cosmeticItem.bundledItems.Length != 0)
			{
				foreach (string key in cosmeticItem.bundledItems)
				{
					if (this.AnyMatch(set, this.allCosmeticsDict[key]))
					{
						if (ewearingCosmeticSet == CosmeticsController.EWearingCosmeticSet.NotASet)
						{
							ewearingCosmeticSet = CosmeticsController.EWearingCosmeticSet.Complete;
						}
						else if (ewearingCosmeticSet == CosmeticsController.EWearingCosmeticSet.NotWearing)
						{
							ewearingCosmeticSet = CosmeticsController.EWearingCosmeticSet.Partial;
						}
					}
					else if (ewearingCosmeticSet == CosmeticsController.EWearingCosmeticSet.NotASet)
					{
						ewearingCosmeticSet = CosmeticsController.EWearingCosmeticSet.NotWearing;
					}
					else if (ewearingCosmeticSet == CosmeticsController.EWearingCosmeticSet.Complete)
					{
						ewearingCosmeticSet = CosmeticsController.EWearingCosmeticSet.Partial;
					}
				}
			}
			return ewearingCosmeticSet;
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x0017BD38 File Offset: 0x00179F38
		public void PressCosmeticStandButton(CosmeticStand pressedStand)
		{
			this.searchIndex = this.currentCart.IndexOf(pressedStand.thisCosmeticItem);
			if (this.searchIndex != -1)
			{
				GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.cart_item_remove, pressedStand.thisCosmeticItem);
				this.currentCart.RemoveAt(this.searchIndex);
				pressedStand.isOn = false;
				for (int i = 0; i < 16; i++)
				{
					if (pressedStand.thisCosmeticItem.itemName == this.tryOnSet.items[i].itemName)
					{
						this.tryOnSet.items[i] = this.nullItem;
					}
				}
			}
			else
			{
				GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.cart_item_add, pressedStand.thisCosmeticItem);
				this.currentCart.Insert(0, pressedStand.thisCosmeticItem);
				pressedStand.isOn = true;
				if (this.currentCart.Count > this.fittingRoomButtons.Length)
				{
					foreach (CosmeticStand cosmeticStand in this.cosmeticStands)
					{
						if (!(cosmeticStand == null) && cosmeticStand.thisCosmeticItem.itemName == this.currentCart[this.fittingRoomButtons.Length].itemName)
						{
							cosmeticStand.isOn = false;
							cosmeticStand.UpdateColor();
							break;
						}
					}
					this.currentCart.RemoveAt(this.fittingRoomButtons.Length);
				}
			}
			pressedStand.UpdateColor();
			this.UpdateShoppingCart();
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x0017BEA4 File Offset: 0x0017A0A4
		public void PressWardrobeItemButton(CosmeticsController.CosmeticItem cosmeticItem, bool isLeftHand)
		{
			if (cosmeticItem.isNullItem)
			{
				return;
			}
			CosmeticsController.CosmeticItem itemFromDict = this.GetItemFromDict(cosmeticItem.itemName);
			List<CosmeticsController.CosmeticSlots> list = CollectionPool<List<CosmeticsController.CosmeticSlots>, CosmeticsController.CosmeticSlots>.Get();
			if (list.Capacity < 16)
			{
				list.Capacity = 16;
			}
			this.ApplyCosmeticItemToSet(this.currentWornSet, itemFromDict, isLeftHand, true, list);
			foreach (CosmeticsController.CosmeticSlots cosmeticSlots in list)
			{
				this.tryOnSet.items[(int)cosmeticSlots] = this.nullItem;
			}
			CollectionPool<List<CosmeticsController.CosmeticSlots>, CosmeticsController.CosmeticSlots>.Release(list);
			this.UpdateShoppingCart();
			this.UpdateWornCosmetics(true);
			Action onCosmeticsUpdated = this.OnCosmeticsUpdated;
			if (onCosmeticsUpdated == null)
			{
				return;
			}
			onCosmeticsUpdated();
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x0017BF64 File Offset: 0x0017A164
		public void PressWardrobeFunctionButton(string function)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(function);
			if (num <= 2554875734U)
			{
				if (num <= 895779448U)
				{
					if (num != 292255708U)
					{
						if (num != 306900080U)
						{
							if (num == 895779448U)
							{
								if (function == "badge")
								{
									if (this.wardrobeType == 2)
									{
										return;
									}
									this.wardrobeType = 2;
								}
							}
						}
						else if (function == "left")
						{
							this.cosmeticsPages[this.wardrobeType] = this.cosmeticsPages[this.wardrobeType] - 1;
							if (this.cosmeticsPages[this.wardrobeType] < 0)
							{
								this.cosmeticsPages[this.wardrobeType] = (this.itemLists[this.wardrobeType].Count - 1) / 3;
							}
						}
					}
					else if (function == "face")
					{
						if (this.wardrobeType == 1)
						{
							return;
						}
						this.wardrobeType = 1;
					}
				}
				else if (num != 1538531746U)
				{
					if (num != 2028154341U)
					{
						if (num == 2554875734U)
						{
							if (function == "chest")
							{
								if (this.wardrobeType == 8)
								{
									return;
								}
								this.wardrobeType = 8;
							}
						}
					}
					else if (function == "right")
					{
						this.cosmeticsPages[this.wardrobeType] = this.cosmeticsPages[this.wardrobeType] + 1;
						if (this.cosmeticsPages[this.wardrobeType] > (this.itemLists[this.wardrobeType].Count - 1) / 3)
						{
							this.cosmeticsPages[this.wardrobeType] = 0;
						}
					}
				}
				else if (function == "back")
				{
					if (this.wardrobeType == 7)
					{
						return;
					}
					this.wardrobeType = 7;
				}
			}
			else if (num <= 3034286914U)
			{
				if (num != 2633735346U)
				{
					if (num != 2953262278U)
					{
						if (num == 3034286914U)
						{
							if (function == "fur")
							{
								if (this.wardrobeType == 4)
								{
									return;
								}
								this.wardrobeType = 4;
							}
						}
					}
					else if (function == "outfit")
					{
						if (this.wardrobeType == 5)
						{
							return;
						}
						this.wardrobeType = 5;
					}
				}
				else if (function == "arms")
				{
					if (this.wardrobeType == 6)
					{
						return;
					}
					this.wardrobeType = 6;
				}
			}
			else if (num <= 3300536096U)
			{
				if (num != 3081164502U)
				{
					if (num == 3300536096U)
					{
						if (function == "hand")
						{
							if (this.wardrobeType == 3)
							{
								return;
							}
							this.wardrobeType = 3;
						}
					}
				}
				else if (function == "tagEffect")
				{
					if (this.wardrobeType == 10)
					{
						return;
					}
					this.wardrobeType = 10;
				}
			}
			else if (num != 3568683773U)
			{
				if (num == 4072609730U)
				{
					if (function == "hat")
					{
						if (this.wardrobeType == 0)
						{
							return;
						}
						this.wardrobeType = 0;
					}
				}
			}
			else if (function == "reserved")
			{
				if (this.wardrobeType == 9)
				{
					return;
				}
				this.wardrobeType = 9;
			}
			this.UpdateWardrobeModelsAndButtons();
			Action onCosmeticsUpdated = this.OnCosmeticsUpdated;
			if (onCosmeticsUpdated == null)
			{
				return;
			}
			onCosmeticsUpdated();
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x0017C2F0 File Offset: 0x0017A4F0
		public void ClearCheckout(bool sendEvent)
		{
			if (sendEvent)
			{
				GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.checkout_cancel, this.currentCart);
			}
			this.itemToBuy = this.nullItem;
			this.checkoutHeadModel.SetCosmeticActive(this.itemToBuy.displayName, false);
			this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Start;
			this.ProcessPurchaseItemState(null, false);
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x0017C348 File Offset: 0x0017A548
		public bool RemoveItemFromCart(CosmeticsController.CosmeticItem cosmeticItem)
		{
			this.searchIndex = this.currentCart.IndexOf(cosmeticItem);
			if (this.searchIndex != -1)
			{
				this.currentCart.RemoveAt(this.searchIndex);
				for (int i = 0; i < 16; i++)
				{
					if (cosmeticItem.itemName == this.tryOnSet.items[i].itemName)
					{
						this.tryOnSet.items[i] = this.nullItem;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x0017C3CC File Offset: 0x0017A5CC
		public void PressCheckoutCartButton(CheckoutCartButton pressedCheckoutCartButton, bool isLeftHand)
		{
			if (this.currentPurchaseItemStage != CosmeticsController.PurchaseItemStages.Buying)
			{
				this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.CheckoutButtonPressed;
				this.tryOnSet.ClearSet(this.nullItem);
				if (this.itemToBuy.displayName == pressedCheckoutCartButton.currentCosmeticItem.displayName)
				{
					this.itemToBuy = this.nullItem;
					this.checkoutHeadModel.SetCosmeticActive(this.itemToBuy.displayName, false);
				}
				else
				{
					this.itemToBuy = pressedCheckoutCartButton.currentCosmeticItem;
					this.checkoutHeadModel.SetCosmeticActive(this.itemToBuy.displayName, false);
					if (this.itemToBuy.bundledItems != null && this.itemToBuy.bundledItems.Length != 0)
					{
						List<string> list = new List<string>();
						foreach (string itemID in this.itemToBuy.bundledItems)
						{
							this.tempItem = this.GetItemFromDict(itemID);
							list.Add(this.tempItem.displayName);
						}
						this.checkoutHeadModel.SetCosmeticActiveArray(list.ToArray(), new bool[list.Count]);
					}
					this.ApplyCosmeticItemToSet(this.tryOnSet, pressedCheckoutCartButton.currentCosmeticItem, isLeftHand, false);
					this.UpdateWornCosmetics(true);
				}
				this.ProcessPurchaseItemState(null, isLeftHand);
				this.UpdateShoppingCart();
			}
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x0005CBEA File Offset: 0x0005ADEA
		public void PressPurchaseItemButton(PurchaseItemButton pressedPurchaseItemButton, bool isLeftHand)
		{
			this.ProcessPurchaseItemState(pressedPurchaseItemButton.buttonSide, isLeftHand);
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x0005CBF9 File Offset: 0x0005ADF9
		public void PurchaseBundle(StoreBundle bundleToPurchase)
		{
			if (bundleToPurchase.playfabBundleID != "NULL")
			{
				this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Start;
				this.ProcessPurchaseItemState("left", false);
				this.buyingBundle = true;
				this.itemToPurchase = bundleToPurchase.playfabBundleID;
				this.SteamPurchase();
			}
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x0017C508 File Offset: 0x0017A708
		public void PressEarlyAccessButton()
		{
			this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Start;
			this.ProcessPurchaseItemState("left", false);
			this.buyingBundle = true;
			this.itemToPurchase = this.BundlePlayfabItemName;
			ATM_Manager.instance.shinyRocksCost = (float)this.BundleShinyRocks;
			this.SteamPurchase();
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x0005CC39 File Offset: 0x0005AE39
		public void PressPurchaseBundleButton(string PlayFabItemName)
		{
			BundleManager.instance.BundlePurchaseButtonPressed(PlayFabItemName);
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x0017C554 File Offset: 0x0017A754
		public void ProcessPurchaseItemState(string buttonSide, bool isLeftHand)
		{
			switch (this.currentPurchaseItemStage)
			{
			case CosmeticsController.PurchaseItemStages.Start:
				this.itemToBuy = this.nullItem;
				this.FormattedPurchaseText("SELECT AN ITEM FROM YOUR CART TO PURCHASE!");
				this.UpdateShoppingCart();
				return;
			case CosmeticsController.PurchaseItemStages.CheckoutButtonPressed:
				GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.checkout_start, this.currentCart);
				this.searchIndex = this.unlockedCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => this.itemToBuy.itemName == x.itemName);
				if (this.searchIndex > -1)
				{
					this.FormattedPurchaseText("YOU ALREADY OWN THIS ITEM!");
					this.leftPurchaseButton.myText.text = "-";
					this.rightPurchaseButton.myText.text = "-";
					this.leftPurchaseButton.buttonRenderer.material = this.leftPurchaseButton.pressedMaterial;
					this.rightPurchaseButton.buttonRenderer.material = this.rightPurchaseButton.pressedMaterial;
					this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.ItemOwned;
					return;
				}
				if (this.itemToBuy.cost <= this.currencyBalance)
				{
					this.FormattedPurchaseText("DO YOU WANT TO BUY THIS ITEM?");
					this.leftPurchaseButton.myText.text = "NO!";
					this.rightPurchaseButton.myText.text = "YES!";
					this.leftPurchaseButton.buttonRenderer.material = this.leftPurchaseButton.unpressedMaterial;
					this.rightPurchaseButton.buttonRenderer.material = this.rightPurchaseButton.unpressedMaterial;
					this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.ItemSelected;
					return;
				}
				this.FormattedPurchaseText("INSUFFICIENT SHINY ROCKS FOR THIS ITEM!");
				this.leftPurchaseButton.myText.text = "-";
				this.rightPurchaseButton.myText.text = "-";
				this.leftPurchaseButton.buttonRenderer.material = this.leftPurchaseButton.pressedMaterial;
				this.rightPurchaseButton.buttonRenderer.material = this.rightPurchaseButton.pressedMaterial;
				this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Start;
				return;
			case CosmeticsController.PurchaseItemStages.ItemSelected:
				if (buttonSide == "right")
				{
					GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.item_select, this.itemToBuy);
					this.FormattedPurchaseText("ARE YOU REALLY SURE?");
					this.leftPurchaseButton.myText.text = "YES! I NEED IT!";
					this.rightPurchaseButton.myText.text = "LET ME THINK ABOUT IT";
					this.leftPurchaseButton.buttonRenderer.material = this.leftPurchaseButton.unpressedMaterial;
					this.rightPurchaseButton.buttonRenderer.material = this.rightPurchaseButton.unpressedMaterial;
					this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.FinalPurchaseAcknowledgement;
					return;
				}
				this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.CheckoutButtonPressed;
				this.ProcessPurchaseItemState(null, isLeftHand);
				return;
			case CosmeticsController.PurchaseItemStages.ItemOwned:
			case CosmeticsController.PurchaseItemStages.Buying:
				break;
			case CosmeticsController.PurchaseItemStages.FinalPurchaseAcknowledgement:
				if (buttonSide == "left")
				{
					this.FormattedPurchaseText("PURCHASING ITEM...");
					this.leftPurchaseButton.myText.text = "-";
					this.rightPurchaseButton.myText.text = "-";
					this.leftPurchaseButton.buttonRenderer.material = this.leftPurchaseButton.pressedMaterial;
					this.rightPurchaseButton.buttonRenderer.material = this.rightPurchaseButton.pressedMaterial;
					this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Buying;
					this.isLastHandTouchedLeft = isLeftHand;
					this.PurchaseItem();
					return;
				}
				this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.CheckoutButtonPressed;
				this.ProcessPurchaseItemState(null, isLeftHand);
				return;
			case CosmeticsController.PurchaseItemStages.Success:
			{
				this.FormattedPurchaseText("SUCCESS! ENJOY YOUR NEW ITEM!");
				VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
				offlineVRRig.concatStringOfCosmeticsAllowed += this.itemToBuy.itemName;
				CosmeticsController.CosmeticItem itemFromDict = this.GetItemFromDict(this.itemToBuy.itemName);
				if (itemFromDict.bundledItems != null)
				{
					foreach (string str in itemFromDict.bundledItems)
					{
						VRRig offlineVRRig2 = GorillaTagger.Instance.offlineVRRig;
						offlineVRRig2.concatStringOfCosmeticsAllowed += str;
					}
				}
				this.tryOnSet.ClearSet(this.nullItem);
				this.UpdateShoppingCart();
				this.ApplyCosmeticItemToSet(this.currentWornSet, itemFromDict, isLeftHand, true);
				this.UpdateShoppingCart();
				this.UpdateWornCosmetics(false);
				this.UpdateWardrobeModelsAndButtons();
				Action onCosmeticsUpdated = this.OnCosmeticsUpdated;
				if (onCosmeticsUpdated != null)
				{
					onCosmeticsUpdated();
				}
				this.leftPurchaseButton.myText.text = "-";
				this.rightPurchaseButton.myText.text = "-";
				this.leftPurchaseButton.buttonRenderer.material = this.leftPurchaseButton.pressedMaterial;
				this.rightPurchaseButton.buttonRenderer.material = this.rightPurchaseButton.pressedMaterial;
				break;
			}
			case CosmeticsController.PurchaseItemStages.Failure:
				this.FormattedPurchaseText("ERROR IN PURCHASING ITEM! NO MONEY WAS SPENT. SELECT ANOTHER ITEM.");
				this.leftPurchaseButton.myText.text = "-";
				this.rightPurchaseButton.myText.text = "-";
				this.leftPurchaseButton.buttonRenderer.material = this.leftPurchaseButton.pressedMaterial;
				this.rightPurchaseButton.buttonRenderer.material = this.rightPurchaseButton.pressedMaterial;
				return;
			default:
				return;
			}
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x0017CA34 File Offset: 0x0017AC34
		public void FormattedPurchaseText(string finalLineVar)
		{
			this.finalLine = finalLineVar;
			this.purchaseText.text = string.Concat(new string[]
			{
				"SELECTION: ",
				this.GetItemDisplayName(this.itemToBuy),
				"\nITEM COST: ",
				this.itemToBuy.cost.ToString(),
				"\nYOU HAVE: ",
				this.currencyBalance.ToString(),
				"\n\n",
				this.finalLine
			});
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x0017CAB8 File Offset: 0x0017ACB8
		public void PurchaseItem()
		{
			PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
			{
				ItemId = this.itemToBuy.itemName,
				Price = this.itemToBuy.cost,
				VirtualCurrency = this.currencyName,
				CatalogVersion = this.catalog
			}, delegate(PurchaseItemResult result)
			{
				if (result.Items.Count > 0)
				{
					foreach (ItemInstance itemInstance in result.Items)
					{
						CosmeticsController.CosmeticItem itemFromDict = this.GetItemFromDict(this.itemToBuy.itemName);
						if (itemFromDict.itemCategory == CosmeticsController.CosmeticCategory.Set)
						{
							this.UnlockItem(itemInstance.ItemId);
							foreach (string itemIdToUnlock in itemFromDict.bundledItems)
							{
								this.UnlockItem(itemIdToUnlock);
							}
						}
						else
						{
							this.UnlockItem(itemInstance.ItemId);
						}
					}
					this.UpdateMyCosmetics();
					if (NetworkSystem.Instance.InRoom)
					{
						base.StartCoroutine(this.CheckIfMyCosmeticsUpdated(this.itemToBuy.itemName));
					}
					this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Success;
					this.currencyBalance -= this.itemToBuy.cost;
					this.UpdateShoppingCart();
					this.ProcessPurchaseItemState(null, this.isLastHandTouchedLeft);
					return;
				}
				this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Failure;
				this.ProcessPurchaseItemState(null, false);
			}, delegate(PlayFabError error)
			{
				this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Failure;
				this.ProcessPurchaseItemState(null, false);
			}, null, null);
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x0017CB24 File Offset: 0x0017AD24
		private void UnlockItem(string itemIdToUnlock)
		{
			int num = this.allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => itemIdToUnlock == x.itemName);
			if (num > -1)
			{
				if (!this.unlockedCosmetics.Contains(this.allCosmetics[num]))
				{
					this.unlockedCosmetics.Add(this.allCosmetics[num]);
				}
				this.concatStringCosmeticsAllowed += this.allCosmetics[num].itemName;
				switch (this.allCosmetics[num].itemCategory)
				{
				case CosmeticsController.CosmeticCategory.Hat:
					if (!this.unlockedHats.Contains(this.allCosmetics[num]))
					{
						this.unlockedHats.Add(this.allCosmetics[num]);
						return;
					}
					break;
				case CosmeticsController.CosmeticCategory.Badge:
					if (!this.unlockedBadges.Contains(this.allCosmetics[num]))
					{
						this.unlockedBadges.Add(this.allCosmetics[num]);
						return;
					}
					break;
				case CosmeticsController.CosmeticCategory.Face:
					if (!this.unlockedFaces.Contains(this.allCosmetics[num]))
					{
						this.unlockedFaces.Add(this.allCosmetics[num]);
						return;
					}
					break;
				case CosmeticsController.CosmeticCategory.Paw:
					if (!this.unlockedPaws.Contains(this.allCosmetics[num]))
					{
						this.unlockedPaws.Add(this.allCosmetics[num]);
						return;
					}
					break;
				case CosmeticsController.CosmeticCategory.Chest:
					if (!this.unlockedChests.Contains(this.allCosmetics[num]))
					{
						this.unlockedChests.Add(this.allCosmetics[num]);
						return;
					}
					break;
				case CosmeticsController.CosmeticCategory.Fur:
					if (!this.unlockedFurs.Contains(this.allCosmetics[num]))
					{
						this.unlockedFurs.Add(this.allCosmetics[num]);
						return;
					}
					break;
				case CosmeticsController.CosmeticCategory.Shirt:
					if (!this.unlockedShirts.Contains(this.allCosmetics[num]))
					{
						this.unlockedShirts.Add(this.allCosmetics[num]);
						return;
					}
					break;
				case CosmeticsController.CosmeticCategory.Back:
					if (!this.unlockedBacks.Contains(this.allCosmetics[num]))
					{
						this.unlockedBacks.Add(this.allCosmetics[num]);
						return;
					}
					break;
				case CosmeticsController.CosmeticCategory.Arms:
					if (!this.unlockedArms.Contains(this.allCosmetics[num]))
					{
						this.unlockedArms.Add(this.allCosmetics[num]);
						return;
					}
					break;
				case CosmeticsController.CosmeticCategory.Pants:
					if (!this.unlockedPants.Contains(this.allCosmetics[num]))
					{
						this.unlockedPants.Add(this.allCosmetics[num]);
						return;
					}
					break;
				case CosmeticsController.CosmeticCategory.TagEffect:
					if (!this.unlockedTagFX.Contains(this.allCosmetics[num]))
					{
						this.unlockedTagFX.Add(this.allCosmetics[num]);
						return;
					}
					break;
				case CosmeticsController.CosmeticCategory.Count:
					break;
				case CosmeticsController.CosmeticCategory.Set:
					foreach (string itemIdToUnlock2 in this.allCosmetics[num].bundledItems)
					{
						this.UnlockItem(itemIdToUnlock2);
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600449B RID: 17563 RVA: 0x0005CC48 File Offset: 0x0005AE48
		private IEnumerator CheckIfMyCosmeticsUpdated(string itemToBuyID)
		{
			Debug.Log("Cosmetic updated check!");
			yield return new WaitForSeconds(1f);
			this.foundCosmetic = false;
			this.attempts = 0;
			while (!this.foundCosmetic && this.attempts < 10 && NetworkSystem.Instance.InRoom)
			{
				this.playerIDList.Clear();
				if (this.UseNewCosmeticsPath())
				{
					this.playerIDList.Add("Inventory");
					PlayFabClientAPI.GetSharedGroupData(new PlayFab.ClientModels.GetSharedGroupDataRequest
					{
						Keys = this.playerIDList,
						SharedGroupId = NetworkSystem.Instance.LocalPlayer.UserId + "Inventory"
					}, delegate(GetSharedGroupDataResult result)
					{
						this.attempts++;
						foreach (KeyValuePair<string, PlayFab.ClientModels.SharedGroupDataRecord> keyValuePair in result.Data)
						{
							if (keyValuePair.Value.Value.Contains(itemToBuyID))
							{
								PhotonNetwork.RaiseEvent(199, null, new RaiseEventOptions
								{
									Receivers = ReceiverGroup.Others
								}, SendOptions.SendReliable);
								this.foundCosmetic = true;
							}
						}
						if (this.foundCosmetic)
						{
							this.UpdateWornCosmetics(true);
						}
					}, delegate(PlayFabError error)
					{
						this.attempts++;
						this.ReauthOrBan(error);
					}, null, null);
					yield return new WaitForSeconds(1f);
				}
				else
				{
					this.playerIDList.Add(PhotonNetwork.LocalPlayer.ActorNumber.ToString());
					PlayFabClientAPI.GetSharedGroupData(new PlayFab.ClientModels.GetSharedGroupDataRequest
					{
						Keys = this.playerIDList,
						SharedGroupId = NetworkSystem.Instance.RoomName + Regex.Replace(NetworkSystem.Instance.CurrentRegion, "[^a-zA-Z0-9]", "").ToUpper()
					}, delegate(GetSharedGroupDataResult result)
					{
						this.attempts++;
						foreach (KeyValuePair<string, PlayFab.ClientModels.SharedGroupDataRecord> keyValuePair in result.Data)
						{
							if (keyValuePair.Value.Value.Contains(itemToBuyID))
							{
								NetworkSystemRaiseEvent.RaiseEvent(199, null, NetworkSystemRaiseEvent.neoOthers, true);
								this.foundCosmetic = true;
							}
							else
							{
								Debug.Log("didnt find it, updating attempts and trying again in a bit. current attempt is " + this.attempts.ToString());
							}
						}
						if (this.foundCosmetic)
						{
							this.UpdateWornCosmetics(true);
						}
					}, delegate(PlayFabError error)
					{
						this.attempts++;
						if (error.Error == PlayFabErrorCode.NotAuthenticated)
						{
							PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
						}
						else if (error.Error == PlayFabErrorCode.AccountBanned)
						{
							GorillaGameManager.ForceStopGame_DisconnectAndDestroy();
						}
						Debug.Log("Got error retrieving user data, on attempt " + this.attempts.ToString());
						Debug.Log(error.GenerateErrorReport());
					}, null, null);
					yield return new WaitForSeconds(1f);
				}
			}
			Debug.Log("done!");
			yield break;
		}

		// Token: 0x0600449C RID: 17564 RVA: 0x0017CE68 File Offset: 0x0017B068
		public void UpdateWardrobeModelsAndButtons()
		{
			if (!CosmeticsV2Spawner_Dirty.allPartsInstantiated)
			{
				return;
			}
			foreach (WardrobeInstance wardrobeInstance in this.wardrobes)
			{
				wardrobeInstance.wardrobeItemButtons[0].currentCosmeticItem = ((this.cosmeticsPages[this.wardrobeType] * 3 < this.itemLists[this.wardrobeType].Count) ? this.itemLists[this.wardrobeType][this.cosmeticsPages[this.wardrobeType] * 3] : this.nullItem);
				wardrobeInstance.wardrobeItemButtons[1].currentCosmeticItem = ((this.cosmeticsPages[this.wardrobeType] * 3 + 1 < this.itemLists[this.wardrobeType].Count) ? this.itemLists[this.wardrobeType][this.cosmeticsPages[this.wardrobeType] * 3 + 1] : this.nullItem);
				wardrobeInstance.wardrobeItemButtons[2].currentCosmeticItem = ((this.cosmeticsPages[this.wardrobeType] * 3 + 2 < this.itemLists[this.wardrobeType].Count) ? this.itemLists[this.wardrobeType][this.cosmeticsPages[this.wardrobeType] * 3 + 2] : this.nullItem);
				this.iterator = 0;
				while (this.iterator < wardrobeInstance.wardrobeItemButtons.Length)
				{
					CosmeticsController.CosmeticItem currentCosmeticItem = wardrobeInstance.wardrobeItemButtons[this.iterator].currentCosmeticItem;
					wardrobeInstance.wardrobeItemButtons[this.iterator].isOn = (!currentCosmeticItem.isNullItem && this.AnyMatch(this.currentWornSet, currentCosmeticItem));
					wardrobeInstance.wardrobeItemButtons[this.iterator].UpdateColor();
					this.iterator++;
				}
				wardrobeInstance.wardrobeItemButtons[0].controlledModel.SetCosmeticActive(wardrobeInstance.wardrobeItemButtons[0].currentCosmeticItem.displayName, false);
				wardrobeInstance.wardrobeItemButtons[1].controlledModel.SetCosmeticActive(wardrobeInstance.wardrobeItemButtons[1].currentCosmeticItem.displayName, false);
				wardrobeInstance.wardrobeItemButtons[2].controlledModel.SetCosmeticActive(wardrobeInstance.wardrobeItemButtons[2].currentCosmeticItem.displayName, false);
				wardrobeInstance.selfDoll.SetCosmeticActiveArray(this.currentWornSet.ToDisplayNameArray(), this.currentWornSet.ToOnRightSideArray());
			}
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x0017D0E8 File Offset: 0x0017B2E8
		public int GetCategorySize(CosmeticsController.CosmeticCategory category)
		{
			int indexForCategory = this.GetIndexForCategory(category);
			if (indexForCategory != -1)
			{
				return this.itemLists[indexForCategory].Count;
			}
			return 0;
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x0005CC5E File Offset: 0x0005AE5E
		public CosmeticsController.CosmeticItem GetCosmetic(int category, int cosmeticIndex)
		{
			if (cosmeticIndex >= this.itemLists[category].Count || cosmeticIndex < 0)
			{
				return this.nullItem;
			}
			return this.itemLists[category][cosmeticIndex];
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x0005CC89 File Offset: 0x0005AE89
		public CosmeticsController.CosmeticItem GetCosmetic(CosmeticsController.CosmeticCategory category, int cosmeticIndex)
		{
			return this.GetCosmetic(this.GetIndexForCategory(category), cosmeticIndex);
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x0017D110 File Offset: 0x0017B310
		private int GetIndexForCategory(CosmeticsController.CosmeticCategory category)
		{
			switch (category)
			{
			case CosmeticsController.CosmeticCategory.Hat:
				return 0;
			case CosmeticsController.CosmeticCategory.Badge:
				return 2;
			case CosmeticsController.CosmeticCategory.Face:
				return 1;
			case CosmeticsController.CosmeticCategory.Paw:
				return 3;
			case CosmeticsController.CosmeticCategory.Chest:
				return 9;
			case CosmeticsController.CosmeticCategory.Fur:
				return 4;
			case CosmeticsController.CosmeticCategory.Shirt:
				return 5;
			case CosmeticsController.CosmeticCategory.Back:
				return 8;
			case CosmeticsController.CosmeticCategory.Arms:
				return 7;
			case CosmeticsController.CosmeticCategory.Pants:
				return 6;
			case CosmeticsController.CosmeticCategory.TagEffect:
				return 10;
			default:
				return 0;
			}
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x0005CC99 File Offset: 0x0005AE99
		public bool IsCosmeticEquipped(CosmeticsController.CosmeticItem cosmetic)
		{
			return this.AnyMatch(this.currentWornSet, cosmetic);
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x0017D16C File Offset: 0x0017B36C
		public CosmeticsController.CosmeticItem GetSlotItem(CosmeticsController.CosmeticSlots slot, bool checkOpposite = true)
		{
			int num = (int)slot;
			if (checkOpposite)
			{
				num = (int)CosmeticsController.CosmeticSet.OppositeSlot(slot);
			}
			return this.currentWornSet.items[num];
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x0005CCA8 File Offset: 0x0005AEA8
		public string[] GetCurrentlyWornCosmetics()
		{
			return this.currentWornSet.ToDisplayNameArray();
		}

		// Token: 0x060044A4 RID: 17572 RVA: 0x0005CCB5 File Offset: 0x0005AEB5
		public bool[] GetCurrentRightEquippedSided()
		{
			return this.currentWornSet.ToOnRightSideArray();
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x0017D198 File Offset: 0x0017B398
		public void UpdateShoppingCart()
		{
			this.iterator = 0;
			while (this.iterator < this.fittingRoomButtons.Length)
			{
				if (this.iterator < this.currentCart.Count)
				{
					this.fittingRoomButtons[this.iterator].currentCosmeticItem = this.currentCart[this.iterator];
					this.checkoutCartButtons[this.iterator].currentCosmeticItem = this.currentCart[this.iterator];
					this.checkoutCartButtons[this.iterator].isOn = (this.checkoutCartButtons[this.iterator].currentCosmeticItem.itemName == this.itemToBuy.itemName);
					this.fittingRoomButtons[this.iterator].isOn = this.AnyMatch(this.tryOnSet, this.fittingRoomButtons[this.iterator].currentCosmeticItem);
				}
				else
				{
					this.checkoutCartButtons[this.iterator].currentCosmeticItem = this.nullItem;
					this.fittingRoomButtons[this.iterator].currentCosmeticItem = this.nullItem;
					this.checkoutCartButtons[this.iterator].isOn = false;
					this.fittingRoomButtons[this.iterator].isOn = false;
				}
				this.checkoutCartButtons[this.iterator].currentImage.sprite = this.checkoutCartButtons[this.iterator].currentCosmeticItem.itemPicture;
				this.fittingRoomButtons[this.iterator].currentImage.sprite = this.fittingRoomButtons[this.iterator].currentCosmeticItem.itemPicture;
				this.checkoutCartButtons[this.iterator].UpdateColor();
				this.fittingRoomButtons[this.iterator].UpdateColor();
				this.iterator++;
			}
			if (CosmeticsV2Spawner_Dirty.allPartsInstantiated)
			{
				this.UpdateWardrobeModelsAndButtons();
			}
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x0017D37C File Offset: 0x0017B57C
		public void UpdateWornCosmetics(bool sync = false)
		{
			GorillaTagger.Instance.offlineVRRig.LocalUpdateCosmeticsWithTryon(this.currentWornSet, this.tryOnSet);
			if (sync && GorillaTagger.Instance.myVRRig != null)
			{
				if (this.isHidingCosmeticsFromRemotePlayers)
				{
					GorillaTagger.Instance.myVRRig.SendRPC("RPC_HideAllCosmetics", RpcTarget.All, Array.Empty<object>());
					return;
				}
				int[] array = this.currentWornSet.ToPackedIDArray();
				int[] array2 = this.tryOnSet.ToPackedIDArray();
				GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryonPacked", RpcTarget.Others, new object[]
				{
					array,
					array2
				});
			}
		}

		// Token: 0x060044A7 RID: 17575 RVA: 0x0005CCC2 File Offset: 0x0005AEC2
		public CosmeticsController.CosmeticItem GetItemFromDict(string itemID)
		{
			if (!this.allCosmeticsDict.TryGetValue(itemID, out this.cosmeticItemVar))
			{
				return this.nullItem;
			}
			return this.cosmeticItemVar;
		}

		// Token: 0x060044A8 RID: 17576 RVA: 0x0005CCE5 File Offset: 0x0005AEE5
		public string GetItemNameFromDisplayName(string displayName)
		{
			if (!this.allCosmeticsItemIDsfromDisplayNamesDict.TryGetValue(displayName, out this.returnString))
			{
				return "null";
			}
			return this.returnString;
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x0017D418 File Offset: 0x0017B618
		public bool AnyMatch(CosmeticsController.CosmeticSet set, CosmeticsController.CosmeticItem item)
		{
			if (item.itemCategory != CosmeticsController.CosmeticCategory.Set)
			{
				return set.IsActive(item.displayName);
			}
			if (item.bundledItems.Length == 1)
			{
				return this.AnyMatch(set, this.GetItemFromDict(item.bundledItems[0]));
			}
			if (item.bundledItems.Length == 2)
			{
				return this.AnyMatch(set, this.GetItemFromDict(item.bundledItems[0])) || this.AnyMatch(set, this.GetItemFromDict(item.bundledItems[1]));
			}
			return item.bundledItems.Length >= 3 && (this.AnyMatch(set, this.GetItemFromDict(item.bundledItems[0])) || this.AnyMatch(set, this.GetItemFromDict(item.bundledItems[1])) || this.AnyMatch(set, this.GetItemFromDict(item.bundledItems[2])));
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x0017D4EC File Offset: 0x0017B6EC
		public void Initialize()
		{
			if (!base.gameObject.activeSelf || this.v2_isCosmeticPlayFabCatalogDataLoaded || this.v2_isGetCosmeticsPlayCatalogDataWaitingForCallback)
			{
				return;
			}
			if (this.v2_allCosmeticsInfoAssetRef_isLoaded)
			{
				this.GetCosmeticsPlayFabCatalogData();
				return;
			}
			this.v2_isGetCosmeticsPlayCatalogDataWaitingForCallback = true;
			this.V2_allCosmeticsInfoAssetRef_OnPostLoad = (Action)Delegate.Combine(this.V2_allCosmeticsInfoAssetRef_OnPostLoad, new Action(this.GetCosmeticsPlayFabCatalogData));
		}

		// Token: 0x060044AB RID: 17579 RVA: 0x0005CD07 File Offset: 0x0005AF07
		public void GetLastDailyLogin()
		{
			PlayFabClientAPI.GetUserReadOnlyData(new PlayFab.ClientModels.GetUserDataRequest(), delegate(GetUserDataResult result)
			{
				if (result.Data.TryGetValue("DailyLogin", out this.userDataRecord))
				{
					this.lastDailyLogin = this.userDataRecord.Value;
					return;
				}
				this.lastDailyLogin = "NONE";
				base.StartCoroutine(this.GetMyDaily());
			}, delegate(PlayFabError error)
			{
				Debug.Log("Got error getting read-only user data:");
				Debug.Log(error.GenerateErrorReport());
				this.lastDailyLogin = "FAILED";
				if (error.Error == PlayFabErrorCode.NotAuthenticated)
				{
					PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
					return;
				}
				if (error.Error == PlayFabErrorCode.AccountBanned)
				{
					Application.Quit();
					NetworkSystem.Instance.ReturnToSinglePlayer();
					UnityEngine.Object.DestroyImmediate(PhotonNetworkController.Instance);
					UnityEngine.Object.DestroyImmediate(GTPlayer.Instance);
					GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
					for (int i = 0; i < array.Length; i++)
					{
						UnityEngine.Object.Destroy(array[i]);
					}
				}
			}, null, null);
		}

		// Token: 0x060044AC RID: 17580 RVA: 0x0005CD2D File Offset: 0x0005AF2D
		private IEnumerator CheckCanGetDaily()
		{
			for (;;)
			{
				if (GorillaComputer.instance != null && GorillaComputer.instance.startupMillis != 0L)
				{
					this.currentTime = new DateTime((GorillaComputer.instance.startupMillis + (long)(Time.realtimeSinceStartup * 1000f)) * 10000L);
					this.secondsUntilTomorrow = (int)(this.currentTime.AddDays(1.0).Date - this.currentTime).TotalSeconds;
					if (this.lastDailyLogin == null || this.lastDailyLogin == "")
					{
						this.GetLastDailyLogin();
					}
					else if (this.currentTime.ToString("o").Substring(0, 10) == this.lastDailyLogin)
					{
						this.checkedDaily = true;
						this.gotMyDaily = true;
					}
					else if (this.currentTime.ToString("o").Substring(0, 10) != this.lastDailyLogin)
					{
						this.checkedDaily = true;
						this.gotMyDaily = false;
						base.StartCoroutine(this.GetMyDaily());
					}
					else if (this.lastDailyLogin == "FAILED")
					{
						this.GetLastDailyLogin();
					}
					this.secondsToWaitToCheckDaily = (this.checkedDaily ? 60f : 10f);
					this.UpdateCurrencyBoard();
					yield return new WaitForSeconds(this.secondsToWaitToCheckDaily);
				}
				else
				{
					yield return new WaitForSeconds(1f);
				}
			}
			yield break;
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x0005CD3C File Offset: 0x0005AF3C
		private IEnumerator GetMyDaily()
		{
			yield return new WaitForSeconds(10f);
			GorillaServer.Instance.TryDistributeCurrency(delegate(ExecuteFunctionResult result)
			{
				this.GetCurrencyBalance();
				this.GetLastDailyLogin();
			}, delegate(PlayFabError error)
			{
				if (error.Error == PlayFabErrorCode.NotAuthenticated)
				{
					PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
					return;
				}
				if (error.Error == PlayFabErrorCode.AccountBanned)
				{
					Application.Quit();
					NetworkSystem.Instance.ReturnToSinglePlayer();
					UnityEngine.Object.DestroyImmediate(PhotonNetworkController.Instance);
					UnityEngine.Object.DestroyImmediate(GTPlayer.Instance);
					GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
					for (int i = 0; i < array.Length; i++)
					{
						UnityEngine.Object.Destroy(array[i]);
					}
				}
			});
			yield break;
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x0005CD4B File Offset: 0x0005AF4B
		public void GetCosmeticsPlayFabCatalogData()
		{
			this.v2_isGetCosmeticsPlayCatalogDataWaitingForCallback = false;
			if (!this.v2_allCosmeticsInfoAssetRef_isLoaded)
			{
				throw new Exception("Method `GetCosmeticsPlayFabCatalogData` was called before `v2_allCosmeticsInfoAssetRef` was loaded. Listen to callback `V2_allCosmeticsInfoAssetRef_OnPostLoad` or check `v2_allCosmeticsInfoAssetRef_isLoaded` before trying to get PlayFab catalog data.");
			}
			PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), delegate(GetUserInventoryResult result)
			{
				PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest
				{
					CatalogVersion = this.catalog
				}, delegate(GetCatalogItemsResult result2)
				{
					this.unlockedCosmetics.Clear();
					this.unlockedHats.Clear();
					this.unlockedBadges.Clear();
					this.unlockedFaces.Clear();
					this.unlockedPaws.Clear();
					this.unlockedFurs.Clear();
					this.unlockedShirts.Clear();
					this.unlockedPants.Clear();
					this.unlockedArms.Clear();
					this.unlockedBacks.Clear();
					this.unlockedChests.Clear();
					this.unlockedTagFX.Clear();
					this.catalogItems = result2.Catalog;
					using (List<CatalogItem>.Enumerator enumerator = this.catalogItems.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							CatalogItem catalogItem = enumerator.Current;
							if (!BuilderSetManager.IsItemIDBuilderItem(catalogItem.ItemId))
							{
								this.searchIndex = this.allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => catalogItem.ItemId == x.itemName);
								if (this.searchIndex > -1)
								{
									this.tempStringArray = null;
									this.hasPrice = false;
									if (catalogItem.Bundle != null)
									{
										this.tempStringArray = catalogItem.Bundle.BundledItems.ToArray();
									}
									uint cost;
									if (catalogItem.VirtualCurrencyPrices.TryGetValue(this.currencyName, out cost))
									{
										this.hasPrice = true;
									}
									CosmeticsController.CosmeticItem cosmeticItem = this.allCosmetics[this.searchIndex];
									cosmeticItem.itemName = catalogItem.ItemId;
									cosmeticItem.displayName = catalogItem.DisplayName;
									cosmeticItem.cost = (int)cost;
									cosmeticItem.bundledItems = this.tempStringArray;
									cosmeticItem.canTryOn = this.hasPrice;
									if (cosmeticItem.displayName == null)
									{
										string text = "null";
										if (this.allCosmetics[this.searchIndex].itemPicture)
										{
											text = this.allCosmetics[this.searchIndex].itemPicture.name;
										}
										string debugCosmeticSOName = this.v2_allCosmetics[this.searchIndex].debugCosmeticSOName;
										Debug.LogError(string.Concat(new string[]
										{
											string.Format("Cosmetic encountered with a null displayName at index {0}! ", this.searchIndex),
											"Setting displayName to id: \"",
											this.allCosmetics[this.searchIndex].itemName,
											"\". iconName=\"",
											text,
											"\".cosmeticSOName=\"",
											debugCosmeticSOName,
											"\". "
										}));
										cosmeticItem.displayName = cosmeticItem.itemName;
									}
									this.V2_ConformCosmeticItemV1DisplayName(ref cosmeticItem);
									this._allCosmetics[this.searchIndex] = cosmeticItem;
									this._allCosmeticsDict[cosmeticItem.itemName] = cosmeticItem;
									this._allCosmeticsItemIDsfromDisplayNamesDict[cosmeticItem.displayName] = cosmeticItem.itemName;
									this._allCosmeticsItemIDsfromDisplayNamesDict[cosmeticItem.overrideDisplayName] = cosmeticItem.itemName;
								}
							}
						}
					}
					for (int i = this._allCosmetics.Count - 1; i > -1; i--)
					{
						this.tempItem = this._allCosmetics[i];
						if (this.tempItem.itemCategory == CosmeticsController.CosmeticCategory.Set && this.tempItem.canTryOn)
						{
							string[] bundledItems = this.tempItem.bundledItems;
							for (int j = 0; j < bundledItems.Length; j++)
							{
								string setItemName = bundledItems[j];
								this.searchIndex = this._allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => setItemName == x.itemName);
								if (this.searchIndex > -1)
								{
									this.tempItem = this._allCosmetics[this.searchIndex];
									this.tempItem.canTryOn = true;
									this._allCosmetics[this.searchIndex] = this.tempItem;
									this._allCosmeticsDict[this._allCosmetics[this.searchIndex].itemName] = this.tempItem;
									this._allCosmeticsItemIDsfromDisplayNamesDict[this._allCosmetics[this.searchIndex].displayName] = this.tempItem.itemName;
								}
							}
						}
					}
					foreach (KeyValuePair<string, StoreBundle> keyValuePair in BundleManager.instance.storeBundlesById)
					{
						string text2;
						StoreBundle bundleData2;
						keyValuePair.Deconstruct(out text2, out bundleData2);
						string key = text2;
						StoreBundle bundleData = bundleData2;
						int num = this._allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => bundleData.playfabBundleID == x.itemName);
						if (num > 0 && this._allCosmetics[num].bundledItems != null)
						{
							string[] bundledItems = this._allCosmetics[num].bundledItems;
							for (int j = 0; j < bundledItems.Length; j++)
							{
								string setItemName = bundledItems[j];
								this.searchIndex = this._allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => setItemName == x.itemName);
								if (this.searchIndex > -1)
								{
									this.tempItem = this._allCosmetics[this.searchIndex];
									this.tempItem.canTryOn = true;
									this._allCosmetics[this.searchIndex] = this.tempItem;
									this._allCosmeticsDict[this._allCosmetics[this.searchIndex].itemName] = this.tempItem;
									this._allCosmeticsItemIDsfromDisplayNamesDict[this._allCosmetics[this.searchIndex].displayName] = this.tempItem.itemName;
								}
							}
						}
						if (!bundleData.HasPrice)
						{
							num = this.catalogItems.FindIndex((CatalogItem ci) => ci.Bundle != null && ci.ItemId == bundleData.playfabBundleID);
							if (num > 0)
							{
								uint bundlePrice;
								if (this.catalogItems[num].VirtualCurrencyPrices.TryGetValue("RM", out bundlePrice))
								{
									BundleManager.instance.storeBundlesById[key].TryUpdatePrice(bundlePrice);
								}
								else
								{
									BundleManager.instance.storeBundlesById[key].TryUpdatePrice(null);
								}
							}
						}
					}
					this.searchIndex = this._allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => "Slingshot" == x.itemName);
					if (this.searchIndex < 0)
					{
						throw new MissingReferenceException("CosmeticsController: Cannot find default slingshot! it is required for players that do not have another slingshot equipped and are playing Paintbrawl.");
					}
					this._allCosmeticsDict["Slingshot"] = this._allCosmetics[this.searchIndex];
					this._allCosmeticsItemIDsfromDisplayNamesDict[this._allCosmetics[this.searchIndex].displayName] = this._allCosmetics[this.searchIndex].itemName;
					this.allCosmeticsDict_isInitialized = true;
					this.allCosmeticsItemIDsfromDisplayNamesDict_isInitialized = true;
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					using (List<ItemInstance>.Enumerator enumerator3 = result.Inventory.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							ItemInstance item = enumerator3.Current;
							if (!BuilderSetManager.IsItemIDBuilderItem(item.ItemId))
							{
								if (item.ItemId == this.m_earlyAccessSupporterPackCosmeticSO.info.playFabID)
								{
									foreach (CosmeticSO cosmeticSO in this.m_earlyAccessSupporterPackCosmeticSO.info.setCosmetics)
									{
										CosmeticsController.CosmeticItem item2;
										if (this.allCosmeticsDict.TryGetValue(cosmeticSO.info.playFabID, out item2))
										{
											this.unlockedCosmetics.Add(item2);
										}
									}
								}
								BundleManager.instance.MarkBundleOwnedByPlayFabID(item.ItemId);
								if (!dictionary.ContainsKey(item.ItemId))
								{
									this.searchIndex = this.allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => item.ItemId == x.itemName);
									if (this.searchIndex > -1)
									{
										dictionary[item.ItemId] = item.ItemId;
										this.unlockedCosmetics.Add(this.allCosmetics[this.searchIndex]);
									}
								}
							}
						}
					}
					foreach (CosmeticsController.CosmeticItem cosmeticItem2 in this.unlockedCosmetics)
					{
						if (cosmeticItem2.itemCategory == CosmeticsController.CosmeticCategory.Hat && !this.unlockedHats.Contains(cosmeticItem2))
						{
							this.unlockedHats.Add(cosmeticItem2);
						}
						else if (cosmeticItem2.itemCategory == CosmeticsController.CosmeticCategory.Face && !this.unlockedFaces.Contains(cosmeticItem2))
						{
							this.unlockedFaces.Add(cosmeticItem2);
						}
						else if (cosmeticItem2.itemCategory == CosmeticsController.CosmeticCategory.Badge && !this.unlockedBadges.Contains(cosmeticItem2))
						{
							this.unlockedBadges.Add(cosmeticItem2);
						}
						else if (cosmeticItem2.itemCategory == CosmeticsController.CosmeticCategory.Paw && !this.unlockedPaws.Contains(cosmeticItem2))
						{
							this.unlockedPaws.Add(cosmeticItem2);
						}
						else if (cosmeticItem2.itemCategory == CosmeticsController.CosmeticCategory.Fur && !this.unlockedFurs.Contains(cosmeticItem2))
						{
							this.unlockedFurs.Add(cosmeticItem2);
						}
						else if (cosmeticItem2.itemCategory == CosmeticsController.CosmeticCategory.Shirt && !this.unlockedShirts.Contains(cosmeticItem2))
						{
							this.unlockedShirts.Add(cosmeticItem2);
						}
						else if (cosmeticItem2.itemCategory == CosmeticsController.CosmeticCategory.Arms && !this.unlockedArms.Contains(cosmeticItem2))
						{
							this.unlockedArms.Add(cosmeticItem2);
						}
						else if (cosmeticItem2.itemCategory == CosmeticsController.CosmeticCategory.Back && !this.unlockedBacks.Contains(cosmeticItem2))
						{
							this.unlockedBacks.Add(cosmeticItem2);
						}
						else if (cosmeticItem2.itemCategory == CosmeticsController.CosmeticCategory.Chest && !this.unlockedChests.Contains(cosmeticItem2))
						{
							this.unlockedChests.Add(cosmeticItem2);
						}
						else if (cosmeticItem2.itemCategory == CosmeticsController.CosmeticCategory.Pants && !this.unlockedPants.Contains(cosmeticItem2))
						{
							this.unlockedPants.Add(cosmeticItem2);
						}
						else if (cosmeticItem2.itemCategory == CosmeticsController.CosmeticCategory.TagEffect && !this.unlockedTagFX.Contains(cosmeticItem2))
						{
							this.unlockedTagFX.Add(cosmeticItem2);
						}
						this.concatStringCosmeticsAllowed += cosmeticItem2.itemName;
					}
					BuilderSetManager.instance.OnGotInventoryItems(result, result2);
					this.currencyBalance = result.VirtualCurrency[this.currencyName];
					int num2;
					this.playedInBeta = (result.VirtualCurrency.TryGetValue("TC", out num2) && num2 > 0);
					Action onGetCurrency = this.OnGetCurrency;
					if (onGetCurrency != null)
					{
						onGetCurrency();
					}
					BundleManager.instance.CheckIfBundlesOwned();
					StoreUpdater.instance.Initialize();
					this.currentWornSet.LoadFromPlayerPreferences(this);
					if (!ATM_Manager.instance.alreadyBegan)
					{
						ATM_Manager.instance.SwitchToStage(ATM_Manager.ATMStages.Begin);
						ATM_Manager.instance.alreadyBegan = true;
					}
					this.ProcessPurchaseItemState(null, false);
					this.UpdateShoppingCart();
					this.UpdateCurrencyBoard();
					if (this.UseNewCosmeticsPath())
					{
						this.ConfirmIndividualCosmeticsSharedGroup(result);
					}
					Action onCosmeticsUpdated = this.OnCosmeticsUpdated;
					if (onCosmeticsUpdated != null)
					{
						onCosmeticsUpdated();
					}
					this.v2_isCosmeticPlayFabCatalogDataLoaded = true;
					Action v2_OnGetCosmeticsPlayFabCatalogData_PostSuccess = this.V2_OnGetCosmeticsPlayFabCatalogData_PostSuccess;
					if (v2_OnGetCosmeticsPlayFabCatalogData_PostSuccess != null)
					{
						v2_OnGetCosmeticsPlayFabCatalogData_PostSuccess();
					}
					if (!CosmeticsV2Spawner_Dirty.startedAllPartsInstantiated && !CosmeticsV2Spawner_Dirty.allPartsInstantiated)
					{
						CosmeticsV2Spawner_Dirty.StartInstantiatingPrefabs();
					}
				}, delegate(PlayFabError error)
				{
					if (error.Error == PlayFabErrorCode.NotAuthenticated)
					{
						PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
					}
					else if (error.Error == PlayFabErrorCode.AccountBanned)
					{
						Application.Quit();
						NetworkSystem.Instance.ReturnToSinglePlayer();
						UnityEngine.Object.DestroyImmediate(PhotonNetworkController.Instance);
						UnityEngine.Object.DestroyImmediate(GTPlayer.Instance);
						GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
						for (int i = 0; i < array.Length; i++)
						{
							UnityEngine.Object.Destroy(array[i]);
						}
					}
					if (!this.tryTwice)
					{
						this.tryTwice = true;
						this.GetCosmeticsPlayFabCatalogData();
					}
				}, null, null);
			}, delegate(PlayFabError error)
			{
				if (error.Error == PlayFabErrorCode.NotAuthenticated)
				{
					PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
				}
				else if (error.Error == PlayFabErrorCode.AccountBanned)
				{
					Application.Quit();
					NetworkSystem.Instance.ReturnToSinglePlayer();
					UnityEngine.Object.DestroyImmediate(PhotonNetworkController.Instance);
					UnityEngine.Object.DestroyImmediate(GTPlayer.Instance);
					GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
					for (int i = 0; i < array.Length; i++)
					{
						UnityEngine.Object.Destroy(array[i]);
					}
				}
				if (!this.tryTwice)
				{
					this.tryTwice = true;
					this.GetCosmeticsPlayFabCatalogData();
				}
			}, null, null);
		}

		// Token: 0x060044AF RID: 17583 RVA: 0x0017D550 File Offset: 0x0017B750
		public void SteamPurchase()
		{
			if (string.IsNullOrEmpty(this.itemToPurchase))
			{
				Debug.Log("Unable to start steam purchase process. itemToPurchase is not set.");
				return;
			}
			Debug.Log(string.Format("attempting to purchase item through steam. Is this a bundle purchase: {0}", this.buyingBundle));
			PlayFabClientAPI.StartPurchase(this.GetStartPurchaseRequest(), new Action<StartPurchaseResult>(this.ProcessStartPurchaseResponse), new Action<PlayFabError>(this.ProcessSteamPurchaseError), null, null);
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x0017D5B4 File Offset: 0x0017B7B4
		private StartPurchaseRequest GetStartPurchaseRequest()
		{
			return new StartPurchaseRequest
			{
				CatalogVersion = this.catalog,
				Items = new List<ItemPurchaseRequest>
				{
					new ItemPurchaseRequest
					{
						ItemId = this.itemToPurchase,
						Quantity = 1U,
						Annotation = "Purchased via in-game store"
					}
				}
			};
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x0017D608 File Offset: 0x0017B808
		private void ProcessStartPurchaseResponse(StartPurchaseResult result)
		{
			Debug.Log("successfully started purchase. attempted to pay for purchase through steam");
			this.currentPurchaseID = result.OrderId;
			PlayFabClientAPI.PayForPurchase(CosmeticsController.GetPayForPurchaseRequest(this.currentPurchaseID), new Action<PayForPurchaseResult>(CosmeticsController.ProcessPayForPurchaseResult), new Action<PlayFabError>(this.ProcessSteamPurchaseError), null, null);
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x0005CD8B File Offset: 0x0005AF8B
		private static PayForPurchaseRequest GetPayForPurchaseRequest(string orderId)
		{
			return new PayForPurchaseRequest
			{
				OrderId = orderId,
				ProviderName = "Steam",
				Currency = "RM"
			};
		}

		// Token: 0x060044B3 RID: 17587 RVA: 0x0005CDAF File Offset: 0x0005AFAF
		private static void ProcessPayForPurchaseResult(PayForPurchaseResult result)
		{
			Debug.Log("succeeded on sending request for paying with steam! waiting for response");
		}

		// Token: 0x060044B4 RID: 17588 RVA: 0x0017D658 File Offset: 0x0017B858
		private void ProcessSteamCallback(MicroTxnAuthorizationResponse_t callBackResponse)
		{
			Debug.Log("Steam has called back that the user has finished the payment interaction");
			if (callBackResponse.m_bAuthorized == 0)
			{
				Debug.Log("Steam has indicated that the payment was not authorised.");
			}
			if (this.buyingBundle)
			{
				PlayFabClientAPI.ConfirmPurchase(this.GetConfirmBundlePurchaseRequest(), delegate(ConfirmPurchaseResult _)
				{
					this.ProcessConfirmPurchaseSuccess();
				}, new Action<PlayFabError>(this.ProcessConfirmPurchaseError), null, null);
				return;
			}
			PlayFabClientAPI.ConfirmPurchase(this.GetConfirmATMPurchaseRequest(), delegate(ConfirmPurchaseResult _)
			{
				this.ProcessConfirmPurchaseSuccess();
			}, new Action<PlayFabError>(this.ProcessConfirmPurchaseError), null, null);
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x0005CDBB File Offset: 0x0005AFBB
		private ConfirmPurchaseRequest GetConfirmBundlePurchaseRequest()
		{
			return new ConfirmPurchaseRequest
			{
				OrderId = this.currentPurchaseID
			};
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x0017D6D4 File Offset: 0x0017B8D4
		private ConfirmPurchaseRequest GetConfirmATMPurchaseRequest()
		{
			return new ConfirmPurchaseRequest
			{
				OrderId = this.currentPurchaseID,
				CustomTags = new Dictionary<string, string>
				{
					{
						"NexusCreatorId",
						ATM_Manager.instance.ValidatedCreatorCode
					},
					{
						"PlayerName",
						GorillaComputer.instance.savedName
					}
				}
			};
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x0017D72C File Offset: 0x0017B92C
		private void ProcessConfirmPurchaseSuccess()
		{
			if (this.buyingBundle)
			{
				this.buyingBundle = false;
				if (PhotonNetwork.InRoom)
				{
					object[] data = new object[0];
					NetworkSystemRaiseEvent.RaiseEvent(9, data, NetworkSystemRaiseEvent.newWeb, true);
				}
				base.StartCoroutine(this.CheckIfMyCosmeticsUpdated(this.BundlePlayfabItemName));
			}
			else
			{
				ATM_Manager.instance.SwitchToStage(ATM_Manager.ATMStages.Success);
			}
			this.GetCurrencyBalance();
			this.UpdateCurrencyBoard();
			this.GetCosmeticsPlayFabCatalogData();
			GorillaTagger.Instance.offlineVRRig.GetCosmeticsPlayFabCatalogData();
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x0005CDCE File Offset: 0x0005AFCE
		private void ProcessConfirmPurchaseError(PlayFabError error)
		{
			this.ProcessSteamPurchaseError(error);
			ATM_Manager.instance.SwitchToStage(ATM_Manager.ATMStages.Failure);
			this.UpdateCurrencyBoard();
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x0017D7A8 File Offset: 0x0017B9A8
		private void ProcessSteamPurchaseError(PlayFabError error)
		{
			PlayFabErrorCode error2 = error.Error;
			if (error2 <= PlayFabErrorCode.PurchaseInitializationFailure)
			{
				if (error2 <= PlayFabErrorCode.FailedByPaymentProvider)
				{
					if (error2 == PlayFabErrorCode.AccountBanned)
					{
						PhotonNetwork.Disconnect();
						UnityEngine.Object.DestroyImmediate(PhotonNetworkController.Instance);
						UnityEngine.Object.DestroyImmediate(GTPlayer.Instance);
						GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
						for (int i = 0; i < array.Length; i++)
						{
							UnityEngine.Object.Destroy(array[i]);
						}
						Application.Quit();
						goto IL_1A1;
					}
					if (error2 != PlayFabErrorCode.FailedByPaymentProvider)
					{
						goto IL_191;
					}
					Debug.Log(string.Format("Attempted to pay for order, but has been Failed by Steam with error: {0}", error));
					goto IL_1A1;
				}
				else
				{
					if (error2 == PlayFabErrorCode.InsufficientFunds)
					{
						Debug.Log(string.Format("Attempting to do purchase through steam, steam has returned insufficient funds: {0}", error));
						goto IL_1A1;
					}
					if (error2 == PlayFabErrorCode.InvalidPaymentProvider)
					{
						Debug.Log(string.Format("Attempted to connect to steam as payment provider, but received error: {0}", error));
						goto IL_1A1;
					}
					if (error2 != PlayFabErrorCode.PurchaseInitializationFailure)
					{
						goto IL_191;
					}
				}
			}
			else if (error2 <= PlayFabErrorCode.InvalidPurchaseTransactionStatus)
			{
				if (error2 == PlayFabErrorCode.NotAuthenticated)
				{
					PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
					goto IL_1A1;
				}
				if (error2 == PlayFabErrorCode.PurchaseDoesNotExist)
				{
					Debug.Log(string.Format("Attempting to confirm purchase for order {0} but received error: {1}", this.currentPurchaseID, error));
					goto IL_1A1;
				}
				if (error2 != PlayFabErrorCode.InvalidPurchaseTransactionStatus)
				{
					goto IL_191;
				}
			}
			else
			{
				if (error2 == PlayFabErrorCode.InternalServerError)
				{
					Debug.Log(string.Format("PlayFab threw an internal server error: {0}", error));
					goto IL_1A1;
				}
				if (error2 == PlayFabErrorCode.StoreNotFound)
				{
					Debug.Log(string.Format("Attempted to load {0} from {1} but received an error: {2}", this.itemToPurchase, this.catalog, error));
					goto IL_1A1;
				}
				if (error2 != PlayFabErrorCode.DuplicatePurchaseTransactionId)
				{
					goto IL_191;
				}
			}
			Debug.Log(string.Format("Attempted to pay for order {0}, however received an error: {1}", this.currentPurchaseID, error));
			goto IL_1A1;
			IL_191:
			Debug.Log(string.Format("Steam purchase flow returned error: {0}", error));
			IL_1A1:
			ATM_Manager.instance.SwitchToStage(ATM_Manager.ATMStages.Failure);
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x0017D964 File Offset: 0x0017BB64
		public void UpdateCurrencyBoard()
		{
			this.FormattedPurchaseText(this.finalLine);
			this.dailyText.text = (this.checkedDaily ? (this.gotMyDaily ? "SUCCESSFULLY GOT DAILY ROCKS!" : "WAITING TO GET DAILY ROCKS...") : "CHECKING DAILY ROCKS...");
			this.currencyBoardText.text = string.Concat(new string[]
			{
				this.currencyBalance.ToString(),
				"\n\n",
				(this.secondsUntilTomorrow / 3600).ToString(),
				" HR, ",
				(this.secondsUntilTomorrow % 3600 / 60).ToString(),
				"MIN"
			});
		}

		// Token: 0x060044BB RID: 17595 RVA: 0x0005CDEA File Offset: 0x0005AFEA
		public void GetCurrencyBalance()
		{
			PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), delegate(GetUserInventoryResult result)
			{
				this.currencyBalance = result.VirtualCurrency[this.currencyName];
				this.UpdateCurrencyBoard();
				Action onGetCurrency = this.OnGetCurrency;
				if (onGetCurrency == null)
				{
					return;
				}
				onGetCurrency();
			}, delegate(PlayFabError error)
			{
				if (error.Error == PlayFabErrorCode.NotAuthenticated)
				{
					PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
					return;
				}
				if (error.Error == PlayFabErrorCode.AccountBanned)
				{
					Application.Quit();
					NetworkSystem.Instance.ReturnToSinglePlayer();
					UnityEngine.Object.DestroyImmediate(PhotonNetworkController.Instance);
					UnityEngine.Object.DestroyImmediate(GTPlayer.Instance);
					GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
					for (int i = 0; i < array.Length; i++)
					{
						UnityEngine.Object.Destroy(array[i]);
					}
				}
			}, null, null);
		}

		// Token: 0x060044BC RID: 17596 RVA: 0x0005CE23 File Offset: 0x0005B023
		public string GetItemDisplayName(CosmeticsController.CosmeticItem item)
		{
			if (item.overrideDisplayName != null && item.overrideDisplayName != "")
			{
				return item.overrideDisplayName;
			}
			return item.displayName;
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x0017DA18 File Offset: 0x0017BC18
		public void UpdateMyCosmetics()
		{
			if (NetworkSystem.Instance.InRoom)
			{
				if (GorillaServer.Instance != null && GorillaServer.Instance.NewCosmeticsPathShouldSetSharedGroupData())
				{
					this.UpdateMyCosmeticsForRoom(true);
				}
				if (GorillaServer.Instance != null && GorillaServer.Instance.NewCosmeticsPathShouldSetRoomData())
				{
					this.UpdateMyCosmeticsForRoom(false);
					return;
				}
			}
			else if (GorillaServer.Instance != null && GorillaServer.Instance.NewCosmeticsPathShouldSetSharedGroupData())
			{
				this.UpdateMyCosmeticsNotInRoom();
			}
		}

		// Token: 0x060044BE RID: 17598 RVA: 0x0005CE4C File Offset: 0x0005B04C
		private void UpdateMyCosmeticsNotInRoom()
		{
			if (GorillaServer.Instance != null)
			{
				GorillaServer.Instance.UpdateUserCosmetics();
			}
		}

		// Token: 0x060044BF RID: 17599 RVA: 0x0017DAA0 File Offset: 0x0017BCA0
		private void UpdateMyCosmeticsForRoom(bool shouldSetSharedGroupData)
		{
			byte eventCode = 9;
			if (shouldSetSharedGroupData)
			{
				eventCode = 10;
			}
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
			WebFlags flags = new WebFlags(1);
			raiseEventOptions.Flags = flags;
			object[] eventContent = new object[0];
			PhotonNetwork.RaiseEvent(eventCode, eventContent, raiseEventOptions, SendOptions.SendReliable);
		}

		// Token: 0x060044C0 RID: 17600 RVA: 0x0017DAE0 File Offset: 0x0017BCE0
		private void AlreadyOwnAllBundleButtons()
		{
			EarlyAccessButton[] array = this.earlyAccessButtons;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].AlreadyOwn();
			}
		}

		// Token: 0x060044C1 RID: 17601 RVA: 0x0005CE69 File Offset: 0x0005B069
		private bool UseNewCosmeticsPath()
		{
			return GorillaServer.Instance != null && GorillaServer.Instance.NewCosmeticsPathShouldReadSharedGroupData();
		}

		// Token: 0x060044C2 RID: 17602 RVA: 0x0005CE88 File Offset: 0x0005B088
		public void CheckCosmeticsSharedGroup()
		{
			this.updateCosmeticsRetries++;
			if (this.updateCosmeticsRetries < this.maxUpdateCosmeticsRetries)
			{
				base.StartCoroutine(this.WaitForNextCosmeticsAttempt());
			}
		}

		// Token: 0x060044C3 RID: 17603 RVA: 0x0005CEB3 File Offset: 0x0005B0B3
		private IEnumerator WaitForNextCosmeticsAttempt()
		{
			int num = (int)Mathf.Pow(3f, (float)(this.updateCosmeticsRetries + 1));
			yield return new WaitForSeconds((float)num);
			this.ConfirmIndividualCosmeticsSharedGroup(this.latestInventory);
			yield break;
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x0017DB0C File Offset: 0x0017BD0C
		private void ConfirmIndividualCosmeticsSharedGroup(GetUserInventoryResult inventory)
		{
			Debug.Log("confirming individual cosmetics with shared group");
			this.latestInventory = inventory;
			if (PhotonNetwork.LocalPlayer.UserId == null)
			{
				base.StartCoroutine(this.WaitForNextCosmeticsAttempt());
				return;
			}
			PlayFabClientAPI.GetSharedGroupData(new PlayFab.ClientModels.GetSharedGroupDataRequest
			{
				Keys = this.inventoryStringList,
				SharedGroupId = PhotonNetwork.LocalPlayer.UserId + "Inventory"
			}, delegate(GetSharedGroupDataResult result)
			{
				bool flag = true;
				foreach (KeyValuePair<string, PlayFab.ClientModels.SharedGroupDataRecord> keyValuePair in result.Data)
				{
					if (keyValuePair.Key != "Inventory")
					{
						break;
					}
					foreach (ItemInstance itemInstance in inventory.Inventory)
					{
						if (itemInstance.CatalogVersion == CosmeticsController.instance.catalog && !keyValuePair.Value.Value.Contains(itemInstance.ItemId))
						{
							flag = false;
							break;
						}
					}
				}
				if (!flag || result.Data.Count == 0)
				{
					this.UpdateMyCosmetics();
					return;
				}
				this.updateCosmeticsRetries = 0;
			}, delegate(PlayFabError error)
			{
				this.ReauthOrBan(error);
				this.CheckCosmeticsSharedGroup();
			}, null, null);
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x0017DBA8 File Offset: 0x0017BDA8
		public void ReauthOrBan(PlayFabError error)
		{
			if (error.Error == PlayFabErrorCode.NotAuthenticated)
			{
				PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
				return;
			}
			if (error.Error == PlayFabErrorCode.AccountBanned)
			{
				Application.Quit();
				PhotonNetwork.Disconnect();
				UnityEngine.Object.DestroyImmediate(PhotonNetworkController.Instance);
				UnityEngine.Object.DestroyImmediate(GTPlayer.Instance);
				GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
				for (int i = 0; i < array.Length; i++)
				{
					UnityEngine.Object.Destroy(array[i]);
				}
			}
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x0017DC18 File Offset: 0x0017BE18
		public void ProcessExternalUnlock(string itemID, bool autoEquip, bool isLeftHand)
		{
			Debug.Log("[ProcessExternalUnlock] Processing external unlock...");
			this.UnlockItem(itemID);
			VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
			offlineVRRig.concatStringOfCosmeticsAllowed += itemID;
			this.UpdateMyCosmetics();
			if (autoEquip)
			{
				Debug.Log("[ProcessExternalUnlock] Auto-equipping item...");
				CosmeticsController.CosmeticItem itemFromDict = this.GetItemFromDict(itemID);
				GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.external_item_claim, itemFromDict);
				List<CosmeticsController.CosmeticSlots> list = CollectionPool<List<CosmeticsController.CosmeticSlots>, CosmeticsController.CosmeticSlots>.Get();
				if (list.Capacity < 16)
				{
					list.Capacity = 16;
				}
				this.ApplyCosmeticItemToSet(this.currentWornSet, itemFromDict, isLeftHand, true, list);
				foreach (CosmeticsController.CosmeticSlots cosmeticSlots in list)
				{
					this.tryOnSet.items[(int)cosmeticSlots] = this.nullItem;
				}
				CollectionPool<List<CosmeticsController.CosmeticSlots>, CosmeticsController.CosmeticSlots>.Release(list);
				this.UpdateShoppingCart();
				this.UpdateWornCosmetics(true);
				Action onCosmeticsUpdated = this.OnCosmeticsUpdated;
				if (onCosmeticsUpdated == null)
				{
					return;
				}
				onCosmeticsUpdated();
			}
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x0005CEC2 File Offset: 0x0005B0C2
		public bool BuildValidationCheck()
		{
			if (this.m_earlyAccessSupporterPackCosmeticSO == null)
			{
				Debug.LogError("m_earlyAccessSupporterPackCosmeticSO is empty, everything will break!");
				return false;
			}
			return true;
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x0005CEDF File Offset: 0x0005B0DF
		public void SetHideCosmeticsFromRemotePlayers(bool hideCosmetics)
		{
			if (hideCosmetics == this.isHidingCosmeticsFromRemotePlayers)
			{
				return;
			}
			this.isHidingCosmeticsFromRemotePlayers = hideCosmetics;
			GorillaTagger.Instance.offlineVRRig.reliableState.SetIsDirty();
			this.UpdateWornCosmetics(true);
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x0017DD1C File Offset: 0x0017BF1C
		public bool ValidatePackedItems(int[] packed)
		{
			if (packed.Length == 0)
			{
				return true;
			}
			int num = 0;
			int num2 = packed[0];
			for (int i = 0; i < 16; i++)
			{
				if ((num2 & 1 << i) != 0)
				{
					num++;
				}
			}
			return packed.Length == num + 1;
		}

		// Token: 0x060044CC RID: 17612 RVA: 0x00032105 File Offset: 0x00030305
		bool IGorillaSliceableSimple.get_isActiveAndEnabled()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x04004562 RID: 17762
		[FormerlySerializedAs("v2AllCosmeticsInfoAssetRef")]
		[FormerlySerializedAs("newSysAllCosmeticsAssetRef")]
		[SerializeField]
		public GTAssetRef<AllCosmeticsArraySO> v2_allCosmeticsInfoAssetRef;

		// Token: 0x04004564 RID: 17764
		private readonly Dictionary<string, CosmeticInfoV2> _allCosmeticsDictV2 = new Dictionary<string, CosmeticInfoV2>();

		// Token: 0x04004565 RID: 17765
		public Action V2_allCosmeticsInfoAssetRef_OnPostLoad;

		// Token: 0x04004569 RID: 17769
		public const int maximumTransferrableItems = 5;

		// Token: 0x0400456A RID: 17770
		[OnEnterPlay_SetNull]
		public static volatile CosmeticsController instance;

		// Token: 0x0400456C RID: 17772
		public Action V2_OnGetCosmeticsPlayFabCatalogData_PostSuccess;

		// Token: 0x0400456D RID: 17773
		public Action OnGetCurrency;

		// Token: 0x0400456E RID: 17774
		[FormerlySerializedAs("allCosmetics")]
		[SerializeField]
		private List<CosmeticsController.CosmeticItem> _allCosmetics;

		// Token: 0x04004570 RID: 17776
		public Dictionary<string, CosmeticsController.CosmeticItem> _allCosmeticsDict = new Dictionary<string, CosmeticsController.CosmeticItem>(2048);

		// Token: 0x04004572 RID: 17778
		public Dictionary<string, string> _allCosmeticsItemIDsfromDisplayNamesDict = new Dictionary<string, string>(2048);

		// Token: 0x04004573 RID: 17779
		public CosmeticsController.CosmeticItem nullItem;

		// Token: 0x04004574 RID: 17780
		public string catalog;

		// Token: 0x04004575 RID: 17781
		private string[] tempStringArray;

		// Token: 0x04004576 RID: 17782
		private CosmeticsController.CosmeticItem tempItem;

		// Token: 0x04004577 RID: 17783
		private VRRigAnchorOverrides anchorOverrides;

		// Token: 0x04004578 RID: 17784
		public List<CatalogItem> catalogItems;

		// Token: 0x04004579 RID: 17785
		public bool tryTwice;

		// Token: 0x0400457A RID: 17786
		[NonSerialized]
		public CosmeticsController.CosmeticSet tryOnSet = new CosmeticsController.CosmeticSet();

		// Token: 0x0400457B RID: 17787
		public FittingRoomButton[] fittingRoomButtons;

		// Token: 0x0400457C RID: 17788
		public CosmeticStand[] cosmeticStands;

		// Token: 0x0400457D RID: 17789
		public List<CosmeticsController.CosmeticItem> currentCart = new List<CosmeticsController.CosmeticItem>();

		// Token: 0x0400457E RID: 17790
		public CosmeticsController.PurchaseItemStages currentPurchaseItemStage;

		// Token: 0x0400457F RID: 17791
		public CheckoutCartButton[] checkoutCartButtons;

		// Token: 0x04004580 RID: 17792
		public PurchaseItemButton leftPurchaseButton;

		// Token: 0x04004581 RID: 17793
		public PurchaseItemButton rightPurchaseButton;

		// Token: 0x04004582 RID: 17794
		public Text purchaseText;

		// Token: 0x04004583 RID: 17795
		public CosmeticsController.CosmeticItem itemToBuy;

		// Token: 0x04004584 RID: 17796
		public HeadModel checkoutHeadModel;

		// Token: 0x04004585 RID: 17797
		private List<string> playerIDList = new List<string>();

		// Token: 0x04004586 RID: 17798
		private List<string> inventoryStringList = new List<string>();

		// Token: 0x04004587 RID: 17799
		private bool foundCosmetic;

		// Token: 0x04004588 RID: 17800
		private int attempts;

		// Token: 0x04004589 RID: 17801
		private string finalLine;

		// Token: 0x0400458A RID: 17802
		private bool isLastHandTouchedLeft;

		// Token: 0x0400458B RID: 17803
		private CosmeticsController.CosmeticSet cachedSet = new CosmeticsController.CosmeticSet();

		// Token: 0x0400458D RID: 17805
		public readonly List<WardrobeInstance> wardrobes = new List<WardrobeInstance>();

		// Token: 0x0400458E RID: 17806
		public List<CosmeticsController.CosmeticItem> unlockedCosmetics = new List<CosmeticsController.CosmeticItem>(2048);

		// Token: 0x0400458F RID: 17807
		public List<CosmeticsController.CosmeticItem> unlockedHats = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x04004590 RID: 17808
		public List<CosmeticsController.CosmeticItem> unlockedFaces = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x04004591 RID: 17809
		public List<CosmeticsController.CosmeticItem> unlockedBadges = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x04004592 RID: 17810
		public List<CosmeticsController.CosmeticItem> unlockedPaws = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x04004593 RID: 17811
		public List<CosmeticsController.CosmeticItem> unlockedChests = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x04004594 RID: 17812
		public List<CosmeticsController.CosmeticItem> unlockedFurs = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x04004595 RID: 17813
		public List<CosmeticsController.CosmeticItem> unlockedShirts = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x04004596 RID: 17814
		public List<CosmeticsController.CosmeticItem> unlockedPants = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x04004597 RID: 17815
		public List<CosmeticsController.CosmeticItem> unlockedBacks = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x04004598 RID: 17816
		public List<CosmeticsController.CosmeticItem> unlockedArms = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x04004599 RID: 17817
		public List<CosmeticsController.CosmeticItem> unlockedTagFX = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x0400459A RID: 17818
		public int[] cosmeticsPages = new int[11];

		// Token: 0x0400459B RID: 17819
		private List<CosmeticsController.CosmeticItem>[] itemLists = new List<CosmeticsController.CosmeticItem>[11];

		// Token: 0x0400459C RID: 17820
		private int wardrobeType;

		// Token: 0x0400459D RID: 17821
		[NonSerialized]
		public CosmeticsController.CosmeticSet currentWornSet = new CosmeticsController.CosmeticSet();

		// Token: 0x0400459E RID: 17822
		public string concatStringCosmeticsAllowed = "";

		// Token: 0x0400459F RID: 17823
		public Action OnCosmeticsUpdated;

		// Token: 0x040045A0 RID: 17824
		public Text infoText;

		// Token: 0x040045A1 RID: 17825
		public Text earlyAccessText;

		// Token: 0x040045A2 RID: 17826
		public Text[] purchaseButtonText;

		// Token: 0x040045A3 RID: 17827
		public Text dailyText;

		// Token: 0x040045A4 RID: 17828
		public int currencyBalance;

		// Token: 0x040045A5 RID: 17829
		public string currencyName;

		// Token: 0x040045A6 RID: 17830
		public PurchaseCurrencyButton[] purchaseCurrencyButtons;

		// Token: 0x040045A7 RID: 17831
		public Text currencyBoardText;

		// Token: 0x040045A8 RID: 17832
		public Text currencyBoxText;

		// Token: 0x040045A9 RID: 17833
		public string startingCurrencyBoxTextString;

		// Token: 0x040045AA RID: 17834
		public string successfulCurrencyPurchaseTextString;

		// Token: 0x040045AB RID: 17835
		public string itemToPurchase;

		// Token: 0x040045AC RID: 17836
		public bool buyingBundle;

		// Token: 0x040045AD RID: 17837
		public bool confirmedDidntPlayInBeta;

		// Token: 0x040045AE RID: 17838
		public bool playedInBeta;

		// Token: 0x040045AF RID: 17839
		public bool gotMyDaily;

		// Token: 0x040045B0 RID: 17840
		public bool checkedDaily;

		// Token: 0x040045B1 RID: 17841
		public string currentPurchaseID;

		// Token: 0x040045B2 RID: 17842
		public bool hasPrice;

		// Token: 0x040045B3 RID: 17843
		private int searchIndex;

		// Token: 0x040045B4 RID: 17844
		private int iterator;

		// Token: 0x040045B5 RID: 17845
		private CosmeticsController.CosmeticItem cosmeticItemVar;

		// Token: 0x040045B6 RID: 17846
		[SerializeField]
		private CosmeticSO m_earlyAccessSupporterPackCosmeticSO;

		// Token: 0x040045B7 RID: 17847
		public EarlyAccessButton[] earlyAccessButtons;

		// Token: 0x040045B8 RID: 17848
		private BundleList bundleList = new BundleList();

		// Token: 0x040045B9 RID: 17849
		public string BundleSkuName = "2024_i_lava_you_pack";

		// Token: 0x040045BA RID: 17850
		public string BundlePlayfabItemName = "LSABG.";

		// Token: 0x040045BB RID: 17851
		public int BundleShinyRocks = 10000;

		// Token: 0x040045BC RID: 17852
		public DateTime currentTime;

		// Token: 0x040045BD RID: 17853
		public string lastDailyLogin;

		// Token: 0x040045BE RID: 17854
		public UserDataRecord userDataRecord;

		// Token: 0x040045BF RID: 17855
		public int secondsUntilTomorrow;

		// Token: 0x040045C0 RID: 17856
		public float secondsToWaitToCheckDaily = 10f;

		// Token: 0x040045C1 RID: 17857
		private int updateCosmeticsRetries;

		// Token: 0x040045C2 RID: 17858
		private int maxUpdateCosmeticsRetries;

		// Token: 0x040045C3 RID: 17859
		private GetUserInventoryResult latestInventory;

		// Token: 0x040045C4 RID: 17860
		private string returnString;

		// Token: 0x040045C5 RID: 17861
		private Callback<MicroTxnAuthorizationResponse_t> _steamMicroTransactionAuthorizationResponse;

		// Token: 0x040045C6 RID: 17862
		private static readonly List<CosmeticsController.CosmeticSlots> _g_default_outAppliedSlotsList_for_applyCosmeticItemToSet = new List<CosmeticsController.CosmeticSlots>(16);

		// Token: 0x02000AB4 RID: 2740
		public enum PurchaseItemStages
		{
			// Token: 0x040045C8 RID: 17864
			Start,
			// Token: 0x040045C9 RID: 17865
			CheckoutButtonPressed,
			// Token: 0x040045CA RID: 17866
			ItemSelected,
			// Token: 0x040045CB RID: 17867
			ItemOwned,
			// Token: 0x040045CC RID: 17868
			FinalPurchaseAcknowledgement,
			// Token: 0x040045CD RID: 17869
			Buying,
			// Token: 0x040045CE RID: 17870
			Success,
			// Token: 0x040045CF RID: 17871
			Failure
		}

		// Token: 0x02000AB5 RID: 2741
		public enum CosmeticCategory
		{
			// Token: 0x040045D1 RID: 17873
			None,
			// Token: 0x040045D2 RID: 17874
			Hat,
			// Token: 0x040045D3 RID: 17875
			Badge,
			// Token: 0x040045D4 RID: 17876
			Face,
			// Token: 0x040045D5 RID: 17877
			Paw,
			// Token: 0x040045D6 RID: 17878
			Chest,
			// Token: 0x040045D7 RID: 17879
			Fur,
			// Token: 0x040045D8 RID: 17880
			Shirt,
			// Token: 0x040045D9 RID: 17881
			Back,
			// Token: 0x040045DA RID: 17882
			Arms,
			// Token: 0x040045DB RID: 17883
			Pants,
			// Token: 0x040045DC RID: 17884
			TagEffect,
			// Token: 0x040045DD RID: 17885
			Count,
			// Token: 0x040045DE RID: 17886
			Set
		}

		// Token: 0x02000AB6 RID: 2742
		public enum CosmeticSlots
		{
			// Token: 0x040045E0 RID: 17888
			Hat,
			// Token: 0x040045E1 RID: 17889
			Badge,
			// Token: 0x040045E2 RID: 17890
			Face,
			// Token: 0x040045E3 RID: 17891
			ArmLeft,
			// Token: 0x040045E4 RID: 17892
			ArmRight,
			// Token: 0x040045E5 RID: 17893
			BackLeft,
			// Token: 0x040045E6 RID: 17894
			BackRight,
			// Token: 0x040045E7 RID: 17895
			HandLeft,
			// Token: 0x040045E8 RID: 17896
			HandRight,
			// Token: 0x040045E9 RID: 17897
			Chest,
			// Token: 0x040045EA RID: 17898
			Fur,
			// Token: 0x040045EB RID: 17899
			Shirt,
			// Token: 0x040045EC RID: 17900
			Pants,
			// Token: 0x040045ED RID: 17901
			Back,
			// Token: 0x040045EE RID: 17902
			Arms,
			// Token: 0x040045EF RID: 17903
			TagEffect,
			// Token: 0x040045F0 RID: 17904
			Count
		}

		// Token: 0x02000AB7 RID: 2743
		[Serializable]
		public class CosmeticSet
		{
			// Token: 0x14000083 RID: 131
			// (add) Token: 0x060044DA RID: 17626 RVA: 0x0017E1F0 File Offset: 0x0017C3F0
			// (remove) Token: 0x060044DB RID: 17627 RVA: 0x0017E228 File Offset: 0x0017C428
			public event CosmeticsController.CosmeticSet.OnSetActivatedHandler onSetActivatedEvent;

			// Token: 0x060044DC RID: 17628 RVA: 0x0005CF97 File Offset: 0x0005B197
			protected void OnSetActivated(CosmeticsController.CosmeticSet prevSet, CosmeticsController.CosmeticSet currentSet, NetPlayer netPlayer)
			{
				if (this.onSetActivatedEvent != null)
				{
					this.onSetActivatedEvent(prevSet, currentSet, netPlayer);
				}
			}

			// Token: 0x17000716 RID: 1814
			// (get) Token: 0x060044DD RID: 17629 RVA: 0x0017E260 File Offset: 0x0017C460
			public static CosmeticsController.CosmeticSet EmptySet
			{
				get
				{
					if (CosmeticsController.CosmeticSet._emptySet == null)
					{
						string[] array = new string[16];
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = "NOTHING";
						}
						CosmeticsController.CosmeticSet._emptySet = new CosmeticsController.CosmeticSet(array, CosmeticsController.instance);
					}
					return CosmeticsController.CosmeticSet._emptySet;
				}
			}

			// Token: 0x060044DE RID: 17630 RVA: 0x0005CFAF File Offset: 0x0005B1AF
			public CosmeticSet()
			{
				this.items = new CosmeticsController.CosmeticItem[16];
			}

			// Token: 0x060044DF RID: 17631 RVA: 0x0017E2AC File Offset: 0x0017C4AC
			public CosmeticSet(string[] itemNames, CosmeticsController controller)
			{
				this.items = new CosmeticsController.CosmeticItem[16];
				for (int i = 0; i < itemNames.Length; i++)
				{
					string displayName = itemNames[i];
					string itemNameFromDisplayName = controller.GetItemNameFromDisplayName(displayName);
					this.items[i] = controller.GetItemFromDict(itemNameFromDisplayName);
				}
			}

			// Token: 0x060044E0 RID: 17632 RVA: 0x0017E308 File Offset: 0x0017C508
			public CosmeticSet(int[] itemNamesPacked, CosmeticsController controller)
			{
				this.items = new CosmeticsController.CosmeticItem[16];
				int num = (itemNamesPacked.Length != 0) ? itemNamesPacked[0] : 0;
				int num2 = 1;
				for (int i = 0; i < this.items.Length; i++)
				{
					if ((num & 1 << i) != 0)
					{
						int num3 = itemNamesPacked[num2];
						CosmeticsController.CosmeticSet.nameScratchSpace[0] = (char)(65 + num3 % 26);
						CosmeticsController.CosmeticSet.nameScratchSpace[1] = (char)(65 + num3 / 26 % 26);
						CosmeticsController.CosmeticSet.nameScratchSpace[2] = (char)(65 + num3 / 676 % 26);
						CosmeticsController.CosmeticSet.nameScratchSpace[3] = (char)(65 + num3 / 17576 % 26);
						CosmeticsController.CosmeticSet.nameScratchSpace[4] = (char)(65 + num3 / 456976 % 26);
						CosmeticsController.CosmeticSet.nameScratchSpace[5] = '.';
						this.items[i] = controller.GetItemFromDict(new string(CosmeticsController.CosmeticSet.nameScratchSpace));
						num2++;
					}
					else
					{
						this.items[i] = controller.GetItemFromDict("null");
					}
				}
			}

			// Token: 0x060044E1 RID: 17633 RVA: 0x0017E410 File Offset: 0x0017C610
			public void CopyItems(CosmeticsController.CosmeticSet other)
			{
				for (int i = 0; i < this.items.Length; i++)
				{
					this.items[i] = other.items[i];
				}
			}

			// Token: 0x060044E2 RID: 17634 RVA: 0x0017E448 File Offset: 0x0017C648
			public void MergeSets(CosmeticsController.CosmeticSet tryOn, CosmeticsController.CosmeticSet current)
			{
				for (int i = 0; i < 16; i++)
				{
					if (tryOn == null)
					{
						this.items[i] = current.items[i];
					}
					else
					{
						this.items[i] = (tryOn.items[i].isNullItem ? current.items[i] : tryOn.items[i]);
					}
				}
			}

			// Token: 0x060044E3 RID: 17635 RVA: 0x0017E4B8 File Offset: 0x0017C6B8
			public void ClearSet(CosmeticsController.CosmeticItem nullItem)
			{
				for (int i = 0; i < 16; i++)
				{
					this.items[i] = nullItem;
				}
			}

			// Token: 0x060044E4 RID: 17636 RVA: 0x0017E4E0 File Offset: 0x0017C6E0
			public bool IsActive(string name)
			{
				int num = 16;
				for (int i = 0; i < num; i++)
				{
					if (this.items[i].displayName == name)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060044E5 RID: 17637 RVA: 0x0017E518 File Offset: 0x0017C718
			public bool HasItemOfCategory(CosmeticsController.CosmeticCategory category)
			{
				int num = 16;
				for (int i = 0; i < num; i++)
				{
					if (!this.items[i].isNullItem && this.items[i].itemCategory == category)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060044E6 RID: 17638 RVA: 0x0017E560 File Offset: 0x0017C760
			public bool HasItem(string name)
			{
				int num = 16;
				for (int i = 0; i < num; i++)
				{
					if (!this.items[i].isNullItem && this.items[i].displayName == name)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060044E7 RID: 17639 RVA: 0x0005CFD1 File Offset: 0x0005B1D1
			public static bool IsSlotLeftHanded(CosmeticsController.CosmeticSlots slot)
			{
				return slot == CosmeticsController.CosmeticSlots.ArmLeft || slot == CosmeticsController.CosmeticSlots.BackLeft || slot == CosmeticsController.CosmeticSlots.HandLeft;
			}

			// Token: 0x060044E8 RID: 17640 RVA: 0x0005CFE1 File Offset: 0x0005B1E1
			public static bool IsSlotRightHanded(CosmeticsController.CosmeticSlots slot)
			{
				return slot == CosmeticsController.CosmeticSlots.ArmRight || slot == CosmeticsController.CosmeticSlots.BackRight || slot == CosmeticsController.CosmeticSlots.HandRight;
			}

			// Token: 0x060044E9 RID: 17641 RVA: 0x0005CFF1 File Offset: 0x0005B1F1
			public static bool IsHoldable(CosmeticsController.CosmeticItem item)
			{
				return item.isHoldable;
			}

			// Token: 0x060044EA RID: 17642 RVA: 0x0017E5AC File Offset: 0x0017C7AC
			public static CosmeticsController.CosmeticSlots OppositeSlot(CosmeticsController.CosmeticSlots slot)
			{
				switch (slot)
				{
				case CosmeticsController.CosmeticSlots.Hat:
					return CosmeticsController.CosmeticSlots.Hat;
				case CosmeticsController.CosmeticSlots.Badge:
					return CosmeticsController.CosmeticSlots.Badge;
				case CosmeticsController.CosmeticSlots.Face:
					return CosmeticsController.CosmeticSlots.Face;
				case CosmeticsController.CosmeticSlots.ArmLeft:
					return CosmeticsController.CosmeticSlots.ArmRight;
				case CosmeticsController.CosmeticSlots.ArmRight:
					return CosmeticsController.CosmeticSlots.ArmLeft;
				case CosmeticsController.CosmeticSlots.BackLeft:
					return CosmeticsController.CosmeticSlots.BackRight;
				case CosmeticsController.CosmeticSlots.BackRight:
					return CosmeticsController.CosmeticSlots.BackLeft;
				case CosmeticsController.CosmeticSlots.HandLeft:
					return CosmeticsController.CosmeticSlots.HandRight;
				case CosmeticsController.CosmeticSlots.HandRight:
					return CosmeticsController.CosmeticSlots.HandLeft;
				case CosmeticsController.CosmeticSlots.Chest:
					return CosmeticsController.CosmeticSlots.Chest;
				case CosmeticsController.CosmeticSlots.Fur:
					return CosmeticsController.CosmeticSlots.Fur;
				case CosmeticsController.CosmeticSlots.Shirt:
					return CosmeticsController.CosmeticSlots.Shirt;
				case CosmeticsController.CosmeticSlots.Pants:
					return CosmeticsController.CosmeticSlots.Pants;
				case CosmeticsController.CosmeticSlots.Back:
					return CosmeticsController.CosmeticSlots.Back;
				case CosmeticsController.CosmeticSlots.Arms:
					return CosmeticsController.CosmeticSlots.Arms;
				case CosmeticsController.CosmeticSlots.TagEffect:
					return CosmeticsController.CosmeticSlots.TagEffect;
				default:
					return CosmeticsController.CosmeticSlots.Count;
				}
			}

			// Token: 0x060044EB RID: 17643 RVA: 0x0005CFF9 File Offset: 0x0005B1F9
			public static string SlotPlayerPreferenceName(CosmeticsController.CosmeticSlots slot)
			{
				return "slot_" + slot.ToString();
			}

			// Token: 0x060044EC RID: 17644 RVA: 0x0017E62C File Offset: 0x0017C82C
			private void ActivateCosmetic(CosmeticsController.CosmeticSet prevSet, VRRig rig, int slotIndex, CosmeticItemRegistry cosmeticsObjectRegistry, BodyDockPositions bDock)
			{
				CosmeticsController.CosmeticItem cosmeticItem = prevSet.items[slotIndex];
				string itemNameFromDisplayName = CosmeticsController.instance.GetItemNameFromDisplayName(cosmeticItem.displayName);
				CosmeticsController.CosmeticItem cosmeticItem2 = this.items[slotIndex];
				string itemNameFromDisplayName2 = CosmeticsController.instance.GetItemNameFromDisplayName(cosmeticItem2.displayName);
				BodyDockPositions.DropPositions dropPositions = CosmeticsController.CosmeticSlotToDropPosition((CosmeticsController.CosmeticSlots)slotIndex);
				if (cosmeticItem2.itemCategory != CosmeticsController.CosmeticCategory.None && !CosmeticsController.CompareCategoryToSavedCosmeticSlots(cosmeticItem2.itemCategory, (CosmeticsController.CosmeticSlots)slotIndex))
				{
					return;
				}
				if (cosmeticItem2.isHoldable && dropPositions == BodyDockPositions.DropPositions.None)
				{
					return;
				}
				if (!(itemNameFromDisplayName == itemNameFromDisplayName2))
				{
					if (!cosmeticItem.isNullItem)
					{
						if (cosmeticItem.isHoldable)
						{
							bDock.TransferrableItemDisableAtPosition(dropPositions);
						}
						CosmeticItemInstance cosmeticItemInstance = cosmeticsObjectRegistry.Cosmetic(cosmeticItem.displayName);
						if (cosmeticItemInstance != null)
						{
							cosmeticItemInstance.DisableItem((CosmeticsController.CosmeticSlots)slotIndex);
						}
					}
					if (!cosmeticItem2.isNullItem)
					{
						if (cosmeticItem2.isHoldable)
						{
							bDock.TransferrableItemEnableAtPosition(cosmeticItem2.displayName, dropPositions);
						}
						CosmeticItemInstance cosmeticItemInstance2 = cosmeticsObjectRegistry.Cosmetic(cosmeticItem2.displayName);
						if (rig.IsItemAllowed(itemNameFromDisplayName2) && cosmeticItemInstance2 != null)
						{
							cosmeticItemInstance2.EnableItem((CosmeticsController.CosmeticSlots)slotIndex);
						}
					}
					return;
				}
				if (cosmeticItem2.isNullItem)
				{
					return;
				}
				CosmeticItemInstance cosmeticItemInstance3 = cosmeticsObjectRegistry.Cosmetic(cosmeticItem2.displayName);
				if (cosmeticItemInstance3 != null)
				{
					if (!rig.IsItemAllowed(itemNameFromDisplayName2))
					{
						cosmeticItemInstance3.DisableItem((CosmeticsController.CosmeticSlots)slotIndex);
						return;
					}
					cosmeticItemInstance3.EnableItem((CosmeticsController.CosmeticSlots)slotIndex);
				}
			}

			// Token: 0x060044ED RID: 17645 RVA: 0x0017E768 File Offset: 0x0017C968
			public void ActivateCosmetics(CosmeticsController.CosmeticSet prevSet, VRRig rig, BodyDockPositions bDock, CosmeticItemRegistry cosmeticsObjectRegistry)
			{
				int num = 16;
				for (int i = 0; i < num; i++)
				{
					this.ActivateCosmetic(prevSet, rig, i, cosmeticsObjectRegistry, bDock);
				}
				this.OnSetActivated(prevSet, this, rig.creator);
			}

			// Token: 0x060044EE RID: 17646 RVA: 0x0017E7A0 File Offset: 0x0017C9A0
			public void DeactivateAllCosmetcs(BodyDockPositions bDock, CosmeticsController.CosmeticItem nullItem, CosmeticItemRegistry cosmeticObjectRegistry)
			{
				bDock.DisableAllTransferableItems();
				int num = 16;
				for (int i = 0; i < num; i++)
				{
					CosmeticsController.CosmeticItem cosmeticItem = this.items[i];
					if (!cosmeticItem.isNullItem)
					{
						CosmeticsController.CosmeticSlots cosmeticSlot = (CosmeticsController.CosmeticSlots)i;
						CosmeticItemInstance cosmeticItemInstance = cosmeticObjectRegistry.Cosmetic(cosmeticItem.displayName);
						if (cosmeticItemInstance != null)
						{
							cosmeticItemInstance.DisableItem(cosmeticSlot);
						}
						this.items[i] = nullItem;
					}
				}
			}

			// Token: 0x060044EF RID: 17647 RVA: 0x0017E800 File Offset: 0x0017CA00
			public void LoadFromPlayerPreferences(CosmeticsController controller)
			{
				int num = 16;
				for (int i = 0; i < num; i++)
				{
					CosmeticsController.CosmeticSlots slot = (CosmeticsController.CosmeticSlots)i;
					string @string = PlayerPrefs.GetString(CosmeticsController.CosmeticSet.SlotPlayerPreferenceName(slot), "NOTHING");
					if (@string == "null" || @string == "NOTHING")
					{
						this.items[i] = controller.nullItem;
					}
					else
					{
						CosmeticsController.CosmeticItem item = controller.GetItemFromDict(@string);
						if (item.isNullItem)
						{
							Debug.Log("LoadFromPlayerPreferences: Could not find item stored in player prefs: \"" + @string + "\"");
							this.items[i] = controller.nullItem;
						}
						else if (!CosmeticsController.CompareCategoryToSavedCosmeticSlots(item.itemCategory, slot))
						{
							this.items[i] = controller.nullItem;
						}
						else if (controller.unlockedCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => item.itemName == x.itemName) >= 0)
						{
							this.items[i] = item;
						}
						else
						{
							this.items[i] = controller.nullItem;
						}
					}
				}
			}

			// Token: 0x060044F0 RID: 17648 RVA: 0x0017E91C File Offset: 0x0017CB1C
			public string[] ToDisplayNameArray()
			{
				int num = 16;
				for (int i = 0; i < num; i++)
				{
					this.returnArray[i] = (string.IsNullOrEmpty(this.items[i].displayName) ? "null" : this.items[i].displayName);
				}
				return this.returnArray;
			}

			// Token: 0x060044F1 RID: 17649 RVA: 0x0017E978 File Offset: 0x0017CB78
			public int[] ToPackedIDArray()
			{
				int num = 0;
				int num2 = 0;
				int num3 = 16;
				for (int i = 0; i < num3; i++)
				{
					if (!this.items[i].isNullItem && this.items[i].itemName.Length == 6)
					{
						num |= 1 << i;
						num2++;
					}
				}
				if (num == 0)
				{
					return CosmeticsController.CosmeticSet.intArrays[0];
				}
				int[] array = CosmeticsController.CosmeticSet.intArrays[num2 + 1];
				array[0] = num;
				int num4 = 1;
				for (int j = 0; j < num3; j++)
				{
					if ((num & 1 << j) != 0)
					{
						string itemName = this.items[j].itemName;
						array[num4] = (int)(itemName[0] - 'A' + '\u001a' * (itemName[1] - 'A' + '\u001a' * (itemName[2] - 'A' + '\u001a' * (itemName[3] - 'A' + '\u001a' * (itemName[4] - 'A')))));
						num4++;
					}
				}
				return array;
			}

			// Token: 0x060044F2 RID: 17650 RVA: 0x0017EA78 File Offset: 0x0017CC78
			public string[] HoldableDisplayNames(bool leftHoldables)
			{
				int num = 16;
				int num2 = 0;
				for (int i = 0; i < num; i++)
				{
					if (this.items[i].isHoldable && this.items[i].isHoldable && this.items[i].itemCategory != CosmeticsController.CosmeticCategory.Chest)
					{
						if (leftHoldables && BodyDockPositions.IsPositionLeft(CosmeticsController.CosmeticSlotToDropPosition((CosmeticsController.CosmeticSlots)i)))
						{
							num2++;
						}
						else if (!leftHoldables && !BodyDockPositions.IsPositionLeft(CosmeticsController.CosmeticSlotToDropPosition((CosmeticsController.CosmeticSlots)i)))
						{
							num2++;
						}
					}
				}
				if (num2 == 0)
				{
					return null;
				}
				int num3 = 0;
				string[] array = new string[num2];
				for (int j = 0; j < num; j++)
				{
					if (this.items[j].isHoldable)
					{
						if (leftHoldables && BodyDockPositions.IsPositionLeft(CosmeticsController.CosmeticSlotToDropPosition((CosmeticsController.CosmeticSlots)j)))
						{
							array[num3] = this.items[j].displayName;
							num3++;
						}
						else if (!leftHoldables && !BodyDockPositions.IsPositionLeft(CosmeticsController.CosmeticSlotToDropPosition((CosmeticsController.CosmeticSlots)j)))
						{
							array[num3] = this.items[j].displayName;
							num3++;
						}
					}
				}
				return array;
			}

			// Token: 0x060044F3 RID: 17651 RVA: 0x0017EB8C File Offset: 0x0017CD8C
			public bool[] ToOnRightSideArray()
			{
				int num = 16;
				bool[] array = new bool[num];
				for (int i = 0; i < num; i++)
				{
					if (this.items[i].isHoldable && this.items[i].itemCategory != CosmeticsController.CosmeticCategory.Chest)
					{
						array[i] = !BodyDockPositions.IsPositionLeft(CosmeticsController.CosmeticSlotToDropPosition((CosmeticsController.CosmeticSlots)i));
					}
					else
					{
						array[i] = false;
					}
				}
				return array;
			}

			// Token: 0x040045F1 RID: 17905
			public CosmeticsController.CosmeticItem[] items;

			// Token: 0x040045F3 RID: 17907
			public string[] returnArray = new string[16];

			// Token: 0x040045F4 RID: 17908
			private static int[][] intArrays = new int[][]
			{
				new int[0],
				new int[1],
				new int[2],
				new int[3],
				new int[4],
				new int[5],
				new int[6],
				new int[7],
				new int[8],
				new int[9],
				new int[10],
				new int[11],
				new int[12],
				new int[13],
				new int[14],
				new int[15],
				new int[16],
				new int[17],
				new int[18],
				new int[19],
				new int[20],
				new int[21]
			};

			// Token: 0x040045F5 RID: 17909
			private static CosmeticsController.CosmeticSet _emptySet;

			// Token: 0x040045F6 RID: 17910
			private static char[] nameScratchSpace = new char[6];

			// Token: 0x02000AB8 RID: 2744
			// (Invoke) Token: 0x060044F6 RID: 17654
			public delegate void OnSetActivatedHandler(CosmeticsController.CosmeticSet prevSet, CosmeticsController.CosmeticSet currentSet, NetPlayer netPlayer);
		}

		// Token: 0x02000ABA RID: 2746
		[Serializable]
		public struct CosmeticItem
		{
			// Token: 0x040045F8 RID: 17912
			[Tooltip("Should match the spreadsheet item name.")]
			public string itemName;

			// Token: 0x040045F9 RID: 17913
			[Tooltip("Determines what wardrobe section the item will show up in.")]
			public CosmeticsController.CosmeticCategory itemCategory;

			// Token: 0x040045FA RID: 17914
			[Tooltip("If this is a holdable item.")]
			public bool isHoldable;

			// Token: 0x040045FB RID: 17915
			[Tooltip("Icon shown in the store menus & hunt watch.")]
			public Sprite itemPicture;

			// Token: 0x040045FC RID: 17916
			public string displayName;

			// Token: 0x040045FD RID: 17917
			public string itemPictureResourceString;

			// Token: 0x040045FE RID: 17918
			[Tooltip("The name shown on the store checkout screen.")]
			public string overrideDisplayName;

			// Token: 0x040045FF RID: 17919
			[DebugReadout]
			[NonSerialized]
			public int cost;

			// Token: 0x04004600 RID: 17920
			[DebugReadout]
			[NonSerialized]
			public string[] bundledItems;

			// Token: 0x04004601 RID: 17921
			[DebugReadout]
			[NonSerialized]
			public bool canTryOn;

			// Token: 0x04004602 RID: 17922
			[Tooltip("Set to true if the item takes up both left and right wearable hand slots at the same time. Used for things like mittens/gloves.")]
			public bool bothHandsHoldable;

			// Token: 0x04004603 RID: 17923
			public bool bLoadsFromResources;

			// Token: 0x04004604 RID: 17924
			public bool bUsesMeshAtlas;

			// Token: 0x04004605 RID: 17925
			public Vector3 rotationOffset;

			// Token: 0x04004606 RID: 17926
			public Vector3 positionOffset;

			// Token: 0x04004607 RID: 17927
			public string meshAtlasResourceString;

			// Token: 0x04004608 RID: 17928
			public string meshResourceString;

			// Token: 0x04004609 RID: 17929
			public string materialResourceString;

			// Token: 0x0400460A RID: 17930
			[HideInInspector]
			public bool isNullItem;
		}

		// Token: 0x02000ABB RID: 2747
		[Serializable]
		public class IAPRequestBody
		{
			// Token: 0x0400460B RID: 17931
			public string userID;

			// Token: 0x0400460C RID: 17932
			public string nonce;

			// Token: 0x0400460D RID: 17933
			public string platform;

			// Token: 0x0400460E RID: 17934
			public string sku;

			// Token: 0x0400460F RID: 17935
			public Dictionary<string, string> customTags;
		}

		// Token: 0x02000ABC RID: 2748
		public enum EWearingCosmeticSet
		{
			// Token: 0x04004611 RID: 17937
			NotASet,
			// Token: 0x04004612 RID: 17938
			NotWearing,
			// Token: 0x04004613 RID: 17939
			Partial,
			// Token: 0x04004614 RID: 17940
			Complete
		}
	}
}
