using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B2F RID: 2863
	public class StandImport
	{
		// Token: 0x060047C9 RID: 18377 RVA: 0x0018BBC8 File Offset: 0x00189DC8
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

		// Token: 0x040048FD RID: 18685
		public List<StandTypeData> standData = new List<StandTypeData>();
	}
}
