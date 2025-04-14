using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A0 RID: 416
public class DevInspectorScanner : MonoBehaviour
{
	// Token: 0x04000C4D RID: 3149
	public Text hintTextOutput;

	// Token: 0x04000C4E RID: 3150
	public float scanDistance = 10f;

	// Token: 0x04000C4F RID: 3151
	public float scanAngle = 30f;

	// Token: 0x04000C50 RID: 3152
	public LayerMask scanLayerMask;

	// Token: 0x04000C51 RID: 3153
	public string targetComponentName;

	// Token: 0x04000C52 RID: 3154
	public float rayPerDegree = 10f;
}
