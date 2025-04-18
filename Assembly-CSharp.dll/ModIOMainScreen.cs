﻿using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200067B RID: 1659
public class ModIOMainScreen : ModIOScreen
{
	// Token: 0x0600291A RID: 10522 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void Initialize()
	{
	}

	// Token: 0x0600291B RID: 10523 RVA: 0x0004B0A9 File Offset: 0x000492A9
	public override void Show()
	{
		base.Show();
		this.UpdateOptionsText();
	}

	// Token: 0x0600291C RID: 10524 RVA: 0x00112318 File Offset: 0x00110518
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

	// Token: 0x0600291D RID: 10525 RVA: 0x0011239C File Offset: 0x0011059C
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

	// Token: 0x04002E50 RID: 11856
	[SerializeField]
	private string[] menuOptions;

	// Token: 0x04002E51 RID: 11857
	[SerializeField]
	private Text menuText;

	// Token: 0x04002E52 RID: 11858
	private int currentOption;
}
