using System;

// Token: 0x020002DE RID: 734
public class TeleportOrientationHandler360 : TeleportOrientationHandler
{
	// Token: 0x060011B6 RID: 4534 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected override void InitializeTeleportDestination()
	{
	}

	// Token: 0x060011B7 RID: 4535 RVA: 0x000AC668 File Offset: 0x000AA868
	protected override void UpdateTeleportDestination()
	{
		base.LocomotionTeleport.OnUpdateTeleportDestination(this.AimData.TargetValid, this.AimData.Destination, null, null);
	}
}
