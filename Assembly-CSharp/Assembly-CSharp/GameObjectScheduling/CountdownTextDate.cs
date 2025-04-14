using System;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000C87 RID: 3207
	[CreateAssetMenu(fileName = "New CountdownText Date", menuName = "Game Object Scheduling/CountdownText Date", order = 1)]
	public class CountdownTextDate : ScriptableObject
	{
		// Token: 0x04005352 RID: 21330
		public string CountdownTo = "1/1/0001 00:00:00";

		// Token: 0x04005353 RID: 21331
		public string FormatString = "{0} {1}";

		// Token: 0x04005354 RID: 21332
		public string DefaultString = "";

		// Token: 0x04005355 RID: 21333
		public int DaysThreshold = 365;
	}
}
