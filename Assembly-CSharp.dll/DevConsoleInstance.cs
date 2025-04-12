using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000196 RID: 406
public class DevConsoleInstance : MonoBehaviour
{
	// Token: 0x060009E2 RID: 2530 RVA: 0x00032A08 File Offset: 0x00030C08
	private void OnEnable()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000C25 RID: 3109
	public GorillaDevButton[] buttons;

	// Token: 0x04000C26 RID: 3110
	public GameObject[] disableWhileActive;

	// Token: 0x04000C27 RID: 3111
	public GameObject[] enableWhileActive;

	// Token: 0x04000C28 RID: 3112
	public float maxHeight;

	// Token: 0x04000C29 RID: 3113
	public float lineHeight;

	// Token: 0x04000C2A RID: 3114
	public int targetLogIndex = -1;

	// Token: 0x04000C2B RID: 3115
	public int currentLogIndex;

	// Token: 0x04000C2C RID: 3116
	public int expandAmount = 20;

	// Token: 0x04000C2D RID: 3117
	public int expandedMessageIndex = -1;

	// Token: 0x04000C2E RID: 3118
	public bool canExpand = true;

	// Token: 0x04000C2F RID: 3119
	public List<DevConsole.DisplayedLogLine> logLines = new List<DevConsole.DisplayedLogLine>();

	// Token: 0x04000C30 RID: 3120
	public HashSet<LogType> selectedLogTypes = new HashSet<LogType>
	{
		LogType.Error,
		LogType.Exception,
		LogType.Log,
		LogType.Warning,
		LogType.Assert
	};

	// Token: 0x04000C31 RID: 3121
	[SerializeField]
	private GorillaDevButton[] logTypeButtons;

	// Token: 0x04000C32 RID: 3122
	[SerializeField]
	private GorillaDevButton BottomButton;

	// Token: 0x04000C33 RID: 3123
	public float lineStartHeight;

	// Token: 0x04000C34 RID: 3124
	public float lineStartZ;

	// Token: 0x04000C35 RID: 3125
	public float textStartHeight;

	// Token: 0x04000C36 RID: 3126
	public float lineStartTextWidth;

	// Token: 0x04000C37 RID: 3127
	public double textScale = 0.5;

	// Token: 0x04000C38 RID: 3128
	public bool isEnabled = true;

	// Token: 0x04000C39 RID: 3129
	[SerializeField]
	private GameObject ConsoleLineExample;
}
