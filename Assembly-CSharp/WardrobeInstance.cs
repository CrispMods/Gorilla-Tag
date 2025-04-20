using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200043E RID: 1086
public class WardrobeInstance : MonoBehaviour
{
	// Token: 0x06001AD9 RID: 6873 RVA: 0x000422DB File Offset: 0x000404DB
	public void Start()
	{
		CosmeticsController.instance.AddWardrobeInstance(this);
	}

	// Token: 0x06001ADA RID: 6874 RVA: 0x000422EA File Offset: 0x000404EA
	public void OnDestroy()
	{
		CosmeticsController.instance.RemoveWardrobeInstance(this);
	}

	// Token: 0x04001D9D RID: 7581
	public WardrobeItemButton[] wardrobeItemButtons;

	// Token: 0x04001D9E RID: 7582
	public HeadModel selfDoll;
}
