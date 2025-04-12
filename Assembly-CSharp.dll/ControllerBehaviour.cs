using System;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x02000707 RID: 1799
public class ControllerBehaviour : MonoBehaviour, IBuildValidation
{
	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x06002CA9 RID: 11433 RVA: 0x0004D8A8 File Offset: 0x0004BAA8
	public bool ButtonDown
	{
		get
		{
			return this.buttonDown;
		}
	}

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x06002CAA RID: 11434 RVA: 0x0004D8B0 File Offset: 0x0004BAB0
	public bool IsLeftStick
	{
		get
		{
			return this.isLeftStick;
		}
	}

	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x06002CAB RID: 11435 RVA: 0x0004D8B8 File Offset: 0x0004BAB8
	public bool IsRightStick
	{
		get
		{
			return this.isRightStick;
		}
	}

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x06002CAC RID: 11436 RVA: 0x0004D8C0 File Offset: 0x0004BAC0
	public bool IsUpStick
	{
		get
		{
			return this.isUpStick;
		}
	}

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06002CAD RID: 11437 RVA: 0x0004D8C8 File Offset: 0x0004BAC8
	public bool IsDownStick
	{
		get
		{
			return this.isDownStick;
		}
	}

	// Token: 0x14000054 RID: 84
	// (add) Token: 0x06002CAE RID: 11438 RVA: 0x001221A4 File Offset: 0x001203A4
	// (remove) Token: 0x06002CAF RID: 11439 RVA: 0x001221DC File Offset: 0x001203DC
	public event ControllerBehaviour.OnActionEvent OnAction;

	// Token: 0x06002CB0 RID: 11440 RVA: 0x00122214 File Offset: 0x00120414
	private void Update()
	{
		this.leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
		Vector2 axis;
		this.leftHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis);
		bool state;
		this.leftHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out state);
		bool state2;
		this.leftHandDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out state2);
		bool state3;
		this.leftHandDevice.TryGetFeatureValue(CommonUsages.triggerButton, out state3);
		this.rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
		Vector2 axis2;
		this.rightHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis2);
		bool state4;
		this.rightHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out state4);
		bool state5;
		this.rightHandDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out state5);
		bool state6;
		this.rightHandDevice.TryGetFeatureValue(CommonUsages.triggerButton, out state6);
		axis = SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.GetAxis(SteamVR_Input_Sources.LeftHand);
		state = SteamVR_Actions.gorillaTag_LeftPrimaryClick.GetState(SteamVR_Input_Sources.LeftHand);
		state2 = SteamVR_Actions.gorillaTag_LeftSecondaryClick.GetState(SteamVR_Input_Sources.LeftHand);
		state3 = SteamVR_Actions.gorillaTag_LeftTriggerClick.GetState(SteamVR_Input_Sources.LeftHand);
		axis2 = SteamVR_Actions.gorillaTag_RightJoystick2DAxis.GetAxis(SteamVR_Input_Sources.RightHand);
		state4 = SteamVR_Actions.gorillaTag_RightPrimaryClick.GetState(SteamVR_Input_Sources.RightHand);
		state5 = SteamVR_Actions.gorillaTag_RightSecondaryClick.GetState(SteamVR_Input_Sources.RightHand);
		state6 = SteamVR_Actions.gorillaTag_RightTriggerClick.GetState(SteamVR_Input_Sources.RightHand);
		this.buttonDown = (state || state4 || state2 || state5);
		bool flag = Mathf.Min(axis.x, axis2.x) < -this.uxSettings.StickSensitvity || state3;
		bool flag2 = Mathf.Max(axis.x, axis2.x) > this.uxSettings.StickSensitvity || state6;
		bool flag3 = Mathf.Max(axis.y, axis2.y) > this.uxSettings.StickSensitvity;
		bool flag4 = Mathf.Min(axis.y, axis2.y) < -this.uxSettings.StickSensitvity;
		bool flag5 = (this.isLeftStick && flag) || (this.IsRightStick && flag2) || (this.isUpStick && flag3) || (this.isDownStick && flag4);
		if (Time.time - this.actionTime < this.actionDelay / this.repeatAction)
		{
			return;
		}
		if (flag5)
		{
			this.repeatAction += this.actionRepeatDelayReduction;
		}
		else
		{
			this.repeatAction = 1f;
		}
		this.isLeftStick = flag;
		this.isRightStick = flag2;
		this.isUpStick = flag3;
		this.isDownStick = flag4;
		if (this.isLeftStick || this.isRightStick || this.isUpStick || this.isDownStick || this.buttonDown)
		{
			this.actionTime = Time.time;
		}
		if (this.OnAction != null)
		{
			this.OnAction();
		}
	}

	// Token: 0x06002CB1 RID: 11441 RVA: 0x001224A4 File Offset: 0x001206A4
	private void OnDisable()
	{
		this.buttonDown = (this.isLeftStick = (this.isRightStick = (this.isUpStick = (this.isDownStick = false))));
	}

	// Token: 0x06002CB2 RID: 11442 RVA: 0x0004D8D0 File Offset: 0x0004BAD0
	public bool BuildValidationCheck()
	{
		if (this.uxSettings == null)
		{
			Debug.LogError("ControllerBehaviour must set UXSettings");
			return false;
		}
		return true;
	}

	// Token: 0x040031F7 RID: 12791
	private InputDevice leftHandDevice;

	// Token: 0x040031F8 RID: 12792
	private InputDevice rightHandDevice;

	// Token: 0x040031F9 RID: 12793
	private float actionTime;

	// Token: 0x040031FA RID: 12794
	private bool buttonDown;

	// Token: 0x040031FB RID: 12795
	private float repeatAction = 1f;

	// Token: 0x040031FC RID: 12796
	private bool isLeftStick;

	// Token: 0x040031FD RID: 12797
	private bool isRightStick;

	// Token: 0x040031FE RID: 12798
	private bool isUpStick;

	// Token: 0x040031FF RID: 12799
	private bool isDownStick;

	// Token: 0x04003200 RID: 12800
	[SerializeField]
	private UXSettings uxSettings;

	// Token: 0x04003201 RID: 12801
	[SerializeField]
	private float actionDelay = 0.5f;

	// Token: 0x04003202 RID: 12802
	[SerializeField]
	private float actionRepeatDelayReduction = 0.5f;

	// Token: 0x02000708 RID: 1800
	// (Invoke) Token: 0x06002CB5 RID: 11445
	public delegate void OnActionEvent();
}
