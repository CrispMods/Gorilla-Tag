using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020002B0 RID: 688
public class NonCosmeticHandItem : MonoBehaviour
{
	// Token: 0x060010B8 RID: 4280 RVA: 0x00051287 File Offset: 0x0004F487
	public void EnableItem(bool enable)
	{
		if (this.itemPrefab)
		{
			this.itemPrefab.gameObject.SetActive(enable);
		}
	}

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x060010B9 RID: 4281 RVA: 0x000512A7 File Offset: 0x0004F4A7
	public bool IsEnabled
	{
		get
		{
			return this.itemPrefab && this.itemPrefab.gameObject.activeSelf;
		}
	}

	// Token: 0x040012B9 RID: 4793
	public CosmeticsController.CosmeticSlots cosmeticSlots;

	// Token: 0x040012BA RID: 4794
	public GameObject itemPrefab;
}
