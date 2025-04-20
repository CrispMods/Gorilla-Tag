using System;
using UnityEngine;

// Token: 0x02000517 RID: 1303
public abstract class CosmeticCritterHoldable : MonoBehaviour
{
	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06001FAF RID: 8111 RVA: 0x000457FA File Offset: 0x000439FA
	// (set) Token: 0x06001FB0 RID: 8112 RVA: 0x00045802 File Offset: 0x00043A02
	public int playerID { get; private set; }

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x06001FB1 RID: 8113 RVA: 0x0004580B File Offset: 0x00043A0B
	// (set) Token: 0x06001FB2 RID: 8114 RVA: 0x00045813 File Offset: 0x00043A13
	public bool isLocal { get; private set; }

	// Token: 0x06001FB3 RID: 8115 RVA: 0x000F0090 File Offset: 0x000EE290
	protected void TrySetID()
	{
		if (this.playerID == 0)
		{
			NetPlayer netPlayer = (this.transferrableObject.myOnlineRig != null) ? this.transferrableObject.myOnlineRig.creator : ((this.transferrableObject.myRig != null) ? (this.transferrableObject.myRig.creator ?? NetworkSystem.Instance.LocalPlayer) : null);
			if (netPlayer != null)
			{
				this.playerID = netPlayer.UserId.GetHashCode();
				this.isLocal = netPlayer.IsLocal;
			}
		}
	}

	// Token: 0x04002381 RID: 9089
	[SerializeField]
	protected TransferrableObject transferrableObject;
}
