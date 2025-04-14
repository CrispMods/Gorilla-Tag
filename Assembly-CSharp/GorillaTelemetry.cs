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

// Token: 0x0200059C RID: 1436
public static class GorillaTelemetry
{
	// Token: 0x060023B8 RID: 9144 RVA: 0x000B2348 File Offset: 0x000B0548
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
		Object.DontDestroyOnLoad(gameObject);
		gameObject.AddComponent<GorillaTelemetry.BatchRunner>();
	}

	// Token: 0x060023B9 RID: 9145 RVA: 0x000B2677 File Offset: 0x000B0877
	private static void QueueTelemetryEvent(EventContents eventContent)
	{
		GorillaTelemetry.telemetryEventsQueue.Enqueue(eventContent);
	}

	// Token: 0x060023BA RID: 9146 RVA: 0x000B2684 File Offset: 0x000B0884
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

	// Token: 0x060023BB RID: 9147 RVA: 0x000B2754 File Offset: 0x000B0954
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

	// Token: 0x060023BC RID: 9148 RVA: 0x000B27BE File Offset: 0x000B09BE
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

	// Token: 0x060023BD RID: 9149 RVA: 0x000B27F1 File Offset: 0x000B09F1
	private static string PlayFabUserId()
	{
		return GorillaTelemetry.gPlayFabAuth.GetPlayFabPlayerId();
	}

	// Token: 0x060023BE RID: 9150 RVA: 0x000B2800 File Offset: 0x000B0A00
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

	// Token: 0x060023BF RID: 9151 RVA: 0x000B2893 File Offset: 0x000B0A93
	public static void PostShopEvent(VRRig playerRig, GTShopEventType shopEvent, CosmeticsController.CosmeticItem item)
	{
		GorillaTelemetry.gSingleItemParam[0] = item;
		GorillaTelemetry.PostShopEvent(playerRig, shopEvent, GorillaTelemetry.gSingleItemParam);
		GorillaTelemetry.gSingleItemParam[0] = default(CosmeticsController.CosmeticItem);
	}

	// Token: 0x060023C0 RID: 9152 RVA: 0x000B28C0 File Offset: 0x000B0AC0
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

	// Token: 0x060023C1 RID: 9153 RVA: 0x000B294C File Offset: 0x000B0B4C
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

	// Token: 0x060023C2 RID: 9154 RVA: 0x000023F4 File Offset: 0x000005F4
	private static void PostShopEvent_OnResult(WriteEventResponse result)
	{
	}

	// Token: 0x060023C3 RID: 9155 RVA: 0x000023F4 File Offset: 0x000005F4
	private static void PostShopEvent_OnError(PlayFabError error)
	{
	}

	// Token: 0x060023C4 RID: 9156 RVA: 0x000B29DE File Offset: 0x000B0BDE
	public static void PostBuilderKioskEvent(VRRig playerRig, GTShopEventType shopEvent, BuilderSetManager.BuilderSetStoreItem item)
	{
		GorillaTelemetry.gSingleItemBuilderParam[0] = item;
		GorillaTelemetry.PostBuilderKioskEvent(playerRig, shopEvent, GorillaTelemetry.gSingleItemBuilderParam);
		GorillaTelemetry.gSingleItemBuilderParam[0] = default(BuilderSetManager.BuilderSetStoreItem);
	}

	// Token: 0x060023C5 RID: 9157 RVA: 0x000B2A0C File Offset: 0x000B0C0C
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

	// Token: 0x060023C6 RID: 9158 RVA: 0x000B2A98 File Offset: 0x000B0C98
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

	// Token: 0x060023C7 RID: 9159 RVA: 0x000B2B2C File Offset: 0x000B0D2C
	public static void PostKidEvent(bool joinGroupsEnabled, bool voiceChatEnabled, bool customUsernamesEnabled, AgeStatusType ageCategory, GTKidEventType kidEvent)
	{
		if ((double)Random.value < 0.1)
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

	// Token: 0x060023C8 RID: 9160 RVA: 0x000B2C1C File Offset: 0x000B0E1C
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

	// Token: 0x060023C9 RID: 9161 RVA: 0x000B2C8C File Offset: 0x000B0E8C
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

	// Token: 0x060023CA RID: 9162 RVA: 0x000B2D98 File Offset: 0x000B0F98
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

	// Token: 0x060023CB RID: 9163 RVA: 0x000B2EAC File Offset: 0x000B10AC
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

	// Token: 0x060023CC RID: 9164 RVA: 0x000023F4 File Offset: 0x000005F4
	public static void PostCustomMapDownloadEvent(string mapName, long mapModId, string mapCreatorUsername)
	{
	}

	// Token: 0x04002793 RID: 10131
	private static readonly float TELEMETRY_FLUSH_SEC = 10f;

	// Token: 0x04002794 RID: 10132
	private static readonly ConcurrentQueue<EventContents> telemetryEventsQueue = new ConcurrentQueue<EventContents>();

	// Token: 0x04002795 RID: 10133
	private static readonly Dictionary<int, List<EventContents>> gListPool = new Dictionary<int, List<EventContents>>();

	// Token: 0x04002796 RID: 10134
	private static readonly string namespacePrefix = "custom";

	// Token: 0x04002797 RID: 10135
	private static readonly string EVENT_NAMESPACE = GorillaTelemetry.namespacePrefix + "." + PlayFabAuthenticatorSettings.TitleId;

	// Token: 0x04002798 RID: 10136
	private static PlayFabAuthenticator gPlayFabAuth;

	// Token: 0x04002799 RID: 10137
	private static readonly Dictionary<string, object> gZoneEventArgs;

	// Token: 0x0400279A RID: 10138
	public static GTZone CurrentZone;

	// Token: 0x0400279B RID: 10139
	public static GTSubZone CurrentSubZone;

	// Token: 0x0400279C RID: 10140
	private static readonly Dictionary<string, object> gShopEventArgs;

	// Token: 0x0400279D RID: 10141
	private static CosmeticsController.CosmeticItem[] gSingleItemParam;

	// Token: 0x0400279E RID: 10142
	private static BuilderSetManager.BuilderSetStoreItem[] gSingleItemBuilderParam;

	// Token: 0x0400279F RID: 10143
	private static Dictionary<string, object> gKidEventArgs;

	// Token: 0x040027A0 RID: 10144
	private static readonly Dictionary<string, object> gWamGameStartArgs;

	// Token: 0x040027A1 RID: 10145
	private static readonly Dictionary<string, object> gWamLevelEndArgs;

	// Token: 0x040027A2 RID: 10146
	private static Dictionary<string, object> gCustomMapPerfArgs;

	// Token: 0x040027A3 RID: 10147
	private static Dictionary<string, object> gCustomMapTrackingMetrics;

	// Token: 0x040027A4 RID: 10148
	private static Dictionary<string, object> gCustomMapDownloadMetrics;

	// Token: 0x0200059D RID: 1437
	public static class k
	{
		// Token: 0x040027A5 RID: 10149
		public const string User = "User";

		// Token: 0x040027A6 RID: 10150
		public const string ZoneId = "ZoneId";

		// Token: 0x040027A7 RID: 10151
		public const string SubZoneId = "SubZoneId";

		// Token: 0x040027A8 RID: 10152
		public const string EventType = "EventType";

		// Token: 0x040027A9 RID: 10153
		public const string Items = "Items";

		// Token: 0x040027AA RID: 10154
		public const string VoiceChatEnabled = "VoiceChatEnabled";

		// Token: 0x040027AB RID: 10155
		public const string JoinGroups = "JoinGroups";

		// Token: 0x040027AC RID: 10156
		public const string CustomUsernameEnabled = "CustomUsernameEnabled";

		// Token: 0x040027AD RID: 10157
		public const string AgeCategory = "AgeCategory";

		// Token: 0x040027AE RID: 10158
		public const string telemetry_zone_event = "telemetry_zone_event";

		// Token: 0x040027AF RID: 10159
		public const string telemetry_shop_event = "telemetry_shop_event";

		// Token: 0x040027B0 RID: 10160
		public const string telemetry_kid_event = "telemetry_kid_event";

		// Token: 0x040027B1 RID: 10161
		public const string NOTHING = "NOTHING";

		// Token: 0x040027B2 RID: 10162
		public const string telemetry_wam_gameStartEvent = "telemetry_wam_gameStartEvent";

		// Token: 0x040027B3 RID: 10163
		public const string telemetry_wam_levelEndEvent = "telemetry_wam_levelEndEvent";

		// Token: 0x040027B4 RID: 10164
		public const string WamMachineId = "WamMachineId";

		// Token: 0x040027B5 RID: 10165
		public const string WamGameId = "WamGameId";

		// Token: 0x040027B6 RID: 10166
		public const string WamMLevelNumber = "WamMLevelNumber";

		// Token: 0x040027B7 RID: 10167
		public const string WamGoodMolesShown = "WamGoodMolesShown";

		// Token: 0x040027B8 RID: 10168
		public const string WamHazardMolesShown = "WamHazardMolesShown";

		// Token: 0x040027B9 RID: 10169
		public const string WamLevelMinScore = "WamLevelMinScore";

		// Token: 0x040027BA RID: 10170
		public const string WamLevelScore = "WamLevelScore";

		// Token: 0x040027BB RID: 10171
		public const string WamHazardMolesHit = "WamHazardMolesHit";

		// Token: 0x040027BC RID: 10172
		public const string WamGameState = "WamGameState";

		// Token: 0x040027BD RID: 10173
		public const string CustomMapName = "CustomMapName";

		// Token: 0x040027BE RID: 10174
		public const string LowestFPS = "LowestFPS";

		// Token: 0x040027BF RID: 10175
		public const string LowestFPSDrawCalls = "LowestFPSDrawCalls";

		// Token: 0x040027C0 RID: 10176
		public const string LowestFPSPlayerCount = "LowestFPSPlayerCount";

		// Token: 0x040027C1 RID: 10177
		public const string AverageFPS = "AverageFPS";

		// Token: 0x040027C2 RID: 10178
		public const string AverageDrawCalls = "AverageDrawCalls";

		// Token: 0x040027C3 RID: 10179
		public const string AveragePlayerCount = "AveragePlayerCount";

		// Token: 0x040027C4 RID: 10180
		public const string HighestFPS = "HighestFPS";

		// Token: 0x040027C5 RID: 10181
		public const string HighestFPSDrawCalls = "HighestFPSDrawCalls";

		// Token: 0x040027C6 RID: 10182
		public const string HighestFPSPlayerCount = "HighestFPSPlayerCount";

		// Token: 0x040027C7 RID: 10183
		public const string CustomMapCreator = "CustomMapCreator";

		// Token: 0x040027C8 RID: 10184
		public const string CustomMapModId = "CustomMapModId";

		// Token: 0x040027C9 RID: 10185
		public const string MinPlayerCount = "MinPlayerCount";

		// Token: 0x040027CA RID: 10186
		public const string MaxPlayerCount = "MaxPlayerCount";

		// Token: 0x040027CB RID: 10187
		public const string PlaytimeOnMap = "PlaytimeOnMap";

		// Token: 0x040027CC RID: 10188
		public const string PlaytimeInSeconds = "PlaytimeInSeconds";

		// Token: 0x040027CD RID: 10189
		public const string PrivateRoom = "PrivateRoom";
	}

	// Token: 0x0200059E RID: 1438
	private class BatchRunner : MonoBehaviour
	{
		// Token: 0x060023CD RID: 9165 RVA: 0x000B2FBC File Offset: 0x000B11BC
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
