using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B05 RID: 2821
	public class StoreDepartment : MonoBehaviour
	{
		// Token: 0x06004683 RID: 18051 RVA: 0x0014F164 File Offset: 0x0014D364
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

		// Token: 0x04004815 RID: 18453
		public StoreDisplay[] Displays;

		// Token: 0x04004816 RID: 18454
		public string departmentName = "";
	}
}
