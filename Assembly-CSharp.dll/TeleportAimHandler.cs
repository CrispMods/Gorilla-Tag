﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002CF RID: 719
public abstract class TeleportAimHandler : TeleportSupport
{
	// Token: 0x0600116F RID: 4463 RVA: 0x0003AD7A File Offset: 0x00038F7A
	protected override void OnEnable()
	{
		base.OnEnable();
		base.LocomotionTeleport.AimHandler = this;
	}

	// Token: 0x06001170 RID: 4464 RVA: 0x0003AD8E File Offset: 0x00038F8E
	protected override void OnDisable()
	{
		if (base.LocomotionTeleport.AimHandler == this)
		{
			base.LocomotionTeleport.AimHandler = null;
		}
		base.OnDisable();
	}

	// Token: 0x06001171 RID: 4465
	public abstract void GetPoints(List<Vector3> points);
}
