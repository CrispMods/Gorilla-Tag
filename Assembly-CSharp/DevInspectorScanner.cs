using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001AB RID: 427
public class DevInspectorScanner : MonoBehaviour
{
	// Token: 0x04000C93 RID: 3219
	public Text hintTextOutput;

	// Token: 0x04000C94 RID: 3220
	public float scanDistance = 10f;

	// Token: 0x04000C95 RID: 3221
	public float scanAngle = 30f;

	// Token: 0x04000C96 RID: 3222
	public LayerMask scanLayerMask;

	// Token: 0x04000C97 RID: 3223
	public string targetComponentName;

	// Token: 0x04000C98 RID: 3224
	public float rayPerDegree = 10f;
}
