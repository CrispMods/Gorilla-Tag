using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002D4 RID: 724
public abstract class TeleportInputHandler : TeleportSupport
{
	// Token: 0x06001186 RID: 4486 RVA: 0x00053A75 File Offset: 0x00051C75
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

	// Token: 0x06001187 RID: 4487 RVA: 0x00053AA1 File Offset: 0x00051CA1
	protected override void AddEventHandlers()
	{
		base.LocomotionTeleport.InputHandler = this;
		base.AddEventHandlers();
		base.LocomotionTeleport.EnterStateReady += this._startReadyAction;
		base.LocomotionTeleport.EnterStateAim += this._startAimAction;
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x00053AD8 File Offset: 0x00051CD8
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

	// Token: 0x06001189 RID: 4489 RVA: 0x00053B2C File Offset: 0x00051D2C
	private IEnumerator TeleportReadyCoroutine()
	{
		while (this.GetIntention() != LocomotionTeleport.TeleportIntentions.Aim)
		{
			yield return null;
		}
		base.LocomotionTeleport.CurrentIntention = LocomotionTeleport.TeleportIntentions.Aim;
		yield break;
	}

	// Token: 0x0600118A RID: 4490 RVA: 0x00053B3B File Offset: 0x00051D3B
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

	// Token: 0x0600118B RID: 4491
	public abstract LocomotionTeleport.TeleportIntentions GetIntention();

	// Token: 0x0600118C RID: 4492
	public abstract void GetAimData(out Ray aimRay);

	// Token: 0x0400136F RID: 4975
	private readonly Action _startReadyAction;

	// Token: 0x04001370 RID: 4976
	private readonly Action _startAimAction;
}
