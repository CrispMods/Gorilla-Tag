﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class CrittersGrabber : CrittersActor
{
	// Token: 0x06000165 RID: 357 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void ProcessRemote()
	{
	}

	// Token: 0x040001B1 RID: 433
	public Transform grabPosition;

	// Token: 0x040001B2 RID: 434
	public bool grabbing;

	// Token: 0x040001B3 RID: 435
	public float grabDistance;

	// Token: 0x040001B4 RID: 436
	public List<CrittersActor> grabbedActors = new List<CrittersActor>();

	// Token: 0x040001B5 RID: 437
	public CrittersActor singleGrabbedActor;

	// Token: 0x040001B6 RID: 438
	public int netPlayerId;

	// Token: 0x040001B7 RID: 439
	public bool isLeft;
}
