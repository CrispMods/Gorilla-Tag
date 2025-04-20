using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x0200048F RID: 1167
public class PrimaryButtonWatcher : MonoBehaviour
{
	// Token: 0x06001C45 RID: 7237 RVA: 0x0004377D File Offset: 0x0004197D
	private void Awake()
	{
		if (this.primaryButtonPress == null)
		{
			this.primaryButtonPress = new PrimaryButtonEvent();
		}
		this.devicesWithPrimaryButton = new List<InputDevice>();
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x000DBE00 File Offset: 0x000DA000
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

	// Token: 0x06001C47 RID: 7239 RVA: 0x0004379D File Offset: 0x0004199D
	private void OnDisable()
	{
		InputDevices.deviceConnected -= this.InputDevices_deviceConnected;
		InputDevices.deviceDisconnected -= this.InputDevices_deviceDisconnected;
		this.devicesWithPrimaryButton.Clear();
	}

	// Token: 0x06001C48 RID: 7240 RVA: 0x000DBE7C File Offset: 0x000DA07C
	private void InputDevices_deviceConnected(InputDevice device)
	{
		bool flag;
		if (device.TryGetFeatureValue(CommonUsages.primaryButton, out flag))
		{
			this.devicesWithPrimaryButton.Add(device);
		}
	}

	// Token: 0x06001C49 RID: 7241 RVA: 0x000437CC File Offset: 0x000419CC
	private void InputDevices_deviceDisconnected(InputDevice device)
	{
		if (this.devicesWithPrimaryButton.Contains(device))
		{
			this.devicesWithPrimaryButton.Remove(device);
		}
	}

	// Token: 0x06001C4A RID: 7242 RVA: 0x000DBEA8 File Offset: 0x000DA0A8
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

	// Token: 0x04001F50 RID: 8016
	public PrimaryButtonEvent primaryButtonPress;

	// Token: 0x04001F51 RID: 8017
	private bool lastButtonState;

	// Token: 0x04001F52 RID: 8018
	private List<InputDevice> devicesWithPrimaryButton;
}
