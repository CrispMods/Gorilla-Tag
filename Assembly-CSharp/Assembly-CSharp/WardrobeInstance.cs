using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000432 RID: 1074
public class WardrobeInstance : MonoBehaviour
{
	// Token: 0x06001A88 RID: 6792 RVA: 0x0008320F File Offset: 0x0008140F
	public void Start()
	{
		CosmeticsController.instance.AddWardrobeInstance(this);
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x0008321E File Offset: 0x0008141E
	public void OnDestroy()
	{
		CosmeticsController.instance.RemoveWardrobeInstance(this);
	}

	// Token: 0x04001D4F RID: 7503
	public WardrobeItemButton[] wardrobeItemButtons;

	// Token: 0x04001D50 RID: 7504
	public HeadModel selfDoll;
}
