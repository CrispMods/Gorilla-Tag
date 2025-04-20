using System;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x0200071B RID: 1819
public class ControllerBehaviour : MonoBehaviour, IBuildValidation
{
	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x06002D37 RID: 11575 RVA: 0x0004EBED File Offset: 0x0004CDED
	public bool ButtonDown
	{
		get
		{
			return this.buttonDown;
		}
	}

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x06002D38 RID: 11576 RVA: 0x0004EBF5 File Offset: 0x0004CDF5
	public bool IsLeftStick
	{
		get
		{
			return this.isLeftStick;
		}
	}

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x06002D39 RID: 11577 RVA: 0x0004EBFD File Offset: 0x0004CDFD
	public bool IsRightStick
	{
		get
		{
			return this.isRightStick;
		}
	}

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x06002D3A RID: 11578 RVA: 0x0004EC05 File Offset: 0x0004CE05
	public bool IsUpStick
	{
		get
		{
			return this.isUpStick;
		}
	}

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x06002D3B RID: 11579 RVA: 0x0004EC0D File Offset: 0x0004CE0D
	public bool IsDownStick
	{
		get
		{
			return this.isDownStick;
		}
	}

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06002D3C RID: 11580 RVA: 0x00126D5C File Offset: 0x00124F5C
	// (remove) Token: 0x06002D3D RID: 11581 RVA: 0x00126D94 File Offset: 0x00124F94
	public event ControllerBehaviour.OnActionEvent OnAction;

	// Token: 0x06002D3E RID: 11582 RVA: 0x00126DCC File Offset: 0x00124FCC
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

	// Token: 0x06002D3F RID: 11583 RVA: 0x0012705C File Offset: 0x0012525C
	private void OnDisable()
	{
		this.buttonDown = (this.isLeftStick = (this.isRightStick = (this.isUpStick = (this.isDownStick = false))));
	}

	// Token: 0x06002D40 RID: 11584 RVA: 0x0004EC15 File Offset: 0x0004CE15
	public bool BuildValidationCheck()
	{
		if (this.uxSettings == null)
		{
			Debug.LogError("ControllerBehaviour must set UXSettings");
			return false;
		}
		return true;
	}

	// Token: 0x0400328E RID: 12942
	private InputDevice leftHandDevice;

	// Token: 0x0400328F RID: 12943
	private InputDevice rightHandDevice;

	// Token: 0x04003290 RID: 12944
	private float actionTime;

	// Token: 0x04003291 RID: 12945
	private bool buttonDown;

	// Token: 0x04003292 RID: 12946
	private float repeatAction = 1f;

	// Token: 0x04003293 RID: 12947
	private bool isLeftStick;

	// Token: 0x04003294 RID: 12948
	private bool isRightStick;

	// Token: 0x04003295 RID: 12949
	private bool isUpStick;

	// Token: 0x04003296 RID: 12950
	private bool isDownStick;

	// Token: 0x04003297 RID: 12951
	[SerializeField]
	private UXSettings uxSettings;

	// Token: 0x04003298 RID: 12952
	[SerializeField]
	private float actionDelay = 0.5f;

	// Token: 0x04003299 RID: 12953
	[SerializeField]
	private float actionRepeatDelayReduction = 0.5f;

	// Token: 0x0200071C RID: 1820
	// (Invoke) Token: 0x06002D43 RID: 11587
	public delegate void OnActionEvent();
}
