using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000270 RID: 624
public static class NetInput
{
	// Token: 0x17000178 RID: 376
	// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x0003A4CA File Offset: 0x000386CA
	public static VRRig LocalPlayerVRRig
	{
		get
		{
			if (NetInput._localPlayerVRRig == null)
			{
				NetInput._localPlayerVRRig = GameObject.Find("Local VRRig").GetComponentInChildren<VRRig>();
			}
			return NetInput._localPlayerVRRig;
		}
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x000A4A84 File Offset: 0x000A2C84
	public static NetworkedInput GetInput()
	{
		NetworkedInput result = default(NetworkedInput);
		if (NetInput.LocalPlayerVRRig == null)
		{
			return result;
		}
		result.headRot_LS = NetInput.LocalPlayerVRRig.head.rigTarget.localRotation;
		result.rightHandPos_LS = NetInput.LocalPlayerVRRig.rightHand.rigTarget.localPosition;
		result.rightHandRot_LS = NetInput.LocalPlayerVRRig.rightHand.rigTarget.localRotation;
		result.leftHandPos_LS = NetInput.LocalPlayerVRRig.leftHand.rigTarget.localPosition;
		result.leftHandRot_LS = NetInput.LocalPlayerVRRig.leftHand.rigTarget.localRotation;
		result.handPoseData = NetInput.LocalPlayerVRRig.ReturnHandPosition();
		result.rootPosition = NetInput.LocalPlayerVRRig.transform.position;
		result.rootRotation = NetInput.LocalPlayerVRRig.transform.rotation;
		result.leftThumbTouch = (ControllerInputPoller.PrimaryButtonTouch(XRNode.LeftHand) || ControllerInputPoller.SecondaryButtonTouch(XRNode.LeftHand));
		result.leftThumbPress = (ControllerInputPoller.PrimaryButtonPress(XRNode.LeftHand) || ControllerInputPoller.SecondaryButtonPress(XRNode.LeftHand));
		result.leftIndexValue = ControllerInputPoller.TriggerFloat(XRNode.LeftHand);
		result.leftMiddleValue = ControllerInputPoller.GripFloat(XRNode.LeftHand);
		result.rightThumbTouch = (ControllerInputPoller.PrimaryButtonTouch(XRNode.RightHand) || ControllerInputPoller.SecondaryButtonPress(XRNode.RightHand));
		result.rightThumbPress = (ControllerInputPoller.PrimaryButtonPress(XRNode.RightHand) || ControllerInputPoller.SecondaryButtonPress(XRNode.RightHand));
		result.rightIndexValue = ControllerInputPoller.TriggerFloat(XRNode.RightHand);
		result.rightMiddleValue = ControllerInputPoller.GripFloat(XRNode.RightHand);
		result.scale = NetInput.LocalPlayerVRRig.scaleFactor;
		return result;
	}

	// Token: 0x04001157 RID: 4439
	private static VRRig _localPlayerVRRig;
}
