using System;
using TMPro;
using UnityEngine;

// Token: 0x02000457 RID: 1111
public class GorillaDebugUI : MonoBehaviour
{
	// Token: 0x04001E40 RID: 7744
	private readonly float Delay = 0.5f;

	// Token: 0x04001E41 RID: 7745
	public GameObject parentCanvas;

	// Token: 0x04001E42 RID: 7746
	public GameObject rayInteractorLeft;

	// Token: 0x04001E43 RID: 7747
	public GameObject rayInteractorRight;

	// Token: 0x04001E44 RID: 7748
	[SerializeField]
	private TMP_Dropdown playfabIdDropdown;

	// Token: 0x04001E45 RID: 7749
	[SerializeField]
	private TMP_Dropdown roomIdDropdown;

	// Token: 0x04001E46 RID: 7750
	[SerializeField]
	private TMP_Dropdown locationDropdown;

	// Token: 0x04001E47 RID: 7751
	[SerializeField]
	private TMP_Dropdown playerNameDropdown;

	// Token: 0x04001E48 RID: 7752
	[SerializeField]
	private TMP_Dropdown gameModeDropdown;

	// Token: 0x04001E49 RID: 7753
	[SerializeField]
	private TMP_Dropdown timeOfDayDropdown;

	// Token: 0x04001E4A RID: 7754
	[SerializeField]
	private TMP_Text networkStateTextBox;

	// Token: 0x04001E4B RID: 7755
	[SerializeField]
	private TMP_Text gameModeTextBox;

	// Token: 0x04001E4C RID: 7756
	[SerializeField]
	private TMP_Text currentRoomTextBox;

	// Token: 0x04001E4D RID: 7757
	[SerializeField]
	private TMP_Text playerCountTextBox;

	// Token: 0x04001E4E RID: 7758
	[SerializeField]
	private TMP_Text roomVisibilityTextBox;

	// Token: 0x04001E4F RID: 7759
	[SerializeField]
	private TMP_Text timeMultiplierTextBox;

	// Token: 0x04001E50 RID: 7760
	[SerializeField]
	private TMP_Text versionTextBox;
}
