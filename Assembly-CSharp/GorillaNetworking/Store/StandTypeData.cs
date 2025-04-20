using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B30 RID: 2864
	public class StandTypeData
	{
		// Token: 0x060047CB RID: 18379 RVA: 0x0018BC48 File Offset: 0x00189E48
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

		// Token: 0x040048FE RID: 18686
		public string departmentID = "";

		// Token: 0x040048FF RID: 18687
		public string displayID = "";

		// Token: 0x04004900 RID: 18688
		public string standID = "";

		// Token: 0x04004901 RID: 18689
		public string bustType = "";

		// Token: 0x04004902 RID: 18690
		public string playFabID = "";

		// Token: 0x02000B31 RID: 2865
		public enum EStandDataID
		{
			// Token: 0x04004904 RID: 18692
			departmentID,
			// Token: 0x04004905 RID: 18693
			displayID,
			// Token: 0x04004906 RID: 18694
			standID,
			// Token: 0x04004907 RID: 18695
			bustType,
			// Token: 0x04004908 RID: 18696
			playFabID,
			// Token: 0x04004909 RID: 18697
			Count
		}
	}
}
