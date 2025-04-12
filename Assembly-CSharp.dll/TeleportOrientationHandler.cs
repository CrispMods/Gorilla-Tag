using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002DB RID: 731
public abstract class TeleportOrientationHandler : TeleportSupport
{
	// Token: 0x060011A7 RID: 4519 RVA: 0x0003B069 File Offset: 0x00039269
	protected TeleportOrientationHandler()
	{
		this._updateOrientationAction = delegate()
		{
			base.StartCoroutine(this.UpdateOrientationCoroutine());
		};
		this._updateAimDataAction = new Action<LocomotionTeleport.AimData>(this.UpdateAimData);
	}

	// Token: 0x060011A8 RID: 4520 RVA: 0x0003B095 File Offset: 0x00039295
	private void UpdateAimData(LocomotionTeleport.AimData aimData)
	{
		this.AimData = aimData;
	}

	// Token: 0x060011A9 RID: 4521 RVA: 0x0003B09E File Offset: 0x0003929E
	protected override void AddEventHandlers()
	{
		base.AddEventHandlers();
		base.LocomotionTeleport.EnterStateAim += this._updateOrientationAction;
		base.LocomotionTeleport.UpdateAimData += this._updateAimDataAction;
	}

	// Token: 0x060011AA RID: 4522 RVA: 0x0003B0C8 File Offset: 0x000392C8
	protected override void RemoveEventHandlers()
	{
		base.RemoveEventHandlers();
		base.LocomotionTeleport.EnterStateAim -= this._updateOrientationAction;
		base.LocomotionTeleport.UpdateAimData -= this._updateAimDataAction;
	}

	// Token: 0x060011AB RID: 4523 RVA: 0x0003B0F2 File Offset: 0x000392F2
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

	// Token: 0x060011AC RID: 4524
	protected abstract void InitializeTeleportDestination();

	// Token: 0x060011AD RID: 4525
	protected abstract void UpdateTeleportDestination();

	// Token: 0x060011AE RID: 4526 RVA: 0x0003B101 File Offset: 0x00039301
	protected Quaternion GetLandingOrientation(TeleportOrientationHandler.OrientationModes mode, Quaternion rotation)
	{
		if (mode != TeleportOrientationHandler.OrientationModes.HeadRelative)
		{
			return rotation * Quaternion.Euler(0f, -base.LocomotionTeleport.LocomotionController.CameraRig.trackingSpace.localEulerAngles.y, 0f);
		}
		return rotation;
	}

	// Token: 0x04001393 RID: 5011
	private readonly Action _updateOrientationAction;

	// Token: 0x04001394 RID: 5012
	private readonly Action<LocomotionTeleport.AimData> _updateAimDataAction;

	// Token: 0x04001395 RID: 5013
	protected LocomotionTeleport.AimData AimData;

	// Token: 0x020002DC RID: 732
	public enum OrientationModes
	{
		// Token: 0x04001397 RID: 5015
		HeadRelative,
		// Token: 0x04001398 RID: 5016
		ForwardFacing
	}
}
