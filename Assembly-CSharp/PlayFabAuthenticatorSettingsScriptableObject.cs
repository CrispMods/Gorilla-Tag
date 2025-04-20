using System;
using UnityEngine;

// Token: 0x020007C8 RID: 1992
[CreateAssetMenu(fileName = "PlayFabAuthenticatorSettings", menuName = "ScriptableObjects/PlayFabAuthenticatorSettings")]
public class PlayFabAuthenticatorSettingsScriptableObject : ScriptableObject
{
	// Token: 0x0400351A RID: 13594
	public string TitleId;

	// Token: 0x0400351B RID: 13595
	public string AuthApiBaseUrl;

	// Token: 0x0400351C RID: 13596
	public string FriendApiBaseUrl;

	// Token: 0x0400351D RID: 13597
	public string HpPromoApiBaseUrl;

	// Token: 0x0400351E RID: 13598
	public string IapApiBaseUrl;

	// Token: 0x0400351F RID: 13599
	public string ProgressionApiBaseUrl;

	// Token: 0x04003520 RID: 13600
	public string TitleDataApiBaseUrl;

	// Token: 0x04003521 RID: 13601
	public string VotingApiBaseUrl;
}
