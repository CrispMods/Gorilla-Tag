using System;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009B3 RID: 2483
	public class EnvItem : MonoBehaviour, IPunInstantiateMagicCallback
	{
		// Token: 0x06003D80 RID: 15744 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnEnable()
		{
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnDisable()
		{
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x0015E4A0 File Offset: 0x0015C6A0
		public void OnPhotonInstantiate(PhotonMessageInfo info)
		{
			object[] instantiationData = info.photonView.InstantiationData;
			this.spawnedByPhotonViewId = (int)instantiationData[0];
		}

		// Token: 0x04003EE4 RID: 16100
		public int spawnedByPhotonViewId;
	}
}
