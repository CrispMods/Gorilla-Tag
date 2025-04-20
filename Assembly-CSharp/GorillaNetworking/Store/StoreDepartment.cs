using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B32 RID: 2866
	public class StoreDepartment : MonoBehaviour
	{
		// Token: 0x060047CC RID: 18380 RVA: 0x0018BD2C File Offset: 0x00189F2C
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

		// Token: 0x0400490A RID: 18698
		public StoreDisplay[] Displays;

		// Token: 0x0400490B RID: 18699
		public string departmentName = "";
	}
}
