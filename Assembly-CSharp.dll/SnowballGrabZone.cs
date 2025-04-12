using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class SnowballGrabZone : HoldableObject
{
	// Token: 0x0600052D RID: 1325 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void DropItemCleanup()
	{
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x0007F5F4 File Offset: 0x0007D7F4
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		SnowballThrowable snowballThrowable;
		((grabbingHand == EquipmentInteractor.instance.leftHand) ? SnowballMaker.leftHandInstance : SnowballMaker.rightHandInstance).TryCreateSnowball(this.materialIndex, out snowballThrowable);
	}

	// Token: 0x04000604 RID: 1540
	[GorillaSoundLookup]
	public int materialIndex;
}
