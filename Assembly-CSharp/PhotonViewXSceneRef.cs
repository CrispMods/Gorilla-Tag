using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000217 RID: 535
public class PhotonViewXSceneRef : MonoBehaviour
{
	// Token: 0x17000133 RID: 307
	// (get) Token: 0x06000C69 RID: 3177 RVA: 0x00042144 File Offset: 0x00040344
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

	// Token: 0x04000FCA RID: 4042
	[SerializeField]
	private XSceneRef reference;
}
