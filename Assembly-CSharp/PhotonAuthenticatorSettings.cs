using System;
using UnityEngine;

// Token: 0x020007AC RID: 1964
public class PhotonAuthenticatorSettings
{
	// Token: 0x0600306F RID: 12399 RVA: 0x000E9A00 File Offset: 0x000E7C00
	static PhotonAuthenticatorSettings()
	{
		PhotonAuthenticatorSettings.Load("PhotonAuthenticatorSettings");
	}

	// Token: 0x06003070 RID: 12400 RVA: 0x000E9A0C File Offset: 0x000E7C0C
	public static void Load(string path)
	{
		PhotonAuthenticatorSettingsScriptableObject photonAuthenticatorSettingsScriptableObject = Resources.Load<PhotonAuthenticatorSettingsScriptableObject>(path);
		PhotonAuthenticatorSettings.PunAppId = photonAuthenticatorSettingsScriptableObject.PunAppId;
		PhotonAuthenticatorSettings.FusionAppId = photonAuthenticatorSettingsScriptableObject.FusionAppId;
		PhotonAuthenticatorSettings.VoiceAppId = photonAuthenticatorSettingsScriptableObject.VoiceAppId;
	}

	// Token: 0x04003462 RID: 13410
	public static string PunAppId;

	// Token: 0x04003463 RID: 13411
	public static string FusionAppId;

	// Token: 0x04003464 RID: 13412
	public static string VoiceAppId;
}
