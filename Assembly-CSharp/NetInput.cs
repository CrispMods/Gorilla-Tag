using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000265 RID: 613
public static class NetInput
{
	// Token: 0x17000171 RID: 369
	// (get) Token: 0x06000E55 RID: 3669 RVA: 0x00047DE0 File Offset: 0x00045FE0
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

	// Token: 0x06000E56 RID: 3670 RVA: 0x00047E08 File Offset: 0x00046008
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

	// Token: 0x04001111 RID: 4369
	private static VRRig _localPlayerVRRig;
}
