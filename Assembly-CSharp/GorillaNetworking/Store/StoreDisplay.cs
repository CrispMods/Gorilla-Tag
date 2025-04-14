using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B06 RID: 2822
	public class StoreDisplay : MonoBehaviour
	{
		// Token: 0x06004685 RID: 18053 RVA: 0x0014F1E7 File Offset: 0x0014D3E7
		private void GetAllDynamicCosmeticStands()
		{
			this.Stands = base.GetComponentsInChildren<DynamicCosmeticStand>();
		}

		// Token: 0x06004686 RID: 18054 RVA: 0x0014F1F8 File Offset: 0x0014D3F8
		private void SetDisplayNameForAllStands()
		{
			DynamicCosmeticStand[] componentsInChildren = base.GetComponentsInChildren<DynamicCosmeticStand>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].CopyChildsName();
			}
		}

		// Token: 0x04004817 RID: 18455
		public string displayName = "";

		// Token: 0x04004818 RID: 18456
		public DynamicCosmeticStand[] Stands;
	}
}
