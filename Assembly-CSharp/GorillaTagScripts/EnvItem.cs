using System;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009B0 RID: 2480
	public class EnvItem : MonoBehaviour, IPunInstantiateMagicCallback
	{
		// Token: 0x06003D74 RID: 15732 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnEnable()
		{
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnDisable()
		{
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x0012261C File Offset: 0x0012081C
		public void OnPhotonInstantiate(PhotonMessageInfo info)
		{
			object[] instantiationData = info.photonView.InstantiationData;
			this.spawnedByPhotonViewId = (int)instantiationData[0];
		}

		// Token: 0x04003ED2 RID: 16082
		public int spawnedByPhotonViewId;
	}
}
