using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B09 RID: 2825
	public class StoreDisplay : MonoBehaviour
	{
		// Token: 0x06004691 RID: 18065 RVA: 0x0005D23B File Offset: 0x0005B43B
		private void GetAllDynamicCosmeticStands()
		{
			this.Stands = base.GetComponentsInChildren<DynamicCosmeticStand>();
		}

		// Token: 0x06004692 RID: 18066 RVA: 0x00184E28 File Offset: 0x00183028
		private void SetDisplayNameForAllStands()
		{
			DynamicCosmeticStand[] componentsInChildren = base.GetComponentsInChildren<DynamicCosmeticStand>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].CopyChildsName();
			}
		}

		// Token: 0x04004829 RID: 18473
		public string displayName = "";

		// Token: 0x0400482A RID: 18474
		public DynamicCosmeticStand[] Stands;
	}
}
