using System;

// Token: 0x020002EB RID: 747
public class TeleportTransitionInstant : TeleportTransition
{
	// Token: 0x060011F0 RID: 4592 RVA: 0x00055056 File Offset: 0x00053256
	protected override void LocomotionTeleportOnEnterStateTeleporting()
	{
		base.LocomotionTeleport.DoTeleport();
	}
}
