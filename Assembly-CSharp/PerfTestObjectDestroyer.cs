using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000232 RID: 562
[GTStripGameObjectFromBuild("!GT_AUTOMATED_PERF_TEST")]
public class PerfTestObjectDestroyer : MonoBehaviour
{
	// Token: 0x06000CEB RID: 3307 RVA: 0x000390ED File Offset: 0x000372ED
	private void Start()
	{
		UnityEngine.Object.DestroyImmediate(base.gameObject, true);
	}
}
