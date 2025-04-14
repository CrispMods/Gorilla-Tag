using System;
using UnityEngine;

// Token: 0x020002E7 RID: 743
public class TeleportTargetHandlerPhysical : TeleportTargetHandler
{
	// Token: 0x060011DE RID: 4574 RVA: 0x00054AA0 File Offset: 0x00052CA0
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
