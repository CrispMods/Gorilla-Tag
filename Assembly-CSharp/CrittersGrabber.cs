using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class CrittersGrabber : CrittersActor
{
	// Token: 0x0600017D RID: 381 RVA: 0x000314E6 File Offset: 0x0002F6E6
	public override void ProcessRemote()
	{
		if (this.rigPlayerId == PhotonNetwork.LocalPlayer.ActorNumber)
		{
			this.UpdateAverageSpeed();
		}
	}

	// Token: 0x0600017E RID: 382 RVA: 0x00031500 File Offset: 0x0002F700
	public override bool ProcessLocal()
	{
		if (this.rigPlayerId == PhotonNetwork.LocalPlayer.ActorNumber)
		{
			this.UpdateAverageSpeed();
		}
		return base.ProcessLocal();
	}

	// Token: 0x040001D6 RID: 470
	public Transform grabPosition;

	// Token: 0x040001D7 RID: 471
	public bool grabbing;

	// Token: 0x040001D8 RID: 472
	public float grabDistance;

	// Token: 0x040001D9 RID: 473
	public List<CrittersActor> grabbedActors = new List<CrittersActor>();

	// Token: 0x040001DA RID: 474
	public bool isLeft;
}
