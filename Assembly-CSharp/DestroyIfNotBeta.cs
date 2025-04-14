using System;
using UnityEngine;

// Token: 0x02000456 RID: 1110
public class DestroyIfNotBeta : MonoBehaviour
{
	// Token: 0x06001B4F RID: 6991 RVA: 0x00037273 File Offset: 0x00035473
	private void Awake()
	{
		Object.Destroy(base.gameObject);
	}
}
