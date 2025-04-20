using System;
using UnityEngine;

// Token: 0x020007C5 RID: 1989
[CreateAssetMenu(fileName = "PhotonAuthenticatorSettings", menuName = "ScriptableObjects/PhotonAuthenticatorSettings")]
public class PhotonAuthenticatorSettingsScriptableObject : ScriptableObject
{
	// Token: 0x0400350F RID: 13583
	public string PunAppId;

	// Token: 0x04003510 RID: 13584
	public string FusionAppId;

	// Token: 0x04003511 RID: 13585
	public string VoiceAppId;
}
