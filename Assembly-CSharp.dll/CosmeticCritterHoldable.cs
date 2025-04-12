using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200050A RID: 1290
public abstract class CosmeticCritterHoldable : MonoBehaviour
{
	// Token: 0x17000330 RID: 816
	// (get) Token: 0x06001F59 RID: 8025 RVA: 0x0004445B File Offset: 0x0004265B
	// (set) Token: 0x06001F5A RID: 8026 RVA: 0x00044463 File Offset: 0x00042663
	public int playerID { get; private set; }

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x06001F5B RID: 8027 RVA: 0x0004446C File Offset: 0x0004266C
	// (set) Token: 0x06001F5C RID: 8028 RVA: 0x00044474 File Offset: 0x00042674
	public bool isLocal { get; private set; }

	// Token: 0x06001F5D RID: 8029 RVA: 0x000ED354 File Offset: 0x000EB554
	protected void TrySetID()
	{
		PlayFabAuthenticator instance = PlayFabAuthenticator.instance;
		if (instance != null && this.playerID == 0)
		{
			this.playerID = instance.GetPlayFabPlayerId().GetHashCode();
		}
		this.isLocal = this.transferrableObject.IsLocalObject();
	}

	// Token: 0x0400232F RID: 9007
	[SerializeField]
	protected TransferrableObject transferrableObject;
}
