using System;
using UnityEngine;

// Token: 0x020007C4 RID: 1988
public class PhotonAuthenticatorSettings
{
	// Token: 0x06003121 RID: 12577 RVA: 0x0005090E File Offset: 0x0004EB0E
	static PhotonAuthenticatorSettings()
	{
		PhotonAuthenticatorSettings.Load("PhotonAuthenticatorSettings");
	}

	// Token: 0x06003122 RID: 12578 RVA: 0x0005091A File Offset: 0x0004EB1A
	public static void Load(string path)
	{
		PhotonAuthenticatorSettingsScriptableObject photonAuthenticatorSettingsScriptableObject = Resources.Load<PhotonAuthenticatorSettingsScriptableObject>(path);
		PhotonAuthenticatorSettings.PunAppId = photonAuthenticatorSettingsScriptableObject.PunAppId;
		PhotonAuthenticatorSettings.FusionAppId = photonAuthenticatorSettingsScriptableObject.FusionAppId;
		PhotonAuthenticatorSettings.VoiceAppId = photonAuthenticatorSettingsScriptableObject.VoiceAppId;
	}

	// Token: 0x0400350C RID: 13580
	public static string PunAppId;

	// Token: 0x0400350D RID: 13581
	public static string FusionAppId;

	// Token: 0x0400350E RID: 13582
	public static string VoiceAppId;
}
