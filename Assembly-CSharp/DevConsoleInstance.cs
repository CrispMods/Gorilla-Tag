using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class DevConsoleInstance : MonoBehaviour
{
	// Token: 0x06000A2C RID: 2604 RVA: 0x00033C0F File Offset: 0x00031E0F
	private void OnEnable()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000C6A RID: 3178
	public GorillaDevButton[] buttons;

	// Token: 0x04000C6B RID: 3179
	public GameObject[] disableWhileActive;

	// Token: 0x04000C6C RID: 3180
	public GameObject[] enableWhileActive;

	// Token: 0x04000C6D RID: 3181
	public float maxHeight;

	// Token: 0x04000C6E RID: 3182
	public float lineHeight;

	// Token: 0x04000C6F RID: 3183
	public int targetLogIndex = -1;

	// Token: 0x04000C70 RID: 3184
	public int currentLogIndex;

	// Token: 0x04000C71 RID: 3185
	public int expandAmount = 20;

	// Token: 0x04000C72 RID: 3186
	public int expandedMessageIndex = -1;

	// Token: 0x04000C73 RID: 3187
	public bool canExpand = true;

	// Token: 0x04000C74 RID: 3188
	public List<DevConsole.DisplayedLogLine> logLines = new List<DevConsole.DisplayedLogLine>();

	// Token: 0x04000C75 RID: 3189
	public HashSet<LogType> selectedLogTypes = new HashSet<LogType>
	{
		LogType.Error,
		LogType.Exception,
		LogType.Log,
		LogType.Warning,
		LogType.Assert
	};

	// Token: 0x04000C76 RID: 3190
	[SerializeField]
	private GorillaDevButton[] logTypeButtons;

	// Token: 0x04000C77 RID: 3191
	[SerializeField]
	private GorillaDevButton BottomButton;

	// Token: 0x04000C78 RID: 3192
	public float lineStartHeight;

	// Token: 0x04000C79 RID: 3193
	public float lineStartZ;

	// Token: 0x04000C7A RID: 3194
	public float textStartHeight;

	// Token: 0x04000C7B RID: 3195
	public float lineStartTextWidth;

	// Token: 0x04000C7C RID: 3196
	public double textScale = 0.5;

	// Token: 0x04000C7D RID: 3197
	public bool isEnabled = true;

	// Token: 0x04000C7E RID: 3198
	[SerializeField]
	private GameObject ConsoleLineExample;
}
