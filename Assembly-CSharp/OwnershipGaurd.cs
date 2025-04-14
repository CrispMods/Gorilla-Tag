using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007AA RID: 1962
internal class OwnershipGaurd : MonoBehaviour
{
	// Token: 0x06003069 RID: 12393 RVA: 0x000E993B File Offset: 0x000E7B3B
	private void Start()
	{
		if (this.autoRegisterAll)
		{
			this.NetViews = base.GetComponents<PhotonView>();
		}
		if (this.NetViews == null)
		{
			return;
		}
		OwnershipGaurdHandler.RegisterViews(this.NetViews);
	}

	// Token: 0x0600306A RID: 12394 RVA: 0x000E9965 File Offset: 0x000E7B65
	private void OnDestroy()
	{
		if (this.NetViews == null)
		{
			return;
		}
		OwnershipGaurdHandler.RemoveViews(this.NetViews);
	}

	// Token: 0x04003460 RID: 13408
	[SerializeField]
	private PhotonView[] NetViews;

	// Token: 0x04003461 RID: 13409
	[SerializeField]
	private bool autoRegisterAll = true;
}
