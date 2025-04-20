using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200054C RID: 1356
public class PhotonViewCache : MonoBehaviour, IPunInstantiateMagicCallback
{
	// Token: 0x17000353 RID: 851
	// (get) Token: 0x060020EC RID: 8428 RVA: 0x000466BB File Offset: 0x000448BB
	// (set) Token: 0x060020ED RID: 8429 RVA: 0x000466C3 File Offset: 0x000448C3
	public bool Initialized { get; private set; }

	// Token: 0x060020EE RID: 8430 RVA: 0x00030607 File Offset: 0x0002E807
	void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
	{
	}

	// Token: 0x040024DE RID: 9438
	private PhotonView[] m_photonViews;

	// Token: 0x040024DF RID: 9439
	[SerializeField]
	private bool m_isRoomObject;
}
