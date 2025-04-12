using System;
using UnityEngine;

// Token: 0x020007B1 RID: 1969
[CreateAssetMenu(fileName = "PlayFabAuthenticatorSettings", menuName = "ScriptableObjects/PlayFabAuthenticatorSettings")]
public class PlayFabAuthenticatorSettingsScriptableObject : ScriptableObject
{
	// Token: 0x04003476 RID: 13430
	public string TitleId;

	// Token: 0x04003477 RID: 13431
	public string AuthApiBaseUrl;

	// Token: 0x04003478 RID: 13432
	public string FriendApiBaseUrl;

	// Token: 0x04003479 RID: 13433
	public string HpPromoApiBaseUrl;

	// Token: 0x0400347A RID: 13434
	public string IapApiBaseUrl;

	// Token: 0x0400347B RID: 13435
	public string ProgressionApiBaseUrl;

	// Token: 0x0400347C RID: 13436
	public string TitleDataApiBaseUrl;

	// Token: 0x0400347D RID: 13437
	public string VotingApiBaseUrl;
}
