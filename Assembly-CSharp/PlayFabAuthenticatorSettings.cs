using System;
using UnityEngine;

// Token: 0x020007C7 RID: 1991
public class PlayFabAuthenticatorSettings
{
	// Token: 0x06003133 RID: 12595 RVA: 0x00050942 File Offset: 0x0004EB42
	static PlayFabAuthenticatorSettings()
	{
		PlayFabAuthenticatorSettings.Load("PlayFabAuthenticatorSettings");
	}

	// Token: 0x06003134 RID: 12596 RVA: 0x00132BA8 File Offset: 0x00130DA8
	public static void Load(string path)
	{
		PlayFabAuthenticatorSettingsScriptableObject playFabAuthenticatorSettingsScriptableObject = Resources.Load<PlayFabAuthenticatorSettingsScriptableObject>(path);
		PlayFabAuthenticatorSettings.TitleId = playFabAuthenticatorSettingsScriptableObject.TitleId;
		PlayFabAuthenticatorSettings.AuthApiBaseUrl = playFabAuthenticatorSettingsScriptableObject.AuthApiBaseUrl;
		PlayFabAuthenticatorSettings.FriendApiBaseUrl = playFabAuthenticatorSettingsScriptableObject.FriendApiBaseUrl;
		PlayFabAuthenticatorSettings.HpPromoApiBaseUrl = playFabAuthenticatorSettingsScriptableObject.HpPromoApiBaseUrl;
		PlayFabAuthenticatorSettings.IapApiBaseUrl = playFabAuthenticatorSettingsScriptableObject.IapApiBaseUrl;
		PlayFabAuthenticatorSettings.ProgressionApiBaseUrl = playFabAuthenticatorSettingsScriptableObject.ProgressionApiBaseUrl;
		PlayFabAuthenticatorSettings.TitleDataApiBaseUrl = playFabAuthenticatorSettingsScriptableObject.TitleDataApiBaseUrl;
		PlayFabAuthenticatorSettings.VotingApiBaseUrl = playFabAuthenticatorSettingsScriptableObject.VotingApiBaseUrl;
	}

	// Token: 0x04003512 RID: 13586
	public static string TitleId;

	// Token: 0x04003513 RID: 13587
	public static string AuthApiBaseUrl;

	// Token: 0x04003514 RID: 13588
	public static string FriendApiBaseUrl;

	// Token: 0x04003515 RID: 13589
	public static string HpPromoApiBaseUrl;

	// Token: 0x04003516 RID: 13590
	public static string IapApiBaseUrl;

	// Token: 0x04003517 RID: 13591
	public static string ProgressionApiBaseUrl;

	// Token: 0x04003518 RID: 13592
	public static string TitleDataApiBaseUrl;

	// Token: 0x04003519 RID: 13593
	public static string VotingApiBaseUrl;
}
