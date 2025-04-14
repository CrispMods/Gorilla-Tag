using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B02 RID: 2818
	public class StandImport
	{
		// Token: 0x06004680 RID: 18048 RVA: 0x0014EFEC File Offset: 0x0014D1EC
		public void DecomposeStandData(string dataString)
		{
			string[] array = dataString.Split('\t', StringSplitOptions.None);
			if (array.Length == 5)
			{
				this.standData.Add(new StandTypeData(array));
				return;
			}
			if (array.Length == 4)
			{
				this.standData.Add(new StandTypeData(array));
				return;
			}
			string text = "";
			foreach (string str in array)
			{
				text = text + str + "|";
			}
			Debug.LogError("Store Importer Data String is not valid : " + text);
		}

		// Token: 0x04004808 RID: 18440
		public List<StandTypeData> standData = new List<StandTypeData>();
	}
}
