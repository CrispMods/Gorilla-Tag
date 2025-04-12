using System;
using TMPro;
using UnityEngine;

// Token: 0x02000675 RID: 1653
public class ModIOAccessScreen : ModIOScreen
{
	// Token: 0x060028F7 RID: 10487 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void Initialize()
	{
	}

	// Token: 0x060028F8 RID: 10488 RVA: 0x0004AF13 File Offset: 0x00049113
	public override void Show()
	{
		base.Show();
		this.errorText.gameObject.SetActive(false);
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(true);
	}

	// Token: 0x060028F9 RID: 10489 RVA: 0x0004AF4E File Offset: 0x0004914E
	public override void Hide()
	{
		this.errorText.gameObject.SetActive(false);
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(true);
		base.Hide();
	}

	// Token: 0x060028FA RID: 10490 RVA: 0x0004AF89 File Offset: 0x00049189
	public void Reset()
	{
		this.errorText.gameObject.SetActive(false);
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(true);
	}

	// Token: 0x060028FB RID: 10491 RVA: 0x001117CC File Offset: 0x0010F9CC
	public void DisplayError(string errorMessage)
	{
		this.terminalControlPromptText.gameObject.SetActive(false);
		this.loginRequiredText.gameObject.SetActive(false);
		this.errorText.text = errorMessage;
		this.errorText.gameObject.SetActive(true);
	}

	// Token: 0x060028FC RID: 10492 RVA: 0x0004AFBE File Offset: 0x000491BE
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
