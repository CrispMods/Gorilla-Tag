using System;
using TMPro;
using UnityEngine;

// Token: 0x02000696 RID: 1686
public class CustomMapsAccessScreen : CustomMapsTerminalScreen
{
	// Token: 0x060029B9 RID: 10681 RVA: 0x00030607 File Offset: 0x0002E807
	public override void Initialize()
	{
	}

	// Token: 0x060029BA RID: 10682 RVA: 0x00117E48 File Offset: 0x00116048
	public override void Show()
	{
		base.Show();
		this.errorText.gameObject.SetActive(false);
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(true);
		for (int i = 0; i < this.buttonsToHide.Length; i++)
		{
			this.buttonsToHide[i].SetActive(false);
		}
	}

	// Token: 0x060029BB RID: 10683 RVA: 0x0004C2D8 File Offset: 0x0004A4D8
	public override void Hide()
	{
		this.errorText.gameObject.SetActive(false);
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(true);
		base.Hide();
	}

	// Token: 0x060029BC RID: 10684 RVA: 0x0004C313 File Offset: 0x0004A513
	public void Reset()
	{
		this.errorText.gameObject.SetActive(false);
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(true);
	}

	// Token: 0x060029BD RID: 10685 RVA: 0x00117EB0 File Offset: 0x001160B0
	public void DisplayError(string errorMessage)
	{
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(false);
		this.errorText.text = errorMessage;
		this.errorText.gameObject.SetActive(true);
	}

	// Token: 0x060029BE RID: 10686 RVA: 0x0004C348 File Offset: 0x0004A548
	public void ShowTerminalControlPrompt()
	{
		this.errorText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(false);
		this.terminalControlPromptText.gameObject.SetActive(true);
	}

	// Token: 0x04002F06 RID: 12038
	[SerializeField]
	private TMP_Text errorText;

	// Token: 0x04002F07 RID: 12039
	[SerializeField]
	private TMP_Text loginRequiredText;

	// Token: 0x04002F08 RID: 12040
	[SerializeField]
	private TMP_Text terminalControlPromptText;

	// Token: 0x04002F09 RID: 12041
	[SerializeField]
	private GameObject[] buttonsToHide;
}
