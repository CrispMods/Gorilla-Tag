using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200067A RID: 1658
public class ModIOMainScreen : ModIOScreen
{
	// Token: 0x06002912 RID: 10514 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void Initialize()
	{
	}

	// Token: 0x06002913 RID: 10515 RVA: 0x000C9E8F File Offset: 0x000C808F
	public override void Show()
	{
		base.Show();
		this.UpdateOptionsText();
	}

	// Token: 0x06002914 RID: 10516 RVA: 0x000C9EA0 File Offset: 0x000C80A0
	protected override void PressButton(ModIOKeyboardButton.ModIOKeyboardBindings buttonPressed)
	{
		if (buttonPressed == ModIOKeyboardButton.ModIOKeyboardBindings.up)
		{
			this.currentOption--;
			if (this.currentOption < 0)
			{
				this.currentOption = this.menuOptions.Length - 1;
			}
			this.UpdateOptionsText();
			return;
		}
		if (buttonPressed == ModIOKeyboardButton.ModIOKeyboardBindings.down)
		{
			this.currentOption++;
			if (this.currentOption >= this.menuOptions.Length)
			{
				this.currentOption = 0;
			}
			this.UpdateOptionsText();
			return;
		}
		if (buttonPressed == ModIOKeyboardButton.ModIOKeyboardBindings.enter)
		{
			int num = this.currentOption;
			if (num != 0)
			{
			}
		}
	}

	// Token: 0x06002915 RID: 10517 RVA: 0x000C9F24 File Offset: 0x000C8124
	private void UpdateOptionsText()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < this.menuOptions.Length; i++)
		{
			stringBuilder.Append(this.menuOptions[i]);
			if (this.currentOption == i)
			{
				stringBuilder.Append(" <-");
			}
			if (i < this.menuOptions.Length - 1)
			{
				stringBuilder.Append("\n");
			}
		}
		this.menuText.text = stringBuilder.ToString();
	}

	// Token: 0x04002E4A RID: 11850
	[SerializeField]
	private string[] menuOptions;

	// Token: 0x04002E4B RID: 11851
	[SerializeField]
	private Text menuText;

	// Token: 0x04002E4C RID: 11852
	private int currentOption;
}
