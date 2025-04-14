using System;
using UnityEngine;

// Token: 0x020002D7 RID: 727
public class TeleportInputHandlerHMD : TeleportInputHandler
{
	// Token: 0x170001FD RID: 509
	// (get) Token: 0x0600119E RID: 4510 RVA: 0x00053FEA File Offset: 0x000521EA
	// (set) Token: 0x0600119F RID: 4511 RVA: 0x00053FF2 File Offset: 0x000521F2
	public Transform Pointer { get; private set; }

	// Token: 0x060011A0 RID: 4512 RVA: 0x00053FFC File Offset: 0x000521FC
	public override LocomotionTeleport.TeleportIntentions GetIntention()
	{
		if (!base.isActiveAndEnabled)
		{
			return LocomotionTeleport.TeleportIntentions.None;
		}
		if (base.LocomotionTeleport.CurrentIntention == LocomotionTeleport.TeleportIntentions.Aim && OVRInput.GetDown(this.TeleportButton, OVRInput.Controller.Active))
		{
			if (!this.FastTeleport)
			{
				return LocomotionTeleport.TeleportIntentions.PreTeleport;
			}
			return LocomotionTeleport.TeleportIntentions.Teleport;
		}
		else if (base.LocomotionTeleport.CurrentIntention == LocomotionTeleport.TeleportIntentions.PreTeleport)
		{
			if (OVRInput.GetUp(this.TeleportButton, OVRInput.Controller.Active))
			{
				return LocomotionTeleport.TeleportIntentions.Teleport;
			}
			return LocomotionTeleport.TeleportIntentions.PreTeleport;
		}
		else
		{
			if (OVRInput.Get(this.AimButton, OVRInput.Controller.Active))
			{
				return LocomotionTeleport.TeleportIntentions.Aim;
			}
			if (this.AimButton == this.TeleportButton)
			{
				return LocomotionTeleport.TeleportIntentions.Teleport;
			}
			return LocomotionTeleport.TeleportIntentions.None;
		}
	}

	// Token: 0x060011A1 RID: 4513 RVA: 0x00054088 File Offset: 0x00052288
	public override void GetAimData(out Ray aimRay)
	{
		Transform centerEyeAnchor = base.LocomotionTeleport.LocomotionController.CameraRig.centerEyeAnchor;
		aimRay = new Ray(centerEyeAnchor.position, centerEyeAnchor.forward);
	}

	// Token: 0x04001379 RID: 4985
	[Tooltip("The button used to begin aiming for a teleport.")]
	public OVRInput.RawButton AimButton;

	// Token: 0x0400137A RID: 4986
	[Tooltip("The button used to trigger the teleport after aiming. It can be the same button as the AimButton, however you cannot abort a teleport if it is.")]
	public OVRInput.RawButton TeleportButton;

	// Token: 0x0400137B RID: 4987
	[Tooltip("When true, the system will not use the PreTeleport intention which will allow a teleport to occur on a button downpress. When false, the button downpress will trigger the PreTeleport intention and the Teleport intention when the button is released.")]
	public bool FastTeleport;
}
