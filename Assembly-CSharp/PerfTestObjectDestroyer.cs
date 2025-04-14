using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000227 RID: 551
[GTStripGameObjectFromBuild("!GT_AUTOMATED_PERF_TEST")]
public class PerfTestObjectDestroyer : MonoBehaviour
{
	// Token: 0x06000CA0 RID: 3232 RVA: 0x00042BE6 File Offset: 0x00040DE6
	private void Start()
	{
		Object.DestroyImmediate(base.gameObject, true);
	}
}
