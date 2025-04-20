using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007C2 RID: 1986
internal class OwnershipGaurd : MonoBehaviour
{
	// Token: 0x0600311B RID: 12571 RVA: 0x0005088B File Offset: 0x0004EA8B
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

	// Token: 0x0600311C RID: 12572 RVA: 0x000508B5 File Offset: 0x0004EAB5
	private void OnDestroy()
	{
		if (this.NetViews == null)
		{
			return;
		}
		OwnershipGaurdHandler.RemoveViews(this.NetViews);
	}

	// Token: 0x0400350A RID: 13578
	[SerializeField]
	private PhotonView[] NetViews;

	// Token: 0x0400350B RID: 13579
	[SerializeField]
	private bool autoRegisterAll = true;
}
