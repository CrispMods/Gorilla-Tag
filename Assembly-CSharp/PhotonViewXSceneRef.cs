using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000222 RID: 546
public class PhotonViewXSceneRef : MonoBehaviour
{
	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x000A0470 File Offset: 0x0009E670
	public PhotonView photonView
	{
		get
		{
			PhotonView result;
			if (this.reference.TryResolve<PhotonView>(out result))
			{
				return result;
			}
			return null;
		}
	}

	// Token: 0x04001010 RID: 4112
	[SerializeField]
	private XSceneRef reference;
}
