using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002DF RID: 735
public abstract class TeleportInputHandler : TeleportSupport
{
	// Token: 0x060011D2 RID: 4562 RVA: 0x0003C244 File Offset: 0x0003A444
	protected TeleportInputHandler()
	{
		this._startReadyAction = delegate()
		{
			base.StartCoroutine(this.TeleportReadyCoroutine());
		};
		this._startAimAction = delegate()
		{
			base.StartCoroutine(this.TeleportAimCoroutine());
		};
	}

	// Token: 0x060011D3 RID: 4563 RVA: 0x0003C270 File Offset: 0x0003A470
	protected override void AddEventHandlers()
	{
		base.LocomotionTeleport.InputHandler = this;
		base.AddEventHandlers();
		base.LocomotionTeleport.EnterStateReady += this._startReadyAction;
		base.LocomotionTeleport.EnterStateAim += this._startAimAction;
	}

	// Token: 0x060011D4 RID: 4564 RVA: 0x000AE9CC File Offset: 0x000ACBCC
	protected override void RemoveEventHandlers()
	{
		if (base.LocomotionTeleport.InputHandler == this)
		{
			base.LocomotionTeleport.InputHandler = null;
		}
		base.LocomotionTeleport.EnterStateReady -= this._startReadyAction;
		base.LocomotionTeleport.EnterStateAim -= this._startAimAction;
		base.RemoveEventHandlers();
	}

	// Token: 0x060011D5 RID: 4565 RVA: 0x0003C2A6 File Offset: 0x0003A4A6
	private IEnumerator TeleportReadyCoroutine()
	{
		while (this.GetIntention() != LocomotionTeleport.TeleportIntentions.Aim)
		{
			yield return null;
		}
		base.LocomotionTeleport.CurrentIntention = LocomotionTeleport.TeleportIntentions.Aim;
		yield break;
	}

	// Token: 0x060011D6 RID: 4566 RVA: 0x0003C2B5 File Offset: 0x0003A4B5
	private IEnumerator TeleportAimCoroutine()
	{
		LocomotionTeleport.TeleportIntentions intention = this.GetIntention();
		while (intention == LocomotionTeleport.TeleportIntentions.Aim || intention == LocomotionTeleport.TeleportIntentions.PreTeleport)
		{
			base.LocomotionTeleport.CurrentIntention = intention;
			yield return null;
			intention = this.GetIntention();
		}
		base.LocomotionTeleport.CurrentIntention = intention;
		yield break;
	}

	// Token: 0x060011D7 RID: 4567
	public abstract LocomotionTeleport.TeleportIntentions GetIntention();

	// Token: 0x060011D8 RID: 4568
	public abstract void GetAimData(out Ray aimRay);

	// Token: 0x040013B7 RID: 5047
	private readonly Action _startReadyAction;

	// Token: 0x040013B8 RID: 5048
	private readonly Action _startAimAction;
}
