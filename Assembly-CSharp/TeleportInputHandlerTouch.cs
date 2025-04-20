using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020002E3 RID: 739
public class TeleportInputHandlerTouch : TeleportInputHandlerHMD
{
	// Token: 0x060011EC RID: 4588 RVA: 0x00030607 File Offset: 0x0002E807
	private void Start()
	{
	}

	// Token: 0x060011ED RID: 4589 RVA: 0x000AEBB8 File Offset: 0x000ACDB8
	public override LocomotionTeleport.TeleportIntentions GetIntention()
	{
		if (!base.isActiveAndEnabled)
		{
			return LocomotionTeleport.TeleportIntentions.None;
		}
		if (this.InputMode == TeleportInputHandlerTouch.InputModes.SeparateButtonsForAimAndTeleport)
		{
			return base.GetIntention();
		}
		if (this.InputMode == TeleportInputHandlerTouch.InputModes.ThumbstickTeleport || this.InputMode == TeleportInputHandlerTouch.InputModes.ThumbstickTeleportForwardBackOnly)
		{
			Vector2 lhs = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.Active);
			Vector2 lhs2 = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.Active);
			bool flag = OVRInput.Get(OVRInput.RawTouch.LThumbstick, OVRInput.Controller.Active);
			bool flag2 = OVRInput.Get(OVRInput.RawTouch.RThumbstick, OVRInput.Controller.Active);
			float num;
			float num2;
			if (this.InputMode == TeleportInputHandlerTouch.InputModes.ThumbstickTeleportForwardBackOnly && base.LocomotionTeleport.CurrentIntention != LocomotionTeleport.TeleportIntentions.Aim)
			{
				num = Mathf.Abs(Vector2.Dot(lhs, Vector2.up));
				num2 = Mathf.Abs(Vector2.Dot(lhs2, Vector2.up));
			}
			else
			{
				num = lhs.magnitude;
				num2 = lhs2.magnitude;
			}
			float num3;
			OVRInput.Controller initiatingController;
			if (this.AimingController == OVRInput.Controller.LTouch)
			{
				num3 = num;
				initiatingController = OVRInput.Controller.LTouch;
			}
			else if (this.AimingController == OVRInput.Controller.RTouch)
			{
				num3 = num2;
				initiatingController = OVRInput.Controller.RTouch;
			}
			else if (num > num2)
			{
				num3 = num;
				initiatingController = OVRInput.Controller.LTouch;
			}
			else
			{
				num3 = num2;
				initiatingController = OVRInput.Controller.RTouch;
			}
			if (num3 <= this.ThumbstickTeleportThreshold && (this.AimingController != OVRInput.Controller.Touch || (!flag && !flag2)) && (this.AimingController != OVRInput.Controller.LTouch || !flag) && (this.AimingController != OVRInput.Controller.RTouch || !flag2))
			{
				if (base.LocomotionTeleport.CurrentIntention == LocomotionTeleport.TeleportIntentions.Aim)
				{
					if (!this.FastTeleport)
					{
						return LocomotionTeleport.TeleportIntentions.PreTeleport;
					}
					return LocomotionTeleport.TeleportIntentions.Teleport;
				}
				else if (base.LocomotionTeleport.CurrentIntention == LocomotionTeleport.TeleportIntentions.PreTeleport)
				{
					return LocomotionTeleport.TeleportIntentions.Teleport;
				}
			}
			else if (base.LocomotionTeleport.CurrentIntention == LocomotionTeleport.TeleportIntentions.Aim)
			{
				return LocomotionTeleport.TeleportIntentions.Aim;
			}
			if (num3 > this.ThumbstickTeleportThreshold)
			{
				this.InitiatingController = initiatingController;
				return LocomotionTeleport.TeleportIntentions.Aim;
			}
			return LocomotionTeleport.TeleportIntentions.None;
		}
		else
		{
			OVRInput.RawButton rawMask = this._rawButtons[(int)this.CapacitiveAimAndTeleportButton];
			if (base.LocomotionTeleport.CurrentIntention == LocomotionTeleport.TeleportIntentions.Aim && OVRInput.GetDown(rawMask, OVRInput.Controller.Active))
			{
				if (!this.FastTeleport)
				{
					return LocomotionTeleport.TeleportIntentions.PreTeleport;
				}
				return LocomotionTeleport.TeleportIntentions.Teleport;
			}
			else if (base.LocomotionTeleport.CurrentIntention == LocomotionTeleport.TeleportIntentions.PreTeleport)
			{
				if (this.FastTeleport || OVRInput.GetUp(rawMask, OVRInput.Controller.Active))
				{
					return LocomotionTeleport.TeleportIntentions.Teleport;
				}
				return LocomotionTeleport.TeleportIntentions.PreTeleport;
			}
			else
			{
				if (OVRInput.GetDown(this._rawTouch[(int)this.CapacitiveAimAndTeleportButton], OVRInput.Controller.Active))
				{
					return LocomotionTeleport.TeleportIntentions.Aim;
				}
				if (base.LocomotionTeleport.CurrentIntention == LocomotionTeleport.TeleportIntentions.Aim && !OVRInput.GetUp(this._rawTouch[(int)this.CapacitiveAimAndTeleportButton], OVRInput.Controller.Active))
				{
					return LocomotionTeleport.TeleportIntentions.Aim;
				}
				return LocomotionTeleport.TeleportIntentions.None;
			}
		}
	}

	// Token: 0x060011EE RID: 4590 RVA: 0x000AEDF0 File Offset: 0x000ACFF0
	public override void GetAimData(out Ray aimRay)
	{
		OVRInput.Controller controller = this.AimingController;
		if (controller == OVRInput.Controller.Touch)
		{
			controller = this.InitiatingController;
		}
		Transform transform = (controller == OVRInput.Controller.LTouch) ? this.LeftHand : this.RightHand;
		aimRay = new Ray(transform.position, transform.forward);
	}

	// Token: 0x060011EF RID: 4591 RVA: 0x000AEE3C File Offset: 0x000AD03C
	public TeleportInputHandlerTouch()
	{
		OVRInput.RawButton[] array = new OVRInput.RawButton[8];
		RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.5F5673AE83EE13B46A7C1D9CE2F8CC01C37CFC893B0AC5E6E9260B79215F5ADC).FieldHandle);
		this._rawButtons = array;
		OVRInput.RawTouch[] array2 = new OVRInput.RawTouch[8];
		RuntimeHelpers.InitializeArray(array2, fieldof(<PrivateImplementationDetails>.1385A3395CDC9F7F90324CB0A06C5AC11AD4A425A35BBB7D5C9C0C33D8ADE9A0).FieldHandle);
		this._rawTouch = array2;
		this.ThumbstickTeleportThreshold = 0.5f;
		base..ctor();
	}

	// Token: 0x040013C3 RID: 5059
	public Transform LeftHand;

	// Token: 0x040013C4 RID: 5060
	public Transform RightHand;

	// Token: 0x040013C5 RID: 5061
	[Tooltip("CapacitiveButtonForAimAndTeleport=Activate aiming via cap touch detection, press the same button to teleport.\nSeparateButtonsForAimAndTeleport=Use one button to begin aiming, and another to trigger the teleport.\nThumbstickTeleport=Push a thumbstick to begin aiming, release to teleport.")]
	public TeleportInputHandlerTouch.InputModes InputMode;

	// Token: 0x040013C6 RID: 5062
	private readonly OVRInput.RawButton[] _rawButtons;

	// Token: 0x040013C7 RID: 5063
	private readonly OVRInput.RawTouch[] _rawTouch;

	// Token: 0x040013C8 RID: 5064
	[Tooltip("Select the controller to be used for aiming. Supports LTouch, RTouch, or Touch for either.")]
	public OVRInput.Controller AimingController;

	// Token: 0x040013C9 RID: 5065
	private OVRInput.Controller InitiatingController;

	// Token: 0x040013CA RID: 5066
	[Tooltip("Select the button to use for triggering aim and teleport when InputMode==CapacitiveButtonForAimAndTeleport")]
	public TeleportInputHandlerTouch.AimCapTouchButtons CapacitiveAimAndTeleportButton;

	// Token: 0x040013CB RID: 5067
	[Tooltip("The thumbstick magnitude required to trigger aiming and teleports when InputMode==InputModes.ThumbstickTeleport")]
	public float ThumbstickTeleportThreshold;

	// Token: 0x020002E4 RID: 740
	public enum InputModes
	{
		// Token: 0x040013CD RID: 5069
		CapacitiveButtonForAimAndTeleport,
		// Token: 0x040013CE RID: 5070
		SeparateButtonsForAimAndTeleport,
		// Token: 0x040013CF RID: 5071
		ThumbstickTeleport,
		// Token: 0x040013D0 RID: 5072
		ThumbstickTeleportForwardBackOnly
	}

	// Token: 0x020002E5 RID: 741
	public enum AimCapTouchButtons
	{
		// Token: 0x040013D2 RID: 5074
		A,
		// Token: 0x040013D3 RID: 5075
		B,
		// Token: 0x040013D4 RID: 5076
		LeftTrigger,
		// Token: 0x040013D5 RID: 5077
		LeftThumbstick,
		// Token: 0x040013D6 RID: 5078
		RightTrigger,
		// Token: 0x040013D7 RID: 5079
		RightThumbstick,
		// Token: 0x040013D8 RID: 5080
		X,
		// Token: 0x040013D9 RID: 5081
		Y
	}
}
