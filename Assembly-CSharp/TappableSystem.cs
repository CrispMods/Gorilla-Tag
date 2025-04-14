using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005EB RID: 1515
public class TappableSystem : GTSystem<Tappable>
{
	// Token: 0x060025B9 RID: 9657 RVA: 0x000BA538 File Offset: 0x000B8738
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
