using System;
using UnityEngine;

// Token: 0x020002E7 RID: 743
public class TeleportTargetHandlerPhysical : TeleportTargetHandler
{
	// Token: 0x060011E1 RID: 4577 RVA: 0x000ACC04 File Offset: 0x000AAE04
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
