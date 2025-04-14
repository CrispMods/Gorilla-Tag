using System;
using UnityEngine;

// Token: 0x02000456 RID: 1110
public class DestroyIfNotBeta : MonoBehaviour
{
	// Token: 0x06001B52 RID: 6994 RVA: 0x00037597 File Offset: 0x00035797
	private void Awake()
	{
		Object.Destroy(base.gameObject);
	}
}
