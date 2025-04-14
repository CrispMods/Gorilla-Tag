using System;
using UnityEngine;

// Token: 0x020007B0 RID: 1968
[CreateAssetMenu(fileName = "PlayFabAuthenticatorSettings", menuName = "ScriptableObjects/PlayFabAuthenticatorSettings")]
public class PlayFabAuthenticatorSettingsScriptableObject : ScriptableObject
{
	// Token: 0x04003470 RID: 13424
	public string TitleId;

	// Token: 0x04003471 RID: 13425
	public string AuthApiBaseUrl;

	// Token: 0x04003472 RID: 13426
	public string FriendApiBaseUrl;

	// Token: 0x04003473 RID: 13427
	public string HpPromoApiBaseUrl;

	// Token: 0x04003474 RID: 13428
	public string IapApiBaseUrl;

	// Token: 0x04003475 RID: 13429
	public string ProgressionApiBaseUrl;

	// Token: 0x04003476 RID: 13430
	public string TitleDataApiBaseUrl;

	// Token: 0x04003477 RID: 13431
	public string VotingApiBaseUrl;
}
