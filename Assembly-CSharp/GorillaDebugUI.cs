using System;
using TMPro;
using UnityEngine;

// Token: 0x02000463 RID: 1123
public class GorillaDebugUI : MonoBehaviour
{
	// Token: 0x04001E8F RID: 7823
	private readonly float Delay = 0.5f;

	// Token: 0x04001E90 RID: 7824
	public GameObject parentCanvas;

	// Token: 0x04001E91 RID: 7825
	public GameObject rayInteractorLeft;

	// Token: 0x04001E92 RID: 7826
	public GameObject rayInteractorRight;

	// Token: 0x04001E93 RID: 7827
	[SerializeField]
	private TMP_Dropdown playfabIdDropdown;

	// Token: 0x04001E94 RID: 7828
	[SerializeField]
	private TMP_Dropdown roomIdDropdown;

	// Token: 0x04001E95 RID: 7829
	[SerializeField]
	private TMP_Dropdown locationDropdown;

	// Token: 0x04001E96 RID: 7830
	[SerializeField]
	private TMP_Dropdown playerNameDropdown;

	// Token: 0x04001E97 RID: 7831
	[SerializeField]
	private TMP_Dropdown gameModeDropdown;

	// Token: 0x04001E98 RID: 7832
	[SerializeField]
	private TMP_Dropdown timeOfDayDropdown;

	// Token: 0x04001E99 RID: 7833
	[SerializeField]
	private TMP_Text networkStateTextBox;

	// Token: 0x04001E9A RID: 7834
	[SerializeField]
	private TMP_Text gameModeTextBox;

	// Token: 0x04001E9B RID: 7835
	[SerializeField]
	private TMP_Text currentRoomTextBox;

	// Token: 0x04001E9C RID: 7836
	[SerializeField]
	private TMP_Text playerCountTextBox;

	// Token: 0x04001E9D RID: 7837
	[SerializeField]
	private TMP_Text roomVisibilityTextBox;

	// Token: 0x04001E9E RID: 7838
	[SerializeField]
	private TMP_Text timeMultiplierTextBox;

	// Token: 0x04001E9F RID: 7839
	[SerializeField]
	private TMP_Text versionTextBox;
}
