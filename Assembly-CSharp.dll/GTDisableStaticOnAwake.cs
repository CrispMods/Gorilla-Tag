using System;
using UnityEngine;

// Token: 0x020001B4 RID: 436
public class GTDisableStaticOnAwake : MonoBehaviour
{
	// Token: 0x06000A4E RID: 2638 RVA: 0x00036404 File Offset: 0x00034604
	private void Awake()
	{
		base.gameObject.isStatic = false;
		UnityEngine.Object.Destroy(this);
	}
}
