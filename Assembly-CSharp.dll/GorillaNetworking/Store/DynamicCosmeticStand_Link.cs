using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B01 RID: 2817
	public class DynamicCosmeticStand_Link : MonoBehaviour
	{
		// Token: 0x0600466E RID: 18030 RVA: 0x0005D0EB File Offset: 0x0005B2EB
		public void SetStandType(HeadModel_CosmeticStand.BustType type)
		{
			this.stand.SetStandType(type);
		}

		// Token: 0x0600466F RID: 18031 RVA: 0x0005D0F9 File Offset: 0x0005B2F9
		public void SpawnItemOntoStand(string PlayFabID)
		{
			this.stand.SpawnItemOntoStand(PlayFabID);
		}

		// Token: 0x06004670 RID: 18032 RVA: 0x0005D107 File Offset: 0x0005B307
		public void SaveCosmeticMountPosition()
		{
			this.stand.UpdateCosmeticsMountPositions();
		}

		// Token: 0x04004805 RID: 18437
		public DynamicCosmeticStand stand;
	}
}
