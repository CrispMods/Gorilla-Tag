using System;

// Token: 0x020002DE RID: 734
public class TeleportOrientationHandler360 : TeleportOrientationHandler
{
	// Token: 0x060011B3 RID: 4531 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void InitializeTeleportDestination()
	{
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x0005418C File Offset: 0x0005238C
	protected override void UpdateTeleportDestination()
	{
		base.LocomotionTeleport.OnUpdateTeleportDestination(this.AimData.TargetValid, this.AimData.Destination, null, null);
	}
}
