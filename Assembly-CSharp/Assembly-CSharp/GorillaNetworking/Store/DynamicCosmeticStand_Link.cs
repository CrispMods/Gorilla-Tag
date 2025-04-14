using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B01 RID: 2817
	public class DynamicCosmeticStand_Link : MonoBehaviour
	{
		// Token: 0x0600466E RID: 18030 RVA: 0x0014EA1C File Offset: 0x0014CC1C
		public void SetStandType(HeadModel_CosmeticStand.BustType type)
		{
			this.stand.SetStandType(type);
		}

		// Token: 0x0600466F RID: 18031 RVA: 0x0014EA2A File Offset: 0x0014CC2A
		public void SpawnItemOntoStand(string PlayFabID)
		{
			this.stand.SpawnItemOntoStand(PlayFabID);
		}

		// Token: 0x06004670 RID: 18032 RVA: 0x0014EA38 File Offset: 0x0014CC38
		public void SaveCosmeticMountPosition()
		{
			this.stand.UpdateCosmeticsMountPositions();
		}

		// Token: 0x04004805 RID: 18437
		public DynamicCosmeticStand stand;
	}
}
