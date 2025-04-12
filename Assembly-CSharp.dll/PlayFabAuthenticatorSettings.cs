using System;
using UnityEngine;

// Token: 0x020007B0 RID: 1968
public class PlayFabAuthenticatorSettings
{
	// Token: 0x06003089 RID: 12425 RVA: 0x0004F540 File Offset: 0x0004D740
	static PlayFabAuthenticatorSettings()
	{
		PlayFabAuthenticatorSettings.Load("PlayFabAuthenticatorSettings");
	}

	// Token: 0x0600308A RID: 12426 RVA: 0x0012D988 File Offset: 0x0012BB88
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

	// Token: 0x0400346E RID: 13422
	public static string TitleId;

	// Token: 0x0400346F RID: 13423
	public static string AuthApiBaseUrl;

	// Token: 0x04003470 RID: 13424
	public static string FriendApiBaseUrl;

	// Token: 0x04003471 RID: 13425
	public static string HpPromoApiBaseUrl;

	// Token: 0x04003472 RID: 13426
	public static string IapApiBaseUrl;

	// Token: 0x04003473 RID: 13427
	public static string ProgressionApiBaseUrl;

	// Token: 0x04003474 RID: 13428
	public static string TitleDataApiBaseUrl;

	// Token: 0x04003475 RID: 13429
	public static string VotingApiBaseUrl;
}
