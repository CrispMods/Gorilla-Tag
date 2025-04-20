using System;

// Token: 0x020002F6 RID: 758
public class TeleportTransitionInstant : TeleportTransition
{
	// Token: 0x06001239 RID: 4665 RVA: 0x0003C66C File Offset: 0x0003A86C
	protected override void LocomotionTeleportOnEnterStateTeleporting()
	{
		base.LocomotionTeleport.DoTeleport();
	}
}
