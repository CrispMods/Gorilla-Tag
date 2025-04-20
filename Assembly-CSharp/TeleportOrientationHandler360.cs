using System;

// Token: 0x020002E9 RID: 745
public class TeleportOrientationHandler360 : TeleportOrientationHandler
{
	// Token: 0x060011FF RID: 4607 RVA: 0x00030607 File Offset: 0x0002E807
	protected override void InitializeTeleportDestination()
	{
	}

	// Token: 0x06001200 RID: 4608 RVA: 0x000AEF00 File Offset: 0x000AD100
	protected override void UpdateTeleportDestination()
	{
		base.LocomotionTeleport.OnUpdateTeleportDestination(this.AimData.TargetValid, this.AimData.Destination, null, null);
	}
}
