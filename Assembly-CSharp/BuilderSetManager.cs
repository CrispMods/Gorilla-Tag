using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ExitGames.Client.Photon;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004F8 RID: 1272
public class BuilderSetManager : MonoBehaviour
{
	// Token: 0x17000322 RID: 802
	// (get) Token: 0x06001EE6 RID: 7910 RVA: 0x00044F41 File Offset: 0x00043141
	// (set) Token: 0x06001EE7 RID: 7911 RVA: 0x00044F48 File Offset: 0x00043148
	public static bool hasInstance { get; private set; }

	// Token: 0x06001EE8 RID: 7912 RVA: 0x000EC5D0 File Offset: 0x000EA7D0
	public string GetStarterSetsConcat()
	{
		if (BuilderSetManager.concatStarterSets.Length > 0)
		{
			return BuilderSetManager.concatStarterSets;
		}
		BuilderSetManager.concatStarterSets = string.Empty;
		foreach (BuilderPieceSet builderPieceSet in this._starterPieceSets)
		{
			BuilderSetManager.concatStarterSets += builderPieceSet.playfabID;
		}
		return BuilderSetManager.concatStarterSets;
	}

	// Token: 0x06001EE9 RID: 7913 RVA: 0x000EC654 File Offset: 0x000EA854
	public string GetAllSetsConcat()
	{
		if (BuilderSetManager.concatAllSets.Length > 0)
		{
			return BuilderSetManager.concatAllSets;
		}
		BuilderSetManager.concatAllSets = string.Empty;
		foreach (BuilderPieceSet builderPieceSet in this._allPieceSets)
		{
			BuilderSetManager.concatAllSets += builderPieceSet.playfabID;
		}
		return BuilderSetManager.concatAllSets;
	}

	// Token: 0x06001EEA RID: 7914 RVA: 0x000EC6D8 File Offset: 0x000EA8D8
	public void Awake()
	{
		if (BuilderSetManager.instance == null)
		{
			BuilderSetManager.instance = this;
			BuilderSetManager.hasInstance = true;
		}
		else if (BuilderSetManager.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.Init();
		if (this.monitor == null)
		{
			this.monitor = base.StartCoroutine(this.MonitorTime());
		}
	}

	// Token: 0x06001EEB RID: 7915 RVA: 0x000EC740 File Offset: 0x000EA940
	private void Init()
	{
		this.catalog = "DLC";
		this.currencyName = "SR";
		this.pulledStoreItems = false;
		BuilderSetManager._setIdToStoreItem = new Dictionary<int, BuilderSetManager.BuilderSetStoreItem>(this._allPieceSets.Count);
		BuilderSetManager._setIdToStoreItem.Clear();
		BuilderSetManager.pieceSetInfos = new List<BuilderSetManager.BuilderPieceSetInfo>(this._allPieceSets.Count * 45);
		BuilderSetManager.pieceSetInfoMap = new Dictionary<int, int>(this._allPieceSets.Count * 45);
		this.livePieceSets = new List<BuilderPieceSet>(this._allPieceSets.Count);
		this.scheduledPieceSets = new List<BuilderPieceSet>(this._allPieceSets.Count);
		foreach (BuilderPieceSet builderPieceSet in this._allPieceSets)
		{
			BuilderSetManager.BuilderSetStoreItem value = new BuilderSetManager.BuilderSetStoreItem
			{
				displayName = builderPieceSet.setName,
				playfabID = builderPieceSet.playfabID,
				setID = builderPieceSet.GetIntIdentifier(),
				cost = 0U,
				setRef = builderPieceSet,
				displayModel = builderPieceSet.displayModel,
				isNullItem = false
			};
			BuilderSetManager._setIdToStoreItem.TryAdd(builderPieceSet.GetIntIdentifier(), value);
			int num = -1;
			if (!string.IsNullOrEmpty(builderPieceSet.materialId))
			{
				num = builderPieceSet.materialId.GetHashCode();
			}
			for (int i = 0; i < builderPieceSet.subsets.Count; i++)
			{
				BuilderPieceSet.BuilderPieceSubset builderPieceSubset = builderPieceSet.subsets[i];
				for (int j = 0; j < builderPieceSubset.pieceInfos.Count; j++)
				{
					BuilderPiece piecePrefab = builderPieceSubset.pieceInfos[j].piecePrefab;
					int staticHash = piecePrefab.name.GetStaticHash();
					int pieceMaterial = num;
					if (piecePrefab.materialOptions == null)
					{
						pieceMaterial = -1;
						this.AddPieceToInfoMap(staticHash, pieceMaterial, builderPieceSet.GetIntIdentifier());
					}
					else if (builderPieceSubset.pieceInfos[j].overrideSetMaterial)
					{
						if (builderPieceSubset.pieceInfos[j].pieceMaterialTypes.Length == 0)
						{
							Debug.LogErrorFormat("Material List for piece {0} in set {1} is empty", new object[]
							{
								piecePrefab.name,
								builderPieceSet.setName
							});
						}
						foreach (string text in builderPieceSubset.pieceInfos[j].pieceMaterialTypes)
						{
							if (string.IsNullOrEmpty(text))
							{
								Debug.LogErrorFormat("Material List Entry for piece {0} in set {1} is empty", new object[]
								{
									piecePrefab.name,
									builderPieceSet.setName
								});
							}
							else
							{
								pieceMaterial = text.GetHashCode();
								this.AddPieceToInfoMap(staticHash, pieceMaterial, builderPieceSet.GetIntIdentifier());
							}
						}
					}
					else
					{
						Material x;
						int num2;
						piecePrefab.materialOptions.GetMaterialFromType(num, out x, out num2);
						if (x == null)
						{
							pieceMaterial = -1;
						}
						this.AddPieceToInfoMap(staticHash, pieceMaterial, builderPieceSet.GetIntIdentifier());
					}
				}
			}
			if (!builderPieceSet.isScheduled)
			{
				this.livePieceSets.Add(builderPieceSet);
			}
			else
			{
				this.scheduledPieceSets.Add(builderPieceSet);
			}
		}
		this._unlockedPieceSets = new List<BuilderPieceSet>(this._allPieceSets.Count);
		this._unlockedPieceSets.AddRange(this._starterPieceSets);
	}

	// Token: 0x06001EEC RID: 7916 RVA: 0x00044F50 File Offset: 0x00043150
	private void OnEnable()
	{
		if (this.monitor == null && this.scheduledPieceSets.Count > 0)
		{
			this.monitor = base.StartCoroutine(this.MonitorTime());
		}
	}

	// Token: 0x06001EED RID: 7917 RVA: 0x00044F7A File Offset: 0x0004317A
	private void OnDisable()
	{
		if (this.monitor != null)
		{
			base.StopCoroutine(this.monitor);
		}
		this.monitor = null;
	}

	// Token: 0x06001EEE RID: 7918 RVA: 0x00044F97 File Offset: 0x00043197
	private IEnumerator MonitorTime()
	{
		while (GorillaComputer.instance == null || GorillaComputer.instance.startupMillis == 0L)
		{
			yield return null;
		}
		while (this.scheduledPieceSets.Count > 0)
		{
			bool flag = false;
			for (int i = this.scheduledPieceSets.Count - 1; i >= 0; i--)
			{
				BuilderPieceSet builderPieceSet = this.scheduledPieceSets[i];
				if (GorillaComputer.instance.GetServerTime() > builderPieceSet.GetScheduleDateTime())
				{
					flag = true;
					this.livePieceSets.Add(builderPieceSet);
					this.scheduledPieceSets.RemoveAt(i);
				}
			}
			if (flag)
			{
				this.OnLiveSetsUpdated.Invoke();
			}
			yield return new WaitForSeconds(60f);
		}
		this.monitor = null;
		yield break;
	}

	// Token: 0x06001EEF RID: 7919 RVA: 0x000ECA98 File Offset: 0x000EAC98
	private void AddPieceToInfoMap(int pieceType, int pieceMaterial, int setID)
	{
		int index;
		if (BuilderSetManager.pieceSetInfoMap.TryGetValue(HashCode.Combine<int, int>(pieceType, pieceMaterial), out index))
		{
			BuilderSetManager.BuilderPieceSetInfo builderPieceSetInfo = BuilderSetManager.pieceSetInfos[index];
			if (!builderPieceSetInfo.setIds.Contains(setID))
			{
				builderPieceSetInfo.setIds.Add(setID);
			}
			BuilderSetManager.pieceSetInfos[index] = builderPieceSetInfo;
			return;
		}
		BuilderSetManager.BuilderPieceSetInfo item = new BuilderSetManager.BuilderPieceSetInfo
		{
			pieceType = pieceType,
			materialType = pieceMaterial,
			setIds = new List<int>
			{
				setID
			}
		};
		BuilderSetManager.pieceSetInfoMap.Add(HashCode.Combine<int, int>(pieceType, pieceMaterial), BuilderSetManager.pieceSetInfos.Count);
		BuilderSetManager.pieceSetInfos.Add(item);
	}

	// Token: 0x06001EF0 RID: 7920 RVA: 0x00044FA6 File Offset: 0x000431A6
	public static bool IsItemIDBuilderItem(string playfabID)
	{
		return BuilderSetManager.instance.GetAllSetsConcat().Contains(playfabID);
	}

	// Token: 0x06001EF1 RID: 7921 RVA: 0x000ECB40 File Offset: 0x000EAD40
	public void OnGotInventoryItems(GetUserInventoryResult inventoryResult, GetCatalogItemsResult catalogResult)
	{
		CosmeticsController cosmeticsController = CosmeticsController.instance;
		cosmeticsController.concatStringCosmeticsAllowed += this.GetStarterSetsConcat();
		this._unlockedPieceSets.Clear();
		this._unlockedPieceSets.AddRange(this._starterPieceSets);
		foreach (CatalogItem catalogItem in catalogResult.Catalog)
		{
			BuilderSetManager.BuilderSetStoreItem builderSetStoreItem;
			if (BuilderSetManager.IsItemIDBuilderItem(catalogItem.ItemId) && BuilderSetManager._setIdToStoreItem.TryGetValue(catalogItem.ItemId.GetStaticHash(), out builderSetStoreItem))
			{
				bool hasPrice = false;
				uint cost = 0U;
				if (catalogItem.VirtualCurrencyPrices.TryGetValue(this.currencyName, out cost))
				{
					hasPrice = true;
				}
				builderSetStoreItem.playfabID = catalogItem.ItemId;
				builderSetStoreItem.cost = cost;
				builderSetStoreItem.hasPrice = hasPrice;
				BuilderSetManager._setIdToStoreItem[builderSetStoreItem.setRef.GetIntIdentifier()] = builderSetStoreItem;
			}
		}
		foreach (ItemInstance itemInstance in inventoryResult.Inventory)
		{
			if (BuilderSetManager.IsItemIDBuilderItem(itemInstance.ItemId))
			{
				BuilderSetManager.BuilderSetStoreItem builderSetStoreItem2;
				if (BuilderSetManager._setIdToStoreItem.TryGetValue(itemInstance.ItemId.GetStaticHash(), out builderSetStoreItem2))
				{
					Debug.LogFormat("BuilderSetManager: Unlocking Inventory Item {0}", new object[]
					{
						itemInstance.ItemId
					});
					this._unlockedPieceSets.Add(builderSetStoreItem2.setRef);
					CosmeticsController cosmeticsController2 = CosmeticsController.instance;
					cosmeticsController2.concatStringCosmeticsAllowed += itemInstance.ItemId;
				}
				else
				{
					Debug.Log("BuilderSetManager: No store item found with id" + itemInstance.ItemId);
				}
			}
		}
		this.pulledStoreItems = true;
		UnityEvent onOwnedSetsUpdated = this.OnOwnedSetsUpdated;
		if (onOwnedSetsUpdated == null)
		{
			return;
		}
		onOwnedSetsUpdated.Invoke();
	}

	// Token: 0x06001EF2 RID: 7922 RVA: 0x00044FBA File Offset: 0x000431BA
	public BuilderSetManager.BuilderSetStoreItem GetStoreItemFromSetID(int setID)
	{
		return BuilderSetManager._setIdToStoreItem.GetValueOrDefault(setID, BuilderKiosk.nullItem);
	}

	// Token: 0x06001EF3 RID: 7923 RVA: 0x000ECD24 File Offset: 0x000EAF24
	public BuilderPieceSet GetPieceSetFromID(int setID)
	{
		BuilderSetManager.BuilderSetStoreItem builderSetStoreItem;
		if (BuilderSetManager._setIdToStoreItem.TryGetValue(setID, out builderSetStoreItem))
		{
			return builderSetStoreItem.setRef;
		}
		return null;
	}

	// Token: 0x06001EF4 RID: 7924 RVA: 0x00044FCC File Offset: 0x000431CC
	public List<BuilderPieceSet> GetAllPieceSets()
	{
		return this._allPieceSets;
	}

	// Token: 0x06001EF5 RID: 7925 RVA: 0x00044FD4 File Offset: 0x000431D4
	public List<BuilderPieceSet> GetLivePieceSets()
	{
		return this.livePieceSets;
	}

	// Token: 0x06001EF6 RID: 7926 RVA: 0x00044FDC File Offset: 0x000431DC
	public List<BuilderPieceSet> GetUnlockedPieceSets()
	{
		return this._unlockedPieceSets;
	}

	// Token: 0x06001EF7 RID: 7927 RVA: 0x00044FE4 File Offset: 0x000431E4
	public List<BuilderPieceSet> GetPermanentSetsForSale()
	{
		return this._setsAlwaysForSale;
	}

	// Token: 0x06001EF8 RID: 7928 RVA: 0x00044FEC File Offset: 0x000431EC
	public List<BuilderPieceSet> GetSeasonalSetsForSale()
	{
		return this._seasonalSetsForSale;
	}

	// Token: 0x06001EF9 RID: 7929 RVA: 0x000ECD48 File Offset: 0x000EAF48
	public bool IsSetSeasonal(string playfabID)
	{
		return !this._seasonalSetsForSale.IsNullOrEmpty<BuilderPieceSet>() && this._seasonalSetsForSale.FindIndex((BuilderPieceSet x) => x.playfabID.Equals(playfabID)) >= 0;
	}

	// Token: 0x06001EFA RID: 7930 RVA: 0x000ECD90 File Offset: 0x000EAF90
	public bool DoesPlayerOwnPieceSet(Player player, int setID)
	{
		BuilderPieceSet pieceSetFromID = this.GetPieceSetFromID(setID);
		if (pieceSetFromID == null)
		{
			return false;
		}
		RigContainer rigContainer;
		if (VRRigCache.Instance.TryGetVrrig(player, out rigContainer))
		{
			bool flag = rigContainer.Rig.IsItemAllowed(pieceSetFromID.playfabID);
			Debug.LogFormat("BuilderSetManager: does player {0} own set {1} {2}", new object[]
			{
				player.ActorNumber,
				pieceSetFromID.setName,
				flag
			});
			return flag;
		}
		Debug.LogFormat("BuilderSetManager: could not get rig for player {0}", new object[]
		{
			player.ActorNumber
		});
		return false;
	}

	// Token: 0x06001EFB RID: 7931 RVA: 0x000ECE24 File Offset: 0x000EB024
	public bool DoesAnyPlayerInRoomOwnPieceSet(int setID)
	{
		BuilderPieceSet pieceSetFromID = this.GetPieceSetFromID(setID);
		if (pieceSetFromID == null)
		{
			return false;
		}
		if (this.GetStarterSetsConcat().Contains(pieceSetFromID.setName))
		{
			return true;
		}
		foreach (NetPlayer targetPlayer in RoomSystem.PlayersInRoom)
		{
			RigContainer rigContainer;
			if (VRRigCache.Instance.TryGetVrrig(targetPlayer, out rigContainer) && rigContainer.Rig.IsItemAllowed(pieceSetFromID.playfabID))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001EFC RID: 7932 RVA: 0x000ECEC4 File Offset: 0x000EB0C4
	public bool IsPieceOwnedByRoom(int pieceType, int materialType)
	{
		int index;
		if (BuilderSetManager.pieceSetInfoMap.TryGetValue(HashCode.Combine<int, int>(pieceType, materialType), out index))
		{
			foreach (int setID in BuilderSetManager.pieceSetInfos[index].setIds)
			{
				if (this.DoesAnyPlayerInRoomOwnPieceSet(setID))
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x06001EFD RID: 7933 RVA: 0x000ECF44 File Offset: 0x000EB144
	public bool IsPieceOwnedLocally(int pieceType, int materialType)
	{
		int index;
		if (BuilderSetManager.pieceSetInfoMap.TryGetValue(HashCode.Combine<int, int>(pieceType, materialType), out index))
		{
			foreach (int setID in BuilderSetManager.pieceSetInfos[index].setIds)
			{
				if (this.IsPieceSetOwnedLocally(setID))
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x06001EFE RID: 7934 RVA: 0x000ECFC4 File Offset: 0x000EB1C4
	public bool IsPieceSetOwnedLocally(int setID)
	{
		return this._unlockedPieceSets.FindIndex((BuilderPieceSet x) => setID == x.GetIntIdentifier()) >= 0;
	}

	// Token: 0x06001EFF RID: 7935 RVA: 0x000ECFFC File Offset: 0x000EB1FC
	public void UnlockSet(int setID)
	{
		int num = this._allPieceSets.FindIndex((BuilderPieceSet x) => setID == x.GetIntIdentifier());
		if (num >= 0 && !this._unlockedPieceSets.Contains(this._allPieceSets[num]))
		{
			Debug.Log("BuilderSetManager: unlocking set " + this._allPieceSets[num].setName);
			this._unlockedPieceSets.Add(this._allPieceSets[num]);
		}
		UnityEvent onOwnedSetsUpdated = this.OnOwnedSetsUpdated;
		if (onOwnedSetsUpdated == null)
		{
			return;
		}
		onOwnedSetsUpdated.Invoke();
	}

	// Token: 0x06001F00 RID: 7936 RVA: 0x000ED094 File Offset: 0x000EB294
	public void TryPurchaseItem(int setID, Action<bool> resultCallback)
	{
		BuilderSetManager.BuilderSetStoreItem storeItem;
		if (!BuilderSetManager._setIdToStoreItem.TryGetValue(setID, out storeItem))
		{
			Debug.Log("BuilderSetManager: no store Item for set " + setID.ToString());
			Action<bool> resultCallback2 = resultCallback;
			if (resultCallback2 == null)
			{
				return;
			}
			resultCallback2(false);
			return;
		}
		else
		{
			if (!this.IsPieceSetOwnedLocally(setID))
			{
				PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
				{
					ItemId = storeItem.playfabID,
					Price = (int)storeItem.cost,
					VirtualCurrency = this.currencyName,
					CatalogVersion = this.catalog
				}, delegate(PurchaseItemResult result)
				{
					if (result.Items.Count > 0)
					{
						foreach (ItemInstance itemInstance in result.Items)
						{
							Debug.Log("BuilderSetManager: unlocking set " + itemInstance.ItemId);
							this.UnlockSet(itemInstance.ItemId.GetStaticHash());
						}
						CosmeticsController.instance.UpdateMyCosmetics();
						if (PhotonNetwork.InRoom)
						{
							this.StartCoroutine(this.CheckIfMyCosmeticsUpdated(storeItem.playfabID));
						}
						Action<bool> resultCallback4 = resultCallback;
						if (resultCallback4 == null)
						{
							return;
						}
						resultCallback4(true);
						return;
					}
					else
					{
						Debug.Log("BuilderSetManager: no items purchased ");
						Action<bool> resultCallback5 = resultCallback;
						if (resultCallback5 == null)
						{
							return;
						}
						resultCallback5(false);
						return;
					}
				}, delegate(PlayFabError error)
				{
					Debug.LogErrorFormat("BuilderSetManager: purchase {0} Error {1}", new object[]
					{
						setID,
						error.ErrorMessage
					});
					Action<bool> resultCallback4 = resultCallback;
					if (resultCallback4 == null)
					{
						return;
					}
					resultCallback4(false);
				}, null, null);
				return;
			}
			Debug.Log("BuilderSetManager: set already owned " + setID.ToString());
			Action<bool> resultCallback3 = resultCallback;
			if (resultCallback3 == null)
			{
				return;
			}
			resultCallback3(false);
			return;
		}
	}

	// Token: 0x06001F01 RID: 7937 RVA: 0x00044FF4 File Offset: 0x000431F4
	private IEnumerator CheckIfMyCosmeticsUpdated(string itemToBuyID)
	{
		yield return new WaitForSeconds(1f);
		this.foundCosmetic = false;
		this.attempts = 0;
		while (!this.foundCosmetic && this.attempts < 10 && PhotonNetwork.InRoom)
		{
			this.playerIDList.Clear();
			if (GorillaServer.Instance != null && GorillaServer.Instance.NewCosmeticsPath())
			{
				this.playerIDList.Add("Inventory");
				PlayFabClientAPI.GetSharedGroupData(new PlayFab.ClientModels.GetSharedGroupDataRequest
				{
					Keys = this.playerIDList,
					SharedGroupId = PhotonNetwork.LocalPlayer.UserId + "Inventory"
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
					bool flag = this.foundCosmetic;
				}, delegate(PlayFabError error)
				{
					this.attempts++;
					CosmeticsController.instance.ReauthOrBan(error);
				}, null, null);
				yield return new WaitForSeconds(1f);
			}
			else
			{
				this.playerIDList.Add(PhotonNetwork.LocalPlayer.ActorNumber.ToString());
				PlayFabClientAPI.GetSharedGroupData(new PlayFab.ClientModels.GetSharedGroupDataRequest
				{
					Keys = this.playerIDList,
					SharedGroupId = PhotonNetwork.CurrentRoom.Name + Regex.Replace(PhotonNetwork.CloudRegion, "[^a-zA-Z0-9]", "").ToUpper()
				}, delegate(GetSharedGroupDataResult result)
				{
					this.attempts++;
					foreach (KeyValuePair<string, PlayFab.ClientModels.SharedGroupDataRecord> keyValuePair in result.Data)
					{
						if (keyValuePair.Value.Value.Contains(itemToBuyID))
						{
							Debug.Log("BuilderSetManager: found it! updating others cosmetic!");
							PhotonNetwork.RaiseEvent(199, null, new RaiseEventOptions
							{
								Receivers = ReceiverGroup.Others
							}, SendOptions.SendReliable);
							this.foundCosmetic = true;
						}
						else
						{
							Debug.Log("BuilderSetManager: didnt find it, updating attempts and trying again in a bit. current attempt is " + this.attempts.ToString());
						}
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
					Debug.Log("BuilderSetManager: Got error retrieving user data, on attempt " + this.attempts.ToString());
					Debug.Log(error.GenerateErrorReport());
				}, null, null);
				yield return new WaitForSeconds(1f);
			}
		}
		Debug.Log("BuilderSetManager: done!");
		yield break;
	}

	// Token: 0x0400228B RID: 8843
	[SerializeField]
	private List<BuilderPieceSet> _allPieceSets;

	// Token: 0x0400228C RID: 8844
	[SerializeField]
	private List<BuilderPieceSet> _starterPieceSets;

	// Token: 0x0400228D RID: 8845
	[SerializeField]
	private List<BuilderPieceSet> _setsAlwaysForSale;

	// Token: 0x0400228E RID: 8846
	[SerializeField]
	private List<BuilderPieceSet> _seasonalSetsForSale;

	// Token: 0x0400228F RID: 8847
	private List<BuilderPieceSet> livePieceSets;

	// Token: 0x04002290 RID: 8848
	private List<BuilderPieceSet> scheduledPieceSets;

	// Token: 0x04002291 RID: 8849
	private Coroutine monitor;

	// Token: 0x04002292 RID: 8850
	private List<BuilderSetManager.BuilderSetStoreItem> _allStoreItems;

	// Token: 0x04002293 RID: 8851
	private List<BuilderPieceSet> _unlockedPieceSets;

	// Token: 0x04002294 RID: 8852
	private static Dictionary<int, BuilderSetManager.BuilderSetStoreItem> _setIdToStoreItem;

	// Token: 0x04002295 RID: 8853
	private static List<BuilderSetManager.BuilderPieceSetInfo> pieceSetInfos;

	// Token: 0x04002296 RID: 8854
	private static Dictionary<int, int> pieceSetInfoMap;

	// Token: 0x04002297 RID: 8855
	[OnEnterPlay_SetNull]
	public static volatile BuilderSetManager instance;

	// Token: 0x04002299 RID: 8857
	[HideInInspector]
	public string catalog;

	// Token: 0x0400229A RID: 8858
	[HideInInspector]
	public string currencyName;

	// Token: 0x0400229B RID: 8859
	private string[] tempStringArray;

	// Token: 0x0400229C RID: 8860
	[HideInInspector]
	public UnityEvent OnLiveSetsUpdated;

	// Token: 0x0400229D RID: 8861
	[HideInInspector]
	public UnityEvent OnOwnedSetsUpdated;

	// Token: 0x0400229E RID: 8862
	[HideInInspector]
	public bool pulledStoreItems;

	// Token: 0x0400229F RID: 8863
	private static string concatStarterSets = string.Empty;

	// Token: 0x040022A0 RID: 8864
	private static string concatAllSets = string.Empty;

	// Token: 0x040022A1 RID: 8865
	private bool foundCosmetic;

	// Token: 0x040022A2 RID: 8866
	private int attempts;

	// Token: 0x040022A3 RID: 8867
	private List<string> playerIDList = new List<string>();

	// Token: 0x020004F9 RID: 1273
	[Serializable]
	public struct BuilderSetStoreItem
	{
		// Token: 0x040022A4 RID: 8868
		public string displayName;

		// Token: 0x040022A5 RID: 8869
		public string playfabID;

		// Token: 0x040022A6 RID: 8870
		public int setID;

		// Token: 0x040022A7 RID: 8871
		public uint cost;

		// Token: 0x040022A8 RID: 8872
		public bool hasPrice;

		// Token: 0x040022A9 RID: 8873
		public BuilderPieceSet setRef;

		// Token: 0x040022AA RID: 8874
		public GameObject displayModel;

		// Token: 0x040022AB RID: 8875
		[NonSerialized]
		public bool isNullItem;
	}

	// Token: 0x020004FA RID: 1274
	[Serializable]
	public struct BuilderPieceSetInfo
	{
		// Token: 0x040022AC RID: 8876
		public int pieceType;

		// Token: 0x040022AD RID: 8877
		public int materialType;

		// Token: 0x040022AE RID: 8878
		public List<int> setIds;
	}
}
