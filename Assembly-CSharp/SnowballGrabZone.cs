using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class SnowballGrabZone : HoldableObject
{
	// Token: 0x0600052B RID: 1323 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void DropItemCleanup()
	{
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x0001ED7C File Offset: 0x0001CF7C
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		SnowballThrowable snowballThrowable;
		((grabbingHand == EquipmentInteractor.instance.leftHand) ? SnowballMaker.leftHandInstance : SnowballMaker.rightHandInstance).TryCreateSnowball(this.materialIndex, out snowballThrowable);
	}

	// Token: 0x04000603 RID: 1539
	[GorillaSoundLookup]
	public int materialIndex;
}
