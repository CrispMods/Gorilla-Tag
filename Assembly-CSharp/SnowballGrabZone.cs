using System;
using UnityEngine;

// Token: 0x020000D2 RID: 210
public class SnowballGrabZone : HoldableObject
{
	// Token: 0x0600056B RID: 1387 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x00030607 File Offset: 0x0002E807
	public override void DropItemCleanup()
	{
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x00081F24 File Offset: 0x00080124
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		SnowballThrowable snowballThrowable;
		((grabbingHand == EquipmentInteractor.instance.leftHand) ? SnowballMaker.leftHandInstance : SnowballMaker.rightHandInstance).TryCreateSnowball(this.materialIndex, out snowballThrowable);
	}

	// Token: 0x04000643 RID: 1603
	[GorillaSoundLookup]
	public int materialIndex;
}
