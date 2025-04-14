using System;
using UnityEngine;

// Token: 0x020001B4 RID: 436
public class GTDisableStaticOnAwake : MonoBehaviour
{
	// Token: 0x06000A4C RID: 2636 RVA: 0x00037E4C File Offset: 0x0003604C
	private void Awake()
	{
		base.gameObject.isStatic = false;
		Object.Destroy(this);
	}
}
