using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200019E RID: 414
public class DevInspector : MonoBehaviour
{
	// Token: 0x060009F0 RID: 2544 RVA: 0x00036006 File Offset: 0x00034206
	private void OnEnable()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04000C46 RID: 3142
	public GameObject pivot;

	// Token: 0x04000C47 RID: 3143
	public Text outputInfo;

	// Token: 0x04000C48 RID: 3144
	public Component[] componentToInspect;

	// Token: 0x04000C49 RID: 3145
	public bool isEnabled;

	// Token: 0x04000C4A RID: 3146
	public bool autoFind = true;

	// Token: 0x04000C4B RID: 3147
	public GameObject canvas;

	// Token: 0x04000C4C RID: 3148
	public int sidewaysOffset;
}
