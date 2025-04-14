using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002CF RID: 719
public abstract class TeleportAimHandler : TeleportSupport
{
	// Token: 0x0600116C RID: 4460 RVA: 0x00053554 File Offset: 0x00051754
	protected override void OnEnable()
	{
		base.OnEnable();
		base.LocomotionTeleport.AimHandler = this;
	}

	// Token: 0x0600116D RID: 4461 RVA: 0x00053568 File Offset: 0x00051768
	protected override void OnDisable()
	{
		if (base.LocomotionTeleport.AimHandler == this)
		{
			base.LocomotionTeleport.AimHandler = null;
		}
		base.OnDisable();
	}

	// Token: 0x0600116E RID: 4462
	public abstract void GetPoints(List<Vector3> points);
}
