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

// Token: 0x020004EB RID: 1259
public class BuilderSetManager : MonoBehaviour
{
	// Token: 0x1700031B RID: 795
	// (get) Token: 0x06001E8D RID: 7821 RVA: 0x00099FAB File Offset: 0x000981AB
	// (set) Token: 0x06001E8E RID: 7822 RVA: 0x00099FB2 File Offset: 0x000981B2
	public static bool hasInstance { get; private set; }

	// Token: 0x06001E8F RID: 7823 RVA: 0x00099FBC File Offset: 0x000981BC
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

	// Token: 0x06001E90 RID: 7824 RVA: 0x0009A040 File Offset: 0x00098240
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

	// Token: 0x06001E91 RID: 7825 RVA: 0x0009A0C4 File Offset: 0x000982C4
	public void Awake()
	{
		if (BuilderSetManager.instance == null)
		{
			BuilderSetManager.instance = this;
			BuilderSetManager.hasInstance = true;
		}
		else if (BuilderSetManager.instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		this.Init();
		if (this.monitor == null)
		{
			this.monitor = base.StartCoroutine(this.MonitorTime());
		}
	}

	// Token: 0x06001E92 RID: 7826 RVA: 0x0009A12C File Offset: 0x0009832C
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

	// Token: 0x06001E93 RID: 7827 RVA: 0x0009A484 File Offset: 0x00098684
	private void OnEnable()
	{
		if (this.monitor == null && this.scheduledPieceSets.Count > 0)
		{
			this.monitor = base.StartCoroutine(this.MonitorTime());
		}
	}

	// Token: 0x06001E94 RID: 7828 RVA: 0x0009A4AE File Offset: 0x000986AE
	private void OnDisable()
	{
		if (this.monitor != null)
		{
			base.StopCoroutine(this.monitor);
		}
		this.monitor = null;
	}

	// Token: 0x06001E95 RID: 7829 RVA: 0x0009A4CB File Offset: 0x000986CB
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

	// Token: 0x06001E96 RID: 7830 RVA: 0x0009A4DC File Offset: 0x000986DC
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

	// Token: 0x06001E97 RID: 7831 RVA: 0x0009A584 File Offset: 0x00098784
	public static bool IsItemIDBuilderItem(string playfabID)
	{
		return BuilderSetManager.instance.GetAllSetsConcat().Contains(playfabID);
	}

	// Token: 0x06001E98 RID: 7832 RVA: 0x0009A598 File Offset: 0x00098798
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

	// Token: 0x06001E99 RID: 7833 RVA: 0x0009A77C File Offset: 0x0009897C
	public BuilderSetManager.BuilderSetStoreItem GetStoreItemFromSetID(int setID)
	{
		return BuilderSetManager._setIdToStoreItem.GetValueOrDefault(setID, BuilderKiosk.nullItem);
	}

	// Token: 0x06001E9A RID: 7834 RVA: 0x0009A790 File Offset: 0x00098990
	public BuilderPieceSet GetPieceSetFromID(int setID)
	{
		BuilderSetManager.BuilderSetStoreItem builderSetStoreItem;
		if (BuilderSetManager._setIdToStoreItem.TryGetValue(setID, out builderSetStoreItem))
		{
			return builderSetStoreItem.setRef;
		}
		return null;
	}

	// Token: 0x06001E9B RID: 7835 RVA: 0x0009A7B4 File Offset: 0x000989B4
	public List<BuilderPieceSet> GetAllPieceSets()
	{
		return this._allPieceSets;
	}

	// Token: 0x06001E9C RID: 7836 RVA: 0x0009A7BC File Offset: 0x000989BC
	public List<BuilderPieceSet> GetLivePieceSets()
	{
		return this.livePieceSets;
	}

	// Token: 0x06001E9D RID: 7837 RVA: 0x0009A7C4 File Offset: 0x000989C4
	public List<BuilderPieceSet> GetUnlockedPieceSets()
	{
		return this._unlockedPieceSets;
	}

	// Token: 0x06001E9E RID: 7838 RVA: 0x0009A7CC File Offset: 0x000989CC
	public List<BuilderPieceSet> GetPermanentSetsForSale()
	{
		return this._setsAlwaysForSale;
	}

	// Token: 0x06001E9F RID: 7839 RVA: 0x0009A7D4 File Offset: 0x000989D4
	public List<BuilderPieceSet> GetSeasonalSetsForSale()
	{
		return this._seasonalSetsForSale;
	}

	// Token: 0x06001EA0 RID: 7840 RVA: 0x0009A7DC File Offset: 0x000989DC
	public bool IsSetSeasonal(string playfabID)
	{
		return !this._seasonalSetsForSale.IsNullOrEmpty<BuilderPieceSet>() && this._seasonalSetsForSale.FindIndex((BuilderPieceSet x) => x.playfabID.Equals(playfabID)) >= 0;
	}

	// Token: 0x06001EA1 RID: 7841 RVA: 0x0009A824 File Offset: 0x00098A24
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

	// Token: 0x06001EA2 RID: 7842 RVA: 0x0009A8B8 File Offset: 0x00098AB8
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

	// Token: 0x06001EA3 RID: 7843 RVA: 0x0009A958 File Offset: 0x00098B58
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

	// Token: 0x06001EA4 RID: 7844 RVA: 0x0009A9D8 File Offset: 0x00098BD8
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

	// Token: 0x06001EA5 RID: 7845 RVA: 0x0009AA58 File Offset: 0x00098C58
	public bool IsPieceSetOwnedLocally(int setID)
	{
		return this._unlockedPieceSets.FindIndex((BuilderPieceSet x) => setID == x.GetIntIdentifier()) >= 0;
	}

	// Token: 0x06001EA6 RID: 7846 RVA: 0x0009AA90 File Offset: 0x00098C90
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

	// Token: 0x06001EA7 RID: 7847 RVA: 0x0009AB28 File Offset: 0x00098D28
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

	// Token: 0x06001EA8 RID: 7848 RVA: 0x0009AC2C File Offset: 0x00098E2C
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
						Object.DestroyImmediate(PhotonNetworkController.Instance);
						Object.DestroyImmediate(GTPlayer.Instance);
						GameObject[] array = Object.FindObjectsOfType<GameObject>();
						for (int i = 0; i < array.Length; i++)
						{
							Object.Destroy(array[i]);
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

	// Token: 0x04002238 RID: 8760
	[SerializeField]
	private List<BuilderPieceSet> _allPieceSets;

	// Token: 0x04002239 RID: 8761
	[SerializeField]
	private List<BuilderPieceSet> _starterPieceSets;

	// Token: 0x0400223A RID: 8762
	[SerializeField]
	private List<BuilderPieceSet> _setsAlwaysForSale;

	// Token: 0x0400223B RID: 8763
	[SerializeField]
	private List<BuilderPieceSet> _seasonalSetsForSale;

	// Token: 0x0400223C RID: 8764
	private List<BuilderPieceSet> livePieceSets;

	// Token: 0x0400223D RID: 8765
	private List<BuilderPieceSet> scheduledPieceSets;

	// Token: 0x0400223E RID: 8766
	private Coroutine monitor;

	// Token: 0x0400223F RID: 8767
	private List<BuilderSetManager.BuilderSetStoreItem> _allStoreItems;

	// Token: 0x04002240 RID: 8768
	private List<BuilderPieceSet> _unlockedPieceSets;

	// Token: 0x04002241 RID: 8769
	private static Dictionary<int, BuilderSetManager.BuilderSetStoreItem> _setIdToStoreItem;

	// Token: 0x04002242 RID: 8770
	private static List<BuilderSetManager.BuilderPieceSetInfo> pieceSetInfos;

	// Token: 0x04002243 RID: 8771
	private static Dictionary<int, int> pieceSetInfoMap;

	// Token: 0x04002244 RID: 8772
	[OnEnterPlay_SetNull]
	public static volatile BuilderSetManager instance;

	// Token: 0x04002246 RID: 8774
	[HideInInspector]
	public string catalog;

	// Token: 0x04002247 RID: 8775
	[HideInInspector]
	public string currencyName;

	// Token: 0x04002248 RID: 8776
	private string[] tempStringArray;

	// Token: 0x04002249 RID: 8777
	[HideInInspector]
	public UnityEvent OnLiveSetsUpdated;

	// Token: 0x0400224A RID: 8778
	[HideInInspector]
	public UnityEvent OnOwnedSetsUpdated;

	// Token: 0x0400224B RID: 8779
	[HideInInspector]
	public bool pulledStoreItems;

	// Token: 0x0400224C RID: 8780
	private static string concatStarterSets = string.Empty;

	// Token: 0x0400224D RID: 8781
	private static string concatAllSets = string.Empty;

	// Token: 0x0400224E RID: 8782
	private bool foundCosmetic;

	// Token: 0x0400224F RID: 8783
	private int attempts;

	// Token: 0x04002250 RID: 8784
	private List<string> playerIDList = new List<string>();

	// Token: 0x020004EC RID: 1260
	[Serializable]
	public struct BuilderSetStoreItem
	{
		// Token: 0x04002251 RID: 8785
		public string displayName;

		// Token: 0x04002252 RID: 8786
		public string playfabID;

		// Token: 0x04002253 RID: 8787
		public int setID;

		// Token: 0x04002254 RID: 8788
		public uint cost;

		// Token: 0x04002255 RID: 8789
		public bool hasPrice;

		// Token: 0x04002256 RID: 8790
		public BuilderPieceSet setRef;

		// Token: 0x04002257 RID: 8791
		public GameObject displayModel;

		// Token: 0x04002258 RID: 8792
		[NonSerialized]
		public bool isNullItem;
	}

	// Token: 0x020004ED RID: 1261
	[Serializable]
	public struct BuilderPieceSetInfo
	{
		// Token: 0x04002259 RID: 8793
		public int pieceType;

		// Token: 0x0400225A RID: 8794
		public int materialType;

		// Token: 0x0400225B RID: 8795
		public List<int> setIds;
	}
}
