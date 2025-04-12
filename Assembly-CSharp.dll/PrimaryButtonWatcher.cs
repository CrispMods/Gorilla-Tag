using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000483 RID: 1155
public class PrimaryButtonWatcher : MonoBehaviour
{
	// Token: 0x06001BF4 RID: 7156 RVA: 0x00042444 File Offset: 0x00040644
	private void Awake()
	{
		if (this.primaryButtonPress == null)
		{
			this.primaryButtonPress = new PrimaryButtonEvent();
		}
		this.devicesWithPrimaryButton = new List<InputDevice>();
	}

	// Token: 0x06001BF5 RID: 7157 RVA: 0x000D9150 File Offset: 0x000D7350
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

	// Token: 0x06001BF6 RID: 7158 RVA: 0x00042464 File Offset: 0x00040664
	private void OnDisable()
	{
		InputDevices.deviceConnected -= this.InputDevices_deviceConnected;
		InputDevices.deviceDisconnected -= this.InputDevices_deviceDisconnected;
		this.devicesWithPrimaryButton.Clear();
	}

	// Token: 0x06001BF7 RID: 7159 RVA: 0x000D91CC File Offset: 0x000D73CC
	private void InputDevices_deviceConnected(InputDevice device)
	{
		bool flag;
		if (device.TryGetFeatureValue(CommonUsages.primaryButton, out flag))
		{
			this.devicesWithPrimaryButton.Add(device);
		}
	}

	// Token: 0x06001BF8 RID: 7160 RVA: 0x00042493 File Offset: 0x00040693
	private void InputDevices_deviceDisconnected(InputDevice device)
	{
		if (this.devicesWithPrimaryButton.Contains(device))
		{
			this.devicesWithPrimaryButton.Remove(device);
		}
	}

	// Token: 0x06001BF9 RID: 7161 RVA: 0x000D91F8 File Offset: 0x000D73F8
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

	// Token: 0x04001F02 RID: 7938
	public PrimaryButtonEvent primaryButtonPress;

	// Token: 0x04001F03 RID: 7939
	private bool lastButtonState;

	// Token: 0x04001F04 RID: 7940
	private List<InputDevice> devicesWithPrimaryButton;
}
