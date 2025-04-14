using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200019E RID: 414
public class DevInspector : MonoBehaviour
{
	// Token: 0x060009EE RID: 2542 RVA: 0x00037273 File Offset: 0x00035473
	private void OnEnable()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000C45 RID: 3141
	public GameObject pivot;

	// Token: 0x04000C46 RID: 3142
	public Text outputInfo;

	// Token: 0x04000C47 RID: 3143
	public Component[] componentToInspect;

	// Token: 0x04000C48 RID: 3144
	public bool isEnabled;

	// Token: 0x04000C49 RID: 3145
	public bool autoFind = true;

	// Token: 0x04000C4A RID: 3146
	public GameObject canvas;

	// Token: 0x04000C4B RID: 3147
	public int sidewaysOffset;
}
