using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002E6 RID: 742
public abstract class TeleportOrientationHandler : TeleportSupport
{
	// Token: 0x060011F0 RID: 4592 RVA: 0x0003C329 File Offset: 0x0003A529
	protected TeleportOrientationHandler()
	{
		this._updateOrientationAction = delegate()
		{
			base.StartCoroutine(this.UpdateOrientationCoroutine());
		};
		this._updateAimDataAction = new Action<LocomotionTeleport.AimData>(this.UpdateAimData);
	}

	// Token: 0x060011F1 RID: 4593 RVA: 0x0003C355 File Offset: 0x0003A555
	private void UpdateAimData(LocomotionTeleport.AimData aimData)
	{
		this.AimData = aimData;
	}

	// Token: 0x060011F2 RID: 4594 RVA: 0x0003C35E File Offset: 0x0003A55E
	protected override void AddEventHandlers()
	{
		base.AddEventHandlers();
		base.LocomotionTeleport.EnterStateAim += this._updateOrientationAction;
		base.LocomotionTeleport.UpdateAimData += this._updateAimDataAction;
	}

	// Token: 0x060011F3 RID: 4595 RVA: 0x0003C388 File Offset: 0x0003A588
	protected override void RemoveEventHandlers()
	{
		base.RemoveEventHandlers();
		base.LocomotionTeleport.EnterStateAim -= this._updateOrientationAction;
		base.LocomotionTeleport.UpdateAimData -= this._updateAimDataAction;
	}

	// Token: 0x060011F4 RID: 4596 RVA: 0x0003C3B2 File Offset: 0x0003A5B2
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

	// Token: 0x060011F5 RID: 4597
	protected abstract void InitializeTeleportDestination();

	// Token: 0x060011F6 RID: 4598
	protected abstract void UpdateTeleportDestination();

	// Token: 0x060011F7 RID: 4599 RVA: 0x0003C3C1 File Offset: 0x0003A5C1
	protected Quaternion GetLandingOrientation(TeleportOrientationHandler.OrientationModes mode, Quaternion rotation)
	{
		if (mode != TeleportOrientationHandler.OrientationModes.HeadRelative)
		{
			return rotation * Quaternion.Euler(0f, -base.LocomotionTeleport.LocomotionController.CameraRig.trackingSpace.localEulerAngles.y, 0f);
		}
		return rotation;
	}

	// Token: 0x040013DA RID: 5082
	private readonly Action _updateOrientationAction;

	// Token: 0x040013DB RID: 5083
	private readonly Action<LocomotionTeleport.AimData> _updateAimDataAction;

	// Token: 0x040013DC RID: 5084
	protected LocomotionTeleport.AimData AimData;

	// Token: 0x020002E7 RID: 743
	public enum OrientationModes
	{
		// Token: 0x040013DE RID: 5086
		HeadRelative,
		// Token: 0x040013DF RID: 5087
		ForwardFacing
	}
}
