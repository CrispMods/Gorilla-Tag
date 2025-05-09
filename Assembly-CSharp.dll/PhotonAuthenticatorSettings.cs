﻿using System;
using UnityEngine;

// Token: 0x020007AD RID: 1965
public class PhotonAuthenticatorSettings
{
	// Token: 0x06003077 RID: 12407 RVA: 0x0004F50C File Offset: 0x0004D70C
	static PhotonAuthenticatorSettings()
	{
		PhotonAuthenticatorSettings.Load("PhotonAuthenticatorSettings");
	}

	// Token: 0x06003078 RID: 12408 RVA: 0x0004F518 File Offset: 0x0004D718
	public static void Load(string path)
	{
		PhotonAuthenticatorSettingsScriptableObject photonAuthenticatorSettingsScriptableObject = Resources.Load<PhotonAuthenticatorSettingsScriptableObject>(path);
		PhotonAuthenticatorSettings.PunAppId = photonAuthenticatorSettingsScriptableObject.PunAppId;
		PhotonAuthenticatorSettings.FusionAppId = photonAuthenticatorSettingsScriptableObject.FusionAppId;
		PhotonAuthenticatorSettings.VoiceAppId = photonAuthenticatorSettingsScriptableObject.VoiceAppId;
	}

	// Token: 0x04003468 RID: 13416
	public static string PunAppId;

	// Token: 0x04003469 RID: 13417
	public static string FusionAppId;

	// Token: 0x0400346A RID: 13418
	public static string VoiceAppId;
}
