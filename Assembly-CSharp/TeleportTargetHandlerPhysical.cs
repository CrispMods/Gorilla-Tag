using System;
using UnityEngine;

// Token: 0x020002F2 RID: 754
public class TeleportTargetHandlerPhysical : TeleportTargetHandler
{
	// Token: 0x0600122A RID: 4650 RVA: 0x000AF49C File Offset: 0x000AD69C
	protected override bool ConsiderTeleport(Vector3 start, ref Vector3 end)
	{
		if (base.LocomotionTeleport.AimCollisionTest(start, end, this.AimCollisionLayerMask, out this.AimData.TargetHitInfo))
		{
			Vector3 normalized = (end - start).normalized;
			end = start + normalized * this.AimData.TargetHitInfo.distance;
			return true;
		}
		return false;
	}
}
