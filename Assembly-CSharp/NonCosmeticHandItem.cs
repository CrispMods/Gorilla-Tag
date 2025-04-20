using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020002BB RID: 699
public class NonCosmeticHandItem : MonoBehaviour
{
	// Token: 0x06001101 RID: 4353 RVA: 0x0003BA1A File Offset: 0x00039C1A
	public void EnableItem(bool enable)
	{
		if (this.itemPrefab)
		{
			this.itemPrefab.gameObject.SetActive(enable);
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x06001102 RID: 4354 RVA: 0x0003BA3A File Offset: 0x00039C3A
	public bool IsEnabled
	{
		get
		{
			return this.itemPrefab && this.itemPrefab.gameObject.activeSelf;
		}
	}

	// Token: 0x04001300 RID: 4864
	public CosmeticsController.CosmeticSlots cosmeticSlots;

	// Token: 0x04001301 RID: 4865
	public GameObject itemPrefab;
}
