using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B08 RID: 2824
	public class StoreDepartment : MonoBehaviour
	{
		// Token: 0x0600468F RID: 18063 RVA: 0x00184DB8 File Offset: 0x00182FB8
		private void FindAllDisplays()
		{
			this.Displays = base.GetComponentsInChildren<StoreDisplay>();
			for (int i = this.Displays.Length - 1; i >= 0; i--)
			{
				if (string.IsNullOrEmpty(this.Displays[i].displayName))
				{
					this.Displays[i] = this.Displays[this.Displays.Length - 1];
					Array.Resize<StoreDisplay>(ref this.Displays, this.Displays.Length - 1);
				}
			}
		}

		// Token: 0x04004827 RID: 18471
		public StoreDisplay[] Displays;

		// Token: 0x04004828 RID: 18472
		public string departmentName = "";
	}
}
