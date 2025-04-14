using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200050A RID: 1290
public abstract class CosmeticCritterHoldable : MonoBehaviour
{
	// Token: 0x17000330 RID: 816
	// (get) Token: 0x06001F59 RID: 8025 RVA: 0x0009E6B3 File Offset: 0x0009C8B3
	// (set) Token: 0x06001F5A RID: 8026 RVA: 0x0009E6BB File Offset: 0x0009C8BB
	public int playerID { get; private set; }

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x06001F5B RID: 8027 RVA: 0x0009E6C4 File Offset: 0x0009C8C4
	// (set) Token: 0x06001F5C RID: 8028 RVA: 0x0009E6CC File Offset: 0x0009C8CC
	public bool isLocal { get; private set; }

	// Token: 0x06001F5D RID: 8029 RVA: 0x0009E6D8 File Offset: 0x0009C8D8
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
