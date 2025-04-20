using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B33 RID: 2867
	public class StoreDisplay : MonoBehaviour
	{
		// Token: 0x060047CE RID: 18382 RVA: 0x0005EC52 File Offset: 0x0005CE52
		private void GetAllDynamicCosmeticStands()
		{
			this.Stands = base.GetComponentsInChildren<DynamicCosmeticStand>();
		}

		// Token: 0x060047CF RID: 18383 RVA: 0x0018BD9C File Offset: 0x00189F9C
		private void SetDisplayNameForAllStands()
		{
			DynamicCosmeticStand[] componentsInChildren = base.GetComponentsInChildren<DynamicCosmeticStand>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].CopyChildsName();
			}
		}

		// Token: 0x0400490C RID: 18700
		public string displayName = "";

		// Token: 0x0400490D RID: 18701
		public DynamicCosmeticStand[] Stands;
	}
}
