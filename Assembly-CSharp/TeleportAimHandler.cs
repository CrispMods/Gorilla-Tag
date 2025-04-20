using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002DA RID: 730
public abstract class TeleportAimHandler : TeleportSupport
{
	// Token: 0x060011B8 RID: 4536 RVA: 0x0003C03A File Offset: 0x0003A23A
	protected override void OnEnable()
	{
		base.OnEnable();
		base.LocomotionTeleport.AimHandler = this;
	}

	// Token: 0x060011B9 RID: 4537 RVA: 0x0003C04E File Offset: 0x0003A24E
	protected override void OnDisable()
	{
		if (base.LocomotionTeleport.AimHandler == this)
		{
			base.LocomotionTeleport.AimHandler = null;
		}
		base.OnDisable();
	}

	// Token: 0x060011BA RID: 4538
	public abstract void GetPoints(List<Vector3> points);
}
