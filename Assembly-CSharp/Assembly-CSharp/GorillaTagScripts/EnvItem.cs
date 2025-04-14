using System;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009B3 RID: 2483
	public class EnvItem : MonoBehaviour, IPunInstantiateMagicCallback
	{
		// Token: 0x06003D80 RID: 15744 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnEnable()
		{
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnDisable()
		{
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x00122BE4 File Offset: 0x00120DE4
		public void OnPhotonInstantiate(PhotonMessageInfo info)
		{
			object[] instantiationData = info.photonView.InstantiationData;
			this.spawnedByPhotonViewId = (int)instantiationData[0];
		}

		// Token: 0x04003EE4 RID: 16100
		public int spawnedByPhotonViewId;
	}
}
