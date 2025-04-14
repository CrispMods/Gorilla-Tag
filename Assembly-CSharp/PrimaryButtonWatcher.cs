using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000483 RID: 1155
public class PrimaryButtonWatcher : MonoBehaviour
{
	// Token: 0x06001BF1 RID: 7153 RVA: 0x000880E3 File Offset: 0x000862E3
	private void Awake()
	{
		if (this.primaryButtonPress == null)
		{
			this.primaryButtonPress = new PrimaryButtonEvent();
		}
		this.devicesWithPrimaryButton = new List<InputDevice>();
	}

	// Token: 0x06001BF2 RID: 7154 RVA: 0x00088104 File Offset: 0x00086304
	private void OnEnable()
	{
		List<InputDevice> list = new List<InputDevice>();
		InputDevices.GetDevices(list);
		foreach (InputDevice device in list)
		{
			this.InputDevices_deviceConnected(device);
		}
		InputDevices.deviceConnected += this.InputDevices_deviceConnected;
		InputDevices.deviceDisconnected += this.InputDevices_deviceDisconnected;
	}

	// Token: 0x06001BF3 RID: 7155 RVA: 0x00088180 File Offset: 0x00086380
	private void OnDisable()
	{
		InputDevices.deviceConnected -= this.InputDevices_deviceConnected;
		InputDevices.deviceDisconnected -= this.InputDevices_deviceDisconnected;
		this.devicesWithPrimaryButton.Clear();
	}

	// Token: 0x06001BF4 RID: 7156 RVA: 0x000881B0 File Offset: 0x000863B0
	private void InputDevices_deviceConnected(InputDevice device)
	{
		bool flag;
		if (device.TryGetFeatureValue(CommonUsages.primaryButton, out flag))
		{
			this.devicesWithPrimaryButton.Add(device);
		}
	}

	// Token: 0x06001BF5 RID: 7157 RVA: 0x000881D9 File Offset: 0x000863D9
	private void InputDevices_deviceDisconnected(InputDevice device)
	{
		if (this.devicesWithPrimaryButton.Contains(device))
		{
			this.devicesWithPrimaryButton.Remove(device);
		}
	}

	// Token: 0x06001BF6 RID: 7158 RVA: 0x000881F8 File Offset: 0x000863F8
	private void Update()
	{
		bool flag = false;
		foreach (InputDevice inputDevice in this.devicesWithPrimaryButton)
		{
			bool flag2 = false;
			flag = ((inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out flag2) && flag2) || flag);
		}
		if (flag != this.lastButtonState)
		{
			this.primaryButtonPress.Invoke(flag);
			this.lastButtonState = flag;
		}
	}

	// Token: 0x04001F01 RID: 7937
	public PrimaryButtonEvent primaryButtonPress;

	// Token: 0x04001F02 RID: 7938
	private bool lastButtonState;

	// Token: 0x04001F03 RID: 7939
	private List<InputDevice> devicesWithPrimaryButton;
}
