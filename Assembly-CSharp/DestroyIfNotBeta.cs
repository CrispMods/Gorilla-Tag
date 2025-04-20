using System;
using UnityEngine;

// Token: 0x02000462 RID: 1122
public class DestroyIfNotBeta : MonoBehaviour
{
	// Token: 0x06001BA3 RID: 7075 RVA: 0x000372C6 File Offset: 0x000354C6
	private void Awake()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
