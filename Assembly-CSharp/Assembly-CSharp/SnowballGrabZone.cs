using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class SnowballGrabZone : HoldableObject
{
	// Token: 0x0600052D RID: 1325 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void DropItemCleanup()
	{
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x0001F0A0 File Offset: 0x0001D2A0
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		SnowballThrowable snowballThrowable;
		((grabbingHand == EquipmentInteractor.instance.leftHand) ? SnowballMaker.leftHandInstance : SnowballMaker.rightHandInstance).TryCreateSnowball(this.materialIndex, out snowballThrowable);
	}

	// Token: 0x04000604 RID: 1540
	[GorillaSoundLookup]
	public int materialIndex;
}
