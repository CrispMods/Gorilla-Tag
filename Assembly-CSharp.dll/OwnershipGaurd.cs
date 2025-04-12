using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007AB RID: 1963
internal class OwnershipGaurd : MonoBehaviour
{
	// Token: 0x06003071 RID: 12401 RVA: 0x0004F489 File Offset: 0x0004D689
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

	// Token: 0x06003072 RID: 12402 RVA: 0x0004F4B3 File Offset: 0x0004D6B3
	private void OnDestroy()
	{
		if (this.NetViews == null)
		{
			return;
		}
		OwnershipGaurdHandler.RemoveViews(this.NetViews);
	}

	// Token: 0x04003466 RID: 13414
	[SerializeField]
	private PhotonView[] NetViews;

	// Token: 0x04003467 RID: 13415
	[SerializeField]
	private bool autoRegisterAll = true;
}
