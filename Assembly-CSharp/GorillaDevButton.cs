using System;
using UnityEngine;

// Token: 0x02000661 RID: 1633
public class GorillaDevButton : GorillaPressableButton
{
	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x06002871 RID: 10353 RVA: 0x000BF35F File Offset: 0x000BD55F
	// (set) Token: 0x06002872 RID: 10354 RVA: 0x000C6AF9 File Offset: 0x000C4CF9
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

	// Token: 0x06002873 RID: 10355 RVA: 0x000C6B11 File Offset: 0x000C4D11
	public void OnEnable()
	{
		this.UpdateColor();
	}

	// Token: 0x04002D55 RID: 11605
	public DevButtonType Type;

	// Token: 0x04002D56 RID: 11606
	public LogType levelType;

	// Token: 0x04002D57 RID: 11607
	public DevConsoleInstance targetConsole;

	// Token: 0x04002D58 RID: 11608
	public int lineNumber;

	// Token: 0x04002D59 RID: 11609
	public bool repeatIfHeld;

	// Token: 0x04002D5A RID: 11610
	public float holdForSeconds;

	// Token: 0x04002D5B RID: 11611
	private Coroutine pressCoroutine;
}
