using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200053F RID: 1343
public class PhotonViewCache : MonoBehaviour, IPunInstantiateMagicCallback
{
	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06002096 RID: 8342 RVA: 0x000A39F4 File Offset: 0x000A1BF4
	// (set) Token: 0x06002097 RID: 8343 RVA: 0x000A39FC File Offset: 0x000A1BFC
	public bool Initialized { get; private set; }

	// Token: 0x06002098 RID: 8344 RVA: 0x000023F4 File Offset: 0x000005F4
	void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
	{
	}

	// Token: 0x0400248C RID: 9356
	private PhotonView[] m_photonViews;

	// Token: 0x0400248D RID: 9357
	[SerializeField]
	private bool m_isRoomObject;
}
