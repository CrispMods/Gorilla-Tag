using System;
using System.Collections.Generic;
using LitJson;
using Newtonsoft.Json;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B0B RID: 2827
	public class StoreUpdateEvent
	{
		// Token: 0x060046A7 RID: 18087 RVA: 0x00002050 File Offset: 0x00000250
		public StoreUpdateEvent()
		{
		}

		// Token: 0x060046A8 RID: 18088 RVA: 0x0014F7AC File Offset: 0x0014D9AC
		public StoreUpdateEvent(string pedestalID, string itemName, DateTime startTimeUTC, DateTime endTimeUTC)
		{
			this.PedestalID = pedestalID;
			this.ItemName = itemName;
			this.StartTimeUTC = startTimeUTC;
			this.EndTimeUTC = endTimeUTC;
		}

		// Token: 0x060046A9 RID: 18089 RVA: 0x0014F7D1 File Offset: 0x0014D9D1
		public static string SerializeAsJSon(StoreUpdateEvent storeEvent)
		{
			return JsonUtility.ToJson(storeEvent);
		}

		// Token: 0x060046AA RID: 18090 RVA: 0x0014F7D9 File Offset: 0x0014D9D9
		public static string SerializeArrayAsJSon(StoreUpdateEvent[] storeEvents)
		{
			return JsonConvert.SerializeObject(storeEvents);
		}

		// Token: 0x060046AB RID: 18091 RVA: 0x0014F7E1 File Offset: 0x0014D9E1
		public static StoreUpdateEvent DeserializeFromJSon(string json)
		{
			return JsonUtility.FromJson<StoreUpdateEvent>(json);
		}

		// Token: 0x060046AC RID: 18092 RVA: 0x0014F7E9 File Offset: 0x0014D9E9
		public static StoreUpdateEvent[] DeserializeFromJSonArray(string json)
		{
			List<StoreUpdateEvent> list = JsonMapper.ToObject<List<StoreUpdateEvent>>(json);
			list.Sort((StoreUpdateEvent x, StoreUpdateEvent y) => x.StartTimeUTC.CompareTo(y.StartTimeUTC));
			return list.ToArray();
		}

		// Token: 0x060046AD RID: 18093 RVA: 0x0014F81B File Offset: 0x0014DA1B
		public static List<StoreUpdateEvent> DeserializeFromJSonList(string json)
		{
			List<StoreUpdateEvent> list = JsonMapper.ToObject<List<StoreUpdateEvent>>(json);
			list.Sort((StoreUpdateEvent x, StoreUpdateEvent y) => x.StartTimeUTC.CompareTo(y.StartTimeUTC));
			return list;
		}

		// Token: 0x04004837 RID: 18487
		public string PedestalID;

		// Token: 0x04004838 RID: 18488
		public string ItemName;

		// Token: 0x04004839 RID: 18489
		public DateTime StartTimeUTC;

		// Token: 0x0400483A RID: 18490
		public DateTime EndTimeUTC;
	}
}
