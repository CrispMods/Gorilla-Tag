using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B03 RID: 2819
	public class StandTypeData
	{
		// Token: 0x06004682 RID: 18050 RVA: 0x0014F080 File Offset: 0x0014D280
		public StandTypeData(string[] spawnData)
		{
			this.departmentID = spawnData[0];
			this.displayID = spawnData[1];
			this.standID = spawnData[2];
			this.bustType = spawnData[3];
			if (spawnData.Length == 5)
			{
				this.playFabID = spawnData[4];
			}
			Debug.Log(string.Concat(new string[]
			{
				"StoreStuff: StandTypeData: ",
				this.departmentID,
				"\n",
				this.displayID,
				"\n",
				this.standID,
				"\n",
				this.bustType,
				"\n",
				this.playFabID
			}));
		}

		// Token: 0x04004809 RID: 18441
		public string departmentID = "";

		// Token: 0x0400480A RID: 18442
		public string displayID = "";

		// Token: 0x0400480B RID: 18443
		public string standID = "";

		// Token: 0x0400480C RID: 18444
		public string bustType = "";

		// Token: 0x0400480D RID: 18445
		public string playFabID = "";

		// Token: 0x02000B04 RID: 2820
		public enum EStandDataID
		{
			// Token: 0x0400480F RID: 18447
			departmentID,
			// Token: 0x04004810 RID: 18448
			displayID,
			// Token: 0x04004811 RID: 18449
			standID,
			// Token: 0x04004812 RID: 18450
			bustType,
			// Token: 0x04004813 RID: 18451
			playFabID,
			// Token: 0x04004814 RID: 18452
			Count
		}
	}
}
