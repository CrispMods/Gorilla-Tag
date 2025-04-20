using System;
using UnityEngine;

// Token: 0x02000662 RID: 1634
public class CustomMapsTerminalControlButton : GorillaPressableButton
{
	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x0600286E RID: 10350 RVA: 0x0004AF72 File Offset: 0x00049172
	// (set) Token: 0x0600286F RID: 10351 RVA: 0x0004B7AC File Offset: 0x000499AC
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

	// Token: 0x06002870 RID: 10352 RVA: 0x0004B7B5 File Offset: 0x000499B5
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		if (this.mapsTerminal == null)
		{
			return;
		}
		this.mapsTerminal.HandleTerminalControlButtonPressed();
	}

	// Token: 0x06002871 RID: 10353 RVA: 0x0011147C File Offset: 0x0010F67C
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

	// Token: 0x06002872 RID: 10354 RVA: 0x001114E0 File Offset: 0x0010F6E0
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

	// Token: 0x04002DD1 RID: 11729
	[SerializeField]
	private Color unlockedTextColor = Color.black;

	// Token: 0x04002DD2 RID: 11730
	[SerializeField]
	private Color lockedTextColor = Color.white;

	// Token: 0x04002DD3 RID: 11731
	[SerializeField]
	private CustomMapsTerminal mapsTerminal;
}
