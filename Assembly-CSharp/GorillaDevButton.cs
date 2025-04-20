using System;
using UnityEngine;

// Token: 0x02000640 RID: 1600
public class GorillaDevButton : GorillaPressableButton
{
	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x0600279C RID: 10140 RVA: 0x0004AF72 File Offset: 0x00049172
	// (set) Token: 0x0600279D RID: 10141 RVA: 0x0004AF7A File Offset: 0x0004917A
	public bool on
	{
		get
		{
			return this.isOn;
		}
		set
		{
			if (this.isOn != value)
			{
				this.isOn = value;
				this.UpdateColor();
			}
		}
	}

	// Token: 0x0600279E RID: 10142 RVA: 0x0004AF92 File Offset: 0x00049192
	public void OnEnable()
	{
		this.UpdateColor();
	}

	// Token: 0x04002CBB RID: 11451
	public DevButtonType Type;

	// Token: 0x04002CBC RID: 11452
	public LogType levelType;

	// Token: 0x04002CBD RID: 11453
	public DevConsoleInstance targetConsole;

	// Token: 0x04002CBE RID: 11454
	public int lineNumber;

	// Token: 0x04002CBF RID: 11455
	public bool repeatIfHeld;

	// Token: 0x04002CC0 RID: 11456
	public float holdForSeconds;

	// Token: 0x04002CC1 RID: 11457
	private Coroutine pressCoroutine;
}
