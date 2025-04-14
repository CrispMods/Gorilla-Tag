using System;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x02000706 RID: 1798
public class ControllerBehaviour : MonoBehaviour, IBuildValidation
{
	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x06002CA1 RID: 11425 RVA: 0x000DC59F File Offset: 0x000DA79F
	public bool ButtonDown
	{
		get
		{
			return this.buttonDown;
		}
	}

	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x06002CA2 RID: 11426 RVA: 0x000DC5A7 File Offset: 0x000DA7A7
	public bool IsLeftStick
	{
		get
		{
			return this.isLeftStick;
		}
	}

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x06002CA3 RID: 11427 RVA: 0x000DC5AF File Offset: 0x000DA7AF
	public bool IsRightStick
	{
		get
		{
			return this.isRightStick;
		}
	}

	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x06002CA4 RID: 11428 RVA: 0x000DC5B7 File Offset: 0x000DA7B7
	public bool IsUpStick
	{
		get
		{
			return this.isUpStick;
		}
	}

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x06002CA5 RID: 11429 RVA: 0x000DC5BF File Offset: 0x000DA7BF
	public bool IsDownStick
	{
		get
		{
			return this.isDownStick;
		}
	}

	// Token: 0x14000054 RID: 84
	// (add) Token: 0x06002CA6 RID: 11430 RVA: 0x000DC5C8 File Offset: 0x000DA7C8
	// (remove) Token: 0x06002CA7 RID: 11431 RVA: 0x000DC600 File Offset: 0x000DA800
	public event ControllerBehaviour.OnActionEvent OnAction;

	// Token: 0x06002CA8 RID: 11432 RVA: 0x000DC638 File Offset: 0x000DA838
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

	// Token: 0x06002CA9 RID: 11433 RVA: 0x000DC8C8 File Offset: 0x000DAAC8
	private void OnDisable()
	{
		this.buttonDown = (this.isLeftStick = (this.isRightStick = (this.isUpStick = (this.isDownStick = false))));
	}

	// Token: 0x06002CAA RID: 11434 RVA: 0x000DC900 File Offset: 0x000DAB00
	public bool BuildValidationCheck()
	{
		if (this.uxSettings == null)
		{
			Debug.LogError("ControllerBehaviour must set UXSettings");
			return false;
		}
		return true;
	}

	// Token: 0x040031F1 RID: 12785
	private InputDevice leftHandDevice;

	// Token: 0x040031F2 RID: 12786
	private InputDevice rightHandDevice;

	// Token: 0x040031F3 RID: 12787
	private float actionTime;

	// Token: 0x040031F4 RID: 12788
	private bool buttonDown;

	// Token: 0x040031F5 RID: 12789
	private float repeatAction = 1f;

	// Token: 0x040031F6 RID: 12790
	private bool isLeftStick;

	// Token: 0x040031F7 RID: 12791
	private bool isRightStick;

	// Token: 0x040031F8 RID: 12792
	private bool isUpStick;

	// Token: 0x040031F9 RID: 12793
	private bool isDownStick;

	// Token: 0x040031FA RID: 12794
	[SerializeField]
	private UXSettings uxSettings;

	// Token: 0x040031FB RID: 12795
	[SerializeField]
	private float actionDelay = 0.5f;

	// Token: 0x040031FC RID: 12796
	[SerializeField]
	private float actionRepeatDelayReduction = 0.5f;

	// Token: 0x02000707 RID: 1799
	// (Invoke) Token: 0x06002CAD RID: 11437
	public delegate void OnActionEvent();
}
