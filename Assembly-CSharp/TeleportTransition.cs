using System;

// Token: 0x020002F3 RID: 755
public abstract class TeleportTransition : TeleportSupport
{
	// Token: 0x0600122C RID: 4652 RVA: 0x0003C5F7 File Offset: 0x0003A7F7
	protected override void AddEventHandlers()
	{
		base.LocomotionTeleport.EnterStateTeleporting += this.LocomotionTeleportOnEnterStateTeleporting;
		base.AddEventHandlers();
	}

	// Token: 0x0600122D RID: 4653 RVA: 0x0003C617 File Offset: 0x0003A817
	protected override void RemoveEventHandlers()
	{
		base.LocomotionTeleport.EnterStateTeleporting -= this.LocomotionTeleportOnEnterStateTeleporting;
		base.RemoveEventHandlers();
	}

	// Token: 0x0600122E RID: 4654
	protected abstract void LocomotionTeleportOnEnterStateTeleporting();
}
