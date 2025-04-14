using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FXP;
using PlayFab;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B0D RID: 2829
	public class StoreUpdater : MonoBehaviour
	{
		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x060046B2 RID: 18098 RVA: 0x0014F867 File Offset: 0x0014DA67
		public DateTime DateTimeNowServerAdjusted
		{
			get
			{
				return GorillaComputer.instance.GetServerTime();
			}
		}

		// Token: 0x060046B3 RID: 18099 RVA: 0x0014F875 File Offset: 0x0014DA75
		public void Awake()
		{
			if (StoreUpdater.instance == null)
			{
				StoreUpdater.instance = this;
				return;
			}
			if (StoreUpdater.instance != this)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x060046B4 RID: 18100 RVA: 0x0014F8A9 File Offset: 0x0014DAA9
		private void OnApplicationFocus(bool hasFocus)
		{
			if (hasFocus)
			{
				this.HandleHMDMounted();
				return;
			}
			this.HandleHMDUnmounted();
		}

		// Token: 0x060046B5 RID: 18101 RVA: 0x0014F8BC File Offset: 0x0014DABC
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

		// Token: 0x060046B6 RID: 18102 RVA: 0x0014F932 File Offset: 0x0014DB32
		private void ServerTimeUpdater()
		{
			base.StartCoroutine(this.InitializeTitleData());
		}

		// Token: 0x060046B7 RID: 18103 RVA: 0x0014F944 File Offset: 0x0014DB44
		public void OnDestroy()
		{
			OVRManager.HMDMounted -= this.HandleHMDMounted;
			OVRManager.HMDUnmounted -= this.HandleHMDUnmounted;
			OVRManager.HMDLost -= this.HandleHMDUnmounted;
			OVRManager.HMDAcquired -= this.HandleHMDMounted;
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x0014F998 File Offset: 0x0014DB98
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

		// Token: 0x060046B9 RID: 18105 RVA: 0x0014FA70 File Offset: 0x0014DC70
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

		// Token: 0x060046BA RID: 18106 RVA: 0x0014FB18 File Offset: 0x0014DD18
		private void FindAllCosmeticItemPrefabs()
		{
			foreach (CosmeticItemPrefab cosmeticItemPrefab in Object.FindObjectsOfType<CosmeticItemPrefab>())
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

		// Token: 0x060046BB RID: 18107 RVA: 0x0014FB8E File Offset: 0x0014DD8E
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

		// Token: 0x060046BC RID: 18108 RVA: 0x0014FBAB File Offset: 0x0014DDAB
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

		// Token: 0x060046BD RID: 18109 RVA: 0x0014FBC4 File Offset: 0x0014DDC4
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

		// Token: 0x060046BE RID: 18110 RVA: 0x0014FC9C File Offset: 0x0014DE9C
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

		// Token: 0x060046BF RID: 18111 RVA: 0x0014FCF0 File Offset: 0x0014DEF0
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

		// Token: 0x060046C0 RID: 18112 RVA: 0x0014FD38 File Offset: 0x0014DF38
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

		// Token: 0x060046C1 RID: 18113 RVA: 0x0014FEC5 File Offset: 0x0014E0C5
		private IEnumerator InitializeTitleData()
		{
			yield return new WaitForSeconds(1f);
			PlayFabTitleDataCache.Instance.UpdateData();
			yield return new WaitForSeconds(1f);
			this.GetEventsFromTitleData();
			yield break;
		}

		// Token: 0x060046C2 RID: 18114 RVA: 0x0014FED4 File Offset: 0x0014E0D4
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

		// Token: 0x060046C3 RID: 18115 RVA: 0x0014FF68 File Offset: 0x0014E168
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

		// Token: 0x060046C4 RID: 18116 RVA: 0x0015012C File Offset: 0x0014E32C
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

		// Token: 0x060046C5 RID: 18117 RVA: 0x001501F8 File Offset: 0x0014E3F8
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

		// Token: 0x060046C6 RID: 18118 RVA: 0x001502D4 File Offset: 0x0014E4D4
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

		// Token: 0x060046C7 RID: 18119 RVA: 0x001503A7 File Offset: 0x0014E5A7
		public void PedestalAsleep(CosmeticItemPrefab pedestal)
		{
			if (this.pedestalUpdateCoroutines.ContainsKey(pedestal.PedestalID) && this.pedestalUpdateCoroutines[pedestal.PedestalID] != null)
			{
				base.StopCoroutine(this.pedestalUpdateCoroutines[pedestal.PedestalID]);
			}
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x001503E8 File Offset: 0x0014E5E8
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

		// Token: 0x0400483E RID: 18494
		public static volatile StoreUpdater instance;

		// Token: 0x0400483F RID: 18495
		private DateTime StoreItemsChangeTimeUTC;

		// Token: 0x04004840 RID: 18496
		private Dictionary<string, CosmeticItemPrefab> cosmeticItemPrefabsDictionary = new Dictionary<string, CosmeticItemPrefab>();

		// Token: 0x04004841 RID: 18497
		private Dictionary<string, List<StoreUpdateEvent>> pedestalUpdateEvents = new Dictionary<string, List<StoreUpdateEvent>>();

		// Token: 0x04004842 RID: 18498
		private Dictionary<string, Coroutine> pedestalUpdateCoroutines = new Dictionary<string, Coroutine>();

		// Token: 0x04004843 RID: 18499
		private Dictionary<string, Coroutine> pedestalClearCartCoroutines = new Dictionary<string, Coroutine>();

		// Token: 0x04004844 RID: 18500
		private string tempJson;

		// Token: 0x04004845 RID: 18501
		private bool bLoadFromJSON = true;

		// Token: 0x04004846 RID: 18502
		private bool bUsePlaceHolderJSON;
	}
}
