using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A0 RID: 416
public class DevConsoleHand : DevConsoleInstance
{
	// Token: 0x04000C59 RID: 3161
	public List<GameObject> otherButtonsList;

	// Token: 0x04000C5A RID: 3162
	public bool isStillEnabled = true;

	// Token: 0x04000C5B RID: 3163
	public bool isLeftHand;

	// Token: 0x04000C5C RID: 3164
	public ConsoleMode mode;

	// Token: 0x04000C5D RID: 3165
	public double debugScale;

	// Token: 0x04000C5E RID: 3166
	public double inspectorScale;

	// Token: 0x04000C5F RID: 3167
	public double componentInspectorScale;

	// Token: 0x04000C60 RID: 3168
	public List<GameObject> consoleButtons;

	// Token: 0x04000C61 RID: 3169
	public List<GameObject> inspectorButtons;

	// Token: 0x04000C62 RID: 3170
	public List<GameObject> componentInspectorButtons;

	// Token: 0x04000C63 RID: 3171
	public GorillaDevButton consoleButton;

	// Token: 0x04000C64 RID: 3172
	public GorillaDevButton inspectorButton;

	// Token: 0x04000C65 RID: 3173
	public GorillaDevButton componentInspectorButton;

	// Token: 0x04000C66 RID: 3174
	public GorillaDevButton showNonStarItems;

	// Token: 0x04000C67 RID: 3175
	public GorillaDevButton showPrivateItems;

	// Token: 0x04000C68 RID: 3176
	public Text componentInspectionText;

	// Token: 0x04000C69 RID: 3177
	public DevInspector selectedInspector;
}
