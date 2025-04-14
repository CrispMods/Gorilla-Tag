using System;
using System.Collections.Generic;
using LitJson;
using Newtonsoft.Json;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B0E RID: 2830
	public class StoreUpdateEvent
	{
		// Token: 0x060046B3 RID: 18099 RVA: 0x00002050 File Offset: 0x00000250
		public StoreUpdateEvent()
		{
		}

		// Token: 0x060046B4 RID: 18100 RVA: 0x0014FD74 File Offset: 0x0014DF74
		public StoreUpdateEvent(string pedestalID, string itemName, DateTime startTimeUTC, DateTime endTimeUTC)
		{
			this.PedestalID = pedestalID;
			this.ItemName = itemName;
			this.StartTimeUTC = startTimeUTC;
			this.EndTimeUTC = endTimeUTC;
		}

		// Token: 0x060046B5 RID: 18101 RVA: 0x0014FD99 File Offset: 0x0014DF99
		public static string SerializeAsJSon(StoreUpdateEvent storeEvent)
		{
			return JsonUtility.ToJson(storeEvent);
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x0014FDA1 File Offset: 0x0014DFA1
		public static string SerializeArrayAsJSon(StoreUpdateEvent[] storeEvents)
		{
			return JsonConvert.SerializeObject(storeEvents);
		}

		// Token: 0x060046B7 RID: 18103 RVA: 0x0014FDA9 File Offset: 0x0014DFA9
		public static StoreUpdateEvent DeserializeFromJSon(string json)
		{
			return JsonUtility.FromJson<StoreUpdateEvent>(json);
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x0014FDB1 File Offset: 0x0014DFB1
		public static StoreUpdateEvent[] DeserializeFromJSonArray(string json)
		{
			List<StoreUpdateEvent> list = JsonMapper.ToObject<List<StoreUpdateEvent>>(json);
			list.Sort((StoreUpdateEvent x, StoreUpdateEvent y) => x.StartTimeUTC.CompareTo(y.StartTimeUTC));
			return list.ToArray();
		}

		// Token: 0x060046B9 RID: 18105 RVA: 0x0014FDE3 File Offset: 0x0014DFE3
		public static List<StoreUpdateEvent> DeserializeFromJSonList(string json)
		{
			List<StoreUpdateEvent> list = JsonMapper.ToObject<List<StoreUpdateEvent>>(json);
			list.Sort((StoreUpdateEvent x, StoreUpdateEvent y) => x.StartTimeUTC.CompareTo(y.StartTimeUTC));
			return list;
		}

		// Token: 0x04004849 RID: 18505
		public string PedestalID;

		// Token: 0x0400484A RID: 18506
		public string ItemName;

		// Token: 0x0400484B RID: 18507
		public DateTime StartTimeUTC;

		// Token: 0x0400484C RID: 18508
		public DateTime EndTimeUTC;
	}
}
