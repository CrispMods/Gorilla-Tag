using System;
using UnityEngine;

// Token: 0x020007AD RID: 1965
[CreateAssetMenu(fileName = "PhotonAuthenticatorSettings", menuName = "ScriptableObjects/PhotonAuthenticatorSettings")]
public class PhotonAuthenticatorSettingsScriptableObject : ScriptableObject
{
	// Token: 0x04003465 RID: 13413
	public string PunAppId;

	// Token: 0x04003466 RID: 13414
	public string FusionAppId;

	// Token: 0x04003467 RID: 13415
	public string VoiceAppId;
}
