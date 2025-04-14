using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000195 RID: 405
public class DevConsoleHand : DevConsoleInstance
{
	// Token: 0x04000C14 RID: 3092
	public List<GameObject> otherButtonsList;

	// Token: 0x04000C15 RID: 3093
	public bool isStillEnabled = true;

	// Token: 0x04000C16 RID: 3094
	public bool isLeftHand;

	// Token: 0x04000C17 RID: 3095
	public ConsoleMode mode;

	// Token: 0x04000C18 RID: 3096
	public double debugScale;

	// Token: 0x04000C19 RID: 3097
	public double inspectorScale;

	// Token: 0x04000C1A RID: 3098
	public double componentInspectorScale;

	// Token: 0x04000C1B RID: 3099
	public List<GameObject> consoleButtons;

	// Token: 0x04000C1C RID: 3100
	public List<GameObject> inspectorButtons;

	// Token: 0x04000C1D RID: 3101
	public List<GameObject> componentInspectorButtons;

	// Token: 0x04000C1E RID: 3102
	public GorillaDevButton consoleButton;

	// Token: 0x04000C1F RID: 3103
	public GorillaDevButton inspectorButton;

	// Token: 0x04000C20 RID: 3104
	public GorillaDevButton componentInspectorButton;

	// Token: 0x04000C21 RID: 3105
	public GorillaDevButton showNonStarItems;

	// Token: 0x04000C22 RID: 3106
	public GorillaDevButton showPrivateItems;

	// Token: 0x04000C23 RID: 3107
	public Text componentInspectionText;

	// Token: 0x04000C24 RID: 3108
	public DevInspector selectedInspector;
}
