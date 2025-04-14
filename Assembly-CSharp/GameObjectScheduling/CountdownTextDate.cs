using System;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000C84 RID: 3204
	[CreateAssetMenu(fileName = "New CountdownText Date", menuName = "Game Object Scheduling/CountdownText Date", order = 1)]
	public class CountdownTextDate : ScriptableObject
	{
		// Token: 0x04005340 RID: 21312
		public string CountdownTo = "1/1/0001 00:00:00";

		// Token: 0x04005341 RID: 21313
		public string FormatString = "{0} {1}";

		// Token: 0x04005342 RID: 21314
		public string DefaultString = "";

		// Token: 0x04005343 RID: 21315
		public int DaysThreshold = 365;
	}
}
