using System;
using System.Buffers;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GorillaNetworking;
using KID.Model;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EventsModels;
using UnityEngine;

// Token: 0x020005AA RID: 1450
public static class GorillaTelemetry
{
	// Token: 0x06002418 RID: 9240 RVA: 0x00101384 File Offset: 0x000FF584
	static GorillaTelemetry()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["User"] = null;
		dictionary["EventType"] = null;
		dictionary["ZoneId"] = null;
		dictionary["SubZoneId"] = null;
		GorillaTelemetry.gZoneEventArgs = dictionary;
		GorillaTelemetry.CurrentZone = GTZone.none;
		GorillaTelemetry.CurrentSubZone = GTSubZone.none;
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		dictionary2["User"] = null;
		dictionary2["EventType"] = null;
		dictionary2["Items"] = null;
		GorillaTelemetry.gShopEventArgs = dictionary2;
		GorillaTelemetry.gSingleItemParam = new CosmeticsController.CosmeticItem[1];
		GorillaTelemetry.gSingleItemBuilderParam = new BuilderSetManager.BuilderSetStoreItem[1];
		Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
		dictionary3["User"] = null;
		dictionary3["EventType"] = null;
		dictionary3["AgeCategory"] = null;
		dictionary3["VoiceChatEnabled"] = null;
		dictionary3["CustomUsernameEnabled"] = null;
		dictionary3["JoinGroups"] = null;
		GorillaTelemetry.gKidEventArgs = dictionary3;
		Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
		dictionary4["User"] = null;
		dictionary4["WamGameId"] = null;
		dictionary4["WamMachineId"] = null;
		GorillaTelemetry.gWamGameStartArgs = dictionary4;
		Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
		dictionary5["User"] = null;
		dictionary5["WamGameId"] = null;
		dictionary5["WamMachineId"] = null;
		dictionary5["WamMLevelNumber"] = null;
		dictionary5["WamGoodMolesShown"] = null;
		dictionary5["WamHazardMolesShown"] = null;
		dictionary5["WamLevelMinScore"] = null;
		dictionary5["WamLevelScore"] = null;
		dictionary5["WamHazardMolesHit"] = null;
		dictionary5["WamGameState"] = null;
		GorillaTelemetry.gWamLevelEndArgs = dictionary5;
		Dictionary<string, object> dictionary6 = new Dictionary<string, object>();
		dictionary6["CustomMapName"] = null;
		dictionary6["CustomMapModId"] = null;
		dictionary6["LowestFPS"] = null;
		dictionary6["LowestFPSDrawCalls"] = null;
		dictionary6["LowestFPSPlayerCount"] = null;
		dictionary6["AverageFPS"] = null;
		dictionary6["AverageDrawCalls"] = null;
		dictionary6["AveragePlayerCount"] = null;
		dictionary6["HighestFPS"] = null;
		dictionary6["HighestFPSDrawCalls"] = null;
		dictionary6["HighestFPSPlayerCount"] = null;
		dictionary6["PlaytimeInSeconds"] = null;
		GorillaTelemetry.gCustomMapPerfArgs = dictionary6;
		Dictionary<string, object> dictionary7 = new Dictionary<string, object>();
		dictionary7["User"] = null;
		dictionary7["CustomMapName"] = null;
		dictionary7["CustomMapModId"] = null;
		dictionary7["CustomMapCreator"] = null;
		dictionary7["MinPlayerCount"] = null;
		dictionary7["MaxPlayerCount"] = null;
		dictionary7["PlaytimeOnMap"] = null;
		dictionary7["PrivateRoom"] = null;
		GorillaTelemetry.gCustomMapTrackingMetrics = dictionary7;
		Dictionary<string, object> dictionary8 = new Dictionary<string, object>();
		dictionary8["User"] = null;
		dictionary8["CustomMapName"] = null;
		dictionary8["CustomMapModId"] = null;
		dictionary8["CustomMapCreator"] = null;
		GorillaTelemetry.gCustomMapDownloadMetrics = dictionary8;
		GameObject gameObject = new GameObject("GorillaTelemetryBatcher");
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		gameObject.AddComponent<GorillaTelemetry.BatchRunner>();
	}

	// Token: 0x06002419 RID: 9241 RVA: 0x00048718 File Offset: 0x00046918
	private static void QueueTelemetryEvent(EventContents eventContent)
	{
		GorillaTelemetry.telemetryEventsQueue.Enqueue(eventContent);
	}

	// Token: 0x0600241A RID: 9242 RVA: 0x001016B4 File Offset: 0x000FF8B4
	private static void FlushTelemetry()
	{
		int count = GorillaTelemetry.telemetryEventsQueue.Count;
		if (count == 0)
		{
			return;
		}
		EventContents[] array = ArrayPool<EventContents>.Shared.Rent(count);
		try
		{
			int i;
			for (i = 0; i < count; i++)
			{
				EventContents eventContents;
				array[i] = (GorillaTelemetry.telemetryEventsQueue.TryDequeue(out eventContents) ? eventContents : null);
			}
			if (i == 0)
			{
				ArrayPool<EventContents>.Shared.Return(array, false);
			}
			else
			{
				WriteEventsRequest writeEventsRequest = new WriteEventsRequest();
				writeEventsRequest.Events = GorillaTelemetry.GetEventListForArray(array, i);
				PlayFabEventsAPI.WriteTelemetryEvents(writeEventsRequest, delegate(WriteEventsResponse result)
				{
				}, delegate(PlayFabError error)
				{
				}, null, null);
			}
		}
		finally
		{
			ArrayPool<EventContents>.Shared.Return(array, false);
		}
	}

	// Token: 0x0600241B RID: 9243 RVA: 0x00101784 File Offset: 0x000FF984
	private static List<EventContents> GetEventListForArray(EventContents[] array, int count)
	{
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			if (array[i] != null)
			{
				num++;
			}
		}
		List<EventContents> list;
		if (!GorillaTelemetry.gListPool.TryGetValue(num, out list))
		{
			list = new List<EventContents>(num);
			GorillaTelemetry.gListPool.TryAdd(num, list);
		}
		else
		{
			list.Clear();
		}
		for (int j = 0; j < count; j++)
		{
			if (array[j] != null)
			{
				list.Add(array[j]);
			}
		}
		return list;
	}

	// Token: 0x0600241C RID: 9244 RVA: 0x00048725 File Offset: 0x00046925
	private static bool IsConnected()
	{
		if (!NetworkSystem.Instance.InRoom)
		{
			return false;
		}
		if (GorillaTelemetry.gPlayFabAuth == null)
		{
			GorillaTelemetry.gPlayFabAuth = PlayFabAuthenticator.instance;
		}
		return !(GorillaTelemetry.gPlayFabAuth == null);
	}

	// Token: 0x0600241D RID: 9245 RVA: 0x00048758 File Offset: 0x00046958
	private static string PlayFabUserId()
	{
		return GorillaTelemetry.gPlayFabAuth.GetPlayFabPlayerId();
	}

	// Token: 0x0600241E RID: 9246 RVA: 0x001017F0 File Offset: 0x000FF9F0
	public static void PostZoneEvent(GTZone zone, GTSubZone subZone, GTZoneEventType zoneEvent)
	{
		if (!GorillaTelemetry.IsConnected())
		{
			return;
		}
		string value = GorillaTelemetry.PlayFabUserId();
		string name = zoneEvent.GetName<GTZoneEventType>();
		string name2 = zone.GetName<GTZone>();
		string name3 = subZone.GetName<GTSubZone>();
		Dictionary<string, object> dictionary = GorillaTelemetry.gZoneEventArgs;
		dictionary["User"] = value;
		dictionary["EventType"] = name;
		dictionary["ZoneId"] = name2;
		dictionary["SubZoneId"] = name3;
		GorillaTelemetry.QueueTelemetryEvent(new EventContents
		{
			Name = "telemetry_zone_event",
			EventNamespace = GorillaTelemetry.EVENT_NAMESPACE,
			Payload = dictionary
		});
	}

	// Token: 0x0600241F RID: 9247 RVA: 0x00048764 File Offset: 0x00046964
	public static void PostShopEvent(VRRig playerRig, GTShopEventType shopEvent, CosmeticsController.CosmeticItem item)
	{
		GorillaTelemetry.gSingleItemParam[0] = item;
		GorillaTelemetry.PostShopEvent(playerRig, shopEvent, GorillaTelemetry.gSingleItemParam);
		GorillaTelemetry.gSingleItemParam[0] = default(CosmeticsController.CosmeticItem);
	}

	// Token: 0x06002420 RID: 9248 RVA: 0x00101884 File Offset: 0x000FFA84
	private static string[] FetchItemArgs(IList<CosmeticsController.CosmeticItem> items)
	{
		int count = items.Count;
		if (count == 0)
		{
			return Array.Empty<string>();
		}
		HashSet<string> hashSet = new HashSet<string>(count);
		int num = 0;
		for (int i = 0; i < items.Count; i++)
		{
			CosmeticsController.CosmeticItem cosmeticItem = items[i];
			if (!cosmeticItem.isNullItem)
			{
				string itemName = cosmeticItem.itemName;
				if (!string.IsNullOrWhiteSpace(itemName) && !itemName.Contains("NOTHING", StringComparison.InvariantCultureIgnoreCase) && hashSet.Add(itemName))
				{
					num++;
				}
			}
		}
		string[] array = new string[num];
		hashSet.CopyTo(array);
		return array;
	}

	// Token: 0x06002421 RID: 9249 RVA: 0x00101910 File Offset: 0x000FFB10
	public static void PostShopEvent(VRRig playerRig, GTShopEventType shopEvent, IList<CosmeticsController.CosmeticItem> items)
	{
		if (!GorillaTelemetry.IsConnected())
		{
			return;
		}
		if (!playerRig.isLocal)
		{
			return;
		}
		string value = GorillaTelemetry.PlayFabUserId();
		string name = shopEvent.GetName<GTShopEventType>();
		string[] value2 = GorillaTelemetry.FetchItemArgs(items);
		Dictionary<string, object> dictionary = GorillaTelemetry.gShopEventArgs;
		dictionary["User"] = value;
		dictionary["EventType"] = name;
		dictionary["Items"] = value2;
		PlayFabClientAPI.WriteTitleEvent(new WriteTitleEventRequest
		{
			EventName = "telemetry_shop_event",
			Body = dictionary
		}, new Action<WriteEventResponse>(GorillaTelemetry.PostShopEvent_OnResult), new Action<PlayFabError>(GorillaTelemetry.PostShopEvent_OnError), null, null);
	}

	// Token: 0x06002422 RID: 9250 RVA: 0x00030607 File Offset: 0x0002E807
	private static void PostShopEvent_OnResult(WriteEventResponse result)
	{
	}

	// Token: 0x06002423 RID: 9251 RVA: 0x00030607 File Offset: 0x0002E807
	private static void PostShopEvent_OnError(PlayFabError error)
	{
	}

	// Token: 0x06002424 RID: 9252 RVA: 0x0004878F File Offset: 0x0004698F
	public static void PostBuilderKioskEvent(VRRig playerRig, GTShopEventType shopEvent, BuilderSetManager.BuilderSetStoreItem item)
	{
		GorillaTelemetry.gSingleItemBuilderParam[0] = item;
		GorillaTelemetry.PostBuilderKioskEvent(playerRig, shopEvent, GorillaTelemetry.gSingleItemBuilderParam);
		GorillaTelemetry.gSingleItemBuilderParam[0] = default(BuilderSetManager.BuilderSetStoreItem);
	}

	// Token: 0x06002425 RID: 9253 RVA: 0x001019A4 File Offset: 0x000FFBA4
	private static string[] BuilderItemsToStrings(IList<BuilderSetManager.BuilderSetStoreItem> items)
	{
		int count = items.Count;
		if (count == 0)
		{
			return Array.Empty<string>();
		}
		HashSet<string> hashSet = new HashSet<string>(count);
		int num = 0;
		for (int i = 0; i < items.Count; i++)
		{
			BuilderSetManager.BuilderSetStoreItem builderSetStoreItem = items[i];
			if (!builderSetStoreItem.isNullItem)
			{
				string playfabID = builderSetStoreItem.playfabID;
				if (!string.IsNullOrWhiteSpace(playfabID) && !playfabID.Contains("NOTHING", StringComparison.InvariantCultureIgnoreCase) && hashSet.Add(playfabID))
				{
					num++;
				}
			}
		}
		string[] array = new string[num];
		hashSet.CopyTo(array);
		return array;
	}

	// Token: 0x06002426 RID: 9254 RVA: 0x00101A30 File Offset: 0x000FFC30
	public static void PostBuilderKioskEvent(VRRig playerRig, GTShopEventType shopEvent, IList<BuilderSetManager.BuilderSetStoreItem> items)
	{
		if (!GorillaTelemetry.IsConnected())
		{
			return;
		}
		if (!playerRig.isLocal)
		{
			return;
		}
		string value = GorillaTelemetry.PlayFabUserId();
		string name = shopEvent.GetName<GTShopEventType>();
		string[] value2 = GorillaTelemetry.BuilderItemsToStrings(items);
		Dictionary<string, object> dictionary = GorillaTelemetry.gShopEventArgs;
		dictionary["User"] = value;
		dictionary["EventType"] = name;
		dictionary["Items"] = value2;
		PlayFabClientAPI.WriteTitleEvent(new WriteTitleEventRequest
		{
			EventName = "telemetry_shop_event",
			Body = dictionary
		}, new Action<WriteEventResponse>(GorillaTelemetry.PostShopEvent_OnResult), new Action<PlayFabError>(GorillaTelemetry.PostShopEvent_OnError), null, null);
	}

	// Token: 0x06002427 RID: 9255 RVA: 0x00101AC4 File Offset: 0x000FFCC4
	public static void PostKidEvent(bool joinGroupsEnabled, bool voiceChatEnabled, bool customUsernamesEnabled, AgeStatusType ageCategory, GTKidEventType kidEvent)
	{
		if ((double)UnityEngine.Random.value < 0.1)
		{
			return;
		}
		if (!GorillaTelemetry.IsConnected())
		{
			return;
		}
		string value = GorillaTelemetry.PlayFabUserId();
		string name = kidEvent.GetName<GTKidEventType>();
		string value2 = (ageCategory == AgeStatusType.LEGALADULT) ? "Not_Managed_Account" : "Managed_Account";
		string value3 = joinGroupsEnabled.ToString().ToUpper();
		string value4 = voiceChatEnabled.ToString().ToUpper();
		string value5 = customUsernamesEnabled.ToString().ToUpper();
		Dictionary<string, object> dictionary = GorillaTelemetry.gKidEventArgs;
		dictionary["User"] = value;
		dictionary["EventType"] = name;
		dictionary["AgeCategory"] = value2;
		dictionary["VoiceChatEnabled"] = value4;
		dictionary["CustomUsernameEnabled"] = value5;
		dictionary["JoinGroups"] = value3;
		GorillaTelemetry.QueueTelemetryEvent(new EventContents
		{
			Name = "telemetry_kid_event",
			EventNamespace = GorillaTelemetry.EVENT_NAMESPACE,
			Payload = dictionary
		});
	}

	// Token: 0x06002428 RID: 9256 RVA: 0x00101BB4 File Offset: 0x000FFDB4
	public static void WamGameStart(string playerId, string gameId, string machineId)
	{
		if (!GorillaTelemetry.IsConnected())
		{
			return;
		}
		GorillaTelemetry.gWamGameStartArgs["User"] = playerId;
		GorillaTelemetry.gWamGameStartArgs["WamGameId"] = gameId;
		GorillaTelemetry.gWamGameStartArgs["WamMachineId"] = machineId;
		GorillaTelemetry.QueueTelemetryEvent(new EventContents
		{
			Name = "telemetry_wam_gameStartEvent",
			EventNamespace = GorillaTelemetry.EVENT_NAMESPACE,
			Payload = GorillaTelemetry.gWamGameStartArgs
		});
	}

	// Token: 0x06002429 RID: 9257 RVA: 0x00101C24 File Offset: 0x000FFE24
	public static void WamLevelEnd(string playerId, int gameId, string machineId, int currentLevelNumber, int levelGoodMolesShown, int levelHazardMolesShown, int levelMinScore, int currentScore, int levelHazardMolesHit, string currentGameResult)
	{
		if (!GorillaTelemetry.IsConnected())
		{
			return;
		}
		GorillaTelemetry.gWamLevelEndArgs["User"] = playerId;
		GorillaTelemetry.gWamLevelEndArgs["WamGameId"] = gameId.ToString();
		GorillaTelemetry.gWamLevelEndArgs["WamMachineId"] = machineId;
		GorillaTelemetry.gWamLevelEndArgs["WamMLevelNumber"] = currentLevelNumber.ToString();
		GorillaTelemetry.gWamLevelEndArgs["WamGoodMolesShown"] = levelGoodMolesShown.ToString();
		GorillaTelemetry.gWamLevelEndArgs["WamHazardMolesShown"] = levelHazardMolesShown.ToString();
		GorillaTelemetry.gWamLevelEndArgs["WamLevelMinScore"] = levelMinScore.ToString();
		GorillaTelemetry.gWamLevelEndArgs["WamLevelScore"] = currentScore.ToString();
		GorillaTelemetry.gWamLevelEndArgs["WamHazardMolesHit"] = levelHazardMolesHit.ToString();
		GorillaTelemetry.gWamLevelEndArgs["WamGameState"] = currentGameResult;
		GorillaTelemetry.QueueTelemetryEvent(new EventContents
		{
			Name = "telemetry_wam_levelEndEvent",
			EventNamespace = GorillaTelemetry.EVENT_NAMESPACE,
			Payload = GorillaTelemetry.gWamLevelEndArgs
		});
	}

	// Token: 0x0600242A RID: 9258 RVA: 0x00101D30 File Offset: 0x000FFF30
	public static void PostCustomMapPerformance(string mapName, long mapModId, int lowestFPS, int lowestDC, int lowestPC, int avgFPS, int avgDC, int avgPC, int highestFPS, int highestDC, int highestPC, int playtime)
	{
		if (!GorillaTelemetry.IsConnected())
		{
			return;
		}
		Dictionary<string, object> dictionary = GorillaTelemetry.gCustomMapPerfArgs;
		dictionary["CustomMapName"] = mapName;
		dictionary["CustomMapModId"] = mapModId.ToString();
		dictionary["LowestFPS"] = lowestFPS.ToString();
		dictionary["LowestFPSDrawCalls"] = lowestDC.ToString();
		dictionary["LowestFPSPlayerCount"] = lowestPC.ToString();
		dictionary["AverageFPS"] = avgFPS.ToString();
		dictionary["AverageDrawCalls"] = avgDC.ToString();
		dictionary["AveragePlayerCount"] = avgPC.ToString();
		dictionary["HighestFPS"] = highestFPS.ToString();
		dictionary["HighestFPSDrawCalls"] = highestDC.ToString();
		dictionary["HighestFPSPlayerCount"] = highestPC.ToString();
		dictionary["PlaytimeInSeconds"] = playtime.ToString();
		GorillaTelemetry.QueueTelemetryEvent(new EventContents
		{
			Name = "CustomMapPerformance",
			EventNamespace = GorillaTelemetry.EVENT_NAMESPACE,
			Payload = dictionary
		});
	}

	// Token: 0x0600242B RID: 9259 RVA: 0x00101E44 File Offset: 0x00100044
	public static void PostCustomMapTracking(string mapName, long mapModId, string mapCreatorUsername, int minPlayers, int maxPlayers, int playtime, bool privateRoom)
	{
		if (!GorillaTelemetry.IsConnected())
		{
			return;
		}
		int num = playtime % 60;
		int num2 = (playtime - num) / 60;
		int num3 = num2 % 60;
		int num4 = (num2 - num3) / 60;
		string value = string.Format("{0}.{1}.{2}", num4, num3, num);
		Dictionary<string, object> dictionary = GorillaTelemetry.gCustomMapTrackingMetrics;
		dictionary["User"] = GorillaTelemetry.PlayFabUserId();
		dictionary["CustomMapName"] = mapName;
		dictionary["CustomMapModId"] = mapModId.ToString();
		dictionary["CustomMapCreator"] = mapCreatorUsername;
		dictionary["MinPlayerCount"] = minPlayers.ToString();
		dictionary["MaxPlayerCount"] = maxPlayers.ToString();
		dictionary["PlaytimeInSeconds"] = playtime.ToString();
		dictionary["PrivateRoom"] = privateRoom.ToString();
		dictionary["PlaytimeOnMap"] = value;
		GorillaTelemetry.QueueTelemetryEvent(new EventContents
		{
			Name = "CustomMapTracking",
			EventNamespace = GorillaTelemetry.EVENT_NAMESPACE,
			Payload = dictionary
		});
	}

	// Token: 0x0600242C RID: 9260 RVA: 0x00030607 File Offset: 0x0002E807
	public static void PostCustomMapDownloadEvent(string mapName, long mapModId, string mapCreatorUsername)
	{
	}

	// Token: 0x040027EF RID: 10223
	private static readonly float TELEMETRY_FLUSH_SEC = 10f;

	// Token: 0x040027F0 RID: 10224
	private static readonly ConcurrentQueue<EventContents> telemetryEventsQueue = new ConcurrentQueue<EventContents>();

	// Token: 0x040027F1 RID: 10225
	private static readonly Dictionary<int, List<EventContents>> gListPool = new Dictionary<int, List<EventContents>>();

	// Token: 0x040027F2 RID: 10226
	private static readonly string namespacePrefix = "custom";

	// Token: 0x040027F3 RID: 10227
	private static readonly string EVENT_NAMESPACE = GorillaTelemetry.namespacePrefix + "." + PlayFabAuthenticatorSettings.TitleId;

	// Token: 0x040027F4 RID: 10228
	private static PlayFabAuthenticator gPlayFabAuth;

	// Token: 0x040027F5 RID: 10229
	private static readonly Dictionary<string, object> gZoneEventArgs;

	// Token: 0x040027F6 RID: 10230
	public static GTZone CurrentZone;

	// Token: 0x040027F7 RID: 10231
	public static GTSubZone CurrentSubZone;

	// Token: 0x040027F8 RID: 10232
	private static readonly Dictionary<string, object> gShopEventArgs;

	// Token: 0x040027F9 RID: 10233
	private static CosmeticsController.CosmeticItem[] gSingleItemParam;

	// Token: 0x040027FA RID: 10234
	private static BuilderSetManager.BuilderSetStoreItem[] gSingleItemBuilderParam;

	// Token: 0x040027FB RID: 10235
	private static Dictionary<string, object> gKidEventArgs;

	// Token: 0x040027FC RID: 10236
	private static readonly Dictionary<string, object> gWamGameStartArgs;

	// Token: 0x040027FD RID: 10237
	private static readonly Dictionary<string, object> gWamLevelEndArgs;

	// Token: 0x040027FE RID: 10238
	private static Dictionary<string, object> gCustomMapPerfArgs;

	// Token: 0x040027FF RID: 10239
	private static Dictionary<string, object> gCustomMapTrackingMetrics;

	// Token: 0x04002800 RID: 10240
	private static Dictionary<string, object> gCustomMapDownloadMetrics;

	// Token: 0x020005AB RID: 1451
	public static class k
	{
		// Token: 0x04002801 RID: 10241
		public const string User = "User";

		// Token: 0x04002802 RID: 10242
		public const string ZoneId = "ZoneId";

		// Token: 0x04002803 RID: 10243
		public const string SubZoneId = "SubZoneId";

		// Token: 0x04002804 RID: 10244
		public const string EventType = "EventType";

		// Token: 0x04002805 RID: 10245
		public const string Items = "Items";

		// Token: 0x04002806 RID: 10246
		public const string VoiceChatEnabled = "VoiceChatEnabled";

		// Token: 0x04002807 RID: 10247
		public const string JoinGroups = "JoinGroups";

		// Token: 0x04002808 RID: 10248
		public const string CustomUsernameEnabled = "CustomUsernameEnabled";

		// Token: 0x04002809 RID: 10249
		public const string AgeCategory = "AgeCategory";

		// Token: 0x0400280A RID: 10250
		public const string telemetry_zone_event = "telemetry_zone_event";

		// Token: 0x0400280B RID: 10251
		public const string telemetry_shop_event = "telemetry_shop_event";

		// Token: 0x0400280C RID: 10252
		public const string telemetry_kid_event = "telemetry_kid_event";

		// Token: 0x0400280D RID: 10253
		public const string NOTHING = "NOTHING";

		// Token: 0x0400280E RID: 10254
		public const string telemetry_wam_gameStartEvent = "telemetry_wam_gameStartEvent";

		// Token: 0x0400280F RID: 10255
		public const string telemetry_wam_levelEndEvent = "telemetry_wam_levelEndEvent";

		// Token: 0x04002810 RID: 10256
		public const string WamMachineId = "WamMachineId";

		// Token: 0x04002811 RID: 10257
		public const string WamGameId = "WamGameId";

		// Token: 0x04002812 RID: 10258
		public const string WamMLevelNumber = "WamMLevelNumber";

		// Token: 0x04002813 RID: 10259
		public const string WamGoodMolesShown = "WamGoodMolesShown";

		// Token: 0x04002814 RID: 10260
		public const string WamHazardMolesShown = "WamHazardMolesShown";

		// Token: 0x04002815 RID: 10261
		public const string WamLevelMinScore = "WamLevelMinScore";

		// Token: 0x04002816 RID: 10262
		public const string WamLevelScore = "WamLevelScore";

		// Token: 0x04002817 RID: 10263
		public const string WamHazardMolesHit = "WamHazardMolesHit";

		// Token: 0x04002818 RID: 10264
		public const string WamGameState = "WamGameState";

		// Token: 0x04002819 RID: 10265
		public const string CustomMapName = "CustomMapName";

		// Token: 0x0400281A RID: 10266
		public const string LowestFPS = "LowestFPS";

		// Token: 0x0400281B RID: 10267
		public const string LowestFPSDrawCalls = "LowestFPSDrawCalls";

		// Token: 0x0400281C RID: 10268
		public const string LowestFPSPlayerCount = "LowestFPSPlayerCount";

		// Token: 0x0400281D RID: 10269
		public const string AverageFPS = "AverageFPS";

		// Token: 0x0400281E RID: 10270
		public const string AverageDrawCalls = "AverageDrawCalls";

		// Token: 0x0400281F RID: 10271
		public const string AveragePlayerCount = "AveragePlayerCount";

		// Token: 0x04002820 RID: 10272
		public const string HighestFPS = "HighestFPS";

		// Token: 0x04002821 RID: 10273
		public const string HighestFPSDrawCalls = "HighestFPSDrawCalls";

		// Token: 0x04002822 RID: 10274
		public const string HighestFPSPlayerCount = "HighestFPSPlayerCount";

		// Token: 0x04002823 RID: 10275
		public const string CustomMapCreator = "CustomMapCreator";

		// Token: 0x04002824 RID: 10276
		public const string CustomMapModId = "CustomMapModId";

		// Token: 0x04002825 RID: 10277
		public const string MinPlayerCount = "MinPlayerCount";

		// Token: 0x04002826 RID: 10278
		public const string MaxPlayerCount = "MaxPlayerCount";

		// Token: 0x04002827 RID: 10279
		public const string PlaytimeOnMap = "PlaytimeOnMap";

		// Token: 0x04002828 RID: 10280
		public const string PlaytimeInSeconds = "PlaytimeInSeconds";

		// Token: 0x04002829 RID: 10281
		public const string PrivateRoom = "PrivateRoom";
	}

	// Token: 0x020005AC RID: 1452
	private class BatchRunner : MonoBehaviour
	{
		// Token: 0x0600242D RID: 9261 RVA: 0x000487BA File Offset: 0x000469BA
		private IEnumerator Start()
		{
			for (;;)
			{
				float start = Time.time;
				while (Time.time < start + GorillaTelemetry.TELEMETRY_FLUSH_SEC)
				{
					yield return null;
				}
				GorillaTelemetry.FlushTelemetry();
			}
			yield break;
		}
	}
}
