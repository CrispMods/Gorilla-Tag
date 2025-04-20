using System;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000CB5 RID: 3253
	[CreateAssetMenu(fileName = "New CountdownText Date", menuName = "Game Object Scheduling/CountdownText Date", order = 1)]
	public class CountdownTextDate : ScriptableObject
	{
		// Token: 0x0400544C RID: 21580
		public string CountdownTo = "1/1/0001 00:00:00";

		// Token: 0x0400544D RID: 21581
		public string FormatString = "{0} {1}";

		// Token: 0x0400544E RID: 21582
		public string DefaultString = "";

		// Token: 0x0400544F RID: 21583
		public int DaysThreshold = 365;
	}
}
