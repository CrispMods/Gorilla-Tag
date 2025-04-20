using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x02000512 RID: 1298
public class ControllerInputPoller : MonoBehaviour
{
	// Token: 0x17000331 RID: 817
	// (get) Token: 0x06001F80 RID: 8064 RVA: 0x000454A7 File Offset: 0x000436A7
	// (set) Token: 0x06001F81 RID: 8065 RVA: 0x000454AF File Offset: 0x000436AF
	public GorillaControllerType controllerType { get; private set; }

	// Token: 0x06001F82 RID: 8066 RVA: 0x000454B8 File Offset: 0x000436B8
	private void Awake()
	{
		if (ControllerInputPoller.instance == null)
		{
			ControllerInputPoller.instance = this;
			return;
		}
		if (ControllerInputPoller.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06001F83 RID: 8067 RVA: 0x000EF72C File Offset: 0x000ED92C
	public static void AddUpdateCallback(Action callback)
	{
		if (!ControllerInputPoller.instance.didModifyOnUpdate)
		{
			ControllerInputPoller.instance.onUpdateNext.Clear();
			ControllerInputPoller.instance.onUpdateNext.AddRange(ControllerInputPoller.instance.onUpdate);
			ControllerInputPoller.instance.didModifyOnUpdate = true;
		}
		ControllerInputPoller.instance.onUpdateNext.Add(callback);
	}

	// Token: 0x06001F84 RID: 8068 RVA: 0x000EF794 File Offset: 0x000ED994
	public static void RemoveUpdateCallback(Action callback)
	{
		if (!ControllerInputPoller.instance.didModifyOnUpdate)
		{
			ControllerInputPoller.instance.onUpdateNext.Clear();
			ControllerInputPoller.instance.onUpdateNext.AddRange(ControllerInputPoller.instance.onUpdate);
			ControllerInputPoller.instance.didModifyOnUpdate = true;
		}
		ControllerInputPoller.instance.onUpdateNext.Remove(callback);
	}

	// Token: 0x06001F85 RID: 8069 RVA: 0x000EF800 File Offset: 0x000EDA00
	private void Update()
	{
		InputDevice inputDevice = this.leftControllerDevice;
		if (!this.leftControllerDevice.isValid)
		{
			this.leftControllerDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
			if (this.leftControllerDevice.isValid)
			{
				this.controllerType = GorillaControllerType.OCULUS_DEFAULT;
				if (this.leftControllerDevice.name.ToLower().Contains("knuckles"))
				{
					this.controllerType = GorillaControllerType.INDEX;
				}
				Debug.Log(string.Format("Found left controller: {0} ControllerType: {1}", this.leftControllerDevice.name, this.controllerType));
			}
		}
		InputDevice inputDevice2 = this.rightControllerDevice;
		if (!this.rightControllerDevice.isValid)
		{
			this.rightControllerDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
		}
		InputDevice inputDevice3 = this.headDevice;
		if (!this.headDevice.isValid)
		{
			this.headDevice = InputDevices.GetDeviceAtXRNode(XRNode.CenterEye);
		}
		InputDevice inputDevice4 = this.leftControllerDevice;
		InputDevice inputDevice5 = this.rightControllerDevice;
		InputDevice inputDevice6 = this.headDevice;
		this.leftControllerDevice.TryGetFeatureValue(CommonUsages.primaryButton, out this.leftControllerPrimaryButton);
		this.leftControllerDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out this.leftControllerSecondaryButton);
		this.leftControllerDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out this.leftControllerPrimaryButtonTouch);
		this.leftControllerDevice.TryGetFeatureValue(CommonUsages.secondaryTouch, out this.leftControllerSecondaryButtonTouch);
		this.leftControllerDevice.TryGetFeatureValue(CommonUsages.grip, out this.leftControllerGripFloat);
		this.leftControllerDevice.TryGetFeatureValue(CommonUsages.trigger, out this.leftControllerIndexFloat);
		this.leftControllerDevice.TryGetFeatureValue(CommonUsages.devicePosition, out this.leftControllerPosition);
		this.leftControllerDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out this.leftControllerRotation);
		this.leftControllerDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out this.leftControllerPrimary2DAxis);
		this.rightControllerDevice.TryGetFeatureValue(CommonUsages.primaryButton, out this.rightControllerPrimaryButton);
		this.rightControllerDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out this.rightControllerSecondaryButton);
		this.rightControllerDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out this.rightControllerPrimaryButtonTouch);
		this.rightControllerDevice.TryGetFeatureValue(CommonUsages.secondaryTouch, out this.rightControllerSecondaryButtonTouch);
		this.rightControllerDevice.TryGetFeatureValue(CommonUsages.grip, out this.rightControllerGripFloat);
		this.rightControllerDevice.TryGetFeatureValue(CommonUsages.trigger, out this.rightControllerIndexFloat);
		this.rightControllerDevice.TryGetFeatureValue(CommonUsages.devicePosition, out this.rightControllerPosition);
		this.rightControllerDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out this.rightControllerRotation);
		this.rightControllerDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out this.rightControllerPrimary2DAxis);
		this.leftControllerPrimaryButton = SteamVR_Actions.gorillaTag_LeftPrimaryClick.GetState(SteamVR_Input_Sources.LeftHand);
		this.leftControllerSecondaryButton = SteamVR_Actions.gorillaTag_LeftSecondaryClick.GetState(SteamVR_Input_Sources.LeftHand);
		this.leftControllerPrimaryButtonTouch = SteamVR_Actions.gorillaTag_LeftPrimaryTouch.GetState(SteamVR_Input_Sources.LeftHand);
		this.leftControllerSecondaryButtonTouch = SteamVR_Actions.gorillaTag_LeftSecondaryTouch.GetState(SteamVR_Input_Sources.LeftHand);
		this.leftControllerGripFloat = SteamVR_Actions.gorillaTag_LeftGripFloat.GetAxis(SteamVR_Input_Sources.LeftHand);
		this.leftControllerIndexFloat = SteamVR_Actions.gorillaTag_LeftTriggerFloat.GetAxis(SteamVR_Input_Sources.LeftHand);
		this.rightControllerPrimaryButton = SteamVR_Actions.gorillaTag_RightPrimaryClick.GetState(SteamVR_Input_Sources.RightHand);
		this.rightControllerSecondaryButton = SteamVR_Actions.gorillaTag_RightSecondaryClick.GetState(SteamVR_Input_Sources.RightHand);
		this.rightControllerPrimaryButtonTouch = SteamVR_Actions.gorillaTag_RightPrimaryTouch.GetState(SteamVR_Input_Sources.RightHand);
		this.rightControllerSecondaryButtonTouch = SteamVR_Actions.gorillaTag_RightSecondaryTouch.GetState(SteamVR_Input_Sources.RightHand);
		this.rightControllerGripFloat = SteamVR_Actions.gorillaTag_RightGripFloat.GetAxis(SteamVR_Input_Sources.RightHand);
		this.rightControllerIndexFloat = SteamVR_Actions.gorillaTag_RightTriggerFloat.GetAxis(SteamVR_Input_Sources.RightHand);
		this.rightControllerPrimary2DAxis = SteamVR_Actions.gorillaTag_RightJoystick2DAxis.GetAxis(SteamVR_Input_Sources.RightHand);
		this.headDevice.TryGetFeatureValue(CommonUsages.devicePosition, out this.headPosition);
		this.headDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out this.headRotation);
		if (this.controllerType == GorillaControllerType.OCULUS_DEFAULT)
		{
			this.CalculateGrabState(this.leftControllerGripFloat, ref this.leftGrab, ref this.leftGrabRelease, ref this.leftGrabMomentary, ref this.leftGrabReleaseMomentary, 0.75f, 0.65f);
			this.CalculateGrabState(this.rightControllerGripFloat, ref this.rightGrab, ref this.rightGrabRelease, ref this.rightGrabMomentary, ref this.rightGrabReleaseMomentary, 0.75f, 0.65f);
		}
		else if (this.controllerType == GorillaControllerType.INDEX)
		{
			this.CalculateGrabState(this.leftControllerGripFloat, ref this.leftGrab, ref this.leftGrabRelease, ref this.leftGrabMomentary, ref this.leftGrabReleaseMomentary, 0.1f, 0.01f);
			this.CalculateGrabState(this.rightControllerGripFloat, ref this.rightGrab, ref this.rightGrabRelease, ref this.rightGrabMomentary, ref this.rightGrabReleaseMomentary, 0.1f, 0.01f);
		}
		if (this.didModifyOnUpdate)
		{
			List<Action> list = this.onUpdateNext;
			List<Action> list2 = this.onUpdate;
			this.onUpdate = list;
			this.onUpdateNext = list2;
			this.didModifyOnUpdate = false;
		}
		foreach (Action action in this.onUpdate)
		{
			action();
		}
	}

	// Token: 0x06001F86 RID: 8070 RVA: 0x000EFCC0 File Offset: 0x000EDEC0
	private void CalculateGrabState(float grabValue, ref bool grab, ref bool grabRelease, ref bool grabMomentary, ref bool grabReleaseMomentary, float grabThreshold, float grabReleaseThreshold)
	{
		bool flag = grabValue >= grabThreshold;
		bool flag2 = grabValue <= grabReleaseThreshold;
		grabMomentary = (flag && flag != grab);
		grabReleaseMomentary = (flag2 && flag2 != grabRelease);
		grab = flag;
		grabRelease = flag2;
	}

	// Token: 0x06001F87 RID: 8071 RVA: 0x000454EC File Offset: 0x000436EC
	public static bool GetGrab(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftGrab;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightGrab;
	}

	// Token: 0x06001F88 RID: 8072 RVA: 0x00045511 File Offset: 0x00043711
	public static bool GetGrabRelease(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftGrabRelease;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightGrabRelease;
	}

	// Token: 0x06001F89 RID: 8073 RVA: 0x00045536 File Offset: 0x00043736
	public static bool GetGrabMomentary(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftGrabMomentary;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightGrabMomentary;
	}

	// Token: 0x06001F8A RID: 8074 RVA: 0x0004555B File Offset: 0x0004375B
	public static bool GetGrabReleaseMomentary(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftGrabReleaseMomentary;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightGrabReleaseMomentary;
	}

	// Token: 0x06001F8B RID: 8075 RVA: 0x00045580 File Offset: 0x00043780
	public static Vector2 Primary2DAxis(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftControllerPrimary2DAxis;
		}
		return ControllerInputPoller.instance.rightControllerPrimary2DAxis;
	}

	// Token: 0x06001F8C RID: 8076 RVA: 0x0004559F File Offset: 0x0004379F
	public static bool PrimaryButtonPress(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftControllerPrimaryButton;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightControllerPrimaryButton;
	}

	// Token: 0x06001F8D RID: 8077 RVA: 0x000455C4 File Offset: 0x000437C4
	public static bool SecondaryButtonPress(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftControllerSecondaryButton;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightControllerSecondaryButton;
	}

	// Token: 0x06001F8E RID: 8078 RVA: 0x000455E9 File Offset: 0x000437E9
	public static bool PrimaryButtonTouch(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftControllerPrimaryButtonTouch;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightControllerPrimaryButtonTouch;
	}

	// Token: 0x06001F8F RID: 8079 RVA: 0x0004560E File Offset: 0x0004380E
	public static bool SecondaryButtonTouch(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftControllerSecondaryButtonTouch;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightControllerSecondaryButtonTouch;
	}

	// Token: 0x06001F90 RID: 8080 RVA: 0x00045633 File Offset: 0x00043833
	public static float GripFloat(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftControllerGripFloat;
		}
		if (node == XRNode.RightHand)
		{
			return ControllerInputPoller.instance.rightControllerGripFloat;
		}
		return 0f;
	}

	// Token: 0x06001F91 RID: 8081 RVA: 0x0004565C File Offset: 0x0004385C
	public static float TriggerFloat(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftControllerIndexFloat;
		}
		if (node == XRNode.RightHand)
		{
			return ControllerInputPoller.instance.rightControllerIndexFloat;
		}
		return 0f;
	}

	// Token: 0x06001F92 RID: 8082 RVA: 0x00045685 File Offset: 0x00043885
	public static float TriggerTouch(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftControllerIndexTouch;
		}
		if (node == XRNode.RightHand)
		{
			return ControllerInputPoller.instance.rightControllerIndexTouch;
		}
		return 0f;
	}

	// Token: 0x06001F93 RID: 8083 RVA: 0x000456AE File Offset: 0x000438AE
	public static Vector3 DevicePosition(XRNode node)
	{
		if (node == XRNode.Head)
		{
			return ControllerInputPoller.instance.headPosition;
		}
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftControllerPosition;
		}
		if (node == XRNode.RightHand)
		{
			return ControllerInputPoller.instance.rightControllerPosition;
		}
		return Vector3.zero;
	}

	// Token: 0x06001F94 RID: 8084 RVA: 0x000456E8 File Offset: 0x000438E8
	public static Quaternion DeviceRotation(XRNode node)
	{
		if (node == XRNode.Head)
		{
			return ControllerInputPoller.instance.headRotation;
		}
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftControllerRotation;
		}
		if (node == XRNode.RightHand)
		{
			return ControllerInputPoller.instance.rightControllerRotation;
		}
		return Quaternion.identity;
	}

	// Token: 0x06001F95 RID: 8085 RVA: 0x000EFD08 File Offset: 0x000EDF08
	public static bool PositionValid(XRNode node)
	{
		if (node == XRNode.Head)
		{
			InputDevice inputDevice = ControllerInputPoller.instance.headDevice;
			return ControllerInputPoller.instance.headDevice.isValid;
		}
		if (node == XRNode.LeftHand)
		{
			InputDevice inputDevice2 = ControllerInputPoller.instance.leftControllerDevice;
			return ControllerInputPoller.instance.leftControllerDevice.isValid;
		}
		if (node == XRNode.RightHand)
		{
			InputDevice inputDevice3 = ControllerInputPoller.instance.rightControllerDevice;
			return ControllerInputPoller.instance.rightControllerDevice.isValid;
		}
		return false;
	}

	// Token: 0x04002345 RID: 9029
	[OnEnterPlay_SetNull]
	public static volatile ControllerInputPoller instance;

	// Token: 0x04002346 RID: 9030
	public float leftControllerIndexFloat;

	// Token: 0x04002347 RID: 9031
	public float leftControllerGripFloat;

	// Token: 0x04002348 RID: 9032
	public float rightControllerIndexFloat;

	// Token: 0x04002349 RID: 9033
	public float rightControllerGripFloat;

	// Token: 0x0400234A RID: 9034
	public float leftControllerIndexTouch;

	// Token: 0x0400234B RID: 9035
	public float rightControllerIndexTouch;

	// Token: 0x0400234C RID: 9036
	public float rightStickLRFloat;

	// Token: 0x0400234D RID: 9037
	public Vector3 leftControllerPosition;

	// Token: 0x0400234E RID: 9038
	public Vector3 rightControllerPosition;

	// Token: 0x0400234F RID: 9039
	public Vector3 headPosition;

	// Token: 0x04002350 RID: 9040
	public Quaternion leftControllerRotation;

	// Token: 0x04002351 RID: 9041
	public Quaternion rightControllerRotation;

	// Token: 0x04002352 RID: 9042
	public Quaternion headRotation;

	// Token: 0x04002353 RID: 9043
	public InputDevice leftControllerDevice;

	// Token: 0x04002354 RID: 9044
	public InputDevice rightControllerDevice;

	// Token: 0x04002355 RID: 9045
	public InputDevice headDevice;

	// Token: 0x04002356 RID: 9046
	public bool leftControllerPrimaryButton;

	// Token: 0x04002357 RID: 9047
	public bool leftControllerSecondaryButton;

	// Token: 0x04002358 RID: 9048
	public bool rightControllerPrimaryButton;

	// Token: 0x04002359 RID: 9049
	public bool rightControllerSecondaryButton;

	// Token: 0x0400235A RID: 9050
	public bool leftControllerPrimaryButtonTouch;

	// Token: 0x0400235B RID: 9051
	public bool leftControllerSecondaryButtonTouch;

	// Token: 0x0400235C RID: 9052
	public bool rightControllerPrimaryButtonTouch;

	// Token: 0x0400235D RID: 9053
	public bool rightControllerSecondaryButtonTouch;

	// Token: 0x0400235E RID: 9054
	public bool leftGrab;

	// Token: 0x0400235F RID: 9055
	public bool leftGrabRelease;

	// Token: 0x04002360 RID: 9056
	public bool rightGrab;

	// Token: 0x04002361 RID: 9057
	public bool rightGrabRelease;

	// Token: 0x04002362 RID: 9058
	public bool leftGrabMomentary;

	// Token: 0x04002363 RID: 9059
	public bool leftGrabReleaseMomentary;

	// Token: 0x04002364 RID: 9060
	public bool rightGrabMomentary;

	// Token: 0x04002365 RID: 9061
	public bool rightGrabReleaseMomentary;

	// Token: 0x04002367 RID: 9063
	public Vector2 leftControllerPrimary2DAxis;

	// Token: 0x04002368 RID: 9064
	public Vector2 rightControllerPrimary2DAxis;

	// Token: 0x04002369 RID: 9065
	private List<Action> onUpdate = new List<Action>();

	// Token: 0x0400236A RID: 9066
	private List<Action> onUpdateNext = new List<Action>();

	// Token: 0x0400236B RID: 9067
	private bool didModifyOnUpdate;
}
