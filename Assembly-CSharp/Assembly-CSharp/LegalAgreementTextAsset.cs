using System;
using UnityEngine;

// Token: 0x02000719 RID: 1817
[CreateAssetMenu(fileName = "NewLegalAgreementAsset", menuName = "Gorilla Tag/Legal Agreement Asset")]
public class LegalAgreementTextAsset : ScriptableObject
{
	// Token: 0x0400325F RID: 12895
	public string title;

	// Token: 0x04003260 RID: 12896
	public string playFabKey;

	// Token: 0x04003261 RID: 12897
	public string latestVersionKey;

	// Token: 0x04003262 RID: 12898
	[TextArea(3, 5)]
	public string errorMessage;

	// Token: 0x04003263 RID: 12899
	public bool optional;

	// Token: 0x04003264 RID: 12900
	public LegalAgreementTextAsset.PostAcceptAction optInAction;

	// Token: 0x0200071A RID: 1818
	public enum PostAcceptAction
	{
		// Token: 0x04003266 RID: 12902
		NONE
	}
}
