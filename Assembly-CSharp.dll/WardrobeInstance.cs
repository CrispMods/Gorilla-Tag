using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000432 RID: 1074
public class WardrobeInstance : MonoBehaviour
{
	// Token: 0x06001A88 RID: 6792 RVA: 0x00040FA2 File Offset: 0x0003F1A2
	public void Start()
	{
		CosmeticsController.instance.AddWardrobeInstance(this);
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x00040FB1 File Offset: 0x0003F1B1
	public void OnDestroy()
	{
		CosmeticsController.instance.RemoveWardrobeInstance(this);
	}

	// Token: 0x04001D4F RID: 7503
	public WardrobeItemButton[] wardrobeItemButtons;

	// Token: 0x04001D50 RID: 7504
	public HeadModel selfDoll;
}
