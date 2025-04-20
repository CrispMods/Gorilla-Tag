using System;
using System.Collections.Generic;
using LitJson;
using Newtonsoft.Json;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B38 RID: 2872
	public class StoreUpdateEvent
	{
		// Token: 0x060047F0 RID: 18416 RVA: 0x00030490 File Offset: 0x0002E690
		public StoreUpdateEvent()
		{
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x0005ED60 File Offset: 0x0005CF60
		public StoreUpdateEvent(string pedestalID, string itemName, DateTime startTimeUTC, DateTime endTimeUTC)
		{
			this.PedestalID = pedestalID;
			this.ItemName = itemName;
			this.StartTimeUTC = startTimeUTC;
			this.EndTimeUTC = endTimeUTC;
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x0005ED85 File Offset: 0x0005CF85
		public static string SerializeAsJSon(StoreUpdateEvent storeEvent)
		{
			return JsonUtility.ToJson(storeEvent);
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x0005ED8D File Offset: 0x0005CF8D
		public static string SerializeArrayAsJSon(StoreUpdateEvent[] storeEvents)
		{
			return JsonConvert.SerializeObject(storeEvents);
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x0005ED95 File Offset: 0x0005CF95
		public static StoreUpdateEvent DeserializeFromJSon(string json)
		{
			return JsonUtility.FromJson<StoreUpdateEvent>(json);
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x0005ED9D File Offset: 0x0005CF9D
		public static StoreUpdateEvent[] DeserializeFromJSonArray(string json)
		{
			List<StoreUpdateEvent> list = JsonMapper.ToObject<List<StoreUpdateEvent>>(json);
			list.Sort((StoreUpdateEvent x, StoreUpdateEvent y) => x.StartTimeUTC.CompareTo(y.StartTimeUTC));
			return list.ToArray();
		}

		// Token: 0x060047F6 RID: 18422 RVA: 0x0005EDCF File Offset: 0x0005CFCF
		public static List<StoreUpdateEvent> DeserializeFromJSonList(string json)
		{
			List<StoreUpdateEvent> list = JsonMapper.ToObject<List<StoreUpdateEvent>>(json);
			list.Sort((StoreUpdateEvent x, StoreUpdateEvent y) => x.StartTimeUTC.CompareTo(y.StartTimeUTC));
			return list;
		}

		// Token: 0x0400492C RID: 18732
		public string PedestalID;

		// Token: 0x0400492D RID: 18733
		public string ItemName;

		// Token: 0x0400492E RID: 18734
		public DateTime StartTimeUTC;

		// Token: 0x0400492F RID: 18735
		public DateTime EndTimeUTC;
	}
}
