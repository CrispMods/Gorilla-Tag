using System;
using UnityEngine;

// Token: 0x02000615 RID: 1557
public class ModIOTerminalControlButton : GorillaPressableButton
{
	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x060026D8 RID: 9944 RVA: 0x000BF35F File Offset: 0x000BD55F
	// (set) Token: 0x060026D9 RID: 9945 RVA: 0x000BF367 File Offset: 0x000BD567
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

	// Token: 0x060026DA RID: 9946 RVA: 0x000BF370 File Offset: 0x000BD570
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		if (this.mapsTerminal == null)
		{
			return;
		}
		this.mapsTerminal.HandleTerminalControlButtonPressed();
	}

	// Token: 0x060026DB RID: 9947 RVA: 0x000BF394 File Offset: 0x000BD594
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

	// Token: 0x060026DC RID: 9948 RVA: 0x000BF3F8 File Offset: 0x000BD5F8
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

	// Token: 0x04002AB0 RID: 10928
	[SerializeField]
	private Color unlockedTextColor = Color.black;

	// Token: 0x04002AB1 RID: 10929
	[SerializeField]
	private Color lockedTextColor = Color.white;

	// Token: 0x04002AB2 RID: 10930
	[SerializeField]
	private ModIOMapsTerminal mapsTerminal;
}
