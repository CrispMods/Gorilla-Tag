using System;

// Token: 0x020002E8 RID: 744
public abstract class TeleportTransition : TeleportSupport
{
	// Token: 0x060011E0 RID: 4576 RVA: 0x00054B14 File Offset: 0x00052D14
	protected override void AddEventHandlers()
	{
		base.LocomotionTeleport.EnterStateTeleporting += this.LocomotionTeleportOnEnterStateTeleporting;
		base.AddEventHandlers();
	}

	// Token: 0x060011E1 RID: 4577 RVA: 0x00054B34 File Offset: 0x00052D34
	protected override void RemoveEventHandlers()
	{
		base.LocomotionTeleport.EnterStateTeleporting -= this.LocomotionTeleportOnEnterStateTeleporting;
		base.RemoveEventHandlers();
	}

	// Token: 0x060011E2 RID: 4578
	protected abstract void LocomotionTeleportOnEnterStateTeleporting();
}
