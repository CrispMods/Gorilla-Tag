using System;
using TMPro;
using UnityEngine;

// Token: 0x02000675 RID: 1653
public class ModIOAccessScreen : ModIOScreen
{
	// Token: 0x060028F7 RID: 10487 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void Initialize()
	{
	}

	// Token: 0x060028F8 RID: 10488 RVA: 0x000C962A File Offset: 0x000C782A
	public override void Show()
	{
		base.Show();
		this.errorText.gameObject.SetActive(false);
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(true);
	}

	// Token: 0x060028F9 RID: 10489 RVA: 0x000C9665 File Offset: 0x000C7865
	public override void Hide()
	{
		this.errorText.gameObject.SetActive(false);
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(true);
		base.Hide();
	}

	// Token: 0x060028FA RID: 10490 RVA: 0x000C96A0 File Offset: 0x000C78A0
	public void Reset()
	{
		this.errorText.gameObject.SetActive(false);
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(true);
	}

	// Token: 0x060028FB RID: 10491 RVA: 0x000C96D8 File Offset: 0x000C78D8
	public void DisplayError(string errorMessage)
	{
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(false);
		this.errorText.text = errorMessage;
		this.errorText.gameObject.SetActive(true);
	}

	// Token: 0x060028FC RID: 10492 RVA: 0x000C9724 File Offset: 0x000C7924
	public void ShowTerminalControlPrompt()
	{
		this.errorText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(false);
		this.terminalControlPromptText.gameObject.SetActive(true);
	}

	// Token: 0x04002E04 RID: 11780
	[SerializeField]
	private TMP_Text errorText;

	// Token: 0x04002E05 RID: 11781
	[SerializeField]
	private TMP_Text loginRequiredText;

	// Token: 0x04002E06 RID: 11782
	[SerializeField]
	private TMP_Text terminalControlPromptText;
}
