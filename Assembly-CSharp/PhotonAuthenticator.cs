using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020007AB RID: 1963
public class PhotonAuthenticator : MonoBehaviour
{
	// Token: 0x0600306C RID: 12396 RVA: 0x000E998A File Offset: 0x000E7B8A
	private void Awake()
	{
		Debug.Log("Environment is *************** PRODUCTION PUN *******************");
		PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = PhotonAuthenticatorSettings.PunAppId;
		PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice = PhotonAuthenticatorSettings.VoiceAppId;
	}

	// Token: 0x0600306D RID: 12397 RVA: 0x000E99C0 File Offset: 0x000E7BC0
	public void SetCustomAuthenticationParameters(Dictionary<string, object> customAuthData)
	{
		AuthenticationValues authenticationValues = new AuthenticationValues();
		authenticationValues.AuthType = CustomAuthenticationType.Custom;
		authenticationValues.SetAuthPostData(customAuthData);
		NetworkSystem.Instance.SetAuthenticationValues(authenticationValues);
		Debug.Log("Set Photon auth data. AppVersion is: " + NetworkSystemConfig.AppVersion);
	}
}
