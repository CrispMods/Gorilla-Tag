using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002DB RID: 731
public abstract class TeleportOrientationHandler : TeleportSupport
{
	// Token: 0x060011A4 RID: 4516 RVA: 0x00054018 File Offset: 0x00052218
	protected TeleportOrientationHandler()
	{
		this._updateOrientationAction = delegate()
		{
			base.StartCoroutine(this.UpdateOrientationCoroutine());
		};
		this._updateAimDataAction = new Action<LocomotionTeleport.AimData>(this.UpdateAimData);
	}

	// Token: 0x060011A5 RID: 4517 RVA: 0x00054044 File Offset: 0x00052244
	private void UpdateAimData(LocomotionTeleport.AimData aimData)
	{
		this.AimData = aimData;
	}

	// Token: 0x060011A6 RID: 4518 RVA: 0x0005404D File Offset: 0x0005224D
	protected override void AddEventHandlers()
	{
		base.AddEventHandlers();
		base.LocomotionTeleport.EnterStateAim += this._updateOrientationAction;
		base.LocomotionTeleport.UpdateAimData += this._updateAimDataAction;
	}

	// Token: 0x060011A7 RID: 4519 RVA: 0x00054077 File Offset: 0x00052277
	protected override void RemoveEventHandlers()
	{
		base.RemoveEventHandlers();
		base.LocomotionTeleport.EnterStateAim -= this._updateOrientationAction;
		base.LocomotionTeleport.UpdateAimData -= this._updateAimDataAction;
	}

	// Token: 0x060011A8 RID: 4520 RVA: 0x000540A1 File Offset: 0x000522A1
	private IEnumerator UpdateOrientationCoroutine()
	{
		this.InitializeTeleportDestination();
		while (base.LocomotionTeleport.CurrentState == LocomotionTeleport.States.Aim || base.LocomotionTeleport.CurrentState == LocomotionTeleport.States.PreTeleport)
		{
			if (this.AimData != null)
			{
				this.UpdateTeleportDestination();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060011A9 RID: 4521
	protected abstract void InitializeTeleportDestination();

	// Token: 0x060011AA RID: 4522
	protected abstract void UpdateTeleportDestination();

	// Token: 0x060011AB RID: 4523 RVA: 0x000540B0 File Offset: 0x000522B0
	protected Quaternion GetLandingOrientation(TeleportOrientationHandler.OrientationModes mode, Quaternion rotation)
	{
		if (mode != TeleportOrientationHandler.OrientationModes.HeadRelative)
		{
			return rotation * Quaternion.Euler(0f, -base.LocomotionTeleport.LocomotionController.CameraRig.trackingSpace.localEulerAngles.y, 0f);
		}
		return rotation;
	}

	// Token: 0x04001392 RID: 5010
	private readonly Action _updateOrientationAction;

	// Token: 0x04001393 RID: 5011
	private readonly Action<LocomotionTeleport.AimData> _updateAimDataAction;

	// Token: 0x04001394 RID: 5012
	protected LocomotionTeleport.AimData AimData;

	// Token: 0x020002DC RID: 732
	public enum OrientationModes
	{
		// Token: 0x04001396 RID: 5014
		HeadRelative,
		// Token: 0x04001397 RID: 5015
		ForwardFacing
	}
}
