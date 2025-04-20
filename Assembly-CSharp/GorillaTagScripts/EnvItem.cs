using System;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009D6 RID: 2518
	public class EnvItem : MonoBehaviour, IPunInstantiateMagicCallback
	{
		// Token: 0x06003E8C RID: 16012 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnEnable()
		{
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnDisable()
		{
		}

		// Token: 0x06003E8E RID: 16014 RVA: 0x00164488 File Offset: 0x00162688
		public void OnPhotonInstantiate(PhotonMessageInfo info)
		{
			object[] instantiationData = info.photonView.InstantiationData;
			this.spawnedByPhotonViewId = (int)instantiationData[0];
		}

		// Token: 0x04003FAC RID: 16300
		public int spawnedByPhotonViewId;
	}
}
