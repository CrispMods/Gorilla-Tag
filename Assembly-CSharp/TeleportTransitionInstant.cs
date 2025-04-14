using System;

// Token: 0x020002EB RID: 747
public class TeleportTransitionInstant : TeleportTransition
{
	// Token: 0x060011ED RID: 4589 RVA: 0x00054CD2 File Offset: 0x00052ED2
	protected override void LocomotionTeleportOnEnterStateTeleporting()
	{
		base.LocomotionTeleport.DoTeleport();
	}
}
