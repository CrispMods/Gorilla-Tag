using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005EC RID: 1516
public class TappableSystem : GTSystem<Tappable>
{
	// Token: 0x060025C1 RID: 9665 RVA: 0x000BA9B8 File Offset: 0x000B8BB8
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
