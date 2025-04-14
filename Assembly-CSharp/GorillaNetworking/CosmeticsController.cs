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
	// Token: 0x02000A86 RID: 2694
	public class CosmeticsController : MonoBehaviour, IGorillaSliceableSimple, IBuildValidation
	{
		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06004319 RID: 17177 RVA: 0x0013C4B5 File Offset: 0x0013A6B5
		// (set) Token: 0x0600431A RID: 17178 RVA: 0x0013C4BD File Offset: 0x0013A6BD
		public CosmeticInfoV2[] v2_allCosmetics { get; private set; }

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x0600431B RID: 17179 RVA: 0x0013C4C6 File Offset: 0x0013A6C6
		// (set) Token: 0x0600431C RID: 17180 RVA: 0x0013C4CE File Offset: 0x0013A6CE
		public bool v2_allCosmeticsInfoAssetRef_isLoaded { get; private set; }

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x0600431D RID: 17181 RVA: 0x0013C4D7 File Offset: 0x0013A6D7
		// (set) Token: 0x0600431E RID: 17182 RVA: 0x0013C4DF File Offset: 0x0013A6DF
		public bool v2_isGetCosmeticsPlayCatalogDataWaitingForCallback { get; private set; }

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x0600431F RID: 17183 RVA: 0x0013C4E8 File Offset: 0x0013A6E8
		// (set) Token: 0x06004320 RID: 17184 RVA: 0x0013C4F0 File Offset: 0x0013A6F0
		public bool v2_isCosmeticPlayFabCatalogDataLoaded { get; private set; }

		// Token: 0x06004321 RID: 17185 RVA: 0x0013C4F9 File Offset: 0x0013A6F9
		private void V2Awake()
		{
			this._allCosmetics = null;
			base.StartCoroutine(this.V2_allCosmeticsInfoAssetRefSO_LoadCoroutine());
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x0013C50F File Offset: 0x0013A70F
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

		// Token: 0x06004323 RID: 17187 RVA: 0x0013C520 File Offset: 0x0013A720
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

		// Token: 0x06004324 RID: 17188 RVA: 0x0013C651 File Offset: 0x0013A851
		public bool TryGetCosmeticInfoV2(string playFabId, out CosmeticInfoV2 cosmeticInfo)
		{
			return this._allCosmeticsDictV2.TryGetValue(playFabId, out cosmeticInfo);
		}

		// Token: 0x06004325 RID: 17189 RVA: 0x0013C660 File Offset: 0x0013A860
		private void V2_ConformCosmeticItemV1DisplayName(ref CosmeticsController.CosmeticItem cosmetic)
		{
			if (cosmetic.itemName == cosmetic.displayName)
			{
				return;
			}
			cosmetic.overrideDisplayName = cosmetic.displayName;
			cosmetic.displayName = cosmetic.itemName;
		}

		// Token: 0x06004326 RID: 17190 RVA: 0x0013C690 File Offset: 0x0013A890
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

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06004327 RID: 17191 RVA: 0x0013C6C5 File Offset: 0x0013A8C5
		// (set) Token: 0x06004328 RID: 17192 RVA: 0x0013C6CC File Offset: 0x0013A8CC
		public static bool hasInstance { get; private set; }

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06004329 RID: 17193 RVA: 0x0013C6D4 File Offset: 0x0013A8D4
		// (set) Token: 0x0600432A RID: 17194 RVA: 0x0013C6DC File Offset: 0x0013A8DC
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

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x0600432B RID: 17195 RVA: 0x0013C6E5 File Offset: 0x0013A8E5
		// (set) Token: 0x0600432C RID: 17196 RVA: 0x0013C6ED File Offset: 0x0013A8ED
		public bool allCosmeticsDict_isInitialized { get; private set; }

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x0600432D RID: 17197 RVA: 0x0013C6F6 File Offset: 0x0013A8F6
		public Dictionary<string, CosmeticsController.CosmeticItem> allCosmeticsDict
		{
			get
			{
				return this._allCosmeticsDict;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x0600432E RID: 17198 RVA: 0x0013C6FE File Offset: 0x0013A8FE
		// (set) Token: 0x0600432F RID: 17199 RVA: 0x0013C706 File Offset: 0x0013A906
		public bool allCosmeticsItemIDsfromDisplayNamesDict_isInitialized { get; private set; }

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06004330 RID: 17200 RVA: 0x0013C70F File Offset: 0x0013A90F
		public Dictionary<string, string> allCosmeticsItemIDsfromDisplayNamesDict
		{
			get
			{
				return this._allCosmeticsItemIDsfromDisplayNamesDict;
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06004331 RID: 17201 RVA: 0x0013C717 File Offset: 0x0013A917
		// (set) Token: 0x06004332 RID: 17202 RVA: 0x0013C71F File Offset: 0x0013A91F
		public bool isHidingCosmeticsFromRemotePlayers { get; private set; }

		// Token: 0x06004333 RID: 17203 RVA: 0x0013C728 File Offset: 0x0013A928
		public void AddWardrobeInstance(WardrobeInstance instance)
		{
			this.wardrobes.Add(instance);
			if (CosmeticsV2Spawner_Dirty.allPartsInstantiated)
			{
				this.UpdateWardrobeModelsAndButtons();
			}
		}

		// Token: 0x06004334 RID: 17204 RVA: 0x0013C743 File Offset: 0x0013A943
		public void RemoveWardrobeInstance(WardrobeInstance instance)
		{
			this.wardrobes.Remove(instance);
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06004335 RID: 17205 RVA: 0x0013C752 File Offset: 0x0013A952
		public int CurrencyBalance
		{
			get
			{
				return this.currencyBalance;
			}
		}

		// Token: 0x06004336 RID: 17206 RVA: 0x0013C75C File Offset: 0x0013A95C
		public void Awake()
		{
			if (CosmeticsController.instance == null)
			{
				CosmeticsController.instance = this;
				CosmeticsController.hasInstance = true;
			}
			else if (CosmeticsController.instance != this)
			{
				Object.Destroy(base.gameObject);
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

		// Token: 0x06004337 RID: 17207 RVA: 0x0013CA04 File Offset: 0x0013AC04
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

		// Token: 0x06004338 RID: 17208 RVA: 0x0013CA60 File Offset: 0x0013AC60
		public void OnEnable()
		{
			GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
			if (SteamManager.Initialized && this._steamMicroTransactionAuthorizationResponse == null)
			{
				this._steamMicroTransactionAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(new Callback<MicroTxnAuthorizationResponse_t>.DispatchDelegate(this.ProcessSteamCallback));
			}
		}

		// Token: 0x06004339 RID: 17209 RVA: 0x0013CA8F File Offset: 0x0013AC8F
		public void OnDisable()
		{
			Callback<MicroTxnAuthorizationResponse_t> steamMicroTransactionAuthorizationResponse = this._steamMicroTransactionAuthorizationResponse;
			if (steamMicroTransactionAuthorizationResponse != null)
			{
				steamMicroTransactionAuthorizationResponse.Unregister();
			}
			GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x0600433A RID: 17210 RVA: 0x000023F4 File Offset: 0x000005F4
		public void SliceUpdate()
		{
		}

		// Token: 0x0600433B RID: 17211 RVA: 0x0013CAAC File Offset: 0x0013ACAC
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

		// Token: 0x0600433C RID: 17212 RVA: 0x0013CB40 File Offset: 0x0013AD40
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

		// Token: 0x0600433D RID: 17213 RVA: 0x0013CBA2 File Offset: 0x0013ADA2
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

		// Token: 0x0600433E RID: 17214 RVA: 0x0013CBD4 File Offset: 0x0013ADD4
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

		// Token: 0x0600433F RID: 17215 RVA: 0x0013CC08 File Offset: 0x0013AE08
		private void SaveItemPreference(CosmeticsController.CosmeticSlots slot, int slotIdx, CosmeticsController.CosmeticItem newItem)
		{
			PlayerPrefs.SetString(CosmeticsController.CosmeticSet.SlotPlayerPreferenceName(slot), newItem.itemName);
			PlayerPrefs.Save();
		}

		// Token: 0x06004340 RID: 17216 RVA: 0x0013CC20 File Offset: 0x0013AE20
		public void SaveCurrentItemPreferences()
		{
			for (int i = 0; i < 16; i++)
			{
				CosmeticsController.CosmeticSlots slot = (CosmeticsController.CosmeticSlots)i;
				this.SaveItemPreference(slot, i, this.currentWornSet.items[i]);
			}
		}

		// Token: 0x06004341 RID: 17217 RVA: 0x0013CC58 File Offset: 0x0013AE58
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

		// Token: 0x06004342 RID: 17218 RVA: 0x0013CCB4 File Offset: 0x0013AEB4
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

		// Token: 0x06004343 RID: 17219 RVA: 0x0013CEB8 File Offset: 0x0013B0B8
		public void ApplyCosmeticItemToSet(CosmeticsController.CosmeticSet set, CosmeticsController.CosmeticItem newItem, bool isLeftHand, bool applyToPlayerPrefs)
		{
			this.ApplyCosmeticItemToSet(set, newItem, isLeftHand, applyToPlayerPrefs, CosmeticsController._g_default_outAppliedSlotsList_for_applyCosmeticItemToSet);
		}

		// Token: 0x06004344 RID: 17220 RVA: 0x0013CECC File Offset: 0x0013B0CC
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

		// Token: 0x06004345 RID: 17221 RVA: 0x0013CFB8 File Offset: 0x0013B1B8
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

		// Token: 0x06004346 RID: 17222 RVA: 0x0013D040 File Offset: 0x0013B240
		public void PressFittingRoomButton(FittingRoomButton pressedFittingRoomButton, bool isLeftHand)
		{
			BundleManager.instance._tryOnBundlesStand.ClearSelectedBundle();
			this.ApplyCosmeticItemToSet(this.tryOnSet, pressedFittingRoomButton.currentCosmeticItem, isLeftHand, false);
			this.UpdateShoppingCart();
			this.UpdateWornCosmetics(true);
		}

		// Token: 0x06004347 RID: 17223 RVA: 0x0013D074 File Offset: 0x0013B274
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

		// Token: 0x06004348 RID: 17224 RVA: 0x0013D0E8 File Offset: 0x0013B2E8
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

		// Token: 0x06004349 RID: 17225 RVA: 0x0013D254 File Offset: 0x0013B454
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

		// Token: 0x0600434A RID: 17226 RVA: 0x0013D314 File Offset: 0x0013B514
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

		// Token: 0x0600434B RID: 17227 RVA: 0x0013D6A0 File Offset: 0x0013B8A0
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

		// Token: 0x0600434C RID: 17228 RVA: 0x0013D6F8 File Offset: 0x0013B8F8
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

		// Token: 0x0600434D RID: 17229 RVA: 0x0013D77C File Offset: 0x0013B97C
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

		// Token: 0x0600434E RID: 17230 RVA: 0x0013D8B7 File Offset: 0x0013BAB7
		public void PressPurchaseItemButton(PurchaseItemButton pressedPurchaseItemButton, bool isLeftHand)
		{
			this.ProcessPurchaseItemState(pressedPurchaseItemButton.buttonSide, isLeftHand);
		}

		// Token: 0x0600434F RID: 17231 RVA: 0x0013D8C6 File Offset: 0x0013BAC6
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

		// Token: 0x06004350 RID: 17232 RVA: 0x0013D908 File Offset: 0x0013BB08
		public void PressEarlyAccessButton()
		{
			this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Start;
			this.ProcessPurchaseItemState("left", false);
			this.buyingBundle = true;
			this.itemToPurchase = this.BundlePlayfabItemName;
			ATM_Manager.instance.shinyRocksCost = (float)this.BundleShinyRocks;
			this.SteamPurchase();
		}

		// Token: 0x06004351 RID: 17233 RVA: 0x0013D954 File Offset: 0x0013BB54
		public void PressPurchaseBundleButton(string PlayFabItemName)
		{
			BundleManager.instance.BundlePurchaseButtonPressed(PlayFabItemName);
		}

		// Token: 0x06004352 RID: 17234 RVA: 0x0013D964 File Offset: 0x0013BB64
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

		// Token: 0x06004353 RID: 17235 RVA: 0x0013DE44 File Offset: 0x0013C044
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

		// Token: 0x06004354 RID: 17236 RVA: 0x0013DEC8 File Offset: 0x0013C0C8
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

		// Token: 0x06004355 RID: 17237 RVA: 0x0013DF34 File Offset: 0x0013C134
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

		// Token: 0x06004356 RID: 17238 RVA: 0x0013E276 File Offset: 0x0013C476
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

		// Token: 0x06004357 RID: 17239 RVA: 0x0013E28C File Offset: 0x0013C48C
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

		// Token: 0x06004358 RID: 17240 RVA: 0x0013E50C File Offset: 0x0013C70C
		public int GetCategorySize(CosmeticsController.CosmeticCategory category)
		{
			int indexForCategory = this.GetIndexForCategory(category);
			if (indexForCategory != -1)
			{
				return this.itemLists[indexForCategory].Count;
			}
			return 0;
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x0013E534 File Offset: 0x0013C734
		public CosmeticsController.CosmeticItem GetCosmetic(int category, int cosmeticIndex)
		{
			if (cosmeticIndex >= this.itemLists[category].Count || cosmeticIndex < 0)
			{
				return this.nullItem;
			}
			return this.itemLists[category][cosmeticIndex];
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x0013E55F File Offset: 0x0013C75F
		public CosmeticsController.CosmeticItem GetCosmetic(CosmeticsController.CosmeticCategory category, int cosmeticIndex)
		{
			return this.GetCosmetic(this.GetIndexForCategory(category), cosmeticIndex);
		}

		// Token: 0x0600435B RID: 17243 RVA: 0x0013E570 File Offset: 0x0013C770
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

		// Token: 0x0600435C RID: 17244 RVA: 0x0013E5CC File Offset: 0x0013C7CC
		public bool IsCosmeticEquipped(CosmeticsController.CosmeticItem cosmetic)
		{
			return this.AnyMatch(this.currentWornSet, cosmetic);
		}

		// Token: 0x0600435D RID: 17245 RVA: 0x0013E5DC File Offset: 0x0013C7DC
		public CosmeticsController.CosmeticItem GetSlotItem(CosmeticsController.CosmeticSlots slot, bool checkOpposite = true)
		{
			int num = (int)slot;
			if (checkOpposite)
			{
				num = (int)CosmeticsController.CosmeticSet.OppositeSlot(slot);
			}
			return this.currentWornSet.items[num];
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x0013E606 File Offset: 0x0013C806
		public string[] GetCurrentlyWornCosmetics()
		{
			return this.currentWornSet.ToDisplayNameArray();
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x0013E613 File Offset: 0x0013C813
		public bool[] GetCurrentRightEquippedSided()
		{
			return this.currentWornSet.ToOnRightSideArray();
		}

		// Token: 0x06004360 RID: 17248 RVA: 0x0013E620 File Offset: 0x0013C820
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

		// Token: 0x06004361 RID: 17249 RVA: 0x0013E804 File Offset: 0x0013CA04
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

		// Token: 0x06004362 RID: 17250 RVA: 0x0013E89F File Offset: 0x0013CA9F
		public CosmeticsController.CosmeticItem GetItemFromDict(string itemID)
		{
			if (!this.allCosmeticsDict.TryGetValue(itemID, out this.cosmeticItemVar))
			{
				return this.nullItem;
			}
			return this.cosmeticItemVar;
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x0013E8C2 File Offset: 0x0013CAC2
		public string GetItemNameFromDisplayName(string displayName)
		{
			if (!this.allCosmeticsItemIDsfromDisplayNamesDict.TryGetValue(displayName, out this.returnString))
			{
				return "null";
			}
			return this.returnString;
		}

		// Token: 0x06004364 RID: 17252 RVA: 0x0013E8E4 File Offset: 0x0013CAE4
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

		// Token: 0x06004365 RID: 17253 RVA: 0x0013E9B8 File Offset: 0x0013CBB8
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

		// Token: 0x06004366 RID: 17254 RVA: 0x0013EA1B File Offset: 0x0013CC1B
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
					Object.DestroyImmediate(PhotonNetworkController.Instance);
					Object.DestroyImmediate(GTPlayer.Instance);
					GameObject[] array = Object.FindObjectsOfType<GameObject>();
					for (int i = 0; i < array.Length; i++)
					{
						Object.Destroy(array[i]);
					}
				}
			}, null, null);
		}

		// Token: 0x06004367 RID: 17255 RVA: 0x0013EA41 File Offset: 0x0013CC41
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

		// Token: 0x06004368 RID: 17256 RVA: 0x0013EA50 File Offset: 0x0013CC50
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
					Object.DestroyImmediate(PhotonNetworkController.Instance);
					Object.DestroyImmediate(GTPlayer.Instance);
					GameObject[] array = Object.FindObjectsOfType<GameObject>();
					for (int i = 0; i < array.Length; i++)
					{
						Object.Destroy(array[i]);
					}
				}
			});
			yield break;
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x0013EA5F File Offset: 0x0013CC5F
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
						Object.DestroyImmediate(PhotonNetworkController.Instance);
						Object.DestroyImmediate(GTPlayer.Instance);
						GameObject[] array = Object.FindObjectsOfType<GameObject>();
						for (int i = 0; i < array.Length; i++)
						{
							Object.Destroy(array[i]);
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
					Object.DestroyImmediate(PhotonNetworkController.Instance);
					Object.DestroyImmediate(GTPlayer.Instance);
					GameObject[] array = Object.FindObjectsOfType<GameObject>();
					for (int i = 0; i < array.Length; i++)
					{
						Object.Destroy(array[i]);
					}
				}
				if (!this.tryTwice)
				{
					this.tryTwice = true;
					this.GetCosmeticsPlayFabCatalogData();
				}
			}, null, null);
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x0013EAA0 File Offset: 0x0013CCA0
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

		// Token: 0x0600436B RID: 17259 RVA: 0x0013EB04 File Offset: 0x0013CD04
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

		// Token: 0x0600436C RID: 17260 RVA: 0x0013EB58 File Offset: 0x0013CD58
		private void ProcessStartPurchaseResponse(StartPurchaseResult result)
		{
			Debug.Log("successfully started purchase. attempted to pay for purchase through steam");
			this.currentPurchaseID = result.OrderId;
			PlayFabClientAPI.PayForPurchase(CosmeticsController.GetPayForPurchaseRequest(this.currentPurchaseID), new Action<PayForPurchaseResult>(CosmeticsController.ProcessPayForPurchaseResult), new Action<PlayFabError>(this.ProcessSteamPurchaseError), null, null);
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x0013EBA5 File Offset: 0x0013CDA5
		private static PayForPurchaseRequest GetPayForPurchaseRequest(string orderId)
		{
			return new PayForPurchaseRequest
			{
				OrderId = orderId,
				ProviderName = "Steam",
				Currency = "RM"
			};
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x0013EBC9 File Offset: 0x0013CDC9
		private static void ProcessPayForPurchaseResult(PayForPurchaseResult result)
		{
			Debug.Log("succeeded on sending request for paying with steam! waiting for response");
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x0013EBD8 File Offset: 0x0013CDD8
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

		// Token: 0x06004370 RID: 17264 RVA: 0x0013EC54 File Offset: 0x0013CE54
		private ConfirmPurchaseRequest GetConfirmBundlePurchaseRequest()
		{
			return new ConfirmPurchaseRequest
			{
				OrderId = this.currentPurchaseID
			};
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x0013EC68 File Offset: 0x0013CE68
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

		// Token: 0x06004372 RID: 17266 RVA: 0x0013ECC0 File Offset: 0x0013CEC0
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

		// Token: 0x06004373 RID: 17267 RVA: 0x0013ED3B File Offset: 0x0013CF3B
		private void ProcessConfirmPurchaseError(PlayFabError error)
		{
			this.ProcessSteamPurchaseError(error);
			ATM_Manager.instance.SwitchToStage(ATM_Manager.ATMStages.Failure);
			this.UpdateCurrencyBoard();
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x0013ED58 File Offset: 0x0013CF58
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
						Object.DestroyImmediate(PhotonNetworkController.Instance);
						Object.DestroyImmediate(GTPlayer.Instance);
						GameObject[] array = Object.FindObjectsOfType<GameObject>();
						for (int i = 0; i < array.Length; i++)
						{
							Object.Destroy(array[i]);
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

		// Token: 0x06004375 RID: 17269 RVA: 0x0013EF14 File Offset: 0x0013D114
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

		// Token: 0x06004376 RID: 17270 RVA: 0x0013EFC8 File Offset: 0x0013D1C8
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
					Object.DestroyImmediate(PhotonNetworkController.Instance);
					Object.DestroyImmediate(GTPlayer.Instance);
					GameObject[] array = Object.FindObjectsOfType<GameObject>();
					for (int i = 0; i < array.Length; i++)
					{
						Object.Destroy(array[i]);
					}
				}
			}, null, null);
		}

		// Token: 0x06004377 RID: 17271 RVA: 0x0013F001 File Offset: 0x0013D201
		public string GetItemDisplayName(CosmeticsController.CosmeticItem item)
		{
			if (item.overrideDisplayName != null && item.overrideDisplayName != "")
			{
				return item.overrideDisplayName;
			}
			return item.displayName;
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x0013F02A File Offset: 0x0013D22A
		public void UpdateMyCosmetics()
		{
			if (NetworkSystem.Instance.InRoom)
			{
				this.UpdateMyCosmeticsForRoom();
				return;
			}
			if (this.UseNewCosmeticsPath())
			{
				this.UpdateMyCosmeticsNotInRoom();
			}
		}

		// Token: 0x06004379 RID: 17273 RVA: 0x0013F04D File Offset: 0x0013D24D
		private void UpdateMyCosmeticsNotInRoom()
		{
			if (GorillaServer.Instance != null)
			{
				GorillaServer.Instance.UpdateUserCosmetics();
			}
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x0013F06C File Offset: 0x0013D26C
		private void UpdateMyCosmeticsForRoom()
		{
			byte eventCode = 9;
			if (this.UseNewCosmeticsPath())
			{
				eventCode = 10;
			}
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
			WebFlags flags = new WebFlags(1);
			raiseEventOptions.Flags = flags;
			object[] eventContent = new object[0];
			PhotonNetwork.RaiseEvent(eventCode, eventContent, raiseEventOptions, SendOptions.SendReliable);
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x0013F0B0 File Offset: 0x0013D2B0
		private void AlreadyOwnAllBundleButtons()
		{
			EarlyAccessButton[] array = this.earlyAccessButtons;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].AlreadyOwn();
			}
		}

		// Token: 0x0600437C RID: 17276 RVA: 0x0013F0DA File Offset: 0x0013D2DA
		private bool UseNewCosmeticsPath()
		{
			return GorillaServer.Instance != null && GorillaServer.Instance.NewCosmeticsPath();
		}

		// Token: 0x0600437D RID: 17277 RVA: 0x0013F0F9 File Offset: 0x0013D2F9
		public void CheckCosmeticsSharedGroup()
		{
			this.updateCosmeticsRetries++;
			if (this.updateCosmeticsRetries < this.maxUpdateCosmeticsRetries)
			{
				base.StartCoroutine(this.WaitForNextCosmeticsAttempt());
			}
		}

		// Token: 0x0600437E RID: 17278 RVA: 0x0013F124 File Offset: 0x0013D324
		private IEnumerator WaitForNextCosmeticsAttempt()
		{
			int num = (int)Mathf.Pow(3f, (float)(this.updateCosmeticsRetries + 1));
			yield return new WaitForSeconds((float)num);
			this.ConfirmIndividualCosmeticsSharedGroup(this.latestInventory);
			yield break;
		}

		// Token: 0x0600437F RID: 17279 RVA: 0x0013F134 File Offset: 0x0013D334
		private void ConfirmIndividualCosmeticsSharedGroup(GetUserInventoryResult inventory)
		{
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

		// Token: 0x06004380 RID: 17280 RVA: 0x0013F1C8 File Offset: 0x0013D3C8
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
				Object.DestroyImmediate(PhotonNetworkController.Instance);
				Object.DestroyImmediate(GTPlayer.Instance);
				GameObject[] array = Object.FindObjectsOfType<GameObject>();
				for (int i = 0; i < array.Length; i++)
				{
					Object.Destroy(array[i]);
				}
			}
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x0013F238 File Offset: 0x0013D438
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

		// Token: 0x06004382 RID: 17282 RVA: 0x0013F33C File Offset: 0x0013D53C
		public bool BuildValidationCheck()
		{
			if (this.m_earlyAccessSupporterPackCosmeticSO == null)
			{
				Debug.LogError("m_earlyAccessSupporterPackCosmeticSO is empty, everything will break!");
				return false;
			}
			return true;
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x0013F359 File Offset: 0x0013D559
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

		// Token: 0x06004384 RID: 17284 RVA: 0x0013F388 File Offset: 0x0013D588
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

		// Token: 0x06004387 RID: 17287 RVA: 0x0000F974 File Offset: 0x0000DB74
		bool IGorillaSliceableSimple.get_isActiveAndEnabled()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x04004468 RID: 17512
		[FormerlySerializedAs("v2AllCosmeticsInfoAssetRef")]
		[FormerlySerializedAs("newSysAllCosmeticsAssetRef")]
		[SerializeField]
		public GTAssetRef<AllCosmeticsArraySO> v2_allCosmeticsInfoAssetRef;

		// Token: 0x0400446A RID: 17514
		private readonly Dictionary<string, CosmeticInfoV2> _allCosmeticsDictV2 = new Dictionary<string, CosmeticInfoV2>();

		// Token: 0x0400446B RID: 17515
		public Action V2_allCosmeticsInfoAssetRef_OnPostLoad;

		// Token: 0x0400446F RID: 17519
		public const int maximumTransferrableItems = 5;

		// Token: 0x04004470 RID: 17520
		[OnEnterPlay_SetNull]
		public static volatile CosmeticsController instance;

		// Token: 0x04004472 RID: 17522
		public Action V2_OnGetCosmeticsPlayFabCatalogData_PostSuccess;

		// Token: 0x04004473 RID: 17523
		public Action OnGetCurrency;

		// Token: 0x04004474 RID: 17524
		[FormerlySerializedAs("allCosmetics")]
		[SerializeField]
		private List<CosmeticsController.CosmeticItem> _allCosmetics;

		// Token: 0x04004476 RID: 17526
		public Dictionary<string, CosmeticsController.CosmeticItem> _allCosmeticsDict = new Dictionary<string, CosmeticsController.CosmeticItem>(2048);

		// Token: 0x04004478 RID: 17528
		public Dictionary<string, string> _allCosmeticsItemIDsfromDisplayNamesDict = new Dictionary<string, string>(2048);

		// Token: 0x04004479 RID: 17529
		public CosmeticsController.CosmeticItem nullItem;

		// Token: 0x0400447A RID: 17530
		public string catalog;

		// Token: 0x0400447B RID: 17531
		private string[] tempStringArray;

		// Token: 0x0400447C RID: 17532
		private CosmeticsController.CosmeticItem tempItem;

		// Token: 0x0400447D RID: 17533
		private VRRigAnchorOverrides anchorOverrides;

		// Token: 0x0400447E RID: 17534
		public List<CatalogItem> catalogItems;

		// Token: 0x0400447F RID: 17535
		public bool tryTwice;

		// Token: 0x04004480 RID: 17536
		[NonSerialized]
		public CosmeticsController.CosmeticSet tryOnSet = new CosmeticsController.CosmeticSet();

		// Token: 0x04004481 RID: 17537
		public FittingRoomButton[] fittingRoomButtons;

		// Token: 0x04004482 RID: 17538
		public CosmeticStand[] cosmeticStands;

		// Token: 0x04004483 RID: 17539
		public List<CosmeticsController.CosmeticItem> currentCart = new List<CosmeticsController.CosmeticItem>();

		// Token: 0x04004484 RID: 17540
		public CosmeticsController.PurchaseItemStages currentPurchaseItemStage;

		// Token: 0x04004485 RID: 17541
		public CheckoutCartButton[] checkoutCartButtons;

		// Token: 0x04004486 RID: 17542
		public PurchaseItemButton leftPurchaseButton;

		// Token: 0x04004487 RID: 17543
		public PurchaseItemButton rightPurchaseButton;

		// Token: 0x04004488 RID: 17544
		public Text purchaseText;

		// Token: 0x04004489 RID: 17545
		public CosmeticsController.CosmeticItem itemToBuy;

		// Token: 0x0400448A RID: 17546
		public HeadModel checkoutHeadModel;

		// Token: 0x0400448B RID: 17547
		private List<string> playerIDList = new List<string>();

		// Token: 0x0400448C RID: 17548
		private List<string> inventoryStringList = new List<string>();

		// Token: 0x0400448D RID: 17549
		private bool foundCosmetic;

		// Token: 0x0400448E RID: 17550
		private int attempts;

		// Token: 0x0400448F RID: 17551
		private string finalLine;

		// Token: 0x04004490 RID: 17552
		private bool isLastHandTouchedLeft;

		// Token: 0x04004491 RID: 17553
		private CosmeticsController.CosmeticSet cachedSet = new CosmeticsController.CosmeticSet();

		// Token: 0x04004493 RID: 17555
		public readonly List<WardrobeInstance> wardrobes = new List<WardrobeInstance>();

		// Token: 0x04004494 RID: 17556
		public List<CosmeticsController.CosmeticItem> unlockedCosmetics = new List<CosmeticsController.CosmeticItem>(2048);

		// Token: 0x04004495 RID: 17557
		public List<CosmeticsController.CosmeticItem> unlockedHats = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x04004496 RID: 17558
		public List<CosmeticsController.CosmeticItem> unlockedFaces = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x04004497 RID: 17559
		public List<CosmeticsController.CosmeticItem> unlockedBadges = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x04004498 RID: 17560
		public List<CosmeticsController.CosmeticItem> unlockedPaws = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x04004499 RID: 17561
		public List<CosmeticsController.CosmeticItem> unlockedChests = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x0400449A RID: 17562
		public List<CosmeticsController.CosmeticItem> unlockedFurs = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x0400449B RID: 17563
		public List<CosmeticsController.CosmeticItem> unlockedShirts = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x0400449C RID: 17564
		public List<CosmeticsController.CosmeticItem> unlockedPants = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x0400449D RID: 17565
		public List<CosmeticsController.CosmeticItem> unlockedBacks = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x0400449E RID: 17566
		public List<CosmeticsController.CosmeticItem> unlockedArms = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x0400449F RID: 17567
		public List<CosmeticsController.CosmeticItem> unlockedTagFX = new List<CosmeticsController.CosmeticItem>(512);

		// Token: 0x040044A0 RID: 17568
		public int[] cosmeticsPages = new int[11];

		// Token: 0x040044A1 RID: 17569
		private List<CosmeticsController.CosmeticItem>[] itemLists = new List<CosmeticsController.CosmeticItem>[11];

		// Token: 0x040044A2 RID: 17570
		private int wardrobeType;

		// Token: 0x040044A3 RID: 17571
		[NonSerialized]
		public CosmeticsController.CosmeticSet currentWornSet = new CosmeticsController.CosmeticSet();

		// Token: 0x040044A4 RID: 17572
		public string concatStringCosmeticsAllowed = "";

		// Token: 0x040044A5 RID: 17573
		public Action OnCosmeticsUpdated;

		// Token: 0x040044A6 RID: 17574
		public Text infoText;

		// Token: 0x040044A7 RID: 17575
		public Text earlyAccessText;

		// Token: 0x040044A8 RID: 17576
		public Text[] purchaseButtonText;

		// Token: 0x040044A9 RID: 17577
		public Text dailyText;

		// Token: 0x040044AA RID: 17578
		public int currencyBalance;

		// Token: 0x040044AB RID: 17579
		public string currencyName;

		// Token: 0x040044AC RID: 17580
		public PurchaseCurrencyButton[] purchaseCurrencyButtons;

		// Token: 0x040044AD RID: 17581
		public Text currencyBoardText;

		// Token: 0x040044AE RID: 17582
		public Text currencyBoxText;

		// Token: 0x040044AF RID: 17583
		public string startingCurrencyBoxTextString;

		// Token: 0x040044B0 RID: 17584
		public string successfulCurrencyPurchaseTextString;

		// Token: 0x040044B1 RID: 17585
		public string itemToPurchase;

		// Token: 0x040044B2 RID: 17586
		public bool buyingBundle;

		// Token: 0x040044B3 RID: 17587
		public bool confirmedDidntPlayInBeta;

		// Token: 0x040044B4 RID: 17588
		public bool playedInBeta;

		// Token: 0x040044B5 RID: 17589
		public bool gotMyDaily;

		// Token: 0x040044B6 RID: 17590
		public bool checkedDaily;

		// Token: 0x040044B7 RID: 17591
		public string currentPurchaseID;

		// Token: 0x040044B8 RID: 17592
		public bool hasPrice;

		// Token: 0x040044B9 RID: 17593
		private int searchIndex;

		// Token: 0x040044BA RID: 17594
		private int iterator;

		// Token: 0x040044BB RID: 17595
		private CosmeticsController.CosmeticItem cosmeticItemVar;

		// Token: 0x040044BC RID: 17596
		[SerializeField]
		private CosmeticSO m_earlyAccessSupporterPackCosmeticSO;

		// Token: 0x040044BD RID: 17597
		public EarlyAccessButton[] earlyAccessButtons;

		// Token: 0x040044BE RID: 17598
		private BundleList bundleList = new BundleList();

		// Token: 0x040044BF RID: 17599
		public string BundleSkuName = "2024_i_lava_you_pack";

		// Token: 0x040044C0 RID: 17600
		public string BundlePlayfabItemName = "LSABG.";

		// Token: 0x040044C1 RID: 17601
		public int BundleShinyRocks = 10000;

		// Token: 0x040044C2 RID: 17602
		public DateTime currentTime;

		// Token: 0x040044C3 RID: 17603
		public string lastDailyLogin;

		// Token: 0x040044C4 RID: 17604
		public UserDataRecord userDataRecord;

		// Token: 0x040044C5 RID: 17605
		public int secondsUntilTomorrow;

		// Token: 0x040044C6 RID: 17606
		public float secondsToWaitToCheckDaily = 10f;

		// Token: 0x040044C7 RID: 17607
		private int updateCosmeticsRetries;

		// Token: 0x040044C8 RID: 17608
		private int maxUpdateCosmeticsRetries;

		// Token: 0x040044C9 RID: 17609
		private GetUserInventoryResult latestInventory;

		// Token: 0x040044CA RID: 17610
		private string returnString;

		// Token: 0x040044CB RID: 17611
		private Callback<MicroTxnAuthorizationResponse_t> _steamMicroTransactionAuthorizationResponse;

		// Token: 0x040044CC RID: 17612
		private static readonly List<CosmeticsController.CosmeticSlots> _g_default_outAppliedSlotsList_for_applyCosmeticItemToSet = new List<CosmeticsController.CosmeticSlots>(16);

		// Token: 0x02000A87 RID: 2695
		public enum PurchaseItemStages
		{
			// Token: 0x040044CE RID: 17614
			Start,
			// Token: 0x040044CF RID: 17615
			CheckoutButtonPressed,
			// Token: 0x040044D0 RID: 17616
			ItemSelected,
			// Token: 0x040044D1 RID: 17617
			ItemOwned,
			// Token: 0x040044D2 RID: 17618
			FinalPurchaseAcknowledgement,
			// Token: 0x040044D3 RID: 17619
			Buying,
			// Token: 0x040044D4 RID: 17620
			Success,
			// Token: 0x040044D5 RID: 17621
			Failure
		}

		// Token: 0x02000A88 RID: 2696
		public enum CosmeticCategory
		{
			// Token: 0x040044D7 RID: 17623
			None,
			// Token: 0x040044D8 RID: 17624
			Hat,
			// Token: 0x040044D9 RID: 17625
			Badge,
			// Token: 0x040044DA RID: 17626
			Face,
			// Token: 0x040044DB RID: 17627
			Paw,
			// Token: 0x040044DC RID: 17628
			Chest,
			// Token: 0x040044DD RID: 17629
			Fur,
			// Token: 0x040044DE RID: 17630
			Shirt,
			// Token: 0x040044DF RID: 17631
			Back,
			// Token: 0x040044E0 RID: 17632
			Arms,
			// Token: 0x040044E1 RID: 17633
			Pants,
			// Token: 0x040044E2 RID: 17634
			TagEffect,
			// Token: 0x040044E3 RID: 17635
			Count,
			// Token: 0x040044E4 RID: 17636
			Set
		}

		// Token: 0x02000A89 RID: 2697
		public enum CosmeticSlots
		{
			// Token: 0x040044E6 RID: 17638
			Hat,
			// Token: 0x040044E7 RID: 17639
			Badge,
			// Token: 0x040044E8 RID: 17640
			Face,
			// Token: 0x040044E9 RID: 17641
			ArmLeft,
			// Token: 0x040044EA RID: 17642
			ArmRight,
			// Token: 0x040044EB RID: 17643
			BackLeft,
			// Token: 0x040044EC RID: 17644
			BackRight,
			// Token: 0x040044ED RID: 17645
			HandLeft,
			// Token: 0x040044EE RID: 17646
			HandRight,
			// Token: 0x040044EF RID: 17647
			Chest,
			// Token: 0x040044F0 RID: 17648
			Fur,
			// Token: 0x040044F1 RID: 17649
			Shirt,
			// Token: 0x040044F2 RID: 17650
			Pants,
			// Token: 0x040044F3 RID: 17651
			Back,
			// Token: 0x040044F4 RID: 17652
			Arms,
			// Token: 0x040044F5 RID: 17653
			TagEffect,
			// Token: 0x040044F6 RID: 17654
			Count
		}

		// Token: 0x02000A8A RID: 2698
		[Serializable]
		public class CosmeticSet
		{
			// Token: 0x1400007F RID: 127
			// (add) Token: 0x06004395 RID: 17301 RVA: 0x0013F974 File Offset: 0x0013DB74
			// (remove) Token: 0x06004396 RID: 17302 RVA: 0x0013F9AC File Offset: 0x0013DBAC
			public event CosmeticsController.CosmeticSet.OnSetActivatedHandler onSetActivatedEvent;

			// Token: 0x06004397 RID: 17303 RVA: 0x0013F9E1 File Offset: 0x0013DBE1
			protected void OnSetActivated(CosmeticsController.CosmeticSet prevSet, CosmeticsController.CosmeticSet currentSet, NetPlayer netPlayer)
			{
				if (this.onSetActivatedEvent != null)
				{
					this.onSetActivatedEvent(prevSet, currentSet, netPlayer);
				}
			}

			// Token: 0x170006FA RID: 1786
			// (get) Token: 0x06004398 RID: 17304 RVA: 0x0013F9FC File Offset: 0x0013DBFC
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

			// Token: 0x06004399 RID: 17305 RVA: 0x0013FA45 File Offset: 0x0013DC45
			public CosmeticSet()
			{
				this.items = new CosmeticsController.CosmeticItem[16];
			}

			// Token: 0x0600439A RID: 17306 RVA: 0x0013FA68 File Offset: 0x0013DC68
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

			// Token: 0x0600439B RID: 17307 RVA: 0x0013FAC4 File Offset: 0x0013DCC4
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

			// Token: 0x0600439C RID: 17308 RVA: 0x0013FBCC File Offset: 0x0013DDCC
			public void CopyItems(CosmeticsController.CosmeticSet other)
			{
				for (int i = 0; i < this.items.Length; i++)
				{
					this.items[i] = other.items[i];
				}
			}

			// Token: 0x0600439D RID: 17309 RVA: 0x0013FC04 File Offset: 0x0013DE04
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

			// Token: 0x0600439E RID: 17310 RVA: 0x0013FC74 File Offset: 0x0013DE74
			public void ClearSet(CosmeticsController.CosmeticItem nullItem)
			{
				for (int i = 0; i < 16; i++)
				{
					this.items[i] = nullItem;
				}
			}

			// Token: 0x0600439F RID: 17311 RVA: 0x0013FC9C File Offset: 0x0013DE9C
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

			// Token: 0x060043A0 RID: 17312 RVA: 0x0013FCD4 File Offset: 0x0013DED4
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

			// Token: 0x060043A1 RID: 17313 RVA: 0x0013FD1C File Offset: 0x0013DF1C
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

			// Token: 0x060043A2 RID: 17314 RVA: 0x0013FD67 File Offset: 0x0013DF67
			public static bool IsSlotLeftHanded(CosmeticsController.CosmeticSlots slot)
			{
				return slot == CosmeticsController.CosmeticSlots.ArmLeft || slot == CosmeticsController.CosmeticSlots.BackLeft || slot == CosmeticsController.CosmeticSlots.HandLeft;
			}

			// Token: 0x060043A3 RID: 17315 RVA: 0x0013FD77 File Offset: 0x0013DF77
			public static bool IsSlotRightHanded(CosmeticsController.CosmeticSlots slot)
			{
				return slot == CosmeticsController.CosmeticSlots.ArmRight || slot == CosmeticsController.CosmeticSlots.BackRight || slot == CosmeticsController.CosmeticSlots.HandRight;
			}

			// Token: 0x060043A4 RID: 17316 RVA: 0x0013FD87 File Offset: 0x0013DF87
			public static bool IsHoldable(CosmeticsController.CosmeticItem item)
			{
				return item.isHoldable;
			}

			// Token: 0x060043A5 RID: 17317 RVA: 0x0013FD90 File Offset: 0x0013DF90
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

			// Token: 0x060043A6 RID: 17318 RVA: 0x0013FE0E File Offset: 0x0013E00E
			public static string SlotPlayerPreferenceName(CosmeticsController.CosmeticSlots slot)
			{
				return "slot_" + slot.ToString();
			}

			// Token: 0x060043A7 RID: 17319 RVA: 0x0013FE28 File Offset: 0x0013E028
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

			// Token: 0x060043A8 RID: 17320 RVA: 0x0013FF64 File Offset: 0x0013E164
			public void ActivateCosmetics(CosmeticsController.CosmeticSet prevSet, VRRig rig, BodyDockPositions bDock, CosmeticItemRegistry cosmeticsObjectRegistry)
			{
				int num = 16;
				for (int i = 0; i < num; i++)
				{
					this.ActivateCosmetic(prevSet, rig, i, cosmeticsObjectRegistry, bDock);
				}
				this.OnSetActivated(prevSet, this, rig.creator);
			}

			// Token: 0x060043A9 RID: 17321 RVA: 0x0013FF9C File Offset: 0x0013E19C
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

			// Token: 0x060043AA RID: 17322 RVA: 0x0013FFFC File Offset: 0x0013E1FC
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

			// Token: 0x060043AB RID: 17323 RVA: 0x00140118 File Offset: 0x0013E318
			public string[] ToDisplayNameArray()
			{
				int num = 16;
				for (int i = 0; i < num; i++)
				{
					this.returnArray[i] = (string.IsNullOrEmpty(this.items[i].displayName) ? "null" : this.items[i].displayName);
				}
				return this.returnArray;
			}

			// Token: 0x060043AC RID: 17324 RVA: 0x00140174 File Offset: 0x0013E374
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

			// Token: 0x060043AD RID: 17325 RVA: 0x00140274 File Offset: 0x0013E474
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

			// Token: 0x060043AE RID: 17326 RVA: 0x00140388 File Offset: 0x0013E588
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

			// Token: 0x040044F7 RID: 17655
			public CosmeticsController.CosmeticItem[] items;

			// Token: 0x040044F9 RID: 17657
			public string[] returnArray = new string[16];

			// Token: 0x040044FA RID: 17658
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

			// Token: 0x040044FB RID: 17659
			private static CosmeticsController.CosmeticSet _emptySet;

			// Token: 0x040044FC RID: 17660
			private static char[] nameScratchSpace = new char[6];

			// Token: 0x02000A8B RID: 2699
			// (Invoke) Token: 0x060043B1 RID: 17329
			public delegate void OnSetActivatedHandler(CosmeticsController.CosmeticSet prevSet, CosmeticsController.CosmeticSet currentSet, NetPlayer netPlayer);
		}

		// Token: 0x02000A8D RID: 2701
		[Serializable]
		public struct CosmeticItem
		{
			// Token: 0x040044FE RID: 17662
			[Tooltip("Should match the spreadsheet item name.")]
			public string itemName;

			// Token: 0x040044FF RID: 17663
			[Tooltip("Determines what wardrobe section the item will show up in.")]
			public CosmeticsController.CosmeticCategory itemCategory;

			// Token: 0x04004500 RID: 17664
			[Tooltip("If this is a holdable item.")]
			public bool isHoldable;

			// Token: 0x04004501 RID: 17665
			[Tooltip("Icon shown in the store menus & hunt watch.")]
			public Sprite itemPicture;

			// Token: 0x04004502 RID: 17666
			public string displayName;

			// Token: 0x04004503 RID: 17667
			public string itemPictureResourceString;

			// Token: 0x04004504 RID: 17668
			[Tooltip("The name shown on the store checkout screen.")]
			public string overrideDisplayName;

			// Token: 0x04004505 RID: 17669
			[DebugReadout]
			[NonSerialized]
			public int cost;

			// Token: 0x04004506 RID: 17670
			[DebugReadout]
			[NonSerialized]
			public string[] bundledItems;

			// Token: 0x04004507 RID: 17671
			[DebugReadout]
			[NonSerialized]
			public bool canTryOn;

			// Token: 0x04004508 RID: 17672
			[Tooltip("Set to true if the item takes up both left and right wearable hand slots at the same time. Used for things like mittens/gloves.")]
			public bool bothHandsHoldable;

			// Token: 0x04004509 RID: 17673
			public bool bLoadsFromResources;

			// Token: 0x0400450A RID: 17674
			public bool bUsesMeshAtlas;

			// Token: 0x0400450B RID: 17675
			public Vector3 rotationOffset;

			// Token: 0x0400450C RID: 17676
			public Vector3 positionOffset;

			// Token: 0x0400450D RID: 17677
			public string meshAtlasResourceString;

			// Token: 0x0400450E RID: 17678
			public string meshResourceString;

			// Token: 0x0400450F RID: 17679
			public string materialResourceString;

			// Token: 0x04004510 RID: 17680
			[HideInInspector]
			public bool isNullItem;
		}

		// Token: 0x02000A8E RID: 2702
		[Serializable]
		public class IAPRequestBody
		{
			// Token: 0x04004511 RID: 17681
			public string accessToken;

			// Token: 0x04004512 RID: 17682
			public string userID;

			// Token: 0x04004513 RID: 17683
			public string nonce;

			// Token: 0x04004514 RID: 17684
			public string platform;

			// Token: 0x04004515 RID: 17685
			public string sku;

			// Token: 0x04004516 RID: 17686
			public string playFabId;

			// Token: 0x04004517 RID: 17687
			public bool[] debugParameters;

			// Token: 0x04004518 RID: 17688
			public Dictionary<string, string> customTags;
		}

		// Token: 0x02000A8F RID: 2703
		public enum EWearingCosmeticSet
		{
			// Token: 0x0400451A RID: 17690
			NotASet,
			// Token: 0x0400451B RID: 17691
			NotWearing,
			// Token: 0x0400451C RID: 17692
			Partial,
			// Token: 0x0400451D RID: 17693
			Complete
		}
	}
}
