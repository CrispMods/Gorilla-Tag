using System;
using UnityEngine;

// Token: 0x0200072D RID: 1837
[CreateAssetMenu(fileName = "NewLegalAgreementAsset", menuName = "Gorilla Tag/Legal Agreement Asset")]
public class LegalAgreementTextAsset : ScriptableObject
{
	// Token: 0x040032F6 RID: 13046
	public string title;

	// Token: 0x040032F7 RID: 13047
	public string playFabKey;

	// Token: 0x040032F8 RID: 13048
	public string latestVersionKey;

	// Token: 0x040032F9 RID: 13049
	[TextArea(3, 5)]
	public string errorMessage;

	// Token: 0x040032FA RID: 13050
	public bool optional;

	// Token: 0x040032FB RID: 13051
	public LegalAgreementTextAsset.PostAcceptAction optInAction;

	// Token: 0x0200072E RID: 1838
	public enum PostAcceptAction
	{
		// Token: 0x040032FD RID: 13053
		NONE
	}
}
