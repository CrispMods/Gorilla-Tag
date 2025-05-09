﻿using System;
using UnityEngine;

// Token: 0x020002F1 RID: 753
public class TeleportTargetHandlerNode : TeleportTargetHandler
{
	// Token: 0x06001228 RID: 4648 RVA: 0x000AF564 File Offset: 0x000AD764
	protected override bool ConsiderTeleport(Vector3 start, ref Vector3 end)
	{
		if (!base.LocomotionTeleport.AimCollisionTest(start, end, this.AimCollisionLayerMask | this.TeleportLayerMask, out this.AimData.TargetHitInfo))
		{
			return false;
		}
		TeleportPoint component = this.AimData.TargetHitInfo.collider.gameObject.GetComponent<TeleportPoint>();
		if (component == null)
		{
			return false;
		}
		Vector3 position = component.destTransform.position;
		Vector3 end2 = new Vector3(position.x, position.y + this.LOSOffset, position.z);
		if (base.LocomotionTeleport.AimCollisionTest(start, end2, this.AimCollisionLayerMask & ~this.TeleportLayerMask, out this.AimData.TargetHitInfo))
		{
			return false;
		}
		end = position;
		return true;
	}

	// Token: 0x040013FF RID: 5119
	[Tooltip("When checking line of sight to the destination, add this value to the vertical offset for targeting collision checks.")]
	public float LOSOffset = 1f;

	// Token: 0x04001400 RID: 5120
	[Tooltip("Teleport logic will only work with TeleportPoint components that exist in the layers specified by this mask.")]
	public LayerMask TeleportLayerMask;
}
