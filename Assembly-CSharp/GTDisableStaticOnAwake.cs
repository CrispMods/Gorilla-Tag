using System;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class GTDisableStaticOnAwake : MonoBehaviour
{
	// Token: 0x06000A98 RID: 2712 RVA: 0x000376C4 File Offset: 0x000358C4
	private void Awake()
	{
		base.gameObject.isStatic = false;
		UnityEngine.Object.Destroy(this);
	}
}
