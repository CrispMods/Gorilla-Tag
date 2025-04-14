using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020002B0 RID: 688
public class NonCosmeticHandItem : MonoBehaviour
{
	// Token: 0x060010B5 RID: 4277 RVA: 0x00050F03 File Offset: 0x0004F103
	public void EnableItem(bool enable)
	{
		if (this.itemPrefab)
		{
			this.itemPrefab.gameObject.SetActive(enable);
		}
	}

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x060010B6 RID: 4278 RVA: 0x00050F23 File Offset: 0x0004F123
	public bool IsEnabled
	{
		get
		{
			return this.itemPrefab && this.itemPrefab.gameObject.activeSelf;
		}
	}

	// Token: 0x040012B8 RID: 4792
	public CosmeticsController.CosmeticSlots cosmeticSlots;

	// Token: 0x040012B9 RID: 4793
	public GameObject itemPrefab;
}
