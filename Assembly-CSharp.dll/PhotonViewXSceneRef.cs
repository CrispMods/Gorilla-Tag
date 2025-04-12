using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000217 RID: 535
public class PhotonViewXSceneRef : MonoBehaviour
{
	// Token: 0x17000133 RID: 307
	// (get) Token: 0x06000C6B RID: 3179 RVA: 0x0009DBE4 File Offset: 0x0009BDE4
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

	// Token: 0x04000FCB RID: 4043
	[SerializeField]
	private XSceneRef reference;
}
