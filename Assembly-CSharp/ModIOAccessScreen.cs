using System;
using TMPro;
using UnityEngine;

// Token: 0x02000674 RID: 1652
public class ModIOAccessScreen : ModIOScreen
{
	// Token: 0x060028EF RID: 10479 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void Initialize()
	{
	}

	// Token: 0x060028F0 RID: 10480 RVA: 0x000C91AA File Offset: 0x000C73AA
	public override void Show()
	{
		base.Show();
		this.errorText.gameObject.SetActive(false);
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(true);
	}

	// Token: 0x060028F1 RID: 10481 RVA: 0x000C91E5 File Offset: 0x000C73E5
	public override void Hide()
	{
		this.errorText.gameObject.SetActive(false);
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(true);
		base.Hide();
	}

	// Token: 0x060028F2 RID: 10482 RVA: 0x000C9220 File Offset: 0x000C7420
	public void Reset()
	{
		this.errorText.gameObject.SetActive(false);
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(true);
	}

	// Token: 0x060028F3 RID: 10483 RVA: 0x000C9258 File Offset: 0x000C7458
	public void DisplayError(string errorMessage)
	{
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(false);
		this.errorText.text = errorMessage;
		this.errorText.gameObject.SetActive(true);
	}

	// Token: 0x060028F4 RID: 10484 RVA: 0x000C92A4 File Offset: 0x000C74A4
	public void ShowTerminalControlPrompt()
	{
		this.errorText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(false);
		this.terminalControlPromptText.gameObject.SetActive(true);
	}

	// Token: 0x04002DFE RID: 11774
	[SerializeField]
	private TMP_Text errorText;

	// Token: 0x04002DFF RID: 11775
	[SerializeField]
	private TMP_Text loginRequiredText;

	// Token: 0x04002E00 RID: 11776
	[SerializeField]
	private TMP_Text terminalControlPromptText;
}
