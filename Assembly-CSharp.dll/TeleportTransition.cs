using System;

// Token: 0x020002E8 RID: 744
public abstract class TeleportTransition : TeleportSupport
{
	// Token: 0x060011E3 RID: 4579 RVA: 0x0003B337 File Offset: 0x00039537
	protected override void AddEventHandlers()
	{
		base.LocomotionTeleport.EnterStateTeleporting += this.LocomotionTeleportOnEnterStateTeleporting;
		base.AddEventHandlers();
	}

	// Token: 0x060011E4 RID: 4580 RVA: 0x0003B357 File Offset: 0x00039557
	protected override void RemoveEventHandlers()
	{
		base.LocomotionTeleport.EnterStateTeleporting -= this.LocomotionTeleportOnEnterStateTeleporting;
		base.RemoveEventHandlers();
	}

	// Token: 0x060011E5 RID: 4581
	protected abstract void LocomotionTeleportOnEnterStateTeleporting();
}
