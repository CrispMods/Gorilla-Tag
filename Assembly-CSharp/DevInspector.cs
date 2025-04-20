using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A9 RID: 425
public class DevInspector : MonoBehaviour
{
	// Token: 0x06000A3A RID: 2618 RVA: 0x000372C6 File Offset: 0x000354C6
	private void OnEnable()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04000C8B RID: 3211
	public GameObject pivot;

	// Token: 0x04000C8C RID: 3212
	public Text outputInfo;

	// Token: 0x04000C8D RID: 3213
	public Component[] componentToInspect;

	// Token: 0x04000C8E RID: 3214
	public bool isEnabled;

	// Token: 0x04000C8F RID: 3215
	public bool autoFind = true;

	// Token: 0x04000C90 RID: 3216
	public GameObject canvas;

	// Token: 0x04000C91 RID: 3217
	public int sidewaysOffset;
}
