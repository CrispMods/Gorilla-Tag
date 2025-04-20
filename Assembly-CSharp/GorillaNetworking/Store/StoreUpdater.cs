using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FXP;
using PlayFab;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B3A RID: 2874
	public class StoreUpdater : MonoBehaviour
	{
		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x060047FB RID: 18427 RVA: 0x0005EE1B File Offset: 0x0005D01B
		public DateTime DateTimeNowServerAdjusted
		{
			get
			{
				return GorillaComputer.instance.GetServerTime();
			}
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x0005EE29 File Offset: 0x0005D029
		public void Awake()
		{
			if (StoreUpdater.instance == null)
			{
				StoreUpdater.instance = this;
				return;
			}
			if (StoreUpdater.instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x0005EE5D File Offset: 0x0005D05D
		private void OnApplicationFocus(bool hasFocus)
		{
			if (hasFocus)
			{
				this.HandleHMDMounted();
				return;
			}
			this.HandleHMDUnmounted();
		}

		// Token: 0x060047FE RID: 18430 RVA: 0x0018C24C File Offset: 0x0018A44C
		public void Initialize()
		{
			this.FindAllCosmeticItemPrefabs();
			OVRManager.HMDMounted += this.HandleHMDMounted;
			OVRManager.HMDUnmounted += this.HandleHMDUnmounted;
			OVRManager.HMDLost += this.HandleHMDUnmounted;
			OVRManager.HMDAcquired += this.HandleHMDMounted;
			Debug.Log("StoreUpdater - Starting");
			if (this.bLoadFromJSON)
			{
				base.StartCoroutine(this.InitializeTitleData());
			}
		}

		// Token: 0x060047FF RID: 18431 RVA: 0x0005EE6F File Offset: 0x0005D06F
		private void ServerTimeUpdater()
		{
			base.StartCoroutine(this.InitializeTitleData());
		}

		// Token: 0x06004800 RID: 18432 RVA: 0x0018C2C4 File Offset: 0x0018A4C4
		public void OnDestroy()
		{
			OVRManager.HMDMounted -= this.HandleHMDMounted;
			OVRManager.HMDUnmounted -= this.HandleHMDUnmounted;
			OVRManager.HMDLost -= this.HandleHMDUnmounted;
			OVRManager.HMDAcquired -= this.HandleHMDMounted;
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x0018C318 File Offset: 0x0018A518
		private void HandleHMDUnmounted()
		{
			foreach (string key in this.pedestalUpdateCoroutines.Keys)
			{
				if (this.pedestalUpdateCoroutines[key] != null)
				{
					base.StopCoroutine(this.pedestalUpdateCoroutines[key]);
				}
			}
			foreach (string key2 in this.cosmeticItemPrefabsDictionary.Keys)
			{
				if (this.cosmeticItemPrefabsDictionary[key2] != null)
				{
					this.cosmeticItemPrefabsDictionary[key2].StopCountdownCoroutine();
				}
			}
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x0018C3F0 File Offset: 0x0018A5F0
		private void HandleHMDMounted()
		{
			foreach (string text in this.cosmeticItemPrefabsDictionary.Keys)
			{
				if (this.cosmeticItemPrefabsDictionary[text] != null && this.pedestalUpdateEvents.ContainsKey(text) && this.cosmeticItemPrefabsDictionary[text].gameObject.activeInHierarchy)
				{
					this.CheckEventsOnResume(this.pedestalUpdateEvents[text]);
					this.StartNextEvent(text, false);
				}
			}
		}

		// Token: 0x06004803 RID: 18435 RVA: 0x0018C498 File Offset: 0x0018A698
		private void FindAllCosmeticItemPrefabs()
		{
			foreach (CosmeticItemPrefab cosmeticItemPrefab in UnityEngine.Object.FindObjectsOfType<CosmeticItemPrefab>())
			{
				if (this.cosmeticItemPrefabsDictionary.ContainsKey(cosmeticItemPrefab.PedestalID))
				{
					Debug.LogWarning("StoreUpdater - Duplicate Pedestal ID " + cosmeticItemPrefab.PedestalID);
				}
				else
				{
					Debug.Log("StoreUpdater - Adding Pedestal " + cosmeticItemPrefab.PedestalID);
					this.cosmeticItemPrefabsDictionary.Add(cosmeticItemPrefab.PedestalID, cosmeticItemPrefab);
				}
			}
		}

		// Token: 0x06004804 RID: 18436 RVA: 0x0005EE7E File Offset: 0x0005D07E
		private IEnumerator HandlePedestalUpdate(StoreUpdateEvent updateEvent, bool playFX)
		{
			this.cosmeticItemPrefabsDictionary[updateEvent.PedestalID].SetStoreUpdateEvent(updateEvent, playFX);
			yield return new WaitForSeconds((float)(updateEvent.EndTimeUTC.ToUniversalTime() - this.DateTimeNowServerAdjusted).TotalSeconds);
			if (this.pedestalClearCartCoroutines.ContainsKey(updateEvent.PedestalID))
			{
				if (this.pedestalClearCartCoroutines[updateEvent.PedestalID] != null)
				{
					base.StopCoroutine(this.pedestalClearCartCoroutines[updateEvent.PedestalID]);
				}
				this.pedestalClearCartCoroutines[updateEvent.PedestalID] = base.StartCoroutine(this.HandleClearCart(updateEvent));
			}
			else
			{
				this.pedestalClearCartCoroutines.Add(updateEvent.PedestalID, base.StartCoroutine(this.HandleClearCart(updateEvent)));
			}
			if (this.cosmeticItemPrefabsDictionary[updateEvent.PedestalID].gameObject.activeInHierarchy)
			{
				this.pedestalUpdateEvents[updateEvent.PedestalID].RemoveAt(0);
				this.StartNextEvent(updateEvent.PedestalID, true);
			}
			yield break;
		}

		// Token: 0x06004805 RID: 18437 RVA: 0x0005EE9B File Offset: 0x0005D09B
		private IEnumerator HandleClearCart(StoreUpdateEvent updateEvent)
		{
			float seconds = Math.Clamp((float)(updateEvent.EndTimeUTC.ToUniversalTime() - this.DateTimeNowServerAdjusted).TotalSeconds + 60f, 0f, 60f);
			yield return new WaitForSeconds(seconds);
			if (CosmeticsController.instance.RemoveItemFromCart(CosmeticsController.instance.GetItemFromDict(updateEvent.ItemName)))
			{
				CosmeticsController.instance.ClearCheckout(true);
				CosmeticsController.instance.UpdateShoppingCart();
				CosmeticsController.instance.UpdateWornCosmetics(true);
			}
			yield break;
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x0018C510 File Offset: 0x0018A710
		private void StartNextEvent(string pedestalID, bool playFX)
		{
			if (this.pedestalUpdateEvents[pedestalID].Count > 0)
			{
				Coroutine value = base.StartCoroutine(this.HandlePedestalUpdate(this.pedestalUpdateEvents[pedestalID].First<StoreUpdateEvent>(), playFX));
				if (this.pedestalUpdateCoroutines.ContainsKey(pedestalID))
				{
					if (this.pedestalUpdateCoroutines[pedestalID] != null && this.pedestalUpdateCoroutines[pedestalID] != null)
					{
						base.StopCoroutine(this.pedestalUpdateCoroutines[pedestalID]);
					}
					this.pedestalUpdateCoroutines[pedestalID] = value;
				}
				else
				{
					this.pedestalUpdateCoroutines.Add(pedestalID, value);
				}
				if (this.pedestalUpdateEvents[pedestalID].Count == 0 && !this.bLoadFromJSON)
				{
					this.GetStoreUpdateEventsPlaceHolder(pedestalID);
					return;
				}
			}
			else if (!this.bLoadFromJSON)
			{
				this.GetStoreUpdateEventsPlaceHolder(pedestalID);
				this.StartNextEvent(pedestalID, true);
			}
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x0018C5E8 File Offset: 0x0018A7E8
		private void GetStoreUpdateEventsPlaceHolder(string PedestalID)
		{
			List<StoreUpdateEvent> list = new List<StoreUpdateEvent>();
			list = this.CreateTempEvents(PedestalID, 1, 15);
			this.CheckEvents(list);
			if (this.pedestalUpdateEvents.ContainsKey(PedestalID))
			{
				this.pedestalUpdateEvents[PedestalID].AddRange(list);
				return;
			}
			this.pedestalUpdateEvents.Add(PedestalID, list);
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x0018C63C File Offset: 0x0018A83C
		private void CheckEvents(List<StoreUpdateEvent> updateEvents)
		{
			for (int i = 0; i < updateEvents.Count; i++)
			{
				if (updateEvents[i].EndTimeUTC.ToUniversalTime() < this.DateTimeNowServerAdjusted)
				{
					updateEvents.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x0018C684 File Offset: 0x0018A884
		private void CheckEventsOnResume(List<StoreUpdateEvent> updateEvents)
		{
			bool flag = false;
			for (int i = 0; i < updateEvents.Count; i++)
			{
				if (updateEvents[i].EndTimeUTC.ToUniversalTime() < this.DateTimeNowServerAdjusted)
				{
					if (Math.Clamp((float)(updateEvents[i].EndTimeUTC.ToUniversalTime() - this.DateTimeNowServerAdjusted).TotalSeconds + 60f, 0f, 60f) <= 0f)
					{
						flag ^= CosmeticsController.instance.RemoveItemFromCart(CosmeticsController.instance.GetItemFromDict(updateEvents[i].ItemName));
					}
					else if (this.pedestalClearCartCoroutines.ContainsKey(updateEvents[i].PedestalID))
					{
						if (this.pedestalClearCartCoroutines[updateEvents[i].PedestalID] != null)
						{
							base.StopCoroutine(this.pedestalClearCartCoroutines[updateEvents[i].PedestalID]);
						}
						this.pedestalClearCartCoroutines[updateEvents[i].PedestalID] = base.StartCoroutine(this.HandleClearCart(updateEvents[i]));
					}
					else
					{
						this.pedestalClearCartCoroutines.Add(updateEvents[i].PedestalID, base.StartCoroutine(this.HandleClearCart(updateEvents[i])));
					}
					updateEvents.RemoveAt(i);
					i--;
				}
			}
			if (flag)
			{
				CosmeticsController.instance.ClearCheckout(true);
				CosmeticsController.instance.UpdateShoppingCart();
				CosmeticsController.instance.UpdateWornCosmetics(true);
			}
		}

		// Token: 0x0600480A RID: 18442 RVA: 0x0005EEB1 File Offset: 0x0005D0B1
		private IEnumerator InitializeTitleData()
		{
			yield return new WaitForSeconds(1f);
			PlayFabTitleDataCache.Instance.UpdateData();
			yield return new WaitForSeconds(1f);
			this.GetEventsFromTitleData();
			yield break;
		}

		// Token: 0x0600480B RID: 18443 RVA: 0x0018C814 File Offset: 0x0018AA14
		private void GetEventsFromTitleData()
		{
			Debug.Log("StoreUpdater - GetEventsFromTitleData");
			if (this.bUsePlaceHolderJSON)
			{
				DateTime startTime = new DateTime(2024, 2, 13, 16, 0, 0, DateTimeKind.Utc);
				List<StoreUpdateEvent> updateEvents = StoreUpdateEvent.DeserializeFromJSonList(StoreUpdateEvent.SerializeArrayAsJSon(this.CreateTempEvents("Pedestal1", 2, 120, startTime).ToArray()));
				this.HandleRecievingEventsFromTitleData(updateEvents);
				return;
			}
			PlayFabTitleDataCache.Instance.GetTitleData("TOTD", delegate(string result)
			{
				Debug.Log("StoreUpdater - Recieved TitleData : " + result);
				List<StoreUpdateEvent> updateEvents2 = StoreUpdateEvent.DeserializeFromJSonList(result);
				this.HandleRecievingEventsFromTitleData(updateEvents2);
			}, delegate(PlayFabError error)
			{
				Debug.Log("StoreUpdater - Error Title Data : " + error.ErrorMessage);
			});
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x0018C8A8 File Offset: 0x0018AAA8
		private void HandleRecievingEventsFromTitleData(List<StoreUpdateEvent> updateEvents)
		{
			Debug.Log("StoreUpdater - HandleRecievingEventsFromTitleData");
			this.CheckEvents(updateEvents);
			if (CosmeticsController.instance.GetItemFromDict("LBAEY.").isNullItem)
			{
				Debug.LogWarning("StoreUpdater - CosmeticsController is not initialized.  Reinitializing TitleData");
				base.StartCoroutine(this.InitializeTitleData());
				return;
			}
			foreach (StoreUpdateEvent storeUpdateEvent in updateEvents)
			{
				if (this.pedestalUpdateEvents.ContainsKey(storeUpdateEvent.PedestalID))
				{
					this.pedestalUpdateEvents[storeUpdateEvent.PedestalID].Add(storeUpdateEvent);
				}
				else
				{
					this.pedestalUpdateEvents.Add(storeUpdateEvent.PedestalID, new List<StoreUpdateEvent>());
					this.pedestalUpdateEvents[storeUpdateEvent.PedestalID].Add(storeUpdateEvent);
				}
			}
			Debug.Log("StoreUpdater - Starting Events");
			foreach (string text in this.pedestalUpdateEvents.Keys)
			{
				if (this.cosmeticItemPrefabsDictionary.ContainsKey(text))
				{
					Debug.Log("StoreUpdater - Starting Event " + text);
					this.StartNextEvent(text, false);
				}
			}
			foreach (string text2 in this.cosmeticItemPrefabsDictionary.Keys)
			{
				if (!this.pedestalUpdateEvents.ContainsKey(text2))
				{
					Debug.Log("StoreUpdater - Adding PlaceHolder Events " + text2);
					this.GetStoreUpdateEventsPlaceHolder(text2);
					this.StartNextEvent(text2, false);
				}
			}
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x0018CA6C File Offset: 0x0018AC6C
		private void PrintJSONEvents()
		{
			string text = StoreUpdateEvent.SerializeArrayAsJSon(this.CreateTempEvents("Pedestal1", 5, 28).ToArray());
			foreach (StoreUpdateEvent storeUpdateEvent in StoreUpdateEvent.DeserializeFromJSonList(text))
			{
				Debug.Log(string.Concat(new string[]
				{
					"Event : ",
					storeUpdateEvent.ItemName,
					" : ",
					storeUpdateEvent.StartTimeUTC.ToString(),
					" : ",
					storeUpdateEvent.EndTimeUTC.ToString()
				}));
			}
			Debug.Log("NewEvents :\n" + text);
			this.tempJson = text;
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x0018CB38 File Offset: 0x0018AD38
		private List<StoreUpdateEvent> CreateTempEvents(string PedestalID, int minuteDelay, int totalEvents)
		{
			string[] array = new string[]
			{
				"LBAEY.",
				"LBAEZ.",
				"LBAFA.",
				"LBAFB.",
				"LBAFC.",
				"LBAFD.",
				"LBAFE.",
				"LBAFF.",
				"LBAFG.",
				"LBAFH.",
				"LBAFO.",
				"LBAFP.",
				"LBAFQ.",
				"LBAFR."
			};
			List<StoreUpdateEvent> list = new List<StoreUpdateEvent>();
			for (int i = 0; i < totalEvents; i++)
			{
				StoreUpdateEvent item = new StoreUpdateEvent(PedestalID, array[i % 14], DateTime.UtcNow + TimeSpan.FromMinutes((double)(minuteDelay * i)), DateTime.UtcNow + TimeSpan.FromMinutes((double)(minuteDelay * (i + 1))));
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x0018CC14 File Offset: 0x0018AE14
		private List<StoreUpdateEvent> CreateTempEvents(string PedestalID, int minuteDelay, int totalEvents, DateTime startTime)
		{
			string[] array = new string[]
			{
				"LBAEY.",
				"LBAEZ.",
				"LBAFA.",
				"LBAFB.",
				"LBAFC.",
				"LBAFD.",
				"LBAFE.",
				"LBAFF.",
				"LBAFG.",
				"LBAFH.",
				"LBAFO.",
				"LBAFP.",
				"LBAFQ.",
				"LBAFR."
			};
			List<StoreUpdateEvent> list = new List<StoreUpdateEvent>();
			for (int i = 0; i < totalEvents; i++)
			{
				StoreUpdateEvent item = new StoreUpdateEvent(PedestalID, array[i % 14], startTime + TimeSpan.FromMinutes((double)(minuteDelay * i)), startTime + TimeSpan.FromMinutes((double)(minuteDelay * (i + 1))));
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06004810 RID: 18448 RVA: 0x0005EEC0 File Offset: 0x0005D0C0
		public void PedestalAsleep(CosmeticItemPrefab pedestal)
		{
			if (this.pedestalUpdateCoroutines.ContainsKey(pedestal.PedestalID) && this.pedestalUpdateCoroutines[pedestal.PedestalID] != null)
			{
				base.StopCoroutine(this.pedestalUpdateCoroutines[pedestal.PedestalID]);
			}
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x0018CCE8 File Offset: 0x0018AEE8
		public void PedestalAwakened(CosmeticItemPrefab pedestal)
		{
			if (!this.cosmeticItemPrefabsDictionary.ContainsKey(pedestal.PedestalID))
			{
				this.cosmeticItemPrefabsDictionary.Add(pedestal.PedestalID, pedestal);
			}
			if (this.pedestalUpdateEvents.ContainsKey(pedestal.PedestalID))
			{
				this.CheckEventsOnResume(this.pedestalUpdateEvents[pedestal.PedestalID]);
				this.StartNextEvent(pedestal.PedestalID, false);
			}
		}

		// Token: 0x04004933 RID: 18739
		public static volatile StoreUpdater instance;

		// Token: 0x04004934 RID: 18740
		private DateTime StoreItemsChangeTimeUTC;

		// Token: 0x04004935 RID: 18741
		private Dictionary<string, CosmeticItemPrefab> cosmeticItemPrefabsDictionary = new Dictionary<string, CosmeticItemPrefab>();

		// Token: 0x04004936 RID: 18742
		private Dictionary<string, List<StoreUpdateEvent>> pedestalUpdateEvents = new Dictionary<string, List<StoreUpdateEvent>>();

		// Token: 0x04004937 RID: 18743
		private Dictionary<string, Coroutine> pedestalUpdateCoroutines = new Dictionary<string, Coroutine>();

		// Token: 0x04004938 RID: 18744
		private Dictionary<string, Coroutine> pedestalClearCartCoroutines = new Dictionary<string, Coroutine>();

		// Token: 0x04004939 RID: 18745
		private string tempJson;

		// Token: 0x0400493A RID: 18746
		private bool bLoadFromJSON = true;

		// Token: 0x0400493B RID: 18747
		private bool bUsePlaceHolderJSON;
	}
}
