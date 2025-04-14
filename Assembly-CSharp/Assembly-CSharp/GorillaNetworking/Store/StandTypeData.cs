using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B06 RID: 2822
	public class StandTypeData
	{
		// Token: 0x0600468E RID: 18062 RVA: 0x0014F648 File Offset: 0x0014D848
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

		// Token: 0x0400481B RID: 18459
		public string departmentID = "";

		// Token: 0x0400481C RID: 18460
		public string displayID = "";

		// Token: 0x0400481D RID: 18461
		public string standID = "";

		// Token: 0x0400481E RID: 18462
		public string bustType = "";

		// Token: 0x0400481F RID: 18463
		public string playFabID = "";

		// Token: 0x02000B07 RID: 2823
		public enum EStandDataID
		{
			// Token: 0x04004821 RID: 18465
			departmentID,
			// Token: 0x04004822 RID: 18466
			displayID,
			// Token: 0x04004823 RID: 18467
			standID,
			// Token: 0x04004824 RID: 18468
			bustType,
			// Token: 0x04004825 RID: 18469
			playFabID,
			// Token: 0x04004826 RID: 18470
			Count
		}
	}
}
