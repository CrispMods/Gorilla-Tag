using System;
using Oculus.Platform;
using Oculus.Platform.Models;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000307 RID: 775
public class AppDeeplinkUI : MonoBehaviour
{
	// Token: 0x06001284 RID: 4740 RVA: 0x000B1460 File Offset: 0x000AF660
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

	// Token: 0x06001285 RID: 4741 RVA: 0x000B1570 File Offset: 0x000AF770
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

	// Token: 0x06001286 RID: 4742 RVA: 0x000B1648 File Offset: 0x000AF848
	private void LaunchUnrealDeeplinkSample()
	{
		Debug.Log(string.Format("LaunchOtherApp({0})", 4055411724486843UL));
		ApplicationOptions applicationOptions = new ApplicationOptions();
		applicationOptions.SetDeeplinkMessage(this.deeplinkMessage.GetComponentInChildren<Text>().text);
		Oculus.Platform.Application.LaunchOtherApp(4055411724486843UL, applicationOptions);
	}

	// Token: 0x06001287 RID: 4743 RVA: 0x000B16A0 File Offset: 0x000AF8A0
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

	// Token: 0x06001288 RID: 4744 RVA: 0x000B16F4 File Offset: 0x000AF8F4
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

	// Token: 0x0400147A RID: 5242
	private const ulong UNITY_COMPANION_APP_ID = 3535750239844224UL;

	// Token: 0x0400147B RID: 5243
	private const ulong UNREAL_COMPANION_APP_ID = 4055411724486843UL;

	// Token: 0x0400147C RID: 5244
	private RectTransform deeplinkAppId;

	// Token: 0x0400147D RID: 5245
	private RectTransform deeplinkMessage;

	// Token: 0x0400147E RID: 5246
	private RectTransform uiLaunchType;

	// Token: 0x0400147F RID: 5247
	private RectTransform uiLaunchSource;

	// Token: 0x04001480 RID: 5248
	private RectTransform uiDeepLinkMessage;

	// Token: 0x04001481 RID: 5249
	private bool inMenu = true;
}
