using System;
using UnityEngine;

// Token: 0x02000616 RID: 1558
public class ModIOTerminalControlButton : GorillaPressableButton
{
	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x060026E0 RID: 9952 RVA: 0x000BF7DF File Offset: 0x000BD9DF
	// (set) Token: 0x060026E1 RID: 9953 RVA: 0x000BF7E7 File Offset: 0x000BD9E7
	public bool IsLocked
	{
		get
		{
			return this.isOn;
		}
		set
		{
			this.isOn = value;
		}
	}

	// Token: 0x060026E2 RID: 9954 RVA: 0x000BF7F0 File Offset: 0x000BD9F0
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		if (this.mapsTerminal == null)
		{
			return;
		}
		this.mapsTerminal.HandleTerminalControlButtonPressed();
	}

	// Token: 0x060026E3 RID: 9955 RVA: 0x000BF814 File Offset: 0x000BDA14
	public void LockTerminalControl()
	{
		if (this.IsLocked)
		{
			return;
		}
		this.IsLocked = true;
		this.UpdateColor();
		if (this.myText != null)
		{
			this.myText.color = this.lockedTextColor;
			return;
		}
		if (this.myTmpText != null)
		{
			this.myTmpText.color = this.lockedTextColor;
		}
	}

	// Token: 0x060026E4 RID: 9956 RVA: 0x000BF878 File Offset: 0x000BDA78
	public void UnlockTerminalControl()
	{
		if (!this.IsLocked)
		{
			return;
		}
		this.IsLocked = false;
		this.UpdateColor();
		if (this.myText != null)
		{
			this.myText.color = this.unlockedTextColor;
			return;
		}
		if (this.myTmpText != null)
		{
			this.myTmpText.color = this.unlockedTextColor;
		}
	}

	// Token: 0x04002AB6 RID: 10934
	[SerializeField]
	private Color unlockedTextColor = Color.black;

	// Token: 0x04002AB7 RID: 10935
	[SerializeField]
	private Color lockedTextColor = Color.white;

	// Token: 0x04002AB8 RID: 10936
	[SerializeField]
	private ModIOMapsTerminal mapsTerminal;
}
