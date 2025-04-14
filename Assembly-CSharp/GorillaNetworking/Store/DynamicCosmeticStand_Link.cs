using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000AFE RID: 2814
	public class DynamicCosmeticStand_Link : MonoBehaviour
	{
		// Token: 0x06004662 RID: 18018 RVA: 0x0014E454 File Offset: 0x0014C654
		public void SetStandType(HeadModel_CosmeticStand.BustType type)
		{
			this.stand.SetStandType(type);
		}

		// Token: 0x06004663 RID: 18019 RVA: 0x0014E462 File Offset: 0x0014C662
		public void SpawnItemOntoStand(string PlayFabID)
		{
			this.stand.SpawnItemOntoStand(PlayFabID);
		}

		// Token: 0x06004664 RID: 18020 RVA: 0x0014E470 File Offset: 0x0014C670
		public void SaveCosmeticMountPosition()
		{
			this.stand.UpdateCosmeticsMountPositions();
		}

		// Token: 0x040047F3 RID: 18419
		public DynamicCosmeticStand stand;
	}
}
