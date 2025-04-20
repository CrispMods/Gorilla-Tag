using System;
using UnityEngine;

// Token: 0x020002E2 RID: 738
public class TeleportInputHandlerHMD : TeleportInputHandler
{
	// Token: 0x17000204 RID: 516
	// (get) Token: 0x060011E7 RID: 4583 RVA: 0x0003C310 File Offset: 0x0003A510
	// (set) Token: 0x060011E8 RID: 4584 RVA: 0x0003C318 File Offset: 0x0003A518
	public Transform Pointer { get; private set; }

	// Token: 0x060011E9 RID: 4585 RVA: 0x000AEAF0 File Offset: 0x000ACCF0
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

	// Token: 0x060011EA RID: 4586 RVA: 0x000AEB7C File Offset: 0x000ACD7C
	public override void GetAimData(out Ray aimRay)
	{
		Transform centerEyeAnchor = base.LocomotionTeleport.LocomotionController.CameraRig.centerEyeAnchor;
		aimRay = new Ray(centerEyeAnchor.position, centerEyeAnchor.forward);
	}

	// Token: 0x040013C0 RID: 5056
	[Tooltip("The button used to begin aiming for a teleport.")]
	public OVRInput.RawButton AimButton;

	// Token: 0x040013C1 RID: 5057
	[Tooltip("The button used to trigger the teleport after aiming. It can be the same button as the AimButton, however you cannot abort a teleport if it is.")]
	public OVRInput.RawButton TeleportButton;

	// Token: 0x040013C2 RID: 5058
	[Tooltip("When true, the system will not use the PreTeleport intention which will allow a teleport to occur on a button downpress. When false, the button downpress will trigger the PreTeleport intention and the Teleport intention when the button is released.")]
	public bool FastTeleport;
}
