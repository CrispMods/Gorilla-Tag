using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000432 RID: 1074
public class WardrobeInstance : MonoBehaviour
{
	// Token: 0x06001A85 RID: 6789 RVA: 0x00082E8B File Offset: 0x0008108B
	public void Start()
	{
		CosmeticsController.instance.AddWardrobeInstance(this);
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x00082E9A File Offset: 0x0008109A
	public void OnDestroy()
	{
		CosmeticsController.instance.RemoveWardrobeInstance(this);
	}

	// Token: 0x04001D4E RID: 7502
	public WardrobeItemButton[] wardrobeItemButtons;

	// Token: 0x04001D4F RID: 7503
	public HeadModel selfDoll;
}
