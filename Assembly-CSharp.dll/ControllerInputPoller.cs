using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x02000505 RID: 1285
public class ControllerInputPoller : MonoBehaviour
{
	// Token: 0x1700032A RID: 810
	// (get) Token: 0x06001F2A RID: 7978 RVA: 0x00044108 File Offset: 0x00042308
	// (set) Token: 0x06001F2B RID: 7979 RVA: 0x00044110 File Offset: 0x00042310
	public GorillaControllerType controllerType { get; private set; }

	// Token: 0x06001F2C RID: 7980 RVA: 0x00044119 File Offset: 0x00042319
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

	// Token: 0x06001F2D RID: 7981 RVA: 0x000EC9F0 File Offset: 0x000EABF0
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

	// Token: 0x06001F2E RID: 7982 RVA: 0x000ECA58 File Offset: 0x000EAC58
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

	// Token: 0x06001F2F RID: 7983 RVA: 0x000ECAC4 File Offset: 0x000EACC4
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

	// Token: 0x06001F30 RID: 7984 RVA: 0x000ECF84 File Offset: 0x000EB184
	private void CalculateGrabState(float grabValue, ref bool grab, ref bool grabRelease, ref bool grabMomentary, ref bool grabReleaseMomentary, float grabThreshold, float grabReleaseThreshold)
	{
		bool flag = grabValue >= grabThreshold;
		bool flag2 = grabValue <= grabReleaseThreshold;
		grabMomentary = (flag && flag != grab);
		grabReleaseMomentary = (flag2 && flag2 != grabRelease);
		grab = flag;
		grabRelease = flag2;
	}

	// Token: 0x06001F31 RID: 7985 RVA: 0x0004414D File Offset: 0x0004234D
	public static bool GetGrab(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftGrab;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightGrab;
	}

	// Token: 0x06001F32 RID: 7986 RVA: 0x00044172 File Offset: 0x00042372
	public static bool GetGrabRelease(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftGrabRelease;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightGrabRelease;
	}

	// Token: 0x06001F33 RID: 7987 RVA: 0x00044197 File Offset: 0x00042397
	public static bool GetGrabMomentary(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftGrabMomentary;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightGrabMomentary;
	}

	// Token: 0x06001F34 RID: 7988 RVA: 0x000441BC File Offset: 0x000423BC
	public static bool GetGrabReleaseMomentary(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftGrabReleaseMomentary;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightGrabReleaseMomentary;
	}

	// Token: 0x06001F35 RID: 7989 RVA: 0x000441E1 File Offset: 0x000423E1
	public static Vector2 Primary2DAxis(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftControllerPrimary2DAxis;
		}
		return ControllerInputPoller.instance.rightControllerPrimary2DAxis;
	}

	// Token: 0x06001F36 RID: 7990 RVA: 0x00044200 File Offset: 0x00042400
	public static bool PrimaryButtonPress(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftControllerPrimaryButton;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightControllerPrimaryButton;
	}

	// Token: 0x06001F37 RID: 7991 RVA: 0x00044225 File Offset: 0x00042425
	public static bool SecondaryButtonPress(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftControllerSecondaryButton;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightControllerSecondaryButton;
	}

	// Token: 0x06001F38 RID: 7992 RVA: 0x0004424A File Offset: 0x0004244A
	public static bool PrimaryButtonTouch(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftControllerPrimaryButtonTouch;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightControllerPrimaryButtonTouch;
	}

	// Token: 0x06001F39 RID: 7993 RVA: 0x0004426F File Offset: 0x0004246F
	public static bool SecondaryButtonTouch(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return ControllerInputPoller.instance.leftControllerSecondaryButtonTouch;
		}
		return node == XRNode.RightHand && ControllerInputPoller.instance.rightControllerSecondaryButtonTouch;
	}

	// Token: 0x06001F3A RID: 7994 RVA: 0x00044294 File Offset: 0x00042494
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

	// Token: 0x06001F3B RID: 7995 RVA: 0x000442BD File Offset: 0x000424BD
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

	// Token: 0x06001F3C RID: 7996 RVA: 0x000442E6 File Offset: 0x000424E6
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

	// Token: 0x06001F3D RID: 7997 RVA: 0x0004430F File Offset: 0x0004250F
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

	// Token: 0x06001F3E RID: 7998 RVA: 0x00044349 File Offset: 0x00042549
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

	// Token: 0x06001F3F RID: 7999 RVA: 0x000ECFCC File Offset: 0x000EB1CC
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

	// Token: 0x040022F3 RID: 8947
	[OnEnterPlay_SetNull]
	public static volatile ControllerInputPoller instance;

	// Token: 0x040022F4 RID: 8948
	public float leftControllerIndexFloat;

	// Token: 0x040022F5 RID: 8949
	public float leftControllerGripFloat;

	// Token: 0x040022F6 RID: 8950
	public float rightControllerIndexFloat;

	// Token: 0x040022F7 RID: 8951
	public float rightControllerGripFloat;

	// Token: 0x040022F8 RID: 8952
	public float leftControllerIndexTouch;

	// Token: 0x040022F9 RID: 8953
	public float rightControllerIndexTouch;

	// Token: 0x040022FA RID: 8954
	public float rightStickLRFloat;

	// Token: 0x040022FB RID: 8955
	public Vector3 leftControllerPosition;

	// Token: 0x040022FC RID: 8956
	public Vector3 rightControllerPosition;

	// Token: 0x040022FD RID: 8957
	public Vector3 headPosition;

	// Token: 0x040022FE RID: 8958
	public Quaternion leftControllerRotation;

	// Token: 0x040022FF RID: 8959
	public Quaternion rightControllerRotation;

	// Token: 0x04002300 RID: 8960
	public Quaternion headRotation;

	// Token: 0x04002301 RID: 8961
	public InputDevice leftControllerDevice;

	// Token: 0x04002302 RID: 8962
	public InputDevice rightControllerDevice;

	// Token: 0x04002303 RID: 8963
	public InputDevice headDevice;

	// Token: 0x04002304 RID: 8964
	public bool leftControllerPrimaryButton;

	// Token: 0x04002305 RID: 8965
	public bool leftControllerSecondaryButton;

	// Token: 0x04002306 RID: 8966
	public bool rightControllerPrimaryButton;

	// Token: 0x04002307 RID: 8967
	public bool rightControllerSecondaryButton;

	// Token: 0x04002308 RID: 8968
	public bool leftControllerPrimaryButtonTouch;

	// Token: 0x04002309 RID: 8969
	public bool leftControllerSecondaryButtonTouch;

	// Token: 0x0400230A RID: 8970
	public bool rightControllerPrimaryButtonTouch;

	// Token: 0x0400230B RID: 8971
	public bool rightControllerSecondaryButtonTouch;

	// Token: 0x0400230C RID: 8972
	public bool leftGrab;

	// Token: 0x0400230D RID: 8973
	public bool leftGrabRelease;

	// Token: 0x0400230E RID: 8974
	public bool rightGrab;

	// Token: 0x0400230F RID: 8975
	public bool rightGrabRelease;

	// Token: 0x04002310 RID: 8976
	public bool leftGrabMomentary;

	// Token: 0x04002311 RID: 8977
	public bool leftGrabReleaseMomentary;

	// Token: 0x04002312 RID: 8978
	public bool rightGrabMomentary;

	// Token: 0x04002313 RID: 8979
	public bool rightGrabReleaseMomentary;

	// Token: 0x04002315 RID: 8981
	public Vector2 leftControllerPrimary2DAxis;

	// Token: 0x04002316 RID: 8982
	public Vector2 rightControllerPrimary2DAxis;

	// Token: 0x04002317 RID: 8983
	private List<Action> onUpdate = new List<Action>();

	// Token: 0x04002318 RID: 8984
	private List<Action> onUpdateNext = new List<Action>();

	// Token: 0x04002319 RID: 8985
	private bool didModifyOnUpdate;
}
