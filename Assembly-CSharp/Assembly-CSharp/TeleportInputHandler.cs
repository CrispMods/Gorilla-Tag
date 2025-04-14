using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002D4 RID: 724
public abstract class TeleportInputHandler : TeleportSupport
{
	// Token: 0x06001189 RID: 4489 RVA: 0x00053DF9 File Offset: 0x00051FF9
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

	// Token: 0x0600118A RID: 4490 RVA: 0x00053E25 File Offset: 0x00052025
	protected override void AddEventHandlers()
	{
		base.LocomotionTeleport.InputHandler = this;
		base.AddEventHandlers();
		base.LocomotionTeleport.EnterStateReady += this._startReadyAction;
		base.LocomotionTeleport.EnterStateAim += this._startAimAction;
	}

	// Token: 0x0600118B RID: 4491 RVA: 0x00053E5C File Offset: 0x0005205C
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

	// Token: 0x0600118C RID: 4492 RVA: 0x00053EB0 File Offset: 0x000520B0
	private IEnumerator TeleportReadyCoroutine()
	{
		while (this.GetIntention() != LocomotionTeleport.TeleportIntentions.Aim)
		{
			yield return null;
		}
		base.LocomotionTeleport.CurrentIntention = LocomotionTeleport.TeleportIntentions.Aim;
		yield break;
	}

	// Token: 0x0600118D RID: 4493 RVA: 0x00053EBF File Offset: 0x000520BF
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

	// Token: 0x0600118E RID: 4494
	public abstract LocomotionTeleport.TeleportIntentions GetIntention();

	// Token: 0x0600118F RID: 4495
	public abstract void GetAimData(out Ray aimRay);

	// Token: 0x04001370 RID: 4976
	private readonly Action _startReadyAction;

	// Token: 0x04001371 RID: 4977
	private readonly Action _startAimAction;
}
