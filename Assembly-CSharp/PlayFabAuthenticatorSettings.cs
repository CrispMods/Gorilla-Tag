using System;
using UnityEngine;

// Token: 0x020007AF RID: 1967
public class PlayFabAuthenticatorSettings
{
	// Token: 0x06003081 RID: 12417 RVA: 0x000E9A34 File Offset: 0x000E7C34
	static PlayFabAuthenticatorSettings()
	{
		PlayFabAuthenticatorSettings.Load("PlayFabAuthenticatorSettings");
	}

	// Token: 0x06003082 RID: 12418 RVA: 0x000E9A40 File Offset: 0x000E7C40
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

	// Token: 0x04003468 RID: 13416
	public static string TitleId;

	// Token: 0x04003469 RID: 13417
	public static string AuthApiBaseUrl;

	// Token: 0x0400346A RID: 13418
	public static string FriendApiBaseUrl;

	// Token: 0x0400346B RID: 13419
	public static string HpPromoApiBaseUrl;

	// Token: 0x0400346C RID: 13420
	public static string IapApiBaseUrl;

	// Token: 0x0400346D RID: 13421
	public static string ProgressionApiBaseUrl;

	// Token: 0x0400346E RID: 13422
	public static string TitleDataApiBaseUrl;

	// Token: 0x0400346F RID: 13423
	public static string VotingApiBaseUrl;
}
