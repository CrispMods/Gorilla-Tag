using System;
using UnityEngine;

// Token: 0x02000718 RID: 1816
[CreateAssetMenu(fileName = "NewLegalAgreementAsset", menuName = "Gorilla Tag/Legal Agreement Asset")]
public class LegalAgreementTextAsset : ScriptableObject
{
	// Token: 0x04003259 RID: 12889
	public string title;

	// Token: 0x0400325A RID: 12890
	public string playFabKey;

	// Token: 0x0400325B RID: 12891
	public string latestVersionKey;

	// Token: 0x0400325C RID: 12892
	[TextArea(3, 5)]
	public string errorMessage;

	// Token: 0x0400325D RID: 12893
	public bool optional;

	// Token: 0x0400325E RID: 12894
	public LegalAgreementTextAsset.PostAcceptAction optInAction;

	// Token: 0x02000719 RID: 1817
	public enum PostAcceptAction
	{
		// Token: 0x04003260 RID: 12896
		NONE
	}
}
