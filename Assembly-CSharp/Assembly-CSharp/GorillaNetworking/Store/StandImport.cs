using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B05 RID: 2821
	public class StandImport
	{
		// Token: 0x0600468C RID: 18060 RVA: 0x0014F5B4 File Offset: 0x0014D7B4
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

		// Token: 0x0400481A RID: 18458
		public List<StandTypeData> standData = new List<StandTypeData>();
	}
}
