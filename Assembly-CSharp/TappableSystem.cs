using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005F9 RID: 1529
public class TappableSystem : GTSystem<Tappable>
{
	// Token: 0x0600261B RID: 9755 RVA: 0x00107F84 File Offset: 0x00106184
	[PunRPC]
	public void SendOnTapRPC(int key, float tapStrength, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "SendOnTapRPC");
		if (key < 0 || key >= this._instances.Count || !float.IsFinite(tapStrength))
		{
			return;
		}
		tapStrength = Mathf.Clamp(tapStrength, 0f, 1f);
		this._instances[key].OnTapLocal(tapStrength, Time.time, new PhotonMessageInfoWrapped(info));
	}
}
