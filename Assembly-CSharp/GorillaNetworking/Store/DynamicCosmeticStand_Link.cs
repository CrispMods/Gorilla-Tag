using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B2B RID: 2859
	public class DynamicCosmeticStand_Link : MonoBehaviour
	{
		// Token: 0x060047AA RID: 18346 RVA: 0x0005EAF5 File Offset: 0x0005CCF5
		public void SetStandType(HeadModel_CosmeticStand.BustType type)
		{
			this.stand.SetStandType(type);
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x0005EB03 File Offset: 0x0005CD03
		public void SpawnItemOntoStand(string PlayFabID)
		{
			this.stand.SpawnItemOntoStand(PlayFabID);
		}

		// Token: 0x060047AC RID: 18348 RVA: 0x0005EB11 File Offset: 0x0005CD11
		public void SaveCosmeticMountPosition()
		{
			this.stand.UpdateCosmeticsMountPositions();
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x0005EB1E File Offset: 0x0005CD1E
		public void ClearCosmeticItems()
		{
			this.stand.ClearCosmetics();
		}

		// Token: 0x040048E8 RID: 18664
		public DynamicCosmeticStand stand;
	}
}
