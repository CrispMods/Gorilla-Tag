﻿using System;
using Oculus.Platform;
using Oculus.Platform.Models;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002FC RID: 764
public class AppDeeplinkUI : MonoBehaviour
{
	// Token: 0x06001238 RID: 4664 RVA: 0x00056EA4 File Offset: 0x000550A4
	private void Start()
	{
		DebugUIBuilder instance = DebugUIBuilder.instance;
		this.uiLaunchType = instance.AddLabel("UnityDeeplinkSample", 0);
		instance.AddDivider(0);
		instance.AddButton("launch OtherApp", new DebugUIBuilder.OnClick(this.LaunchOtherApp), -1, 0, false);
		instance.AddButton("launch UnrealDeeplinkSample", new DebugUIBuilder.OnClick(this.LaunchUnrealDeeplinkSample), -1, 0, false);
		this.deeplinkAppId = CustomDebugUI.instance.AddTextField(3535750239844224UL.ToString(), 0);
		this.deeplinkMessage = CustomDebugUI.instance.AddTextField("MSG_UNITY_SAMPLE", 0);
		instance.AddButton("LaunchSelf", new DebugUIBuilder.OnClick(this.LaunchSelf), -1, 0, false);
		if (UnityEngine.Application.platform == RuntimePlatform.Android && !Core.IsInitialized())
		{
			Core.Initialize(null);
		}
		this.uiLaunchType = instance.AddLabel("LaunchType: ", 0);
		this.uiLaunchSource = instance.AddLabel("LaunchSource: ", 0);
		this.uiDeepLinkMessage = instance.AddLabel("DeeplinkMessage: ", 0);
		instance.ToggleLaserPointer(true);
		instance.Show();
	}

	// Token: 0x06001239 RID: 4665 RVA: 0x00056FB4 File Offset: 0x000551B4
	private void Update()
	{
		DebugUIBuilder instance = DebugUIBuilder.instance;
		if (UnityEngine.Application.platform == RuntimePlatform.Android)
		{
			LaunchDetails launchDetails = ApplicationLifecycle.GetLaunchDetails();
			this.uiLaunchType.GetComponentInChildren<Text>().text = "LaunchType: " + launchDetails.LaunchType.ToString();
			this.uiLaunchSource.GetComponentInChildren<Text>().text = "LaunchSource: " + launchDetails.LaunchSource;
			this.uiDeepLinkMessage.GetComponentInChildren<Text>().text = "DeeplinkMessage: " + launchDetails.DeeplinkMessage;
		}
		if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.Active) || OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.Active))
		{
			if (this.inMenu)
			{
				DebugUIBuilder.instance.Hide();
			}
			else
			{
				DebugUIBuilder.instance.Show();
			}
			this.inMenu = !this.inMenu;
		}
	}

	// Token: 0x0600123A RID: 4666 RVA: 0x0005708C File Offset: 0x0005528C
	private void LaunchUnrealDeeplinkSample()
	{
		Debug.Log(string.Format("LaunchOtherApp({0})", 4055411724486843UL));
		ApplicationOptions applicationOptions = new ApplicationOptions();
		applicationOptions.SetDeeplinkMessage(this.deeplinkMessage.GetComponentInChildren<Text>().text);
		Oculus.Platform.Application.LaunchOtherApp(4055411724486843UL, applicationOptions);
	}

	// Token: 0x0600123B RID: 4667 RVA: 0x000570E4 File Offset: 0x000552E4
	private void LaunchSelf()
	{
		ulong num;
		if (ulong.TryParse(PlatformSettings.MobileAppID, out num))
		{
			Debug.Log(string.Format("LaunchSelf({0})", num));
			ApplicationOptions applicationOptions = new ApplicationOptions();
			applicationOptions.SetDeeplinkMessage(this.deeplinkMessage.GetComponentInChildren<Text>().text);
			Oculus.Platform.Application.LaunchOtherApp(num, applicationOptions);
		}
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x00057138 File Offset: 0x00055338
	private void LaunchOtherApp()
	{
		ulong num;
		if (ulong.TryParse(this.deeplinkAppId.GetComponentInChildren<Text>().text, out num))
		{
			Debug.Log(string.Format("LaunchOtherApp({0})", num));
			ApplicationOptions applicationOptions = new ApplicationOptions();
			applicationOptions.SetDeeplinkMessage(this.deeplinkMessage.GetComponentInChildren<Text>().text);
			Oculus.Platform.Application.LaunchOtherApp(num, applicationOptions);
		}
	}

	// Token: 0x04001432 RID: 5170
	private const ulong UNITY_COMPANION_APP_ID = 3535750239844224UL;

	// Token: 0x04001433 RID: 5171
	private const ulong UNREAL_COMPANION_APP_ID = 4055411724486843UL;

	// Token: 0x04001434 RID: 5172
	private RectTransform deeplinkAppId;

	// Token: 0x04001435 RID: 5173
	private RectTransform deeplinkMessage;

	// Token: 0x04001436 RID: 5174
	private RectTransform uiLaunchType;

	// Token: 0x04001437 RID: 5175
	private RectTransform uiLaunchSource;

	// Token: 0x04001438 RID: 5176
	private RectTransform uiDeepLinkMessage;

	// Token: 0x04001439 RID: 5177
	private bool inMenu = true;
}
