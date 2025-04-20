using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020007C3 RID: 1987
public class PhotonAuthenticator : MonoBehaviour
{
	// Token: 0x0600311E RID: 12574 RVA: 0x000508DA File Offset: 0x0004EADA
	private void Awake()
	{
		Debug.Log("Environment is *************** PRODUCTION PUN *******************");
		PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = PhotonAuthenticatorSettings.PunAppId;
		PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice = PhotonAuthenticatorSettings.VoiceAppId;
	}

	// Token: 0x0600311F RID: 12575 RVA: 0x00132B68 File Offset: 0x00130D68
	public void SetCustomAuthenticationParameters(Dictionary<string, object> customAuthData)
	{
		AuthenticationValues authenticationValues = new AuthenticationValues();
		authenticationValues.AuthType = CustomAuthenticationType.Custom;
		authenticationValues.SetAuthPostData(customAuthData);
		NetworkSystem.Instance.SetAuthenticationValues(authenticationValues);
		Debug.Log("Set Photon auth data. AppVersion is: " + NetworkSystemConfig.AppVersion);
	}
}
