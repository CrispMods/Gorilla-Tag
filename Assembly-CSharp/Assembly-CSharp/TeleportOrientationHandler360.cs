using System;

// Token: 0x020002DE RID: 734
public class TeleportOrientationHandler360 : TeleportOrientationHandler
{
	// Token: 0x060011B6 RID: 4534 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void InitializeTeleportDestination()
	{
	}

	// Token: 0x060011B7 RID: 4535 RVA: 0x00054510 File Offset: 0x00052710
	protected override void UpdateTeleportDestination()
	{
		base.LocomotionTeleport.OnUpdateTeleportDestination(this.AimData.TargetValid, this.AimData.Destination, null, null);
	}
}
