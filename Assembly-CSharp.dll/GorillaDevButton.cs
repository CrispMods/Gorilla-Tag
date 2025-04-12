using System;
using UnityEngine;

// Token: 0x02000662 RID: 1634
public class GorillaDevButton : GorillaPressableButton
{
	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x06002879 RID: 10361 RVA: 0x000497AC File Offset: 0x000479AC
	// (set) Token: 0x0600287A RID: 10362 RVA: 0x0004A9DD File Offset: 0x00048BDD
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

	// Token: 0x0600287B RID: 10363 RVA: 0x0004A9F5 File Offset: 0x00048BF5
	public void OnEnable()
	{
		this.UpdateColor();
	}

	// Token: 0x04002D5B RID: 11611
	public DevButtonType Type;

	// Token: 0x04002D5C RID: 11612
	public LogType levelType;

	// Token: 0x04002D5D RID: 11613
	public DevConsoleInstance targetConsole;

	// Token: 0x04002D5E RID: 11614
	public int lineNumber;

	// Token: 0x04002D5F RID: 11615
	public bool repeatIfHeld;

	// Token: 0x04002D60 RID: 11616
	public float holdForSeconds;

	// Token: 0x04002D61 RID: 11617
	private Coroutine pressCoroutine;
}
