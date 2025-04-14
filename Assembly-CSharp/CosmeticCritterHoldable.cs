using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200050A RID: 1290
public abstract class CosmeticCritterHoldable : MonoBehaviour
{
	// Token: 0x17000330 RID: 816
	// (get) Token: 0x06001F56 RID: 8022 RVA: 0x0009E32F File Offset: 0x0009C52F
	// (set) Token: 0x06001F57 RID: 8023 RVA: 0x0009E337 File Offset: 0x0009C537
	public int playerID { get; private set; }

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x06001F58 RID: 8024 RVA: 0x0009E340 File Offset: 0x0009C540
	// (set) Token: 0x06001F59 RID: 8025 RVA: 0x0009E348 File Offset: 0x0009C548
	public bool isLocal { get; private set; }

	// Token: 0x06001F5A RID: 8026 RVA: 0x0009E354 File Offset: 0x0009C554
	protected void TrySetID()
	{
		PlayFabAuthenticator instance = PlayFabAuthenticator.instance;
		if (instance != null && this.playerID == 0)
		{
			this.playerID = instance.GetPlayFabPlayerId().GetHashCode();
		}
		this.isLocal = this.transferrableObject.IsLocalObject();
	}

	// Token: 0x0400232E RID: 9006
	[SerializeField]
	protected TransferrableObject transferrableObject;
}
