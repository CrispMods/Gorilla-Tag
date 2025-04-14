using System;
using UnityEngine;

// Token: 0x020002E6 RID: 742
public class TeleportTargetHandlerNode : TeleportTargetHandler
{
	// Token: 0x060011DC RID: 4572 RVA: 0x000549B0 File Offset: 0x00052BB0
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

	// Token: 0x040013B7 RID: 5047
	[Tooltip("When checking line of sight to the destination, add this value to the vertical offset for targeting collision checks.")]
	public float LOSOffset = 1f;

	// Token: 0x040013B8 RID: 5048
	[Tooltip("Teleport logic will only work with TeleportPoint components that exist in the layers specified by this mask.")]
	public LayerMask TeleportLayerMask;
}
