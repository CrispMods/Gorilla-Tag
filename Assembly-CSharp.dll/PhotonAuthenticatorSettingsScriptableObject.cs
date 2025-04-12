using System;
using UnityEngine;

// Token: 0x020007AE RID: 1966
[CreateAssetMenu(fileName = "PhotonAuthenticatorSettings", menuName = "ScriptableObjects/PhotonAuthenticatorSettings")]
public class PhotonAuthenticatorSettingsScriptableObject : ScriptableObject
{
	// Token: 0x0400346B RID: 13419
	public string PunAppId;

	// Token: 0x0400346C RID: 13420
	public string FusionAppId;

	// Token: 0x0400346D RID: 13421
	public string VoiceAppId;
}
